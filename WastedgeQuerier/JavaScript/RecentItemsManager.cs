using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal class RecentItemsManager
    {
        private readonly string _baseKey;

        public event RecentItemEventHandler Opened;

        public RecentItemsManager(string baseKey)
        {
            if (baseKey == null)
                throw new ArgumentNullException(nameof(baseKey));

            _baseKey = baseKey;
        }

        public void Add(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            path = Path.GetFullPath(path);

            var items = Load();
            items.Remove(path);
            items.Insert(0, path);

            using (var key = Program.BaseKey)
            {
                key.DeleteSubKeyTree(_baseKey, false);

                using (var subKey = key.CreateSubKey(_baseKey))
                {
                    for (int i = 0; i < Math.Min(items.Count, 10); i++)
                    {
                        subKey.SetValue(((char)('a' + i)).ToString(), items[i]);
                    }
                }
            }
        }

        public void Update(ToolStripMenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            menuItem.DropDownItems.Clear();

            var items = Load();

            for (int i = 0; i < items.Count; i++)
            {
                var childItem = new ToolStripMenuItem
                {
                    Text = (i == 9 ? "1&0" : "&" + (i + 1)) + " " + FormatPath(items[i]),
                    Tag = items[i]
                };

                childItem.Click += ChildItem_Click;

                menuItem.DropDownItems.Add(childItem);
            }

            menuItem.Enabled = items.Count > 0;
            if (!menuItem.Enabled)
                menuItem.DropDownItems.Add("dummy");
        }

        private void ChildItem_Click(object sender, EventArgs e)
        {
            OnOpened(new RecentItemEventArgs((string)((ToolStripMenuItem)sender).Tag));
        }

        private string FormatPath(string path)
        {
            var parts = path.Split(Path.DirectorySeparatorChar);

            int available = 60;
            int offset = -1;

            available -= parts[0].Length + 1 + 3; // Initial length + separator + ellipsis

            for (int i = parts.Length - 1; i >= 1; i--)
            {
                if (parts[i].Length + 1 < available)
                {
                    available -= parts[i].Length + 1;
                }
                else
                {
                    offset = i + 1;
                    break;
                }
            }

            if (offset == -1)
                return path;

            var sb = new StringBuilder()
                .Append(parts[0])
                .Append(Path.DirectorySeparatorChar)
                .Append("...");

            for (int i = offset; i < parts.Length; i++)
            {
                sb.Append(Path.DirectorySeparatorChar).Append(parts[i]);
            }

            return sb.ToString();
        }

        private List<string> Load()
        {
            using (var key = Program.BaseKey)
            using (var subKey = key.CreateSubKey(_baseKey))
            {
                return subKey
                    .GetValueNames()
                    .OrderBy(p => p)
                    .Select(p => subKey.GetValue(p) as string)
                    .Where(p => p != null)
                    .ToList();
            }
        }

        protected virtual void OnOpened(RecentItemEventArgs e)
        {
            Opened?.Invoke(this, e);
        }
    }
}
