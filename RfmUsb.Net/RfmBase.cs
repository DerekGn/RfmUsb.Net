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
using RfmUsb.Net.Io;
using RfmUsb.Net.Ports;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace RfmUsb.Net
{
    /// <summary>
    /// A base Rfm class
    /// </summary>
    public abstract class RfmBase : IRfm
    {
        internal const string ResponseOk = "OK";
        internal readonly AutoResetEvent _signal;
        internal readonly ILogger<IRfm> Logger;
        internal ISerialPort SerialPort;
        private readonly ISerialPortFactory SerialPortFactory;
        private List<string> _responses;
        private int _signalTimeout;
        private RfmUsbStream _stream;

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
            _signal = new AutoResetEvent(false);
            _responses = new List<string>();
            _stream = new RfmUsbStream(this);
            _signalTimeout = 500;
        }

        ///<inheritdoc/>
        public event EventHandler<DioIrq> DioInterrupt;

        ///<inheritdoc/>
        public AddressFilter AddressFiltering
        {
            get => (AddressFilter)SendCommand(Commands.GetAddressFiltering).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetAddressFiltering} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public short Afc => SendCommand(Commands.GetAfc).ConvertToInt16();

        ///<inheritdoc/>
        public bool AfcAutoClear
        {
            get => SendCommand(Commands.GetAfcAutoClear).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetAfcAutoClear} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AfcAutoOn
        {
            get => SendCommand(Commands.GetAfcAutoOn).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetAfcAutoOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public uint BitRate
        {
            get => SendCommand(Commands.GetBitRate).ConvertToUInt32();
            set => SendCommandWithCheck($"{Commands.SetBitRate} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte BroadcastAddress
        {
            get => SendCommand(Commands.GetBroadcastAddress).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetBroadcastAddress} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool BufferedIoEnable 
        { 
            get => SendCommand(Commands.GetBufferEnable).StartsWith("1"); 
            set => SendCommandWithCheck($"{Commands.SetBufferEnable} {(value ? "1" : "0")}", ResponseOk); 
        }

        ///<inheritdoc/>
        public bool CrcAutoClearOff
        {
            get => SendCommand(Commands.GetCrcAutoClearOff).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetCrcAutoClearOff} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool CrcOn
        {
            get => SendCommand(Commands.GetCrcOn).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetCrcOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public DcFreeEncoding DcFreeEncoding
        {
            get => (DcFreeEncoding)SendCommand(Commands.GetDcFree).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDcFree} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public DioIrq DioInterruptMask
        {
            get => GetDioInterruptMask();
            set => SetDioInterrupMask(value);
        }

        ///<inheritdoc/>
        public short Fei => SendCommand(Commands.GetFei).ConvertToInt16();

        ///<inheritdoc/>
        public IEnumerable<byte> Fifo
        {
            get => SendCommand(Commands.GetFifo).ToBytes();
            set => SendCommandWithCheck($"{Commands.SetFifo} {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte FifoThreshold
        {
            get => SendCommand(Commands.GetFifoThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetFifoThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public string FirmwareVersion => SendCommand(Commands.GetFirmwareVersion).Replace(Environment.NewLine, string.Empty);

        ///<inheritdoc/>
        public uint Frequency
        {
            get => Convert.ToUInt32(SendCommand(Commands.GetFrequency).Trim('[', ']'), 16);
            set => SendCommandWithCheck($"{Commands.SetFrequency} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort FrequencyDeviation
        {
            get => SendCommand(Commands.GetFrequencyDeviation).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetFrequencyDeviation} 0x{value:X4}", ResponseOk);
        }

        ///<inheritdoc/>
        public FskModulationShaping FskModulationShaping
        {
            get => (FskModulationShaping)SendCommand(Commands.GetFskModulationShaping).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFskModulationShaping} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte InterPacketRxDelay
        {
            get => SendCommand(Commands.GetInterPacketRxDelay).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetInterPacketRxDelay} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public IoBufferInfo IoBufferInfo => GetIoBufferInfo();

        ///<inheritdoc/>
        public sbyte LastRssi => (sbyte)SendCommand(Commands.GetLastRssi).ConvertToInt32();

        ///<inheritdoc/>
        public LnaGain LnaGainSelect
        {
            get => (LnaGain)SendCommand(Commands.GetLnaGainSelect).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetLnaGainSelect} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public Mode Mode
        {
            get => (Mode)SendCommand(Commands.GetMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetMode} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public ModulationType ModulationType
        {
            get => (ModulationType)SendCommand(Commands.GetModulationType).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetModulationType} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte NodeAddress
        {
            get => SendCommand(Commands.GetNodeAddress).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetNodeAddress} 0x{value:X}", ResponseOk);
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
        public OokAverageThresholdFilter OokAverageThresholdFilter
        {
            get => (OokAverageThresholdFilter)SendCommand(Commands.GetOokAverageThresholdFilter).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokAverageThresholdFilter} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte OokFixedThreshold
        {
            get => SendCommand(Commands.GetOokFixedThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetOokFixedThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public OokModulationShaping OokModulationShaping
        {
            get => (OokModulationShaping)SendCommand(Commands.GetOokModulationShaping).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokModulationShaping} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public OokThresholdDec OokPeakThresholdDec
        {
            get => (OokThresholdDec)SendCommand(Commands.GetOokPeakThresholdDec).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokPeakThresholdDec} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public OokThresholdStep OokPeakThresholdStep
        {
            get => (OokThresholdStep)SendCommand(Commands.GetOokPeakThresholdStep).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokPeakThresholdStep} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public OokThresholdType OokThresholdType
        {
            get => (OokThresholdType)SendCommand(Commands.GetOokThresholdType).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokThresholdType} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool PacketFormat
        {
            get => SendCommand(Commands.GetPacketFormat).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetPacketFormat} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public PaRamp PaRamp
        {
            get => (PaRamp)SendCommand(Commands.GetPaRamp).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetPaRamp} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort PayloadLength
        {
            get => SendCommand(Commands.GetPayloadLength).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetPayloadLength} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort PreambleSize
        {
            get => SendCommand(Commands.GetPreambleSize).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetPreambleSize} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RadioVersion => SendCommand(Commands.GetRadioVersion).ConvertToByte();

        ///<inheritdoc/>
        public sbyte Rssi => (sbyte)SendCommand(Commands.GetRssi).ConvertToUInt32();

        ///<inheritdoc/>
        public byte RxBw
        {
            get => Convert.ToByte(SendCommand(Commands.GetRxBw).Substring(0, 4), 16);
            set => SendCommandWithCheck($"{Commands.SetRxBw} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RxBwAfc
        {
            get => Convert.ToByte(SendCommand(Commands.GetRxBwAfc).Substring(0, 4), 16);
            set => SendCommandWithCheck($"{Commands.SetRxBwAfc} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public string SerialNumber => SendCommand(Commands.GetSerialNumber).Replace(Environment.NewLine, string.Empty);

        ///<inheritdoc/>
        public RfmUsbStream Stream => _stream;

        ///<inheritdoc/>
        public IEnumerable<byte> Sync
        {
            get => SendCommand(Commands.GetSync).ToBytes();
            set => SendCommandWithCheck($"{Commands.SetSync} {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool SyncEnable
        {
            get => SendCommand(Commands.GetSyncEnable).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetSyncEnable} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte SyncSize
        {
            get => SendCommand(Commands.GetSyncSize).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetSyncSize} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public sbyte Temperature => SendCommand(Commands.GetTemperatureValue).ConvertToSByte();

        ///<inheritdoc/>
        public int Timeout
        {
            get => SerialPort.ReadTimeout;
            set
            {
                _signalTimeout = value;
                SerialPort.ReadTimeout = value;
                SerialPort.WriteTimeout = value;
            }
        }

        ///<inheritdoc/>
        public bool TxStartCondition
        {
            get => SendCommand(Commands.GetTxStartCondition).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetTxStartCondition} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public void Close()
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Close();

                SerialPort = null;
            }
        }

        ///<inheritdoc/>
        public void EnterBootloader()
        {
            SendCommand(Commands.ExecuteBootloader);
        }

        ///<inheritdoc/>
        public void ExecuteReset()
        {
            FlushSerialPort();
            SendCommandWithCheck(Commands.ExecuteReset, ResponseOk);
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
                    SerialPort.ReadTimeout = _signalTimeout;
                    SerialPort.WriteTimeout = _signalTimeout;
                    SerialPort.Open();

                    SerialPort.DataReceived += SerialPortDataReceived;
                }

                ExecuteReset();

                CheckDeviceVersion(FirmwareVersion);

                SendCommand($"{Commands.SetOutputbase} 0");
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex, "Exception occurred opening serial port");

                throw new RfmUsbSerialPortOpenFailedException(
                    $"Unable to open serial port [{serialPort}] Reason: [{ex.Message}]. " +
                    $"Available Serial Ports: [{string.Join(", ", SerialPortFactory.GetSerialPorts())}]");
            }
        }

        ///<inheritdoc/>
        public void RcCalibration()
        {
            SendCommandWithCheck(Commands.ExecuteRcCalibration, ResponseOk);
        }

        ///<inheritdoc/>
        public void SetDioMapping(Dio dio, DioMapping mapping)
        {
            SendCommandWithCheck($"{Commands.SetDioMapping} {(int)dio} {(int)mapping}", ResponseOk);
        }

        internal void FlushSerialPort()
        {
            if (SerialPort != null)
            {
                Logger.LogDebug("Flushing Serial Ports input buffer");
                SerialPort.DiscardInBuffer();
            }
        }

        internal virtual string GetDeviceName()
        {
            return GetType().Name;
        }

        internal void ResetSerialPort(ISerialPort serialPort)
        {
            serialPort.DataReceived -= SerialPortDataReceived;
            SerialPort = null;
        }

        internal string SendCommand(string command)
        {
            return SendCommandListResponse(command).FirstOrDefault(string.Empty);
        }

        internal List<string> SendCommandListResponse(string command)
        {
            CheckOpen();

            lock (SerialPort)
            {
                SerialPort.Write($"{command}\n");

                do
                {
                    WaitForSerialPortDataSignal();
                } while (_responses.Count == 0);

                Logger.LogDebug("Command: [{command}]", command);
                Logger.LogDebug("Result: [{response}]", string.Join(" ", _responses));

                var result = new List<string>();
                result.AddRange(_responses);

                _responses.Clear();

                return result;
            }
        }

        internal void SendCommandWithCheck(string command, string response)
        {
            var result = SendCommand(command);

            if (!result.StartsWith(response))
            {
                throw new RfmUsbCommandExecutionException($"Command: [{command}] Execution Failed Reason: [{result}]");
            }
        }

        internal void SetupSerialPort(ISerialPort serialPort)
        {
            SerialPort = serialPort;
            serialPort.DataReceived += SerialPortDataReceived;
        }

        internal void TransmitBuffer()
        {
            SendCommandWithCheck($"{Commands.TransmitBuffer}", ResponseOk);
        }

        internal virtual void WaitForSerialPortDataSignal()
        {
            if (!_signal.WaitOne(_signalTimeout))
            {
                throw new RfmUsbTimeoutException($"No response received from Rfm device within [{_signalTimeout}]");
            }
        }

        internal void WriteToBuffer(IEnumerable<byte> chunk)
        {
            var response = SendCommand($"{Commands.WriteBuffer} {BitConverter.ToString(chunk.ToArray()).Replace("-", string.Empty)}");

            switch (response)
            {
                case ResponseOk:
                    break;
                case "ERROR:IO_BUFFER_NOT_ENABLED":
                    throw new RfmUsbBufferedIoNotEnabledException();
                case "ERROR:OVERFLOW":
                    throw new RfmUsbBufferedIoOverflowException();
            }
        }

        /// <summary>
        /// Check the firmware version of the connected device
        /// </summary>
        /// <param name="firmwareVersion">The firmware version string to check</param>
        private void CheckDeviceVersion(string firmwareVersion)
        {
            if (string.IsNullOrWhiteSpace(firmwareVersion))
            {
                throw new RfmUsbInvalidDeviceTypeException("FirmwareVersion is empty");
            }

            if (!firmwareVersion.Contains(GetDeviceName(), StringComparison.InvariantCultureIgnoreCase))
            {
                throw new RfmUsbInvalidDeviceTypeException($"Invalid Device Type firmware value {firmwareVersion}");
            }
        }

        private void CheckOpen()
        {
            if (SerialPort != null && !SerialPort.IsOpen)
            {
                throw new InvalidOperationException("Instance not open");
            }
        }

        private DioIrq GetDioInterruptMask()
        {
            DioIrq irqMask = DioIrq.None;

            var responses = SendCommandListResponse(Commands.GetDioInterruptMask);

            responses.ForEach(_ =>
            {
                var parts = _.Split('-');

                switch (parts[1])
                {
                    case "DIO0":
                        if (Convert.ToByte(parts[0], 16) == 1)
                        {
                            irqMask |= DioIrq.Dio0;
                        }
                        break;

                    case "DIO1":
                        if (Convert.ToByte(parts[0], 16) == 1)
                        {
                            irqMask |= DioIrq.Dio1;
                        }
                        break;

                    case "DIO2":
                        if (Convert.ToByte(parts[0], 16) == 1)
                        {
                            irqMask |= DioIrq.Dio2;
                        }
                        break;

                    case "DIO3":
                        if (Convert.ToByte(parts[0], 16) == 1)
                        {
                            irqMask |= DioIrq.Dio3;
                        }
                        break;

                    case "DIO4":
                        if (Convert.ToByte(parts[0], 16) == 1)
                        {
                            irqMask |= DioIrq.Dio4;
                        }
                        break;

                    case "DIO5":
                        if (Convert.ToByte(parts[0], 16) == 1)
                        {
                            irqMask |= DioIrq.Dio5;
                        }
                        break;

                    default:
                        break;
                }
            });

            return irqMask;
        }

        private IoBufferInfo GetIoBufferInfo()
        {
            var lines = SendCommandListResponse(Commands.GetIoBufferInfo);

            int capacity = 0;
            int count = 0;

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[0].ToUpper())
                {
                    case "CAPACITY":
                        capacity = parts[1].ConvertToInt32();
                        break;

                    case "COUNT":
                        count = parts[1].ConvertToInt32();
                        break;
                }
            });

            return new IoBufferInfo(capacity, count);
        }

        private void RaiseDioInterrupt(DioIrq values) =>
                                                DioInterrupt?.Invoke(this, values);

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                do
                {
                    var response = SerialPort.ReadLine();

                    if (response.StartsWith("DIO PIN IRQ"))
                    {
                        RaiseDioInterrupt((DioIrq)
                            Convert.ToInt32(
                                    response.Split(" ")
                                    .Last()
                                    .Replace("[", string.Empty)
                                    .Replace("]", string.Empty), 16));
                    }
                    else
                    {
                        _responses.Add(response);
                    }
                } while (SerialPort.BytesToRead != 0);

                Logger.LogTrace("Received Serial Port Data: {type}", e.EventType);

                _signal.Set();
            }
        }

        private void SetDioInterrupMask(DioIrq value)
        {
            SendCommandWithCheck($"{Commands.SetDioInterruptMask} 0x{value:X}", ResponseOk);
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