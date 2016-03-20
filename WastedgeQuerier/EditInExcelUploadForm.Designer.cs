namespace WastedgeQuerier
{
    partial class EditInExcelUploadForm
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
            this.formFlowFooter1 = new SystemEx.Windows.Forms.FormFlowFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this.formHeader1 = new SystemEx.Windows.Forms.FormHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._newCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this._modifiedCheckBox = new System.Windows.Forms.CheckBox();
            this._deletedCheckBox = new System.Windows.Forms.CheckBox();
            this.formFlowFooter1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFlowFooter1
            // 
            this.formFlowFooter1.Controls.Add(this._cancelButton);
            this.formFlowFooter1.Controls.Add(this._acceptButton);
            this.formFlowFooter1.Location = new System.Drawing.Point(0, 168);
            this.formFlowFooter1.Name = "formFlowFooter1";
            this.formFlowFooter1.Size = new System.Drawing.Size(462, 45);
            this.formFlowFooter1.TabIndex = 0;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(366, 11);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 0;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(254, 11);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(106, 23);
            this._acceptButton.TabIndex = 1;
            this._acceptButton.Text = "Upload Changes";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // formHeader1
            // 
            this.formHeader1.Location = new System.Drawing.Point(0, 0);
            this.formHeader1.Name = "formHeader1";
            this.formHeader1.Size = new System.Drawing.Size(462, 47);
            this.formHeader1.SubText = "Upload changes made in Excel back to Wastedge.";
            this.formHeader1.TabIndex = 1;
            this.formHeader1.Text = "Upload Changes";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 47);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(462, 121);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._newCheckBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._modifiedCheckBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._deletedCheckBox, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(444, 9999);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(444, 103);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _newCheckBox
            // 
            this._newCheckBox.AutoSize = true;
            this._newCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._newCheckBox.Location = new System.Drawing.Point(20, 35);
            this._newCheckBox.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this._newCheckBox.Name = "_newCheckBox";
            this._newCheckBox.Size = new System.Drawing.Size(133, 18);
            this._newCheckBox.TabIndex = 3;
            this._newCheckBox.Text = "{0} records were new";
            this._newCheckBox.UseVisualStyleBackColor = true;
            this._newCheckBox.CheckedChanged += new System.EventHandler(this._newCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following changes were detected. Please confirm the changes by ticking the ch" +
    "eck boxes and click Upload Changes to upload the changes back to Wastedge.";
            // 
            // _modifiedCheckBox
            // 
            this._modifiedCheckBox.AutoSize = true;
            this._modifiedCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._modifiedCheckBox.Location = new System.Drawing.Point(20, 59);
            this._modifiedCheckBox.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this._modifiedCheckBox.Name = "_modifiedCheckBox";
            this._modifiedCheckBox.Size = new System.Drawing.Size(152, 18);
            this._modifiedCheckBox.TabIndex = 1;
            this._modifiedCheckBox.Text = "{0} records were modified";
            this._modifiedCheckBox.UseVisualStyleBackColor = true;
            this._modifiedCheckBox.CheckedChanged += new System.EventHandler(this._modifiedCheckBox_CheckedChanged);
            // 
            // _deletedCheckBox
            // 
            this._deletedCheckBox.AutoSize = true;
            this._deletedCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._deletedCheckBox.Location = new System.Drawing.Point(20, 83);
            this._deletedCheckBox.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this._deletedCheckBox.Name = "_deletedCheckBox";
            this._deletedCheckBox.Size = new System.Drawing.Size(148, 18);
            this._deletedCheckBox.TabIndex = 2;
            this._deletedCheckBox.Text = "{0} records were deleted";
            this._deletedCheckBox.UseVisualStyleBackColor = true;
            this._deletedCheckBox.CheckedChanged += new System.EventHandler(this._deletedCheckBox_CheckedChanged);
            // 
            // EditInExcelUploadForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(462, 213);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formHeader1);
            this.Controls.Add(this.formFlowFooter1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditInExcelUploadForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upload Changes";
            this.formFlowFooter1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SystemEx.Windows.Forms.FormFlowFooter formFlowFooter1;
        private SystemEx.Windows.Forms.FormHeader formHeader1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _modifiedCheckBox;
        private System.Windows.Forms.CheckBox _deletedCheckBox;
        private System.Windows.Forms.CheckBox _newCheckBox;
    }
}