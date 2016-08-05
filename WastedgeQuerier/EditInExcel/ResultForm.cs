﻿using System;
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
using WastedgeQuerier.Export;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Util;
using Cell = SourceGrid.Cells.Cell;

namespace WastedgeQuerier.EditInExcel
{
    public partial class ResultForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;
        private readonly EntitySchema _entity;
        private List<Filter> _filters;
        private ResultSet _resultSet;
        private readonly IView _defaultView;
        private readonly IView _numberView;
        private readonly List<ResultSet> _resultSets = new List<ResultSet>();
        private ApiQuery _query;
        private int[] _columnMap;

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

            _editInExcel.Enabled = entity.CanCreate || entity.CanUpdate || entity.CanDelete;

            _defaultView = new SourceGrid.Cells.Views.Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer()
            };

            _numberView = new SourceGrid.Cells.Views.Cell
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer(),
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight
            };

            UpdateEnabled();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_grid.ContainsFocus && _grid.RowsCount > _grid.FixedRows)
            {
                switch (keyData)
                {
                    case Keys.Home:
                        _grid.Selection.FocusRow(1);
                        return true;

                    case Keys.End:
                        _grid.Selection.FocusRow(_grid.RowsCount - 1);
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadResultSet(ResultSet resultSet)
        {
            if (IsDisposed)
                return;

            if (_grid.RowsCount == 0)
                CreateHeaders(resultSet);

            _resultSet = resultSet;
            _resultSets.Add(resultSet);

            int row = _grid.Rows.Count - _grid.FixedRows;

            while (resultSet.Next())
            {
                _grid.Rows.Insert(++row);

                for (int i = 0; i < resultSet.FieldCount; i++)
                {
                    _grid[row, _columnMap[i]] = BuildCell(resultSet[i], resultSet.GetField(i).DataType);
                }
            }

            if (_grid.RowsCount > _grid.FixedRows)
            {
                _grid.AutoSizeCells(new SourceGrid.Range(
                    new SourceGrid.Position(_grid.FixedRows, 0),
                    new SourceGrid.Position(Math.Min(20, _grid.Rows.Count - _grid.FixedRows), _grid.ColumnsCount - 1)
                ));

                for (int i = 0; i < _grid.ColumnsCount; i++)
                {
                    if (_grid.Columns[i].Width < 60)
                        _grid.Columns[i].Width = 60;
                }
            }

            UpdateEnabled();
        }

        private void CreateHeaders(ResultSet resultSet)
        {
            var headerView = new SourceGrid.Cells.Views.ColumnHeader
            {
                ElementText = new DevAge.Drawing.VisualElements.TextRenderer()
            };

            var toolTipController = new ToolTipText();

            _grid.ColumnsCount = _entity.Members.Count;
            _grid.FixedRows = 1;

            _grid.Rows.Insert(0);

            _columnMap = ApiUtils.BuildColumnMap(resultSet);

            for (int i = 0; i < resultSet.FieldCount; i++)
            {
                var member = _entity.Members[resultSet.GetFieldName(i)];

                _grid[0, _columnMap[i]] = new SourceGrid.Cells.ColumnHeader(HumanText.GetMemberName(member))
                {
                    View = headerView,
                    ToolTipText = HumanText.GetMemberName(member),
                    AutomaticSortEnabled = false
                };

                _grid[0, _columnMap[i]].AddController(toolTipController);
            }
        }

        private void UpdateEnabled()
        {
            _getMoreResults.Enabled = _getAllResults.Enabled = _resultSet == null || _resultSet.HasMore;
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
                return new Cell
                {
                    View = _defaultView
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

        private void _editFilters_Click(object sender, EventArgs e)
        {
            using (var form = new EntityFiltersForm(_entity, _filters))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _filters = form.GetFilters();
                    ReloadResults();
                }
            }
        }

        private void _getMoreResults_Click(object sender, EventArgs e)
        {
            LoadingForm.Show(this, async p =>
            {
                p.LoadingText = $"Loading {Constants.LimitPageSize} results...";

                _query.Start = _resultSet?.NextResult;

                LoadResultSet(await _query.ExecuteReaderAsync());
            });
        }

        private void _getAllResults_Click(object sender, EventArgs e)
        {
            GetAllResults();
        }

        private void GetAllResults()
        {
            if (_resultSet != null && !_resultSet.HasMore)
                return;

            LoadingForm.Show(this, async p =>
            {
                p.LoadingText = "Loading results...";

                int count = 0;
                var resultSets = new List<ResultSet>();
                string nextResult = _resultSet?.NextResult;

                _query.Count = null;

                while (true)
                {
                    _query.Start = nextResult;

                    if (p.IsDisposed)
                        return;

                    var resultSet = await _query.ExecuteReaderAsync();

                    count += resultSet.RowCount;

                    p.LoadingText = $"Loading {count} results...";

                    resultSets.Add(resultSet);

                    if (!resultSet.HasMore)
                        break;

                    nextResult = resultSet.NextResult;
                }

                foreach (var resultSet in resultSets)
                {
                    LoadResultSet(resultSet);
                }
            });
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

            LoadingForm.Show(this, async p =>
            {
                p.LoadingText = $"Loading {Constants.PageSize} results...";

                _query = _api.CreateQuery(_entity);
                _query.Filters.AddRange(_filters);
                _query.Count = Constants.PageSize;

                LoadResultSet(await _query.ExecuteReaderAsync());
            });
        }
    }
}
