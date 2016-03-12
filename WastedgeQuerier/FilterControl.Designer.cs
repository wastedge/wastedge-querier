namespace WastedgeQuerier
{
    partial class FilterControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._name = new System.Windows.Forms.Label();
            this._textBox = new System.Windows.Forms.TextBox();
            this._dateTime = new SystemEx.Windows.Forms.DateTimePickerEx();
            this._numericTextBox = new SystemEx.Windows.Forms.SimpleNumericTextBox();
            this._date = new SystemEx.Windows.Forms.DateTimePicker();
            this._contextMenu = new System.Windows.Forms.ContextMenu();
            this._filter = new WastedgeQuerier.SimpleButton();
            this._close = new WastedgeQuerier.SimpleButton();
            this.SuspendLayout();
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(3, 6);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(48, 13);
            this._name.TabIndex = 0;
            this._name.Text = "<name>:";
            this._name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _textBox
            // 
            this._textBox.Location = new System.Drawing.Point(92, 3);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(176, 20);
            this._textBox.TabIndex = 2;
            // 
            // _dateTime
            // 
            this._dateTime.DateFormat = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dateTime.Location = new System.Drawing.Point(92, 56);
            this._dateTime.Name = "_dateTime";
            this._dateTime.Size = new System.Drawing.Size(176, 21);
            this._dateTime.TabIndex = 4;
            // 
            // _numericTextBox
            // 
            this._numericTextBox.Location = new System.Drawing.Point(92, 83);
            this._numericTextBox.Name = "_numericTextBox";
            this._numericTextBox.Size = new System.Drawing.Size(176, 20);
            this._numericTextBox.TabIndex = 5;
            // 
            // _date
            // 
            this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._date.Location = new System.Drawing.Point(92, 30);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(176, 20);
            this._date.TabIndex = 3;
            // 
            // _filter
            // 
            this._filter.Image = null;
            this._filter.Location = new System.Drawing.Point(65, 3);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(21, 21);
            this._filter.TabIndex = 1;
            this._filter.Text = "=";
            this._filter.Click += new System.EventHandler(this._filter_Click);
            // 
            // _close
            // 
            this._close.Image = global::WastedgeQuerier.NeutralResources.close;
            this._close.Location = new System.Drawing.Point(274, 3);
            this._close.Name = "_close";
            this._close.Padding = new System.Windows.Forms.Padding(5);
            this._close.Size = new System.Drawing.Size(19, 19);
            this._close.TabIndex = 6;
            this._close.Click += new System.EventHandler(this._close_Click);
            // 
            // FilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._date);
            this.Controls.Add(this._numericTextBox);
            this.Controls.Add(this._dateTime);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._filter);
            this.Controls.Add(this._close);
            this.Controls.Add(this._name);
            this.Name = "FilterControl";
            this.Size = new System.Drawing.Size(308, 116);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _name;
        private SimpleButton _close;
        private SimpleButton _filter;
        private System.Windows.Forms.TextBox _textBox;
        private SystemEx.Windows.Forms.DateTimePickerEx _dateTime;
        private SystemEx.Windows.Forms.SimpleNumericTextBox _numericTextBox;
        private SystemEx.Windows.Forms.DateTimePicker _date;
        private System.Windows.Forms.ContextMenu _contextMenu;
    }
}
