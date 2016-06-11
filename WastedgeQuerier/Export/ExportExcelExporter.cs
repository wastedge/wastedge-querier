using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WastedgeApi;
using WastedgeQuerier.Formats;
using WastedgeQuerier.Util;

namespace WastedgeQuerier.Export
{
    internal class ExportExcelExporter
    {
        public void Export(FileStream stream, List<JObject> results, ExportDefinition export)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (results == null)
                throw new ArgumentNullException(nameof(results));
            if (export == null)
                throw new ArgumentNullException(nameof(export));

            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet(PrettifyName(export.Entity.Name));
            sheet.DefaultRowHeightInPoints = 15;

            var headerStyle = CreateHeaderStyle(workbook);
            var dateStyle = CreateDateStyle(workbook, false);
            var dateTimeStyle = CreateDateStyle(workbook, true);
            var wrapStyle = workbook.CreateCellStyle();
            wrapStyle.WrapText = true;

            // Create the headers.

            var row = sheet.CreateRow(0);

            for (int i = 0; i < export.Fields.Count; i++)
            {
                AddHeader(row, i, HumanText.GetEntityMemberPath(export.Fields[i]), headerStyle);
            }

            int rowOffset = 0;
            bool haveAutosized = false;

            foreach (var result in results)
            {
                row = sheet.CreateRow(++rowOffset);

                for (int i = 0; i < export.Fields.Count; i++)
                {
                    var field = export.Fields[i];
                    ICellStyle cellStyle = null;

                    switch (((EntityTypedField)field.Tail).DataType)
                    {
                        case EntityDataType.Date:
                            cellStyle = dateStyle;
                            break;

                        case EntityDataType.DateTime:
                        case EntityDataType.DateTimeTz:
                            cellStyle = dateTimeStyle;
                            break;
                    }

                    AddCell(row, i, GetValue(field, result), cellStyle);
                }

                // We only auto size the top 20 rows for performance reasons.

                if (rowOffset == 20)
                {
                    haveAutosized = true;
                    AutoSizeColumns(export.Fields.Count, sheet);
                }
            }

            if (!haveAutosized)
                AutoSizeColumns(export.Fields.Count, sheet);

            workbook.Write(stream);
        }

        private object GetValue(EntityMemberPath field, JToken entity)
        {
            JToken result = entity;

            for (int i = 0; i < field.Count; i++)
            {
                result = ((JObject)result)[field[i].Name];
                if (result.Type == JTokenType.Null)
                    return null;
            }

            return ((JValue)result).Value;
        }

        private static void AutoSizeColumns(int fields, ISheet sheet)
        {
            for (int i = 0; i < fields; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        public static string PrettifyName(string name)
        {
            int pos = name.IndexOf('/');
            if (pos == -1)
                return name;

            return name.Substring(pos + 1) + " (" + name.Substring(0, pos).Replace('/', ' ') + ")";
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
            else if (value is double)
                cell.SetCellValue((double)value);
            else if (value is bool)
                cell.SetCellValue((bool)value);
            else if (value is DateTime)
                cell.SetCellValue((DateTime)value);
            else if (value is DateTimeOffset)
                cell.SetCellValue(((DateTimeOffset)value).LocalDateTime);
            else
                throw new ArgumentException("Invalid type");
        }

        private static ICellStyle CreateDateStyle(XSSFWorkbook workbook, bool withTime)
        {
            var dateStyle = workbook.CreateCellStyle();
            var dataFormat = workbook.CreateDataFormat();

            string format = withTime ? ExcelUtil.GetDateTimeFormat() : ExcelUtil.GetDateFormat();

            dateStyle.DataFormat = dataFormat.GetFormat(format);

            return dateStyle;
        }

        private static ICellStyle CreateHeaderStyle(XSSFWorkbook workbook)
        {
            var style = (XSSFCellStyle)workbook.CreateCellStyle();

            style.FillForegroundXSSFColor = new XSSFColor(new byte[] { 192, 192, 192 });
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = HorizontalAlignment.Center;

            var font = workbook.CreateFont();
            var defaultFont = workbook.GetFontAt(0);

            font.FontName = defaultFont.FontName;
            font.FontHeightInPoints = defaultFont.FontHeightInPoints;

            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }
    }
}
