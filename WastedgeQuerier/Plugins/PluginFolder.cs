using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOPath = System.IO.Path;

namespace WastedgeQuerier.Plugins
{
    internal class PluginFolder : PluginNode
    {
        public static PluginFolder FromFolder(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var children = new List<PluginNode>();

            foreach (string directory in Directory.GetDirectories(path))
            {
                children.Add(PluginFolder.FromFolder(directory));
            }

            foreach (string fileName in Directory.GetFiles(path, "*.wqpkg"))
            {
                try
                {
                    children.Add(Plugin.Load(fileName));
                }
                catch
                {
                    // Ignore exceptions. Just skip everything that doesn't load.
                }
            }

            return new PluginFolder(path, new ReadOnlyCollection<PluginNode>(children));
        }

        public IList<PluginNode> Children { get; }

        private PluginFolder(string path, IList<PluginNode> children)
            : base(IOPath.GetFileName(path), path)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Children = children;
        }
    }
}
