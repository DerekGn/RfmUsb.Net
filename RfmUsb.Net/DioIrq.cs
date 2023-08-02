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

using System;

namespace RfmUsb.Net
{
    /// <summary>
    /// The Dio irq line interrupt bit
    /// </summary>
    [Flags]
    public enum DioIrq : byte
    {
        /// <summary>
        /// No Dio line set
        /// </summary>
        None,

        /// <summary>
        /// Dio0 set
        /// </summary>
        Dio0 = 2,

        /// <summary>
        /// Dio1 set
        /// </summary>
        Dio1 = 4,

        /// <summary>
        /// Dio1 set
        /// </summary>
        Dio2 = 8,

        /// <summary>
        /// Dio3 set
        /// </summary>
        Dio3 = 16,

        /// <summary>
        /// Dio3 set
        /// </summary>
        Dio4 = 32,

        /// <summary>
        /// Dio5 set
        /// </summary>
        Dio5 = 64
    }
}