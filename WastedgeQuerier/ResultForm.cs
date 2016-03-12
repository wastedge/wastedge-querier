using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Views;
using WastedgeApi;
using Cell = SourceGrid.Cells.Cell;

namespace WastedgeQuerier
{
    public partial class ResultForm : SystemEx.Windows.Forms.Form
    {
        private ResultSet _resultSet;
        private readonly IView _defaultView;
        private readonly IView _numberView;
        private readonly IView _nullView;
        private readonly List<ResultSet> _resultSets = new List<ResultSet>();

        public ResultForm(ResultSet resultSet)
        {
            if (resultSet == null)
                throw new ArgumentNullException(nameof(resultSet));

            InitializeComponent();

            var headerView = new SourceGrid.Cells.Views.ColumnHeader
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer()
            };

            _defaultView = new SourceGrid.Cells.Views.Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer()
            };

            _numberView = new SourceGrid.Cells.Views.Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer(),
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight
            };

            _nullView = new SourceGrid.Cells.Views.Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer(),
                ForeColor = SystemColors.GrayText,
                Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Italic)
            };

            var toolTipController = new ToolTipText();

            _grid.ColumnsCount = resultSet.FieldCount;
            _grid.FixedRows = 1;

            _grid.Rows.Insert(0);

            for (int i = 0; i < resultSet.FieldCount; i++)
            {
                _grid[0, i] = new SourceGrid.Cells.ColumnHeader(resultSet.GetFieldName(i))
                {
                    View = headerView,
                    ToolTipText = resultSet.GetFieldName(i),
                    AutomaticSortEnabled = false
                };

                _grid[0, i].AddController(toolTipController);
            }

            LoadResultSet(resultSet);
        }

        private void LoadResultSet(ResultSet resultSet)
        {
            _resultSet = resultSet;
            _resultSets.Add(resultSet);

            int row = _grid.Rows.Count - _grid.FixedRows;

            while (resultSet.Next())
            {
                _grid.Rows.Insert(++row);

                for (int i = 0; i < resultSet.FieldCount; i++)
                {
                    _grid[row, i] = BuildCell(resultSet[i], resultSet.GetField(i).DataType);
                }
            }

            _grid.AutoSizeCells(new SourceGrid.Range(
                new SourceGrid.Position(1, 0),
                new SourceGrid.Position(Math.Min(20, _grid.Rows.Count - 1), _grid.ColumnsCount - 1)
            ));

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _getMoreResults.Enabled = _getAllResults.Enabled = _resultSet.HasMore;
        }

        private ICell BuildCell(object value, EntityDataType dataType)
        {
            if (value is DateTimeOffset)
                value = ((DateTimeOffset)value).LocalDateTime;

            if (value is DateTime)
            {
                switch (dataType)
                {
                    case EntityDataType.Date:
                        value = ((DateTime)value).ToShortDateString();
                        break;

                    case EntityDataType.DateTime:
                    case EntityDataType.DateTimeTz:
                        value = ((DateTime)value).ToShortDateString() + " " + ((DateTime)value).ToLongTimeString();
                        break;
                }
            }
            if (value == null)
            {
                return new Cell("null")
                {
                    View = _nullView
                };
            }

            if (value is string)
            {
                return new Cell(value)
                {
                    View = _defaultView
                };
            }
            if (value is decimal)
            {
                return new Cell(((decimal)value).ToString(CultureInfo.CurrentUICulture))
                {
                    View = _numberView
                };
            }
            if (value is long)
            {
                return new Cell(((long)value).ToString(CultureInfo.CurrentUICulture))
                {
                    View = _numberView
                };
            }
            if (value is bool)
                return new SourceGrid.Cells.CheckBox(null, (bool)value);

            throw new InvalidOperationException();
        }

        private void _getMoreResults_Click(object sender, EventArgs e)
        {
            using (var form = new LoadingForm())
            {
                form.LoadingText = $"Loading {Constants.PageSize} results...";

                form.Shown += async (s, ea) =>
                {
                    LoadResultSet(await _resultSet.GetNextAsync(Constants.PageSize));

                    form.Dispose();
                };

                form.ShowDialog(this);
            }
        }

        private void _getAllResults_Click(object sender, EventArgs e)
        {
            GetAllResults();
        }

        private void GetAllResults()
        {
            if (!_resultSet.HasMore)
                return;

            using (var form = new LoadingForm())
            {
                form.LoadingText = "Loading results...";

                form.Shown += async (s, ea) =>
                {
                    int count = 0;
                    var resultSets = new List<ResultSet>();

                    while (true)
                    {
                        var resultSet = await _resultSet.GetNextAsync();

                        count += resultSet.RowCount;

                        form.LoadingText = $"Loading {count} results...";

                        resultSets.Add(resultSet);

                        if (!resultSet.HasMore)
                            break;
                    }

                    foreach (var resultSet in resultSets)
                    {
                        LoadResultSet(resultSet);
                    }

                    form.Dispose();
                };

                form.ShowDialog(this);
            }
        }

        private void _exportToExcel_Click(object sender, EventArgs e)
        {
            GetAllResults();

            string fileName;

            using (var form = new SaveFileDialog())
            {
                form.Filter = "Excel (*.xls)|*.xls|All Files (*.*)|*.*";
                form.RestoreDirectory = true;
                form.FileName = "Wastedge Export.xls";

                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = form.FileName;
            }

            using (var stream = File.Create(fileName))
            {
                new ExcelExporter().Export(stream, _resultSets);
            }

            try
            {
                Process.Start(fileName);
            }
            catch
            {
                // Ignore exceptions.
            }
        }
    }
}
