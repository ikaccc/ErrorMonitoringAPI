using System;

namespace ExceptionHandler.API.Common
{
    public static class TUtil
    {
        public static object OptionalParam<T>(this T value, T optionalValue = default(T))
        {
            return Equals(value, optionalValue) ? (object)DBNull.Value : value;
        }
    }
}
