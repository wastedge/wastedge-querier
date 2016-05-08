using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript
{
    partial class JavaScriptForm
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
            this.components = new System.ComponentModel.Container();
            this._folderAddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._folderAddFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._folderDeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._folderRenameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._folderContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._projectAddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._projectAddFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this._projectPropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._projectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._fileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this._fileDeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._fileRenameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._fileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._folderContextMenu.SuspendLayout();
            this._projectContextMenu.SuspendLayout();
            this._fileContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _folderAddMenuItem
            // 
            this._folderAddMenuItem.Name = "_folderAddMenuItem";
            this._folderAddMenuItem.Size = new System.Drawing.Size(136, 22);
            this._folderAddMenuItem.Text = "&Add File";
            this._folderAddMenuItem.Click += new System.EventHandler(this._folderAddMenuItem_Click);
            // 
            // _folderAddFolderMenuItem
            // 
            this._folderAddFolderMenuItem.Name = "_folderAddFolderMenuItem";
            this._folderAddFolderMenuItem.Size = new System.Drawing.Size(136, 22);
            this._folderAddFolderMenuItem.Text = "Add &Folder";
            this._folderAddFolderMenuItem.Click += new System.EventHandler(this._folderAddFolderMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(133, 6);
            // 
            // _folderDeleteMenuItem
            // 
            this._folderDeleteMenuItem.Name = "_folderDeleteMenuItem";
            this._folderDeleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this._folderDeleteMenuItem.Size = new System.Drawing.Size(136, 22);
            this._folderDeleteMenuItem.Text = "&Delete";
            this._folderDeleteMenuItem.Click += new System.EventHandler(this._folderDeleteMenuItem_Click);
            // 
            // _folderRenameMenuItem
            // 
            this._folderRenameMenuItem.Name = "_folderRenameMenuItem";
            this._folderRenameMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this._folderRenameMenuItem.Size = new System.Drawing.Size(136, 22);
            this._folderRenameMenuItem.Text = "&Rename";
            this._folderRenameMenuItem.Click += new System.EventHandler(this._folderRenameMenuItem_Click);
            // 
            // _folderContextMenu
            // 
            this._folderContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._folderAddMenuItem,
            this._folderAddFolderMenuItem,
            this.toolStripMenuItem1,
            this._folderDeleteMenuItem,
            this._folderRenameMenuItem});
            this._folderContextMenu.Name = "_folderContextMenu";
            this._folderContextMenu.Size = new System.Drawing.Size(137, 98);
            // 
            // _projectAddMenuItem
            // 
            this._projectAddMenuItem.Name = "_projectAddMenuItem";
            this._projectAddMenuItem.Size = new System.Drawing.Size(152, 22);
            this._projectAddMenuItem.Text = "&Add File";
            this._projectAddMenuItem.Click += new System.EventHandler(this._projectAddMenuItem_Click);
            // 
            // _projectAddFolderMenuItem
            // 
            this._projectAddFolderMenuItem.Name = "_projectAddFolderMenuItem";
            this._projectAddFolderMenuItem.Size = new System.Drawing.Size(152, 22);
            this._projectAddFolderMenuItem.Text = "Add &Folder";
            this._projectAddFolderMenuItem.Click += new System.EventHandler(this._projectAddFolderMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(149, 6);
            // 
            // _projectPropertiesMenuItem
            // 
            this._projectPropertiesMenuItem.Name = "_projectPropertiesMenuItem";
            this._projectPropertiesMenuItem.Size = new System.Drawing.Size(152, 22);
            this._projectPropertiesMenuItem.Text = "P&roperties";
            this._projectPropertiesMenuItem.Click += new System.EventHandler(this._projectPropertiesMenuItem_Click);
            // 
            // _projectContextMenu
            // 
            this._projectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._projectAddMenuItem,
            this._projectAddFolderMenuItem,
            this.toolStripMenuItem6,
            this._projectPropertiesMenuItem});
            this._projectContextMenu.Name = "_folderContextMenu";
            this._projectContextMenu.Size = new System.Drawing.Size(153, 98);
            // 
            // _fileOpenMenuItem
            // 
            this._fileOpenMenuItem.Name = "_fileOpenMenuItem";
            this._fileOpenMenuItem.Size = new System.Drawing.Size(136, 22);
            this._fileOpenMenuItem.Text = "&Open";
            this._fileOpenMenuItem.Click += new System.EventHandler(this._fileOpenMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(133, 6);
            // 
            // _fileDeleteMenuItem
            // 
            this._fileDeleteMenuItem.Name = "_fileDeleteMenuItem";
            this._fileDeleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this._fileDeleteMenuItem.Size = new System.Drawing.Size(136, 22);
            this._fileDeleteMenuItem.Text = "&Delete";
            this._fileDeleteMenuItem.Click += new System.EventHandler(this._fileDeleteMenuItem_Click);
            // 
            // _fileRenameMenuItem
            // 
            this._fileRenameMenuItem.Name = "_fileRenameMenuItem";
            this._fileRenameMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this._fileRenameMenuItem.Size = new System.Drawing.Size(136, 22);
            this._fileRenameMenuItem.Text = "&Rename";
            this._fileRenameMenuItem.Click += new System.EventHandler(this._fileRenameMenuItem_Click);
            // 
            // _fileContextMenu
            // 
            this._fileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileOpenMenuItem,
            this.toolStripMenuItem3,
            this._fileDeleteMenuItem,
            this._fileRenameMenuItem});
            this._fileContextMenu.Name = "_fileContextMenu";
            this._fileContextMenu.Size = new System.Drawing.Size(137, 76);
            // 
            // JavaScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(691, 515);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "JavaScriptForm";
            this._folderContextMenu.ResumeLayout(false);
            this._projectContextMenu.ResumeLayout(false);
            this._fileContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem _folderAddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _folderAddFolderMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _folderDeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _folderRenameMenuItem;
        private System.Windows.Forms.ContextMenuStrip _folderContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _projectAddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _projectAddFolderMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem _projectPropertiesMenuItem;
        private System.Windows.Forms.ContextMenuStrip _projectContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _fileOpenMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem _fileDeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _fileRenameMenuItem;
        private System.Windows.Forms.ContextMenuStrip _fileContextMenu;
    }
}
