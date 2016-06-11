using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeApi;

namespace WastedgeQuerier
{
    internal class EntitySchemaComparer : IComparer<EntitySchema>
    {
        public static readonly EntitySchemaComparer Instance = new EntitySchemaComparer();

        private EntitySchemaComparer()
        {
        }

        public int Compare(EntitySchema x, EntitySchema y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));

            var aName = new EntityName(x.Name);
            var bName = new EntityName(y.Name);

            int result;
            if (aName.Header == null && bName.Header == null)
                result = 0;
            else if (aName.Header == null)
                result = -1;
            else if (bName.Header == null)
                result = 1;
            else
                result = String.Compare(aName.Header, bName.Header, StringComparison.CurrentCultureIgnoreCase);

            if (result != 0)
                return result;

            return String.Compare(aName.Name, bName.Name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
