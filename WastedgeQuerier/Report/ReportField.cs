using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeApi;

namespace WastedgeQuerier.Report
{
    internal class ReportField
    {
        public IList<EntityMember> Fields { get; }
        public ReportFieldType Type { get; set; }
        public ReportFieldTransform Transform { get; set; }

        public ReportField(IList<EntityMember> fields)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            Fields = fields;
        }

        public override string ToString()
        {
            var tail = Fields[Fields.Count - 1];

            if (Transform != ReportFieldTransform.None)
            {
                string prefix = Transform == ReportFieldTransform.CountNumbers ? "Count" : Transform.ToString();

                return prefix + " of " + tail.Name;
            }

            return tail.Name;
        }

        public ReportField Clone()
        {
            return new ReportField(new List<EntityMember>(Fields))
            {
                Type = Type,
                Transform = Transform
            };
        }
    }
}
