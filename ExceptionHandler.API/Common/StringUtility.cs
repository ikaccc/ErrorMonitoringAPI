using System.Collections.Generic;
using System.Text;

namespace ExceptionHandler.API.Common
{
    public static class StringUtility
    {
        private const string truncationIndicator = "...";

        public static string Combine(IEnumerable<string> substrings, string substringSeparator)
        {
            if (substrings == null)
                return string.Empty;
            return string.Join(substringSeparator, substrings);
        }

        public static byte[] GetEncodingBytes(string input, Encoding encoding)
        {
            if (input == null)
                return new byte[0];
            return encoding.GetBytes(input);
        }

        public static int CalculateMaxEncodingBytes(string input, Encoding encoding)
        {
            if (input == null)
                return 0;
            return encoding.GetMaxByteCount(input.Length);
        }

        public static int CalculateExactEncodingBytes(string input, Encoding encoding)
        {
            if (input == null)
                return 0;
            return encoding.GetByteCount(input);
        }

        public static string Truncate(string input, Encoding encoding, int encodedBytesLimit)
        {
            if (input == null)
                return input;
            byte[] bytes = encoding.GetBytes(input);
            Assumption.AssertEqual<int>(bytes.Length, encoding.GetByteCount(input), "Length");
            if (bytes.Length <= encodedBytesLimit)
                return input;
            int byteCount = encoding.GetByteCount("...");
            int count = encodedBytesLimit - byteCount;
            if (count < 0)
                count = 0;
            string str = encoding.GetString(bytes, 0, count);
            if (count > 0 && (int)str[str.Length - 1] != (int)input[str.Length - 1])
                str = str.Substring(0, str.Length - 1);
            string s = str + "...";
            Assumption.AssertTrue(encoding.GetByteCount(s) <= encodedBytesLimit || encodedBytesLimit < byteCount, "truncatedInput");
            return s;
        }
    }
}
