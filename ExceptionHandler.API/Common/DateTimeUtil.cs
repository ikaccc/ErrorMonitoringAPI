using System;

namespace ExceptionHandler.API.Common
{
    public static class DateTimeUtil
    {
        public static long ConvertToUnixTimestampInMilliseconds(DateTime dateTime)
        {
            return Convert.ToInt64(dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
        }

        public static long ConvertToUnixTimestampInSeconds(DateTime dateTime)
        {
            return Convert.ToInt64(dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public static DateTimeOffset ConvertFromUnixTimestampInSeconds(long unixTimestampInSeconds)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero);
            dateTimeOffset = dateTimeOffset.AddSeconds(Convert.ToDouble(unixTimestampInSeconds));
            return dateTimeOffset;
        }
    }
}
