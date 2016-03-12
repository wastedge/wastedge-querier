using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using WastedgeApi;

namespace WastedgeQuerier
{
    public class ExcelExporter
    {
        private const string DefaultFont = "Calibri";
        private const int DefaultFontHeight = 11;

        public void Export(Stream stream, IList<ResultSet> resultSets)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (resultSets == null)
                throw new ArgumentNullException(nameof(resultSets));

            var firstResultSet = resultSets[0];

            var workbook = new HSSFWorkbook();
            var defaultFont = workbook.GetFontAt(0);
            defaultFont.FontName = DefaultFont;
            defaultFont.FontHeightInPoints = DefaultFontHeight;

            var sheet = workbook.CreateSheet(PrettifyName(firstResultSet.Entity.Name));
            sheet.DefaultRowHeightInPoints = 15;

            var headerStyle = CreateHeaderStyle(workbook);
            var dateStyle = CreateDateStyle(workbook, false);
            var dateTimeStyle = CreateDateStyle(workbook, true);
            var wrapStyle = workbook.CreateCellStyle();
            wrapStyle.WrapText = true;

            // Create the headers.

            var row = sheet.CreateRow(0);

            for (int i = 0; i < firstResultSet.FieldCount; i++)
            {
                AddHeader(row, i, firstResultSet.GetFieldName(i), headerStyle);
            }

            int rowOffset = 0;
            bool haveAutosized = false;

            foreach (var resultSet in resultSets)
            {
                resultSet.Reset();

                while (resultSet.Next())
                {
                    row = sheet.CreateRow(++rowOffset);

                    for (int i = 0; i < resultSet.FieldCount; i++)
                    {
                        ICellStyle cellStyle = null;

                        switch (resultSet.GetField(i).DataType)
                        {
                            case EntityDataType.Date:
                                cellStyle = dateStyle;
                                break;

                            case EntityDataType.DateTime:
                            case EntityDataType.DateTimeTz:
                                cellStyle = dateTimeStyle;
                                break;
                        }

                        AddCell(row, i, resultSet[i], cellStyle);
                    }

                    // We only auto size the top 20 rows for performance reasons.

                    if (rowOffset == 20)
                    {
                        haveAutosized = true;
                        AutoSizeColumns(firstResultSet, sheet);
                    }
                }
            }

            if (!haveAutosized)
                AutoSizeColumns(firstResultSet, sheet);

            workbook.Write(stream);
        }

        private static void AutoSizeColumns(ResultSet firstResultSet, ISheet sheet)
        {
            for (int i = 0; i < firstResultSet.FieldCount; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        private static string PrettifyName(string name)
        {
            int pos = name.IndexOf('/');
            if (pos == -1)
                return name;

            return name.Substring(0, pos) + " (" + name.Substring(pos + 1).Replace('/', ' ') + ")";
        }

        private void AddCell(IRow row, int column, object value, ICellStyle style)
        {
            var cell = row.CreateCell(column);
            SetValue(cell, value);
            if (style != null)
                cell.CellStyle = style;
        }

        private void AddHeader(IRow row, int column, object value, ICellStyle headerStyle)
        {
            var cell = row.CreateCell(column);
            SetValue(cell, value);
            cell.CellStyle = headerStyle;
        }

        private void SetValue(ICell cell, object value)
        {
            if (value == null || value is string)
                cell.SetCellValue((string)value);
            else if (value is long)
                cell.SetCellValue((long)value);
            else if (value is decimal)
                cell.SetCellValue((double)(decimal)value);
            else if (value is bool)
                cell.SetCellValue((bool)value);
            else if (value is DateTime)
                cell.SetCellValue((DateTime)value);
            else if (value is DateTimeOffset)
                cell.SetCellValue(((DateTimeOffset)value).LocalDateTime);
            else
                throw new ArgumentException("Invalid type");
        }

        private static ICellStyle CreateDateStyle(HSSFWorkbook workbook, bool withTime)
        {
            var dateStyle = workbook.CreateCellStyle();

            var dataFormat = workbook.CreateDataFormat();

            var dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;

            string format = dateTimeFormat.ShortDatePattern;

            if (withTime)
                format += " " + dateTimeFormat.ShortTimePattern;

            if (format.EndsWith(" tt") || format.EndsWith(" TT"))
                format = format.Substring(0, format.Length - 2) + "AM/PM";

            dateStyle.DataFormat = dataFormat.GetFormat(format);

            return dateStyle;
        }

        private static ICellStyle CreateHeaderStyle(HSSFWorkbook workbook)
        {
            var palette = workbook.GetCustomPalette();

            var style = workbook.CreateCellStyle();

            style.FillForegroundColor = palette.FindColor(192, 192, 192).Indexed;
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            var font = workbook.CreateFont();

            font.FontName = DefaultFont;
            font.FontHeightInPoints = DefaultFontHeight;

            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }
    }
}
