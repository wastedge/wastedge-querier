using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeApi;
using WastedgeQuerier.Util;

namespace WastedgeQuerier.Report
{
    public class ReportField
    {
        public EntityMemberPath Fields { get; }
        public ReportFieldType Type { get; set; }
        public ReportFieldTransform Transform { get; set; }

        public ReportField(EntityMemberPath fields)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            Fields = fields;
        }

        public override string ToString()
        {
            if (Transform == ReportFieldTransform.None)
                return HumanText.GetEntityMemberPath(Fields);

            return
                (Transform == ReportFieldTransform.CountNumbers ? "Count" : Transform.ToString()) +
                " of " +
                HumanText.GetEntityMemberPath(Fields);
        }
    }
}
