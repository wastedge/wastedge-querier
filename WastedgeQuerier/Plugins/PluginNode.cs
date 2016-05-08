using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Plugins
{
    internal abstract class PluginNode
    {
        public string Name { get; }
        public string Path { get; }

        protected PluginNode(string name, string path)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Name = name;
            Path = path;
        }
    }
}
