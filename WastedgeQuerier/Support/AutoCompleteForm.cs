using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WastedgeQuerier.Util;

namespace WastedgeQuerier.Support
{
    internal partial class AutoCompleteForm : Form
    {
        public const int DefaultHeight = 240;

        public AutoCompleteFormMode Mode { get; set; }

        internal AutoCompleteForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Opaque | ControlStyles.Selectable, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            MouseActivated = false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new Control Owner { get; set; }

        public bool MouseActivated { get; private set; }

        public event CmdKeyEventHandler CmdKey;

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;

                createParams.Style = unchecked((int)(NativeMethods.WS_POPUP | NativeMethods.WS_CLIPSIBLINGS | NativeMethods.WS_CLIPCHILDREN | NativeMethods.WS_BORDER));
                createParams.ExStyle = unchecked((int)(NativeMethods.WS_EX_LEFT | NativeMethods.WS_EX_LTRREADING | NativeMethods.WS_EX_TOPMOST | NativeMethods.WS_EX_NOPARENTNOTIFY | NativeMethods.WS_EX_TOOLWINDOW));

                return createParams;
            }
        }

        internal new void Show()
        {
            // Force creation of the underlying window.
            var handle = Handle;

            var borderSize = new Size(
                (Owner.Width - Owner.ClientRectangle.Width) / 2,
                (Owner.Height - Owner.ClientRectangle.Height) / 2
            );

            var point = new Point(-borderSize.Width, -borderSize.Height);

            NativeMethods.ClientToScreen(Owner.Handle, ref point);

            var screen = Screen.FromHandle(Owner.Handle);

            int bottom = point.Y + Owner.Height + DefaultHeight;

            bool atTop = bottom > screen.Bounds.Bottom;

            if (atTop)
                Bounds = new Rectangle(point.X, point.Y - DefaultHeight, Owner.Width, DefaultHeight);
            else
                Bounds = new Rectangle(point.X, point.Y + Owner.Height, Owner.Width, DefaultHeight);

            if (Mode == AutoCompleteFormMode.NoFocus)
                NativeMethods.ShowWindow(Handle, NativeMethods.SW_SHOWNOACTIVATE);
            else
                NativeMethods.ShowWindow(Handle, NativeMethods.SW_SHOW);
        }

        protected override void WndProc(ref Message m)
        {
            if (Mode == AutoCompleteFormMode.NoFocus && m.Msg == NativeMethods.WM_MOUSEACTIVATE)
            {
                MouseActivated = true;
                m.Result = (IntPtr)NativeMethods.MA_NOACTIVATEANDEAT;
                return;
            }

            base.WndProc(ref m);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var e = new CmdKeyEventArgs(keyData);
            OnCmdKey(e);
            if (e.Handled)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected virtual void OnCmdKey(CmdKeyEventArgs e)
        {
            CmdKey?.Invoke(this, e);
        }
    }
}
