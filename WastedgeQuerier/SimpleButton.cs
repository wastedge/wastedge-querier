using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WastedgeQuerier
{
    public class SimpleButton : Control
    {
        private const TextFormatFlags TextFlags = TextFormatFlags.NoPrefix;

        private State _state;
        private Image _image;

        public Image Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;

                    Size = Size.Empty;
                    Invalidate();
                }
            }
        }

        protected override Padding DefaultPadding => new Padding(3);

        public SimpleButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            Size = Size.Empty;
            Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            Size = Size.Empty;
            Invalidate();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            Size size;

            if (_image != null)
                size = _image.Size;
            else
                size = TextRenderer.MeasureText(Text.Length > 0 ? Text : "W", Font, new Size(int.MaxValue, int.MaxValue), TextFlags);

            width = size.Width + Padding.Horizontal + 2;
            height = size.Height + Padding.Vertical + 2;

            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void SetState(State state)
        {
            _state = state;

            Invalidate();
            Update();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SetState(State.Down);

            Capture = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            SetState(State.Over);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            SetState(State.Normal);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Capture)
                SetState(ClientRectangle.Contains(e.Location) ? State.Down : State.Normal);
            else
                SetState(State.Over);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            Capture = false;

            SetState(ClientRectangle.Contains(e.Location) ? State.Over : State.Normal);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (_state)
            {
                case State.Down:
                    e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, Width - 1, 0);
                    e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, 0, Height - 1);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, Width - 1, 0, Width - 1, Height - 1);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, Height - 1, Width - 1, Height - 1);
                    break;

                case State.Over:
                    e.Graphics.DrawLine(SystemPens.ControlDark, Width - 1, 0, Width - 1, Height - 1);
                    e.Graphics.DrawLine(SystemPens.ControlDark, 0, Height - 1, Width - 1, Height - 1);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, 0, Width - 1, 0);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, 0, 0, Height - 1);
                    break;
            }

            if (_image != null)
                e.Graphics.DrawImage(_image, Padding.Left + 1, Padding.Top + 1);
            if (Text.Length > 0)
                TextRenderer.DrawText(e.Graphics, Text, Font, new Point(Padding.Left + 1, Padding.Top + 1), SystemColors.ControlText, TextFlags);

            if (Focused)
            {
                var rect = ClientRectangle;
                rect.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(e.Graphics, rect);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Return:
                case Keys.Space:
                    OnClick(EventArgs.Empty);
                    break;
            }
        }

        private enum State
        {
            Normal,
            Over,
            Down
        }
    }
}
