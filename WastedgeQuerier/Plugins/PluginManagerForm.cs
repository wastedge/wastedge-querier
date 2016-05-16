using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.JavaScript;

namespace WastedgeQuerier.Plugins
{
    public partial class PluginManagerForm : SystemEx.Windows.Forms.Form
    {
        private readonly ApiCredentials _credentials;
        private readonly string _path;
        private string _pendingPackage;

        private PluginFolder SelectedFolder => (PluginFolder)_folders.SelectedNode?.Tag;

        private Plugin SelectedPlugin
        {
            get
            {
                if (_plugins.SelectedItems.Count == 0)
                    return null;
                return (Plugin)_plugins.SelectedItems[0].Tag;
            }
        }

        public PluginManagerForm(ApiCredentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            _credentials = credentials;

            InitializeComponent();

            VisualStyleUtil.StyleTreeView(_folders);
            VisualStyleUtil.StyleListView(_plugins);

            _path = Path.Combine(Program.DataPath, "Plugins");

            Reload(null, null);
        }

        private void Reload(string selectedFolder, string selectedPlugin)
        {
            _folders.BeginUpdate();

            if (selectedFolder == null)
                selectedFolder = SelectedFolder?.Path;

            var plugins = PluginFolder.FromFolder(_path);

            _folders.Nodes.Clear();

            LoadFolders(_folders.Nodes, plugins, selectedFolder);

            _folders.ExpandAll();

            if (SelectedFolder == null && _folders.Nodes.Count > 0)
                _folders.SelectedNode = _folders.Nodes[0];

            _folders.EndUpdate();

            ReloadPlugins(selectedPlugin);
        }

        private void ReloadPlugins(string selectedPlugin)
        {
            _plugins.BeginUpdate();

            if (selectedPlugin == null)
                selectedPlugin = SelectedPlugin?.Path;

            _plugins.Items.Clear();

            if (SelectedFolder != null)
                LoadPlugins(SelectedFolder, selectedPlugin);

            if (_plugins.Items.Count > 0 && _plugins.SelectedItems.Count == 0)
                _plugins.Items[0].Selected = true;

            _plugins.EndUpdate();

            UpdateEnabled();
        }

        private void LoadFolders(TreeNodeCollection nodes, PluginFolder folder, string selectedFolder)
        {
            var node = new TreeNode
            {
                Text = folder.Name,
                Tag = folder
            };

            nodes.Add(node);

            foreach (var child in folder.Children.OfType<PluginFolder>())
            {
                LoadFolders(node.Nodes, child, selectedFolder);
            }

            if (String.Equals(folder.Path, selectedFolder, StringComparison.OrdinalIgnoreCase))
                _folders.SelectedNode = node;
        }

        private void LoadPlugins(PluginFolder folder, string selectedPlugin)
        {
            foreach (var child in folder.Children.OfType<Plugin>())
            {
                var item = new ListViewItem
                {
                    Text = child.Name,
                    Tag = child
                };

                _plugins.Items.Add(item);

                if (String.Equals(child.Path, selectedPlugin, StringComparison.OrdinalIgnoreCase))
                    item.Selected = true;
            }
        }

        private void _folders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateEnabled();

            ReloadPlugins(null);
        }

        private void UpdateEnabled()
        {
            _addFolder.Enabled = _folders.SelectedNode != null;
            _deleteFolder.Enabled = _folders.SelectedNode?.Parent != null;
            _renameFolder.Enabled = _folders.SelectedNode?.Parent != null;
            _addPlugin.Enabled = _folders.SelectedNode != null;

            _deletePlugin.Enabled = SelectedPlugin != null;
            _runPlugin.Enabled = SelectedPlugin != null;
        }

        private void _plugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _addPlugin_Click(object sender, EventArgs e)
        {
            using (var form = new OpenFileDialog())
            {
                form.Title = "Open Plugin";
                form.Filter = "Wastedge Querier Plugin (*.wqpkg)|*.wqpkg|All Files (*.*)|*.*";

                if (form.ShowDialog(this) == DialogResult.OK)
                    AddPlugin(form.FileName);
            }
        }

        private bool AddPlugin(string package)
        {
            string target = Path.Combine(SelectedFolder.Path, Path.GetFileName(package));

            if (File.Exists(target))
            {
                var result = TaskDialogEx.Show(this, "This plugin has already been loaded. Do you want to overwrite the existing plugin?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
                if (result != DialogResult.Yes)
                    return false;
            }

            File.Copy(package, target, true);

            Reload(null, null);

            return true;
        }

        private void _deletePlugin_Click(object sender, EventArgs e)
        {
            var result = TaskDialogEx.Show(this, "Are you sure you want to delete this plugin?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
            if (result == DialogResult.Yes)
            {
                File.Delete(SelectedPlugin.Path);
                Reload(null, null);
            }
        }

        private void _folders_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
                return;

            string source = ((PluginFolder)e.Node.Tag).Path;
            string target = Path.Combine(Path.GetDirectoryName(source), e.Label);

            try
            {
                Directory.Move(source, target);
            }
            catch
            {
                e.CancelEdit = true;
                return;
            }

            BeginInvoke(new Action(() => Reload(target, null)));
        }

        private void _folders_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (_folders.SelectedNode?.Parent == null)
                e.CancelEdit = true;
        }

