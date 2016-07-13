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
    public partial class LoadingForm : SystemEx.Windows.Forms.Form
    {
        public static void Show(IWin32Window owner, Func<LoadingForm, Task> callback)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            Exception exception = null;

            using (var form = new LoadingForm())
            {
                form.Shown += async (s, e) =>
                {
                    try
                    {
                        await callback(form);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        form.Dispose();
                    }
                };

                form.ShowDialog(owner);
            }

            if (exception != null)
                throw exception;
        }

        public event EventHandler CancelClicked;

        private LoadingForm()
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
