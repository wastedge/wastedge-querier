using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native.Object;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class InteropInstance : ObjectInstance
    {
        public InteropInstance(Engine engine)
            : base(engine)
        {
            Extensible = true;
        }
    }
}
