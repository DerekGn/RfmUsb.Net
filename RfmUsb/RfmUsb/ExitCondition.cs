/*
* MIT License
*
* Copyright (c) 2021 Derek Goslin
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

namespace RfmUsb
{
    /// <summary>
    /// The interrupt condition for exiting the intermediate mode
    /// </summary>
    public enum ExitCondition
    {
        /// <summary>
        /// None (AutoModes Off)
        /// </summary>
        Off,
        /// <summary>
        /// Falling edge of FifoNotEmpty (i.e. FIFO empty)
        /// </summary>
        FifoEmpty,
        /// <summary>
        /// Rising edge of FifoLevel or Timeout
        /// </summary>
        FifoLevel,
        /// <summary>
        /// Rising edge of CrcOk or Timeout
        /// </summary>
        CrcOk,
        /// <summary>
        /// Rising edge of PayloadReady or Timeout
        /// </summary>
        PayloadReady,
        /// <summary>
        /// Rising edge of SyncAddress or Timeout
        /// </summary>
        SyncAddressMatch,
        /// <summary>
        /// Rising edge of PacketSent
        /// </summary>
        PacketSent,
        /// <summary>
        /// Rising edge of Timeout
        /// </summary>
        RxTimeout
    }
}