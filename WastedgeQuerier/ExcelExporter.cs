﻿using System;
using System.Collections.Generic;
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
        public void Export(Stream stream, IList<ResultSet> resultSets)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (resultSets == null)
                throw new ArgumentNullException(nameof(resultSets));

            var firstResultSet = resultSets[0];

            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet(PrettifyName(firstResultSet.Entity.Name));
            sheet.DefaultRowHeightInPoints = 15;

            var headerStyle = CreateColumnHeaderStyle(workbook);
            var dateStyle = CreateDateStyle(workbook, false);
            var dateTimeStyle = CreateDateStyle(workbook, true);
            var wrapStyle = workbook.CreateCellStyle();
            wrapStyle.WrapText = true;

            // Create the headers.

            var row = sheet.CreateRow(0);

            var columnMap = ApiUtils.BuildColumnMap(firstResultSet);

            for (int i = 0; i < firstResultSet.FieldCount; i++)
            {
                var member = firstResultSet.Entity.Members[firstResultSet.GetFieldName(i)];
                AddHeader(row, columnMap[i], HumanText.GetMemberName(member), member.Comments, headerStyle);
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

                        AddCell(row, columnMap[i], resultSet[i], cellStyle);
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

        private void AddHeader(IRow row, int column, object value, string comment, ICellStyle headerStyle)
        {
            var cell = row.CreateCell(column);
            if (!String.IsNullOrEmpty(comment))
            {
                var cellComment = ((XSSFSheet)row.Sheet).CreateComment();
                cellComment.String = new XSSFRichTextString(comment);
                cell.CellComment = cellComment;
            }

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

        public static ICellStyle CreateDateStyle(XSSFWorkbook workbook, bool withTime)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            var dateStyle = workbook.CreateCellStyle();
            var dataFormat = workbook.CreateDataFormat();

            string format = withTime ? ExcelUtil.GetDateTimeFormat() : ExcelUtil.GetDateFormat();

            dateStyle.DataFormat = dataFormat.GetFormat(format);

            return dateStyle;
        }

        public readonly static XSSFColor DefaultColumnHeaderFillColor = new XSSFColor(new byte[] { 219, 229, 241 });
        public readonly static XSSFColor DefaultCellFillColor = new XSSFColor(new byte[] { 255, 255, 255 });
        public readonly static XSSFColor DefaultCellBorderColor = new XSSFColor(new byte[] { 181, 202, 227 });
        
        public static ICellStyle CreateColumnHeaderStyle(XSSFWorkbook workbook)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            var style = (XSSFCellStyle)workbook.CreateCellStyle();

            style.FillForegroundXSSFColor = ExcelExporter.DefaultColumnHeaderFillColor;
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Top;

            var font = workbook.CreateFont();

            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }

        public static ICellStyle CreateRowHeaderStyle(XSSFWorkbook workbook)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            var style = (XSSFCellStyle)workbook.CreateCellStyle();

            style.FillForegroundXSSFColor = ExcelExporter.DefaultCellFillColor;
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Top;
            style.BorderBottom = BorderStyle.Thin;

            var font = workbook.CreateFont();

            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }

        public static ICellStyle CreateDefaultCellStyle(XSSFWorkbook workbook)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            var style = (XSSFCellStyle)workbook.CreateCellStyle();

            style.FillForegroundXSSFColor = ExcelExporter.DefaultCellFillColor;
            style.FillPattern = FillPattern.SolidForeground;
            style.BorderBottom = BorderStyle.Thin;
            style.SetBorderColor(NPOI.XSSF.UserModel.Extensions.BorderSide.BOTTOM, ExcelExporter.DefaultCellBorderColor);
            return style;
        }
    }
}
