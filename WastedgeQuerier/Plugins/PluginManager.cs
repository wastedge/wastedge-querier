using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IOPath = System.IO.Path;

namespace WastedgeQuerier.Plugins
{
    internal class PluginManager
    {
        private readonly MenuItem _pluginsMenuItem;
        private readonly MenuItem _pluginsSeparatorMenuItem;

        public string Path { get; }

        public event PluginEventHandler PluginOpened;

        public PluginManager(string path, MenuItem pluginsMenuItem, MenuItem pluginsSeparatorMenuItem)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (pluginsMenuItem == null)
                throw new ArgumentNullException(nameof(pluginsMenuItem));
            if (pluginsSeparatorMenuItem == null)
                throw new ArgumentNullException(nameof(pluginsSeparatorMenuItem));

            Directory.CreateDirectory(path);

            Path = path;
            _pluginsMenuItem = pluginsMenuItem;
            _pluginsSeparatorMenuItem = pluginsSeparatorMenuItem;
        }

        public void Reload()
        {
            CleanMenu();

            var plugins = PluginFolder.FromFolder(Path);

            LoadMenuItems(_pluginsMenuItem.MenuItems, plugins);

            _pluginsSeparatorMenuItem.Visible = _pluginsMenuItem.MenuItems.IndexOf(_pluginsSeparatorMenuItem) > 0;
        }

        private void LoadMenuItems(Menu.MenuItemCollection menuItems, PluginFolder folder)
        {
            int offset = 0;

            foreach (var child in folder.Children.OfType<PluginFolder>())
            {
                if (HasAnyPlugins(child))
                    menuItems.Add(offset++, BuildMenuItem(child));
            }

            foreach (var child in folder.Children.OfType<Plugin>())
            {
                menuItems.Add(offset++, BuildMenuItem(child));
            }
        }

        private bool HasAnyPlugins(PluginFolder folder)
        {
            foreach (var item in folder.Children)
            {
                if (item is Plugin || HasAnyPlugins((PluginFolder)item))
                    return true;
            }

            return false;
        }

        private MenuItem BuildMenuItem(PluginNode node)
        {
            var folder = node as PluginFolder;
            if (folder != null)
            {
                var menuItem = new MenuItem
                {
                    Text = folder.Name,
                    Tag = folder
                };

                LoadMenuItems(menuItem.MenuItems, folder);

                return menuItem;
            }

            var plugin = (Plugin)node;

            var pluginMenuItem = new MenuItem
            {
                Text = EscapeMenuText(plugin.Name),
                Tag = plugin
            };

            pluginMenuItem.Click += PluginMenuItem_Click;

            return pluginMenuItem;
        }

        private void PluginMenuItem_Click(object sender, EventArgs e)
        {
            var plugin = (Plugin)((MenuItem)sender).Tag;

            OnPluginOpened(new PluginEventArgs(plugin));
        }

        private string EscapeMenuText(string name)
        {
            return name.Replace("&", "&&");
        }

        private void CleanMenu()
        {
            while (_pluginsMenuItem.MenuItems[0] != _pluginsSeparatorMenuItem)
            {
                _pluginsMenuItem.MenuItems[0].Dispose();
            }
        }

        protected virtual void OnPluginOpened(PluginEventArgs e)
        {
            PluginOpened?.Invoke(this, e);
        }
    }
}
