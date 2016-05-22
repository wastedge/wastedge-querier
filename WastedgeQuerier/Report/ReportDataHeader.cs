using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Report
{
    [DebuggerDisplay("Label = {Label}, Level = {Level}, Offset = {Offset}, Span = {Span}, Children = {Children.Count}")]
    internal class ReportDataHeader
    {
        public string Label { get; }
        public int Level { get; }
        public int Offset { get; }
        public int Span { get; }
        public ReportDataHeaderCollection Children { get; }

        public ReportDataHeader(string label, int level, int offset, int span, ReportDataHeaderCollection children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Label = label;
            Level = level;
            Offset = offset;
            Span = span;
            Children = children;
        }
    }
}
