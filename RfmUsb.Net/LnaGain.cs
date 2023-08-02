﻿/*
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
    /// LNA gain setting
    /// </summary>
    public enum LnaGain
    {
        /// <summary>
        /// Auto
        /// </summary>
        Auto,

        /// <summary>
        /// Max
        /// </summary>
        Max,

        /// <summary>
        /// Max - 6db
        /// </summary>
        MaxMinus6db,

        /// <summary>
        /// Max - 12db
        /// </summary>
        MaxMinus12db,

        /// <summary>
        /// Max - 12db
        /// </summary>
        MaxMinus24db,

        /// <summary>
        /// Max - 36db
        /// </summary>
        MaxMinus36db,

        /// <summary>
        /// Max - 48db
        /// </summary>
        MaxMinus48db
    }
}