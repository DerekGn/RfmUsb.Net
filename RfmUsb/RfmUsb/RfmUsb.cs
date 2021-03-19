﻿/*
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

using Microsoft.Extensions.Logging;
using RfmUsb.Exceptions;
using RfmUsb.Extensions;
using RfmUsb.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RfmUsb
{
    public class RfmUsb : IRfmUsb
    {
        private const string ResponseOk = "OK";

        private readonly ISerialPortFactory _serialPortFactory;
        private readonly ILogger<IRfmUsb> _logger;

        private ISerialPort _serialPort;

        public RfmUsb(ILogger<IRfmUsb> logger, ISerialPortFactory serialPortFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serialPortFactory = serialPortFactory ?? throw new ArgumentNullException(nameof(serialPortFactory));
        }
        ///<inheritdoc/>
        public IEnumerable<byte> Fifo
        {
            get => SendCommand("g-fifo").ToBytes();
            set
            {
                SendCommandWithCheck($"s-fifo {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
            }
        }
        public bool Sequencer
        {
            get => SendCommand("g-so").StartsWith("1");
            set => SendCommandWithCheck($"s-so {(value ? "1" : "0")}", ResponseOk);
        }
        public bool ListenerOn
        {
            get => SendCommand("g-lo").StartsWith("1");
            set => SendCommandWithCheck($"s-lo {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public Mode Mode
        {
            get => (Mode)SendCommand("g-om").ConvertToInt32();
            set => SendCommand($"s-om 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public Modulation Modulation
        {
            get => (Modulation)SendCommand("g-mt").ConvertToInt32();
            set => SendCommand($"s-mt 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public FskModulationShaping FskModulationShaping
        {
            get => (FskModulationShaping)SendCommand("g-fs").ConvertToInt32();
            set => SendCommand($"s-fs 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public OokModulationShaping OokModulationShaping
        {
            get => (OokModulationShaping)SendCommand("g-os").ConvertToInt32();
            set => SendCommand($"s-os 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public ushort BitRate
        {
            get => Convert.ToUInt16(SendCommand("g-br"), 16);
            set => SendCommandWithCheck($"s-br 0x{(int)value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public ushort FreqencyDeviation
        {
            get => SendCommand("g-fd").ConvertToUInt16();
            set => SendCommandWithCheck($"s-fd {value}", ResponseOk);
        }
        ///<inheritdoc/>
        public uint Frequency
        {
            get => Convert.ToUInt32(SendCommand("g-f").Trim('[',']'), 16);
            set => SendCommandWithCheck($"s-f 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool AfcLowBetaOn
        {
            get => SendCommand("g-ab").StartsWith("1");
            set => SendCommandWithCheck($"s-ab {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public ListenResolution ListenResolutionIdle
        {
            get => (ListenResolution)SendCommand("g-ir").ConvertToInt32();
            set => SendCommand($"s-ir 0x{value:X}");
        }
        ///<inheritdoc/>
        public ListenResolution ListenResolutionRx
        {
            get => (ListenResolution)SendCommand("g-rr").ConvertToInt32();
            set => SendCommand($"s-rr 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool ListenCriteria
        {
            get => SendCommand("g-lc").StartsWith("1");
            set => SendCommandWithCheck($"s-lc {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public ListenEnd ListenEnd
        {
            get => (ListenEnd)SendCommand("g-lem").ConvertToInt32();
            set => SendCommand($"s-lem 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte ListenCoefficentIdle
        {
            get => SendCommand("g-lic").ConvertToByte();
            set => SendCommandWithCheck($"s-lic 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte ListenCoefficentRx
        {
            get => SendCommand("g-lrc").ConvertToByte();
            set => SendCommandWithCheck($"s-lrc 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public string Version => SendCommand("g-fv");
        ///<inheritdoc/>
        public byte OutputPower
        {
            get => SendCommand("g-op").ConvertToByte();
            set => SendCommand($"s-op 0x{value:X}");
        }
        ///<inheritdoc/>
        public PaRamp PaRamp
        {
            get => (PaRamp)SendCommand("g-par").ConvertToInt32();
            set => SendCommand($"s-par 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool OcpEnable
        {
            get => SendCommand("g-ocp").StartsWith("1");
            set => SendCommandWithCheck($"s-ocp {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public OcpTrim OcpTrim
        {
            get => (OcpTrim)SendCommand("g-ocpt").ConvertToInt32();
            set => SendCommand($"s-ocpt 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool Impedance
        {
            get => SendCommand("g-lnaz").StartsWith("1");
            set => SendCommandWithCheck($"s-lnaz {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public LnaGain CurrentLnaGain => (LnaGain)SendCommand("g-lnag").ConvertToInt32();
        ///<inheritdoc/>
        public LnaGain LnaGainSelect
        {
            get => (LnaGain)SendCommand("g-lnags").ConvertToInt32();
            set => SendCommand($"s-lnags 0x{value:X}");
        }
        ///<inheritdoc/>
        public DccFreq DccFreq
        {
            get => (DccFreq)SendCommand("g-df").ConvertToInt32();
            set => SendCommandWithCheck($"s-df 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte RxBw
        {
            get => Convert.ToByte(SendCommand("g-rxbw").Substring(0, 4), 16);
            set => SendCommandWithCheck($"s-rxbw 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public DccFreq DccFreqAfc
        {
            get => (DccFreq)SendCommand("g-dfa").ConvertToInt32();
            set => SendCommand($"s-dfa 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte RxBwAfc
        {
            get => Convert.ToByte(SendCommand("g-rxbwa").Substring(0, 4), 16);
            set => SendCommandWithCheck($"s-rxbwa 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public OokThresholdType OokThresholdType
        {
            get => (OokThresholdType)SendCommand("g-ott").ConvertToInt32();
            set => SendCommand($"s-ott 0x{value:X}");
        }
        ///<inheritdoc/>
        public OokThresholdStep OokPeakThresholdStep
        {
            get => (OokThresholdStep)SendCommand("g-ots").ConvertToInt32();
            set => SendCommand($"s-ots 0x{value:X}");
        }
        ///<inheritdoc/>
        public OokThresholdDec OokPeakThresholdDec
        {
            get => (OokThresholdDec)SendCommand("g-optd").ConvertToInt32();
            set => SendCommand($"s-optd 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public OokAverageThresholdFilter OokAverageThresholdFilter
        {
            get => (OokAverageThresholdFilter)SendCommand("g-oatf").ConvertToInt32();
            set => SendCommand($"s-oatf 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte OokFixedThreshold
        {
            get => SendCommand("g-oft").ConvertToByte();
            set => SendCommandWithCheck($"s-oft 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool AfcAutoClear
        {
            get => SendCommand("g-aac").StartsWith("1");
            set => SendCommandWithCheck($"s-aac {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool AfcAutoOn
        {
            get => SendCommand("g-aao").StartsWith("1");
            set => SendCommandWithCheck($"s-aao {(value ? "1" : "0")}", ResponseOk);
        }
        public ushort Afc => SendCommand("g-a").ConvertToUInt16();
        ///<inheritdoc/>
        public ushort Fei => SendCommand("g-a").ConvertToUInt16();
        ///<inheritdoc/>
        public byte Rssi => SendCommand("g-rssi").ConvertToByte();
        ///<inheritdoc/>
        public Irq Irq => GetIrqInternal();
        ///<inheritdoc/>
        public byte RssiThreshold
        {
            get => SendCommand("g-rt").ConvertToByte();
            set => SendCommandWithCheck($"s-rt 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte TimeoutRxStart
        {
            get => SendCommand("g-trs").ConvertToByte();
            set => SendCommandWithCheck($"s-trs 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte TimeoutRssiThreshold
        {
            get => SendCommand("g-trt").ConvertToByte();
            set => SendCommandWithCheck($"s-trt 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public ushort PreambleSize
        {
            get => SendCommand("g-ps").ConvertToUInt16();
            set => SendCommandWithCheck($"s-ps 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool SyncEnable
        {
            get => SendCommand("g-se").StartsWith("1");
            set => SendCommandWithCheck($"s-se {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool FifoFill
        {
            get => SendCommand("g-ffc").StartsWith("1");
            set => SendCommandWithCheck($"s-ffc {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte SyncBitErrors
        {
            get => SendCommand("g-sbe").ConvertToByte();
            set => SendCommandWithCheck($"s-sbe 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public IEnumerable<byte> Sync
        {
            get => SendCommand("g-sync").ToBytes();
            set
            {
                SendCommandWithCheck($"s-sync {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
                SendCommandWithCheck($"s-ss {value.Count() - 1}", ResponseOk);
            }
        }
        ///<inheritdoc/>
        public bool PacketFormat
        {
            get => SendCommand("g-pf").StartsWith("1");
            set => SendCommandWithCheck($"s-pf {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public DcFree DcFree
        {
            get => (DcFree)SendCommand("g-dfe").ConvertToInt32();
            set => SendCommand($"s-dfe 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool CrcOn
        {
            get => SendCommand("g-cc").StartsWith("1");
            set => SendCommandWithCheck($"s-cc {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool CrcAutoClear
        {
            get => SendCommand("g-caco").StartsWith("1");
            set => SendCommandWithCheck($"s-caco {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public AddressFilter AddressFiltering
        {
            get => (AddressFilter)SendCommand("g-af").ConvertToInt32();
            set => SendCommandWithCheck($"s-af 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte PayloadLength
        {
            get => SendCommand("g-pl").ConvertToByte();
            set => SendCommandWithCheck($"s-pl 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte NodeAddress
        {
            get => SendCommand("g-na").ConvertToByte();
            set => SendCommandWithCheck($"s-na 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte BroadcastAddress
        {
            get => SendCommand("g-ba").ConvertToByte();
            set => SendCommandWithCheck($"s-ba 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public EnterCondition EnterCondition
        {
            get => (EnterCondition)SendCommand("g-amec").ConvertToInt32();
            set => SendCommand($"s-amec 0x{value:X}");
        }
        ///<inheritdoc/>
        public ExitCondition ExitCondition
        {
            get => (ExitCondition)SendCommand("g-amexc").ConvertToInt32();
            set => SendCommand($"s-amexc 0x{value:X}");
        }
        ///<inheritdoc/>
        public IntermediateMode IntermediateMode
        {
            get => (IntermediateMode)SendCommand("g-im").ConvertToInt32();
            set => SendCommand($"s-im 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool TxStartCondition
        {
            get => SendCommand("g-tsc").StartsWith("1");
            set => SendCommandWithCheck($"s-tsc {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte FifoThreshold
        {
            get => SendCommand("g-ft").ConvertToByte();
            set => SendCommandWithCheck($"s-ft 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte InterPacketRxDelay
        {
            get => SendCommand("g-iprd").ConvertToByte();
            set => SendCommandWithCheck($"s-iprd 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool AutoRxRestartOn
        {
            get => SendCommand("g-arre").StartsWith("1");
            set => SendCommandWithCheck($"s-arre {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public bool AesOn
        {
            get => SendCommand("g-ae").Substring(0, 1) == "1";
            set => SendCommandWithCheck($"s-ae {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte TemperatureValue => SendCommand("g-t").ConvertToByte();
        ///<inheritdoc/>
        public bool SensitivityBoost
        {
            get => SendCommand("g-sb").StartsWith("1");
            set => SendCommandWithCheck($"s-sb {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public ContinuousDagc ContinuousDagc
        {
            get => (ContinuousDagc)SendCommand("g-cd").ConvertToInt32();
            set => SendCommand($"s-cd 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool LowBetaAfcOffset
        {
            get => SendCommand("g-lbao").StartsWith("1");
            set => SendCommandWithCheck($"s-lbao {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte DioInterruptMask
        {
            get => SendCommand("g-di").ConvertToByte();
            set => SendCommandWithCheck($"s-di 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public int Timeout
        {
            get => _serialPort.ReadTimeout;
            set
            {
                _serialPort.ReadTimeout = value;
                _serialPort.WriteTimeout = value;
            }
        }
        ///<inheritdoc/>
        public byte RadioConfig
        {
            get => SendCommand("g-rc").ConvertToByte();
            set => SendCommandWithCheck($"s-rc 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public void AfcClear()
        {
            SendCommandWithCheck("e-ac", ResponseOk);
        }
        ///<inheritdoc/>
        public void AfcStart()
        {
            SendCommandWithCheck("e-a", ResponseOk);
        }
        ///<inheritdoc/>
        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
        ///<inheritdoc/>
        public void FeiStart()
        {
            SendCommandWithCheck("e-fei", ResponseOk);
        }
        ///<inheritdoc/>
        public void ListenAbort()
        {
            SendCommandWithCheck("e-lma", ResponseOk);
        }
        ///<inheritdoc/>
        public void MeasureTemperature()
        {
            SendCommandWithCheck("e-tm", ResponseOk);
        }
        ///<inheritdoc/>
        public void Open(string serialPort, int baudRate)
        {
            try
            {
                if (_serialPort == null)
                {
                    _serialPort = _serialPortFactory.CreateSerialPortInstance(serialPort);

                    _serialPort.BaudRate = baudRate;
                    _serialPort.NewLine = "\r\n";
                    _serialPort.DtrEnable = true;
                    _serialPort.RtsEnable = true;
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;
                    _serialPort.Open();
                }
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogDebug(ex, "Exception occurred opening serial port");

                throw new RfmUsbSerialPortNotFoundException(
                    $"Unable to open serial port [{serialPort}] Reason: [{ex.Message}]. " +
                    $"Available Serial Ports: [{string.Join(", ", _serialPortFactory.GetSerialPorts())}]");
            }
        }
        ///<inheritdoc/>
        public void RcCalibration()
        {
            SendCommandWithCheck("e-rc", ResponseOk);
        }
        ///<inheritdoc/>
        public void Reset()
        {
            SendCommandWithCheck($"e-r", ResponseOk);
        }
        ///<inheritdoc/>
        public void RestartRx()
        {
            SendCommandWithCheck($"e-rr", ResponseOk);
        }
        ///<inheritdoc/>
        public void SetAesKey(IEnumerable<byte> key)
        {
            SendCommandWithCheck($"s-aes {BitConverter.ToString(key.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }
        ///<inheritdoc/>
        public void SetDioMapping(Dio dio, DioMapping mapping)
        {
            SendCommandWithCheck($"s-dio {(int)dio} {(int)mapping}", $"[0x{(int)mapping:X4}]-Map {(int)mapping:D2}");
        }
        ///<inheritdoc/>
        public void GetDioMapping(out Dio dio, out DioMapping mapping)
        {
            SendCommand("g-dio");

            dio = Dio.Dio0;
            mapping = DioMapping.DioMapping3;
        }

        ///<inheritdoc/>
        public void StartRssi()
        {
            SendCommandWithCheck($"e-rssi", ResponseOk);
        }
        ///<inheritdoc/>
        public IList<string> GetRadioConfiurations()
        {
            lock (_serialPort)
            {
                List<string> configs = new List<string>();

                var result = SendCommand("g-rcl");
                configs.Add(result);

                do
                {
                    try
                    {
                        configs.Add(_serialPort.ReadLine());
                    }
                    catch (TimeoutException)
                    {
                        break;
                    }

                } while (true);

                return configs;
            }
        }
        ///<inheritdoc/>
        public void Transmit(IList<byte> data)
        {
            var response = SendCommand($"e-tx {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)}");

            if (response.Contains("TX") || response.Contains("RX"))
            {
                throw new RfmUsbTransmitException($"Packet transmission failed: [{response}]");
            }
        }
        ///<inheritdoc/>
        public IList<byte> TransmitReceive(IList<byte> data, int timeout)
        {
            var response = SendCommand($"e-txrx {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)} {timeout}");

            if (response.Contains("TX") || response.Contains("RX"))
            {
                throw new RfmUsbTransmitException($"Packet transmission failed: [{response}]");
            }

            return response.ToBytes();
        }
        ///<inheritdoc/>
        public void WaitForIrq()
        {
            lock (_serialPort)
            {
                var irq = _serialPort.ReadLine();

                if (!irq.StartsWith("DIO PIN IRQ"))
                {
                    throw new RfmUsbCommandExecutionException($"Invalid response received for IRQ signal: [{irq}]");
                }
            }
        }
        private Irq GetIrqInternal()
        {
            lock (_serialPort)
            {
                Irq irq = Irq.None;

                var result = SendCommand("g-irq");

                while (!result.Contains("RX_RDY"))
                {
                    var parts = result.Split(':');

                    switch (parts[1])
                    {
                        case "CRC_OK":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.CrcOK;
                            }
                            break;
                        case "PAYLOAD_READY":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.PayloadReady;
                            }
                            break;
                        case "FIFO_OVERRUN":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.FifoOverrun;
                            }
                            break;
                        case "FIFO_LEVEL":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.FifoLevel;
                            }
                            break;
                        case "FIFO_NOT_EMPTY":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.FifoNotEmpty;
                            }
                            break;
                        case "FIFO_FULL":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.FifoFull;
                            }
                            break;
                        case "ADDRESS_MATCH":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.AddressMatch;
                            }
                            break;
                        case "AUTO_MODE":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.AutoMode;
                            }
                            break;
                        case "TIMEOUT":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.Timeout;
                            }
                            break;
                        case "RSSI":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.Rssi;
                            }
                            break;
                        case "PLL_LOCK":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.PllLock;
                            }
                            break;
                        case "TX_RDY":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.TxReady;
                            }
                            break;
                        case "RX_RDY":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.RxReady;
                            }
                            break;
                        case "MODE_RDY":
                            if (parts[0] == "1")
                            {
                                irq |= Irq.ModeReady;
                            }
                            break;
                    }

                    result = _serialPort.ReadLine();
                };

                return irq;
            }
        }
        private string SendCommand(string command)
        {
            lock (_serialPort)
            {
                _serialPort.Write($"{command}\n");
                
                var response = _serialPort.ReadLine();

                _logger.LogDebug($"Command: [{command}] Result: [{response}]");

                return response;
            }
        }
        private void SendCommandWithCheck(string command, string response)
        {
            var result = SendCommand(command);

            if (!result.StartsWith(response))
            {
                throw new RfmUsbCommandExecutionException($"Command: [{command}] Execution Failed Reason: [{result}]");
            }
        }

        #region IDisposible
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_serialPort != null)
                    {
                        _serialPort.Close();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}
