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
    /// The lora modem bandwidth
    /// </summary>
    public enum ModemBandwidth
    {
        /// <summary>
        /// 7.8 kHz Bandwidth
        /// </summary>
        Bandwidth7_8KHZ,

        /// <summary>
        /// 10.4 kHz Bandwidth
        /// </summary>
        Bandwidth10_4KHZ,

        /// <summary>
        /// 15.6 kHz Bandwidth
        /// </summary>
        Bandwidth15_6KHZ,

        /// <summary>
        /// 20.8kHz Bandwidth
        /// </summary>
        Bandwidth20_8KHZ,

        /// <summary>
        /// 31.25 kHz Bandwidth
        /// </summary>
        Bandwidth31_25KHZ,

        /// <summary>
        /// 41.7 kHz Bandwidth
        /// </summary>
        Bandwidth41_7KHZ,

        /// <summary>
        /// 62.5 kHz Bandwidth
        /// </summary>
        Bandwidth62_5KHZ,

        /// <summary>
        /// 125 kHz Bandwidth
        /// </summary>
        Bandwidth125KHZ,

        /// <summary>
        /// 250 kHz Bandwidth
        /// </summary>
        Bandwidth250KHZ,

        /// <summary>
        /// 500 KHz Bandwidth
        /// </summary>
        Bandwidth500KHZ
    }
}