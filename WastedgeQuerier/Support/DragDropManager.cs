using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using Control = System.Windows.Forms.Control;

namespace WastedgeQuerier.Support
{
    internal class DragDropManager : IDisposable
    {
        private bool _disposed;
        private readonly Control _control;

        public event FilesDragEventHandler DragEnter;

        public event FilesDragEventHandler DragOver;

        public event EventHandler DragLeave;

        public event FilesDragEventHandler DragDrop;

        public DragDropManager(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            _control = control;
            _control.AllowDrop = true;

            _control.DragEnter += _control_DragEnter;
            _control.DragOver += _control_DragOver;
            _control.DragLeave += _control_DragLeave;
            _control.DragDrop += _control_DragDrop;
        }

        void _control_DragDrop(object sender, DragEventArgs e)
        {
            var form = _control.FindForm();

            form.Activate();

            var dropData = new FilesDropData(e.Data);

            if (dropData.HasFiles)
                OnDragDrop(new FilesDragEventArgs(dropData, new Point(e.X, e.Y), e.AllowedEffect, e.KeyState));
        }

        void _control_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterOver(e, DragType.Enter);
        }

        void _control_DragOver(object sender, DragEventArgs e)
        {
            DragEnterOver(e, DragType.Over);
        }

        void _control_DragLeave(object sender, EventArgs e)
        {
            OnDragLeave(EventArgs.Empty);
        }

        private void DragEnterOver(DragEventArgs e, DragType type)
        {
            var dropData = new FilesDropData(e.Data);

            if (dropData.HasFiles)
            {
                var filesEventArgs = new FilesDragEventArgs(dropData, new Point(e.X, e.Y), e.AllowedEffect, e.KeyState);
                filesEventArgs.Effect = e.Effect;

                switch (type)
                {
                    case DragType.Enter:
                        OnDragEnter(filesEventArgs);
                        break;
                    case DragType.Over:
                        OnDragOver(filesEventArgs);
                        break;
                }

                e.Effect = filesEventArgs.Effect;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _control.AllowDrop = false;

                _control.DragEnter -= _control_DragOver;
                _control.DragDrop -= _control_DragDrop;

                _disposed = true;
            }
        }

        protected virtual void OnDragEnter(FilesDragEventArgs e)
        {
            DragEnter?.Invoke(this, e);
        }

        protected virtual void OnDragDrop(FilesDragEventArgs e)
        {
            DragDrop?.Invoke(this, e);
        }

        protected virtual void OnDragOver(FilesDragEventArgs e)
        {
            DragOver?.Invoke(this, e);
        }

        protected virtual void OnDragLeave(EventArgs e)
        {
            DragLeave?.Invoke(this, e);
        }

        private enum DragType
        {
            Enter,
            Over
        }
    }
}
