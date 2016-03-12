using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using WastedgeApi;

namespace WastedgeQuerier
{
    static class Program
    {
        public static RegistryKey BaseKey => Registry.CurrentUser.CreateSubKey("Software\\Wastedge Querier");

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApiCredentials credentials;

            using (var form = new LoginForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                credentials = form.Credentials;
            }

            Application.Run(new MainForm(credentials));
        }
    }
}
