using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript
{
    internal class Disposer : IDisposable
    {
        private bool _disposed;
        private readonly Action _action;

        public Disposer(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _action = action;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _action();
                _disposed = true;
            }
        }
    }
}
