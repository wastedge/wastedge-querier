using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Native.Array;

namespace WastedgeQuerier.JavaScript
{
    internal static class JintExtensions
    {
        public static void ForEach(this ArrayInstance self, Action<int, JsValue> callback)
        {
            for (int i = 0;; i++)
            {
                string index = i.ToString();
                if (!self.HasOwnProperty(index))
                    break;

                callback(i, self.Get(index));
            }
        }
    }
}
