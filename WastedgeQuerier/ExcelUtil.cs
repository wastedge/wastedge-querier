using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WastedgeQuerier
{
    internal static class ExcelUtil
    {
        public static string GetDateFormat()
        {
            return GetDateFormat(false);
        }

        public static string GetDateTimeFormat()
        {
            return GetDateFormat(true);
        }

        private static string GetDateFormat(bool withTime)
        {
            var dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;

            string format = dateTimeFormat.ShortDatePattern;

            if (withTime)
                format += " " + dateTimeFormat.ShortTimePattern;

            if (format.EndsWith(" tt") || format.EndsWith(" TT"))
                format = format.Substring(0, format.Length - 2) + "AM/PM";

            return format;
        }
    }
}
