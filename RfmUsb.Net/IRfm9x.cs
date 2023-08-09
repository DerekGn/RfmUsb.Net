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
        /// <para><see langword="false"/> Access LoRa registers</para>
        /// <para><see langword="true"/> Access FSK registers</para>
        /// </remarks>
        bool AccessSharedRegisters { get; set; }

        /// <summary>
        /// The agc value
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> LNA gain set by register LnaGain</para>
        /// <para><see langword="true"/> LNA gain set by the internal AGC loop</para>
        /// </remarks>
        bool AgcAutoOn { get; set; }

        /// <summary>
        /// Controls the Image calibration mechanism
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> Calibration of the receiver depending on the temperature is disabled</para>
        /// <para><see langword="true"/> Calibration of the receiver depending on the temperature enabled</para>
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
        /// Enables the Bit Synchronizer.
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> Bit Sync disabled (not possible in Packet mode)</para>
        /// <para><see langword="true"/> Bit Sync enabled</para>
        /// </remarks>
        bool BitSyncOn { get; set; }

        /// <summary>
        /// Selects the CRC and whitening algorithms
        /// </summary>
        CrcWhiteningType CrcWhiteningType { get; set; }

        /// <summary>
        /// The Error coding rate
        /// </summary>
        ErrorCodingRate ErrorCodingRate { get; set; }

        /// <summary>
        /// Bypasses the main state machine for a quick frequency hop.
        /// </summary>
        bool FastHopOn { get; set; }

        /// <summary>
        /// The fifo address pointer
        /// </summary>
        byte FifoAddressPointer { get; set; }

        /// <summary>
        /// Read base address in FIFO data buffer for RX
        /// </summary>
        byte FifoRxBaseAddress { get; set; }

        /// <summary>
        /// Current value of RX databuffer pointer
        /// </summary>
        byte FifoRxByteAddressPointer { get; }

        /// <summary>
        /// Number of payload bytes of latest packetreceived
        /// </summary>
        byte FifoRxBytesNumber { get; }

        /// <summary>
        /// Start address (in data buffer) of last packet received
        /// </summary>
        byte FifoRxCurrentAddress { get; }

        /// <summary>
        /// Write base address in FIFO data buffer for TX
        /// </summary>
        byte FifoTxBaseAddress { get; set; }

        /// <summary>
        /// The Temperature saved during the latest IQ (RSSI and Image) calibration
        /// </summary>
        sbyte FormerTemperature { get; set; }

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
        /// <para><see langword="false"/> To transmit state</para>
        /// <para><see langword="false"/> To receive state</para>
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
        /// <para><see langword="false"/> To lowpowerselection on a packetsent interrupt</para>
        /// <para><see langword="true"/> To receive state on a packetsent interrupt</para>
        /// </remarks>
        bool FromTransmit { get; set; }

        /// <summary>
        /// FHSS channel information
        /// </summary>
        HopChannel HopChannel { get; }

        /// <summary>
        /// Selects the chip mode during the state.
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> standby mode</para>
        /// <para><see langword="true"/> sleep mode</para>
        /// </remarks>
        bool IdleMode { get; set; }

        /// <summary>
        /// The implicit header mode
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> explicit header mode</para>
        /// <para><see langword="true"/> implicit header mode</para>
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
        /// Get or set the Irq flags
        /// </summary>
        Rfm9xIrqFlags IrqFlags { get; set; }

        /// <summary>
        /// Estimation of SNR on last packet received.
        /// </summary>
        byte LastPacketSnr { get; }

        /// <summary>
        /// Enable Low Frequency (RFI_LF) LNA current adjustment
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> Default LNA current</para>
        /// <para><see langword="true"/> Boost on, 150% LNA current</para>
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
        /// <para><see langword="false"/> LNA gain set by register LnaGain</para>
        /// <para><see langword="true"/> LNA gain set by the internal AGC loop</para>
        /// </remarks>
        bool LoraAgcAutoOn { get; set; }

        /// <summary>
        /// Get the lora Irq flags
        /// </summary>
        /// <remarks>
        /// Setting a specific flag clears the corresponding irq
        /// </remarks>
        LoraIrqFlags LoraIrqFlags { get; set; }

        /// <summary>
        /// Irq flag mask
        /// </summary>
        LoraIrqFlagsMask LoraIrqFlagsMask { get; set; }

        /// <summary>
        /// Get the lora mode
        /// </summary>
        LoraMode LoraMode { get; set; }

        /// <summary>
        /// Payload length in bytes.
        /// </summary>
        /// <remarks>
        /// The register needs to be set in implicit header
        /// mode for the expected packet length. A 0 value is not permitted
        /// </remarks>
        byte LoraPayloadLength { get; set; }

        /// <summary>
        /// Low Battery detector enable signa
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> LowBat detector disabled</para>
        /// <para><see langword="true"/> LowBat detector enabled</para>
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
        /// <para><see langword="false"/> Disabled</para>
        /// <para><see langword="true"/> Enabled; mandated for when the symbol length exceeds 16ms</para>
        /// </remarks>
        bool LowDataRateOptimize { get; set; }

        /// <summary>
        /// Access Low Frequency Mode registers
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> High Frequency Mode (access to HF test registers)</para>
        /// <para><see langword="true"/> Low Frequency Mode(access to LF test registers)</para>
        /// </remarks>
        bool LowFrequencyMode { get; set; }

        /// <summary>
        /// Selects Sequencer LowPower state after a to LowPowerSelection transition
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> SequencerOff state with chip on Initial mode</para>
        /// <para><see langword="true"/> Idle state with chip on Standby or Sleep mode depending on IdleMode</para>
        /// </remarks>
        bool LowPowerSelection { get; set; }

        /// <summary>
        /// Allows the mapping of either Rssi Or PreambleDetect to the DIO pins
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> Rssi interrupt</para>
        /// <para><see langword="true"/> PreambleDetect interrupt</para>
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
        /// Get or set the output power in dbm
        /// </summary>
        byte OutputPower { get; set; }
        /// <summary>
        /// RSSI of the latest packet received (dBm)
        /// </summary>
        byte PacketRssi { get; }

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
        /// Number of Preamble bytes to detect to trigger an interrupt
        /// </summary>
        PreambleDetectorSize PreambleDetectorSize { get; set; }

        /// <summary>
        /// Number or chip errors tolerated overPreambleDetectorSize. 4 chips per bit
        /// </summary>
        byte PreambleDetectorTotalerance { get; set; }

        /// <summary>
        /// The lora Preamble length
        /// </summary>
        ushort PreambleLength { get; set; }

        /// <summary>
        /// Sets the polarity of the Preamble
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> 0xAA (default)</para>
        /// <para><see langword="true"/> 0x55</para>
        /// </remarks>
        bool PreamblePolarity { get; set; }

        /// <summary>
        /// Turns on the mechanism restarting the receiver
        /// automatically if it gets saturated or a packet
        /// collision is detected
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> No automatic Restart</para>
        /// <para><see langword="true"/> Automatic restart On</para>
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
        /// <para><see langword="false"/> CRC disable</para>
        /// <para><see langword="true"/> CRC enable</para>
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
        /// <para><see langword="false"/> Crystal Oscillator with external Crystal</para>
        /// <para><see langword="true"/> External clipped sine TCXO AC-connected to XTA pin</para>
        /// </remarks>
        bool TcxoInputOn { get; set; }

        /// <summary>
        /// IRQ flag witnessing a temperature change exceeding
        /// TempThreshold since the last Image and RSSI calibration:
        /// <remarks>
        /// <para><see langword="false"/> Temperature change lower than TempThreshold</para>
        /// <para><see langword="true"/> Temperature change greater than TempThreshold</para>
        /// </remarks>
        /// </summary>
        bool TemperatureChange { get; }
        /// <summary>
        /// Temperature change threshold to trigger a new I/Q calibration
        /// </summary>
        TemperatureThreshold TemperatureThreshold { get; set; }

        /// <summary>
        /// Controls the temperature monitoring
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> Temperature monitoring done in all modes except Sleep and Standby</para>
        /// <para><see langword="true"/> Temperature monitoring stopped</para>
        /// </remarks>
        bool TempMonitorOff { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutRxPreamble*16*Tbit after
        /// switching to Rx mode if Preamble interrupt doesn’t occur
        /// </summary>
        /// <remarks>
        /// 0 Disabled
        /// </remarks>
        byte TimeoutRxPreamble { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutRxRssi*16*Tbit after
        /// switching to Rx mode if Rssi interrupt doesn’t occur
        /// (i.e. RssiValue > RssiThreshold)
        /// </summary>
        /// <remarks>
        /// 0 Disabled
        /// </remarks>
        byte TimeoutRxRssi { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutSignalSync*16*Tbit after
        /// the Rx mode is programmed, if SyncAddress doesn’t occur
        /// </summary>
        /// <remarks>
        /// 0 Disabled
        /// </remarks>
        byte TimeoutSignalSync { get; set; }

        /// <summary>
        /// Tx continous mode
        /// </summary>
        /// <remarks>
        /// <para><see langword="false"/> Normal mode, a single packet is sent</para>
        /// <para><see langword="true"/> Continuous mode, send multiple packets across the FIFO</para>
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