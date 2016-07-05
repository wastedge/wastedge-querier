using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WastedgeQuerier.JavaScript;
using IOPath = System.IO.Path;

namespace WastedgeQuerier
{
    public partial class SaveForm : SystemEx.Windows.Forms.Form
    {
        private string _path;
        private string _extension;

        public string Path
        {
            get { return IOPath.Combine(_path, _nameTextBox.Text + _extension); }
            set
            {
                _nameTextBox.Text = IOPath.GetFileNameWithoutExtension(value);
                _extension = IOPath.GetExtension(value);
                _path = IOPath.GetDirectoryName(value);
            }
        }

        public SaveForm()
        {
            InitializeComponent();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            if (_nameTextBox.Text.Length == 0)
                TaskDialogEx.Show(this, "Please enter a name", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
            else if (File.Exists(Path))
                TaskDialogEx.Show(this, "File name already exists", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
            else if (Directory.Exists(Path))
                TaskDialogEx.Show(this, "Directory already exists", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
            else
                DialogResult = DialogResult.OK;
        }

        private void SaveForm_Shown(object sender, EventArgs e)
        {
            string extension = IOPath.GetExtension(_nameTextBox.Text);
            if (!String.IsNullOrEmpty(extension))
                _nameTextBox.Select(0, _nameTextBox.Text.Length - extension.Length);
        }
    }
}
