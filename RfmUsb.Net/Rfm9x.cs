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
        public bool AutoImageCalibrationOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool BeaconOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte BirRateFractional { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool BitSyncOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public CodingRate CodingRate => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public bool CrcWhiteningType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool FastHopOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public bool FromTransmit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte HopChannel => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public bool IdleMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool ImplicitHeaderModeOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool IoHomeOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool IoHomePowerFrame { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LnaBoostHf { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LongRangeMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public LoraMode LoraMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LowBatteryOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public LowBatteryTrim LowBatteryTrim { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LowDataRateOptimize { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LowFrequencyMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool LowPowerSelection { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool MapPreambleDetect { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public bool PreambleDetectorOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public PreambleDetectorSize PreambleDetectorSize { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte PreambleDetectorTotal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public ushort PreambleLength { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool PreamblePolarity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool RestartRxOnCollision { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        public bool RxPayloadCrcOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public SpreadingFactor SpredingFactor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public ushort SymbolTimeout { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool TcxoInputOn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool TempChange { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public TemperatureThreshold TemperatureThreshold { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool TempMonitorOff { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte TimeoutRxPreamble { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte TimeoutRxRssi { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public byte TimeoutSignalSync { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        ///<inheritdoc/>
        public bool TxContinuousMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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