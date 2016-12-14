using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using WastedgeApi;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    public class ExcelImporter
    {
        public RecordSet Import(Stream stream, EntitySchema entity)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var calculatedFields = new HashSet<EntityMember>(entity.Members.OfType<EntityCalculatedField>());

            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            var headers = GetHeaders(sheet, entity);

            var recordSet = new RecordSet();

            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                var record = GetRecord(sheet.GetRow(i), headers, calculatedFields);
                if (record != null)
                    recordSet.Add(record);
            }

            return recordSet;
        }

        private Record GetRecord(IRow row, List<EntityMember> headers, HashSet<EntityMember> calculatedFields)
        {
            var record = new Record();

            for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
            {
                if (i >= headers.Count || calculatedFields.Contains(headers[i]))
                    continue;

                object value = null;

                var cell = row.GetCell(i);
                if (cell != null)
                {
                    var cellType = cell.CellType;
                    if (cellType == CellType.Formula)
                        cellType = cell.CachedFormulaResultType;

                    switch (cellType)
                    {
                        case CellType.Numeric:
                            switch (((EntityTypedField)headers[i]).DataType)
                            {
                                case EntityDataType.Date:
                                case EntityDataType.DateTime:
                                case EntityDataType.DateTimeTz:
                                    value = DateTime.FromOADate(cell.NumericCellValue);
                                    break;

                                case EntityDataType.Int:
                                case EntityDataType.Long:
                                    value = (long)cell.NumericCellValue;
                                    break;

                                default:
                                    value = (decimal)cell.NumericCellValue;
                                    break;
                            }
                            break;
                        case CellType.String:
                            value = cell.StringCellValue.Length == 0 ? null : cell.StringCellValue;
                            break;
                        case CellType.Blank:
                            value = null;
                            break;
                        case CellType.Boolean:
                            value = cell.BooleanCellValue;
                            break;
                        case CellType.Unknown:
                        case CellType.Error:
                            throw new Exception("Excel document contains errors");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                record[headers[i].Name] = CoerceType(value, ((EntityTypedField)headers[i]).DataType);
            }

            if (record.Count == 0)
                return null;

            return record;
        }

        private object CoerceType(object value, EntityDataType type)
        {
            if (value == null)
                return null;

            switch (type)
            {
                case EntityDataType.String:
                    if (value is long)
                        value = ((long)value).ToString(CultureInfo.InvariantCulture);
                    else if (value is decimal)
                        value = ((decimal)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case EntityDataType.Long:
                case EntityDataType.Int:
                    if (value is string)
                        value = long.Parse((string)value);
                    break;
                case EntityDataType.Bool:
                    if (value is string)
                    {
                        switch (((string)value).ToLower())
                        {
                            case "true":
                            case "1":
                            case "yes":
                                value = true;
                                break;
                            case "false":
                            case "0":
                            case "no":
                                value = false;
                                break;
                        }
                    }
                    break;
            }

            return value;
        }

        private List<EntityMember> GetHeaders(ISheet sheet, EntitySchema entity)
        {
            var headers = new List<EntityMember>();
            var row = sheet.GetRow(0);
            var members = entity.Members.ToDictionary(HumanText.GetMemberName, StringComparer.OrdinalIgnoreCase);

            for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
            {
                EntityMember member;
                if (members.TryGetValue(row.GetCell(i).StringCellValue, out member))
                    headers.Add(member);
            }

            return headers;
        }
    }
}
