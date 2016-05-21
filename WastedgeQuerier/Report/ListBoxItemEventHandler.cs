using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.Report
{
    internal class ListBoxItemEventArgs : EventArgs
    {
        public MouseButtons Button { get; }
        public int Index { get; }

        public ListBoxItemEventArgs(MouseButtons button, int index)
        {
            Button = button;
            Index = index;
        }
    }

    internal delegate void ListBoxItemEventHandler(object sender, ListBoxItemEventArgs e);
}
