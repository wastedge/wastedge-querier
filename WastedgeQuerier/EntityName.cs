using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier
{
    internal struct EntityName
    {
        public string Header { get; }
        public string Name { get; }

        public EntityName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            int pos = name.IndexOf('/');
            if (pos == -1)
            {
                Header = null;
                Name = name;
            }
            else
            {
                Header = name.Substring(0, pos);
                Name = name.Substring(pos + 1);
            }
        }
    }
}
