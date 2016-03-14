using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal class Continuation : IDisposable
    {
        private bool _disposed;
        private ManualResetEvent _event = new ManualResetEvent(false);

        public void Signal()
        {
            _event.Set();
        }

        public void Wait()
        {
            _event.WaitOne();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_event != null)
                {
                    _event.Close();
                    _event = null;
                }

                _disposed = true;
            }
        }
    }
}
