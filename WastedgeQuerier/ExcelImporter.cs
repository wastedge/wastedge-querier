using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using WastedgeApi;

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

            var calculatedFields = new HashSet<string>(entity.Members.OfType<EntityCalculatedField>().Select(p => p.Name));

            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            var headers = GetHeaders(sheet);

            var recordSet = new RecordSet();

            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                var record = GetRecord(sheet.GetRow(i), headers, calculatedFields);
                if (record != null)
                    recordSet.Add(record);
            }

            return recordSet;
        }

        private Record GetRecord(IRow row, List<string> headers, HashSet<string> calculatedFields)
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
                            if (DateUtil.IsCellDateFormatted(cell))
                                value = DateTime.FromOADate(cell.NumericCellValue);
                            else
                                value = cell.NumericCellValue;
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

                record[headers[i]] = value;
            }

            if (record.Count == 0)
                return null;

            return record;
        }

        private List<string> GetHeaders(ISheet sheet)
        {
            var headers = new List<string>();
            var row = sheet.GetRow(0);

            for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
            {
                headers.Add(row.GetCell(i).StringCellValue);
            }

            return headers;
        }
    }
}
