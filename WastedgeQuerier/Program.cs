using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using NBug.Core.Submission.Web;
using WastedgeApi;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    internal static class Program
    {
        public static RegistryKey BaseKey => Registry.CurrentUser.CreateSubKey("Software\\Wastedge Querier");
        public static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wastedge Querier");

        [STAThread]
        static void Main()
        {
#if !DEBUG
            NBug.Settings.ReleaseMode = true;
            NBug.Settings.StoragePath = NBug.Enums.StoragePath.IsolatedStorage;
            NBug.Settings.UIMode = NBug.Enums.UIMode.Full;
            NBug.Settings.Destinations.Add(new Http { Url = "https://bugreport.gmt.nl/api" });

            AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            Application.ThreadException += NBug.Handler.ThreadException;
#endif

            if (SendData())
                return;

            Directory.CreateDirectory(DataPath);

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

            Application.Run(new MainForm(credentials));
        }

        private static bool SendData()
        {
            if ((Control.ModifierKeys & Keys.Shift) != 0)
                return false;

            return CopyDataTarget.SendData(Environment.CommandLine);
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

            ShellRegistration.Register(
                "WastedgeQuerier.Export",
                ShellRegistrationLocation.CurrentUser,
                new ShellRegistrationConfiguration
                {
                    ContentType = "application/wqexport",
                    DefaultIconIndex = 0,
                    FileExtension = ".wqexport",
                    FriendlyAppName = "Wastedge Querier",
                    FriendlyTypeName = "Wastedge Querier Export",
                    OpenCommandArguments = "\"%1\"",
                    ExecutableFileName = typeof(Program).Assembly.Location,
                    DefaultIcon = LoadIcon("report.ico")
                }
            );

            ShellRegistration.Register(
                "WastedgeQuerier.Report",
                ShellRegistrationLocation.CurrentUser,
                new ShellRegistrationConfiguration
                {
                    ContentType = "application/wqreport",
                    DefaultIconIndex = 0,
                    FileExtension = ".wqreport",
                    FriendlyAppName = "Wastedge Querier",
                    FriendlyTypeName = "Wastedge Querier Report",
                    OpenCommandArguments = "\"%1\"",
                    ExecutableFileName = typeof(Program).Assembly.Location,
                    DefaultIcon = LoadIcon("report.ico")
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
