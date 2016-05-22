using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WastedgeQuerier.Report
{
    internal abstract class ReportLoader
    {
        private int _headerRows;
        private int _headerColumns;

        public virtual void Load(ReportDataSet data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _headerRows = data.ColumnLevels;
            _headerColumns = data.RowLevels;

            Resize(
                _headerRows,
                _headerColumns,
                data.RowCount,
                data.ColumnCount
            );

            BuildHeaders(data.Columns, false);
            BuildHeaders(data.Rows, true);

            var values = data.Values;

            for (int row = 0; row < values.GetLength(0); row++)
            {
                for (int column = 0; column < values.GetLength(1); column++)
                {
                    var value = values[row, column];
                    if (!double.IsNaN(value))
                        SetCell(row, column, value);
                    else
                        SetCell(row, column, null);
                }
            }
        }

        private void BuildHeaders(ReportDataHeaderCollection headers, bool inverse)
        {
            int offset = 0;

            foreach (var header in headers)
            {
                BuildHeader(header, inverse, offset);
                offset += header.Span;
            }
        }

        private void BuildHeader(ReportDataHeader header, bool inverse, int offset)
        {
            int row = inverse ? offset + _headerRows : header.Level;
            int column = inverse ? header.Level : offset + _headerColumns;
            int rowSpan = inverse ? header.Span : 1;
            int columnSpan = inverse ? 1 : header.Span;

            SetHeader(row, column, header.Label, rowSpan, columnSpan);

            foreach (var child in header.Children)
            {
                BuildHeader(child, inverse, offset);
                offset += child.Span;
            }
        }

        protected abstract void Resize(int headerRows, int headerColumns, int rows, int columns);

        protected abstract void SetCell(int row, int column, object value);

        protected abstract void SetHeader(int row, int column, string data, int rowSpan, int columnSpan);
    }
}
