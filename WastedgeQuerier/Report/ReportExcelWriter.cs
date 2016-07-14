using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.Report
{
    internal class ReportExcelWriter : ReportLoader
    {
        private readonly Stream _stream;
        private readonly string _sheetName;
        private XSSFWorkbook _workbook;
        private ISheet _sheet;
        private ICellStyle _columnHeaderStyle;
        private ICellStyle _rowHeaderStyle;
        private ICellStyle _defaultCellStyle;
        private ICellStyle _dateStyle;
        private ICellStyle _dateTimeStyle;
        private int _headerRows;
        private int _headerColumns;

        public ReportExcelWriter(Stream stream, string sheetName)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (sheetName == null)
                throw new ArgumentNullException(nameof(sheetName));

            _stream = stream;
            _sheetName = sheetName;
        }

        public override void Load(ReportDataSet data)
        {
            _workbook = new XSSFWorkbook();
            _sheet = _workbook.CreateSheet(_sheetName);

            _columnHeaderStyle = ExcelExporter.CreateColumnHeaderStyle(_workbook);
            _rowHeaderStyle = ExcelExporter.CreateRowHeaderStyle(_workbook);
            _defaultCellStyle = ExcelExporter.CreateDefaultCellStyle(_workbook);

            _dateStyle = ExcelExporter.CreateDateStyle(_workbook, false);
            _dateTimeStyle = ExcelExporter.CreateDateStyle(_workbook, true);

            base.Load(data);

            for (int i = 0; i < _sheet.GetRow(0).LastCellNum; i++)
            {
                _sheet.AutoSizeColumn(i);
            }

            _workbook.Write(_stream);
        }

        protected override void Resize(int headerRows, int headerColumns, int rows, int columns)
        {
            _headerRows = headerRows;
            _headerColumns = headerColumns;
            _sheet.CreateFreezePane(headerColumns, headerRows);

            for (int row = 0; row < headerRows; row++)
            {
                for (int column = 0; column < headerColumns; column++)
                {
                    SetHeader(row, column, "", 1, 1);
                }
            }
        }

        private ICell GetCell(int row, int column)
        {
            var sheetRow = _sheet.GetRow(row) ?? _sheet.CreateRow(row);
            var cell = sheetRow.GetCell(column) ?? sheetRow.CreateCell(column);
            return cell;
        }

        protected override void SetCell(int row, int column, object value)
        {
            var cell = GetCell(_headerRows + row, _headerColumns + column);
            cell.CellStyle = _defaultCellStyle;

            if (value == null || value is string)
            {
                cell.SetCellValue((string)value);
            }
            else if (value is long)
            {
                cell.SetCellValue((long)value);
            }
            else if (value is double)
            {
                cell.SetCellValue((double)value);
            }
            else if (value is bool)
            {
                cell.SetCellValue((bool)value);
            }
            else if (value is DateTime)
            {
                cell.SetCellValue((DateTime)value);
                cell.CellStyle = _dateTimeStyle;
            }
            else if (value is DateTimeOffset)
            {
                cell.SetCellValue(((DateTimeOffset)value).LocalDateTime);
                cell.CellStyle = _dateTimeStyle;
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
        }

        protected override void SetHeader(int row, int column, string data, int rowSpan, int columnSpan)
        {
            var cell = GetCell(row, column);

            cell.CellStyle = (row < _headerRows) ? _columnHeaderStyle : _rowHeaderStyle;
            cell.SetCellValue(data ?? "(blank)");

            if (rowSpan > 1 || columnSpan > 1)
            {
                _sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(
                    row,
                    row + rowSpan - 1,
                    column,
                    column + columnSpan - 1
                ));
            }
        }
    }
}
