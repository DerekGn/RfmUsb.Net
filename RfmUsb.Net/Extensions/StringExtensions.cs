/*
* MIT License
*
* Copyright (c) 2023 Derek Goslin
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using RfmUsb.Net.Exceptions;
using System;

namespace RfmUsb.Net.Extensions
{
    internal static class StringExtensions
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
