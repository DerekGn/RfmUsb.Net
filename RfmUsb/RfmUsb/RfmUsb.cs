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

using Microsoft.Extensions.Logging;
using RfmUsb.Ports;
using RfmUsb.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            get => SendCommand($"g-fifo").ToBytes();
            set
            {
                SendCommandWithCheck($"s-fifo {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
            }
        }
        public bool Sequencer
        {
            get => SendCommand("g-so").ToBytes().First() == 1;
            set => SendCommandWithCheck($"s-so 0x0{(value ? 0x01 : 0x00):X}", ResponseOk);
        }
        public bool ListenerOn
        {
            get => SendCommand("g-lo").ToBytes().First() == 1;
            set => SendCommandWithCheck($"s-lo 0x0{(value ? 0x01 : 0x00):X}", ResponseOk);
        }
        ///<inheritdoc/>
        public Mode Mode
        {
            get => (Mode)int.Parse(SendCommand("g-om"));
            set => SendCommand($"s-om 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public Modulation Modulation
        {
            get => (Modulation)int.Parse(SendCommand("g-mt"));
            set => SendCommand($"s-mt 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public FskModulationShaping FskModulationShaping
        {
            get => (FskModulationShaping)int.Parse(SendCommand("g-fs"));
            set => SendCommand($"s-fs 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public OokModulationShaping OokModulationShaping
        {
            get => (OokModulationShaping)int.Parse(SendCommand("g-os"));
            set => SendCommand($"s-os 0x{(int)value:X}");
        }
        public int BitRate
        {
            get => int.Parse(SendCommand("g-br"));
            set => SendCommand($"s-br 0x{(int)value:X}");
        }
        ///<inheritdoc/>
        public int FreqencyDeviation
        {
            get => int.Parse(SendCommand($"g-fd"));
            set => SendCommand($"s-fd {value}");
        }
        ///<inheritdoc/>
        public int Frequency
        {
            get => int.Parse(SendCommand($"g-f"));
            set => SendCommand($"s-f 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool AfcLowBetaOn
        {
            get => bool.Parse(SendCommand($"g-ab"));
            set => SendCommand($"s-ab 0x{value:X}");
        }
        ///<inheritdoc/>
        public ListenResolution ListenResolutionIdle
        {
            get => (ListenResolution)int.Parse(SendCommand("g-ir"));
            set => SendCommand($"s-ir 0x{value:X}");
        }
        ///<inheritdoc/>
        public ListenResolution ListenResolutionRx
        {
            get => (ListenResolution)int.Parse(SendCommand("g-rr"));
            set => SendCommand($"s-rr 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool ListenCriteria
        {
            get => bool.Parse(SendCommand($"g-lc"));
            set => SendCommand($"s-lc 0x{value:X}");
        }
        ///<inheritdoc/>
        public Mode ListenEnd
        {
            get => (Mode)int.Parse(SendCommand($"g-lem"));
            set => SendCommand($"s-lem 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte ListenCoefficentIdle
        {
            get => byte.Parse(SendCommand($"g-lic"));
            set => SendCommand($"s-lic 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte ListenCoefficentRx
        {
            get => byte.Parse(SendCommand($"g-lrc"));
            set => SendCommand($"s-lrc 0x{value:X}");
        }
        ///<inheritdoc/>
        public string Version => SendCommand("g-fv");
        ///<inheritdoc/>
        public byte OutputPower
        {
            get => byte.Parse(SendCommand($"g-op"));
            set => SendCommandWithCheck($"s-op {value}", ResponseOk);
        }
        ///<inheritdoc/>
        public PaRamp PaRamp
        {
            get => (PaRamp)int.Parse(SendCommand($"g-par"));
            set => SendCommand($"s-par 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool OcpEnable
        {
            get => bool.Parse(SendCommand($"g-ocp"));
            set => SendCommand($"s-ocp 0x{value:X}");
        }
        ///<inheritdoc/>
        public OcpTrim OcpTrim
        {
            get => (OcpTrim)int.Parse(SendCommand($"g-ocpt"));
            set => SendCommand($"s-ocpt 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool Impedance
        {
            get => bool.Parse(SendCommand($"g-lnaz"));
            set => SendCommand($"s-lnaz 0x{value:X}");
        }
        ///<inheritdoc/>
        public LnaGain CurrentLnaGain => (LnaGain)int.Parse(SendCommand($"g-lnag"));
        ///<inheritdoc/>
        public LnaGain LnaGainSelect
        {
            get => (LnaGain)int.Parse(SendCommand($"g-lnags"));
            set => SendCommand($"s-lnazs 0x{value:X}");
        }
        ///<inheritdoc/>
        public DccFreq DccFreq
        {
            get => (DccFreq)int.Parse(SendCommand($"g-df"));
            set => SendCommand($"s-df 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte RxBw
        {
            get => byte.Parse(SendCommand($"g-rxbwa"));
            set => SendCommand($"s-rxbwa 0x{value:X}");
        }
        ///<inheritdoc/>
        public DccFreq DccFreqAfc
        {
            get => (DccFreq)int.Parse(SendCommand($"g-dfa"));
            set => SendCommand($"s-dfa 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte RxBwAfc
        {
            get => byte.Parse(SendCommand($"g-rxbwa"));
            set => SendCommand($"s-rxbwa 0x{value:X}");
        }
        ///<inheritdoc/>
        public OokThresholdType OokThresholdType
        {
            get => (OokThresholdType)int.Parse(SendCommand($"g-ott"));
            set => SendCommand($"s-ott 0x{value:X}");
        }
        ///<inheritdoc/>
        public OokThresholdStep OokPeakThresholdStep
        {
            get => (OokThresholdStep)int.Parse(SendCommand($"g-ots"));
            set => SendCommand($"s-ots 0x{value:X}");
        }
        ///<inheritdoc/>
        public OokThresholdDec OokPeakThresholdDec
        {
            get => (OokThresholdDec)int.Parse(SendCommand($"g-optd"));
            set => SendCommand($"s-optd 0x{value:X}");
        }
        ///<inheritdoc/>
        public OokAverageThresholdFilter OokAverageThresholdFilter
        {
            get => (OokAverageThresholdFilter)int.Parse(SendCommand($"g-oatf"));
            set => SendCommand($"s-oatf 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte OokFixedThreshold
        {
            get => byte.Parse(SendCommand($"g-oft"));
            set => SendCommand($"s-oft 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool AfcAutoClear
        {
            get => bool.Parse(SendCommand($"g-aac"));
            set => SendCommand($"s-aac 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool AfcAutoOn
        {
            get => bool.Parse(SendCommand($"g-aao"));
            set => SendCommand($"s-aao 0x{value:X}");
        }
        public short Afc => byte.Parse(SendCommand($"g-a"));
        ///<inheritdoc/>
        public short Fei => byte.Parse(SendCommand($"g-a"));
        ///<inheritdoc/>
        public byte Rssi => byte.Parse(SendCommand($"g-rssi"));
        ///<inheritdoc/>
        public Irq Irq => GetIrqInternal();
        ///<inheritdoc/>
        public byte RssiThreshold
        {
            get => byte.Parse(SendCommand($"g-rt"));
            set => SendCommand($"s-rt 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte TimeoutRxStart => byte.Parse(SendCommand($"g-trs"));
        ///<inheritdoc/>
        public byte TimeoutRssiThreshold => byte.Parse(SendCommand($"g-trt"));
        ///<inheritdoc/>
        public short PreambleSize => short.Parse(SendCommand($"g-ps"));
        ///<inheritdoc/>
        public bool SyncEnable
        {
            get => bool.Parse(SendCommand($"g-se"));
            set => SendCommand($"s-se 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool FifoFill
        {
            get => bool.Parse(SendCommand($"g-ffc"));
            set => SendCommand($"s-ffc 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte SyncBitErrors
        {
            get => byte.Parse(SendCommand($"g-sbe"));
            set => SendCommand($"s-sbe 0x{value:X}");
        }
        ///<inheritdoc/>
        public IEnumerable<byte> Sync
        {
            get => SendCommand($"g-sync").ToBytes();
            set
            {
                SendCommandWithCheck($"s-sync {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
                SendCommandWithCheck($"s-ss {value.Count() - 1}", ResponseOk);
            }
        }
        ///<inheritdoc/>
        public bool PacketFormat
        {
            get => bool.Parse(SendCommand($"g-pf"));
            set => SendCommand($"s-pf 0x{value:X}");
        }
        ///<inheritdoc/>
        public DcFree DcFree
        {
            get => (DcFree)int.Parse(SendCommand($"g-dfe"));
            set => SendCommand($"s-dfe 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool CrcOn
        {
            get => bool.Parse(SendCommand($"g-cc"));
            set => SendCommand($"s-cc 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool CrcAutoClear
        {
            get => bool.Parse(SendCommand($"g-caco"));
            set => SendCommand($"s-caco 0x{value:X}");
        }
        ///<inheritdoc/>
        public AddressFilter AddressFiltering
        {
            get => (AddressFilter)int.Parse(SendCommand($"g-af"));
            set => SendCommand($"s-af 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte PayloadLength
        {
            get => byte.Parse(SendCommand($"g-pl"));
            set => SendCommand($"s-pl 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte NodeAddress
        {
            get => byte.Parse(SendCommand($"g-na"));
            set => SendCommand($"s-na 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte BroadcastAddress
        {
            get => byte.Parse(SendCommand($"g-ba"));
            set => SendCommand($"s-ba 0x{value:X}");
        }
        ///<inheritdoc/>
        public EnterCondition EnterCondition
        {
            get => (EnterCondition)int.Parse(SendCommand($"g-amec"));
            set => SendCommand($"s-amec 0x{value:X}");
        }
        ///<inheritdoc/>
        public ExitCondition ExitCondition
        {
            get => (ExitCondition)int.Parse(SendCommand($"g-amexc"));
            set => SendCommand($"s-amexc 0x{value:X}");
        }
        ///<inheritdoc/>
        public Mode IntermediateMode
        {
            get => (Mode)int.Parse(SendCommand($"g-im"));
            set => SendCommand($"s-im 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool TxStartCondition
        {
            get => bool.Parse(SendCommand($"g-tsc"));
            set => SendCommand($"s-tsc 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte FifoThreshold
        {
            get => SendCommand("g-ft").ToBytes().First();
            set => SendCommandWithCheck($"s-ft 0x{value:X}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte InterPacketRxDelay
        {
            get => byte.Parse(SendCommand($"g-iprd"));
            set => SendCommand($"s-iprd 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool AutoRxRestartOn
        {
            get => bool.Parse(SendCommand($"g-arre"));
            set => SendCommand($"s-arre 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool AesOn
        {
            get => bool.Parse(SendCommand($"g-ae"));
            set => SendCommand($"s-ae 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte TemperatureValue => byte.Parse(SendCommand($"g-t"));
        ///<inheritdoc/>
        public bool SensitivityBoost
        {
            get => bool.Parse(SendCommand($"g-sb"));
            set => SendCommand($"s-sb 0x{value:X}");
        }
        ///<inheritdoc/>
        public ContinuousDagc ContinuousDagc
        {
            get => (ContinuousDagc)int.Parse(SendCommand($"g-cd"));
            set => SendCommand($"s-cd 0x{value:X}");
        }
        ///<inheritdoc/>
        public bool LowBetaAfcOffset
        {
            get => bool.Parse(SendCommand($"g-lbao"));
            set => SendCommand($"s-lbao 0x{value:X}");
        }
        ///<inheritdoc/>
        public byte DioInterruptMask
        {
            get => SendCommand("g-di").ToBytes().First();
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
            get => byte.Parse(SendCommand($"g-rc"));
            set => SendCommand($"s-rc 0x{value:X}");
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
        public void MeaseureTemperature()
        {
            SendCommandWithCheck("e-t", ResponseOk);
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
        public void StartRssi()
        {
            SendCommandWithCheck($"e-rssi", ResponseOk);
        }
        ///<inheritdoc/>
        public IList<string> GetRadioConfiurations()
        {
            lock(_serialPort)
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

        public void LeistenAbort()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
