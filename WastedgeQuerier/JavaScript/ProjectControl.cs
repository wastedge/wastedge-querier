using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WastedgeQuerier.JavaScript
{
    internal partial class ProjectControl : DockContent
    {
        private readonly JavaScriptForm _owner;
        private TreeViewManager _manager;

        public ProjectItem SelectedProjectItem => _treeView.SelectedNode?.GetProjectItem();

        public event EventHandler SelectedProjectItemChanged;

        public event ProjectItemEventHandler ProjectItemActivated;

        public event ProjectItemEventHandler ProjectItemContextMenu;

        public ProjectControl(JavaScriptForm owner)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            _owner = owner;
            _owner.ProjectChanged += _owner_ProjectChanged;

            Font = SystemFonts.MessageBoxFont;

            InitializeComponent();

            VisualStyleUtil.StyleTreeView(_treeView);
        }

        private void _owner_ProjectChanged(object sender, EventArgs e)
        {
            _manager?.Dispose();
            _manager = null;
            _treeView.Nodes.Clear();

            if (_owner.Project != null)
                _manager = new TreeViewManager(_treeView, _owner.Project);
        }

        public void SelectFile(string fileName)
        {
            _manager?.SelectFile(fileName);
        }

        protected virtual void OnSelectedProjectItemChanged(EventArgs e)
        {
            SelectedProjectItemChanged?.Invoke(this, e);
        }

        protected virtual void OnProjectItemActivated(ProjectItemEventArgs e)
        {
            ProjectItemActivated?.Invoke(this, e);
        }

        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnSelectedProjectItemChanged(EventArgs.Empty);
        }

        private void _treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeView.SelectedNode = e.Node;
            OnProjectItemActivated(new ProjectItemEventArgs(e.Node.GetProjectItem()));
        }

        private void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _treeView.SelectedNode = e.Node;
                OnProjectItemContextMenu(new ProjectItemEventArgs(e.Node.GetProjectItem()));
            }
        }

        protected virtual void OnProjectItemContextMenu(ProjectItemEventArgs e)
        {
            ProjectItemContextMenu?.Invoke(this, e);
        }
    }
}
