using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Util
{
    internal static class CommandLineUtil
    {
        public static string[] Parse(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine))
                return new string[0];

            var args = new List<string>();
            var sb = new StringBuilder();
            bool inString = false;
            bool lastQuote = false;
            bool hadAny = false;

            foreach (char c in commandLine)
            {
                if (c == '"')
                {
                    hadAny = true;

                    if (inString)
                    {
                        inString = false;
                        lastQuote = true;
                        continue;
                    }
                    inString = true;
                    if (lastQuote)
                        sb.Append('"');
                }
                else if (!inString && Char.IsWhiteSpace(c))
                {
                    if (hadAny)
                        args.Add(sb.ToString());
                    hadAny = false;
                    sb.Length = 0;
                }
                else
                {
                    sb.Append(c);
                    hadAny = true;
                }

                lastQuote = false;
            }

            if (hadAny)
                args.Add(sb.ToString());

            return args.ToArray();
        }
    }
}
