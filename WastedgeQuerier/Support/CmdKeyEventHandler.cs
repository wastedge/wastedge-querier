using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.Support
{
    internal class CmdKeyEventArgs : EventArgs
    {
        public Keys KeyData { get; }
        public bool Handled { get; set; }

        public CmdKeyEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }
    }

    internal delegate void CmdKeyEventHandler(object sender, CmdKeyEventArgs e);
}
