using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Jint;
using Jint.Debugger;
using Jint.Native;
using WeifenLuo.WinFormsUI.Docking;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal partial class ProgramControl : DockContent
    {
        private readonly JavaScriptForm _form;
        private CaretMark _caretMark;
        private JintEngine _engine;
        private readonly IStatusBarProvider _statusBarProvider;

        public ProgramControl(Jint.Expressions.Program program, JavaScriptForm form, IStatusBarProvider statusBarProvider)
        {
            if (program == null)
                throw new ArgumentNullException(nameof(program));
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (statusBarProvider == null)
                throw new ArgumentNullException(nameof(statusBarProvider));

            _statusBarProvider = statusBarProvider;
            _form = form;

            InitializeComponent();

            _textEditor.SetHighlighting("JavaScript");
            _textEditor.IsReadOnly = true;

            _textEditor.Text = program.ProgramSource;

            const string consolas = "Consolas";

            var font = new Font(consolas, _textEditor.Font.Size);

            if (font.FontFamily.Name == consolas)
                _textEditor.Font = font;

            _textEditor.ActiveTextAreaControl.TextArea.IconBarMargin.MouseDown += IconBarMargin_MouseDown;

            _textEditor.ActiveTextAreaControl.Caret.PositionChanged += Caret_PositionChanged;
        }

        void IconBarMargin_MouseDown(AbstractMargin sender, Point mousepos, MouseButtons mouseButtons)
        {
            if (mouseButtons != MouseButtons.Left)
                return;

            var textArea = _textEditor.ActiveTextAreaControl.TextArea;

            int yPos = mousepos.Y;
            int lineHeight = textArea.TextView.FontHeight;
            int lineNumber = (yPos + textArea.VirtualTop.Y) / lineHeight;

            foreach (var breakPoint in _engine.BreakPoints)
            {
                if (breakPoint.Line == lineNumber + 1)
                    return;
            }

            string text = textArea.Document.GetText(textArea.Document.GetLineSegment(lineNumber));
            int offset = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (
                    !Char.IsWhiteSpace(text[i]) &&
                    text[i] != '/'
                ) {
                    offset = i;
                    break;
                }
            }

            if (offset != -1)
            {
                _engine.BreakPoints.Add(new BreakPoint(lineNumber + 1, offset));

                _form.ReloadAllBreakPoints();

                _textEditor.Refresh();
            }
        }

        public void ProcessStep(JintEngine engine, DebugInformation e, BreakType breakType)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (_engine != engine)
            {
                _engine = engine;
                LoadBreakPoints();
            }

            var document = _textEditor.Document;

            int start = GetOffset(e.CurrentStatement.Source.Start);
            int end = GetOffset(e.CurrentStatement.Source.Stop);

            var position = document.OffsetToPosition(start);

            document.MarkerStrategy.AddMarker(new TextMarker(start, end - start, TextMarkerType.SolidBlock, Color.Yellow));
            _textEditor.ActiveTextAreaControl.TextArea.Caret.Position = position;

            _caretMark = new CaretMark(document, position);

            document.BookmarkManager.AddMark(_caretMark);

            _textEditor.Refresh();
        }

        public void LoadBreakPoints()
        {
            var document = _textEditor.Document;
            document.BookmarkManager.Clear();

            foreach (var breakPoint in _engine.BreakPoints)
            {
                if (breakPoint.Line > _textEditor.Document.TotalNumberOfLines)
                    continue;

                var mark = new BreakPointMark(
                    document,
                    new TextLocation(
                        breakPoint.Char,
                        breakPoint.Line - 1
                    )
                );

                var breakPointCopy = breakPoint;

                mark.Removed += (s, e) =>
                {
                    _engine.BreakPoints.Remove(breakPointCopy);
                    _form.ReloadAllBreakPoints();
                };

                document.BookmarkManager.AddMark(mark);
            }

            if (_caretMark != null)
                document.BookmarkManager.AddMark(_caretMark);
        }

        private int GetOffset(SourceCodeDescriptor.Location location)
        {
            return _textEditor.Document.PositionToOffset(new TextLocation(
                location.Char, location.Line - 1
            ));
        }

        public void ResetState()
        {
            _textEditor.Document.MarkerStrategy.RemoveAll(p => true);
            _textEditor.Document.BookmarkManager.RemoveMark(_caretMark);
            _caretMark = null;

            _textEditor.Refresh();
        }

        void Caret_PositionChanged(object sender, EventArgs e)
        {
            UpdateLineCol();
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
