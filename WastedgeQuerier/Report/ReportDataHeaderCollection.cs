using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Report
{
    [DebuggerDisplay("Count = {Count}")]
    internal class ReportDataHeaderCollection : IEnumerable<ReportDataHeader>
    {
        public static readonly ReportDataHeaderCollection Empty = new ReportDataHeaderCollection(new List<ReportDataHeader>());

        private readonly List<ReportDataHeader> _headers;

        public int Count => _headers.Count;

        public ReportDataHeader this[int index] => _headers[index];

        private ReportDataHeaderCollection(List<ReportDataHeader> headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            _headers = headers;
        }

        public static ReportDataHeaderCollection NewSorted(List<ReportDataHeader> headers)
        {
            headers.Sort((a, b) => Compare(a.Label, b.Label));
            return new ReportDataHeaderCollection(headers);
        }

        private static int Compare(string lhs, string rhs)
        {
            if (lhs == "")
                lhs = null;
            if (rhs == "")
                rhs = null;
            if (lhs == rhs)
                return 0;
            if (lhs == null)
                return 1;
            if (rhs == null)
                return -1;

            decimal lhsDecimal;
            decimal rhsDecimal;

            if (decimal.TryParse(lhs, out lhsDecimal) && decimal.TryParse(rhs, out rhsDecimal))
                return lhsDecimal.CompareTo(rhsDecimal);

            return String.Compare(lhs, rhs, StringComparison.CurrentCultureIgnoreCase);
        }

        public IEnumerator<ReportDataHeader> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
