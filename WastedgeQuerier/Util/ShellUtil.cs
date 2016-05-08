using System;
using System.Collections.Generic;
using System.Text;

namespace WastedgeQuerier.Util
{
    public static class ShellUtil
    {
        public static void NotifyFileAssociationsChanged()
        {
            NativeMethods.SHChangeNotify(
                NativeMethods.SHCNE_ASSOCCHANGED, NativeMethods.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero
            );
        }
    }
}
