using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WastedgeApi;
using WastedgeQuerier.Export;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    public class ExcelExporter
    {
        public void Export(Stream stream, IList<ResultSet> resultSets, ApiRowErrorsCollection errors)
        {
            if (resultSets == null)
                throw new ArgumentNullException(nameof(resultSets));

            var recordSet = new RecordSet();

            foreach (var resultSet in resultSets)
            {
                recordSet.AddResultSet(resultSet);
            }

            Export(stream, resultSets[0].Entity, recordSet, errors);
        }

        public void Export(Stream stream, EntitySchema entity, RecordSet recordSet, ApiRowErrorsCollection errors)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (recordSet == null)
                throw new ArgumentNullException(nameof(recordSet));

            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet(PrettifyName(entity.Name));
            sheet.DefaultRowHeightInPoints = 15;

            var errorColor = new XSSFColor(Color.Orange);
            var errorStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            errorStyle.FillForegroundColorColor = errorColor;
            errorStyle.FillPattern = FillPattern.SolidForeground;
            var headerStyle = CreateHeaderStyle(workbook);
            var dateStyle = CreateDateStyle(workbook, null, false);
            var dateErrorStyle = CreateDateStyle(workbook, errorColor, false);
            var dateTimeStyle = CreateDateStyle(workbook, null, true);
            var dateTimeErrorStyle = CreateDateStyle(workbook, errorColor, true);

            // Create the headers.

            var row = sheet.CreateRow(0);

            var fieldNames = recordSet[0].FieldNames.OrderBy(p => p.ToLower()).ToList();

            for (int i = 0; i < fieldNames.Count; i++)
            {
                var member = entity.Members[fieldNames[i]];
                AddHeader(row, i, HumanText.GetMemberName(member), member.Comments, headerStyle);
            }

            bool haveAutosized = false;
            Dictionary<int, ApiRowErrors> errorMap = null;
            if (errors != null)
                errorMap = errors.ToDictionary(p => p.Row, p => p);

            for (int i = 0; i < recordSet.Count; i++)
            {
                var record = recordSet[i];
                row = sheet.CreateRow(i + 1);
                ApiRowErrors rowErrors = null;
                if (errorMap != null)
                    errorMap.TryGetValue(i, out rowErrors);

                for (int j = 0; j < fieldNames.Count; j++)
                {
                    ICellStyle cellStyle = null;
                    if (rowErrors != null)
                        cellStyle = errorStyle;
                    var field = fieldNames[j];

                    switch (((EntityTypedField)entity.Members[field]).DataType)
                    {
                        case EntityDataType.Date:
                            cellStyle = rowErrors == null ? dateStyle : dateErrorStyle;
                            break;

                        case EntityDataType.DateTime:
                        case EntityDataType.DateTimeTz:
                            cellStyle = rowErrors == null ? dateTimeStyle : dateTimeErrorStyle;
                            break;
                    }

                    string fieldError = GetFieldError(rowErrors, field);

                    AddCell(row, j, record[field], cellStyle, fieldError);
                }

                // We only auto size the top 20 rows for performance reasons.

                if (i == 20)
                {
                    haveAutosized = true;
                    AutoSizeColumns(fieldNames.Count, sheet);
                }
            }

            if (!haveAutosized)
                AutoSizeColumns(fieldNames.Count, sheet);

            workbook.Write(stream);
        }

        private string GetFieldError(ApiRowErrors rowErrors, string field)
        {
            if (rowErrors == null)
                return null;

            StringBuilder sb = null;

            foreach (var error in rowErrors.Errors)
            {
                if (
                    (field == "$id" && String.IsNullOrEmpty(error.Field)) ||
                    String.Equals(field, error.Field, StringComparison.OrdinalIgnoreCase)
                ) {
                    if (sb == null)
                        sb = new StringBuilder();
                    else
                        sb.AppendLine();

                    sb.Append(error.Error);
                }
            }

            return sb?.ToString();
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

        private void AddCell(IRow row, int column, object value, ICellStyle style, string comment)
        {
            var cell = row.CreateCell(column);
            AddComment(row, cell, comment);
            SetValue(cell, value);
            if (style != null)
                cell.CellStyle = style;
        }

        private void AddHeader(IRow row, int column, object value, string comment, ICellStyle headerStyle)
        {
            var cell = row.CreateCell(column);
            AddComment(row, cell, comment);

            SetValue(cell, value);
            cell.CellStyle = headerStyle;
        }

        private static void AddComment(IRow row, ICell cell, string comment)
        {
            if (!String.IsNullOrEmpty(comment))
            {
                var cellComment = ((XSSFSheet)row.Sheet).CreateComment();
                cellComment.String = new XSSFRichTextString(comment);
                cell.CellComment = cellComment;
            }
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

        private static ICellStyle CreateDateStyle(XSSFWorkbook workbook, XSSFColor fillColor, bool withTime)
        {
            var dateStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            var dataFormat = workbook.CreateDataFormat();

            string format = withTime ? ExcelUtil.GetDateTimeFormat() : ExcelUtil.GetDateFormat();

            dateStyle.DataFormat = dataFormat.GetFormat(format);
            if (fillColor != null)
            {
                dateStyle.FillPattern = FillPattern.SolidForeground;
                dateStyle.FillForegroundColorColor = fillColor;
            }

            return dateStyle;
        }

        private static ICellStyle CreateHeaderStyle(XSSFWorkbook workbook)
        {
            var style = (XSSFCellStyle)workbook.CreateCellStyle();

            style.FillForegroundXSSFColor = new XSSFColor(new byte[] { 192, 192, 192 });
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

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
