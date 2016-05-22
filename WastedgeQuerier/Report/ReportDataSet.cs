using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WastedgeQuerier.Report
{
    internal class ReportDataSet
    {
        public double[,] Values { get; }
        public ReportDataHeaderCollection Columns { get; }
        public ReportDataHeaderCollection Rows { get; }
        public int ColumnLevels { get; }
        public int ColumnCount { get; }
        public int RowLevels { get; }
        public int RowCount { get; }

        public ReportDataSet(JObject data, IList<ReportField> columns, IList<ReportField> rows, IList<ReportField> values)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Columns = ParseHeaders((JArray)data["columns"]);
            Rows = ParseHeaders((JArray)data["rows"]);
            Values = BuildValues((JArray)data["values"]);

            RowLevels = GetLevels(Rows);
            RowCount = Values.GetLength(0);
            ColumnLevels = GetLevels(Columns);
            ColumnCount = Values.GetLength(1);
        }

        private int GetLevels(ReportDataHeaderCollection columns)
        {
            if (columns.Count == 0)
                return 0;
            return GetLevels(columns[0].Children) + 1;
        }

        private double[,] BuildValues(JArray values)
        {
            int rows = values.Count;
            int columns = values.Count == 0 ? 0 : ((JArray)values[0]).Count;

            var columnMap = BuildMap(Columns, columns);
            var rowMap = BuildMap(Rows, rows);

            var result = new double[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                var valuesRow = (JArray)values[row];

                for (int column = 0; column < columns; column++)
                {
                    var value = valuesRow[column];
                    double cellValue = value.Type == JTokenType.Null ? double.NaN : (double)value;
                    result[rowMap[row], columnMap[column]] = cellValue;
                }
            }

            return result;
        }

        private int[] BuildMap(ReportDataHeaderCollection headers, int size)
        {
            var map = new int[size];
            int offset = 0;

            BuildMap(headers, map, ref offset);

            return map;
        }

        private void BuildMap(ReportDataHeaderCollection headers, int[] map, ref int offset)
        {
            foreach (var header in headers)
            {
                if (header.Children.Count == 0)
                    map[header.Offset] = offset++;
                else
                    BuildMap(header.Children, map, ref offset);
            }
        }

        private ReportDataHeaderCollection ParseHeaders(JArray data)
        {
            var headers = new List<ReportDataHeader>();
            int span = 0;

            for (int i = 0; i < data.Count; i++)
            {
                var header = ParseHeader(data[i], 0, span);
                headers.Add(header);
                span += header.Span;
            }

            return ReportDataHeaderCollection.NewSorted(headers);
        }

        private ReportDataHeader ParseHeader(JToken data, int level, int offset)
        {
            if (data.Type != JTokenType.Array)
            {
                var label = (string)data;
                if (String.IsNullOrEmpty(label))
                    label = null;

                return new ReportDataHeader(
                    label,
                    level,
                    offset,
                    1,
                    ReportDataHeaderCollection.Empty
                );
            }

            var subData = (JArray)data;
            int span = 0;
            var children = new List<ReportDataHeader>();

            for (int i = 1; i < subData.Count; i++)
            {
                var header = ParseHeader(subData[i], level + 1, offset + span);
                children.Add(header);
                span += header.Span;
            }

            if (span == 0)
                span = 1;

            var subLabel = (string)subData[0];
            if (String.IsNullOrEmpty(subLabel))
                subLabel = null;

            return new ReportDataHeader(
                subLabel,
                level,
                offset,
                span,
                ReportDataHeaderCollection.NewSorted(children)
            );
        }
    }
}
