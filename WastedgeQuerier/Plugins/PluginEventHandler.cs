using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Plugins
{
    internal class PluginEventArgs : EventArgs
    {
        public Plugin Plugin { get; }

        public PluginEventArgs(Plugin plugin)
        {
            if (plugin == null)
                throw new ArgumentNullException(nameof(plugin));

            Plugin = plugin;
        }
    }

    internal delegate void PluginEventHandler(object sender, PluginEventArgs e);
}
