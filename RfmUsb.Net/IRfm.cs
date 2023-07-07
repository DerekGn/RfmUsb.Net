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

using System;
using System.Collections.Generic;

namespace RfmUsb.Net
{
    /// <summary>
    /// An Rfm device
    /// </summary>
    public interface IRfm : IDisposable
    {
        /// <summary>
        /// Defines address based filtering in Rx
        /// </summary>
        AddressFilter AddressFiltering { get; set; }

        /// <summary>
        /// The Afc value
        /// </summary>
        short Afc { get; }

        /// <summary>
        /// Only valid if AfcAutoOn is set
        /// false : AFC register is not cleared before a new AFC phase
        /// true : AFC register is cleared before a new AFC phase
        /// </summary>
        bool AfcAutoClear { get; set; }

        /// <summary>
        /// false : AFC is performed each time AfcStart is set
        /// true : AFC is performed each time Rx mode is entered
        /// </summary>
        bool AfcAutoOn { get; set; }

        /// <summary>
        /// Get or set the radio Tx/Rx bit rate
        /// </summary>
        uint BitRate { get; set; }

        /// <summary>
        /// Broadcast address used in address filtering
        /// </summary>
        byte BroadcastAddress { get; set; }

        /// <summary>
        /// Defines the behavior of the packet handler when CRC check fails:
        /// false : Clear FIFO and restart new packet reception. NoPayloadReady interrupt issued.
        /// true : Do not clear FIFO. PayloadReady interrupt issued.
        /// </summary>
        bool CrcAutoClearOff { get; set; }

        /// <summary>
        /// Enables CRC calculation/check (Tx/Rx)
        /// false : Off
        /// true : On
        /// </summary>
        bool CrcOn { get; set; }

        /// <summary>
        /// Defines DC-free encoding/decoding performed
        /// </summary>
        DcFreeEncoding DcFreeEncoding { get; set; }

        /// <summary>
        /// Gets or sets the Dio Interrupt Mask
        /// </summary>
        DioIrq DioInterruptMask { get; set; }

        /// <summary>
        /// The Fei value
        /// </summary>
        short Fei { get; }

        /// <summary>
        /// Get or set the fifo data
        /// </summary>
        IEnumerable<byte> Fifo { get; set; }

        /// <summary>
        /// Used to trigger FifoLevel interrupt.
        /// </summary>
        byte FifoThreshold { get; set; }

        /// <summary>
        /// Get the RfmUsb firmware version
        /// </summary>
        string FirmwareVersion { get; }

        /// <summary>
        /// Get or set the frequency
        /// </summary>
        uint Frequency { get; set; }

        /// <summary>
        /// Get or set the frequency deviation
        /// </summary>
        ushort FrequencyDeviation { get; set; }

        /// <summary>
        /// The Fsk data shaping
        /// </summary>
        FskModulationShaping FskModulationShaping { get; set; }

        /// <summary>
        /// After PayloadReady occurred, defines the delay between FIFO empty and the
        /// start of a new RSSI phase for next packet. Must match the transmitter’s PA ramp-down time.
        /// - Tdelay = 0 if InterpacketRxDelay >= 12
        /// - Tdelay = (2^InterpacketRxDelay) / BitRate otherwise
        /// </summary>
        byte InterPacketRxDelay { get; set; }

        /// <summary>
        /// Get the Rssi value after last packet recieved
        /// </summary>
        byte LastRssi { get; }

        /// <summary>
        /// LNA gain setting
        /// </summary>
        LnaGain LnaGainSelect { get; set; }

        /// <summary>
        /// Get or set the current mode
        /// </summary>
        Mode Mode { get; set; }

        /// <summary>
        /// Get or set the modulation type
        /// </summary>
        ModulationType ModulationType { get; set; }

        /// <summary>
        /// Node address used in address filtering
        /// </summary>
        byte NodeAddress { get; set; }

        /// <summary>
        /// Enables overload current protection (OCP) for the PA
        /// </summary>
        bool OcpEnable { get; set; }

        /// <summary>
        /// Trimming of OCP current
        /// </summary>
        OcpTrim OcpTrim { get; set; }

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
        /// The Ook data shaping
        /// </summary>
        OokModulationShaping OokModulationShaping { get; set; }

        /// <summary>
        /// Period of decrement of the RSSI threshold in the OOK demodulator
        /// </summary>
        OokThresholdDec OokPeakThresholdDec { get; set; }

        /// <summary>
        /// Size of each decrement of the RSSI threshold in the OOKdemodulator
        /// </summary>
        OokThresholdStep OokPeakThresholdStep { get; set; }

