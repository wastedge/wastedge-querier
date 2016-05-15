using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class IdentityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        public static readonly IdentityComparer<T> Instance = new IdentityComparer<T>();

        private IdentityComparer()
        {
        }

        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
