using RfmUsb.Exceptions;
using System;

namespace RfmUsb.Extensions
{
    public static class StringExtensions
    {
        public static int ConvertToInt32(this string value)
        {
            return ParseAndConvert(s => Convert.ToInt32(s, 16), value);
        }

        public static uint ConvertToUInt32(this string value)
        {
            return ParseAndConvert(s => Convert.ToUInt32(s, 16), value);
        }

        public static ushort ConvertToUInt16(this string value)
        {
            return ParseAndConvert(s => Convert.ToUInt16(s, 16), value);
        }

        public static byte ConvertToByte(this string value)
        {
            return ParseAndConvert(s => Convert.ToByte(s, 16), value);
        }

        private static T ParseAndConvert<T>(Func<string,T> convertOperation, string value)
        {
            var elements = value.Split('-');

            if (elements.Length >= 1)
            {
                return convertOperation(elements[0]);
            }
            else
            {
                throw new RfmUsbCommandExecutionException($"Unable to parse result value [{value}]");
            }
        }
    }
}
