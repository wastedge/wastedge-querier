using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal static class Inflector
    {
        public static string Camelize(string value)
        {
            return Camelize(value, true);
        }

        public static string Camelize(string value, bool firstLetterUppercase)
        {
            if (firstLetterUppercase)
            {
                return
                    Regex.Replace(
                        Regex.Replace(value, "/(.?)", p => "::" + p.Groups[1].Value.ToUpperInvariant()),
                        "(?:^|_)(.)", p => p.Groups[1].Value.ToUpperInvariant()
                    );
            }
            else
            {
                return
                    value.Substring(0, 1).ToLowerInvariant() +
                    Camelize(value.Substring(1));
            }
        }

        public static string Underscore(string value)
        {
            value = value.Replace("::", "/");
            value = Regex.Replace(value, "([A-Z]+)([A-Z][a-z])", p => p.Groups[1].Value + "_" + p.Groups[2].Value);
            value = Regex.Replace(value, "([a-z\\d])([A-Z])", p => p.Groups[1].Value + "_" + p.Groups[2].Value);
            value = value.Replace("-", "_");

            return value.ToLowerInvariant();
        }
    }
}
