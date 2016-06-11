using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeApi;

namespace WastedgeQuerier.Export
{
    internal static class ApiUtils
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string DateTimeFormat = DateFormat + "'T'HH:mm:ss.fff";
        private const string DateTimeTzFormat = DateTimeFormat + "zzz";

        public static DateTime? ParseDate(string value)
        {
            if (value == null)
                return null;

            return DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
        }

        public static DateTime? ParseDateTime(string value)
        {
            if (value == null)
                return null;

            return DateTime.ParseExact(value, DateTimeFormat, CultureInfo.InvariantCulture);
        }

        public static DateTimeOffset? ParseDateTimeOffset(string value)
        {
            if (value == null)
                return null;

            return DateTimeOffset.ParseExact(value, DateTimeTzFormat, CultureInfo.InvariantCulture);
        }

        public static string PrintDate(DateTime? value)
        {
            return value?.ToString(DateFormat);
        }

        public static string PrintDateTime(DateTime? value)
        {
            return value?.ToString(DateTimeFormat);
        }

        public static string PrintDateTimeOffset(DateTime? value)
        {
            return value?.ToString(DateTimeTzFormat);
        }

        public static string PrintDate(DateTimeOffset? value)
        {
            return value?.ToString(DateFormat);
        }

        public static string PrintDateTime(DateTimeOffset? value)
        {
            return value?.ToString(DateTimeFormat);
        }

        public static string PrintDateTimeOffset(DateTimeOffset? value)
        {
            return value?.ToString(DateTimeTzFormat);
        }

        public static string Serialize(object value, EntityDataType dataType)
        {
            if (value == null)
                return "";
            if (value is string)
                return (string)value;
            if (value is DateTime)
            {
                switch (dataType)
                {
                    case EntityDataType.Date:
                        return PrintDate((DateTime)value);
                    case EntityDataType.DateTime:
                        return PrintDateTime((DateTime)value);
                    case EntityDataType.DateTimeTz:
                        return PrintDateTimeOffset((DateTime)value);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
            if (value is DateTimeOffset)
            {
                switch (dataType)
                {
                    case EntityDataType.Date:
                        return PrintDate((DateTimeOffset)value);
                    case EntityDataType.DateTime:
                        return PrintDateTime((DateTimeOffset)value);
                    case EntityDataType.DateTimeTz:
                        return PrintDateTimeOffset((DateTimeOffset)value);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
            if (value is int)
                return ((int)value).ToString(CultureInfo.InvariantCulture);
            if (value is long)
                return ((long)value).ToString(CultureInfo.InvariantCulture);
            if (value is float)
                return ((float)value).ToString(CultureInfo.InvariantCulture);
            if (value is double)
                return ((double)value).ToString(CultureInfo.InvariantCulture);
            if (value is decimal)
                return ((decimal)value).ToString(CultureInfo.InvariantCulture);

            throw new ArgumentOutOfRangeException(nameof(value));
        }

        public static string BuildFilters(IEnumerable<Filter> filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            var sb = new StringBuilder();

            foreach (var filter in filters)
            {
                if (sb.Length > 0)
                    sb.Append('&');
                sb.Append(Uri.EscapeDataString(filter.Field.Name)).Append('=');

                if (filter.Field is EntityIdField)
                {
                    if (filter.Type != FilterType.Equal)
                        throw new ApiException("ID field can only be compared equal");

                    Append(sb, filter.Value, filter.Field.DataType);
                    continue;
                }

                switch (filter.Type)
                {
                    case FilterType.IsNull:
                        sb.Append("is.null");
                        break;
                    case FilterType.NotIsNull:
                        sb.Append("not.is.null");
                        break;
                    case FilterType.IsTrue:
                        sb.Append("is.true");
                        break;
                    case FilterType.NotIsTrue:
                        sb.Append("not.is.true");
                        break;
                    case FilterType.IsFalse:
                        sb.Append("is.false");
                        break;
                    case FilterType.NotIsFalse:
                        sb.Append("not.is.false");
                        break;
                    case FilterType.In:
                        sb.Append("in.");
                        AppendList(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.NotIn:
                        sb.Append("not.in.");
                        AppendList(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.Like:
                        sb.Append("like.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.NotLike:
                        sb.Append("not.like.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.Equal:
                        sb.Append("eq.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.NotEqual:
                        sb.Append("ne.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.GreaterThan:
                        sb.Append("gt.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.GreaterEqual:
                        sb.Append("gte.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.LessThan:
                        sb.Append("lt.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    case FilterType.LessEqual:
                        sb.Append("lte.");
                        Append(sb, filter.Value, filter.Field.DataType);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return sb.ToString();
        }

        private static void AppendList(StringBuilder sb, object value, EntityDataType dataType)
        {
            if (value == null)
                return;

            var enumerable = value as IEnumerable;
            if (enumerable == null)
                throw new ApiException("Expected parameter to the IN filter to be a collection");

            bool hadOne = false;

            foreach (object element in enumerable)
            {
                string serialized = Serialize(element, dataType) ?? "";

                if (serialized.IndexOf('\'') != -1 || serialized.IndexOf(',') != -1)
                    serialized = "'" + serialized.Replace("'", "''") + "'";

                if (hadOne)
                    sb.Append(',');
                else
                    hadOne = true;

                sb.Append(Uri.EscapeDataString(serialized));
            }
        }

        private static void Append(StringBuilder sb, object value, EntityDataType dataType)
        {
            sb.Append(Uri.EscapeDataString(Serialize(value, dataType)));
        }
    }
}
