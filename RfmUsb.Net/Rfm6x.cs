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
using RfmUsb.Exceptions;
using RfmUsb.Extensions;
using RfmUsb.Net;
using RfmUsb.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RfmUsb
{
    /// <summary>
    /// An implementation of the <see cref="IRfm6x"/> interface
    /// </summary>
    public class Rfm6x : RfmBase, IRfm6x
    {
        /// <summary>
        /// Create an instance of a <see cref="RfmUsb"/>
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{T}"/> for logging</param>
        /// <param name="serialPortFactory">The <see cref="ISerialPortFactory"/> instance for creating and querying serial port instances</param>
        public Rfm6x(ILogger<IRfm> logger, ISerialPortFactory serialPortFactory) : base(logger, serialPortFactory)
        {
        }

        ///<inheritdoc/>
        public AddressFilter AddressFiltering
        {
            get => (AddressFilter)SendCommand(Commands.GetAddressFiltering).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetAddressFiltering} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AesOn
        {
            get => SendCommand(Commands.GetAesOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetAesOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort Afc => SendCommand(Commands.GetAfc).ConvertToUInt16();

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
        public bool AfcLowBetaOn
        {
            get => SendCommand(Commands.GetAfcLowBetaOn).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetAfcLowBetaOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AutoRxRestartOn
        {
            get => SendCommand(Commands.GetAutoRxRestartOn).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetAutoRxRestartOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort BitRate
        {
            get => SendCommand(Commands.GetBitRate).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetBitRate} 0x{(int)value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte BroadcastAddress
        {
            get => SendCommand(Commands.GetBroadcastAddress).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetBroadcastAddress} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ContinuousDagc ContinuousDagc
        {
            get => (ContinuousDagc)SendCommand(Commands.GetContinuousDagc).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetContinuousDagc} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool CrcAutoClear
        {
            get => SendCommand(Commands.GetCrcAutoClear).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetCrcAutoClear} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool CrcOn
        {
            get => SendCommand(Commands.GetCrcOn).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetCrcOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public LnaGain CurrentLnaGain => (LnaGain)SendCommand(Commands.GetCurrentLnaGain).ConvertToInt32();

        ///<inheritdoc/>
        public DccFreq DccFreq
        {
            get => (DccFreq)SendCommand(Commands.GetDccFreq).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDccFreq} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public DccFreq DccFreqAfc
        {
            get => (DccFreq)SendCommand(Commands.GetDccFreqAfc).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDccFreqAfc} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public DcFree DcFree
        {
            get => (DcFree)SendCommand(Commands.GetDcFree).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDcFree} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public DioIrq DioInterruptMask
        {
            get => GetDioInterruptMask();
            set => SetDioInterrupMask(value);
        }

        ///<inheritdoc/>
        public EnterCondition EnterCondition
        {
            get => (EnterCondition)SendCommand(Commands.GetEnterCondition).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetEnterCondition} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ExitCondition ExitCondition
        {
            get => (ExitCondition)SendCommand(Commands.GetExitCondition).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetExitCondition} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort Fei => SendCommand(Commands.GetFei).ConvertToUInt16();

        ///<inheritdoc/>
        public IEnumerable<byte> Fifo
        {
            get => SendCommand(Commands.GetFifo).ToBytes();
            set => SendCommandWithCheck($"{Commands.SetFifo} {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool FifoFill
        {
            get => SendCommand(Commands.GetFifoFill).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetFifoFill} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte FifoThreshold
        {
            get => SendCommand(Commands.GetFifoThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetFifoThreshold} 0x{value:X}", ResponseOk);
        }

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
            set => SendCommandWithCheck($"{Commands.SetFrequencyDeviation} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public FskModulationShaping FskModulationShaping
        {
            get => (FskModulationShaping)SendCommand(Commands.GetFskModulationShaping).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFskModulationShaping} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool Impedance
        {
            get => SendCommand(Commands.GetImpedance).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetImpedance} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public IntermediateMode IntermediateMode
        {
            get => (IntermediateMode)SendCommand(Commands.GetIntermediateMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetIntermediateMode} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte InterPacketRxDelay
        {
            get => SendCommand(Commands.GetInterPacketRxDelay).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetInterPacketRxDelay} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public Irq Irq => GetIrqInternal();

        ///<inheritdoc/>
        public byte ListenCoefficentIdle
        {
            get => SendCommand(Commands.GetListenCoefficentIdle).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetListenCoefficentIdle} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte ListenCoefficentRx
        {
            get => SendCommand(Commands.GetListenCoefficentRx).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetListenCoefficentRx} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool ListenCriteria
        {
            get => SendCommand(Commands.GetListenCriteria).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetListenCriteria} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ListenEnd ListenEnd
        {
            get => (ListenEnd)SendCommand(Commands.GetListenEnd).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetListenEnd} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool ListenerOn
        {
            get => SendCommand(Commands.GetListenerOn).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetListenerOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ListenResolution ListenResolutionIdle
        {
            get => (ListenResolution)SendCommand(Commands.GetListenResolutionIdle).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetListenResolutionIdle} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ListenResolution ListenResolutionRx
        {
            get => (ListenResolution)SendCommand(Commands.GetListenResolutionRx).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetListenResolutionRx} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public LnaGain LnaGainSelect
        {
            get => (LnaGain)SendCommand(Commands.GetLnaGainSelect).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetLnaGainSelect} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte LowBetaAfcOffset
        {
            get => SendCommand(Commands.GetLowBetaAfcOffset).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetLowBetaAfcOffset} 0x{value:x}", ResponseOk);
        }

        ///<inheritdoc/>
        public Mode Mode
        {
            get => (Mode)SendCommand(Commands.GetMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetMode} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public Modulation Modulation
        {
            get => (Modulation)SendCommand(Commands.GetModulation).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetModulation} 0x{value:X}", ResponseOk);
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
        public int OutputPower
        {
            get => SendCommand(Commands.GetOutputPower).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOutputPower} 0x{value:X}", ResponseOk);
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
        public byte PayloadLength
        {
            get => SendCommand(Commands.GetPayloadLength).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetPayloadLength} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort PreambleSize
        {
            get => SendCommand(Commands.GetPreambleSize).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetPreambleSize} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RadioConfig
        {
            get => SendCommand(Commands.GetRadioConfig).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetRadioConfig} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte Rssi => SendCommand(Commands.GetRssi).ConvertToByte();

        ///<inheritdoc/>
        public byte RssiThreshold
        {
            get => SendCommand(Commands.GetRssiThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetRssiThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RxBw
        {
            get => Convert.ToByte(SendCommand(Commands.GetRxBw).Substring(0, 4), 16);
            set => SendCommandWithCheck($"{Commands.SetRxBw} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RxBwAfc
        {
            get => Convert.ToByte(SendCommand(Commands.GetRxBwAfc).Substring(0, 4), 16);
            set => SendCommandWithCheck($"{Commands.SetRxBwAfc} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool SensitivityBoost
        {
            get => SendCommand(Commands.GetSensitivityBoost).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetSensitivityBoost} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool Sequencer
        {
            get => SendCommand(Commands.GetSequencer).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetSequencer} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public IEnumerable<byte> Sync
        {
            get => SendCommand(Commands.GetSync).ToBytes();
            set => SendCommandWithCheck($"{Commands.SetSync} {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte SyncBitErrors
        {
            get => SendCommand(Commands.GetSyncBitErrors).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetSyncBitErrors} 0x{value:X}", ResponseOk);
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
            set => SendCommandWithCheck($"{Commands.SetSyncSize} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TemperatureValue => SendCommand(Commands.GetTemperatureValue).ConvertToByte();

        ///<inheritdoc/>
        public int Timeout
        {
            get => SerialPort.ReadTimeout;
            set
            {
                SerialPort.ReadTimeout = value;
                SerialPort.WriteTimeout = value;
            }
        }

        ///<inheritdoc/>
        public byte TimeoutRssiThreshold
        {
            get => SendCommand(Commands.GetTimeoutRssiThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutRssiThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TimeoutRxStart
        {
            get => SendCommand(Commands.GetTimeoutRxStart).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutRxStart} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool TxStartCondition
        {
            get => SendCommand(Commands.GetTxStartCondition).StartsWith("1");
            set => SendCommandWithCheck($"{Commands.SetTxStartCondition} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public string Version => SendCommand(Commands.GetVersion);

        ///<inheritdoc/>
        public void AfcClear()
        {
            SendCommandWithCheck(Commands.ExecuteAfcClear, ResponseOk);
        }

        ///<inheritdoc/>
        public void AfcStart()
        {
            SendCommandWithCheck(Commands.ExecuteAfcStart, ResponseOk);
        }

        ///<inheritdoc/>
        public void FeiStart()
        {
            SendCommandWithCheck(Commands.ExecuteFeiStart, ResponseOk);
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
        public IList<string> GetRadioConfigurations()
        {
            lock (SerialPort)
            {
                List<string> configs = new List<string>();

                var result = SendCommand(Commands.GetRadioConfig);

                configs.Add(result);

                do
                {
                    try
                    {
                        configs.Add(SerialPort.ReadLine());
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
        public void ListenAbort()
        {
            SendCommandWithCheck(Commands.ExecuteListenAbort, ResponseOk);
        }

        ///<inheritdoc/>
        public void MeasureTemperature()
        {
            SendCommandWithCheck(Commands.ExecuteMeasureTemperature, ResponseOk);
        }

        ///<inheritdoc/>
        public void RcCalibration()
        {
            SendCommandWithCheck(Commands.ExecuteRcCalibration, ResponseOk);
        }

        ///<inheritdoc/>
        public void Reset()
        {
            FlushSerialPort();
            SendCommandWithCheck(Commands.ExecuteReset, ResponseOk);
        }

        ///<inheritdoc/>
        public void RestartRx()
        {
            SendCommandWithCheck(Commands.ExecuteRestartRx, ResponseOk);
        }

        ///<inheritdoc/>
        public void SetAesKey(IEnumerable<byte> key)
        {
            SendCommandWithCheck($"{Commands.ExecuteSetAesKey} {BitConverter.ToString(key.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        ///<inheritdoc/>
        public void SetDioMapping(Dio dio, DioMapping mapping)
        {
            SendCommandWithCheck($"{Commands.SetDioMapping} {(int)dio} {(int)mapping}", ResponseOk);
        }

        ///<inheritdoc/>
        public void StartRssi()
        {
            SendCommandWithCheck(Commands.ExecuteStartRssi, ResponseOk);
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

        ///<inheritdoc/>
        public void WaitForIrq()
        {
            lock (SerialPort)
            {
                var irq = SerialPort.ReadLine();

                if (!irq.StartsWith("DIO PIN IRQ"))
                {
                    throw new RfmUsbCommandExecutionException($"Invalid response received for IRQ signal: [{irq}]");
                }
            }
        }

        private void CheckOpen()
        {
            if (SerialPort == null)
            {
                throw new InvalidOperationException("Instance not open");
            }
        }

        private void FlushSerialPort()
        {
            if (SerialPort != null)
            {
                Logger.LogDebug("Flushing Serial Ports input buffer");
                SerialPort.DiscardInBuffer();
            }
        }

        private DioIrq GetDioInterruptMask()
        {
            lock (SerialPort)
            {
                DioIrq irqMask = DioIrq.None;

                SerialPort.Write($"{Commands.GetDioInterrupt}\n");

                for (int i = 0; i < 6; i++)
                {
                    var result = SerialPort.ReadLine();

                    var parts = result.Split('-');

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
                }

                return irqMask;
            }
        }

        private Irq GetIrqInternal()
        {
            lock (SerialPort)
            {
                Irq irq = Irq.None;

                var result = SendCommand(Commands.GetIrq);

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

                    result = SerialPort.ReadLine();
                }

                return irq;
            }
        }

        private string SendCommand(string command)
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

        private void SendCommandWithCheck(string command, string response)
        {
            CheckOpen();

            var result = SendCommand(command);

            if (!result.StartsWith(response))
            {
                throw new RfmUsbCommandExecutionException($"Command: [{command}] Execution Failed Reason: [{result}]");
            }
        }

        private void SetDioInterrupMask(DioIrq value)
        {
            lock (SerialPort)
            {
                byte mask = (byte)(((byte)value) >> 1);

                SendCommandWithCheck($"{Commands.SetDio} 0x{mask:X}", ResponseOk);

                if (SerialPort.BytesToRead != 0)
                {
                    SerialPort.ReadLine();
                }
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
                    response = SerialPort.ReadLine();
                    Logger.LogDebug("Response: [{response}]", response);
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
    }
}