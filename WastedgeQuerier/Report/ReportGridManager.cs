using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Views;
using Cell = SourceGrid.Cells.Views.Cell;

namespace WastedgeQuerier.Report
{
    internal class ReportGridManager : ReportLoader
    {
        private readonly Grid _grid;
        private readonly IView _headerView;
        private readonly IView _defaultView;
        private readonly IView _numberView;
        private readonly IView _nullView;
        private readonly ToolTipText _toolTipController;

        public ReportGridManager(Grid grid)
        {
            _grid = grid;

            _headerView = new SourceGrid.Cells.Views.ColumnHeader
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer()
            };

            _defaultView = new Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer()
            };

            _numberView = new Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer(),
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight
            };

            _nullView = new Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer(),
                ForeColor = SystemColors.GrayText,
                Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Italic)
            };

            _toolTipController = new ToolTipText();
        }

        private ICell BuildCell(object value)
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
                    View = _defaultView
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
                        View = _numberView
                    };
                }
            }
            if (value is long)
            {
                return new SourceGrid.Cells.Cell(((long)value).ToString(CultureInfo.CurrentUICulture))
                {
                    View = _numberView
                };
            }
            if (value is bool)
                return new SourceGrid.Cells.CheckBox(null, (bool)value);

            throw new InvalidOperationException();
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
            _grid.FixedRows = headerRows;
            _grid.FixedColumns = headerColumns;
            _grid.Redim(
                _grid.FixedRows + rows,
                _grid.FixedColumns + columns
            );

            for (int row = 0; row < _grid.FixedRows; row++)
            {
                for (int column = 0; column < _grid.FixedColumns; column++)
                {
                    _grid[row, column] = BuildHeader(null, 1, 1);
                }
            }
        }

        protected override void SetCell(int row, int column, object value)
        {
            _grid[row + _grid.FixedRows, column + _grid.FixedColumns] = BuildCell(value);
        }

        protected override void SetHeader(int row, int column, string data, int rowSpan, int columnSpan)
        {
            _grid[row, column] = BuildHeader(data ?? "(blank)", rowSpan, columnSpan);
        }

        private ICell BuildHeader(string value, int rowSpan, int columnSpan)
        {
            var result = new SourceGrid.Cells.ColumnHeader(value)
            {
                View = _headerView,
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
    }
}
