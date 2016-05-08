using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Jint.Parser;
using JintDebugger;
using WastedgeApi;
using WeifenLuo.WinFormsUI.Docking;

namespace WastedgeQuerier.JavaScript
{
    internal partial class JavaScriptForm : JintDebugger.JavaScriptForm
    {
        private const string Filter = "Wastedge Querier Project (*.weproj)|*.weproj|All Files (*.*)|*.*";

        private readonly Api _api;
        private readonly ProjectControl _projectControl;
        private Project _project;
        private readonly RecentItemsManager _fileRecentItems = new RecentItemsManager("Files");
        private readonly RecentItemsManager _projectRecentItems = new RecentItemsManager("Projects");

        public Project Project
        {
            get { return _project; }
            private set
            {
                if (_project != value)
                {
                    _project = value;
                    _closeProject.Enabled = value != null;
                    _exportProject.Enabled = value != null;
                    OnProjectChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler ProjectChanged;

        public JavaScriptForm(Api api)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));

            _api = api;

            InitializeComponent();

            Icon = NeutralResources.mainicon;

            SetupMenu();

            _fileRecentItems.Opened += (s, e) => OpenEditor(e.Path);
            _projectRecentItems.Opened += (s, e) => OpenProject(Project.Open(e.Path));

            _fileRecentItems.Update(_recentFiles);
            _projectRecentItems.Update(_recentProjects);

            _projectControl = new ProjectControl(this);
            _projectControl.Show(DockPanel, DockState.DockLeft);
            _projectControl.SelectedProjectItemChanged += _projectControl_SelectedProjectItemChanged;
            _projectControl.ProjectItemActivated += _projectControl_ProjectItemActivated;
            _projectControl.ProjectItemContextMenu += _projectControl_ProjectItemContextMenu;

            DockPanel.ActiveDocumentChanged += DockPanel_ActiveDocumentChanged;
        }

        protected override void OnEditorOpened(EditorEventArgs e)
        {
            if (e.Editor.FileName != null)
            {
                _fileRecentItems.Add(e.Editor.FileName);
                _fileRecentItems.Update(_recentFiles);
            }

            base.OnEditorOpened(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_projectControl.ContainsFocus)
            {
                switch (keyData)
                {
                    case Keys.F2:
                        Rename();
                        return true;
                    case Keys.Delete:
                        Delete();
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void _projectControl_ProjectItemContextMenu(object sender, ProjectItemEventArgs e)
        {
            switch (e.ProjectItem.Type)
            {
                case ProjectItemType.Folder:
                    _folderContextMenu.Show(Cursor.Position);
                    break;
                case ProjectItemType.Root:
                    _projectContextMenu.Show(Cursor.Position);
                    break;
                case ProjectItemType.File:
                    _fileContextMenu.Show(Cursor.Position);
                    break;
            }
        }

        private void _projectControl_ProjectItemActivated(object sender, ProjectItemEventArgs e)
        {
            if (e.ProjectItem.Type == ProjectItemType.File && FindEditor(e.ProjectItem) == null)
                OpenEditor(e.ProjectItem.Path);
        }

        private void _projectControl_SelectedProjectItemChanged(object sender, EventArgs e)
        {
            var projectItem = _projectControl.SelectedProjectItem;

            if (projectItem != null && projectItem.Type == ProjectItemType.File)
                FindEditor(projectItem)?.Show();
        }

        private IEditor FindEditor(ProjectItem projectItem)
        {
            if (projectItem != null && projectItem.Type == ProjectItemType.File)
                return FindEditor(projectItem.Path);

            return null;
        }

        private void DockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (DockPanel.ActiveDocument != null)
                _projectControl.SelectFile(((IEditor)DockPanel.ActiveDocument).FileName);
        }

        protected override void OnEngineCreated(EngineCreatedEventArgs e)
        {
            base.OnEngineCreated(e);

            new JavaScriptUtil(_api.Credentials).Setup(e.Engine, this);
        }

        private void NewProject_Click(object sender, EventArgs e)
        {
            using (var form = new NewProjectForm())
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                if (!CloseProject())
                    return;

                var directory = Path.Combine(form.ProjectLocation, form.ProjectName);
                Directory.CreateDirectory(directory);
                OpenProject(Project.Create(Path.Combine(directory, form.ProjectName + ".weproj")));
            }
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            using (var form = new OpenFileDialog())
            {
                form.Title = "Open Project";
                form.Filter = Filter;

                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                if (CloseProject())
                    OpenProject(Project.Open(form.FileName));
            }
        }

        public void OpenProject(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            OpenProject(Project.Open(fileName));
        }

        private void OpenProject(Project project)
        {
            Project = project;

            _projectRecentItems.Add(Path.Combine(project.Path, project.FileName));
            _projectRecentItems.Update(_recentProjects);

            string mainFile = Path.Combine(project.Path, "main.js");
            if (File.Exists(mainFile))
                OpenEditor(mainFile);
        }

        private bool CloseProject()
        {
            if (Editors.Any(p => p.IsDirty))
            {
                var result = TaskDialogEx.Show(this, "Do you want to save your changes?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel, TaskDialogIcon.Warning);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (Editors.Any(p => !p.Save()))
                            return false;
                        break;

                    case DialogResult.Cancel:
                        return false;
                }
            }

            foreach (var editor in Editors.ToList())
            {
                editor.Close(true);
            }

            Project = null;

            return true;
        }

        private void SaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        private bool SaveAll()
        {
            bool success = true;

            foreach (var editor in Editors)
            {
                success = editor.Save() && success;
            }

            return success;
        }

        private void CloseProject_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private void ExportProject_Click(object sender, EventArgs e)
        {
            using (var form = new SaveFileDialog())
            {
                form.AddExtension = true;
                form.CheckPathExists = true;
                form.Filter = "Wastedge Querier Plugin (*.wqpkg)|*.wqpkg|All Files (*.*)|*.*";
                form.OverwritePrompt = true;
                form.RestoreDirectory = true;
                form.FileName = Path.GetFileNameWithoutExtension(_project.FileName) + ".wqpkg";

                if (form.ShowDialog(this) == DialogResult.OK)
                    ExportProject(form.FileName);
            }
        }

        private void ExportProject(string fileName)
        {
            if (!SaveAll())
                return;

            // The package format is just a blind ZIP file of the entire contents of the project directory.

            new FastZip().CreateZip(fileName, _project.Path, true, null);
        }

        protected virtual void OnProjectChanged(EventArgs e)
        {
            ProjectChanged?.Invoke(this, e);
        }

        private void _projectAddMenuItem_Click(object sender, EventArgs e)
        {
            AddFile();
        }

        private void _projectAddFolderMenuItem_Click(object sender, EventArgs e)
        {
            AddFolder();
        }

        private void _projectPropertiesMenuItem_Click(object sender, EventArgs e)
        {
            EditProperties();
        }

        private void _fileOpenMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void _fileDeleteMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void _fileRenameMenuItem_Click(object sender, EventArgs e)
        {
            Rename();
        }

        private void _folderAddMenuItem_Click(object sender, EventArgs e)
        {
            AddFile();
        }

        private void _folderAddFolderMenuItem_Click(object sender, EventArgs e)
        {
            AddFolder();
        }

        private void _folderDeleteMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void _folderRenameMenuItem_Click(object sender, EventArgs e)
        {
            Rename();
        }

        private void OpenFile()
        {
            OpenEditor(_projectControl.SelectedProjectItem.Path);
        }

        private void EditProperties()
        {
            using (var form = new ProjectPropertiesForm())
            {
                if (String.IsNullOrEmpty(_project.Title))
                    form.Title = Path.GetFileNameWithoutExtension(_project.FileName);
                else
                    form.Title = _project.Title;
                form.Description = _project.Description;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (form.Title == Path.GetFileNameWithoutExtension(_project.FileName))
                        _project.Title = null;
                    else
                        _project.Title = form.Title;
                    if (String.IsNullOrEmpty(form.Description))
                        _project.Description = null;
                    else
                        _project.Description = form.Description;

                    _project.Save();
                }
            }
        }

