using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SystemEx;
using SystemEx.Windows.Forms;
using WastedgeApi;
using TextBox = System.Windows.Forms.TextBox;

namespace WastedgeQuerier
{
    public partial class FilterControl : UserControl
    {
        private static readonly FilterType[] BoolFilterTypes = { FilterType.IsTrue, FilterType.NotIsTrue, FilterType.IsFalse, FilterType.NotIsFalse, FilterType.IsNull, FilterType.NotIsNull };
        private static readonly FilterType[] TextFilterTypes = { FilterType.Equal, FilterType.NotEqual, FilterType.GreaterThan, FilterType.GreaterEqual, FilterType.LessThan, FilterType.LessEqual, FilterType.Like, FilterType.NotLike, FilterType.IsNull, FilterType.NotIsNull };
        private static readonly FilterType[] OtherFilterTypes = { FilterType.Equal, FilterType.NotEqual, FilterType.GreaterThan, FilterType.GreaterEqual, FilterType.LessThan, FilterType.LessEqual, FilterType.IsNull, FilterType.NotIsNull };

        private FilterType _filterType;
        private Control _control;
        private int _filterWidth;

        public EntityPhysicalField Field { get; }

        public FilterType FilterType
        {
            get { return _filterType; }
            set
            {
                if (_filterType != value)
                {
                    _filterType = value;

                    _filter.Text = GetFilterText(_filterType);

                    if (_control != null)
                        _control.Visible = RequiresEditor(_filterType);
                }
            }
        }

        public FilterControl(EntityPhysicalField field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            Field = field;

            InitializeComponent();

            _name.Text = Field.Name;

            foreach (var control in new Control[] { _textBox, _date, _dateTime, _numericTextBox })
            {
                control.Visible = false;
            }

            FilterType[] filterTypes;

            switch (field.DataType)
            {
                case EntityDataType.String:
                    _control = _textBox;
                    filterTypes = TextFilterTypes;
                    break;

                case EntityDataType.Date:
                    _control = _date;
                    filterTypes = OtherFilterTypes;
                    break;

                case EntityDataType.DateTime:
                case EntityDataType.DateTimeTz:
                    _control = _dateTime;
                    filterTypes = OtherFilterTypes;
                    break;

                case EntityDataType.Decimal:
                    _control = _numericTextBox;
                    filterTypes = OtherFilterTypes;
                    break;

                case EntityDataType.Long:
                case EntityDataType.Int:
                    _control = _numericTextBox;
                    _numericTextBox.NumberScale = 0;
                    filterTypes = OtherFilterTypes;
                    break;

                case EntityDataType.Bool:
                    filterTypes = BoolFilterTypes;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var filterType in filterTypes)
            {
                var menuItem = new MenuItem
                {
                    Text = GetFilterText(filterType),
                    Tag = filterType
                };

                menuItem.Click += FilterSelected;

                _contextMenu.MenuItems.Add(menuItem);
            }

            if (_control != null)
                _control.Visible = true;

            // Force an update.
            _filterType = FilterType.NotEqual;
            FilterType = Field.DataType == EntityDataType.Bool ? FilterType.IsTrue : FilterType.Equal;
        }

        private void FilterSelected(object sender, EventArgs e)
        {
            FilterType = (FilterType)((MenuItem)sender).Tag;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            _filterWidth = 0;

            PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (_filterWidth == 0)
            {
                _filterWidth =
                    _filter.Padding.Horizontal + 2 +
                    Enum<FilterType>.GetValues()
                        .Select(p => TextRenderer.MeasureText(GetFilterText(p), Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPrefix).Width)
                        .Max();
            }

            // Calculate the fixed width.

            int fixedWidth = 0;

            // Left and right margin.
            fixedWidth += 3 + 3;
            // Space between name and filter, filter and control and control and close button.
            fixedWidth += 3 + 3 + 3;
            // Width of the filter.
            fixedWidth += _filterWidth;
            // Width of close button.
            fixedWidth += _close.Width;

            // Calculate the size of the label and the control. The label gets 1/3 and the control 2/3.

            _name.Width = (Width - fixedWidth) / 3;

            if (_control != null)
                _control.Width = Width - (fixedWidth + _name.Width);

            // Left position everyting.

            _name.Left = 3;
            _filter.Left = _name.Right + 3;
            if (_control != null)
                _control.Left = _filter.Left + _filterWidth + 3;
            _close.Left = Width - (_close.Width + 3);

            // Calculate the largest height.

            var controls = new[] { _name, _filter, _control, _close };

            int height = controls.Where(p => p != null).Select(p => p.Height).Max();

            Height = height + 6;
            _name.Height = height;

            // And update the top of everything.

            foreach (var control in controls)
            {
                if (control != null)
                    control.Top = (Height - control.Height) / 2;
            }
        }

        private string GetFilterText(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.IsNull:
                    return "NULL";
                case FilterType.NotIsNull:
                    return "NOT NULL";
                case FilterType.IsTrue:
                    return "TRUE";
                case FilterType.NotIsTrue:
                    return "NOT TRUE";
                case FilterType.IsFalse:
                    return "FALSE";
                case FilterType.NotIsFalse:
                    return "NOT FALSE";
                case FilterType.In:
                    return "IN";
                case FilterType.NotIn:
                    return "NOT IN";
                case FilterType.Like:
                    return "LIKE";
                case FilterType.NotLike:
                    return "NOT LIKE";
                case FilterType.Equal:
                    return "=";
                case FilterType.NotEqual:
                    return "!=";
                case FilterType.GreaterThan:
                    return ">";
                case FilterType.GreaterEqual:
                    return ">=";
                case FilterType.LessThan:
                    return "<";
                case FilterType.LessEqual:
                    return "<=";
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }

        private bool RequiresEditor(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.IsNull:
                case FilterType.NotIsNull:
                case FilterType.IsTrue:
                case FilterType.NotIsTrue:
                case FilterType.IsFalse:
                case FilterType.NotIsFalse:
                    return false;

                case FilterType.In:
                case FilterType.NotIn:
                case FilterType.Like:
                case FilterType.NotLike:
                case FilterType.Equal:
                case FilterType.NotEqual:
                case FilterType.GreaterThan:
                case FilterType.GreaterEqual:
                case FilterType.LessThan:
                case FilterType.LessEqual:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }

        private void _close_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void _filter_Click(object sender, EventArgs e)
        {
            _contextMenu.Show(_filter, new Point(0, _filter.Height));
        }

        public Filter GetFilter()
        {
            object value;
            if (_control == null)
                value = null;
            else if (_control is SimpleNumericTextBox)
                value = ((SimpleNumericTextBox)_control).Value;
            else if (_control is TextBox)
                value = ((TextBox)_control).Text;
            else if (_control is SystemEx.Windows.Forms.DateTimePicker)
                value = ((SystemEx.Windows.Forms.DateTimePicker)_control).Value;
            else if (_control is DateTimePickerEx)
                value = ((DateTimePickerEx)_control).SelectedDateTime;
            else
                throw new InvalidOperationException();

            return new Filter(Field, FilterType, value);
        }
    }
}
