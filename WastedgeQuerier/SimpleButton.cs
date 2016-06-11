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
        private Image _grayImage;

        public Image Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    if (_grayImage != null)
                        _grayImage.Dispose();

                    _image = value;
                    _grayImage = value == null ? null : ImageUtil.ConvertToGrayscale(value);

                    Size = GetPreferredSize(Size.Empty);
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

            Size = GetPreferredSize(Size.Empty);
            Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            Size = GetPreferredSize(Size.Empty);
            Invalidate();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            var size = GetPreferredSize(Size.Empty);

            width = size.Width + Padding.Horizontal + 2;
            height = size.Height + Padding.Vertical + 2;

            base.SetBoundsCore(x, y, width, height, specified);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (_image != null)
                return _image.Size;

            return TextRenderer.MeasureText(Text.Length > 0 ? Text : "W", Font, new Size(int.MaxValue, int.MaxValue), TextFlags);
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

            if (!Enabled)
                return;

            SetState(State.Down);

            Capture = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!Enabled)
                return;

            SetState(State.Over);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!Enabled)
                return;

            SetState(State.Normal);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!Enabled)
                return;

            if (Capture)
                SetState(ClientRectangle.Contains(e.Location) ? State.Down : State.Normal);
            else
                SetState(State.Over);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (!Enabled)
                return;

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
                e.Graphics.DrawImage(Enabled ? _image : _grayImage, Padding.Left + 1, Padding.Top + 1);
            if (Text.Length > 0)
            {
                var color = Enabled ? SystemColors.ControlText : SystemColors.GrayText;
                TextRenderer.DrawText(e.Graphics, Text, Font, new Point(Padding.Left + 1, Padding.Top + 1), color, TextFlags);
            }

            if (Focused)
            {
                var rect = ClientRectangle;
                rect.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(e.Graphics, rect);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (!Enabled)
                SetState(State.Normal);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!Enabled)
                return;

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
