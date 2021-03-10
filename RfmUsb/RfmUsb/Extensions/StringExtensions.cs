using System;

namespace RfmUsb.Extensions
{
    public static class StringExtensions
    {
        public static int ConvertToInt32(this string value)
        {
            return Convert.ToInt32(value.Substring(0, 8).Trim('[', ']'), 16);
        }

        public static uint ConvertToUInt32(this string value)
        {
            return Convert.ToUInt32(value.Substring(0, 8).Trim('[', ']'), 16);
        }

        public static ushort ConvertToUInt16(this string value)
        {
            return Convert.ToUInt16(value.Substring(0, 8).Trim('[', ']'), 16);
        }

        public static byte ConvertToByte(this string value)
        {
            return Convert.ToByte(value.Substring(0, 8).Trim('[', ']'), 16);
        }
    }
}
