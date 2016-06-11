using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    internal class CopyDataTarget : Form
    {
        public event CopyDataEventHandler DataCopied;

        private const string Id = "0308a1a1-f4c1-4240-b490-3bf0bd74f25f";

        public CopyDataTarget()
        {
            Text = Id;
        }

        protected override void SetVisibleCore(bool value)
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_COPYDATA)
                DoCopyData(ref m);

            base.WndProc(ref m);
        }

        private void DoCopyData(ref Message m)
        {
            var copyData = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.COPYDATASTRUCT));
            string data = Marshal.PtrToStringUni(copyData.lpData, copyData.cbData / 2);

            OnDataCopied(new CopyDataEventArgs(data));
        }

        public static bool SendData(string data)
        {
            var handle = NativeMethods.FindWindow(null, Id);
            if (handle == IntPtr.Zero)
                return false;

            var buffer = Marshal.StringToHGlobalUni(data ?? "");
            var copyData = new NativeMethods.COPYDATASTRUCT();
            copyData.dwData = IntPtr.Zero;
            copyData.lpData = buffer;
            copyData.cbData = data?.Length * 2 ?? 0;
            var copyDataBuffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
            Marshal.StructureToPtr(copyData, copyDataBuffer, false);
            NativeMethods.SendMessage(handle, NativeMethods.WM_COPYDATA, IntPtr.Zero, copyDataBuffer);
            Marshal.FreeHGlobal(copyDataBuffer);
            Marshal.FreeHGlobal(buffer);

            return true;
        }

        private void OnDataCopied(CopyDataEventArgs e)
        {
            DataCopied?.Invoke(this, e);
        }
    }
}
