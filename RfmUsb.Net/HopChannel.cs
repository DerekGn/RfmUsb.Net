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

// Ignore Spelling: pll

namespace RfmUsb.Net
{
    /// <summary>
    /// The FHSS hop channel information
    /// </summary>
    public struct HopChannel
    {
        /// <summary>
        /// Create a <see cref="HopChannel"/> instance
        /// </summary>
        /// <param name="pllTimeout"></param>
        /// <param name="crcOnPayload"></param>
        /// <param name="channel"></param>
        public HopChannel(bool pllTimeout, bool crcOnPayload, byte channel)
        {
            PllTimeout = pllTimeout;
            CrcOnPayload = crcOnPayload;
            Channel = channel;
        }

        /// <summary>
        /// PLL failed to lock while attempting a TX/RX/CAD operation
        /// </summary>
        /// <remarks>
        /// true : PLL did not lock
        /// false : PLL did lock
        /// </remarks>
        public bool PllTimeout { get; }

        /// <summary>
        /// CRC Information extracted from the received packet header (Explicit header mode only)
        /// </summary>
        /// <remarks>
        /// false : Header indicates CRC off
        /// true : Header indicates CRC on
        /// </remarks>
        public bool CrcOnPayload { get; }

        /// <summary>
        /// Current value of frequency hopping channel in use
        /// </summary>
        public byte Channel { get; }
    }
}