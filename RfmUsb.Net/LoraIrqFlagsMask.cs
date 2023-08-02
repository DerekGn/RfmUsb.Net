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
    /// THe lora irq flags mask
    /// </summary>
    [Flags]
    public enum LoraIrqFlagsMask
    {
        /// <summary>
        /// No irq mask flags are set
        /// </summary>
        None,
        /// <summary>
        /// Cad Detected Interrupt Mask: setting this bit masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        CadDetectedMask = 0x01,

        /// <summary>
        /// FHSS change channel interrupt mask: setting this bit masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        FhssChangeChannelMask = 0x02,

        /// <summary>
        /// CAD complete interrupt mask: setting this bit masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        CadDoneMask = 0x04,

        /// <summary>
        /// FIFO Payload transmission complete interrupt mask: setting this bit Masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        TxDoneMask = 0x08,

        /// <summary>
        /// Valid header received in Rx Mask: setting this bit masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        ValidHeaderMask = 0x10,

        /// <summary>
        /// Payload CRC error interrupt Mask: setting this bit masks the  corresponding IRQ in RegIrqFlags
        /// </summary>
        PayloadCrcErrorMask = 0x20,

        /// <summary>
        /// Packet reception complete interrupt Mask: setting this bit masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        RxDoneMask = 0x40,

        /// <summary>
        /// Timeout interrupt Mask: setting this bit masks the corresponding IRQ in RegIrqFlags
        /// </summary>
        RxTimeoutMask = 0x80
    }
}