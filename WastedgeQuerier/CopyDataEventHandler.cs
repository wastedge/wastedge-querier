using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier
{
    internal class CopyDataEventArgs : EventArgs
    {
        public string Data { get; }

        public CopyDataEventArgs(string data)
        {
            Data = data;
        }
    }

    internal delegate void CopyDataEventHandler(object sender, CopyDataEventArgs e);
}
