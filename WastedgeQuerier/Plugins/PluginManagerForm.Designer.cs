namespace WastedgeQuerier.Plugins
{
    partial class PluginManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginManagerForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._folders = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._addFolder = new System.Windows.Forms.ToolStripButton();
            this._deleteFolder = new System.Windows.Forms.ToolStripButton();
            this._plugins = new SystemEx.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this._addPlugin = new System.Windows.Forms.ToolStripButton();
            this._deletePlugin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._runPlugin = new System.Windows.Forms.ToolStripButton();
            this.formFlowFooter1 = new SystemEx.Windows.Forms.FormFlowFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.formFlowFooter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(707, 346);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(9, 9);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._folders);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._plugins);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(689, 328);
            this.splitContainer1.SplitterDistance = 228;
            this.splitContainer1.TabIndex = 0;
            // 
            // _folders
            // 
            this._folders.AllowDrop = true;
            this._folders.Dock = System.Windows.Forms.DockStyle.Fill;
            this._folders.HideSelection = false;
            this._folders.ImageIndex = 0;
            this._folders.ImageList = this.imageList1;
            this._folders.LabelEdit = true;
            this._folders.Location = new System.Drawing.Point(0, 25);
            this._folders.Name = "_folders";
            this._folders.SelectedImageIndex = 0;
            this._folders.Size = new System.Drawing.Size(228, 303);
            this._folders.TabIndex = 1;
            this._folders.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this._folders_BeforeLabelEdit);
            this._folders.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this._folders_AfterLabelEdit);
            this._folders.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._folders_ItemDrag);
            this._folders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._folders_AfterSelect);
            this._folders.DragDrop += new System.Windows.Forms.DragEventHandler(this._folders_DragDrop);
            this._folders.DragOver += new System.Windows.Forms.DragEventHandler(this._folders_DragOver);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addFolder,
            this._deleteFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(228, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // _plugins
            // 
            this._plugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this._plugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this._plugins.HideSelection = false;
            this._plugins.Location = new System.Drawing.Point(0, 25);
            this._plugins.MultiSelect = false;
            this._plugins.Name = "_plugins";
            this._plugins.Size = new System.Drawing.Size(457, 303);
            this._plugins.TabIndex = 1;
            this._plugins.UseCompatibleStateImageBehavior = false;
            this._plugins.View = System.Windows.Forms.View.Details;
            this._plugins.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._plugins_ItemDrag);
            this._plugins.SelectedIndexChanged += new System.EventHandler(this._plugins_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Title";
            this.columnHeader1.Width = 419;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addPlugin,
            this._deletePlugin,
            this.toolStripSeparator1,
            this._runPlugin});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(457, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // _addPlugin
            // 
            this._addPlugin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._addPlugin.Image = global::WastedgeQuerier.NeutralResources.add;
            this._addPlugin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._addPlugin.Name = "_addPlugin";
            this._addPlugin.Size = new System.Drawing.Size(23, 22);
            this._addPlugin.Text = "Add Plugin";
            this._addPlugin.Click += new System.EventHandler(this._addPlugin_Click);
            // 
            // _deletePlugin
            // 
            this._deletePlugin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._deletePlugin.Image = global::WastedgeQuerier.NeutralResources.delete;
            this._deletePlugin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._deletePlugin.Name = "_deletePlugin";
            this._deletePlugin.Size = new System.Drawing.Size(23, 22);
            this._deletePlugin.Text = "Delete Plugin";
            this._deletePlugin.Click += new System.EventHandler(this._deletePlugin_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _runPlugin
            // 
            this._runPlugin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._runPlugin.Image = global::WastedgeQuerier.NeutralResources._continue;
            this._runPlugin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._runPlugin.Name = "_runPlugin";
            this._runPlugin.Size = new System.Drawing.Size(23, 22);
            this._runPlugin.Text = "Run Plugin";
            this._runPlugin.Click += new System.EventHandler(this._runPlugin_Click);
            // 
            // formFlowFooter1
            // 
            this.formFlowFooter1.Controls.Add(this._cancelButton);
            this.formFlowFooter1.Location = new System.Drawing.Point(0, 346);
            this.formFlowFooter1.Name = "formFlowFooter1";
            this.formFlowFooter1.Size = new System.Drawing.Size(707, 45);
            this.formFlowFooter1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(611, 11);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 0;
            this._cancelButton.Text = "Close";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // PluginManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(707, 391);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFlowFooter1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginManagerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Plugin Manager";
            this.Shown += new System.EventHandler(this.PluginManagerForm_Shown);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.formFlowFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SystemEx.Windows.Forms.FormFlowFooter formFlowFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView _folders;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private SystemEx.Windows.Forms.ListView _plugins;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton _addFolder;
        private System.Windows.Forms.ToolStripButton _deleteFolder;
        private System.Windows.Forms.ToolStripButton _addPlugin;
        private System.Windows.Forms.ToolStripButton _deletePlugin;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _runPlugin;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}