        private void AddFile()
        {
            using (var form = new RenameForm())
            {
                form.Text = "New File";

                for (int i = 1; ; i++)
                {
                    form.Path = Path.Combine(GetProjectItemDirectory(_projectControl.SelectedProjectItem), "JavaScript" + i + ".js");
                    if (!File.Exists(form.Path))
                        break;
                }

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllText(form.Path, "");

                    BeginInvoke(new Action<string>(p => OpenEditor(p)), form.Path);
                }
            }
        }

        private void AddFolder()
        {
            var basePath = GetProjectItemDirectory(_projectControl.SelectedProjectItem);

            for (int i = 1; ; i++)
            {
                string path = Path.Combine(basePath, "NewFolder" + i);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Rename(path);
                    return;
                }
            }
        }

        private static string GetProjectItemDirectory(ProjectItem projectItem)
        {
            var basePath = projectItem.Path;
            if (projectItem.Type == ProjectItemType.Root)
                basePath = Path.GetDirectoryName(basePath);
            return basePath;
        }

        private void Delete()
        {
            if (_projectControl.SelectedProjectItem.Type == ProjectItemType.Root)
                return;

            if (TaskDialogEx.Show(this, "Are you sure you want to delete this item?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning) != DialogResult.Yes)
                return;

            string path = _projectControl.SelectedProjectItem.Path;

            FindEditor(path)?.Close();

            if (File.Exists(path))
                File.Delete(path);
            else if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        private void Rename()
        {
            if (_projectControl.SelectedProjectItem.Type == ProjectItemType.Root)
                return;

            var projectItemPath = _projectControl.SelectedProjectItem.Path;

            Rename(projectItemPath);
        }

        private void Rename(string path)
        {
            using (var form = new RenameForm())
            {
                var editor = FindEditor(path);

                form.Path = path;

                if (form.ShowDialog(this) == DialogResult.OK && !path.Equals(form.Path, StringComparison.OrdinalIgnoreCase))
                {
                    editor?.Save();

                    string newPath = form.Path;
                    if (Directory.Exists(path))
                        Directory.Move(path, newPath);
                    else if (File.Exists(path))
                        File.Move(path, newPath);

                    if (editor != null)
                    {
                        editor.Open(newPath);
                        BeginInvoke(new Action(() => _projectControl.SelectFile(newPath)));
                    }
                }
            }
        }
    }
}
