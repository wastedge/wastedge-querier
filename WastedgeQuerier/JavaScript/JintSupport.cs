using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Newtonsoft.Json;

namespace WastedgeQuerier.JavaScript
{
    public static class JintSupport
    {
        public static string Stringify(JsInstance value, object replacer, JsInstance space)
        {
            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    if (space is JsNumber)
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        jsonWriter.Indentation = (int)(double)space.Value;
                    }
                    else if (space is JsString)
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        jsonWriter.Indentation = ((string)space.Value).Length;
                    }

                    Stringify(jsonWriter, value);
                }

                return writer.GetStringBuilder().ToString();
            }
        }

        private static void Stringify(JsonTextWriter jsonWriter, JsInstance value)
        {
            if (value is JsArray)
            {
                var array = (JsArray)value;

                jsonWriter.WriteStartArray();

                for (int i = 0; i < array.Length; i++)
                {
                    Stringify(jsonWriter, array.get(i));
                }

                jsonWriter.WriteEndArray();
            }
            else if (value is JsString)
            {
                jsonWriter.WriteValue((string)value.Value);
            }
            else if (value is JsNumber)
            {
                jsonWriter.WriteValue((double)value.Value);
            }
            else if (value is JsBoolean)
            {
                jsonWriter.WriteValue((bool)value.Value);
            }
            else if (value is JsNull)
            {
                jsonWriter.WriteNull();
            }
            else if (value is JsUndefined)
            {
                jsonWriter.WriteUndefined();
            }
            else if (value is JsObject)
            {
                var obj = (JsObject)value;

                jsonWriter.WriteStartObject();

                foreach (var entry in obj)
                {
                    jsonWriter.WritePropertyName(entry.Key);

                    Stringify(jsonWriter, entry.Value);
                }

                jsonWriter.WriteEndObject();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static JsInstance Parse(IGlobal global, string json)
        {
            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                jsonReader.DateParseHandling = DateParseHandling.None;

                if (!jsonReader.Read())
                    throw new InvalidOperationException();

                return Parse(global, jsonReader);
            }
        }

        private static JsInstance Parse(IGlobal global, JsonTextReader jsonReader)
        {
            switch (jsonReader.TokenType)
            {
                case JsonToken.StartObject:
                    return ParseObject(global, jsonReader);
                case JsonToken.StartArray:
                    return ParseArray(global, jsonReader);
                case JsonToken.Integer:
                    return global.NumberClass.New((long)jsonReader.Value);
                case JsonToken.Float:
                    return global.NumberClass.New((double)jsonReader.Value);
                case JsonToken.String:
                    return global.StringClass.New((string)jsonReader.Value);
                case JsonToken.Boolean:
                    return global.BooleanClass.New((bool)jsonReader.Value);
                case JsonToken.Null:
                    return JsNull.Instance;
                case JsonToken.Undefined:
                    return JsUndefined.Instance;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static JsInstance ParseObject(IGlobal global, JsonTextReader jsonReader)
        {
            var obj = global.ObjectClass.New();

            while (true)
            {
                if (!jsonReader.Read())
                    throw new InvalidOperationException();

                if (jsonReader.TokenType == JsonToken.EndObject)
                    return obj;

                string key = (string)jsonReader.Value;

                if (!jsonReader.Read())
                    throw new InvalidOperationException();

                obj[key] = Parse(global, jsonReader);
            }
        }

        private static JsInstance ParseArray(IGlobal global, JsonTextReader jsonReader)
        {
            var array = global.ArrayClass.New();

            for (int i = 0;; i++)
            {
                if (!jsonReader.Read())
                    throw new InvalidOperationException();

                if (jsonReader.TokenType == JsonToken.EndArray)
                    return array;

                array.put(i, Parse(global, jsonReader));
            }
        }
    }
}
