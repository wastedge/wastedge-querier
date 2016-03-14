using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Jint;
using Jint.Debugger;
using WastedgeApi;
using WeifenLuo.WinFormsUI.Docking;

namespace WastedgeQuerier.JavaScript
{
    internal partial class EditorControl : DockContent
    {
        private readonly IStatusBarProvider _statusBarProvider;
        private string _tabText;

        public string FileName { get; private set; }
        public bool IsDirty { get; private set; }

        public EditorControl(IStatusBarProvider statusBarProvider)
        {
            if (statusBarProvider == null)
                throw new ArgumentNullException(nameof(statusBarProvider));

            _statusBarProvider = statusBarProvider;

            Font = SystemFonts.MessageBoxFont;

            InitializeComponent();

            _textEditor.SetHighlighting("JavaScript");

            const string consolas = "Consolas";

            var font = new Font(consolas, _textEditor.Font.Size);

            if (font.FontFamily.Name == consolas)
                _textEditor.Font = font;

            _textEditor.ActiveTextAreaControl.Caret.PositionChanged += Caret_PositionChanged;

            SetTabText("New File");
        }

        void Caret_PositionChanged(object sender, EventArgs e)
        {
            UpdateLineCol();
        }

        private void SetTabText(string tabText)
        {
            _tabText = tabText;

            if (IsDirty)
                tabText += "*";

            Text = TabText = tabText;
        }

        internal void Open(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            FileName = fileName;

            SetTabText(Path.GetFileName(fileName));

            _textEditor.Text = File.ReadAllText(fileName);

            SetDirty(false);
        }

        private void SetDirty(bool isDirty)
        {
            if (IsDirty != isDirty)
            {
                IsDirty = isDirty;
                SetTabText(_tabText);
            }
        }

        private void _textEditor_TextChanged(object sender, EventArgs e)
        {
            SetDirty(true);
        }

        internal void Save(string fileName)
        {
            if (fileName != null)
            {
                FileName = fileName;
                SetTabText(Path.GetFileName(fileName));
            }

            File.WriteAllText(FileName, _textEditor.Text);
            SetDirty(false);
        }

        public string GetText()
        {
            return _textEditor.Text;
        }

        public bool PerformSave()
        {
            if (FileName == null)
                return PerformSaveAs();

            Save(null);

            return true;
        }

        public bool PerformSaveAs()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.AddExtension = true;
                dialog.CheckPathExists = true;
                dialog.Filter = JavaScriptForm.FileDialogFilter;
                dialog.OverwritePrompt = true;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Save(dialog.FileName);
                    return true;
                }
            }

            return false;
        }

        private void EditorControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            e.Cancel = !PerformSaveIfDirty();
        }

        private bool PerformSaveIfDirty()
        {
            if (IsDirty)
            {
                var result = MessageBox.Show(
                    this,
                    "Do you want to save your changes?",
                    Text,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!PerformSave())
                            return false;
                        break;

                    case DialogResult.Cancel:
                        return false;
                }
            }

            return true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            UpdateLineCol();
        }

        private void UpdateLineCol()
        {
            var position = _textEditor.ActiveTextAreaControl.Caret.Position;
            var line = _textEditor.Document.GetLineSegment(position.Line);
            int chars = _textEditor.Document.GetText(line).Substring(0, position.Column).Replace("\t", "    ").Length;

            _statusBarProvider.SetLineColumn(
                position.Line + 1, chars + 1, position.Column + 1
            );
        }
    }
}
