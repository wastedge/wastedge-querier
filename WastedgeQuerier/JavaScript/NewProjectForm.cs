using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    public partial class NewProjectForm : SystemEx.Windows.Forms.Form
    {
        public string ProjectName => _name.Text;

        public string ProjectLocation => _location.Text;

        public NewProjectForm()
        {
            InitializeComponent();

            using (var key = Program.BaseKey)
            {
                _location.Text =
                    key.GetValue("Last Project Location") as string ??
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        private void _browse_Click(object sender, EventArgs e)
        {
            string directory = BrowseForFolderDialog.Show(
                this,
                "Project Location",
                BrowseForFolderOptions.ReturnOnlyFileSystemDirectories | BrowseForFolderOptions.UseNewUI
            );

            if (directory != null)
                _location.Text = directory;
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            if (_name.Text.Length == 0)
            {
                TaskDialogEx.Show(this, "Please enter a name", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Warning);
            }
            else if (!Directory.Exists(_location.Text))
            {
                TaskDialogEx.Show(this, "Please select a valid location", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Warning);
            }
            else
            {
                var fullPath = Path.Combine(_location.Text, _name.Text);
                if (Directory.Exists(fullPath))
                {
                    TaskDialogEx.Show(this, $"The directory {fullPath} already exists", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Warning);
                }
                else
                {
                    using (var key = Program.BaseKey)
                    {
                        key.SetValue("Last Project Location", _location.Text);
                    }

                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
