using System;

namespace ExceptionHandler.API.Common
{
    public interface IReconfigurable<T, TBase> where T : TBase
    {
        T Reconfigure(TBase likeMe);

        event EventHandler Reconfigured;
    }
}
