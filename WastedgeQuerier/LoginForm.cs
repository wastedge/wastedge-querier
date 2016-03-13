using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WastedgeApi;

namespace WastedgeQuerier
{
    public partial class LoginForm : SystemEx.Windows.Forms.Form
    {
        private const string PackageCode = "WastedgeQuerier";

        public ApiCredentials Credentials => new ApiCredentials(_url.Text, _company.Text, _userName.Text, _password.Text);

        public LoginForm()
        {
            InitializeComponent();

            using (var key = Program.BaseKey)
            {
                _url.Text = (string)key.GetValue("URL");
                _company.Text = (string)key.GetValue("Company");
                _userName.Text = (string)key.GetValue("User name");

#if DEBUG
                _password.Text = (string)key.GetValue("Password");
#endif
            }

            UpdateEnabled();
        }

        private void _url_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _company_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _userName_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _password_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _acceptButton.Enabled =
                _url.Text.Length > 0 &&
                _company.Text.Length > 0 &&
                _userName.Text.Length > 0 &&
                _password.Text.Length > 0;
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
#if !DEBUG
            ThreadPool.QueueUserWorkItem(p => CheckForUpdates());
#endif

            if (_password.Text.Length > 0)
                _acceptButton.PerformClick();
            else if (_userName.Text.Length > 0)
                _password.Focus();
        }

        private async void _acceptButton_Click(object sender, EventArgs e)
        {
            Enabled = false;

            try
            {
                await new Api(Credentials).GetSchemaAsync();

                using (var key = Program.BaseKey)
                {
                    key.SetValue("URL", _url.Text);
                    key.SetValue("Company", _company.Text);
                    key.SetValue("User name", _userName.Text);

#if DEBUG
                    key.SetValue("Password", _password.Text);
#endif
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                Enabled = true;

                MessageBox.Show(this, "Invalid user name or password", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckForUpdates()
        {
            bool updateAvailable = NuGetUpdate.Update.IsUpdateAvailable(PackageCode);
            if (!updateAvailable)
                return;

            BeginInvoke(new Action(InstallUpdate));
        }

        private void InstallUpdate()
        {
            if (IsDisposed)
                return;

            MessageBox.Show(
                this,
                "A new version of Wastedge Querier is available.",
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();

            NuGetUpdate.Update.StartUpdate(PackageCode, args);

            Dispose();
        }
    }
}
