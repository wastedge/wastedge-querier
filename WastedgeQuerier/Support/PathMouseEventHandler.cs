using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.Support
{
    public class PathMouseEventArgs : MouseEventArgs
    {
        public string Path { get; }

        public PathMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta, string path)
            : base(button, clicks, x, y, delta)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Path = path;
        }
    }

    public delegate void PathMouseEventHandler(object sender, PathMouseEventArgs e);
}
