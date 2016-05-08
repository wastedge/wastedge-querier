namespace WastedgeQuerier.JavaScript
{
    partial class NewProjectForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._location = new System.Windows.Forms.TextBox();
            this._browse = new System.Windows.Forms.Button();
            this.formFlowFooter1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formHeader1
            // 
            this.formHeader1.Location = new System.Drawing.Point(0, 0);
            this.formHeader1.Name = "formHeader1";
            this.formHeader1.Size = new System.Drawing.Size(464, 60);
            this.formHeader1.SubText = "Below enter the name of the project to be created and select the directory to cre" +
    "ate the new project in.";
            this.formHeader1.TabIndex = 0;
            this.formHeader1.Text = "Create a new Wastedge Querier project";
            // 
            // formFlowFooter1
            // 
            this.formFlowFooter1.Controls.Add(this._cancelButton);
            this.formFlowFooter1.Controls.Add(this._acceptButton);
            this.formFlowFooter1.Location = new System.Drawing.Point(0, 129);
            this.formFlowFooter1.Name = "formFlowFooter1";
            this.formFlowFooter1.Size = new System.Drawing.Size(464, 45);
            this.formFlowFooter1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(464, 69);
            this.panel1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(368, 11);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(287, 11);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 0;
            this._acceptButton.Text = "OK";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._name, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._location, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._browse, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 53);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _name
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._name, 2);
            this._name.Dock = System.Windows.Forms.DockStyle.Fill;
            this._name.Location = new System.Drawing.Point(60, 3);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(385, 20);
            this._name.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "Location:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _location
            // 
            this._location.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this._location.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._location.Dock = System.Windows.Forms.DockStyle.Fill;
            this._location.Location = new System.Drawing.Point(60, 29);
            this._location.Name = "_location";
            this._location.Size = new System.Drawing.Size(304, 20);
            this._location.TabIndex = 3;
            // 
            // _browse
            // 
            this._browse.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browse.Location = new System.Drawing.Point(370, 28);
            this._browse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._browse.Name = "_browse";
            this._browse.Size = new System.Drawing.Size(75, 23);
            this._browse.TabIndex = 4;
            this._browse.Text = "Browse";
            this._browse.UseVisualStyleBackColor = true;
            this._browse.Click += new System.EventHandler(this._browse_Click);
            // 
            // NewProjectForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 174);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFlowFooter1);
            this.Controls.Add(this.formHeader1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Project";
            this.formFlowFooter1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SystemEx.Windows.Forms.FormHeader formHeader1;
        private SystemEx.Windows.Forms.FormFlowFooter formFlowFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _location;
        private System.Windows.Forms.Button _browse;
    }
}