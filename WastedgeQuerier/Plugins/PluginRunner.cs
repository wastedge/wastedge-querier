using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Jint;
using Jint.Parser;
using Jint.Runtime;
using JintDebugger;
using WastedgeApi;
using WastedgeQuerier.JavaScript;

namespace WastedgeQuerier.Plugins
{
    internal class PluginRunner : IDisposable
    {
        private Engine _engine;
        private ZipFile _zipFile;
        private bool _disposed;
        private readonly CaptureOutput _output = new CaptureOutput();

        public PluginRunner(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            _zipFile = new ZipFile(path);
        }

        public void Run(ApiCredentials credentials, Form owner, bool debugMode)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            _engine = new Engine(p => p
                .DebugMode(debugMode)
                .AllowClr()
            );

            _engine.SetValue("require", new Func<string, object>(RequireFunction));
            _engine.SetValue("console", FirebugConsole.CreateFirebugConsole(_engine, _output));

            new JavaScriptUtil(credentials).Setup(_engine, owner);

            try
            {
                RequireFunction("main.js");
            }
            catch (Exception ex)
            {
                var javaScriptException = ex as JavaScriptException;
                if (javaScriptException != null)
                {
                    ExceptionForm.Show(owner, javaScriptException);
                    return;
                }

                MessageBox.Show(
                    owner,
                    new StringBuilder()
                        .AppendLine("An exception occurred while executing the script:")
                        .AppendLine()
                        .Append(ex.Message).Append(" (").Append(ex.GetType().FullName).AppendLine(")")
                        .AppendLine()
                        .AppendLine(ex.StackTrace)
                        .ToString(),
                    owner.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private object RequireFunction(string fileName)
        {
            fileName = fileName.Replace('\\', '/');

            int entry = _zipFile.FindEntry(fileName, true);
            if (entry == -1)
                throw new FileNotFoundException("Cannot find file", fileName);

            string content;

            using (var stream = _zipFile.GetInputStream(entry))
            using (var reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }

            return _engine.Execute(content, new ParserOptions { Source = fileName });
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_zipFile != null)
                {
                    _zipFile.Close();
                    _zipFile = null;
                }

                _disposed = true;
            }
        }
    }
}
