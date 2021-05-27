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

using System;
using System.Collections.Generic;

namespace RfmUsb
{
    /// <summary>
    /// An rfm69 Usb device
    /// </summary>
    public interface IRfmUsb : IDisposable
    {
        /// <summary>
        /// Get or set the fifo data
        /// </summary>
        IEnumerable<byte> Fifo { get; set; }
        /// <summary>
        /// Get or set the automatic Sequencer enable
        /// </summary>
        bool Sequencer { get; set; }
        /// <summary>
        /// Enables Listen mode, should be enabled whilst in Standby mode
        /// </summary>
        bool ListenerOn { get; set; }
        /// <summary>
        /// Get or set the current mode
        /// </summary>
        Mode Mode { get; set; }
        /// <summary>
        /// Get or set the modulation type
        /// </summary>
        Modulation Modulation { get; set; }
        /// <summary>
        /// The Fsk data shaping
        /// </summary>
        FskModulationShaping FskModulationShaping { get; set; }
        /// <summary>
        /// The Ook data shaping
        /// </summary>
        OokModulationShaping OokModulationShaping { get; set; }
        /// <summary>
        /// Get or set the radio Tx/Rx bit rate
        /// </summary>
        ushort BitRate { get; set; }
        /// <summary>
        /// Get or set the frequency deviation
        /// </summary>
        ushort FrequencyDeviation { get; set; }
        /// <summary>
        /// Get or set the frequency
        /// </summary>
        uint Frequency { get; set; }
        /// <summary>
        /// Improved AFC routine for signals with modulation index lower than 2.
        /// </summary>
        bool AfcLowBetaOn { get; set; }
        /// <summary>
        /// The resolution of Listen mode Idle time 
        /// </summary>
        ListenResolution ListenResolutionIdle { get; set; }
        /// <summary>
        /// The resolution of Listen mode Rx time 
        /// </summary>
        ListenResolution ListenResolutionRx { get; set; }
        /// <summary>
        /// Criteria for packet acceptance in Listen mode:
        /// false → signal strength is above RssiThreshold
        /// true → signal strength is above RssiThreshold and SyncAddress matched
        /// </summary>
        bool ListenCriteria { get; set; }
        /// <summary>
        /// Action taken after acceptance of a packet in Listen mode
        /// </summary>
        ListenEnd ListenEnd { get; set; }
        /// <summary>
        /// Duration of the Idle phase in Listen mode
        /// </summary>
        byte ListenCoefficentIdle { get; set; }
        /// <summary>
        /// Duration of the Idle phase in Rx phase
        /// </summary>
        byte ListenCoefficentRx { get; set; }
        /// <summary>
        /// Get the FrmUsb version
        /// </summary>
        string Version { get; }
        /// <summary>
        /// Get or set the output power in dbm
        /// </summary>
        int OutputPower { get; set; }
        /// <summary>
        /// Rise/Fall time of ramp up/down in FSK
        /// </summary>
        PaRamp PaRamp { get; set; }
        /// <summary>
        /// Enables overload current protection (OCP) for the PA
        /// </summary>
        bool OcpEnable { get; set; }
        /// <summary>
        /// Trimming of OCP current
        /// </summary>
        OcpTrim OcpTrim { get; set; }
        /// <summary>
        /// LNA’s input impedance
        /// false → 50 ohms
        /// true → 200 ohms
        /// </summary>
        bool Impedance { get; set; }
        /// <summary>
        /// Current LNA gain, set either manually, or by the AGC
        /// </summary>
        LnaGain CurrentLnaGain { get; }
        /// <summary>
        /// LNA gain setting
        /// </summary>
        LnaGain LnaGainSelect { get; set; }
        /// <summary>
        /// Cut-off frequency of the DC offset canceller (DCC)
        /// </summary>
        DccFreq DccFreq { get; set; }
        /// <summary>
        /// Gets the Rx channel filter bandwidth
        /// </summary>
        byte RxBw { get; set; }
        /// <summary>
        /// Cut-off frequency of the DC offset canceller (DCC)
        /// </summary>
        DccFreq DccFreqAfc { get; set; }
        /// <summary>
        /// Gets the Rx channel filter bandwidth for Afc
        /// </summary>
        byte RxBwAfc { get; set; }
        /// <summary>
        /// Selects type of threshold in the OOK data slicer
        /// </summary>
        OokThresholdType OokThresholdType { get; set; }
        /// <summary>
        /// Size of each decrement of the RSSI threshold in the OOKdemodulator
        /// </summary>
        OokThresholdStep OokPeakThresholdStep { get; set; }
        /// <summary>
        /// Period of decrement of the RSSI threshold in the OOK demodulator
        /// </summary>
        OokThresholdDec OokPeakThresholdDec { get; set; }
        /// <summary>
        /// Filter coefficients in average mode of the OOK demodulator
        /// </summary>
        OokAverageThresholdFilter OokAverageThresholdFilter { get; set; }
        /// <summary>
        /// Fixed threshold value (in dB) in the OOK demodulator.
        /// Used when OokThresholdType is <see cref="OokThresholdType.Fixed"/>
        /// </summary>
        byte OokFixedThreshold { get; set; }
        /// <summary>
        /// Only valid if AfcAutoOn is set 
        /// false → AFC register is not cleared before a new AFC phase
        /// true → AFC register is cleared before a new AFC phase
        /// </summary>
        bool AfcAutoClear { get; set; }
        /// <summary>
        /// false → AFC is performed each time AfcStart is set
        /// true → AFC is performed each time Rx mode is entered
        /// </summary>
        bool AfcAutoOn { get; set; }
        /// <summary>
        /// The Afc value
        /// </summary>
        ushort Afc { get; }
        /// <summary>
        /// The Fei value
        /// </summary>
        ushort Fei { get; }
        /// <summary>
        /// Absolute value of the RSSI in dBm, 0.5dB steps. RSSI = -RssiValue/2 [dBm
        /// </summary>
        byte Rssi { get; }
        /// <summary>
        /// Get the Irq flags
        /// </summary>
        Irq Irq { get; }
        /// <summary>
        /// RSSI trigger level for Rssi interrupt
        /// </summary>
        byte RssiThreshold { get; set; }
        /// <summary>
        /// Timeout interrupt is generated TimeoutRxStart*16*Tbit after switching to Rx mode 
        /// if Rssi interrupt doesn’t occur(i.e. RssiValue > RssiThreshold)
        /// 0x00: TimeoutRxStart is disabled
        /// </summary>
        byte TimeoutRxStart { get; set; }
        /// <summary>
        /// Timeout interrupt is generated TimeoutRxStart*16*Tbit after switching to Rx mode 
        /// if Rssi interrupt doesn’t occur(i.e. RssiValue > RssiThreshold)
        /// 0x00: TimeoutRxStart is disabled
        /// </summary>
        byte TimeoutRssiThreshold { get; set; }
        /// <summary>
        /// Size of the preamble to be sent (from TxStartConditionfulfilled)
        /// </summary>
        ushort PreambleSize { get; set; }
        /// <summary>
        /// Enable sync word generation and detection
        /// </summary>
        bool SyncEnable { get; set; }
        /// <summary>
        /// FIFO filling condition: 
        /// false → if SyncAddress interrupt occurs
        /// true → as long as FifoFillCondition is set
        /// </summary>
        bool FifoFill { get; set; }
        /// <summary>
        /// Get or set the number of tolerated bit errors in Sync word
        /// </summary>
        byte SyncBitErrors { get; set; }
        /// <summary>
        /// The radio sync bytes
        /// </summary>
        IEnumerable<byte> Sync { get; set; }
        /// <summary>
        /// The radio sync word size
        /// </summary>
        byte SyncSize { get; set; }
        /// <summary>
        /// Defines the packet format used:
        /// false → Fixed length
        /// true → Variable length
        /// </summary>
        bool PacketFormat { get; set; }
        /// <summary>
        /// Defines DC-free encoding/decoding performed
        /// </summary>
        DcFree DcFree { get; set; }
        /// <summary>
        /// Enables CRC calculation/check (Tx/Rx)
        /// false → Off
        /// true → On
        /// </summary>
        bool CrcOn { get; set; }
        /// <summary>
        /// Defines the behavior of the packet handler when CRC check fails:
        /// false → Clear FIFO and restart new packet reception. NoPayloadReady interrupt issued.
        /// true → Do not clear FIFO. PayloadReady interrupt issued.
        /// </summary>
        bool CrcAutoClear { get; set; }
        /// <summary>
        /// Defines address based filtering in Rx
        /// </summary>
        AddressFilter AddressFiltering { get; set; }
        /// <summary>
        /// If PacketFormat = false (fixed), payload length.
        /// If PacketFormat = true (variable), max length in Rx, not used in Tx.
        /// </summary>
        byte PayloadLength { get; set; }
        /// <summary>
        /// Node address used in address filtering
        /// </summary>
        byte NodeAddress { get; set; }
        /// <summary>
        /// Broadcast address used in address filtering
        /// </summary>
        byte BroadcastAddress { get; set; }
        /// <summary>
        /// Interrupt condition for entering the intermediate mode
        /// </summary>
        EnterCondition EnterCondition { get; set; }
        /// <summary>
        /// Interrupt condition for exiting the intermediate mode
        /// </summary>
        ExitCondition ExitCondition { get; set; }
        /// <summary>
        /// Intermediate mode
        /// </summary>
        IntermediateMode IntermediateMode { get; set; }
        /// <summary>
        /// Defines the condition to start packet transmission : 
        /// false → FifoLevel (i.e. the number of bytes in the FIFO exceeds FifoThreshold)
        /// true → FifoNotEmpty (i.e. at least one byte in the FIFO)
        /// </summary>
        bool TxStartCondition { get; set; }
        /// <summary>
        /// Used to trigger FifoLevel interrupt.
        /// </summary>
        byte FifoThreshold { get; set; }
        /// <summary>
        /// After PayloadReady occurred, defines the delay between FIFO empty and the 
        /// start of a new RSSI phase for next packet. Must match the transmitter’s PA ramp-down time.
        /// - Tdelay = 0 if InterpacketRxDelay >= 12 
        /// - Tdelay = (2^InterpacketRxDelay) / BitRate otherwise
        /// </summary>
        byte InterPacketRxDelay { get; set; }
        /// <summary>
        /// Enables automatic Rx restart (RSSI phase) after PayloadReady occurred and packet has been completely read from FIFO: 
        /// false → Off. RestartRx can be used. 
        /// true→ On. Rx automatically restarted after InterPacketRxDelay.
        /// </summary>
        bool AutoRxRestartOn { get; set; }
        /// <summary>
        /// Enable the AES encryption/decryption: 
        /// false → Off
        /// true → On (payload limited to 66 bytes maximum)
        /// </summary>
        bool AesOn { get; set; }
        /// <summary>
        /// Measured temperature value
        /// </summary>
        byte TemperatureValue { get; }
        /// <summary>
        /// High sensitivity or normal sensitivity mode
        /// </summary>
        bool SensitivityBoost { get; set; }
        /// <summary>
        /// Fading Margin Improvement
        /// </summary>
        ContinuousDagc ContinuousDagc {get;set;}
        /// <summary>
        /// AFC offset set for low modulation index systems, used if AfcLowBetaOn = true.
        /// </summary>
        byte LowBetaAfcOffset { get; set; }
        /// <summary>
        /// The Dio interrupt mask
        /// </summary>
        DioIrq DioInterruptMask { get; set; }
        /// <summary>
        /// Get or set the serial port timeout
        /// </summary>
        int Timeout { get; set; }
        /// <summary>
        /// Wait for a configured Irq to be signaled
        /// </summary>
        void WaitForIrq();
        /// <summary>
        /// Sets the radio configuration to one of the preset configuration sets.
        /// </summary>
        byte RadioConfig { get; set; }
        /// <summary>
        /// Reset the radio
        /// </summary>
        public void Reset();
        /// <summary>
        /// Close the connection to the RfmUsb device
        /// </summary>
        public void Close();
        /// <summary>
        /// Open the RfmUsb device
        /// </summary>
        /// <param name="serialPort">The serial port</param>
        /// <param name="baudRate">The baud rate</param>
        public void Open(string serialPort, int baudRate);
        /// <summary>
        /// Abort listen mode
        /// </summary>
        void ListenAbort();
        /// <summary>
        /// Triggers the calibration of the RC oscillator
        /// </summary>
        void RcCalibration();
        /// <summary>
        /// Triggers a FEI measurement
        /// </summary>
        void FeiStart();
        /// <summary>
        /// Clears the AfcValue if set in Rx mode
        /// </summary>
        void AfcClear();
        /// <summary>
        /// Triggers an AFC
        /// </summary>
        void AfcStart();
        /// <summary>
        /// Trigger a RSSI measurement
        /// </summary>
        void StartRssi();
        /// <summary>
        /// Get a list of the pre-configured radio configurations
        /// </summary>
        /// <returns></returns>
        IList<string> GetRadioConfigurations();
        /// <summary>
        /// Set the <see cref="Dio"/> mapping configuration <see cref="DioMapping"/>
        /// </summary>
        /// <param name="dio">The <see cref="Dio"/> configuration</param>
        /// <param name="mapping">The <see cref="DioMapping"/></param>
        void SetDioMapping(Dio dio, DioMapping mapping);
        /// <summary>
        /// Get the <see cref="Dio"/> mapping configuration <see cref="DioMapping"/>
        /// </summary>
        /// <param name="dio">The <see cref="Dio"/></param>
        /// <param name="mapping">The <see cref="DioMapping"/></param>
        void GetDioMapping(Dio dio, out DioMapping mapping);
        /// <summary>
        /// Forces the Receiver in WAIT mode, in Continuous Rx mode.
        /// </summary>
        void RestartRx();
        /// <summary>
        /// Execute a temperature measurement
        /// </summary>
        void MeasureTemperature();
        /// <summary>
        /// Set the AES encryption key
        /// </summary>
        /// <param name="key"></param>
        void SetAesKey(IEnumerable<byte> key);
        /// <summary>
        /// Transmit a packet of data bytes and wait for a response
        /// </summary>
        /// <param name="data">The data to transmit</param>
        IList<byte> TransmitReceive(IList<byte> data);
        /// <summary>
        /// Transmit a packet of data bytes and wait for a response
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txTimeout">The timeout in milliseconds </param>
        /// <returns>The received packet bytes</returns>
        IList<byte> TransmitReceive(IList<byte> data, int txTimeout);
        /// <summary>
        /// Transmit a packet of data bytes and wait for a response
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txTimeout">The transmit timeout in milliseconds</param>
        /// <param name="rxTimeout">The receive timeout in milliseconds</param>
        IList<byte> TransmitReceive(IList<byte> data, int txTimeout, int rxTimeout);
        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        void Transmit(IList<byte> data);
        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txTimeout">The transmit timeout</param>
        void Transmit(IList<byte> data, int txTimeout);
    }
}
