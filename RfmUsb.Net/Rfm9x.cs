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
using RfmUsb.Net.Extensions;
using RfmUsb.Net.Ports;

namespace RfmUsb.Net
{
    /// <summary>
    /// An implementation of the <see cref="IRfm9x"/> interface
    /// </summary>
    public class Rfm9x : RfmBase, IRfm9x
    {
        /// <summary>
        /// Create an instance of a <see cref="Rfm9x"/> device
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{T}"/> for logging</param>
        /// <param name="serialPortFactory">The <see cref="ISerialPortFactory"/> instance for creating and querying serial port instances</param>
        public Rfm9x(ILogger<IRfm> logger, ISerialPortFactory serialPortFactory) : base(logger, serialPortFactory)
        {
        }

        ///<inheritdoc/>
        public bool AccessSharedRegisters
        {
            get => SendCommand(Commands.GetAccessSharedRegisters).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetAccessSharedRegisters} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AgcAutoOn
        {
            get => SendCommand(Commands.GetAgcAutoOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetAgcAutoOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AutoImageCalibrationOn
        {
            get => SendCommand(Commands.GetAutoImageCalibrationOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetAutoImageCalibrationOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public AutoRestartRxMode AutoRestartRxMode
        {
            get => (AutoRestartRxMode)SendCommand(Commands.GetAutoRestartRxMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetAutoRestartRxMode} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool BeaconOn
        {
            get => SendCommand(Commands.GetBeaconOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetBeaconOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte BitRateFractional
        {
            get => SendCommand(Commands.GetBitRateFractional).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetBitRateFractional} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool BitSyncOn
        {
            get => SendCommand(Commands.GetBitSyncOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetBitSyncOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public CrcWhiteningType CrcWhiteningType
        {
            get => (CrcWhiteningType)SendCommand(Commands.GetCrcWhiteningType).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetCrcWhiteningType} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public ErrorCodingRate ErrorCodingRate
        {
            get => (ErrorCodingRate)SendCommand(Commands.GetErrorCodingRate).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetErrorCodingRate} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool FastHopOn
        {
            get => SendCommand(Commands.GetFastHopOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetFastHopOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte FifoAddressPointer
        {
            get => SendCommand(Commands.GetFifoAddressPointer).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetFifoAddressPointer} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte FifoRxBaseAddress
        {
            get => SendCommand(Commands.GetFifoRxBaseAddress).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetFifoRxBaseAddress} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte FifoRxByteAddressPointer => SendCommand(Commands.GetFifoRxByteAddressPointer).ConvertToByte();

        ///<inheritdoc/>
        public byte FifoRxBytesNumber => SendCommand(Commands.GetFifoRxBytesNumber).ConvertToByte();

        ///<inheritdoc/>
        public byte FifoRxCurrentAddress => SendCommand(Commands.GetFifoRxCurrentAddress).ConvertToByte();

        ///<inheritdoc/>
        public byte FifoTxBaseAddress
        {
            get => SendCommand(Commands.GetFifoTxBaseAddress).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetFifoTxBaseAddress} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public sbyte FormerTemperatureValue
        {
            get => SendCommand(Commands.GetFormerTemperatureValue).ConvertToSByte();
            set => SendCommandWithCheck($"{Commands.SetFormerTemperatureValue} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public int FrequencyError => SendCommand(Commands.GetFreqError).ConvertToInt32();

        ///<inheritdoc/>
        public byte FrequencyHoppingPeriod
        {
            get => SendCommand(Commands.GetFreqHoppingPeriod).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetFreqHoppingPeriod} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool FromIdle
        {
            get => SendCommand(Commands.GetFromIdle).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetFromIdle} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public FromPacketReceived FromPacketReceived
        {
            get => (FromPacketReceived)SendCommand(Commands.GetFromPacketReceived).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFromPacketReceived} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public FromReceive FromReceive
        {
            get => (FromReceive)SendCommand(Commands.GetFromReceive).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFromReceive} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public FromRxTimeout FromRxTimeout
        {
            get => (FromRxTimeout)SendCommand(Commands.GetFromRxTimeout).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFromRxTimeout} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public FromStart FromStart
        {
            get => (FromStart)SendCommand(Commands.GetFromStart).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFromStart} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool FromTransmit
        {
            get => SendCommand(Commands.GetFromTransmit).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetFromTransmit} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public HopChannel HopChannel => GetHopChannel();

        ///<inheritdoc/>
        public bool IdleMode
        {
            get => SendCommand(Commands.GetIdleMode).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetIdleMode} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool ImplicitHeaderModeOn
        {
            get => SendCommand(Commands.GetImplicitHeaderModeOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetImplicitHeaderModeOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool IoHomeOn
        {
            get => SendCommand(Commands.GetIoHomeOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetIoHomeOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool IoHomePowerFrame
        {
            get => SendCommand(Commands.GetIoHomePowerFrame).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetIoHomePowerFrame} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public Rfm9xIrqFlags IrqFlags
        {
            get => GetIrqInternal();
            set => SendCommandWithCheck($"{Commands.SetIrqFlags} 0x{(ushort)value:X4}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte LastPacketSnr => SendCommand(Commands.GetLastPacketSnr).ConvertToByte();

        ///<inheritdoc/>
        public bool LnaBoostHf
        {
            get => SendCommand(Commands.GetLnaBoostHf).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLnaBoostHf} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool LongRangeMode
        {
            get => SendCommand(Commands.GetLongRangeMode).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLongRangeMode} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool LoraAgcAutoOn
        {
            get => SendCommand(Commands.GetLoraAgcAutoOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLoraAgcAutoOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public LoraIrqFlags LoraIrqFlags
        {
            get => GetLoraIrqFlags();
            set => SendCommandWithCheck($"{Commands.SetLoraIrqFlags} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public LoraIrqFlagsMask LoraIrqFlagsMask
        {
            get => GetLoraIrqFlagsMask();
            set => SendCommandWithCheck($"{Commands.SetLoraIrqFlagsMask} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public LoraMode LoraMode
        {
            get => (LoraMode)SendCommand(Commands.GetLoraMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetLoraMode} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte LoraPayloadLength
        {
            get => SendCommand(Commands.GetLoraPayloadLength).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetLoraPayloadLength} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool LowBatteryOn
        {
            get => SendCommand(Commands.GetLowBatteryOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLowBatteryOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public LowBatteryTrim LowBatteryTrim
        {
            get => (LowBatteryTrim)SendCommand(Commands.GetLowBatteryTrim).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetLowBatteryTrim} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool LowDataRateOptimize
        {
            get => SendCommand(Commands.GetLowDataRateOptimize).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLowDataRateOptimize} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool LowFrequencyMode
        {
            get => SendCommand(Commands.GetLowFrequencyMode).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLowFrequencyMode} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool LowPowerSelection
        {
            get => SendCommand(Commands.GetLowPowerSelection).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLowPowerSelection} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool MapPreambleDetect
        {
            get => SendCommand(Commands.GetMapPreambleDetect).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetMapPreambleDetect} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ModemBandwidth ModemBandwidth
        {
            get => (ModemBandwidth)SendCommand(Commands.GetModemBandwidth).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetModemBandwidth} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ModemStatus ModemStatus => GetModemStatus();

        ///<inheritdoc/>
        public OokAverageOffset OokAverageOffset
        {
            get => (OokAverageOffset)SendCommand(Commands.GetOokAverageOffset).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokAverageOffset} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte PacketRssi => SendCommand(Commands.GetPacketRssi).ConvertToByte();

        ///<inheritdoc/>
        public byte PayloadMaxLength
        {
            get => SendCommand(Commands.GetPayloadMaxLength).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetPayloadMaxLength} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte PpmCorrection
        {
            get => SendCommand(Commands.GetPpmCorrection).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetPpmCorrection} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool PreambleDetectorOn
        {
            get => SendCommand(Commands.GetPreambleDetectorOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetPreambleDetectorOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public PreambleDetectorSize PreambleDetectorSize
        {
            get => (PreambleDetectorSize)SendCommand(Commands.GetPreambleDetectorSize).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetPreambleDetectorSize} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte PreambleDetectorTotalerance
        {
            get => SendCommand(Commands.GetPreambleDetectorTotalerance).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetPreambleDetectorTotalerance} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort PreambleLength
        {
            get => SendCommand(Commands.GetPreambleLength).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetPreambleLength} 0x{value:X4}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool PreamblePolarity
        {
            get => SendCommand(Commands.GetPreamblePolarity).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetPreamblePolarity} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool RestartRxOnCollision
        {
            get => SendCommand(Commands.GetRestartRxOnCollision).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetRestartRxOnCollision} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RssiCollisionThreshold
        {
            get => SendCommand(Commands.GetRssiCollisionThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetRssiCollisionThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public sbyte RssiOffset
        {
            get => (sbyte)SendCommand(Commands.GetRssiOffset).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetRssiOffset} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public RssiSmoothing RssiSmoothing
        {
            get => (RssiSmoothing)SendCommand(Commands.GetRssiSmoothing).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetRssiSmoothing} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public sbyte RssiThreshold
        {
            get => (sbyte)SendCommand(Commands.GetRssiThreshold).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetRssiThreshold} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte RssiWideband => SendCommand(Commands.GetRssiWideband).ConvertToByte();

        ///<inheritdoc/>
        public byte RxCodingRate => SendCommand(Commands.GetRxCodingRate).ConvertToByte();

        ///<inheritdoc/>
        public bool RxPayloadCrcOn
        {
            get => SendCommand(Commands.GetRxPayloadCrcOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetRxPayloadCrcOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public SpreadingFactor SpreadingFactor
        {
            get => (SpreadingFactor)SendCommand(Commands.GetSpreadingFactor).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetSpreadingFactor} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort SymbolTimeout
        {
            get => SendCommand(Commands.GetSymbolTimeout).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetSymbolTimeout} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool TcxoInputOn
        {
            get => SendCommand(Commands.GetTcxoInputOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetTcxoInputOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool TemperatureChange => SendCommand(Commands.GetTemperatureChange).Substring(0, 1) == "1";

        ///<inheritdoc/>
        public TemperatureThreshold TemperatureThreshold
        {
            get => (TemperatureThreshold)SendCommand(Commands.GetTemperatureThreshold).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetTemperatureThreshold} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool TempMonitorOff
        {
            get => SendCommand(Commands.GetTempMonitorOff).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetTempMonitorOff} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TimeoutRxPreamble
        {
            get => SendCommand(Commands.GetTimeoutRxPreamble).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutRxPreamble} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TimeoutRxRssi
        {
            get => SendCommand(Commands.GetTimeoutRxRssi).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutRxRssi} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TimeoutSignalSync
        {
            get => SendCommand(Commands.GetTimeoutSignalSync).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutSignalSync} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool TxContinuousMode
        {
            get => SendCommand(Commands.GetTxContinuousMode).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetTxContinuousMode} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte ValidHeaderCount => SendCommand(Commands.GetValidHeaderCount).ConvertToByte();

        ///<inheritdoc/>
        public byte ValidPacketCount => SendCommand(Commands.GetValidPacketCount).ConvertToByte();

        ///<inheritdoc/>
        public void ExecuteAgcStart()
        {
            SendCommandWithCheck(Commands.ExecuteAgcStart, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteImageCalibration()
        {
            SendCommandWithCheck(Commands.ExecuteImageCalibration, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteRestartRxWithoutPllLock()
        {
            SendCommandWithCheck(Commands.ExecuteRestartRxWithoutPllLock, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteRestartRxWithPllLock()
        {
            SendCommandWithCheck(Commands.ExecuteRestartRxWithPllLock, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteSequencerStart()
        {
            SendCommandWithCheck(Commands.ExecuteSequencerStart, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteSequencerStop()
        {
            SendCommandWithCheck(Commands.ExecuteSequencerStop, ResponseOk);
        }

        ///<inheritdoc/>
        public byte GetTimerCoefficient(Timer timer)
        {
            return SendCommand($"{Commands.GetTimerCoefficient} {(int)timer}").ConvertToByte();
        }

        ///<inheritdoc/>
        public TimerResolution GetTimerResolution(Timer timer)
        {
            return (TimerResolution)SendCommand($"{Commands.GetTimerResolution} {(int)timer}").ConvertToInt32();
        }

        ///<inheritdoc/>
        public void SetTimerCoefficient(Timer timer, byte value)
        {
            SendCommandWithCheck($"{Commands.SetTimerCoefficient} {(int)timer} 0x{value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public void SetTimerResolution(Timer timer, TimerResolution value)
        {
            SendCommandWithCheck($"{Commands.SetTimerResolution} {(int)timer} 0x{(byte)value:X2}", ResponseOk);
        }

        private HopChannel GetHopChannel()
        {
            var lines = SendCommandListResponse(Commands.GetHopChannel);

            bool pll = false;
            bool crc = false;
            byte channel = 0;

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[1])
                {
                    case "PLL_TIMEOUT":
                        if (parts[0] == "1")
                        {
                            pll = true;
                        }
                        break;

                    case "CRC_ON_PAYLOAD":
                        if (parts[0] == "1")
                        {
                            crc = true;
                        }
                        break;

                    case "FHSS_PRESENT_CHANNEL":
                        channel = parts[0].ConvertToByte();
                        break;
                }
            });

            return new HopChannel(pll, crc, channel);
        }

        private Rfm9xIrqFlags GetIrqInternal()
        {
            Rfm9xIrqFlags irq = Rfm9xIrqFlags.None;

            var lines = SendCommandListResponse(Commands.GetIrqFlags);

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[1])
                {
                    case "LOW_BATTERY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.LowBattery;
                        }
                        break;

                    case "CRC_OK":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.CrcOK;
                        }
                        break;

                    case "PAYLOAD_READY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.PayloadReady;
                        }
                        break;

                    case "PACKET_SENT":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.PacketSent;
                        }
                        break;

                    case "FIFO_OVERRUN":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.FifoOverrun;
                        }
                        break;

                    case "FIFO_LEVEL":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.FifoLevel;
                        }
                        break;

                    case "FIFO_NOT_EMPTY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.FifoNotEmpty;
                        }
                        break;

                    case "FIFO_FULL":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.FifoFull;
                        }
                        break;

                    case "ADDRESS_MATCH":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.SyncAddressMatch;
                        }
                        break;

                    case "PREAMBLE_DETECT":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.PreambleDetect;
                        }
                        break;

                    case "TIMEOUT":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.Timeout;
                        }
                        break;

                    case "RSSI":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.Rssi;
                        }
                        break;

                    case "PLL_LOCK":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.PllLock;
                        }
                        break;

                    case "TX_RDY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.TxReady;
                        }
                        break;

                    case "RX_RDY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.RxReady;
                        }
                        break;

                    case "MODE_RDY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm9xIrqFlags.ModeReady;
                        }
                        break;
                }
            });

            return irq;
        }

        private LoraIrqFlags GetLoraIrqFlags()
        {
            LoraIrqFlags irq = LoraIrqFlags.None;

            var lines = SendCommandListResponse(Commands.GetLoraIrqFlags);

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[1])
                {
                    case "CAD_DETECTED":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.CadDetected;
                        }
                        break;

                    case "FHSS_CHANGE_CHANNEL":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.FhssChangeChannel;
                        }
                        break;

                    case "CAD_DONE":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.CadDone;
                        }
                        break;

                    case "TX_DONE":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.TxDone;
                        }
                        break;

                    case "VALID_HEADER":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.ValidHeader;
                        }
                        break;

                    case "RX_DONE":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.RxDone;
                        }
                        break;

                    case "RX_TIMEOUT":
                        if (parts[0] == "1")
                        {
                            irq |= LoraIrqFlags.RxTimeout;
                        }
                        break;
                }
            });

            return irq;
        }

        private LoraIrqFlagsMask GetLoraIrqFlagsMask()
        {
            LoraIrqFlagsMask mask = LoraIrqFlagsMask.None;

            var lines = SendCommandListResponse(Commands.GetLoraIrqFlagsMask);

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[1])
                {
                    case "CAD_DETECTED_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.CadDetectedMask;
                        }
                        break;

                    case "FHSS_CHANGE_CHANNEL_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.FhssChangeChannelMask;
                        }
                        break;

                    case "CAD_DONE_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.CadDoneMask;
                        }
                        break;

                    case "TX_DONE_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.TxDoneMask;
                        }
                        break;

                    case "VALID_HEADER_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.ValidHeaderMask;
                        }
                        break;

                    case "RX_DONE_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.RxDoneMask;
                        }
                        break;

                    case "RX_TIMEOUT_MASK":
                        if (parts[0] == "1")
                        {
                            mask |= LoraIrqFlagsMask.RxTimeoutMask;
                        }
                        break;
                }
            });

            return mask;
        }

        private ModemStatus GetModemStatus()
        {
            ModemStatus status = ModemStatus.None;

            var lines = SendCommandListResponse(Commands.GetModemStatus);

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[1])
                {
                    case "SIGNAL_DETECTED":
                        if (parts[0] == "1")
                        {
                            status |= ModemStatus.SignalDetected;
                        }
                        break;

                    case "SIGNAL_SYNCHRONIZED":
                        if (parts[0] == "1")
                        {
                            status |= ModemStatus.SignalSynchronized;
                        }
                        break;

                    case "RX_ONGOING":
                        if (parts[0] == "1")
                        {
                            status |= ModemStatus.RxOnGoing;
                        }
                        break;

                    case "HEADER_INFO_VALID":
                        if (parts[0] == "1")
                        {
                            status |= ModemStatus.HeaderInfoValid;
                        }
                        break;

                    case "MODEM_CLEAR":
                        if (parts[0] == "1")
                        {
                            status |= ModemStatus.ModemClear;
                        }
                        break;
                }
            });

            return status;
        }
    }
}