using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal partial class ProjectPropertiesForm : SystemEx.Windows.Forms.Form
    {
        public string Title
        {
            get { return _title.Text; }
            set { _title.Text = value; }
        }

        public string Description
        {
            get { return _description.Text; }
            set { _description.Text = value; }
        }

        public ProjectPropertiesForm()
        {
            InitializeComponent();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