        private void _addFolder_Click(object sender, EventArgs e)
        {
            string basePath = SelectedFolder.Path;

            for (int i = 0;; i++)
            {
                string name = "New Folder";
                if (i > 0)
                    name += " (" + i + ")";

                var path = Path.Combine(basePath, name);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Reload(path, null);

                    _folders.SelectedNode.BeginEdit();
                    return;
                }
            }
        }

        private void _deleteFolder_Click(object sender, EventArgs e)
        {
            var result = TaskDialogEx.Show(this, "Are you sure you want to delete this folder and its entire contents?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Directory.Delete(SelectedFolder.Path, true);
                Reload(null, null);
            }
        }

        private void _plugins_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop((Plugin)((ListViewItem)e.Item).Tag, DragDropEffects.Move);
        }

        private void _folders_DragOver(object sender, DragEventArgs e)
        {
            var node = _folders.GetNodeAt(_folders.PointToClient(new Point(e.X, e.Y)));
            if (node != null)
            {
                var target = (PluginFolder)node.Tag;

                var plugin = (Plugin)e.Data.GetData(typeof(Plugin));
                if (plugin != null)
                {
                    if (!String.Equals(target.Path, Path.GetDirectoryName(plugin.Path), StringComparison.OrdinalIgnoreCase))
                    {
                        e.Effect = e.AllowedEffect;
                        return;
                    }
                }
                else
                {
                    var folder = (PluginFolder)e.Data.GetData(typeof(PluginFolder));

                    if (!DirectoryEqualsOrContains(target.Path, folder.Path))
                    {
                        string renamed = Path.Combine(target.Path, Path.GetFileName(folder.Path));
                        if (!String.Equals(folder.Path, renamed, StringComparison.OrdinalIgnoreCase) && !Directory.Exists(renamed))
                        {
                            e.Effect = e.AllowedEffect;
                            return;
                        }
                    }
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private bool DirectoryEqualsOrContains(string target, string source)
        {
            if (String.IsNullOrEmpty(target) || String.IsNullOrEmpty(source))
                return false;

            if (String.Equals(source, target, StringComparison.OrdinalIgnoreCase))
                return true;

            return DirectoryEqualsOrContains(Path.GetDirectoryName(target), source);
        }

        private void _folders_DragDrop(object sender, DragEventArgs e)
        {
            var node = _folders.GetNodeAt(_folders.PointToClient(new Point(e.X, e.Y)));
            if (node == null)
                return;

            var plugin = (Plugin)e.Data.GetData(typeof(Plugin));
            if (plugin != null)
            {
                string source = plugin.Path;
                string target = Path.Combine(((PluginFolder)node.Tag).Path, Path.GetFileName(source));

                if (File.Exists(target))
                {
                    var result = TaskDialogEx.Show(this, "This folder already contains a plugin with the same name. Do you want to overwrite the existing plugin?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
                    if (result != DialogResult.Yes)
                        return;

                    File.Delete(target);
                }

                File.Move(source, target);

                Reload(Path.GetDirectoryName(target), target);
            }
            else
            {
                var folder = (PluginFolder)e.Data.GetData(typeof(PluginFolder));
                if (folder != null)
                {
                    string target = Path.Combine(((PluginFolder)node.Tag).Path, Path.GetFileName(folder.Path));
                    Directory.Move(folder.Path, target);

                    Reload(target, null);
                }
            }
        }

        private void _folders_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop((PluginFolder)((TreeNode)e.Item).Tag, DragDropEffects.Move);
        }

        private void _runPlugin_Click(object sender, EventArgs e)
        {
            SelectedPlugin.Run(_credentials, this);
        }

        public void LoadPackage(string package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            _pendingPackage = package;
        }

        private void PluginManagerForm_Shown(object sender, EventArgs e)
        {
            if (_pendingPackage == null)
                return;

            if (AddPlugin(_pendingPackage))
                TaskDialogEx.Show(this, "The new plugin has been added. You can start the plugin by clicking the run button or through the Plugins menu.", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Information);
        }

        private void _renameFolder_Click(object sender, EventArgs e)
        {
            _folders.SelectedNode.BeginEdit();
        }
    }
}
