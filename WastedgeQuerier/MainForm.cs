using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.EditInExcel;
using WastedgeQuerier.Export;
using WastedgeQuerier.Formats;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Plugins;
using WastedgeQuerier.Report;
using WastedgeQuerier.Support;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;
        private JavaScriptForm _javaScriptForm;
        private readonly CopyDataTarget _copyDataTarget = new CopyDataTarget();

        public MainForm(ApiCredentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            _api = new Api(credentials);

            InitializeComponent();

            _directoryBrowser.FileBrowserManager = _fileBrowser.FileBrowserManager = new FileBrowserManager();
            var filesPath = Path.Combine(Program.DataPath, "Files");
            Directory.CreateDirectory(filesPath);
            _directoryBrowser.Root = filesPath;

            UpdateEnabled();

            Enabled = false;

            Text = $"{Text} - {credentials.Company}\\{credentials.UserName} - {credentials.Url}";

            var handle = _copyDataTarget.Handle; // Force creation of the handle.
            _copyDataTarget.DataCopied += _copyDataTarget_DataCopied;
        }

        private void _copyDataTarget_DataCopied(object sender, CopyDataEventArgs e)
        {
            NativeMethods.SetForegroundWindow(Handle);

            ParseArguments(e.Data);
        }

        private void UpdateEnabled()
        {
            _addFolder.Enabled = _directoryBrowser.CanAdd;
            _deleteFolder.Enabled = _directoryBrowser.CanDelete;
            _renameFolder.Enabled = _directoryBrowser.CanRename;

            bool haveSingleSelection = _fileBrowser.SelectedFiles.Length == 1;

            _addFile.Enabled = _fileBrowser.Directory != null;
            _deleteFile.Enabled = _fileBrowser.CanDelete;
            _renameFile.Enabled = _fileBrowser.CanRename;
            _runFile.Enabled = haveSingleSelection;

            _addFile.Enabled = _fileBrowser.Directory != null;
            _fileAddPluginMenuItem.Enabled = _fileBrowser.Directory != null;
            _fileAddExportMenuItem.Enabled = _fileBrowser.Directory != null;
            _fileAddReportMenuItem.Enabled = _fileBrowser.Directory != null;
        }

        private void _directoryBrowser_DirectoryChanged(object sender, EventArgs e)
        {
            _fileBrowser.Directory = _directoryBrowser.Directory;

            UpdateEnabled();
        }

        private void _addFolder_Click(object sender, EventArgs e)
        {
            _directoryBrowser.DoAdd();
        }

        private void _deleteFolder_Click(object sender, EventArgs e)
        {
            _directoryBrowser.DoDelete();
        }

        private void _renameFolder_Click(object sender, EventArgs e)
        {
            _directoryBrowser.DoRename();
        }

        private void _directoryBrowser_DirectoryClick(object sender, PathMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            _addFolderMenuItem.Enabled = _directoryBrowser.CanAdd;
            _deleteFolderMenuItem.Enabled = _directoryBrowser.CanDelete;
            _renameFolderMenuItem.Enabled = _directoryBrowser.CanRename;

            _directoryContextMenu.Show(_directoryBrowser, e.Location);
        }

        private void _addFolderMenuItem_Click(object sender, EventArgs e)
        {
            _directoryBrowser.DoAdd();
        }

        private void _deleteFolderMenuItem_Click(object sender, EventArgs e)
        {
            _directoryBrowser.DoDelete();
        }

        private void _renameFolderMenuItem_Click(object sender, EventArgs e)
        {
            _directoryBrowser.DoRename();
        }

        private void _fileBrowser_SelectedFilesChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _deleteFile_Click(object sender, EventArgs e)
        {
            DoDeleteFile();
        }

        private void DoDeleteFile()
        {
            _fileBrowser.DoDelete();
        }

        private void _renameFile_Click(object sender, EventArgs e)
        {
            _fileBrowser.DoRename();
        }

        private void _fileBrowser_FileClick(object sender, PathMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            _deleteFileMenuItem.Enabled = _fileBrowser.CanDelete;
            _renameFileMenuItem.Enabled = _fileBrowser.CanRename;

            _fileContextMenu.Show(_fileBrowser, e.Location);
        }

        private void _deleteFileMenuItem_Click(object sender, EventArgs e)
        {
            _fileBrowser.DoDelete();
        }

        private void _renameFileMenuItem_Click(object sender, EventArgs e)
        {
            _fileBrowser.DoRename();
        }

        private void _fileExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _toolsJavaScriptConsoleMenuItem_Click(object sender, EventArgs e)
        {

            EnsureJavaScriptForm();
        }

        private void EnsureJavaScriptForm()
        {
            if (_javaScriptForm != null)
            {
                _javaScriptForm.BringToFront();
                return;
            }

            _javaScriptForm = new JavaScriptForm(_api);
            _javaScriptForm.Disposed += (s, ea) => _javaScriptForm = null;

            _javaScriptForm.Show();
        }

        private void _helpOpenHelpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/wastedge/wastedge-querier/wiki");
            }
            catch
            {
                // Ignore exceptions.
            }
        }

        private void _helpAboutMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/wastedge/wastedge-querier");
            }
            catch
            {
                // Ignore exceptions.
            }
        }

        private void _fileBrowser_FileActivate(object sender, PathEventArgs e)
        {
            DoOpenFile(e.Path);
        }

        private void _runFile_Click(object sender, EventArgs e)
        {
            var selectedFiles = _fileBrowser.SelectedFiles;
            if (selectedFiles.Length != 1)
                return;

            DoOpenFile(selectedFiles[0]);
        }

        private void DoOpenFile(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".wqpkg":
                    DoRunPlugin(path);
                    break;

                case ".wqexport":
                    DoRunExport(path);
                    break;

                case ".wqreport":
                    DoRunReport(path);
                    break;
            }
        }

        private void DoRunPlugin(string path)
        {
            var plugin = Plugin.Load(path);

            plugin.Run(_api.Credentials, this);
        }

        private void DoRunExport(string path)
        {
            ExportDefinition export;

            try
            {
                export = ExportDefinition.Load(_api, path);
            }
            catch
            {
                TaskDialogEx.Show(this, "Unable to load report", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
                return;
            }

            using (var form = new ExportDefinitionForm(_api, Path.GetDirectoryName(path), Path.GetFileName(path), export))
            {
                form.ShowDialog(this);
            }
        }

        private void DoRunReport(string path)
        {
            ReportDefinition report;

            try
            {
                report = ReportDefinition.Load(_api, path);
            }
            catch
            {
                TaskDialogEx.Show(this, "Unable to load report", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
                return;
            }

            using (var form = new ReportForm(_api, Path.GetDirectoryName(path), Path.GetFileName(path), report))
            {
                form.ShowDialog(this);
            }
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            await _api.CacheFullEntitySchemaAsync();

            Enabled = true;

            ParseArguments(Environment.CommandLine);
        }

        private void ParseArguments(string commandLine)
        {
            var args = CommandLineUtil.Parse(commandLine);

            foreach (string arg in args.Skip(1))
            {
                if (File.Exists(arg) && Path.HasExtension(arg))
                {
                    switch (Path.GetExtension(arg).ToLowerInvariant())
                    {
                        case ".js":
                            EnsureJavaScriptForm();
                            _javaScriptForm.OpenEditor(arg);
                            break;

                        case ".weproj":
                            EnsureJavaScriptForm();
                            _javaScriptForm.OpenProject(arg);
                            break;

                        case ".wqpkg":
                        case ".wqreport":
                        case ".wqexport":
                            AddFile(arg);
                            return;
                    }
                }
            }
        }

        private void _addPlugin_Click(object sender, EventArgs e)
        {
            DoAddPlugin();
        }

        private void _addPluginMenuItem_Click(object sender, EventArgs e)
        {
            DoAddPlugin();
        }

        private void _fileAddPluginMenuItem_Click(object sender, EventArgs e)
        {
            DoAddPlugin();
        }

        private void DoAddPlugin()
        {
            using (var form = new OpenFileDialog())
            {
                form.Title = "Open Plugin";
                form.Filter = "Wastedge Querier Plugin (*.wqpkg)|*.wqpkg|All Files (*.*)|*.*";

                if (form.ShowDialog(this) == DialogResult.OK)
                    AddFile(form.FileName);
            }
        }

        private void AddFile(string fileName)
        {
            string target = Path.Combine(_fileBrowser.Directory, Path.GetFileName(fileName));

            if (File.Exists(target))
            {
                var result = TaskDialogEx.Show(this, "A file with the same name already exists. Do you want to overwrite the existing file?", Text, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.Warning);
                if (result == DialogResult.No)
                    return;

                File.Delete(target);
            }

            File.Copy(fileName, target);
        }

        private void _fileAddReportMenuItem_Click(object sender, EventArgs e)
        {
            DoAddReport();
        }

        private void _addReport_Click(object sender, EventArgs e)
        {
            DoAddReport();
        }

        private void _addReportMenuItem_Click(object sender, EventArgs e)
        {
            DoAddReport();
        }

        private void DoAddReport()
        {
            EntitySchema entity;

            using (var form = new SelectEntityForm(_api))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                entity = form.SelectedEntity;
            }

            var report = new ReportDefinition
            {
                Entity = entity
            };

            using (var form = new ReportForm(_api, _fileBrowser.Directory, null, report))
            {
                form.ShowDialog(this);
            }
        }

        private void DoAddExport()
        {
            EntitySchema entity;

            using (var form = new SelectEntityForm(_api))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                entity = form.SelectedEntity;
            }

            var export = new ExportDefinition
            {
                Entity = entity
            };

            using (var form = new ExportDefinitionForm(_api, _fileBrowser.Directory, null, export))
            {
                form.ShowDialog(this);
            }
        }

        private void _toolsOpenTableMenuItem_Click(object sender, EventArgs e)
        {
            EntitySchema entity;

            using (var form = new SelectEntityForm(_api))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                entity = form.SelectedEntity;
            }

            List<Filter> filters;

            using (var form = new EntityFiltersForm(entity, new Filter[0]))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                filters = form.GetFilters();
            }

            using (var form = new ResultForm(_api, entity, filters))
            {
                form.ShowDialog(this);
            }
        }

        private void _fileAddExportMenuItem_Click(object sender, EventArgs e)
        {
            DoAddExport();
        }

        private void _addExport_Click(object sender, EventArgs e)
        {
            DoAddExport();
        }

        private void _addExportMenuItem_Click(object sender, EventArgs e)
        {
            DoAddExport();
        }

        private class FileBrowserManager : Support.FileBrowserManager
        {
            public FileBrowserManager()
            {
                Filters.Add(".wqpkg");
                Filters.Add(".wqexport");
                Filters.Add(".wqreport");

                Columns.Add(new FileBrowserColumn("Type", 120, HorizontalAlignment.Left));
                Columns.Add(new FileBrowserColumn("Description", 200, HorizontalAlignment.Left));
            }

            public override string[] GetValues(string path)
            {
                string type;
                string description = null;

                switch (Path.GetExtension(path).ToLowerInvariant())
                {
                    case ".wqpkg":
                        type = "Plugin";
                        try
                        {
                            description = Plugin.Load(path).Project.Description;
                        }
                        catch
                        {
                            // Ignore.
                        }
                        break;

                    case ".wqexport":
                        type = "Export";
                        break;

                    case ".wqreport":
                        type = "Report";
                        break;

                    default:
                        type = "";
                        break;
                }

                return new[] { type, description };
            }
        }
    }
}
