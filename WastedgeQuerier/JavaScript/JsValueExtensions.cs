using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Runtime;

namespace WastedgeQuerier.JavaScript
{
    internal static class JsValueExtensions
    {
        public static bool? ConvertToBoolean(this JsValue self)
        {
            if (self.IsNullOrUndefined())
                return null;

            return TypeConverter.ToBoolean(self);
        }

        public static int? ConvertToInt32(this JsValue self)
        {
            if (self.IsNullOrUndefined())
                return null;

            return TypeConverter.ToInt32(self);
        }

        public static long? ConvertToInt64(this JsValue self)
        {
            if (self.IsNullOrUndefined())
                return null;

            return (long)TypeConverter.ToInteger(self);
        }

        public static double? ConvertToDouble(this JsValue self)
        {
            if (self.IsNullOrUndefined())
                return null;

            return TypeConverter.ToNumber(self);
        }

        public static string ConvertToString(this JsValue self)
        {
            if (self.IsNullOrUndefined())
                return null;

            return TypeConverter.ToString(self);
        }

        public static object ConvertToNative(this JsValue self)
        {
            switch (self.Type)
            {
                case Types.None:
                case Types.Undefined:
                case Types.Null:
                    return null;
                case Types.Boolean:
                    return self.AsBoolean();
                case Types.String:
                    return self.AsString();
                case Types.Number:
                    double value = self.AsNumber();
                    if (Math.Abs(value - (int)value) < Double.Epsilon)
                        return (int)value;
                    return value;
                case Types.Object:
                    throw new ArgumentException("Cannot convert object to a native value");
                default:
                    throw new ArgumentOutOfRangeException(nameof(self));
            }
        }

        public static bool IsNullOrUndefined(this JsValue self)
        {
            switch (self.Type)
            {
                case Types.Null:
                case Types.Undefined:
                    return true;

                default:
                    return false;
            }
        }
    }
}
