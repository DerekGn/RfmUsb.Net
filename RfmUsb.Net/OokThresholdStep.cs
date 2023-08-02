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
    /// Size of each decrement of the RSSI threshold in the OOKdemodulator
    /// </summary>
    public enum OokThresholdStep
    {
        /// <summary>
        /// 0.5 db
        /// </summary>
        Step0_5db,

        /// <summary>
        /// 1 db
        /// </summary>
        Step1db,

        /// <summary>
        /// 1.5 db
        /// </summary>
        Step1_5db,

        /// <summary>
        /// 2 db
        /// </summary>
        Step2db,

        /// <summary>
        /// 3 db
        /// </summary>
        Step3db,

        /// <summary>
        /// 4 db
        /// </summary>
        Step4db,

        /// <summary>
        /// 5 db
        /// </summary>
        Step5db,

        /// <summary>
        /// 6 db
        /// </summary>
        Step6db,
    }
}