using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal class JintDebuggerSteppedEventArgs
    {
        public JintEngine Engine { get; }
        public BreakType BreakType { get; }

        public JintDebuggerSteppedEventArgs(JintEngine engine, BreakType breakType)
        {
            Engine = engine;
            BreakType = breakType;
        }
    }

    internal delegate void JintDebuggerSteppedEventHandler(object sender, JintDebuggerSteppedEventArgs e);
}
