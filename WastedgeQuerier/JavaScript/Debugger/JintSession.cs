using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jint;
using Jint.Debugger;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal class JintSession : IDisposable
    {
        private readonly IJintSessionCallback _callback;
        private bool _disposed;
        private Thread _thread;
        private string _baseDirectory;
        private readonly object _syncRoot = new object();
        private bool _breakOnNextStatement;
        private JintEngine _engine;

        public JintSession(IJintSessionCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _callback = callback;
        }

        public event JintDebuggerStoppedEventHandler Stopped;

        protected virtual void OnStopped(JintDebuggerStoppedEventArgs e)
        {
            Stopped?.Invoke(this, e);
        }

        public void Run(string baseDirectory, string script, bool debug, Action<JintEngine> setup)
        {
            _baseDirectory = baseDirectory;

            _thread = new Thread(() => ThreadProc(script, debug, setup));
            _thread.Start();
        }

        private void ThreadProc(string script, bool debug, Action<JintEngine> setup)
        {
            _engine = new JintEngine();

            _engine.Break += engine_Break;
            _engine.Step += engine_Step;

            _engine.SetDebugMode(debug);
            _engine.DisableSecurity();
            _engine.AllowClr();

            _engine.SetFunction("require", new Func<string, object>(RequireFunction));

            setup?.Invoke(_engine);

            Exception exception = null;

            try
            {
                _engine.Run(script);

                _engine = null;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                OnStopped(new JintDebuggerStoppedEventArgs(exception));
            }
        }

        private object RequireFunction(string fileName)
        {
            if (!Path.IsPathRooted(fileName))
                fileName = Path.Combine(_baseDirectory, fileName);

            return _engine.Run(File.ReadAllText(fileName));
        }

        public bool BreakOnNextStatement
        {
            get { return _breakOnNextStatement; }
            set
            {
                if (_breakOnNextStatement != value)
                {
                    _breakOnNextStatement = value;
                    OnBreakOnNextStatementChanged(EventArgs.Empty);
                }
            }
        }

        internal event EventHandler BreakOnNextStatementChanged;

        private void OnBreakOnNextStatementChanged(EventArgs e)
        {
            BreakOnNextStatementChanged?.Invoke(null, e);
        }

        void engine_Break(object sender, DebugInformation e)
        {
            ProcessStep(sender, e, BreakType.Break);
        }

        void engine_Step(object sender, DebugInformation e)
        {
            ProcessStep(sender, e, BreakType.Step);
        }

        private void ProcessStep(object sender, DebugInformation e, BreakType breakType)
        {
            lock (_syncRoot)
            {
                if (breakType == BreakType.Step && !BreakOnNextStatement)
                    return;

                BreakOnNextStatement = false;

                using (var continuation = _callback.ProcessStep((JintEngine)sender, e, breakType))
                {
                    continuation.Wait();
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_thread != null)
                {
                    _thread.Abort();
                    _thread = null;
                }

                _disposed = true;
            }
        }
    }
}
