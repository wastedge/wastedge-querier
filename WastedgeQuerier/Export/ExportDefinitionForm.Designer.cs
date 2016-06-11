namespace WastedgeQuerier.Export
{
    partial class ExportDefinitionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportDefinitionForm));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this._editFilters = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._save = new System.Windows.Forms.ToolStripButton();
            this._exportToExcel = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._path = new SystemEx.Windows.Forms.TextBox();
            this._availableFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._reportFields = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._moveRight = new WastedgeQuerier.SimpleButton();
            this._moveLeft = new WastedgeQuerier.SimpleButton();
            this._moveAllRight = new WastedgeQuerier.SimpleButton();
            this._moveAllLeft = new WastedgeQuerier.SimpleButton();
            this.toolStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._editFilters,
            this.toolStripSeparator2,
            this._save,
            this._exportToExcel});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(657, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // _editFilters
            // 
            this._editFilters.Image = global::WastedgeQuerier.NeutralResources.funnel;
            this._editFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._editFilters.Name = "_editFilters";
            this._editFilters.Size = new System.Drawing.Size(81, 22);
            this._editFilters.Text = "Edit Filters";
            this._editFilters.Click += new System.EventHandler(this._editFilters_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // _save
            // 
            this._save.Image = global::WastedgeQuerier.NeutralResources.floppy_disk;
            this._save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(51, 22);
            this._save.Text = "Save";
            this._save.Click += new System.EventHandler(this._save_Click);
            // 
            // _exportToExcel
            // 
            this._exportToExcel.Image = global::WastedgeQuerier.NeutralResources.excel;
            this._exportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._exportToExcel.Name = "_exportToExcel";
            this._exportToExcel.Size = new System.Drawing.Size(103, 22);
            this._exportToExcel.Text = "Export to Excel";
            this._exportToExcel.Click += new System.EventHandler(this._exportToExcel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(657, 497);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(639, 479);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 1, 5, 5);
            this.groupBox1.Size = new System.Drawing.Size(298, 473);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Fields";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this._path, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this._availableFields, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 14);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(288, 454);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // _path
            // 
            this._path.Dock = System.Windows.Forms.DockStyle.Fill;
            this._path.Location = new System.Drawing.Point(3, 3);
            this._path.Name = "_path";
            this._path.ReadOnly = true;
            this._path.Size = new System.Drawing.Size(282, 20);
            this._path.TabIndex = 0;
            // 
            // _availableFields
            // 
            this._availableFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this._availableFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this._availableFields.HideSelection = false;
            this._availableFields.Location = new System.Drawing.Point(3, 29);
            this._availableFields.Name = "_availableFields";
            this._availableFields.Size = new System.Drawing.Size(282, 422);
            this._availableFields.TabIndex = 1;
            this._availableFields.UseCompatibleStateImageBehavior = false;
            this._availableFields.View = System.Windows.Forms.View.Details;
            this._availableFields.SelectedIndexChanged += new System.EventHandler(this._availableFields_SelectedIndexChanged);
            this._availableFields.SizeChanged += new System.EventHandler(this._availableFields_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._reportFields);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(337, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.groupBox2.Size = new System.Drawing.Size(299, 473);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Fields";
            // 
            // _reportFields
            // 
            this._reportFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this._reportFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this._reportFields.HideSelection = false;
            this._reportFields.Location = new System.Drawing.Point(8, 17);
            this._reportFields.Name = "_reportFields";
            this._reportFields.Size = new System.Drawing.Size(283, 448);
            this._reportFields.TabIndex = 1;
            this._reportFields.UseCompatibleStateImageBehavior = false;
            this._reportFields.View = System.Windows.Forms.View.Details;
            this._reportFields.SelectedIndexChanged += new System.EventHandler(this._reportFields_SelectedIndexChanged);
            this._reportFields.SizeChanged += new System.EventHandler(this._reportFields_SizeChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this._moveRight, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._moveLeft, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this._moveAllRight, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this._moveAllLeft, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(304, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(30, 479);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // _moveRight
            // 
            this._moveRight.Image = global::WastedgeQuerier.NeutralResources.navigate_right;
            this._moveRight.Location = new System.Drawing.Point(3, 63);
            this._moveRight.Name = "_moveRight";
            this._moveRight.Size = new System.Drawing.Size(24, 24);
            this._moveRight.TabIndex = 0;
            this._moveRight.Click += new System.EventHandler(this._moveRight_Click);
            // 
            // _moveLeft
            // 
            this._moveLeft.Image = global::WastedgeQuerier.NeutralResources.navigate_left;
            this._moveLeft.Location = new System.Drawing.Point(3, 93);
            this._moveLeft.Name = "_moveLeft";
            this._moveLeft.Size = new System.Drawing.Size(24, 24);
            this._moveLeft.TabIndex = 0;
            this._moveLeft.Click += new System.EventHandler(this._moveLeft_Click);
            // 
            // _moveAllRight
            // 
            this._moveAllRight.Image = global::WastedgeQuerier.NeutralResources.navigate_right2;
            this._moveAllRight.Location = new System.Drawing.Point(3, 123);
            this._moveAllRight.Name = "_moveAllRight";
            this._moveAllRight.Size = new System.Drawing.Size(24, 24);
            this._moveAllRight.TabIndex = 0;
            this._moveAllRight.Click += new System.EventHandler(this._moveAllRight_Click);
            // 
            // _moveAllLeft
            // 
            this._moveAllLeft.Image = global::WastedgeQuerier.NeutralResources.navigate_left2;
            this._moveAllLeft.Location = new System.Drawing.Point(3, 153);
            this._moveAllLeft.Name = "_moveAllLeft";
            this._moveAllLeft.Size = new System.Drawing.Size(24, 24);
            this._moveAllLeft.TabIndex = 0;
            this._moveAllLeft.Click += new System.EventHandler(this._moveAllLeft_Click);
            // 
            // ExportDefinitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 522);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportDefinitionForm";
            this.ShowInTaskbar = false;
            this.Text = "Export Definition";
            this.Shown += new System.EventHandler(this.ExportDefinitionForm_Shown);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton _editFilters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _save;
        private System.Windows.Forms.ToolStripButton _exportToExcel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private SimpleButton _moveRight;
        private SimpleButton _moveLeft;
        private SimpleButton _moveAllRight;
        private SimpleButton _moveAllLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private SystemEx.Windows.Forms.TextBox _path;
        private System.Windows.Forms.ListView _availableFields;
        private System.Windows.Forms.ListView _reportFields;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}