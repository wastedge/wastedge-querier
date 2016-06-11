using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Support
{
    internal static class DropMatchKindExtensions
    {
        public static bool IsDirectory(this DropMatchKind self)
        {
            switch (self)
            {
                case DropMatchKind.DirectoryMove:
                case DropMatchKind.DirectoryCopy:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsFile(this DropMatchKind self)
        {
            return !self.IsDirectory();
        }

        public static bool IsMove(this DropMatchKind self)
        {
            switch (self)
            {
                case DropMatchKind.DirectoryMove:
                case DropMatchKind.FileMove:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsCopy(this DropMatchKind self)
        {
            switch (self)
            {
                case DropMatchKind.DirectoryCopy:
                case DropMatchKind.FileCopy:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsVirtual(this DropMatchKind self)
        {
            return self == DropMatchKind.FileVirtual;
        }
    }
}