        /// <summary>
        /// Selects type of threshold in the OOK data slicer
        /// </summary>
        OokThresholdType OokThresholdType { get; set; }

        /// <summary>
        /// Defines the packet format used:
        /// false : Fixed length
        /// true : Variable length
        /// </summary>
        bool PacketFormat { get; set; }

        /// <summary>
        /// Rise/Fall time of ramp up/down in FSK
        /// </summary>
        PaRamp PaRamp { get; set; }

        /// <summary>
        /// The payload length
        /// If PacketFormat = false (fixed), payload length.
        /// If PacketFormat = true (variable), max length in Rx, not used in Tx.
        /// </summary>
        ushort PayloadLength { get; set; }

        /// <summary>
        /// Size of the preamble to be sent (from TxStartConditionfulfilled)
        /// </summary>
        ushort PreambleSize { get; set; }

        /// <summary>
        /// The current predefined radio configuration
        /// </summary>
        byte RadioConfig { get; set; }

        /// <summary>
        /// Get the RfmUsb radio version
        /// </summary>
        byte RadioVersion { get; }

        /// <summary>
        /// Absolute value of the RSSI in dBm, 0.5dB steps. RSSI = -RssiValue/2 [dBm
        /// </summary>
        sbyte Rssi { get; }

        /// <summary>
        /// Gets the Rx channel filter bandwidth
        /// </summary>
        byte RxBw { get; set; }

        /// <summary>
        /// Gets the Rx channel filter bandwidth for Afc
        /// </summary>
        byte RxBwAfc { get; set; }

        /// <summary>
        /// Get the mcu serial number
        /// </summary>
        string SerialNumber { get; }

        /// <summary>
        /// The sync bytes
        /// </summary>
        IEnumerable<byte> Sync { get; set; }

        /// <summary>
        /// Enable sync word generation and detection
        /// </summary>
        bool SyncEnable { get; set; }

        /// <summary>
        /// The sync word size
        /// </summary>
        byte SyncSize { get; set; }

        /// <summary>
        /// Measured temperature value
        /// </summary>
        byte Temperature { get; }

        /// <summary>
        /// Defines the condition to start packet transmission :
        /// false : FifoLevel (i.e. the number of bytes in the FIFO exceeds FifoThreshold)
        /// true : FifoNotEmpty (i.e. at least one byte in the FIFO)
        /// </summary>
        bool TxStartCondition { get; set; }

        /// <summary>
        /// Close the connection to the RfmUsb device
        /// </summary>
        void Close();

        /// <summary>
        /// Enter the device bootloader
        /// </summary>
        void EnterBootloader();

        /// <summary>
        /// Reset the radio settings to default
        /// </summary>
        void ExecuteReset();

        /// <summary>
        /// Get the <see cref="Dio"/> mapping configuration <see cref="DioMapping"/>
        /// </summary>
        /// <param name="dio">The <see cref="Dio"/></param>
        /// <returns>The <see cref="DioMapping"/></returns>
        DioMapping GetDioMapping(Dio dio);

        /// <summary>
        /// Get the list of predefined radio configurations.
        /// </summary>
        /// <returns>The discriptive list of ratio configurations</returns>
        IList<string> GetRadioConfigurations();

        /// <summary>
        /// Open the RfmUsb device
        /// </summary>
        /// <param name="serialPort">The serial port</param>
        /// <param name="baudRate">The baud rate</param>
        void Open(string serialPort, int baudRate);

        /// <summary>
        /// Triggers the calibration of the RC oscillator
        /// </summary>
        void RcCalibration();

        /// <summary>
        /// Set the <see cref="Dio"/> mapping configuration <see cref="DioMapping"/>
        /// </summary>
        /// <param name="dio">The <see cref="Dio"/> configuration</param>
        /// <param name="mapping">The <see cref="DioMapping"/></param>
        void SetDioMapping(Dio dio, DioMapping mapping);

        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        void Transmit(IList<byte> data);

        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txCount">The number of transmissions</param>
        void Transmit(IList<byte> data, int txCount);

        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txCount">The number of transmissions</param>
        /// <param name="txInterval">The interval between transmissions</param>
        void Transmit(IList<byte> data, int txCount, int txInterval);

        /// <summary>
        /// Transmit a packet of data bytes
        /// </summary>
        /// <param name="data">The data to transmit</param>
        /// <param name="txCount">The number of transmissions</param>
        /// <param name="txInterval">The interval between transmissions</param>
        /// <param name="txTimeout">The tx timeout</param>
        void Transmit(IList<byte> data, int txCount, int txInterval, int txTimeout);

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
    }
}