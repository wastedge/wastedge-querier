using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WastedgeApi;

namespace WastedgeQuerier.Report
{
    internal class ReportFieldListBox : DragDropListBox
    {
        private ReportFieldTransform _draggingOldTransform;
        private ReportField _draggingField;

        public ReportFieldType FieldType { get; set; }

        [DefaultValue(true)]
        public override bool AllowDrop { get; set; }

        public ReportFieldListBox()
        {
            AllowDrop = true;
        }

        protected override void OnItemDrag(ListBoxItemEventArgs e)
        {
            var field = (ReportField)Items[e.Index];
            Items.RemoveAt(e.Index);

            if (DoDragDrop(field, DragDropEffects.Move) == DragDropEffects.None)
                Items.Insert(e.Index, field);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            var field = (ReportField)e.Data.GetData(typeof(ReportField));
            if (field == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = e.AllowedEffect & (DragDropEffects.Copy | DragDropEffects.Move);

            // Calculate the index where the item should be.

            var location = PointToClient(new Point(e.X, e.Y));
            int targetIndex = TargetIndexFromPoint(location);
            int currentIndex = Items.IndexOf(field);
            if (currentIndex == targetIndex)
                return;


            BeginUpdate();
            int topIndex = TopIndex;
            SetDraggingField(field);
            Items.Insert(Math.Max(Math.Min(targetIndex, Items.Count), 0), field);
            TopIndex = topIndex;
            EndUpdate();
        }

        private void SetDraggingField(ReportField field)
        {
            if (_draggingField != null)
                Items.Remove(_draggingField);

            if (_draggingField == field)
                return;

            _draggingField = field;
            _draggingOldTransform = _draggingField.Transform;

            ForceFieldTransform(_draggingField);
        }

        public void ForceFieldTransform(ReportField field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (FieldType != ReportFieldType.Value)
            {
                field.Transform = ReportFieldTransform.None;
            }
            else if (field.Transform == ReportFieldTransform.None)
            {
                var dataType = (field.Fields.Tail as EntityTypedField)?.DataType;

                switch (dataType.GetValueOrDefault(EntityDataType.String))
                {
                    case EntityDataType.Decimal:
                    case EntityDataType.Long:
                    case EntityDataType.Int:
                        field.Transform = ReportFieldTransform.Sum;
                        break;

                    default:
                        field.Transform = ReportFieldTransform.Count;
                        break;
                }
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);

            if (_draggingField != null)
            {
                Items.Remove(_draggingField);
                _draggingField.Transform = _draggingOldTransform;
                _draggingField = null;
            }
        }

        private int TargetIndexFromPoint(Point location)
        {
            return Math.Max(Math.Min(location.Y / ItemHeight + TopIndex, Items.Count), 0);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            _draggingField = null;
        }

        public void UpdateLabels()
        {
            BeginUpdate();
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] = Items[i];
            }
            EndUpdate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            SelectedIndex = -1;
        }
    }
}
