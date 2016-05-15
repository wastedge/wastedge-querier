using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class EnumConverter<T>
        where T : struct
    {
        private readonly Dictionary<string, T> _fromString = new Dictionary<string, T>();
        private readonly Dictionary<T, string> _toString = new Dictionary<T, string>();

        public EnumConverter()
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Type must be an enum");

            foreach (T value in typeof(T).GetEnumValues())
            {
                _fromString.Add(Inflector.Underscore(value.ToString()), value);
                _toString.Add(value, Inflector.Underscore(value.ToString()));
            }
        }

        public bool TryFromString(string value, out T result)
        {
            result = default(T);
            if (value == null)
                return false;
            return _fromString.TryGetValue(value, out result);
        }

        public T FromString(string value, T @default)
        {
            T result;
            if (!TryFromString(value, out result))
                result = @default;
            return result;
        }

        public string ToString(T value)
        {
            return _toString[value];
        }
    }
}
