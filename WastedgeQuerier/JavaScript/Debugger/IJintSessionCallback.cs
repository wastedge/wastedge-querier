using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Debugger;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal interface IJintSessionCallback
    {
        Continuation ProcessStep(JintEngine engine, DebugInformation debugInformation, BreakType breakType);
    }
}
