using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal class TreeViewManager : IDisposable
    {
        private readonly TreeView _treeView;
        private readonly string _path;
        private FileSystemWatcher _watcher;
        private bool _disposed;
        private readonly TreeNode _rootNode;
        private bool _loaded;

        public TreeViewManager(TreeView treeView, Project project)
        {
            if (treeView == null)
                throw new ArgumentNullException(nameof(treeView));
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            _treeView = treeView;
            _path = project.Path.TrimEnd(Path.DirectorySeparatorChar);

            _watcher = new FileSystemWatcher(_path);
            _watcher.Created += _watcher_Event;
            _watcher.Deleted += _watcher_Event;
            _watcher.Renamed += _watcher_Renamed;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;

            _rootNode = new TreeNode(project.GetTitle(), (int)Image.Project, (int)Image.Project);
            _rootNode.Tag = new ProjectRoot(_rootNode, Path.Combine(project.Path, project.FileName));

            treeView.Nodes.Add(_rootNode);

            treeView.BeginUpdate();
            RefreshPath(_path);
            treeView.EndUpdate();

            _loaded = true;

            _rootNode.Expand();
        }

        private void _watcher_Event(object sender, FileSystemEventArgs e)
        {
            _treeView.BeginInvoke(
                new Action<string>(p =>
                {
                    _treeView.BeginUpdate();
                    RefreshPath(p);
                    _treeView.EndUpdate();
                }),
                e.FullPath
            );
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            _treeView.BeginInvoke(
                new Action<string, string>((p1, p2) =>
                {
                    _treeView.BeginUpdate();
                    RefreshPath(p1);
                    RefreshPath(p2);
                    _treeView.EndUpdate();
                }),
                e.OldFullPath,
                e.FullPath
            );
        }

        private void RefreshPath(string path)
        {
            Debug.Assert(path.StartsWith(_path, StringComparison.OrdinalIgnoreCase));

            if (!path.StartsWith(_path, StringComparison.OrdinalIgnoreCase))
                return;

            path = path.Substring(_path.Length).TrimStart(Path.DirectorySeparatorChar);

            RefreshPath(_rootNode.Nodes, _path, path);
        }

        private void RefreshPath(TreeNodeCollection nodes, string basePath, string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                int index = path.IndexOf(Path.DirectorySeparatorChar);
                string directory;

                if (index == -1)
                {
                    directory = path;
                    path = null;
                }
                else
                {
                    directory = path.Substring(0, index);
                    path = path.Substring(index + 1);
                }

                var node = FindNode(nodes, directory);

                if (Directory.Exists(Path.Combine(basePath, directory)))
                {
                    if (node == null)
                    {
                        node = CreateFolderNode(Path.Combine(basePath, directory));
                        InsertNode(nodes, node);
                    }

                    RefreshPath(node.Nodes, Path.Combine(basePath, directory), path);
                }
                else if (File.Exists(Path.Combine(basePath, directory)))
                {
                    if (node == null && IncludeFile(directory))
                    {
                        node = CreateFileNode(Path.Combine(basePath, directory));
                        InsertNode(nodes, node);
                    }
                }
                else
                {
                    node?.Remove();
                }
            }
            else
            {
                foreach (var node in nodes.Cast<TreeNode>())
                {
                    if (node.GetProjectItem().Type == ProjectItemType.Folder)
                    {
                        if (!Directory.Exists(Path.Combine(basePath, node.Text)))
                            node.Remove();
                        else
                            RefreshPath(node.Nodes, Path.Combine(basePath, node.Text), null);
                    }
                    else
                    {
                        if (!File.Exists(Path.Combine(basePath, node.Text)))
                            node.Remove();
                    }
                }

                foreach (string directory in Directory.GetDirectories(basePath))
                {
                    var node = FindNode(nodes, Path.GetFileName(directory));
                    if (node == null)
                    {
                        node = CreateFolderNode(directory);
                        InsertNode(nodes, node);
                    }

                    RefreshPath(node.Nodes, directory, null);
                }

                foreach (string fileName in Directory.GetFiles(basePath))
                {
                    if (IncludeFile(fileName) && FindNode(nodes, Path.GetFileName(fileName)) == null)
                    {
                        var node = CreateFileNode(fileName);
                        InsertNode(nodes, node);
                    }
                }
            }
        }

        private bool IncludeFile(string fileName)
        {
            return (Path.GetExtension(fileName)?.Equals(".js", StringComparison.OrdinalIgnoreCase)).GetValueOrDefault();
        }

        private static TreeNode CreateFileNode(string fileName)
        {
            var node = new TreeNode(Path.GetFileName(fileName), (int)Image.JavaScript, (int)Image.JavaScript);
            node.Tag = new ProjectFile(node, fileName);
            return node;
        }

        private static TreeNode CreateFolderNode(string directory)
        {
            var node = new TreeNode(Path.GetFileName(directory), (int)Image.Directory, (int)Image.Directory);
            node.Tag = new ProjectFolder(node, directory);
            return node;
        }

        private void InsertNode(TreeNodeCollection nodes, TreeNode node)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (CompareNodes(nodes[i], node) > 0)
                {
                    nodes.Insert(i, node);
                    if (_loaded)
                        node.EnsureVisible();
                    return;
                }
            }

            nodes.Add(node);
            if (_loaded)
                node.EnsureVisible();
        }

        private int CompareNodes(TreeNode lhs, TreeNode rhs)
        {
            var lhsItem = lhs.GetProjectItem();
            var rhsItem = rhs.GetProjectItem();

            int result = lhsItem.Type.CompareTo(rhsItem.Type);
            if (result != 0)
                return result;

            return String.Compare(
                Path.GetFileNameWithoutExtension(lhsItem.Name),
                Path.GetFileNameWithoutExtension(rhsItem.Name),
                StringComparison.OrdinalIgnoreCase
            );
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string name)
        {
            return nodes.Cast<TreeNode>().SingleOrDefault(p => p.Text.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectFile(string fileName)
        {
            if (fileName == null || !fileName.StartsWith(_path, StringComparison.OrdinalIgnoreCase))
                return;

            fileName = fileName.Substring(_path.Length).TrimStart(Path.DirectorySeparatorChar);

            RefreshPath(_rootNode.Nodes, _path, fileName);
            SelectFile(_rootNode.Nodes, fileName);
        }

        private void SelectFile(TreeNodeCollection nodes, string fileName)
        {
            int index = fileName.IndexOf(Path.DirectorySeparatorChar);

            string directory;
            if (index == -1)
            {
                directory = fileName;
                fileName = null;
            }
            else
            {
                directory = fileName.Substring(0, index);
                fileName = fileName.Substring(index + 1);
            }

            var node = FindNode(nodes, directory);
            if (node == null)
                return;

            if (fileName != null)
                SelectFile(node.Nodes, fileName);
            else
                _treeView.SelectedNode = node;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_watcher != null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }

                _disposed = true;
            }
        }

        private enum Image
        {
            Directory,
            Project,
            JavaScript
        }
    }
}
