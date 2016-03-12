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
            this._fileExitMenuItem = new System.Windows.Forms.MenuItem();
            this._toolsMenuItem = new System.Windows.Forms.MenuItem();
            this._toolsJavaScriptConsoleMenuItem = new System.Windows.Forms.MenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._tables = new System.Windows.Forms.ComboBox();
            this._footerPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._export = new System.Windows.Forms.Button();
            this._reset = new System.Windows.Forms.Button();
            this._filters = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this._filterContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._filter = new System.Windows.Forms.ComboBox();
            this._add = new System.Windows.Forms.Button();
            this._filterControls = new System.Windows.Forms.Panel();
            this._container = new System.Windows.Forms.Panel();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this._footerPanel.SuspendLayout();
            this._filters.SuspendLayout();
            this.panel3.SuspendLayout();
            this._filterContainer.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._fileMenuItem,
            this._toolsMenuItem});
            // 
            // _fileMenuItem
            // 
            this._fileMenuItem.Index = 0;
            this._fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._fileExitMenuItem});
            this._fileMenuItem.Text = "&File";
            // 
            // _fileExitMenuItem
            // 
            this._fileExitMenuItem.Index = 0;
            this._fileExitMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this._fileExitMenuItem.Text = "E&xit";
            this._fileExitMenuItem.Click += new System.EventHandler(this._fileExitMenuItem_Click);
            // 
            // _toolsMenuItem
            // 
            this._toolsMenuItem.Index = 1;
            this._toolsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._toolsJavaScriptConsoleMenuItem});
            this._toolsMenuItem.Text = "&Tools";
            // 
            // _toolsJavaScriptConsoleMenuItem
            // 
            this._toolsJavaScriptConsoleMenuItem.Index = 0;
            this._toolsJavaScriptConsoleMenuItem.Text = "&JavaScript Console";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._tables, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._footerPanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._filters, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(498, 342);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Table:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tables
            // 
            this._tables.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tables.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._tables.FormattingEnabled = true;
            this._tables.Location = new System.Drawing.Point(46, 3);
            this._tables.Name = "_tables";
            this._tables.Size = new System.Drawing.Size(449, 21);
            this._tables.TabIndex = 1;
            this._tables.SelectedIndexChanged += new System.EventHandler(this._tables_SelectedIndexChanged);
            // 
            // _footerPanel
            // 
            this._footerPanel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._footerPanel, 2);
            this._footerPanel.Controls.Add(this._export);
            this._footerPanel.Controls.Add(this._reset);
            this._footerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._footerPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this._footerPanel.Location = new System.Drawing.Point(3, 310);
            this._footerPanel.Name = "_footerPanel";
            this._footerPanel.Size = new System.Drawing.Size(492, 29);
            this._footerPanel.TabIndex = 3;
            // 
            // _export
            // 
            this._export.Location = new System.Drawing.Point(414, 3);
            this._export.Name = "_export";
            this._export.Size = new System.Drawing.Size(75, 23);
            this._export.TabIndex = 1;
            this._export.Text = "&Export";
            this._export.UseVisualStyleBackColor = true;
            this._export.Click += new System.EventHandler(this._export_Click);
            // 
            // _reset
            // 
            this._reset.Location = new System.Drawing.Point(333, 3);
            this._reset.Name = "_reset";
            this._reset.Size = new System.Drawing.Size(75, 23);
            this._reset.TabIndex = 0;
            this._reset.Text = "&Reset";
            this._reset.UseVisualStyleBackColor = true;
            this._reset.Click += new System.EventHandler(this._reset_Click);
            // 
            // _filters
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._filters, 2);
            this._filters.Controls.Add(this.panel3);
            this._filters.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filters.Location = new System.Drawing.Point(3, 30);
            this._filters.Name = "_filters";
            this._filters.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this._filters.Size = new System.Drawing.Size(492, 274);
            this._filters.TabIndex = 2;
            this._filters.TabStop = false;
            this._filters.Text = "Filters";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this._filterContainer);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(8, 17);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(476, 249);
            this.panel3.TabIndex = 2;
            // 
            // _filterContainer
            // 
            this._filterContainer.AutoScroll = true;
            this._filterContainer.BackColor = System.Drawing.SystemColors.Control;
            this._filterContainer.Controls.Add(this.tableLayoutPanel2);
            this._filterContainer.Controls.Add(this._filterControls);
            this._filterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filterContainer.Location = new System.Drawing.Point(1, 1);
            this._filterContainer.Name = "_filterContainer";
            this._filterContainer.Size = new System.Drawing.Size(474, 247);
            this._filterContainer.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this._filter, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._add, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(474, 29);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // _filter
            // 
            this._filter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this._filter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filter.FormattingEnabled = true;
            this._filter.Location = new System.Drawing.Point(3, 3);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(418, 21);
            this._filter.TabIndex = 0;
            this._filter.SizeChanged += new System.EventHandler(this._filter_SizeChanged);
            this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
            // 
            // _add
            // 
            this._add.Location = new System.Drawing.Point(427, 3);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(44, 23);
            this._add.TabIndex = 1;
            this._add.Text = "Add";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _filterControls
            // 
            this._filterControls.AutoSize = true;
            this._filterControls.Dock = System.Windows.Forms.DockStyle.Top;
            this._filterControls.Location = new System.Drawing.Point(0, 0);
            this._filterControls.Name = "_filterControls";
            this._filterControls.Size = new System.Drawing.Size(474, 0);
            this._filterControls.TabIndex = 0;
            // 
            // _container
            // 
            this._container.Controls.Add(this.tableLayoutPanel1);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            this._container.Padding = new System.Windows.Forms.Padding(9);
            this._container.Size = new System.Drawing.Size(516, 360);
            this._container.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 360);
            this.Controls.Add(this._container);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wastedge Querier";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._footerPanel.ResumeLayout(false);
            this._filters.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this._filterContainer.ResumeLayout(false);
            this._filterContainer.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _fileMenuItem;
        private System.Windows.Forms.MenuItem _fileExitMenuItem;
        private System.Windows.Forms.MenuItem _toolsMenuItem;
        private System.Windows.Forms.MenuItem _toolsJavaScriptConsoleMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _tables;
        private System.Windows.Forms.Panel _container;
        private System.Windows.Forms.FlowLayoutPanel _footerPanel;
        private System.Windows.Forms.Button _export;
        private System.Windows.Forms.GroupBox _filters;
        private System.Windows.Forms.Panel _filterContainer;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Button _reset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox _filter;
        private System.Windows.Forms.Button _add;
        private System.Windows.Forms.Panel _filterControls;
    }
}

