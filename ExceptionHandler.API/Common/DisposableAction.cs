using System;

namespace ExceptionHandler.API.Common
{
    public class DisposableAction : IDisposable
    {
        private Action _action;
        private bool disposedValue;

        public DisposableAction(Action action)
        {
            this._action = action;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
                return;
            if (disposing && this._action != null)
            {
                this._action();
                this._action = (Action)null;
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
