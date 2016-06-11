using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier
{
    internal class EntityMemberPathComparer : IComparer<EntityMemberPath>
    {
        public static readonly EntityMemberPathComparer Instance = new EntityMemberPathComparer();

        private EntityMemberPathComparer()
        {
        }

        public int Compare(EntityMemberPath x, EntityMemberPath y)
        {
            for (int i = 0; i < Math.Min(x.Count, y.Count) - 1; i++)
            {
                int result = String.Compare(x[i].Name, y[i].Name, StringComparison.OrdinalIgnoreCase);
                if (result != 0)
                    return result;
            }

            if (x.Count != y.Count)
                return x.Count - y.Count;

            return String.Compare(x.Tail.Name, y.Tail.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
