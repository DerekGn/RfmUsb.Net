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

namespace RfmUsb.Net
{
    /// <summary>
    /// Period of decrement of the RSSI threshold in the OOK demodulator
    /// </summary>
    public enum OokThresholdDec
    {
        /// <summary>
        /// Once every chip
        /// </summary>
        OncePerChip,

        /// <summary>
        /// Once every 2 chips
        /// </summary>
        OnceEvery2Chips,

        /// <summary>
        /// Once every 4 chips
        /// </summary>
        OnceEvery4Chips,

        /// <summary>
        /// Once every 8 chips
        /// </summary>
        OnceEvery8Chips,

        /// <summary>
        /// 2 times in each chip
        /// </summary>
        TwiceInEachChip,

        /// <summary>
        /// 4 times in each chip
        /// </summary>
        FourTimesInEachChip,

        /// <summary>
        /// 8 times in each chip
        /// </summary>
        EightTimesInEachChip,

        /// <summary>
        /// 16 times in each chip
        /// </summary>
        SixteeenTimesInEachChip
    }
}