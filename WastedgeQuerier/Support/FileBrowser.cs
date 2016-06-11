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
    public partial class FileBrowser : UserControl
    {
        private readonly SystemImageList _imageList = new SystemImageList();
        private readonly FileSystemWatcher _watcher = new FileSystemWatcher();
        private string _directory;
        private bool _reloadPending;
        private FileBrowserManager _fileBrowserManager;
        private int _updating;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Directory
        {
            get { return _directory; }
            set
            {
                if (_directory == value)
                    return;

                _watcher.EnableRaisingEvents = false;
                _watcher.Path = value;

                _directory = value;
                Reload();
                _watcher.EnableRaisingEvents = true;

                Enabled = _directory != null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileBrowserManager FileBrowserManager
        {
            get { return _fileBrowserManager; }
            set
            {
                _fileBrowserManager = value;
                RebuildColumns();
            }
        }

        public string SelectedFile => _listView.SelectedItems.Count > 0 ? (string)_listView.SelectedItems[0].Tag : null;

        public string[] SelectedFiles => _listView.SelectedItems.Cast<ListViewItem>().Select(p => (string)p.Tag).ToArray();

        public event EventHandler SelectedFilesChanged;

        public bool CanRename => _listView.SelectedItems.Count == 1;

        public bool CanDelete => _listView.SelectedItems.Count > 0;

        public event PathMouseEventHandler FileClick;

        public event PathEventHandler FileActivate;

        private void RebuildColumns()
        {
            while (_listView.Columns.Count > 1)
            {
                _listView.Columns.RemoveAt(_listView.Columns.Count - 1);
            }

            if (_fileBrowserManager != null)
            {
                foreach (var column in _fileBrowserManager.Columns)
                {
                    _listView.Columns.Add(new ColumnHeader
                    {
                        Text = column.ColumnHeader,
                        Width = column.ColumnWidth,
                        TextAlign = column.Alignment
                    });
                }
            }

            Reload();
        }

        public FileBrowser()
        {
            InitializeComponent();

            Enabled = false;

            VisualStyleUtil.StyleListView(_listView);

            _imageList.Assign(_listView);

            _watcher.IncludeSubdirectories = false;
            _watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
            _watcher.SynchronizingObject = this;
            _watcher.Changed += _watcher_Changed;
            _watcher.Created += _watcher_Changed;
            _watcher.Deleted += _watcher_Changed;
            _watcher.Renamed += _watcher_Changed;

            var dragDropManager = new DragDropManager(this);
            dragDropManager.DragEnter += _dragDropManager_DragEnter;
            dragDropManager.DragDrop += _dragDropManager_DragDrop;

            Reload();
        }

        private void _dragDropManager_DragEnter(object sender, FilesDragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (Directory == null)
                return;

            var matches = BuildMatches(e.DropData);
            if (matches == null)
                return;

            var effectiveEffect = DragDropEffects.None;

            foreach (var match in matches)
            {
                DragDropEffects effect;

                if (match.Kind.IsDirectory())
                    effect = DragDropEffects.None;
                else if (match.Kind.IsMove())
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

        private List<DropMatch> BuildMatches(FilesDropData dropData)
        {
            return DropMatch.FromDropData(Directory, Directory, FileBrowserManager, dropData);
        }

        private void _dragDropManager_DragDrop(object sender, FilesDragEventArgs e)
        {
            var matches = BuildMatches(e.DropData);
            if (matches == null)
                return;

            try
            {
                foreach (var match in matches)
                {
                    if (match.Kind.IsDirectory())
                        continue;

                    string target = Path.Combine(Directory, Path.GetFileName(match.Path));

                    if (File.Exists(target))
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

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            if (!IsHandleCreated)
                return;

            if (!_reloadPending)
            {
                _reloadPending = true;
                BeginInvoke(new Action(DoReload));
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            Reload();
        }

        private void DoReload()
        {
            _reloadPending = false;

            if (Directory == null)
                return;

            _listView.BeginUpdate();

            _updating++;

            string[] selectedFiles = SelectedFiles;

            _listView.Items.Clear();

            if (IODirectory.Exists(Directory))
            {
                foreach (string path in IODirectory.GetFiles(Directory))
                {
                    if (new FileInfo(path).Attributes.IsHidden())
                        continue;
                    if (FileBrowserManager == null || !FileBrowserManager.Matches(path))
                        continue;

                    var item = new ListViewItem(Path.GetFileNameWithoutExtension(path));
                    item.ImageIndex = _imageList.AddShellIcon(path, ShellIconType.UseFileAttributes);
                    item.Tag = path;

                    var values = FileBrowserManager.GetValues(path);

                    for (int i = 0; i < values.Length; i++)
                    {
                        item.SubItems.Add(values[i]);
                    }

                    _listView.Items.Add(item);
                }
            }

            foreach (string selectedFile in selectedFiles)
            {
                var item = _listView.Items.Cast<ListViewItem>().SingleOrDefault(p => (string)p.Tag == selectedFile);
                if (item != null)
                    item.Selected = true;
            }

            _updating--;

            _listView.EndUpdate();

            OnSelectedFilesChanged(EventArgs.Empty);
        }

        private void _listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var fileNames = _listView.SelectedItems.Cast<ListViewItem>().Select(p => (string)p.Tag).ToArray();

            DoDragDrop(new DataObject(DataFormats.FileDrop, fileNames), DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void _listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (_updating > 0)
                return;

            OnSelectedFilesChanged(EventArgs.Empty);
        }

        protected virtual void OnSelectedFilesChanged(EventArgs e)
        {
            SelectedFilesChanged?.Invoke(this, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_listView.ContainsFocus)
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

        public void DoDelete()
        {
            if (!CanDelete)
                return;

            string message = _listView.SelectedItems.Count == 1 ? "Are you sure you want to delete this file?" : $"Are you sure you want to delete {_listView.SelectedItems.Count} files?";

            var result = TaskDialogEx.Show(this, message, FindForm().Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            foreach (string fileName in SelectedFiles)
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                    // Ignore.
                }
            }
        }

        public void DoRename()
        {
            if (!CanRename)
                return;

            _listView.SelectedItems[0].BeginEdit();
        }

        private void _listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var listViewItem = _listView.Items[e.Item];

            if (String.IsNullOrEmpty(e.Label))
            {
                listViewItem.Text = Path.GetFileNameWithoutExtension((string)listViewItem.Tag);
                return;
            }

            string target = Path.Combine(Directory, e.Label + Path.GetExtension((string)listViewItem.Tag));

            try
            {
                File.Move((string)listViewItem.Tag, target);
                Reload();
            }
            catch
            {
                // Ignore.
            }
        }

        private void _listView_MouseClick(object sender, MouseEventArgs e)
        {
            if (_listView.SelectedItems.Count != 1)
                return;

            OnFileClick(new PathMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, SelectedFile));
        }

        protected virtual void OnFileClick(PathMouseEventArgs e)
        {
            FileClick?.Invoke(this, e);
        }

        private void _listView_ItemActivate(object sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count != 1)
                return;

            OnFileActivate(new PathEventArgs((string)_listView.SelectedItems[0].Tag));
        }

        protected virtual void OnFileActivate(PathEventArgs e)
        {
            FileActivate?.Invoke(this, e);
        }
    }
}
