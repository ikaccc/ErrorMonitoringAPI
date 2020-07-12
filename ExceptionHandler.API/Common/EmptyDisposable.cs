using System;

namespace ExceptionHandler.API.Common
{
    public sealed class EmptyDisposable : IDisposable
    {
        private EmptyDisposable()
        {
        }

        public static EmptyDisposable Instance => EmptyDisposable.NestedSingleInstance.Instance;

        public void Dispose()
        {
        }

        private sealed class NestedSingleInstance
        {
            internal static readonly EmptyDisposable Instance = new EmptyDisposable();
        }
    }
}
