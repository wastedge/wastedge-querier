namespace WastedgeQuerier.EditInExcel
{
    partial class ValidationErrorsForm
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
            this.formHeader1 = new SystemEx.Windows.Forms.FormHeader();
            this.formFlowFooter1 = new SystemEx.Windows.Forms.FormFlowFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this._listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.formFlowFooter1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formHeader1
            // 
            this.formHeader1.Location = new System.Drawing.Point(0, 0);
            this.formHeader1.Name = "formHeader1";
            this.formHeader1.Size = new System.Drawing.Size(558, 60);
            this.formHeader1.SubText = "One or more changes could not be made because validation failed. Review the error" +
    "s below and either re-open the Excel file or cancel your changes.";
            this.formHeader1.TabIndex = 0;
            this.formHeader1.Text = "Validation failed";
            // 
            // formFlowFooter1
            // 
            this.formFlowFooter1.Controls.Add(this._cancelButton);
            this.formFlowFooter1.Controls.Add(this._acceptButton);
            this.formFlowFooter1.Location = new System.Drawing.Point(0, 352);
            this.formFlowFooter1.Name = "formFlowFooter1";
            this.formFlowFooter1.Size = new System.Drawing.Size(558, 45);
            this.formFlowFooter1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(462, 11);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 0;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(352, 11);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(104, 23);
            this._acceptButton.TabIndex = 1;
            this._acceptButton.Text = "&Edit Excel";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._listView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(558, 292);
            this.panel1.TabIndex = 2;
            // 
            // _listView
            // 
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FullRowSelect = true;
            this._listView.HideSelection = false;
            this._listView.Location = new System.Drawing.Point(9, 9);
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(540, 274);
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Row";
            this.columnHeader1.Width = 38;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Field";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Message";
            this.columnHeader3.Width = 342;
            // 
            // ValidationErrorsForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(558, 397);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFlowFooter1);
            this.Controls.Add(this.formHeader1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidationErrorsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Validation Errors";
            this.formFlowFooter1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SystemEx.Windows.Forms.FormHeader formHeader1;
        private SystemEx.Windows.Forms.FormFlowFooter formFlowFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView _listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}