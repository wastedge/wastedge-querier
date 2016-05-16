using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    partial class JavaScriptForm
    {
        private ToolStripMenuItem _closeProject;
        private ToolStripMenuItem _recentFiles;
        private ToolStripMenuItem _recentProjects;
        private ToolStripMenuItem _exportProject;

        private void SetupMenu()
        {
            var fileMenu = (ToolStripMenuItem)Menu.Items[0];

            // Project new and open come after the first divider.

            var newOpenProject = fileMenu.DropDownItems.OfType<ToolStripSeparator>().First();
            var saveSeparator = fileMenu.DropDownItems.OfType<ToolStripSeparator>().Skip(1).First();
            var closeSeparator = fileMenu.DropDownItems.OfType<ToolStripSeparator>().Skip(2).First();

            var newProject = new ToolStripMenuItem
            {
                Text = "New &Project...",
                ShortcutKeys = Keys.Control | Keys.Shift | Keys.N
            };

            newProject.Click += NewProject_Click;

            var openProject = new ToolStripMenuItem
            {
                Text = "O&pen Project...",
                ShortcutKeys = Keys.Control | Keys.Shift | Keys.O
            };

            openProject.Click += OpenProject_Click;

            var projectSeparator = new ToolStripSeparator();

            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(newOpenProject) + 1, projectSeparator);
            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(newOpenProject) + 1, openProject);
            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(newOpenProject) + 1, newProject);

            // Add a save all menu item after the save items.

            var saveAs = (ToolStripMenuItem)fileMenu.DropDownItems[fileMenu.DropDownItems.IndexOf(saveSeparator) - 1];

            saveAs.ShortcutKeys = 0;

            var saveAll = new ToolStripMenuItem
            {
                Text = "Save A&ll",
                ShortcutKeys = Keys.Control | Keys.Shift | Keys.S
            };

            saveAll.Click += SaveAll_Click;

            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(saveSeparator), saveAll);

            // Add a close project menu item.

            _closeProject = new ToolStripMenuItem
            {
                Text = "Clos&e Project",
                Enabled = false
            };

            _closeProject.Click += CloseProject_Click;

            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(closeSeparator), _closeProject);

            // Add an export menu item.

            _exportProject = new ToolStripMenuItem
            {
                Text = "&Export Project...",
                Enabled = false
            };

            _exportProject.Click += ExportProject_Click;

            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(closeSeparator) + 1, new ToolStripSeparator());
            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(closeSeparator) + 1, _exportProject);

            // Add the recent items menu items.

            _recentFiles = new ToolStripMenuItem
            {
                Text = "Recent &Files"
            };

            _recentProjects = new ToolStripMenuItem
            {
                Text = "Recent P&rojects"
            };

            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(closeSeparator) + 1, new ToolStripSeparator());
            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(closeSeparator) + 1, _recentProjects);
            fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.IndexOf(closeSeparator) + 1, _recentFiles);

            var help = new ToolStripMenuItem
            {
                Text = "&Help"
            };

            Menu.Items.Add(help);

            var helpOpenHelp = new ToolStripMenuItem
            {
                Text = "&Open Help",
                ShortcutKeys = Keys.F1
            };

            help.DropDownItems.Add(helpOpenHelp);

            helpOpenHelp.Click += HelpOpenHelp_Click;

            var helpJavaScriptAPIHelp = new ToolStripMenuItem
            {
                Text = "&JavaScript API Help"
            };

            help.DropDownItems.Add(helpJavaScriptAPIHelp);

            helpJavaScriptAPIHelp.Click += HelpJavaScriptAPIHelp_Click;
        }
    }
}
