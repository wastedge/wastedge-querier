namespace WastedgeQuerier
{
    partial class SelectEntityForm
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
            this._acceptButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._filter = new System.Windows.Forms.TextBox();
            this._entities = new WastedgeQuerier.Support.DoubleBufferListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._acceptButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._cancelButton, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this._filter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._entities, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(488, 386);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(329, 360);
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
            this._cancelButton.Location = new System.Drawing.Point(410, 360);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _filter
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._filter, 3);
            this._filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filter.Location = new System.Drawing.Point(3, 3);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(482, 20);
            this._filter.TabIndex = 0;
            this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
            // 
            // _entities
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._entities, 3);
            this._entities.Dock = System.Windows.Forms.DockStyle.Fill;
            this._entities.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._entities.FormattingEnabled = true;
            this._entities.IntegralHeight = false;
            this._entities.Location = new System.Drawing.Point(3, 29);
            this._entities.Name = "_entities";
            this._entities.Size = new System.Drawing.Size(482, 325);
            this._entities.TabIndex = 1;
            this._entities.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._entities_DrawItem);
            this._entities.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this._entities_MeasureItem);
            this._entities.SelectedIndexChanged += new System.EventHandler(this._entities_SelectedIndexChanged);
            this._entities.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._entities_MouseDoubleClick);
            // 
            // SelectEntityForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(506, 404);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectEntityForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Table";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TextBox _filter;
        private WastedgeQuerier.Support.DoubleBufferListBox _entities;
    }
}