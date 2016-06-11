using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Util
{
    internal static class FileAttributesExtensions
    {
        public static bool IsHidden(this FileAttributes self)
        {
            return (self & (FileAttributes.Hidden | FileAttributes.System)) != 0;
        }

        public static bool IsDirectory(this FileAttributes self)
        {
            return (self & FileAttributes.Directory) != 0;
        }

        public static bool IsFile(this FileAttributes self)
        {
            return !self.IsDirectory();
        }
    }
}
