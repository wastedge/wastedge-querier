using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.ComTypes;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace WastedgeQuerier.Util
{
    internal static class NativeMethods
    {
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("shell32")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("ole32", PreserveSig = false)]
        public static extern ILockBytes CreateILockBytesOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease);

        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("ole32", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IStorage StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode, uint reserved);

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const uint SHCNE_ASSOCCHANGED = 0x08000000;
        public const uint SHCNF_IDLIST = 0x0000;

        public const int WM_COPYDATA = 0x004A;
        public const int WM_MOUSEACTIVATE = 0x21;

        public const uint WS_POPUP = 0x80000000;
        public const uint WS_CLIPSIBLINGS = 0x04000000;
        public const uint WS_CLIPCHILDREN = 0x02000000;
        public const uint WS_BORDER = 0x00800000;

        public const uint WS_EX_LEFT = 0x00000000;
        public const uint WS_EX_LTRREADING = 0x00000000;
        public const uint WS_EX_TOPMOST = 0x00000008;
        public const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const uint WS_EX_TOOLWINDOW = 0x00000080;

        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;

        public const int MA_NOACTIVATEANDEAT = 4;

        [StructLayout(LayoutKind.Sequential)]
        public sealed class POINTL
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public sealed class SIZEL
        {
            public int cx;
            public int cy;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public sealed class FILEDESCRIPTORA
        {
            public uint dwFlags;
            public Guid clsid;
            public SIZEL sizel;
            public POINTL pointl;
            public uint dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public sealed class FILEDESCRIPTORW
        {
            public uint dwFlags;
            public Guid clsid;
            public SIZEL sizel;
            public POINTL pointl;
            public uint dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000B-0000-0000-C000-000000000046")]
        public interface IStorage
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream CreateStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream OpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr reserved1, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
            [return: MarshalAs(UnmanagedType.Interface)]
            IStorage CreateStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
            [return: MarshalAs(UnmanagedType.Interface)]
            IStorage OpenStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr pstgPriority, [In, MarshalAs(UnmanagedType.U4)] int grfMode, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.U4)] int reserved);
            void CopyTo(int ciidExclude, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] pIIDExclude, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest);
            void MoveElementTo([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName, [In, MarshalAs(UnmanagedType.U4)] int grfFlags);
            void Commit(int grfCommitFlags);
            void Revert();
            void EnumElements([In, MarshalAs(UnmanagedType.U4)] int reserved1, IntPtr reserved2, [In, MarshalAs(UnmanagedType.U4)] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
            void DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);
            void RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);
            void SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] FILETIME pctime, [In] FILETIME patime, [In] FILETIME pmtime);
            void SetClass([In] ref Guid clsid);
            void SetStateBits(int grfStateBits, int grfMask);
            void Stat([Out]out STATSTG pStatStg, int grfStatFlag);
        }

        [ComImport, Guid("0000000A-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ILockBytes
        {
            void ReadAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbRead);
            void WriteAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbWritten);
            void Flush();
            void SetSize([In, MarshalAs(UnmanagedType.U8)] long cb);
            void LockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
            void UnlockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
            void Stat([Out]out STATSTG pstatstg, [In, MarshalAs(UnmanagedType.U4)] int grfStatFlag);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;    // Any value the sender chooses.  Perhaps its main window handle?
            public int cbData;       // The count of bytes in the message.
            public IntPtr lpData;    // The address of the message.
        }
    }
}
