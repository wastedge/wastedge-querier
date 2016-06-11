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
    internal class ExportDefinition
    {
        private static readonly XNamespace Ns = "https://github.com/wastedge/wastedge-querier/2016/06/export";

        public EntitySchema Entity { get; set; }
        public List<Filter> Filters { get; }
        public List<EntityMemberPath> Fields { get; }

        public ExportDefinition()
        {
            Filters = new List<Filter>();
            Fields = new List<EntityMemberPath>();
        }

        public static ExportDefinition Load(Api api, string fileName)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var doc = XDocument.Load(fileName);
            if (doc.Root.Name != Ns + "export")
                throw new ArgumentException("Invalid export");

            var root = doc.Root;

            var result = new ExportDefinition();
            result.Entity = api.GetEntitySchema(root.Attribute("entity").Value);

            var filters = root.Element(Ns + "filters");
            if (filters != null)
                result.Filters.AddRange(FormatUtil.DeserializeFilters(result.Entity, filters));

            var fields = root.Element(Ns + "fields");
            if (fields != null)
            {
                foreach (var field in fields.Elements(Ns + "field"))
                {
                    var fieldPath = EntityMemberPath.Parse(api, result.Entity, field.Value);
                    if (fieldPath == null)
                        continue;
                    result.Fields.Add(fieldPath);
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
                    field.ToString()
                ));
            }

            var root = new XElement(
                Ns + "export",
                new XAttribute("entity", Entity.Name),
                FormatUtil.SerializeFilters(Ns, Filters),
                fields
            );

            new XDocument(root).Save(fileName);
        }
    }
}
