using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevAge.Drawing;
using DevAge.Drawing.VisualElements;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Views;
using Cell = SourceGrid.Cells.Views.Cell;
using ContentAlignment = DevAge.Drawing.ContentAlignment;

namespace WastedgeQuerier.Report
{
    internal class ReportGridManager : ReportLoader
    {
        private static readonly Color HeaderBackgroundColor = Color.FromArgb(221, 235, 247);
        private static readonly Color HeaderBorderColor = Color.FromArgb(155, 194, 230);

        private readonly Grid _grid;
        private readonly IView _columnHeaderView;
        private readonly IView _defaultView;
        private readonly IView _numberView;
        private readonly ToolTipText _toolTipController;
        private readonly IView _lastColumHeaderView;
        private readonly IView _rowBreakHeaderView;
        private readonly IView _rowHeaderView;
        private readonly IView _defaultBreakView;
        private readonly IView _numberBreakView;
        private bool[] _breaks;

        public ReportGridManager(Grid grid)
        {
            _grid = grid;

            var font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);

            _columnHeaderView = BuildHeaderView(HeaderBackgroundColor, null, font);
            _lastColumHeaderView = BuildHeaderView(HeaderBackgroundColor, HeaderBorderColor, font);
            _rowBreakHeaderView = BuildHeaderView(Color.White, HeaderBorderColor, font);
            _rowHeaderView = BuildHeaderView(Color.White, null, font);
            _defaultView = BuildView(null, ContentAlignment.MiddleLeft);
            _defaultBreakView = BuildView(HeaderBorderColor, ContentAlignment.MiddleLeft);
            _numberView = BuildView(null, ContentAlignment.MiddleRight);
            _numberBreakView = BuildView(HeaderBorderColor, ContentAlignment.MiddleRight);

            _toolTipController = new ToolTipText();
        }

        private IView BuildView(Color? bottomBorderColor, ContentAlignment alignment)
        {
            return new Cell
            {
                ElementText = new TextRenderer(),
                TextAlignment = alignment,
                Border = BuildBorder(bottomBorderColor)
            };
        }

        private IView BuildHeaderView(Color backgroundColor, Color? bottomBorderColor, Font font)
        {
            return new SourceGrid.Cells.Views.ColumnHeader
            {
                ElementText = new TextRenderer(),
                Background = new FlatColumnHeaderVisual
                {
                    BackColor = backgroundColor,
                    Border = BuildBorder(bottomBorderColor)
                },
                Font = font
            };
        }

        private static RectangleBorder BuildBorder(Color? bottomBorderColor)
        {
            RectangleBorder border;

            if (bottomBorderColor.HasValue)
            {
                border = new RectangleBorder
                {
                    Bottom = new BorderLine
                    {
                        Color = bottomBorderColor.Value,
                        Width = 1
                    },
                    Left = BorderLine.NoBorder,
                    Top = BorderLine.NoBorder,
                    Right = BorderLine.NoBorder
                };
            }
            else
            {
                border = new RectangleBorder
                {
                    Bottom = BorderLine.NoBorder,
                    Left = BorderLine.NoBorder,
                    Top = BorderLine.NoBorder,
                    Right = BorderLine.NoBorder
                };
            }
            return border;
        }

        private ICell BuildCell(object value, int row)
        {
            if (value is DateTimeOffset)
                value = ((DateTimeOffset)value).LocalDateTime;

            if (value is DateTime)
            {
                value = ((DateTime)value).ToShortDateString() + " " + ((DateTime)value).ToLongTimeString();
            }
            if (value == null || value is string)
            {
                return new SourceGrid.Cells.Cell(value)
                {
                    View = GetCellView(row, _defaultView, _defaultBreakView)
                };
            }
            if (value is double)
            {
                var doubleValue = (double)value;

                if (doubleValue % 1 == 0)
                {
                    value = (long)doubleValue;
                }
                else
                {
                    return new SourceGrid.Cells.Cell(doubleValue.ToString(CultureInfo.CurrentUICulture))
                    {
                        View = GetCellView(row, _numberView, _numberBreakView)
                    };
                }
            }
            if (value is long)
            {
                return new SourceGrid.Cells.Cell(((long)value).ToString(CultureInfo.CurrentUICulture))
                {
                    View = GetCellView(row, _numberView, _numberBreakView)
                };
            }
            if (value is bool)
                return new SourceGrid.Cells.CheckBox(null, (bool)value);

            throw new InvalidOperationException();
        }

        private IView GetCellView(int row, IView view, IView breakView)
        {
            bool isBreak = _breaks[row];
            return isBreak ? breakView : view;
        }

        public void Reset()
        {
            _grid.Redim(0, 0);
        }

        public override void Load(ReportDataSet data)
        {
            base.Load(data);

            _grid.AutoSizeCells();
            _grid.RecalcCustomScrollBars();
        }

        protected override void Resize(int headerRows, int headerColumns, int rows, int columns)
        {
            _breaks = new bool[rows];

            _grid.FixedRows = headerRows;
            _grid.FixedColumns = headerColumns;
            _grid.Redim(
                _grid.FixedRows + rows,
                _grid.FixedColumns + columns
            );

            for (int row = 0; row < _grid.FixedRows; row++)
            {
                var view = row == _grid.FixedRows - 1 ? _lastColumHeaderView : _columnHeaderView;

                for (int column = 0; column < _grid.FixedColumns; column++)
                {
                    _grid[row, column] = BuildHeader(null, 1, 1, view);
                }
            }
        }

