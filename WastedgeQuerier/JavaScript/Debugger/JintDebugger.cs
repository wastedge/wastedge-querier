using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Debugger;
using Jint.Native;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal class JintDebugger : IDisposable, IJintSessionCallback
    {
        private bool _isRunning;
        private JintSession _session;
        private bool _disposed;
        private Continuation _continuation;
        private List<JsScope> _allowedScopes;

        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnIsRunningChanged(EventArgs.Empty);
                }
            }
        }

        public DebugInformation DebugInformation { get; private set; }

        public event EventHandler IsRunningChanged;

        protected virtual void OnIsRunningChanged(EventArgs e)
        {
            IsRunningChanged?.Invoke(this, e);
        }

        public event JintDebuggerStoppedEventHandler Stopped;

        protected virtual void OnStopped(JintDebuggerStoppedEventArgs e)
        {
            Stopped?.Invoke(this, e);
        }

        public event JintDebuggerSteppedEventHandler Stepped;

        protected virtual void OnStepped(JintDebuggerSteppedEventArgs e)
        {
            Stepped?.Invoke(this, e);
        }

        public JintDebugger()
        {
            _session = new JintSession(this);
            _session.Stopped += _session_Stopped;
        }

        private void _session_Stopped(object sender, JintDebuggerStoppedEventArgs e)
        {
            IsRunning = false;

            OnStopped(e);
        }

        public void Run(string fileName, string script, Action<JintEngine> setup, bool startBreaked)
        {
            _session.BreakOnNextStatement = startBreaked;
            _session.Run(Path.GetDirectoryName(fileName), script, true, setup);

            IsRunning = true;
        }

        public void Break()
        {
            _session.BreakOnNextStatement = true;
        }

        public void Continue()
        {
            DebugInformation = null;
            IsRunning = true;

            var continuation = _continuation;
            _continuation = null;
            continuation.Signal();
        }

        public void Step(StepType stepType)
        {
            if (stepType != StepType.Into)
            {
                _allowedScopes = new List<JsScope>(DebugInformation.Scopes);

                _allowedScopes.Reverse();

                if (stepType == StepType.Out)
                    _allowedScopes.RemoveAt(_allowedScopes.Count - 1);
            }

            _session.BreakOnNextStatement = true;

            Continue();
        }

        Continuation IJintSessionCallback.ProcessStep(JintEngine engine, DebugInformation debugInformation, BreakType breakType)
        {
            IsRunning = false;

            Debug.Assert(_continuation == null);

            var continuation = new Continuation();

            if (ProcessAllowedScopes(debugInformation))
            {
                continuation.Signal();
            }
            else
            {
                _continuation = continuation;

                DebugInformation = debugInformation;

                OnStepped(new JintDebuggerSteppedEventArgs(engine, breakType));
            }

            return continuation;
        }

        private bool ProcessAllowedScopes(DebugInformation debugInformation)
        {
            // Verify whether we need to step.

            if (_allowedScopes != null)
            {
                var currentScopes = new List<JsScope>(debugInformation.Scopes);

                currentScopes.Reverse();

                bool areEqual = true;

                for (int i = 0, count = Math.Min(currentScopes.Count, _allowedScopes.Count); i < count; i++)
                {
                    if (currentScopes[i] != _allowedScopes[i])
                    {
                        areEqual = false;
                        break;
                    }
                }

                bool doStep = false;

                if (!areEqual)
                {
                    // If the part of the stacks that overlap are not equal, we
                    // know for sure we need to step because we're in a different
                    // scope.

                    doStep = true;
                }
                else if (currentScopes.Count <= _allowedScopes.Count)
                {
                    // If the depth of the current stack is less than the allowed
                    // stacks, we're sure we can step because we went to a
                    // higher scope.

                    doStep = true;
                }

                if (!doStep)
                {
                    _session.BreakOnNextStatement = true;

                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_session != null)
                {
                    _session.Dispose();
                    _session = null;
                }

                _disposed = true;
            }
        }
    }
}
