namespace WastedgeQuerier
{
    partial class EntityFiltersForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this._filterContainer = new System.Windows.Forms.Panel();
            this._filterControls = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._add = new System.Windows.Forms.Button();
            this._filter = new System.Windows.Forms.ComboBox();
            this._acceptButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this._filterContainer.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._acceptButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._cancelButton, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(456, 358);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tableLayoutPanel1.SetColumnSpan(this.panel3, 3);
            this.panel3.Controls.Add(this._filterContainer);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 32);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(450, 294);
            this.panel3.TabIndex = 1;
            // 
            // _filterContainer
            // 
            this._filterContainer.AutoScroll = true;
            this._filterContainer.BackColor = System.Drawing.SystemColors.Control;
            this._filterContainer.Controls.Add(this._filterControls);
            this._filterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filterContainer.Location = new System.Drawing.Point(1, 1);
            this._filterContainer.Name = "_filterContainer";
            this._filterContainer.Size = new System.Drawing.Size(448, 292);
            this._filterContainer.TabIndex = 0;
            // 
            // _filterControls
            // 
            this._filterControls.AutoSize = true;
            this._filterControls.Dock = System.Windows.Forms.DockStyle.Top;
            this._filterControls.Location = new System.Drawing.Point(0, 0);
            this._filterControls.Name = "_filterControls";
            this._filterControls.Size = new System.Drawing.Size(448, 0);
            this._filterControls.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this._add, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._filter, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(456, 29);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // _add
            // 
            this._add.Location = new System.Drawing.Point(409, 3);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(44, 23);
            this._add.TabIndex = 1;
            this._add.Text = "Add";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _filter
            // 
            this._filter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this._filter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filter.FormattingEnabled = true;
            this._filter.Location = new System.Drawing.Point(3, 3);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(400, 21);
            this._filter.TabIndex = 0;
            this._filter.SizeChanged += new System.EventHandler(this._filter_SizeChanged);
            this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(297, 332);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 2;
            this._acceptButton.Text = "OK";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(378, 332);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // EntityFiltersForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(474, 376);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EntityFiltersForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filters";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this._filterContainer.ResumeLayout(false);
            this._filterContainer.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox _filter;
        private System.Windows.Forms.Button _add;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel _filterContainer;
        private System.Windows.Forms.Panel _filterControls;
    }
}