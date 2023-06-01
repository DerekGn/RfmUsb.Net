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
using System.Collections.Generic;

namespace RfmUsb.Net
{
    /// <summary>
    /// An Rfm device
    /// </summary>
    public interface IRfm : IDisposable
    {
        /// <summary>
        /// Get or set the radio Tx/Rx bit rate
        /// </summary>
        ushort BitRate { get; set; }

        /// <summary>
        /// Get or set the fifo data
        /// </summary>
        IEnumerable<byte> Fifo { get; set; }

        /// <summary>
        /// Get or set the frequency
        /// </summary>
        uint Frequency { get; set; }

        /// <summary>
        /// LNA gain setting
        /// </summary>
        LnaGain LnaGainSelect { get; set; }

        /// <summary>
        /// Enables overload current protection (OCP) for the PA
        /// </summary>
        bool OcpEnable { get; set; }

        /// <summary>
        /// Trimming of OCP current
        /// </summary>
        OcpTrim OcpTrim { get; set; }

        /// <summary>
        /// Rise/Fall time of ramp up/down in FSK
        /// </summary>
        PaRamp PaRamp { get; set; }

        /// <summary>
        /// Get the FrmUsb version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Close the connection to the RfmUsb device
        /// </summary>
        void Close();

        /// <summary>
        /// Enter the device bootloader
        /// </summary>
        void EnterBootloader();

        /// <summary>
        /// Get the <see cref="Dio"/> mapping configuration <see cref="DioMapping"/>
        /// </summary>
        /// <param name="dio">The <see cref="Dio"/></param>
        /// <returns>The <see cref="DioMapping"/></returns>
        DioMapping GetDioMapping(Dio dio);

        /// <summary>
        /// Open the RfmUsb device
        /// </summary>
        /// <param name="serialPort">The serial port</param>
        /// <param name="baudRate">The baud rate</param>
        void Open(string serialPort, int baudRate);

        /// <summary>
        /// Set the <see cref="Dio"/> mapping configuration <see cref="DioMapping"/>
        /// </summary>
        /// <param name="dio">The <see cref="Dio"/> configuration</param>
        /// <param name="mapping">The <see cref="DioMapping"/></param>
        void SetDioMapping(Dio dio, DioMapping mapping);

        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        void Transmit(IList<byte> data);

        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txTimeout">The transmit timeout</param>
        void Transmit(IList<byte> data, int txTimeout);

        /// <summary>
        /// Transmit a packet of data bytes and wait for a response
        /// </summary>
        /// <param name="data">The data to transmit</param>
        IList<byte> TransmitReceive(IList<byte> data);

        /// <summary>
        /// Transmit a packet of data bytes and wait for a response
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txTimeout">The timeout in milliseconds </param>
        /// <returns>The received packet bytes</returns>
        IList<byte> TransmitReceive(IList<byte> data, int txTimeout);

        /// <summary>
        /// Transmit a packet of data bytes and wait for a response
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txTimeout">The transmit timeout in milliseconds</param>
        /// <param name="rxTimeout">The receive timeout in milliseconds</param>
        IList<byte> TransmitReceive(IList<byte> data, int txTimeout, int rxTimeout);
    }
}