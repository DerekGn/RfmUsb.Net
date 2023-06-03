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

#warning TODO rfm95 Irq flags

namespace RfmUsb.Net
{
    /// <summary>
    /// An rfm9x device
    /// </summary>
    public interface IRfm9x : IRfm
    {
        /// <summary>
        /// Enable agc auto on
        /// </summary>
        /// <remarks>
        /// 0: LNA gain set by register LnaGain
        /// 1: LNA gain set by the internal AGC loop
        /// </remarks>
        bool LoraAgcAutoOn { get; set; }

        bool AutoImageCalibrationOn { get; set; }

        bool BeaconOn { get; set; }

        byte BitRateFractional { get; set; }

        bool BitSyncOn { get; set; }

        /// <summary>
        /// The Error coding rate
        /// </summary>
        CodingRate CodingRate { get; }

        bool CrcWhiteningType { get; set; }

        bool FastHopOn { get; set; }

        byte FifoAddressPointer { get; set; }

        byte FifoRxBaseAddress { get; set; }

        byte FifoRxByteAddressPointer { get; }

        byte FifoRxBytesNumber { get; }

        byte FifoTxBaseAddress { get; set; }

        /// <summary>
        /// The Temperature saved during the latest IQ (RSSI and Image) calibration
        /// </summary>
        sbyte FormerTemperatureValue { get; set; }

        int FreqError { get; }

        byte FreqHoppingPeriod { get; set; }

        bool FromIdle { get; set; }

        FromPacketReceived FromPacketReceived { get; set; }

        FromReceive FromReceive { get; set; }

        FromRxTimeout FromRxTimeout { get; set; }

        FromStart FromStart { get; set; }

        bool FromTransmit { get; set; }

        byte HopChannel { get; }

        bool IdleMode { get; set; }

        /// <summary>
        /// The Implicit header mode
        /// </summary>
        bool ImplicitHeaderModeOn { get; set; }

        bool IoHomeOn { get; set; }

        bool IoHomePowerFrame { get; set; }

        /// <summary>
        /// Enable Low Frequency (RFI_LF) LNA current adjustment
        /// </summary>
        bool LnaBoostHf { get; set; }

        /// <summary>
        /// Switch the device to long range mode
        /// </summary>
        bool LongRangeMode { get; set; }

        /// <summary>
        /// Get the lora mode
        /// </summary>
        LoraMode LoraMode { get; set; }

        bool LowBatteryOn { get; set; }

        LowBatteryTrim LowBatteryTrim { get; set; }

        bool LowDataRateOptimize { get; set; }

        /// <summary>
        /// Access Low Frequency Mode registers
        /// </summary>
        bool LowFrequencyMode { get; set; }

        bool LowPowerSelection { get; set; }

        bool MapPreambleDetect { get; set; }

        ModemBandwidth ModemBandwidth { get; }

        ModemStatus ModemStatus { get; }

        /// <summary>
        /// Static offset added to the threshold in average mode in order to reduce glitching activity (OOK only)
        /// </summary>
        OokAverageOffset OokAverageOffset { get; set; }
        byte PacketRssi { get; }
        byte PacketSnr { get; }
        byte PayloadMaxLength { get; set; }
        byte PpmCorrection { get; set; }
        bool PreambleDetectorOn { get; set; }
        PreambleDetectorSize PreambleDetectorSize { get; set; }
        byte PreambleDetectorTotal { get; set; }
        ushort PreambleLength { get; set; }
        bool PreamblePolarity { get; set; }
        bool RestartRxOnCollision { get; set; }
        byte RssiCollisionThreshold { get; set; }
        byte RssiOffset { get; set; }
        RssiSmoothing RssiSmoothing { get; set; }
        byte RssiThreshold { get; set; }
        byte RssiWideband { get; }
        byte RxCodingRate { get; }
        bool RxPayloadCrcOn { get; set; }

        /// <summary>
        /// The spreading factor rate
        /// </summary>
        SpreadingFactor SpreadingFactor { get; set; }

        ushort SymbolTimeout { get; set; }

        /// <summary>
        /// Controls the crystal oscillator
        /// </summary>
        /// <remarks>
        /// 0: Crystal Oscillator with external Crystal
        /// 1: External clipped sine TCXO AC-connected to XTA pin
        /// </remarks>
        bool TcxoInputOn { get; set; }

        bool TempChange { get; set; }
        TemperatureThreshold TemperatureThreshold { get; set; }
        bool TempMonitorOff { get; set; }
        byte TimeoutRxPreamble { get; set; }
        byte TimeoutRxRssi { get; set; }
        byte TimeoutSignalSync { get; set; }
        bool TxContinuousMode { get; set; }

        byte ValidHeaderCount { get; }

        byte ValidPacketCount { get; }

        void ClearFifoOverrun();

        void ClearLowBattery();

        void ClearPreambleDetect();

        void ClearSyncAddressMatch();

        void ExecuteAgcStart();

        void ExecuteImageCalibration();

        void ExecuteSequencerStart();

        void ExecuteSequencerStop();

        byte GetTimerCoefficient(Timer timer);

        TimerResolution GetTimerResolution(Timer timer);

        void ExecuteRestartRxWithoutPllLock();

        void ExecuteRestartRxWithPllLock();
    }
}