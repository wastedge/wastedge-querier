using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript
{
    internal static class NullableExtensions
    {
        public static TResult? Map<TValue, TResult>(this TValue? self, Func<TValue, TResult> func)
            where TValue : struct
            where TResult : struct
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (self.HasValue)
                return func(self.Value);
            return null;
        }

        public static TResult Map<TValue, TResult>(this TValue? self, TResult @default, Func<TValue, TResult> func)
            where TValue : struct
            where TResult : struct
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (self.HasValue)
                return func(self.Value);
            return @default;
        }
    }
}
