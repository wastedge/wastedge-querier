namespace WastedgeQuerier.JavaScript
{
    partial class UiForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.formFlowFooter1 = new SystemEx.Windows.Forms.FormFlowFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this._container = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.formFlowFooter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this._container);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(478, 23);
            this.panel1.TabIndex = 0;
            // 
            // formFlowFooter1
            // 
            this.formFlowFooter1.Controls.Add(this._cancelButton);
            this.formFlowFooter1.Controls.Add(this._acceptButton);
            this.formFlowFooter1.Location = new System.Drawing.Point(0, 23);
            this.formFlowFooter1.Name = "formFlowFooter1";
            this.formFlowFooter1.Size = new System.Drawing.Size(478, 45);
            this.formFlowFooter1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(382, 11);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 0;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(301, 11);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 1;
            this._acceptButton.Text = "OK";
            this._acceptButton.UseVisualStyleBackColor = true;
            // 
            // _container
            // 
            this._container.AutoSize = true;
            this._container.ColumnCount = 2;
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(8, 8);
            this._container.Name = "_container";
            this._container.RowCount = 1;
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._container.Size = new System.Drawing.Size(462, 7);
            this._container.TabIndex = 0;
            // 
            // UiForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(478, 68);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFlowFooter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UiForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UiForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.formFlowFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SystemEx.Windows.Forms.FormFlowFooter formFlowFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.TableLayoutPanel _container;
    }
}