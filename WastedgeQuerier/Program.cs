using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using WastedgeApi;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    static class Program
    {
        public static RegistryKey BaseKey => Registry.CurrentUser.CreateSubKey("Software\\Wastedge Querier");
        public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
            RegisterExtensions();
#endif

            ToolStripManager.Renderer = new VS2012ToolStripRenderer();

            ApiCredentials credentials;

            using (var form = new LoginForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                credentials = form.Credentials;
            }

            Application.Run(new MainForm(credentials, args));
        }

        private static void RegisterExtensions()
        {
            ShellRegistration.Register(
                "WastedgeQuerier.Project",
                ShellRegistrationLocation.CurrentUser,
                new ShellRegistrationConfiguration
                {
                    ContentType = "text/xml",
                    DefaultIconIndex = 0,
                    FileExtension = ".weproj",
                    FriendlyAppName = "Wastedge Querier",
                    FriendlyTypeName = "Wastedge Querier Project",
                    OpenCommandArguments = "\"%1\"",
                    ExecutableFileName = typeof(Program).Assembly.Location,
                    DefaultIcon = LoadIcon("document.ico")
                }
            );

            ShellRegistration.Register(
                "WastedgeQuerier.Plugin",
                ShellRegistrationLocation.CurrentUser,
                new ShellRegistrationConfiguration
                {
                    ContentType = "application/wqpkg",
                    DefaultIconIndex = 0,
                    FileExtension = ".wqpkg",
                    FriendlyAppName = "Wastedge Querier",
                    FriendlyTypeName = "Wastedge Querier Plugin",
                    OpenCommandArguments = "\"%1\"",
                    ExecutableFileName = typeof(Program).Assembly.Location,
                    DefaultIcon = LoadIcon("package.ico")
                }
            );
        }

        private static string LoadIcon(string fileName)
        {
            string resourceName = typeof(Program).Namespace + ".Resources." + fileName;
            var targetFileName = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), fileName);

            using (var source = typeof(Program).Assembly.GetManifestResourceStream(resourceName))
            using (var target = File.Create(targetFileName))
            {
                source.CopyTo(target);
            }

            return targetFileName;
        }
    }
}