        protected override void SetCell(int row, int column, object value)
        {
            _grid[row + _grid.FixedRows, column + _grid.FixedColumns] = BuildCell(value, row);
        }

        protected override void SetHeader(int row, int column, string data, int rowSpan, int columnSpan)
        {
            bool isRow = column < _grid.FixedColumns;

            IView view;

            if (isRow)
            {
                if (column == 0)
                    _breaks[row - _grid.FixedRows + rowSpan - 1] = true;

                bool isBreak = _breaks[row - _grid.FixedRows + rowSpan - 1];

                view = isBreak ? _rowBreakHeaderView : _rowHeaderView;
            }
            else
            {
                view = row == _grid.FixedRows - 1 ? _lastColumHeaderView : _columnHeaderView;
            }

            _grid[row, column] = BuildHeader(data ?? "(blank)", rowSpan, columnSpan, view);
        }

        private ICell BuildHeader(string value, int rowSpan, int columnSpan, IView view)
        {
            var result = new SourceGrid.Cells.ColumnHeader(value)
            {
                View = view,
                ToolTipText = value,
                AutomaticSortEnabled = false
            };

            result.AddController(_toolTipController);

            if (rowSpan > 1)
                result.RowSpan = rowSpan;
            if (columnSpan > 1)
                result.ColumnSpan = columnSpan;

            return result;
        }

        private sealed class FlatHeaderVisual : HeaderBase
        {
            public FlatHeaderVisual()
            {
                _background = new BackgroundSolid(Color.Empty);

                BackColor = Color.FromKnownColor(KnownColor.Control);

                var darkdarkControl = Utilities.CalculateLightDarkColor(BackColor, -0.2f);
                var darkB = new BorderLine(darkdarkControl, 1);
                _border = new RectangleBorder(darkB, darkB);
            }

            private FlatHeaderVisual(FlatHeaderVisual other)
                : base(other)
            {
                BackColor = other.BackColor;
                Border = other.Border;
            }

            public Color BackColor { get; set; }

            private RectangleBorder _border;

            public RectangleBorder Border
            {
                get { return _border; }
                set { _border = value; }
            }

            private readonly BackgroundSolid _background;

            public override object Clone()
            {
                return new FlatHeaderVisual(this);
            }

            public override RectangleF GetBackgroundContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
            {
                backGroundArea = _border.GetContentRectangle(backGroundArea);

                return base.GetBackgroundContentRectangle(measure, backGroundArea);
            }

            public override SizeF GetBackgroundExtent(MeasureHelper measure, SizeF contentSize)
            {
                var extend = base.GetBackgroundExtent(measure, contentSize);

                extend = _border.GetExtent(contentSize);

                return extend;
            }

            protected override void OnDraw(GraphicsCache graphics, RectangleF area)
            {
                OnDrawBackground(graphics, area);
                OnDrawBorder(graphics, area);
            }

            private void OnDrawBorder(GraphicsCache graphics, RectangleF area)
            {
                _border.Draw(graphics, area);
            }

            private void OnDrawBackground(GraphicsCache graphics, RectangleF area)
            {
                var lightControl = Utilities.CalculateLightDarkColor(BackColor, 0.5f);
                var hotLightControl = Utilities.CalculateMiddleColor(Color.FromKnownColor(KnownColor.Highlight), lightControl);

                if (Style == ControlDrawStyle.Pressed)
                    _background.BackColor = hotLightControl;
                else
                    _background.BackColor = BackColor;

                _background.Draw(graphics, area);
            }

        }

        private class FlatColumnHeaderVisual : ColumnHeaderBase
        {
            public FlatColumnHeaderVisual()
            {
            }

            private FlatColumnHeaderVisual(FlatColumnHeaderVisual other)
                : base(other)
            {
                _background = (FlatHeaderVisual)other._background.Clone();
            }

            public override object Clone()
            {
                return new FlatColumnHeaderVisual(this);
            }

            public override ControlDrawStyle Style
            {
                get { return base.Style; }
                set
                {
                    base.Style = value;
                    _background.Style = value;
                }
            }

            public Color BackColor
            {
                get { return _background.BackColor; }
                set { _background.BackColor = value; }
            }

            public RectangleBorder Border
            {
                get { return _background.Border; }
                set { _background.Border = value; }
            }

            private readonly FlatHeaderVisual _background = new FlatHeaderVisual();

            protected override void OnDraw(GraphicsCache graphics, RectangleF area)
            {
                base.OnDraw(graphics, area);

                _background.Draw(graphics, area);
            }

            public override RectangleF GetBackgroundContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
            {
                backGroundArea = base.GetBackgroundContentRectangle(measure, backGroundArea);

                return _background.GetBackgroundContentRectangle(measure, backGroundArea);
            }

            public override SizeF GetBackgroundExtent(MeasureHelper measure, SizeF contentSize)
            {
                contentSize = _background.GetBackgroundExtent(measure, contentSize);

                return base.GetBackgroundExtent(measure, contentSize);
            }
        }
    }
}
