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

using System.Runtime.Intrinsics.X86;

namespace RfmUsb.Net
{
    /// <summary>
    /// An rfm9x device
    /// </summary>
    public interface IRfm9x : IRfm
    {
        /// <summary>
        /// This bit operates when device is in Lora mode; if set it allows
        /// access to FSK registers page located in address space
        /// (0x0D:0x3F) while in LoRa mode
        /// </summary>
        /// <remarks>
        /// 0 : Access LoRa registers
        /// 1 : Access FSK registers
        /// </remarks>
        bool AccessSharedRegisters { get; set; }

        /// <summary>
        /// Controls the Image calibration mechanism
        /// </summary>
        /// <remarks>
        /// 0 : Calibration of the receiver depending on the temperature is disabled
        /// 1 : Calibration of the receiver depending on the temperature enabled.
        /// </remarks>
        bool AutoImageCalibrationOn { get; set; }

        /// <summary>
        /// Controls the automatic restart of the receiver after the reception of
        /// a valid packet(PayloadReady or CrcOk)
        /// </summary>
        AutoRestartRxMode AutoRestartRxMode { get; set; }

        /// <summary>
        /// Enables the Beacon mode in Fixed packetformat
        /// </summary>
        bool BeaconOn { get; set; }

        /// <summary>
        /// Fractional part of the bit rate divider (Only valid for FSK)
        /// </summary>
        byte BitRateFractional { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
        byte FifoRxCurrentAddress { get; }
#warning TODO check accesssharedregisters state in fsk
        byte FifoTxBaseAddress { get; set; }

        /// <summary>
        /// The Temperature saved during the latest IQ (RSSI and Image) calibration
        /// </summary>
        sbyte FormerTemperatureValue { get; set; }

        /// <summary>
        /// Estimated frequency error from modem
        /// </summary>
        int FrequencyError { get; }

        /// <summary>
        /// Symbol periods between frequency hops. (0 = disabled).
        /// 1st hop always happen after the 1st header symbol
        /// </summary>
        byte FrequencyHoppingPeriod { get; set; }

        /// <summary>
        /// Controls the sequencer transition from the 
        /// idle state on a T1 interrupt.
        /// </summary>
        /// <remarks>
        /// 0 : To transmit state
        /// 0 : To receive state
        /// </remarks>
        bool FromIdle { get; set; }

        /// <summary>
        /// Controls the state-machine transition from the PacketReceived state
        /// </summary>
        FromPacketReceived FromPacketReceived { get; set; }

        /// <summary>
        /// Controls the Sequencer transition from the Receivestate
        /// </summary>
        FromReceive FromReceive { get; set; }

        /// <summary>
        /// Controls the state-machine transition from the 
        /// Receive state on a RxTimeout interrupt
        /// </summary>
        FromRxTimeout FromRxTimeout { get; set; }

        /// <summary>
        /// Controls the sequencer transition when sequencer 
        /// is a set to true in sleep or standby mode
        /// </summary>
        FromStart FromStart { get; set; }

        /// <summary>
        /// Controls the sequencer transition from the transmit state
        /// </summary>
        /// <remarks>
        /// 0: To lowpowerselection on a packetsent interrupt
        /// 1: To receive state on a packetsent interrupt
        /// </remarks>
        bool FromTransmit { get; set; }

        /// <summary>
        /// Current value of frequency hopping channel inuse
        /// </summary>
        byte HopChannel { get; }

        /// <summary>
        /// Selects the chip mode during the state.
        /// </summary>
        /// <remarks>
        /// 0: standby mode
        /// 1: sleep mode
        /// </remarks>
        bool IdleMode { get; set; }

        /// <summary>
        /// The implicit header mode
        /// </summary>
        /// <remarks>
        /// 0: explicit header mode
        /// 1: implicit header mode
        /// </remarks>
        bool ImplicitHeaderModeOn { get; set; }

        /// <summary>
        /// Enables the io-homecontrol compatibility mode
        /// </summary>
        bool IoHomeOn { get; set; }

        /// <summary>
        /// Linked to io-homecontrol compatibility mode
        /// </summary>
        bool IoHomePowerFrame { get; set; }

        /// <summary>
        /// Enable Low Frequency (RFI_LF) LNA current adjustment
        /// </summary>
        /// <remarks>
        /// 0: Default LNA current
        /// 1: Boost on, 150% LNA current
        /// </remarks>
        bool LnaBoostHf { get; set; }

        /// <summary>
        /// Switch the device to long range mode
        /// </summary>
        bool LongRangeMode { get; set; }

        /// <summary>
        /// Enable agc auto on
        /// </summary>
        /// <remarks>
        /// 0: LNA gain set by register LnaGain
        /// 1: LNA gain set by the internal AGC loop
        /// </remarks>
        bool LoraAgcAutoOn { get; set; }

        /// <summary>
        /// Get the lora mode
        /// </summary>
        LoraMode LoraMode { get; set; }

        /// <summary>
        /// Low Battery detector enable signa
        /// </summary>
        /// <remarks>
        /// 0: LowBat detector disabled
        /// 1: LowBat detector enabled
        /// </remarks>
        bool LowBatteryOn { get; set; }

        /// <summary>
        /// Trimming of the LowBat threshold
        /// </summary>
        LowBatteryTrim LowBatteryTrim { get; set; }

        /// <summary>
        /// Low data rate optimize
        /// </summary>
        /// <remarks>
        /// 0: Disabled
        /// 1: Enabled; mandated for when the symbol length exceeds 16ms
        /// </remarks>
        bool LowDataRateOptimize { get; set; }

        /// <summary>
        /// Access Low Frequency Mode registers
        /// </summary>
        /// <remarks>
        /// 0: High Frequency Mode (access to HF test registers)
        /// 1: Low Frequency Mode(access to LF test registers)
        /// </remarks>
        bool LowFrequencyMode { get; set; }

        /// <summary>
        /// Access Low Frequency Mode registers
        /// </summary>
        /// <remarks>
        /// 0 : High Frequency Mode
        /// 1 : Low Frequency Mode
        /// </remarks>
        bool LowFrequencyModeOn { get; set; }

        /// <summary>
        /// Selects Sequencer LowPower state after a to LowPowerSelection transition
        /// </summary>
        /// <remarks>
        /// 0: SequencerOff state with chip on Initial mode
        /// 1: Idle state with chip on Standby or Sleep mode depending on IdleMode
        /// </remarks>
        bool LowPowerSelection { get; set; }

        /// <summary>
        /// Allows the mapping of either Rssi Or PreambleDetect to the DIO pins
        /// </summary>
        /// <remarks>
        /// 0: Rssi interrupt
        /// 1: PreambleDetect interrupt
        /// </remarks>
        bool MapPreambleDetect { get; set; }

        /// <summary>
        /// The lora modem bandwidth
        /// </summary>
        ModemBandwidth ModemBandwidth { get; set; }

        /// <summary>
        /// The modem status
        /// </summary>
        ModemStatus ModemStatus { get; }

        /// <summary>
        /// Static offset added to the threshold in average mode in order to reduce glitching activity (OOK only)
        /// </summary>
        OokAverageOffset OokAverageOffset { get; set; }

        /// <summary>
        /// RSSI of the latest packet received (dBm)
        /// </summary>
        byte PacketRssi { get; }

        /// <summary>
        /// Estimation of SNR on last packet received.
        /// </summary>
        byte PacketSnr { get; }

        /// <summary>
        /// Maximum payload length; if header payload length exceeds value a 
        /// header CRC error is generated.Allows filtering of packet with a bad size.
        /// </summary>
        byte PayloadMaxLength { get; set; }

        /// <summary>
        /// Data rate offset value, used in conjunction with AFC
        /// </summary>
        byte PpmCorrection { get; set; }

        /// <summary>
        /// Enables Preamble detector when set to 1. The AGC settings
        /// supersede this bit during the startup / AGC phase.
        /// </summary>
        bool PreambleDetectorOn { get; set; }

        /// <summary>
        /// Number of Preamble bytes to detect to trigger aninterrupt
        /// </summary>
        PreambleDetectorSize PreambleDetectorSize { get; set; }

        /// <summary>
        /// Number or chip errors tolerated overPreambleDetectorSize. 4 chips per bit
        /// </summary>
        byte PreambleDetectorTotal { get; set; }

        /// <summary>
        /// Sets the polarity of the Preamble
        /// </summary>
        /// <remarks>
        /// 0 : 0xAA (default)
        /// 1 : 0x55
        /// </remarks>
        bool PreamblePolarity { get; set; }

        /// <summary>
        /// Turns on the mechanism restarting the receiver 
        /// automatically if it gets saturated or a packet 
        /// collision is detected
        /// </summary>
        /// <remarks>
        /// 0 : No automatic Restart
        /// 1 : Automatic restart On
        /// </remarks>
        bool RestartRxOnCollision { get; set; }

        /// <summary>
        /// Sets the threshold used to consider that an interferer is detected, 
        /// witnessing a packet collision. 1dB/LSB
        /// </summary>
        byte RssiCollisionThreshold { get; set; }

        /// <summary>
        /// Signed RSSI offset, to compensate for the possible losses/gains in the front-end
        /// </summary>
        sbyte RssiOffset { get; set; }

        /// <summary>
        /// Defines the number of samples taken to average the RSSI result
        /// </summary>
        RssiSmoothing RssiSmoothing { get; set; }

        /// <summary>
        /// RSSI trigger level for the Rssi interrupt
        /// </summary>
        sbyte RssiThreshold { get; set; }

        /// <summary>
        /// Wideband RSSI measurement used to locally generate
        /// a random number
        /// </summary>
        byte RssiWideband { get; }

        /// <summary>
        /// Coding rate of last header received
        /// </summary>
        byte RxCodingRate { get; }

        /// <summary>
        /// Enable CRC generation and check on payload
        /// 0: CRC disable
        /// 1: CRC enable
        /// </summary>
        bool RxPayloadCrcOn { get; set; }

        /// <summary>
        /// The spreading factor rate
        /// </summary>
        SpreadingFactor SpreadingFactor { get; set; }

        /// <summary>
        /// RX symbol timeout
        /// </summary>
        ushort SymbolTimeout { get; set; }

        /// <summary>
        /// Controls the crystal oscillator
        /// </summary>
        /// <remarks>
        /// 0: Crystal Oscillator with external Crystal
        /// 1: External clipped sine TCXO AC-connected to XTA pin
        /// </remarks>
        bool TcxoInputOn { get; set; }

        /// <summary>
        /// Temperature change threshold to trigger a new I/Q calibration
        /// </summary>
        TemperatureThreshold TemperatureThreshold { get; set; }

        /// <summary>
        /// Controls the temperature monitoring
        /// </summary>
        /// <remarks>
        /// false Temperature monitoring done in all modes except Sleep and Standby
        /// Treu Temperature monitoring stopped.
        /// </remarks>
        bool TempMonitorOff { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutRxPreamble*16*Tbit after
        /// switching to Rx mode if Preamble interrupt doesn’t occur
        /// </summary>
        /// <remarks>
        /// 0 Diabled
        /// </remarks>
        byte TimeoutRxPreamble { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutRxRssi*16*Tbit after
        /// switching to Rx mode if Rssi interrupt doesn’t occur
        /// (i.e. RssiValue > RssiThreshold)
        /// </summary>
        /// <remarks>
        /// 0 Diabled
        /// </remarks>
        byte TimeoutRxRssi { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutSignalSync*16*Tbit after
        /// the Rx mode is programmed, if SyncAddress doesn’t occur
        /// </summary>
        /// /// <remarks>
        /// 0 Diabled
        /// </remarks>
        byte TimeoutSignalSync { get; set; }

        /// <summary>
        /// Tx continous mode
        /// </summary>
        /// <remarks>
        /// false : Normal mode, a single packet is sent
        /// true : Continuous mode, send multiple packets across the FIFO
        /// (used for spectral analysis)
        /// </remarks>
        bool TxContinuousMode { get; set; }

        /// <summary>
        /// Number of valid headers received since last transition into Rx mode
        /// </summary>
        /// <remarks>
        /// Header and packet counters are reset in Sleep mode.
        /// </remarks>
        byte ValidHeaderCount { get; }

        /// <summary>
        /// Number of valid headers received since last transition into Rx mode
        /// </summary>
        /// <remarks>
        /// Header and packet counters are reset in Sleep mode.
        /// </remarks>
        byte ValidPacketCount { get; }

        /// <summary>
        /// Clear the fifo over run irq flag
        /// </summary>
        void ClearFifoOverrun();

        /// <summary>
        /// Clear the low battery irq flag
        /// </summary>
        void ClearLowBattery();

        /// <summary>
        /// Clear the preamble detect irq flag
        /// </summary>
        void ClearPreambleDetect();

        /// <summary>
        /// Clear the sync address match irq
        /// </summary>
        /// <remarks>
        /// Only cleared in Continuous mode
        /// </remarks>
        void ClearSyncAddressMatch();

        /// <summary>
        /// Start an AGC sequence
        /// </summary>
        void ExecuteAgcStart();

        /// <summary>
        /// Triggers the IQ and RSSI calibration when set in Standby mode
        /// </summary>
        void ExecuteImageCalibration();

        /// <summary>
        /// Triggers a manual Restart of the Receiver chain.
        /// Use this when there is no frequency change,
        /// <see cref="ExecuteRestartRxWithPllLock"/> otherwise
        /// </summary>
        void ExecuteRestartRxWithoutPllLock();

        /// <summary>
        /// Triggers a manual Restart of the Receiver chain.
        /// Use this when there is a frequency change,
        /// requiring some time for the PLL to re-lock.
        /// </summary>
        void ExecuteRestartRxWithPllLock();

        /// <summary>
        /// Execute the sequencer start condition.
        /// </summary>
        /// <remarks>
        /// the sequencer can only be enabled when the chip is in sleep or standby mode
        /// </remarks>
        void ExecuteSequencerStart();

        /// <summary>
        /// Forces the sequencer to stop
        /// </summary>
        void ExecuteSequencerStop();

        /// <summary>
        /// Get the timer coefficent
        /// </summary>
        /// <param name="timer">The timer to read</param>
        /// <returns>The coefficent</returns>
        byte GetTimerCoefficient(Timer timer);

        /// <summary>
        /// Get the timer resolution
        /// </summary>
        /// <param name="timer">The timer to read</param>
        /// <returns>The <see cref="TimerResolution"/></returns>
        TimerResolution GetTimerResolution(Timer timer);

        /// <summary>
        /// Set the timer coefficent
        /// </summary>
        /// <param name="timer">The <see cref="Timer"/> to read</param>
        /// <param name="value">value to set</param>
        void SetTimerCoefficient(Timer timer, byte value);

        /// <summary>
        /// Set the timer resolution
        /// </summary>
        /// <param name="timer">The <see cref="Timer"/> to read</param>
        /// <param name="value">The <see cref="TimerResolution"/> value to set</param>
        void SetTimerResolution(Timer timer, TimerResolution value);
    }
}