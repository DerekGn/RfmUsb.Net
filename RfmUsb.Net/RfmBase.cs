using Microsoft.Extensions.Logging;
using RfmUsb.Exceptions;
using RfmUsb.Ports;
using System;
using System.IO;

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
    /// A base Rfm class
    /// </summary>
    public abstract class RfmBase : IRfm
    {
        internal const string ResponseOk = "OK";
        protected internal ISerialPort SerialPort;
        protected readonly ILogger<IRfm> Logger;
        protected readonly ISerialPortFactory SerialPortFactory;

        /// <summary>
        /// Create an instance of an <see cref="RfmBase"/> derived type.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{T}"/> instance</param>
        /// <param name="serialPortFactory">The <see cref="SerialPortFactory"/> instance</param>
        /// <exception cref="ArgumentNullException">if any parameter is null</exception>
        protected RfmBase(ILogger<IRfm> logger, ISerialPortFactory serialPortFactory)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SerialPortFactory = serialPortFactory ?? throw new ArgumentNullException(nameof(serialPortFactory));
        }

        ///<inheritdoc/>
        public void Close()
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Close();
            }
        }

        ///<inheritdoc/>
        public void Open(string serialPort, int baudRate)
        {
            try
            {
                if (SerialPort == null)
                {
                    SerialPort = SerialPortFactory.CreateSerialPortInstance(serialPort);

                    SerialPort.BaudRate = baudRate;
                    SerialPort.NewLine = "\r\n";
                    SerialPort.DtrEnable = true;
                    SerialPort.RtsEnable = true;
                    SerialPort.ReadTimeout = 500;
                    SerialPort.WriteTimeout = 500;
                    SerialPort.Open();
                }
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogDebug(ex, "Exception occurred opening serial port");

                throw new RfmUsbSerialPortNotFoundException(
                    $"Unable to open serial port [{serialPort}] Reason: [{ex.Message}]. " +
                    $"Available Serial Ports: [{string.Join(", ", SerialPortFactory.GetSerialPorts())}]");
            }
        }

        #region IDisposible

        private bool disposedValue;

        /// <summary>
        /// Dispose the <see cref="IRfm"/> instance
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the <see cref="IRfm"/> instance
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SerialPort?.Close();
                }

                disposedValue = true;
            }
        }

        #endregion IDisposible
    }
}