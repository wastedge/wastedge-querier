using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript
{
    internal class RecentItemEventArgs : EventArgs
    {
        public string Path { get; }

        public RecentItemEventArgs(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Path = path;
        }
    }

    internal delegate void RecentItemEventHandler(object sender, RecentItemEventArgs e);
}
