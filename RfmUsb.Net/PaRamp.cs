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
    /// Rise/Fall time of ramp up/down in FSK
    /// </summary>
    public enum PaRamp
    {
        /// <summary>
        /// 3.4 ms
        /// </summary>
        PowerAmpRamp3400,

        /// <summary>
        /// 2 ms
        /// </summary>
        PowerAmpRamp2000,

        /// <summary>
        /// 1 us
        /// </summary>
        PowerAmpRamp1000,

        /// <summary>
        /// 500 us
        /// </summary>
        PowerAmpRamp500,

        /// <summary>
        /// 250 us
        /// </summary>
        PowerAmpRamp250,

        /// <summary>
        /// 125 us
        /// </summary>
        PowerAmpRamp125,

        /// <summary>
        /// 100 us
        /// </summary>
        PowerAmpRamp100,

        /// <summary>
        /// 62 us
        /// </summary>
        PowerAmpRamp62,

        /// <summary>
        /// 50 us
        /// </summary>
        PowerAmpRamp50,

        /// <summary>
        /// 40 us
        /// </summary>
        PowerAmpRamp40,

        /// <summary>
        /// 31 us
        /// </summary>
        PowerAmpRamp31,

        /// <summary>
        /// 25 us
        /// </summary>
        PowerAmpRamp25,

        /// <summary>
        /// 20 us
        /// </summary>
        PowerAmpRamp20,

        /// <summary>
        /// 15 us
        /// </summary>
        PowerAmpRamp15,

        /// <summary>
        /// 12 us
        /// </summary>
        PowerAmpRamp12,

        /// <summary>
        /// 10 us
        /// </summary>
        PowerAmpRamp10
    }
}