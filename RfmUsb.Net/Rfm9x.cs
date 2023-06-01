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
        public bool LoraAgcAutoOn
        {
            get => SendCommand(Commands.GetLoraAgcAutoOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLoraAgcAutoOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AutoImageCalibrationOn
        {
            get => SendCommand(Commands.GetAutoImageCalibrationOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetAutoImageCalibrationOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool BeaconOn
        {
            get => SendCommand(Commands.GetBeaconOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetBeaconOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte BirRateFractional { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool BitSyncOn
        {
            get => SendCommand(Commands.GetBitSyncOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetBitSyncOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public CodingRate CodingRate => throw new System.NotImplementedException();

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

        ///<inheritdoc/>
        public byte FifoAddressPointer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte FifoRxBaseAddress { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte FifoRxByteAddressPointer => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte FifoRxBytesNumber => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte FifoTxBaseAddress { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public sbyte FormerTemperatureValue { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public int FreqError => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte FreqHoppingPeriod { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool FromIdle { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public FromPacketReceived FromPacketReceived { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public FromReceive FromReceive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public FromRxTimeout FromRxTimeout { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public FromStart FromStart { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool FromTransmit
        {
            get => SendCommand(Commands.GetFromTransmit).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetFromTransmit} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte HopChannel => throw new System.NotImplementedException();

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
        public LoraMode LoraMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LowBatteryOn
        {
            get => SendCommand(Commands.GetLowBatteryOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetLowBatteryOn} {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public LowBatteryTrim LowBatteryTrim { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public ModemBw ModemBandwidth => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public ModemStatus ModemStatus => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte PacketRssi => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte PacketSnr => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte PayloadMaxLength { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte PpmCorrection { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool PreambleDetectorOn
        {
            get => SendCommand(Commands.GetPreambleDetectorOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetPreambleDetectorOn} {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public PreambleDetectorSize PreambleDetectorSize { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte PreambleDetectorTotal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public ushort PreambleLength { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public byte RssiCollisionThreshold { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte RssiOffset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public RssiSmoothing RssiSmoothing { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte RssiThreshold { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte RssiWideband => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte RxCodingRate => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public bool RxPayloadCrcOn
        {
            get => SendCommand(Commands.GetRxPayloadCrcOn).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetRxPayloadCrcOn} {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public SpreadingFactor SpredingFactor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public ushort SymbolTimeout { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public TemperatureThreshold TemperatureThreshold { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool TempMonitorOff
        {
            get => SendCommand(Commands.GetTempMonitorOff).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetTempMonitorOff} {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte TimeoutRxPreamble { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte TimeoutRxRssi { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte TimeoutSignalSync { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool TxContinuousMode
        {
            get => SendCommand(Commands.GetTxContinuousMode).Substring(0, 1) == "1";
            set => SendCommandWithCheck($"{Commands.SetTxContinuousMode} {(value ? "1" : "0")}", ResponseOk);
        }
        ///<inheritdoc/>
        public byte ValidHeaderCount => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public byte ValidPacketCount => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public void ClearFifoOverrun()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ClearLowBattery()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ClearPreambleDetect()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ClearSyncAddressMatch()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ExecuteAgcStart()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ExecuteImageCalibration()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ExecuteSequencerStart()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void ExecuteSequencerStop()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public byte GetTimerCoefficient(Timer timer)
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public TimerResolution GetTimerResolution(Timer timer)
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void RestartRxWithoutPllLock()
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public void RestartRxWithPllLock()
        {
            throw new System.NotImplementedException();
        }
    }
}