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


// Ignore Spelling: Irq Rfm

using System;

namespace RfmUsb.Net
{
    /// <summary>
    /// The set of <see cref="Rfm69"/> Irq bits
    /// </summary>
    [Flags]
    public enum Rfm69IrqFlags
    {
        /// <summary>
        /// No Irq bit is set
        /// </summary>
        None = 0,

        /// <summary>
        /// Set in Rx when the CRC of the payload is Ok. Cleared when FIFO is empty.
        /// </summary>
        CrcOK = 0x0002,

        /// <summary>
        /// Set in Rx when the payload is ready (i.e. last byte received and CRC,
        /// if enabled and CrcAutoClearOff is cleared,is Ok). Cleared when FIFO is empty.
        /// </summary>
        PayloadReady = 0x0004,

        /// <summary>
        /// Set in Tx when the complete packet has been sent. Cleared when exiting Tx.
        /// </summary>
        PacketSent = 0x0008,

        /// <summary>
        /// Set when FIFO overrun occurs. (except in Sleep mode) Flag(s) and FIFO are cleared when this bit is set.
        /// The FIFO then becomes immediately available for the next transmission/reception
        /// </summary>
        FifoOverrun = 0x0010,

        /// <summary>
        /// Set when the number of bytes in the FIFO strictly exceeds FifoThreshold, else cleared.
        /// </summary>
        FifoLevel = 0x0020,

        /// <summary>
        /// Set when FIFO contains at least one byte, else cleared
        /// </summary>
        FifoNotEmpty = 0x0040,

        /// <summary>
        /// Set when FIFO is full (i.e. contains 66 bytes), else cleared
        /// </summary>
        FifoFull = 0x0080,

        /// <summary>
        /// Set when Sync and Address (if enabled) are detected. Cleared when leaving Rx or FIFO is emptied.
        /// This bit is read only in Packet mode, rwc in Continuous mode
        /// </summary>
        SyncAddressMatch = 0x0100,

        /// <summary>
        /// Set when entering Intermediate mode. Cleared when exiting
        /// Intermediate mode. Please note that in Sleep mode a small
        /// delay can be observed between AutoMode interrupt and the
        /// corresponding enter/exit condition.
        /// </summary>
        AutoMode = 0x0200,

        /// <summary>
        /// Set when a timeout occurs (see TimeoutRxStart and TimeoutRssiThresh) Cleared when leaving Rx or FIFO is emptied.
        /// </summary>
        Timeout = 0x0400,

        /// <summary>
        /// Set in Rx when the RssiValue exceeds RssiThreshold. Cleared when leaving Rx.
        /// </summary>
        Rssi = 0x0800,

        /// <summary>
        /// Set (in FS, Rx or Tx) when the PLL is locked. Cleared when it is not.
        /// </summary>
        PllLock = 0x1000,

        /// <summary>
        /// Set in Tx mode, after PA ramp-up. Cleared when leaving Tx.
        /// </summary>
        TxReady = 0x2000,

        /// <summary>
        /// Set in Rx mode, after RSSI, AGC and AFC. Cleared when leaving Rx.
        /// </summary>
        RxReady = 0x4000,

        /// <summary>
        /// Set when the operation mode requested in Mode, is ready
        /// - Sleep: Entering Sleep mode
        /// - Standby: XO is running
        /// - FS: PLL is locked
        /// - Rx: RSSI sampling starts
        /// - Tx: PA ramp-up completed
        /// Cleared when changing operating mode.
        /// </summary>
        ModeReady = 0x8000,
    }
}