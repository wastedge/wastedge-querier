using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier
{
    public partial class LoadingForm : Form
    {
        public event EventHandler CancelClicked;

        public LoadingForm()
        {
            InitializeComponent();
        }

        public string LoadingText
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
                Update();
            }
        }

        protected virtual void OnCancelClicked(EventArgs e)
        {
            CancelClicked?.Invoke(this, e);
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            OnCancelClicked(EventArgs.Empty);
        }
    }
}
