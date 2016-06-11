using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Util
{
    internal static class HumanText
    {
        public static string ToHuman(string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            text = text.Replace('_', ' ');

            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }
    }
}
