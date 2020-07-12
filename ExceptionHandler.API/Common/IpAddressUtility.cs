namespace ExceptionHandler.API.Common
{
    public static class IpAddressUtility
    {
        public static string Anonymize(string exactIpAddress)
        {
            if (string.IsNullOrEmpty(exactIpAddress))
                return exactIpAddress;
            string[] strArray1 = exactIpAddress.Split('.');
            if (strArray1 != null && strArray1.Length == 4)
            {
                strArray1[3] = "0/24";
                return string.Join(string.Format("{0}", (object)'.'), strArray1);
            }
            string[] strArray2 = exactIpAddress.Split(':');
            if (strArray2 == null || strArray2.Length == 0)
                return (string)null;
            if (exactIpAddress.Length > 12)
                return exactIpAddress.Substring(0, 12) + "...";
            return exactIpAddress;
        }
    }
}
