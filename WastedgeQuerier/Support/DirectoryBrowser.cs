using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Util;
using IODirectory = System.IO.Directory;

namespace WastedgeQuerier.Support
{
    public partial class DirectoryBrowser : UserControl
    {
        private readonly SystemImageList _imageList = new SystemImageList();
        private readonly FileSystemWatcher _watcher = new FileSystemWatcher();
        private bool _dragging;
        private TreeNode _beforeDragSelection;
        private string _lastSelectedDirectory;
        private int _suspendUpdate;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Root
        {
            get
            {
                if (!String.IsNullOrEmpty(_watcher.Path))
                    return _watcher.Path;
                return null;
            }
            set
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Path = value;
                _treeView.Nodes[0].Tag = value;
                if (value != null)
                    _treeView.Nodes[0].ImageIndex = _treeView.Nodes[0].SelectedImageIndex = _imageList.AddShellIcon(value, 0);
                _watcher.EnableRaisingEvents = value != null;

                Reload();

                if (_treeView.Nodes.Count > 0)
                {
                    _treeView.SelectedNode = _treeView.Nodes[0];
                    OnDirectoryChanged(EventArgs.Empty);

                    _treeView.SelectedNode.Expand();
                }
            }
        }

        public string RootName
        {
            get { return _treeView.Nodes[0].Text; }
            set { _treeView.Nodes[0].Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileBrowserManager FileBrowserManager { get; set; }

        public string Directory => (string)_treeView.SelectedNode?.Tag;

        public bool CanAdd => Directory != null;

        public bool CanRename => IsNonRootNode(_treeView.SelectedNode);

        public bool CanDelete => IsNonRootNode(_treeView.SelectedNode);

        public event EventHandler DirectoryChanged;

        public event PathMouseEventHandler DirectoryClick;

        public DirectoryBrowser()
        {
            InitializeComponent();

            VisualStyleUtil.StyleTreeView(_treeView);

            _imageList.Assign(_treeView);

            _watcher.IncludeSubdirectories = true;
            _watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
            _watcher.SynchronizingObject = this;
            _watcher.Changed += _watcher_Changed;
            _watcher.Created += _watcher_Changed;
            _watcher.Deleted += _watcher_Changed;
            _watcher.Renamed += _watcher_Renamed;

            _treeView.Nodes.Add(new TreeNode("Root"));

            var dragDropManager = new DragDropManager(this);
            dragDropManager.DragEnter += _dragDropManager_DragEnter;
            dragDropManager.DragLeave += _dragDropManager_DragLeave;
            dragDropManager.DragOver += _dragDropManager_DragOver;
            dragDropManager.DragDrop += _dragDropManager_DragDrop;

            Reload();
        }

        private bool IsNonRootNode(TreeNode treeNode)
        {
            return treeNode != null && treeNode != _treeView.Nodes[0];
        }

        public void DoAdd()
        {
            if (!CanAdd)
                return;

            string basePath = Directory;

            for (int i = 0; ; i++)
            {
                string name = "New Folder";
                if (i > 0)
                    name += " (" + i + ")";

                var path = Path.Combine(basePath, name);

                if (!IODirectory.Exists(path))
                {
                    IODirectory.CreateDirectory(path);
                    var treeNode = EnsureNode(path);
                    Reload(treeNode);
                    _treeView.SelectedNode = treeNode;

                    _treeView.SelectedNode.BeginEdit();
                    return;
                }
            }
        }

        public void DoRename()
        {
            if (!CanRename)
                return;

            _treeView.SelectedNode.BeginEdit();
        }

        public void DoDelete()
        {
            if (!CanDelete)
                return;

            var result = TaskDialogEx.Show(this, "Are you sure you want to delete this folder and its entire contents?", FindForm().Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    IODirectory.Delete(Directory, true);
                    Reload();
                }
                catch
                {
                    TaskDialogEx.Show(this, "An error occured while deleting the directory", FindForm().Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_treeView.ContainsFocus)
            {
                switch (keyData)
                {
                    case Keys.F2:
                        DoRename();
                        return true;
                    case Keys.Delete:
                        DoDelete();
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void _dragDropManager_DragEnter(object sender, FilesDragEventArgs e)
        {
            if (_dragging)
                return;

            _dragging = true;
            _beforeDragSelection = _treeView.SelectedNode;
        }

        private void _dragDropManager_DragLeave(object sender, EventArgs e)
        {
            if (!_dragging)
                return;

            EndDragDrop();
        }

        private void EndDragDrop()
        {
            _treeView.SelectedNode = _beforeDragSelection;
            _dragging = false;
        }

        private void _dragDropManager_DragOver(object sender, FilesDragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (Root == null)
                return;

            Debug.Assert(_dragging);

            var node = _treeView.GetNodeAt(_treeView.PointToClient(new Point(e.X, e.Y)));
            _treeView.SelectedNode = node ?? _beforeDragSelection;
            if (node == null)
                return;

            var matches = BuildMatches((string)node.Tag, e.DropData);
            if (matches == null)
                return;

            var effectiveEffect = DragDropEffects.None;

            foreach (var match in matches)
            {
                DragDropEffects effect;

                if (match.Kind.IsMove())
                    effect = (e.KeyState & 8) != 0 ? DragDropEffects.Copy : DragDropEffects.Move;
                else if (match.Kind.IsCopy())
                    effect = (e.KeyState & 4) != 0 ? DragDropEffects.Move : DragDropEffects.Copy;
                else
                    effect = DragDropEffects.Copy;

                if (effect != DragDropEffects.None)
                {
                    if (effectiveEffect == DragDropEffects.None)
                        effectiveEffect = effect;
                    else
                        effectiveEffect = effectiveEffect & effect;
                }
            }

            e.Effect = effectiveEffect & e.AllowedEffect;
        }

        private List<DropMatch> BuildMatches(string targetPath, FilesDropData dropData)
        {
            return DropMatch.FromDropData(Root, targetPath, FileBrowserManager, dropData);
        }

        private void _dragDropManager_DragDrop(object sender, FilesDragEventArgs e)
        {
            EndDragDrop();

            var node = _treeView.GetNodeAt(_treeView.PointToClient(new Point(e.X, e.Y)));
            if (node == null)
                return;

            string targetPath = (string)node.Tag;

            var matches = BuildMatches(targetPath, e.DropData);
            if (matches == null)
                return;

            try
            {
                foreach (var match in matches)
                {
                    string target = Path.Combine(targetPath, Path.GetFileName(match.Path));

                    if (match.Kind.IsFile() && File.Exists(target))
                    {
                        var result = TaskDialogEx.Show(
                            this,
                            "The destination already has a file with this name. Do you want to overwrite the existing file?",
                            FindForm().Text,
                            TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel,
                            TaskDialogIcon.Warning
                            );

                        if (result == DialogResult.No)
                            continue;
                        if (result == DialogResult.Cancel)
                            return;

                        File.Delete(target);
                    }

                    switch (match.Kind)
                    {
                        case DropMatchKind.DirectoryMove:
                            IODirectory.Move(match.Path, target);
                            break;
                        case DropMatchKind.DirectoryCopy:
                            PathUtil.CopyDirectory(match.Path, target);
                            break;
                        case DropMatchKind.FileMove:
                            File.Move(match.Path, target);
                            break;
                        case DropMatchKind.FileCopy:
                            File.Copy(match.Path, target);
                            break;
                        case DropMatchKind.FileVirtual:
                            File.WriteAllBytes(target, e.DropData.GetFileData(match.Index));
                            break;
                    }
                }
            }
            catch
            {
                TaskDialogEx.Show(this, "Could not complete the operation", FindForm().Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
            }
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (IsNonRootNode(FindNode(e.OldFullPath)))
                FindNode(e.OldFullPath).Remove();

            _watcher_Changed(_watcher, e);
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Renamed:
                    Reload(FindNode(Path.GetDirectoryName(e.FullPath)) ?? _treeView.Nodes[0]);
                    break;
                case WatcherChangeTypes.Changed:
                    var treeNode = FindNode(e.FullPath);
                    if (treeNode != null)
                        Reload(treeNode);
                    break;
                case WatcherChangeTypes.Deleted:
                    FindNode(e.FullPath)?.Remove();
                    break;
            }
        }

        private TreeNode EnsureNode(string fullPath)
        {
            _treeView.BeginUpdate();

            var treeNode = EnsureNode(_treeView.Nodes[0], fullPath);

            _treeView.EndUpdate();

            return treeNode;
        }

        private TreeNode EnsureNode(TreeNode node, string fullPath)
        {
            // Does this node match?

            switch (PathUtil.ContainsPath((string)node.Tag, fullPath))
            {
                case PathContains.Not:
                    return null;
                case PathContains.Equals:
                    return node;
                case PathContains.Contains:
                    break;
            }

            // Do we already have a child node?

            foreach (TreeNode childNode in node.Nodes)
            {
                switch (PathUtil.ContainsPath((string)childNode.Tag, fullPath))
                {
                    case PathContains.Equals:
                        return node;
                    case PathContains.Contains:
                        return EnsureNode(childNode, fullPath);
                }
            }

            // Create the first level child node and retry.

            string subPath = PathUtil.RemoveSubPath((string)node.Tag, fullPath);
            int pos = subPath.IndexOf(Path.DirectorySeparatorChar);
            if (pos != -1)
                subPath = subPath.Substring(0, pos);

            var newNode = BuildNode(Path.Combine((string)node.Tag, subPath));
            InsertSorted(node.Nodes, newNode);

            return EnsureNode(newNode, fullPath);
        }

        private void InsertSorted(TreeNodeCollection nodes, TreeNode node)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (String.Compare(nodes[i].Text, node.Text, StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    nodes.Insert(i, node);
                    return;
                }
            }

            nodes.Add(node);
        }

        private TreeNode FindNode(string fullPath)
        {
            Debug.Assert(PathUtil.ContainsPath(Root, fullPath) != PathContains.Not);

            return FindNode(_treeView.Nodes[0].Nodes, fullPath);
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string path)
        {
            foreach (TreeNode node in nodes)
            {
                switch (PathUtil.ContainsPath((string)node.Tag, path))
                {
                    case PathContains.Equals:
                        return node;
                    case PathContains.Contains:
                        return FindNode(node.Nodes, path);
                }
            }

            return null;
        }

        private void Reload()
        {
            if (Root != null)
                Reload(_treeView.Nodes[0]);
        }

        private void Reload(TreeNode treeNode)
        {
            _treeView.BeginUpdate();

            ReloadNodes(treeNode.Nodes, (string)treeNode.Tag);

            _treeView.EndUpdate();
        }

        private void ReloadNodes(TreeNodeCollection nodes, string path)
        {
            SuspendUpdate();

            foreach (TreeNode node in nodes)
            {
                if (!IODirectory.Exists((string)node.Tag))
                    node.Remove();
            }

            foreach (string subPath in IODirectory.GetDirectories(path))
            {
                if (new FileInfo(subPath).Attributes.IsHidden())
                    continue;

                var node = nodes.Cast<TreeNode>().SingleOrDefault(p => PathUtil.ContainsPath((string)p.Tag, subPath) == PathContains.Equals);
                if (node == null)
                    InsertSorted(nodes, BuildNode(subPath));
            }

            ResumeUpdate();
        }

        private void SuspendUpdate()
        {
            if (_suspendUpdate == 0)
                _lastSelectedDirectory = Directory;
            _suspendUpdate++;
        }

        private void ResumeUpdate()
        {
            if (_suspendUpdate == 1 && _lastSelectedDirectory != null && Directory != _lastSelectedDirectory)
                _treeView.SelectedNode = FindNode(_lastSelectedDirectory) ?? _treeView.Nodes[0];

            _suspendUpdate--;

            if (Directory != _lastSelectedDirectory)
                OnDirectoryChanged(EventArgs.Empty);
        }

        private TreeNode BuildNode(string path)
        {
            var node = new TreeNode(Path.GetFileName(path));
            node.ImageIndex = node.SelectedImageIndex = _imageList.AddShellIcon(path, 0);
            node.Tag = path;

            ReloadNodes(node.Nodes, path);

            return node;
        }

        private void _treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (IsNonRootNode((TreeNode)e.Item))
            {
                var fileNames = new[] { (string)((TreeNode)e.Item).Tag };

                DoDragDrop(new DataObject(DataFormats.FileDrop, fileNames), DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        protected virtual void OnDirectoryChanged(EventArgs e)
        {
            DirectoryChanged?.Invoke(this, e);
        }

        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_dragging || _suspendUpdate > 0)
                return;

            OnDirectoryChanged(EventArgs.Empty);
        }

        private void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeView.SelectedNode = e.Node;

            OnDirectoryClick(new PathMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, (string)e.Node.Tag));
        }

        protected virtual void OnDirectoryClick(PathMouseEventArgs e)
        {
            DirectoryClick?.Invoke(this, e);
        }
    }
}
