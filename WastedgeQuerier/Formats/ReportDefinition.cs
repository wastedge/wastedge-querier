using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WastedgeApi;
using WastedgeQuerier.Report;

namespace WastedgeQuerier.Formats
{
    public class ReportDefinition
    {
        private static readonly XNamespace Ns = "https://github.com/wastedge/wastedge-querier/2016/06/report";

        public EntitySchema Entity { get; set; }
        public List<Filter> Filters { get; }
        public List<ReportField> Fields { get; }

        public ReportDefinition()
        {
            Filters = new List<Filter>();
            Fields = new List<ReportField>();
        }

        public static ReportDefinition Load(Api api, string fileName)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var doc = XDocument.Load(fileName);
            if (doc.Root.Name != Ns + "report")
                throw new ArgumentException("Invalid report");

            var root = doc.Root;

            var result = new ReportDefinition();
            result.Entity = api.GetEntitySchema(root.Attribute("entity").Value);

            var filters = root.Element(Ns + "filters");
            if (filters != null)
                result.Filters.AddRange(FormatUtil.DeserializeFilters(result.Entity, filters));

            var fields = root.Element(Ns + "fields");
            if (fields != null)
            {
                foreach (var field in fields.Elements(Ns + "field"))
                {
                    var fieldPath = EntityMemberPath.Parse(api, result.Entity, field.Attribute("path").Value);
                    if (fieldPath == null)
                        continue;
                    var reportField = new ReportField(fieldPath);
                    reportField.Type = (ReportFieldType)Enum.Parse(typeof(ReportFieldType), field.Attribute("type").Value, true);
                    reportField.Transform = (ReportFieldTransform)Enum.Parse(typeof(ReportFieldTransform), field.Attribute("transform").Value, true);
                    result.Fields.Add(reportField);
                }
            }

            return result;
        }

        public void Save(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var fields = new XElement(Ns + "fields");

            foreach (var field in Fields)
            {
                fields.Add(new XElement(
                    Ns + "field",
                    new XAttribute("path", String.Join(".", field.Fields.ToString())),
                    new XAttribute("type", field.Type.ToString().ToLowerInvariant()),
                    new XAttribute("transform", field.Transform.ToString().ToLower())
                ));
            }

            var root = new XElement(
                Ns + "report",
                new XAttribute("entity", Entity.Name),
                FormatUtil.SerializeFilters(Ns, Filters),
                fields
            );

            new XDocument(root).Save(fileName);
        }
    }
}
