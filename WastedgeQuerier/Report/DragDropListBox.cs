using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.Report
{
    internal class DragDropListBox : ListBox
    {
        private int _dragIndex;
        private Point? _dragStart;
        private MouseButtons _dragButton;

        public event ListBoxItemEventHandler ItemDrag;

        public event ListBoxItemEventHandler ItemClick;

        protected virtual void OnItemDrag(ListBoxItemEventArgs e)
        {
            ItemDrag?.Invoke(this, e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _dragIndex = IndexFromPoint(e.Location);
            if (_dragIndex == -1)
                return;

            _dragStart = e.Location;
            _dragButton = e.Button;

            Capture = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!Capture || !_dragStart.HasValue)
                return;

            int dx = Math.Abs(e.X - _dragStart.Value.X);
            int dy = Math.Abs(e.Y - _dragStart.Value.Y);

            var dragSize = SystemInformation.DragSize;

            if (dx >= dragSize.Width || dy >= dragSize.Height)
            {
                Capture = false;
                OnItemDrag(new ListBoxItemEventArgs(_dragButton, Math.Max(Math.Min(_dragIndex, Items.Count - 1), 0)));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_dragStart.HasValue)
            {
                Capture = false;
                _dragStart = null;
                if (_dragIndex != -1)
                    OnItemClick(new ListBoxItemEventArgs(_dragButton, _dragIndex));
            }
        }

        protected virtual void OnItemClick(ListBoxItemEventArgs e)
        {
            ItemClick?.Invoke(this, e);
        }
    }
}
