using System;
using System.Collections.Generic;
using System.Text;

namespace WastedgeQuerier.Util
{
    public class ShellRegistrationConfiguration
    {
        public string ExecutableFileName { get; set; }
        public string FriendlyAppName { get; set; }
        public string FriendlyTypeName { get; set; }
        public string DefaultIcon { get; set; }
        public int? DefaultIconIndex { get; set; }
        public string OpenCommandArguments { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public string PerceivedType { get; set; }
    }
}
