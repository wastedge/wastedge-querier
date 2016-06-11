namespace WastedgeQuerier.EditInExcel
{
    partial class ResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this._grid = new SourceGrid.Grid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._exportToExcel = new System.Windows.Forms.ToolStripButton();
            this._editInExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._getMoreResults = new System.Windows.Forms.ToolStripButton();
            this._getAllResults = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12);
            this.panel1.Size = new System.Drawing.Size(709, 493);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this._grid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(685, 469);
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
            this._grid.Size = new System.Drawing.Size(683, 467);
            this._grid.TabIndex = 0;
            this._grid.TabStop = true;
            this._grid.ToolTipText = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exportToExcel,
            this._editInExcel,
            this.toolStripSeparator1,
            this._getMoreResults,
            this._getAllResults});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(709, 25);
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
            // _editInExcel
            // 
            this._editInExcel.Image = global::WastedgeQuerier.NeutralResources.excel;
            this._editInExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._editInExcel.Name = "_editInExcel";
            this._editInExcel.Size = new System.Drawing.Size(89, 22);
            this._editInExcel.Text = "Edit in Excel";
            this._editInExcel.Click += new System.EventHandler(this._editInExcel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _getMoreResults
            // 
            this._getMoreResults.Image = global::WastedgeQuerier.NeutralResources.navigate_down;
            this._getMoreResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._getMoreResults.Name = "_getMoreResults";
            this._getMoreResults.Size = new System.Drawing.Size(116, 22);
            this._getMoreResults.Text = "Get More Results";
            this._getMoreResults.Click += new System.EventHandler(this._getMoreResults_Click);
            // 
            // _getAllResults
            // 
            this._getAllResults.Image = global::WastedgeQuerier.NeutralResources.navigate_down2;
            this._getAllResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._getAllResults.Name = "_getAllResults";
            this._getAllResults.Size = new System.Drawing.Size(102, 22);
            this._getAllResults.Text = "Get All Results";
            this._getAllResults.Click += new System.EventHandler(this._getAllResults_Click);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 518);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResultForm";
            this.ShowInTaskbar = false;
            this.Text = "Results";
            this.Shown += new System.EventHandler(this.ResultForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _exportToExcel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private SourceGrid.Grid _grid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _getMoreResults;
        private System.Windows.Forms.ToolStripButton _getAllResults;
        private System.Windows.Forms.ToolStripButton _editInExcel;
    }
}