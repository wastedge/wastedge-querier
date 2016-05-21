namespace WastedgeQuerier.Report
{
    partial class ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this._grid = new SourceGrid.Grid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._exportToExcel = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._fields = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._update = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._fieldContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._moveUpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._moveDownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._moveToBeginningMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._moveToEndMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._moveToRowLabelsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._moveToColumnLabelsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._moveToValuesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this._removeFieldMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this._aggregateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sumMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._countMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._averageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._maxMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._minMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._productMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._countNumbersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sampleStandardDeviationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._standardDeviationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sampleVarianceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._varianceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._columns = new WastedgeQuerier.Report.ReportFieldListBox();
            this._rows = new WastedgeQuerier.Report.ReportFieldListBox();
            this._values = new WastedgeQuerier.Report.ReportFieldListBox();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this._fieldContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this._grid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(452, 496);
            this.panel2.TabIndex = 4;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.SystemColors.Window;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EnableSort = true;
            this._grid.Location = new System.Drawing.Point(1, 1);
            this._grid.Name = "_grid";
            this._grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this._grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this._grid.Size = new System.Drawing.Size(450, 494);
            this._grid.TabIndex = 0;
            this._grid.TabStop = true;
            this._grid.ToolTipText = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exportToExcel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(717, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(12);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(9);
            this.splitContainer1.Size = new System.Drawing.Size(717, 520);
            this.splitContainer1.SplitterDistance = 476;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._fields, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._update, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(219, 502);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose fields to add to the report:";
            // 
            // _fields
            // 
            this._fields.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this._fields, 2);
            this._fields.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fields.HideSelection = false;
            this._fields.Location = new System.Drawing.Point(3, 22);
            this._fields.Name = "_fields";
            this._fields.ShowLines = false;
            this._fields.Size = new System.Drawing.Size(213, 253);
            this._fields.TabIndex = 1;
            this._fields.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this._fields_BeforeExpand);
            this._fields.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._fields_ItemDrag);
            this._fields.DragDrop += new System.Windows.Forms.DragEventHandler(this._fields_DragDrop);
            this._fields.DragOver += new System.Windows.Forms.DragEventHandler(this._fields_DragOver);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this._columns, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._rows, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this._values, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 300);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(219, 172);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Columns";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Rows";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label5, 2);
            this.label5.Location = new System.Drawing.Point(3, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Values";
            // 
            // _update
            // 
            this._update.Location = new System.Drawing.Point(141, 475);
            this._update.Name = "_update";
            this._update.Size = new System.Drawing.Size(75, 23);
            this._update.TabIndex = 3;
            this._update.Text = "Update";
            this._update.UseVisualStyleBackColor = true;
            this._update.Click += new System.EventHandler(this._update_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Location = new System.Drawing.Point(3, 281);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Drag fields between areas below:";
            // 
            // _fieldContextMenu
            // 
            this._fieldContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._moveUpMenuItem,
            this._moveDownMenuItem,
            this._moveToBeginningMenuItem,
            this._moveToEndMenuItem,
            this.toolStripMenuItem1,
            this._moveToRowLabelsMenuItem,
            this._moveToColumnLabelsMenuItem,
            this._moveToValuesMenuItem,
            this.toolStripMenuItem2,
            this._removeFieldMenuItem,
            this.toolStripMenuItem3,
            this._aggregateMenuItem});
            this._fieldContextMenu.Name = "_fieldContextMenu";
            this._fieldContextMenu.Size = new System.Drawing.Size(201, 220);
            // 
            // _moveUpMenuItem
            // 
            this._moveUpMenuItem.Name = "_moveUpMenuItem";
            this._moveUpMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveUpMenuItem.Text = "Move &Up";
            this._moveUpMenuItem.Click += new System.EventHandler(this._moveUpMenuItem_Click);
            // 
            // _moveDownMenuItem
            // 
            this._moveDownMenuItem.Name = "_moveDownMenuItem";
            this._moveDownMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveDownMenuItem.Text = "Move &Down";
            this._moveDownMenuItem.Click += new System.EventHandler(this._moveDownMenuItem_Click);
            // 
            // _moveToBeginningMenuItem
            // 
            this._moveToBeginningMenuItem.Name = "_moveToBeginningMenuItem";
            this._moveToBeginningMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveToBeginningMenuItem.Text = "Move to Be&ginning";
            this._moveToBeginningMenuItem.Click += new System.EventHandler(this._moveToBeginningMenuItem_Click);
            // 
            // _moveToEndMenuItem
            // 
            this._moveToEndMenuItem.Name = "_moveToEndMenuItem";
            this._moveToEndMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveToEndMenuItem.Text = "Move to &End";
            this._moveToEndMenuItem.Click += new System.EventHandler(this._moveToEndMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 6);
            // 
            // _moveToRowLabelsMenuItem
            // 
            this._moveToRowLabelsMenuItem.Name = "_moveToRowLabelsMenuItem";
            this._moveToRowLabelsMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveToRowLabelsMenuItem.Text = "Move to &Row Labels";
            this._moveToRowLabelsMenuItem.Click += new System.EventHandler(this._moveToRowLabelsMenuItem_Click);
            // 
            // _moveToColumnLabelsMenuItem
            // 
            this._moveToColumnLabelsMenuItem.Name = "_moveToColumnLabelsMenuItem";
            this._moveToColumnLabelsMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveToColumnLabelsMenuItem.Text = "Move to &Column Labels";
            this._moveToColumnLabelsMenuItem.Click += new System.EventHandler(this._moveToColumnLabelsMenuItem_Click);
            // 
            // _moveToValuesMenuItem
            // 
            this._moveToValuesMenuItem.Name = "_moveToValuesMenuItem";
            this._moveToValuesMenuItem.Size = new System.Drawing.Size(200, 22);
            this._moveToValuesMenuItem.Text = "Move to &Values";
            this._moveToValuesMenuItem.Click += new System.EventHandler(this._moveToValuesMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(197, 6);
            // 
            // _removeFieldMenuItem
            // 
            this._removeFieldMenuItem.Name = "_removeFieldMenuItem";
            this._removeFieldMenuItem.Size = new System.Drawing.Size(200, 22);
            this._removeFieldMenuItem.Text = "Re&move Field";
            this._removeFieldMenuItem.Click += new System.EventHandler(this._removeFieldMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(197, 6);
            // 
            // _aggregateMenuItem
            // 
            this._aggregateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._sumMenuItem,
            this._countMenuItem,
            this._averageMenuItem,
            this._maxMenuItem,
            this._minMenuItem,
            this._productMenuItem,
            this._countNumbersMenuItem,
            this._sampleStandardDeviationMenuItem,
            this._standardDeviationMenuItem,
            this._sampleVarianceMenuItem,
            this._varianceMenuItem});
            this._aggregateMenuItem.Name = "_aggregateMenuItem";
            this._aggregateMenuItem.Size = new System.Drawing.Size(200, 22);
            this._aggregateMenuItem.Text = "&Aggregate";
            // 
            // _sumMenuItem
            // 
            this._sumMenuItem.Name = "_sumMenuItem";
            this._sumMenuItem.Size = new System.Drawing.Size(216, 22);
            this._sumMenuItem.Tag = "Sum";
            this._sumMenuItem.Text = "&Sum";
            this._sumMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _countMenuItem
            // 
            this._countMenuItem.Name = "_countMenuItem";
            this._countMenuItem.Size = new System.Drawing.Size(216, 22);
            this._countMenuItem.Tag = "Count";
            this._countMenuItem.Text = "&Count";
            this._countMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _averageMenuItem
            // 
            this._averageMenuItem.Name = "_averageMenuItem";
            this._averageMenuItem.Size = new System.Drawing.Size(216, 22);
            this._averageMenuItem.Tag = "Average";
            this._averageMenuItem.Text = "&Average";
            this._averageMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _maxMenuItem
            // 
            this._maxMenuItem.Name = "_maxMenuItem";
            this._maxMenuItem.Size = new System.Drawing.Size(216, 22);
            this._maxMenuItem.Tag = "Max";
            this._maxMenuItem.Text = "Ma&x";
            this._maxMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _minMenuItem
            // 
            this._minMenuItem.Name = "_minMenuItem";
            this._minMenuItem.Size = new System.Drawing.Size(216, 22);
            this._minMenuItem.Tag = "Min";
            this._minMenuItem.Text = "M&in";
            this._minMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _productMenuItem
            // 
            this._productMenuItem.Name = "_productMenuItem";
            this._productMenuItem.Size = new System.Drawing.Size(216, 22);
            this._productMenuItem.Tag = "Product";
            this._productMenuItem.Text = "&Product";
            this._productMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _countNumbersMenuItem
            // 
            this._countNumbersMenuItem.Name = "_countNumbersMenuItem";
            this._countNumbersMenuItem.Size = new System.Drawing.Size(216, 22);
            this._countNumbersMenuItem.Tag = "CountNumbers";
            this._countNumbersMenuItem.Text = "Count &Numbers";
            this._countNumbersMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _sampleStandardDeviationMenuItem
            // 
            this._sampleStandardDeviationMenuItem.Name = "_sampleStandardDeviationMenuItem";
            this._sampleStandardDeviationMenuItem.Size = new System.Drawing.Size(216, 22);
            this._sampleStandardDeviationMenuItem.Tag = "StdDev";
            this._sampleStandardDeviationMenuItem.Text = "Sample Standard &Deviation";
            this._sampleStandardDeviationMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _standardDeviationMenuItem
            // 
            this._standardDeviationMenuItem.Name = "_standardDeviationMenuItem";
            this._standardDeviationMenuItem.Size = new System.Drawing.Size(216, 22);
            this._standardDeviationMenuItem.Tag = "StdDevp";
            this._standardDeviationMenuItem.Text = "Standard D&eviation";
            this._standardDeviationMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _sampleVarianceMenuItem
            // 
            this._sampleVarianceMenuItem.Name = "_sampleVarianceMenuItem";
            this._sampleVarianceMenuItem.Size = new System.Drawing.Size(216, 22);
            this._sampleVarianceMenuItem.Tag = "Var";
            this._sampleVarianceMenuItem.Text = "Sample &Variance";
            this._sampleVarianceMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _varianceMenuItem
            // 
            this._varianceMenuItem.Name = "_varianceMenuItem";
            this._varianceMenuItem.Size = new System.Drawing.Size(216, 22);
            this._varianceMenuItem.Tag = "Varp";
            this._varianceMenuItem.Text = "Va&riance";
            this._varianceMenuItem.Click += new System.EventHandler(this._selectAverageMenuItem_Click);
            // 
            // _columns
            // 
            this._columns.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columns.FieldType = WastedgeQuerier.Report.ReportFieldType.Column;
            this._columns.FormattingEnabled = true;
            this._columns.IntegralHeight = false;
            this._columns.Location = new System.Drawing.Point(3, 16);
            this._columns.Name = "_columns";
            this._columns.Size = new System.Drawing.Size(103, 67);
            this._columns.TabIndex = 3;
            this._columns.ItemClick += new WastedgeQuerier.Report.ListBoxItemEventHandler(this._fieldsList_ItemClick);
            // 
            // _rows
            // 
            this._rows.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rows.FieldType = WastedgeQuerier.Report.ReportFieldType.Row;
            this._rows.FormattingEnabled = true;
            this._rows.IntegralHeight = false;
            this._rows.Location = new System.Drawing.Point(112, 16);
            this._rows.Name = "_rows";
            this._rows.Size = new System.Drawing.Size(104, 67);
            this._rows.TabIndex = 3;
            this._rows.ItemClick += new WastedgeQuerier.Report.ListBoxItemEventHandler(this._fieldsList_ItemClick);
            // 
            // _values
            // 
            this.tableLayoutPanel2.SetColumnSpan(this._values, 2);
            this._values.Dock = System.Windows.Forms.DockStyle.Fill;
            this._values.FieldType = WastedgeQuerier.Report.ReportFieldType.Value;
            this._values.FormattingEnabled = true;
            this._values.IntegralHeight = false;
            this._values.Location = new System.Drawing.Point(3, 102);
            this._values.Name = "_values";
            this._values.Size = new System.Drawing.Size(213, 67);
            this._values.TabIndex = 3;
            this._values.ItemClick += new WastedgeQuerier.Report.ListBoxItemEventHandler(this._fieldsList_ItemClick);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 545);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportForm";
            this.ShowInTaskbar = false;
            this.Text = "Report";
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this._fieldContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _exportToExcel;
        private System.Windows.Forms.Panel panel2;
        private SourceGrid.Grid _grid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView _fields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button _update;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ReportFieldListBox _columns;
        private ReportFieldListBox _rows;
        private ReportFieldListBox _values;
        private System.Windows.Forms.ContextMenuStrip _fieldContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _moveUpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _moveDownMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _moveToBeginningMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _moveToEndMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _moveToRowLabelsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _moveToColumnLabelsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _moveToValuesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem _removeFieldMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem _aggregateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _sumMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _countMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _averageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _maxMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _minMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _productMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _countNumbersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _sampleStandardDeviationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _standardDeviationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _sampleVarianceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _varianceMenuItem;
    }
}