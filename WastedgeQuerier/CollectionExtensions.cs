using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> values)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (var value in values)
            {
                self.Add(value);
            }
        }
    }
}
