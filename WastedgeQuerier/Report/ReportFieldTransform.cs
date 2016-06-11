using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Report
{
    public enum ReportFieldTransform
    {
        None,
        Sum,
        Count,
        Average,
        Max,
        Min,
        Product,
        CountNumbers,
        StdDev,
        StdDevp,
        Var,
        Varp
    }
}
