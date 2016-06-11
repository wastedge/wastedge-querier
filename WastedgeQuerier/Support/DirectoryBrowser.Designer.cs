namespace WastedgeQuerier.Support
{
    partial class DirectoryBrowser
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
            this._treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // _treeView
            // 
            this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeView.HideSelection = false;
            this._treeView.LabelEdit = true;
            this._treeView.Location = new System.Drawing.Point(0, 0);
            this._treeView.Name = "_treeView";
            this._treeView.Size = new System.Drawing.Size(150, 150);
            this._treeView.TabIndex = 0;
            this._treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._treeView_ItemDrag);
            this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeView_AfterSelect);
            this._treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeView_NodeMouseClick);
            // 
            // DirectoryBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._treeView);
            this.Name = "DirectoryBrowser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _treeView;
    }
}
