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
        public JObject Data { get; }
        public List<ReportField> Columns { get; }
        public List<ReportField> Rows { get; }
        public List<ReportField> Values { get; }

        public ReportDataSet(JObject data, List<ReportField> columns, List<ReportField> rows, List<ReportField> values)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Data = data;
            Columns = columns;
            Rows = rows;
            Values = values;
        }
    }
}
