namespace WastedgeQuerier
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this._fileMenuItem = new System.Windows.Forms.MenuItem();
            this._fileAddExportMenuItem = new System.Windows.Forms.MenuItem();
            this._fileAddReportMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this._fileAddPluginMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this._fileExitMenuItem = new System.Windows.Forms.MenuItem();
            this._toolsMenuItem = new System.Windows.Forms.MenuItem();
            this._toolsOpenTableMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this._toolsJavaScriptConsoleMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this._helpOpenHelpMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this._helpAboutMenuItem = new System.Windows.Forms.MenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this._addFolder = new System.Windows.Forms.ToolStripButton();
            this._deleteFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._renameFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._addFile = new System.Windows.Forms.ToolStripDropDownButton();
            this._addExport = new System.Windows.Forms.ToolStripMenuItem();
            this._addReport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this._addPlugin = new System.Windows.Forms.ToolStripMenuItem();
            this._deleteFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._renameFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._runFile = new System.Windows.Forms.ToolStripButton();
            this._directoryContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._addFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._deleteFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._renameFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._fileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this._addExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this._addPluginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._deleteFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._renameFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._directoryBrowser = new WastedgeQuerier.Support.DirectoryBrowser();
            this._fileBrowser = new WastedgeQuerier.Support.FileBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this._directoryContextMenu.SuspendLayout();
            this._fileContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._fileMenuItem,
            this._toolsMenuItem,
            this.menuItem1});
            // 
            // _fileMenuItem
            // 
            this._fileMenuItem.Index = 0;
            this._fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._fileAddExportMenuItem,
            this._fileAddReportMenuItem,
            this.menuItem3,
            this._fileAddPluginMenuItem,
            this.menuItem6,
            this._fileExitMenuItem});
            this._fileMenuItem.Text = "&File";
            // 
            // _fileAddExportMenuItem
            // 
            this._fileAddExportMenuItem.Index = 0;
            this._fileAddExportMenuItem.Text = "New &Export";
            this._fileAddExportMenuItem.Click += new System.EventHandler(this._fileAddExportMenuItem_Click);
            // 
            // _fileAddReportMenuItem
            // 
            this._fileAddReportMenuItem.Index = 1;
            this._fileAddReportMenuItem.Text = "New &Report";
            this._fileAddReportMenuItem.Click += new System.EventHandler(this._fileAddReportMenuItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // _fileAddPluginMenuItem
            // 
            this._fileAddPluginMenuItem.Index = 3;
            this._fileAddPluginMenuItem.Text = "Add &Plugin";
            this._fileAddPluginMenuItem.Click += new System.EventHandler(this._fileAddPluginMenuItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "-";
            // 
            // _fileExitMenuItem
            // 
            this._fileExitMenuItem.Index = 5;
            this._fileExitMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this._fileExitMenuItem.Text = "E&xit";
            this._fileExitMenuItem.Click += new System.EventHandler(this._fileExitMenuItem_Click);
            // 
            // _toolsMenuItem
            // 
            this._toolsMenuItem.Index = 1;
            this._toolsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._toolsOpenTableMenuItem,
            this.menuItem5,
            this._toolsJavaScriptConsoleMenuItem});
            this._toolsMenuItem.Text = "&Tools";
            // 
            // _toolsOpenTableMenuItem
            // 
            this._toolsOpenTableMenuItem.Index = 0;
            this._toolsOpenTableMenuItem.Text = "&Open Table";
            this._toolsOpenTableMenuItem.Click += new System.EventHandler(this._toolsOpenTableMenuItem_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            // 
            // _toolsJavaScriptConsoleMenuItem
            // 
            this._toolsJavaScriptConsoleMenuItem.Index = 2;
            this._toolsJavaScriptConsoleMenuItem.Shortcut = System.Windows.Forms.Shortcut.F11;
            this._toolsJavaScriptConsoleMenuItem.Text = "&JavaScript Console";
            this._toolsJavaScriptConsoleMenuItem.Click += new System.EventHandler(this._toolsJavaScriptConsoleMenuItem_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._helpOpenHelpMenuItem,
            this.menuItem2,
            this._helpAboutMenuItem});
            this.menuItem1.Text = "&Help";
            // 
            // _helpOpenHelpMenuItem
            // 
            this._helpOpenHelpMenuItem.Index = 0;
            this._helpOpenHelpMenuItem.Shortcut = System.Windows.Forms.Shortcut.F1;
            this._helpOpenHelpMenuItem.Text = "&Open Help";
            this._helpOpenHelpMenuItem.Click += new System.EventHandler(this._helpOpenHelpMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "-";
            // 
            // _helpAboutMenuItem
            // 
            this._helpAboutMenuItem.Index = 2;
            this._helpAboutMenuItem.Text = "A&bout";
            this._helpAboutMenuItem.Click += new System.EventHandler(this._helpAboutMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._directoryBrowser);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._fileBrowser);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(721, 164);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip3
            // 
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addFolder,
            this._deleteFolder,
            this.toolStripSeparator2,
            this._renameFolder});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(239, 25);
            this.toolStrip3.TabIndex = 2;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // _addFolder
            // 
            this._addFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._addFolder.Image = global::WastedgeQuerier.NeutralResources.add;
            this._addFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._addFolder.Name = "_addFolder";
            this._addFolder.Size = new System.Drawing.Size(23, 22);
            this._addFolder.Text = "Add Folder";
            this._addFolder.Click += new System.EventHandler(this._addFolder_Click);
            // 
            // _deleteFolder
            // 
            this._deleteFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._deleteFolder.Image = global::WastedgeQuerier.NeutralResources.delete;
            this._deleteFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._deleteFolder.Name = "_deleteFolder";
            this._deleteFolder.Size = new System.Drawing.Size(23, 22);
            this._deleteFolder.Text = "Delete Folder";
            this._deleteFolder.Click += new System.EventHandler(this._deleteFolder_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // _renameFolder
            // 
            this._renameFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._renameFolder.Image = global::WastedgeQuerier.NeutralResources.text_field;
            this._renameFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._renameFolder.Name = "_renameFolder";
            this._renameFolder.Size = new System.Drawing.Size(23, 22);
            this._renameFolder.Text = "Rename";
            this._renameFolder.Click += new System.EventHandler(this._renameFolder_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addFile,
            this._deleteFile,
            this.toolStripSeparator1,
            this._renameFile,
            this.toolStripSeparator3,
            this._runFile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(478, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _addFile
            // 
            this._addFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._addFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addExport,
            this._addReport,
            this.toolStripMenuItem3,
            this._addPlugin});
            this._addFile.Image = global::WastedgeQuerier.NeutralResources.add;
            this._addFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._addFile.Name = "_addFile";
            this._addFile.Size = new System.Drawing.Size(29, 22);
            this._addFile.Text = "Add File";
            // 
            // _addExport
            // 
            this._addExport.Name = "_addExport";
            this._addExport.Size = new System.Drawing.Size(136, 22);
            this._addExport.Text = "New &Export";
            this._addExport.Click += new System.EventHandler(this._addExport_Click);
            // 
            // _addReport
            // 
            this._addReport.Name = "_addReport";
            this._addReport.Size = new System.Drawing.Size(136, 22);
            this._addReport.Text = "New &Report";
            this._addReport.Click += new System.EventHandler(this._addReport_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(133, 6);
            // 
            // _addPlugin
            // 
            this._addPlugin.Name = "_addPlugin";
            this._addPlugin.Size = new System.Drawing.Size(136, 22);
            this._addPlugin.Text = "Add &Plugin";
            this._addPlugin.Click += new System.EventHandler(this._addPlugin_Click);
            // 
            // _deleteFile
            // 
            this._deleteFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._deleteFile.Image = global::WastedgeQuerier.NeutralResources.delete;
            this._deleteFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._deleteFile.Name = "_deleteFile";
            this._deleteFile.Size = new System.Drawing.Size(23, 22);
            this._deleteFile.Text = "Delete File";
            this._deleteFile.Click += new System.EventHandler(this._deleteFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _renameFile
            // 
            this._renameFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._renameFile.Image = global::WastedgeQuerier.NeutralResources.text_field;
            this._renameFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._renameFile.Name = "_renameFile";
            this._renameFile.Size = new System.Drawing.Size(23, 22);
            this._renameFile.Text = "Rename";
            this._renameFile.Click += new System.EventHandler(this._renameFile_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // _runFile
            // 
            this._runFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._runFile.Image = global::WastedgeQuerier.NeutralResources._continue;
            this._runFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._runFile.Name = "_runFile";
            this._runFile.Size = new System.Drawing.Size(23, 22);
            this._runFile.Text = "Run File";
            this._runFile.Click += new System.EventHandler(this._runFile_Click);
            // 
            // _directoryContextMenu
            // 
            this._directoryContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addFolderMenuItem,
            this.toolStripMenuItem1,
            this._deleteFolderMenuItem,
            this._renameFolderMenuItem});
            this._directoryContextMenu.Name = "_directoryContextMenu";
            this._directoryContextMenu.Size = new System.Drawing.Size(137, 76);
            // 
            // _addFolderMenuItem
            // 
            this._addFolderMenuItem.Name = "_addFolderMenuItem";
            this._addFolderMenuItem.Size = new System.Drawing.Size(136, 22);
            this._addFolderMenuItem.Text = "&New Folder";
            this._addFolderMenuItem.Click += new System.EventHandler(this._addFolderMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(133, 6);
            // 
            // _deleteFolderMenuItem
            // 
            this._deleteFolderMenuItem.Name = "_deleteFolderMenuItem";
            this._deleteFolderMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this._deleteFolderMenuItem.Size = new System.Drawing.Size(136, 22);
            this._deleteFolderMenuItem.Text = "&Delete";
            this._deleteFolderMenuItem.Click += new System.EventHandler(this._deleteFolderMenuItem_Click);
            // 
            // _renameFolderMenuItem
            // 
            this._renameFolderMenuItem.Name = "_renameFolderMenuItem";
            this._renameFolderMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this._renameFolderMenuItem.Size = new System.Drawing.Size(136, 22);
            this._renameFolderMenuItem.Text = "&Rename";
            this._renameFolderMenuItem.Click += new System.EventHandler(this._renameFolderMenuItem_Click);
            // 
            // _fileContextMenu
            // 
            this._fileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripSeparator4,
            this._deleteFileMenuItem,
            this._renameFileMenuItem});
            this._fileContextMenu.Name = "_directoryContextMenu";
            this._fileContextMenu.Size = new System.Drawing.Size(137, 76);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addExportMenuItem,
            this._addReportMenuItem,
            this.toolStripMenuItem4,
            this._addPluginMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem2.Text = "&Add";
            // 
            // _addExportMenuItem
            // 
            this._addExportMenuItem.Name = "_addExportMenuItem";
            this._addExportMenuItem.Size = new System.Drawing.Size(136, 22);
            this._addExportMenuItem.Text = "New &Export";
            this._addExportMenuItem.Click += new System.EventHandler(this._addExportMenuItem_Click);
            // 
            // _addReportMenuItem
            // 
            this._addReportMenuItem.Name = "_addReportMenuItem";
            this._addReportMenuItem.Size = new System.Drawing.Size(136, 22);
            this._addReportMenuItem.Text = "New &Report";
            this._addReportMenuItem.Click += new System.EventHandler(this._addReportMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(133, 6);
            // 
            // _addPluginMenuItem
            // 
            this._addPluginMenuItem.Name = "_addPluginMenuItem";
            this._addPluginMenuItem.Size = new System.Drawing.Size(136, 22);
            this._addPluginMenuItem.Text = "Add &Plugin";
            this._addPluginMenuItem.Click += new System.EventHandler(this._addPluginMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(133, 6);
            // 
            // _deleteFileMenuItem
            // 
            this._deleteFileMenuItem.Name = "_deleteFileMenuItem";
            this._deleteFileMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this._deleteFileMenuItem.Size = new System.Drawing.Size(136, 22);
            this._deleteFileMenuItem.Text = "&Delete";
            this._deleteFileMenuItem.Click += new System.EventHandler(this._deleteFileMenuItem_Click);
            // 
            // _renameFileMenuItem
            // 
            this._renameFileMenuItem.Name = "_renameFileMenuItem";
            this._renameFileMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this._renameFileMenuItem.Size = new System.Drawing.Size(136, 22);
            this._renameFileMenuItem.Text = "&Rename";
            this._renameFileMenuItem.Click += new System.EventHandler(this._renameFileMenuItem_Click);
            // 
            // _directoryBrowser
            // 
            this._directoryBrowser.AllowDrop = true;
            this._directoryBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._directoryBrowser.Location = new System.Drawing.Point(0, 25);
            this._directoryBrowser.Name = "_directoryBrowser";
            this._directoryBrowser.RootName = "Wastedge";
            this._directoryBrowser.Size = new System.Drawing.Size(239, 139);
            this._directoryBrowser.TabIndex = 1;
            this._directoryBrowser.DirectoryChanged += new System.EventHandler(this._directoryBrowser_DirectoryChanged);
            this._directoryBrowser.DirectoryClick += new WastedgeQuerier.Support.PathMouseEventHandler(this._directoryBrowser_DirectoryClick);
            // 
            // _fileBrowser
            // 
            this._fileBrowser.AllowDrop = true;
            this._fileBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fileBrowser.Enabled = false;
            this._fileBrowser.Location = new System.Drawing.Point(0, 25);
            this._fileBrowser.Name = "_fileBrowser";
            this._fileBrowser.Size = new System.Drawing.Size(478, 139);
            this._fileBrowser.TabIndex = 1;
            this._fileBrowser.SelectedFilesChanged += new System.EventHandler(this._fileBrowser_SelectedFilesChanged);
            this._fileBrowser.FileClick += new WastedgeQuerier.Support.PathMouseEventHandler(this._fileBrowser_FileClick);
            this._fileBrowser.FileActivate += new WastedgeQuerier.Support.PathEventHandler(this._fileBrowser_FileActivate);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 164);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Wastedge Querier";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this._directoryContextMenu.ResumeLayout(false);
            this._fileContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _fileMenuItem;
        private System.Windows.Forms.MenuItem _fileExitMenuItem;
        private System.Windows.Forms.MenuItem _toolsMenuItem;
        private System.Windows.Forms.MenuItem _toolsJavaScriptConsoleMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem _helpOpenHelpMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem _helpAboutMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Support.DirectoryBrowser _directoryBrowser;
        private Support.FileBrowser _fileBrowser;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton _addFolder;
        private System.Windows.Forms.ToolStripButton _deleteFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _renameFolder;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _deleteFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _runFile;
        private System.Windows.Forms.ContextMenuStrip _directoryContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _addFolderMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _deleteFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _renameFolderMenuItem;
        private System.Windows.Forms.ToolStripButton _renameFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ContextMenuStrip _fileContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _deleteFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _renameFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _addPluginMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _addExportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _addReportMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton _addFile;
        private System.Windows.Forms.ToolStripMenuItem _addPlugin;
        private System.Windows.Forms.ToolStripMenuItem _addExport;
        private System.Windows.Forms.ToolStripMenuItem _addReport;
        private System.Windows.Forms.MenuItem _fileAddPluginMenuItem;
        private System.Windows.Forms.MenuItem _fileAddExportMenuItem;
        private System.Windows.Forms.MenuItem _fileAddReportMenuItem;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.MenuItem _toolsOpenTableMenuItem;
        private System.Windows.Forms.MenuItem menuItem5;
    }
}