using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WastedgeApi;

namespace WastedgeQuerier.Formats
{
    internal static class FormatUtil
    {
        public static XElement SerializeFilters(XNamespace ns, IEnumerable<Filter> filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            var element = new XElement(ns + "filters");

            foreach (var filter in filters)
            {
                var filterElement = new XElement(
                    ns + "filter",
                    new XAttribute("name", filter.Field.Name),
                    new XAttribute("type", filter.Type.ToString().ToLowerInvariant())
                );

                element.Add(filterElement);

                if (filter.Value != null)
                {
                    string type;
                    string value;

                    if (filter.Value is string)
                    {
                        type = "string";
                        value = (string)filter.Value;
                    }
                    else if (filter.Value is decimal)
                    {
                        type = "decimal";
                        value = XmlConvert.ToString((decimal)filter.Value);
                    }
                    else if (filter.Value is DateTime)
                    {
                        type = "datetime";
                        value = XmlConvert.ToString((DateTime)filter.Value, XmlDateTimeSerializationMode.Local);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported filter type");
                    }

                    filterElement.Add(new XElement(
                        ns + "value",
                        new XAttribute("type", type),
                        new XText(value)
                    ));
                }
            }

            return element;
        }

        public static List<Filter> DeserializeFilters(EntitySchema entity, XElement element)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var result = new List<Filter>();

            if (element.Name.LocalName != "filters")
                throw new InvalidOperationException();

            foreach (var filter in element.Elements(element.Name.Namespace + "filter"))
            {
                string name = filter.Attribute("name").Value;
                string type = filter.Attribute("type").Value;
                var valueElement = filter.Element(element.Name.Namespace + "value");
                object value = null;

                if (valueElement != null)
                {
                    string valueType = valueElement.Attribute("type").Value;
                    switch (valueType)
                    {
                        case "string":
                            value = valueElement.Value;
                            break;
                        case "decimal":
                            value = XmlConvert.ToDecimal(valueElement.Value);
                            break;
                        case "datetime":
                            value = XmlConvert.ToDateTime(valueElement.Value, XmlDateTimeSerializationMode.Local);
                            break;
                        default:
                            throw new InvalidOperationException("Unsupported filter type");
                    }
                }

                var member = entity.Members[name] as EntityPhysicalField;
                if (member != null)
                    result.Add(new Filter(member, (FilterType)Enum.Parse(typeof(FilterType), type, true), value));
            }

            return result;
        }
    }
}
