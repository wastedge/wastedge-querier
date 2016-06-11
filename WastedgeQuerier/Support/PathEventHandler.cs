using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Support
{
    public class PathEventArgs : EventArgs
    {
        public string Path { get; }

        public PathEventArgs(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Path = path;
        }
    }

    public delegate void PathEventHandler(object sender, PathEventArgs e);
}
