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
using System.Collections.Generic;

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
        public bool AutoImageCalibrationOn
        {
            get => SendCommand(Commands.GetAutoImageCalibrationOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetAutoImageCalibrationOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public AutoRestartRxMode AutoRestartRxMode
        { 
            get => (AutoRestartRxMode)SendCommand(Commands.GetAutoRestartRxMode).ConvertToInt32(); 
            set => SendCommandWithCheck($"{Commands.GetAutoRestartRxMode} 0x{value:X}", ResponseOk);
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
        public CodingRate CodingRate => (CodingRate)SendCommand(Commands.GetCodingRate).ConvertToInt32();

        ///<inheritdoc/>
        public bool CrcWhiteningType
        {
            get => SendCommand(Commands.GetCrcWhiteningType).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetCrcWhiteningType} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool FastHopOn
        {
            get => SendCommand(Commands.GetFastHopOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetFastHopOn} {(value ? "1" : "0")}", ResponseOk);
        }

        public override IEnumerable<byte> Fifo { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public int FreqError => SendCommand(Commands.GetFreqError).ConvertToInt32();

        ///<inheritdoc/>
        public byte FreqHoppingPeriod
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
            set => SendCommandWithCheck($"{Commands.SetFromPacketReceived} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public FromReceive FromReceive
        {
            get => (FromReceive)SendCommand(Commands.GetFromReceive).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetFromReceive} 0x{value:X}", ResponseOk);
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
        public byte HopChannel => SendCommand(Commands.GetHopChannel).ConvertToByte();

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
        public LoraMode LoraMode
        {
            get => (LoraMode)SendCommand(Commands.GetLoraMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetLoraMode} 0x{value:X}", ResponseOk);
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
            set => SendCommandWithCheck($"{Commands.SetLowBatteryTrim} 0x{value:X}", ResponseOk);
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
        public ModemBandwidth ModemBandwidth => (ModemBandwidth)SendCommand(Commands.GetModemBandwidth).ConvertToInt32();

        ///<inheritdoc/>
        public ModemStatus ModemStatus => (ModemStatus)SendCommand(Commands.GetModemStatus).ConvertToInt32();

        ///<inheritdoc/>
        public OokAverageOffset OokAverageOffset
        {
            get => (OokAverageOffset)SendCommand(Commands.GetOokAverageOffset).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOokAverageOffset} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte PacketRssi => SendCommand(Commands.GetPacketRssi).ConvertToByte();

        ///<inheritdoc/>
        public byte PacketSnr => SendCommand(Commands.GetPacketSnr).ConvertToByte();

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
            set => SendCommandWithCheck($"{Commands.SetPreambleDetectorSize} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte PreambleDetectorTotal
        {
            get => SendCommand(Commands.GetPreambleDetectorTotal).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetPreambleDetectorTotal} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ushort PreambleLength
        {
            get => SendCommand(Commands.GetPreambleLength).ConvertToUInt16();
            set => SendCommandWithCheck($"{Commands.SetPreambleLength} 0x{value:X}", ResponseOk);
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
            get => SendCommand(Commands.GetRssiOffset).ConvertToSByte();
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
            get => SendCommand(Commands.GetRssiThreshold).ConvertToSByte();
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
        public bool TempChange
        {
            get => SendCommand(Commands.GetTempChange).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetTempChange} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public TemperatureThreshold TemperatureThreshold
        {
            get => (TemperatureThreshold)SendCommand(Commands.GetTemperatureThreshold).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetTemperatureThreshold} 0x{value:X}", ResponseOk);
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
        public void ClearFifoOverrun()
        {
            SendCommandWithCheck(Commands.ExecuteClearFifoOverrun, ResponseOk);
        }

        ///<inheritdoc/>
        public void ClearLowBattery()
        {
            SendCommandWithCheck(Commands.ExecuteClearLowBattery, ResponseOk);
        }

        ///<inheritdoc/>
        public void ClearPreambleDetect()
        {
            SendCommandWithCheck(Commands.ExecuteClearPreambleDetect, ResponseOk);
        }

        ///<inheritdoc/>
        public void ClearSyncAddressMatch()
        {
            SendCommandWithCheck(Commands.ExecuteClearSyncAddressMatch, ResponseOk);
        }

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
            SendCommandWithCheck($"{Commands.SetTimerCoefficient} {(int)timer} {value}", ResponseOk);
        }

        ///<inheritdoc/>
        public void SetTimerResolution(Timer timer, TimerResolution value)
        {
            SendCommandWithCheck($"{Commands.SetTimerCoefficient} {(int)timer} {(int)value}", ResponseOk);
        }
    }
}