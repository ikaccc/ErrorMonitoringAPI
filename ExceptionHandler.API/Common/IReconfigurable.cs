using System;

namespace ExceptionHandler.API.Common
{
    public interface IReconfigurable<T>
    {
        T Reconfigure(T likeMe);

        event EventHandler Reconfigured;
    }
}
