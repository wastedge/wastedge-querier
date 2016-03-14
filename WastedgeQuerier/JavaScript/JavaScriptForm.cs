using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Jint;
using Jint.Debugger;
using Jint.Native;
using WastedgeApi;
using WastedgeQuerier.JavaScript.Debugger;
using WeifenLuo.WinFormsUI.Docking;

namespace WastedgeQuerier.JavaScript
{
    internal partial class JavaScriptForm : SystemEx.Windows.Forms.Form, IStatusBarProvider
    {
        public const string FileDialogFilter = "JavaScript (*.js)|*.js|All Files (*.*)|*.*";

        private readonly OutputControl _outputControl;
        private readonly Api _api;
        private readonly VariablesControl _localsControl;
        private readonly VariablesControl _globalsControl;
        private readonly CallStackControl _callStackControl;
        private JintDebugger _debugger;
        private readonly Dictionary<string, ProgramControl> _controls = new Dictionary<string, ProgramControl>();
        private int _programCounter;

        private EditorControl ActiveEditor => _dockPanel.ActiveDocument as EditorControl;

        public JavaScriptForm(Api api)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));

            _api = api;

            InitializeComponent();

            _dockPanel.Theme = new VS2012LightTheme();

            _outputControl = new OutputControl();

            _outputControl.Show(_dockPanel, DockState.DockBottom);

            _localsControl = new VariablesControl
            {
                Text = "Locals"
            };

            _localsControl.Show(_dockPanel, DockState.DockBottom);

            _globalsControl = new VariablesControl
            {
                Text = "Globals"
            };

            _globalsControl.Show(_dockPanel, DockState.DockBottom);

            _callStackControl = new CallStackControl();

            _callStackControl.Show(_dockPanel, DockState.DockBottom);

            _outputControl.DockHandler.Activate();

            ResetTabs();

            SetDebugger(null);
        }

        private void SetDebugger(JintDebugger debugger)
        {
            _debugger?.Dispose();

            _debugger = debugger;

            if (_debugger != null)
            {
                _debugger.IsRunningChanged += _debugger_IsRunningChanged;
                _debugger.Stopped += _debugger_Stopped;
                _debugger.Stepped += _debugger_Stepped;
            }

            _viewLocals.Enabled =
            _viewGlobals.Enabled =
            _viewCallStack.Enabled =
                _debugger != null;

            if (_debugger != null)
            {
                _localsControl.DockHandler.Show();
                _globalsControl.DockHandler.Show();
                _callStackControl.DockHandler.Show();
                _outputControl.DockHandler.Activate();
            }
            else
            {
                _localsControl.DockHandler.Hide();
                _globalsControl.DockHandler.Hide();
                _callStackControl.DockHandler.Hide();
            }

            if (_debugger == null)
            {
                foreach (var document in _dockPanel.Documents.OfType<ProgramControl>().ToList())
                {
                    document.Dispose();
                }

                _controls.Clear();
            }
            else
            {
                _programCounter = 0;
            }

            UpdateEnabled();
        }

        private void _debugger_Stepped(object sender, JintDebuggerSteppedEventArgs e)
        {
            BeginInvoke(new Action(() => Stepped(e)));
        }

        private void Stepped(JintDebuggerSteppedEventArgs e)
        {
            ProgramControl control;

            var programSource = _debugger.DebugInformation.Program.ProgramSource;

            if (!_controls.TryGetValue(programSource, out control))
            {
                control = new ProgramControl(_debugger.DebugInformation.Program, this, this);

                control.Text += "Program " + ++_programCounter;

                control.Disposed += (s, ea) => _controls.Remove(programSource);

                _controls.Add(programSource, control);

                control.Show(_dockPanel, DockState.Document);
            }
            else
                control.DockHandler.Activate();

            control.ProcessStep(e.Engine, _debugger.DebugInformation, e.BreakType);
        }

        private void _debugger_IsRunningChanged(object sender, EventArgs e)
        {
            BeginInvoke(new Action(IsRunningChanged));
        }

        private void IsRunningChanged()
        {
            if (_debugger?.DebugInformation != null)
            {
                LoadTabs(_debugger.DebugInformation);
            }
            else
            {
                ResetTabs();

                foreach (var control in _dockPanel.Documents.OfType<ProgramControl>())
                {
                    control.ResetState();
                }
            }

            UpdateEnabled();
        }

        private void _debugger_Stopped(object sender, JintDebuggerStoppedEventArgs e)
        {
            BeginInvoke(new Action(() => Completed(e.Exception)));
        }

        private void Completed(Exception exception)
        {
            SetDebugger(null);

            if (exception == null || exception is ThreadAbortException || exception.InnerException is ThreadAbortException)
                return;

            MessageBox.Show(
                this,
                new StringBuilder()
                    .AppendLine("An exception occurred while executing the script:")
                    .AppendLine()
                    .Append(exception.Message).Append(" (").Append(exception.GetType().FullName).AppendLine(")")
                    .AppendLine()
                    .AppendLine(exception.StackTrace)
                    .ToString(),
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void ResetTabs()
        {
            _localsControl.ResetVariables();
            _globalsControl.ResetVariables();
            _callStackControl.ResetCallStack();
        }

        private void _fileNew_Click(object sender, EventArgs e)
        {
            OpenEditor(null);
        }

        private void _fileOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = true;
                dialog.Filter = FileDialogFilter;
                dialog.Multiselect = false;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    OpenEditor(dialog.FileName);
                }
            }
        }

        private void OpenEditor(string fileName)
        {
            var editor = new EditorControl(this);

            if (fileName != null)
                editor.Open(fileName);

            editor.Show(_dockPanel, DockState.Document);
        }

        private void _dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            bool haveEditor = ActiveEditor != null && _debugger == null;

            _fileSave.Enabled = haveEditor;
            _fileSaveAs.Enabled = haveEditor;
            _fileClose.Enabled = haveEditor;
            _windowNextTab.Enabled = haveEditor;
            _windowPreviousTab.Enabled = haveEditor;

            if (_debugger == null)
            {
                _run.Enabled = _debugRun.Enabled = haveEditor;
                _break.Enabled = _debugBreak.Enabled = false;
                _stop.Enabled = _debugStop.Enabled = false;
                _stepInto.Enabled = _debugStepInto.Enabled = haveEditor;
                _stepOver.Enabled = _debugStepOver.Enabled = false;
                _stepOut.Enabled = _debugStepOut.Enabled = false;
            }
            else
            {
                _run.Enabled = _debugRun.Enabled = !_debugger.IsRunning;
                _break.Enabled = _debugBreak.Enabled = _debugger.IsRunning;
                _stop.Enabled = _debugStop.Enabled = true;
                _stepInto.Enabled = _debugStepInto.Enabled = !_debugger.IsRunning;
                _stepOver.Enabled = _debugStepOver.Enabled = !_debugger.IsRunning;
                _stepOut.Enabled = _debugStepOut.Enabled = !_debugger.IsRunning;
            }
        }

        private void _fileSave_Click(object sender, EventArgs e)
        {
            ActiveEditor.PerformSave();
        }

        private void _fileSaveAs_Click(object sender, EventArgs e)
        {
            ActiveEditor.PerformSaveAs();
        }

        private void _fileClose_Click(object sender, EventArgs e)
        {
            ActiveEditor.Close();
        }

        private void _stop_Click(object sender, EventArgs e)
        {
            SetDebugger(null);
        }

        private void ClearOutput()
        {
            _outputControl.ClearOutput();
        }

        private void ActivateNextTab(bool forward)
        {
            var documents = new List<IDockContent>(_dockPanel.Documents);

            if (documents.Count == 0)
                return;

            int activeDocumentIndex = documents.IndexOf(ActiveEditor);

            if (activeDocumentIndex == -1)
                activeDocumentIndex = 0;
            else
            {
                activeDocumentIndex += (forward ? 1 : -1);

                if (activeDocumentIndex < 0)
                    activeDocumentIndex = documents.Count - 1;
                if (activeDocumentIndex >= documents.Count)
                    activeDocumentIndex = 0;
            }

            ((DockContent)documents[activeDocumentIndex]).Show(_dockPanel);
        }

        private void _windowNextTab_Click(object sender, EventArgs e)
        {
            ActivateNextTab(true);
        }

        private void _windowPreviousTab_Click(object sender, EventArgs e)
        {
            ActivateNextTab(false);
        }

        public void SetLineColumn(int? line, int? column, int? chars)
        {
            _statusLine.Text = line?.ToString();
            _statusCol.Text = column?.ToString();
            _statusCh.Text = chars?.ToString();
		}

        public void SetStatus(string status)
        {
            _statusLabel.Text = status?.Replace("&", "&&");
            _statusStrip.Update();
        }

        private void _fileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _viewLocals_Click(object sender, EventArgs e)
        {
            _localsControl.Show();
        }

        private void _viewGlobals_Click(object sender, EventArgs e)
        {
            _globalsControl.Show();
        }

        private void _viewCallStack_Click(object sender, EventArgs e)
        {
            _callStackControl.Close();
        }

        private void JavaScriptForm_Shown(object sender, EventArgs e)
        {
            foreach (string fileName in Environment.GetCommandLineArgs().Skip(1).Where(p => p.EndsWith(".js") && File.Exists(p)))
            {
                OpenEditor(fileName);
            }
        }

        private void _break_Click(object sender, EventArgs e)
        {
            _debugger.Break();
        }

        private void _run_Click(object sender, EventArgs e)
        {
            if (_debugger == null)
                StartDebugger(false);
            else
                _debugger.Continue();
        }

        private void _stepInto_Click(object sender, EventArgs e)
        {
            if (_debugger == null)
                StartDebugger(true);
            else
                _debugger.Step(StepType.Into);
        }

        private void _stepOver_Click(object sender, EventArgs e)
        {
            _debugger.Step(StepType.Over);
        }

        private void StartDebugger(bool startBreaked)
        {
            foreach (var control in _dockPanel.Documents.OfType<EditorControl>())
            {
                control.PerformSave();
            }

            ClearOutput();

            var debugger = new JintDebugger();

            SetDebugger(debugger);

            debugger.Run(
                ActiveEditor.FileName,
                ActiveEditor.GetText(),
                p => SetupEngine(p),
                startBreaked
            );
        }

        private void SetupEngine(JintEngine engine)
        {
            engine.SetParameter("api", new Api(_api.Credentials));

            string resourceName = GetType().Namespace + ".Lib.js";

            using (var stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                engine.Run(reader.ReadToEnd());
            }
        }

        private void _stepOut_Click(object sender, EventArgs e)
        {
            _debugger.Step(StepType.Out);
        }

        private void LoadTabs(DebugInformation debug)
        {
            _localsControl.LoadVariables(debug, VariablesMode.Locals);
            _globalsControl.LoadVariables(debug, VariablesMode.Globals);
            _callStackControl.LoadCallStack(debug);
        }

        public void ReloadAllBreakPoints()
        {
            foreach (var control in _controls.Values)
            {
                control.LoadBreakPoints();
            }
        }
    }
}
