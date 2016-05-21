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
    internal class ReportGridManager
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
            if (value == null)
            {
                return new SourceGrid.Cells.Cell("null")
                {
                    View = _nullView
                };
            }

            if (value is string)
            {
                return new SourceGrid.Cells.Cell(value)
                {
                    View = _defaultView
                };
            }
            if (value is decimal)
            {
                var decimalValue = (decimal)value;

                if (decimalValue % 1 == 0)
                {
                    value = (long)decimalValue;
                }
                else
                {
                    return new SourceGrid.Cells.Cell(decimalValue.ToString(CultureInfo.CurrentUICulture))
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

        public void Load(JObject data, List<ReportField> columns, List<ReportField> rows, List<ReportField> values)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            int columnDepth = columns.Count + Math.Min(values.Count, 1);

            var dataValues = (JArray)data["values"];

            _grid.FixedRows = columnDepth;
            _grid.FixedColumns = rows.Count;
            _grid.Redim(
                _grid.FixedRows + dataValues.Count,
                _grid.FixedColumns + (dataValues.Count == 0 ? 0 : ((JArray)dataValues[0]).Count)
            );

            for (int row = 0; row < _grid.FixedRows; row++)
            {
                for (int column = 0; column < _grid.FixedColumns; column++)
                {
                    _grid[row, column] = BuildHeader(null, 1, 1);
                }
            }

            BuildColumns((JArray)data["columns"]);
            BuildRows((JArray)data["rows"]);
            if (dataValues.Count > 0)
                BuildValues(dataValues);

            _grid.AutoSizeCells();
            _grid.RecalcCustomScrollBars();
        }

        private void BuildValues(JArray data)
        {
            int rows = data.Count;
            int columns = ((JArray)data[0]).Count;

            for (int row = 0; row < rows; row++)
            {
                var rowData = (JArray)data[row];

                for (int column = 0; column < columns; column++)
                {
                    _grid[row + _grid.FixedRows, column + _grid.FixedColumns] = BuildCell(
                        ((JValue)rowData[column]).Value
                    );
                }
            }
        }

        private void BuildColumns(JArray data)
        {
            int span = 0;

            for (int i = 0; i < data.Count; i++)
            {
                span += BuildColumn(data[i], 0, _grid.FixedColumns + span);
            }
        }

        private int BuildColumn(JToken data, int row, int column)
        {
            if (data.Type != JTokenType.Array)
            {
                _grid[row, column] = BuildHeader((string)data, 1, 1);
                return 1;
            }

            var subData = (JArray)data;
            int span = 0;

            for (int i = 1; i < subData.Count; i++)
            {
                span += BuildColumn(subData[i], row + 1, column + span);
            }

            if (span == 0)
                span = 1;

            _grid[row, column] = BuildHeader((string)subData[0], 1, span);

            return span;
        }

        private void BuildRows(JArray data)
        {
            int span = 0;

            for (int i = 0; i < data.Count; i++)
            {
                span += BuildRow(data[i], _grid.FixedRows + span, 0);
            }
        }

        private int BuildRow(JToken data, int row, int column)
        {
            if (data.Type != JTokenType.Array)
            {
                _grid[row, column] = BuildHeader((string)data, 1, 1);
                return 1;
            }

            var subData = (JArray)data;
            int span = 0;

            for (int i = 1; i < subData.Count; i++)
            {
                span += BuildRow(subData[i], row + span, column + 1);
            }

            if (span == 0)
                span = 1;

            _grid[row, column] = BuildHeader((string)subData[0], span, 1);

            return span;
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
