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
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Views;
using WastedgeApi;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Util;
using Cell = SourceGrid.Cells.Cell;

namespace WastedgeQuerier
{
    public partial class ResultForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;
        private readonly EntitySchema _entity;
        private readonly List<Filter> _filters;
        private ResultSet _resultSet;
        private readonly IView _defaultView;
        private readonly IView _numberView;
        private readonly IView _nullView;
        private readonly List<ResultSet> _resultSets = new List<ResultSet>();
        private ApiQuery _query;

        public ResultForm(Api api, EntitySchema entity, List<Filter> filters)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            _api = api;
            _entity = entity;
            _filters = filters;

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

            _grid.ColumnsCount = _entity.Members.Count;
            _grid.FixedRows = 1;

            _grid.Rows.Insert(0);

            for (int i = 0; i < _entity.Members.Count; i++)
            {
                _grid[0, i] = new SourceGrid.Cells.ColumnHeader(HumanText.GetMemberName(_entity.Members[i]))
                {
                    View = headerView,
                    ToolTipText = HumanText.GetMemberName(_entity.Members[i]),
                    AutomaticSortEnabled = false
                };

                _grid[0, i].AddController(toolTipController);
            }
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

            if (_grid.RowsCount > _grid.FixedRows)
            {
                _grid.AutoSizeCells(new SourceGrid.Range(
                    new SourceGrid.Position(_grid.FixedRows, 0),
                    new SourceGrid.Position(Math.Min(20, _grid.Rows.Count - _grid.FixedRows), _grid.ColumnsCount - _grid.FixedRows)
                ));
            }

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
                form.LoadingText = $"Loading {Constants.LimitPageSize} results...";

                form.Shown += async (s, ea) =>
                {
                    _query.Start = _resultSet.NextResult;

                    LoadResultSet(await _query.ExecuteReaderAsync());

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
                    string nextResult = _resultSet.NextResult;

                    _query.Count = null;

                    while (true)
                    {
                        _query.Start = nextResult;

                        if (form.IsDisposed)
                            return;

                        var resultSet = await _query.ExecuteReaderAsync();

                        count += resultSet.RowCount;

                        form.LoadingText = $"Loading {count} results...";

                        resultSets.Add(resultSet);

                        if (!resultSet.HasMore)
                            break;

                        nextResult = resultSet.NextResult;
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
                form.Filter = "Excel (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                form.RestoreDirectory = true;
                form.FileName = "Wastedge Export.xlsx";

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

        private void _editInExcel_Click(object sender, EventArgs e)
        {
            GetAllResults();

            using (var form = new EditInExcelInstructionsForm())
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;
            }

            string fileName;

            for (int i = 0;; i++)
            {
                fileName = "Wastedge Export";
                if (i > 0)
                    fileName += $" ({i})";
                fileName += ".xlsx";

                fileName = Path.Combine(Path.GetTempPath(), fileName);

                if (!File.Exists(fileName))
                    break;
            }

            try
            {
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

                using (var form = new EditInExcelWaitForm())
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                        return;
                }

                RecordSetChanges changes;

                while (true)
                {
                    try
                    {
                        changes = BuildChanges(_resultSets, fileName);
                        break;
                    }
                    catch (IOException)
                    {
                        var result = TaskDialogEx.Show(
                            this,
                            "The Excel file cannot be opened. Please close the Excel file before uploading your changes",
                            Text,
                            TaskDialogCommonButtons.OK | TaskDialogCommonButtons.Cancel,
                            TaskDialogIcon.Error
                        );
                        if (result == DialogResult.Cancel)
                            return;
                    }
                }

                using (var form = new EditInExcelUploadForm(_api, _entity, changes))
                {
                    form.ShowDialog(this);
                }

                ReloadResults();
            }
            finally
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                    // Ignore.
                }
            }
        }

        private RecordSetChanges BuildChanges(List<ResultSet> resultSets, string fileName)
        {
            var original = new RecordSet();

            foreach (var resultSet in resultSets)
            {
                original.AddResultSet(resultSet);
            }

            RecordSet modified;

            using (var stream = File.OpenRead(fileName))
            {
                modified = new ExcelImporter().Import(stream, _entity);
            }

            return RecordSetChanges.Create(original, modified);
        }

        private void ResultForm_Shown(object sender, EventArgs e)
        {
            ReloadResults();
        }

        private void ReloadResults()
        {
            if (_grid.Rows.Count > _grid.FixedRows)
                _grid.Rows.RemoveRange(_grid.FixedRows, _grid.Rows.Count - _grid.FixedRows);

            _resultSets.Clear();

            using (var form = new LoadingForm())
            {
                form.LoadingText = $"Loading {Constants.PageSize} results...";

                form.Shown += async (s, ea) =>
                {
                    _query = _api.CreateQuery(_entity);
                    _query.Filters.AddRange(_filters);
                    _query.Count = Constants.PageSize;

                    LoadResultSet(await _query.ExecuteReaderAsync());

                    form.Dispose();
                };

                form.ShowDialog(this);
            }
        }
    }
}
