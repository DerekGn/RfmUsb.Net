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

using Microsoft.Extensions.Logging;
using RfmUsb.Net.Exceptions;
using RfmUsb.Net.Extensions;
using RfmUsb.Net.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RfmUsb.Net
{
    /// <summary>
    /// A base Rfm class
    /// </summary>
    public abstract class RfmBase : IRfm
    {
        internal const string ResponseOk = "OK";
        internal readonly ILogger<IRfm> Logger;
        internal readonly ISerialPortFactory SerialPortFactory;
        internal ISerialPort SerialPort;

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
        public ushort BitRate
        {
            get => SendCommand(Commands.GetBitRate).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetBitRate} 0x{(int)value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public IEnumerable<byte> Fifo
        {
            get => SendCommand(Commands.GetFifo).ToBytes();
            set => SendCommandWithCheck($"{Commands.SetFifo} {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        ///<inheritdoc/>
        public uint Frequency
        {
            get => Convert.ToUInt32(SendCommand(Commands.GetFrequency).Trim('[', ']'), 16);
            set => SendCommandWithCheck($"{Commands.SetFrequency} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public LnaGain LnaGainSelect
        {
            get => (LnaGain)SendCommand(Commands.GetLnaGainSelect).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetLnaGainSelect} 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool OcpEnable
        {
            get => SendCommand(Commands.GetOcpEnable).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetOcpEnable} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public OcpTrim OcpTrim
        {
            get => (OcpTrim)SendCommand(Commands.GetOcpTrim).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOcpTrim} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public PaRamp PaRamp
        {
            get => (PaRamp)SendCommand(Commands.GetPaRamp).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetPaRamp} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public string Version => SendCommand(Commands.GetVersion);

        ///<inheritdoc/>
        public void Close()
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Close();
            }
        }

        ///<inheritdoc/>
        public void EnterBootloader()
        {
            SendCommand(Commands.ExecuteBootloader);
        }

        ///<inheritdoc/>
        public DioMapping GetDioMapping(Dio dio)
        {
            var result = SendCommand($"{Commands.GetDioMapping} 0x{(byte)dio:X}");

            var parts = result.Split('-');

            if (parts.Length >= 2)
            {
                var subParts = parts[1].Split(' ');

                if (subParts.Length >= 2)
                {
                    return (DioMapping)Convert.ToInt32(subParts[1]);
                }
            }

            throw new RfmUsbCommandExecutionException($"Invalid response [{result}]");
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

        ///<inheritdoc/>
        public void Reset()
        {
            FlushSerialPort();
            SendCommandWithCheck(Commands.ExecuteReset, ResponseOk);
        }

        ///<inheritdoc/>
        public void SetDioMapping(Dio dio, DioMapping mapping)
        {
            SendCommandWithCheck($"{Commands.SetDioMapping} {(int)dio} {(int)mapping}", ResponseOk);
        }
        ///<inheritdoc/>
        public void Transmit(IList<byte> data)
        {
            TransmitInternal($"{Commands.ExecuteTransmit} {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)}");
        }

        ///<inheritdoc/>
        public void Transmit(IList<byte> data, int txTimeout)
        {
            TransmitInternal($"{Commands.ExecuteTransmit} {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)} {txTimeout}");
        }

        ///<inheritdoc/>
        public IList<byte> TransmitReceive(IList<byte> data)
        {
            return TransmitReceiveInternal($"{Commands.ExecuteTransmitReceive} {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)}");
        }

        ///<inheritdoc/>
        public IList<byte> TransmitReceive(IList<byte> data, int txTimeout)
        {
            return TransmitReceiveInternal($"{Commands.ExecuteTransmitReceive} {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)} {txTimeout}");
        }

        ///<inheritdoc/>
        public IList<byte> TransmitReceive(IList<byte> data, int txTimeout, int rxTimeout)
        {
            return TransmitReceiveInternal($"{Commands.ExecuteTransmitReceive} {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)} {txTimeout} {rxTimeout}");
        }

        internal void FlushSerialPort()
        {
            if (SerialPort != null)
            {
                Logger.LogDebug("Flushing Serial Ports input buffer");
                SerialPort.DiscardInBuffer();
            }
        }

        internal string SendCommand(string command)
        {
            CheckOpen();

            lock (SerialPort)
            {
                SerialPort.Write($"{command}\n");

                var response = SerialPort.ReadLine();

                Logger.LogDebug($"Command: [{command}] Result: [{response}]");

                return response;
            }
        }

        internal void SendCommandWithCheck(string command, string response)
        {
            CheckOpen();

            var result = SendCommand(command);

            if (!result.StartsWith(response))
            {
                throw new RfmUsbCommandExecutionException($"Command: [{command}] Execution Failed Reason: [{result}]");
            }
        }

        private void CheckOpen()
        {
            if (SerialPort == null)
            {
                throw new InvalidOperationException("Instance not open");
            }
        }

        private void TransmitInternal(string command)
        {
            lock (SerialPort)
            {
                var response = SendCommand(command);

                if (response.StartsWith("DIO"))
                {
                    response = SerialPort.ReadLine();
                    Logger.LogDebug("Response: [{response}]", response);
                }

                if (response.Contains("TX") || response.Contains("RX"))
                {
                    throw new RfmUsbTransmitException($"Packet transmission failed: [{response}]");
                }
            }
        }

        private IList<byte> TransmitReceiveInternal(string command)
        {
            lock (SerialPort)
            {
                var response = SendCommand(command);

                if (response.StartsWith("DIO"))
                {
                    Logger.LogDebug("Response: [{response}]", response);
                    response = SerialPort.ReadLine();
                }

                if (response.Contains("TX") || response.Contains("RX"))
                {
                    throw new RfmUsbTransmitException($"Packet transmission failed: [{response}]");
                }

                if (response.StartsWith("DIO"))
                {
                    response = SerialPort.ReadLine();
                    Logger.LogDebug($"Response: [{response}]");
                }

                return response.ToBytes();
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