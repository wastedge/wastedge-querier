namespace WastedgeQuerier.Support
{
    partial class FileBrowser
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
            this._listView = new SystemEx.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // _listView
            // 
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FullRowSelect = true;
            this._listView.HideSelection = false;
            this._listView.LabelEdit = true;
            this._listView.Location = new System.Drawing.Point(0, 0);
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(463, 290);
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this._listView_AfterLabelEdit);
            this._listView.ItemActivate += new System.EventHandler(this._listView_ItemActivate);
            this._listView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._listView_ItemDrag);
            this._listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this._listView_ItemSelectionChanged);
            this._listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this._listView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 260;
            // 
            // FileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._listView);
            this.Name = "FileBrowser";
            this.Size = new System.Drawing.Size(463, 290);
            this.ResumeLayout(false);

        }

        #endregion

        private SystemEx.Windows.Forms.ListView _listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
