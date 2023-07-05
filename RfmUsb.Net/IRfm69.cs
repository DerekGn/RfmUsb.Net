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

using System.Collections.Generic;

namespace RfmUsb.Net
{
    /// <summary>
    /// An rfm6x device
    /// </summary>
    public interface IRfm69 : IRfm
    {
        /// <summary>
        /// Enable the AES encryption/decryption:
        /// false : Off
        /// true : On (payload limited to 66 bytes maximum)
        /// </summary>
        bool AesOn { get; set; }

        /// <summary>
        /// Improved AFC routine for signals with modulation index lower than 2.
        /// </summary>
        bool AfcLowBetaOn { get; set; }

        /// <summary>
        /// Enables automatic Rx restart (RSSI phase) after PayloadReady occurred and packet has been completely read from FIFO:
        /// false : Off. RestartRx can be used.
        /// true: On. Rx automatically restarted after InterPacketRxDelay.
        /// </summary>
        bool AutoRxRestartOn { get; set; }

        /// <summary>
        /// Fading Margin Improvement
        /// </summary>
        ContinuousDagc ContinuousDagc { get; set; }

        /// <summary>
        /// Current LNA gain, set either manually, or by the AGC
        /// </summary>
        LnaGain CurrentLnaGain { get; }

        /// <summary>
        /// Cut-off frequency of the DC offset canceller (DCC)
        /// </summary>
        DccFreq DccFreq { get; set; }

        /// <summary>
        /// Cut-off frequency of the DC offset canceller (DCC)
        /// </summary>
        DccFreq DccFreqAfc { get; set; }

        /// <summary>
        /// The data processing mode
        /// </summary>
        Rfm69DataMode DataMode { get; set; }

        /// <summary>
        /// Interrupt condition for entering the intermediate mode
        /// </summary>
        EnterCondition EnterCondition { get; set; }

        /// <summary>
        /// Interrupt condition for exiting the intermediate mode
        /// </summary>
        ExitCondition ExitCondition { get; set; }

        /// <summary>
        /// FIFO filling condition:
        /// false : if SyncAddress interrupt occurs
        /// true : as long as FifoFillCondition is set
        /// </summary>
        bool FifoFill { get; set; }

        /// <summary>
        /// LNA’s input impedance
        /// false : 50 ohms
        /// true : 200 ohms
        /// </summary>
        bool Impedance { get; set; }

        /// <summary>
        /// Intermediate mode
        /// </summary>
        IntermediateMode IntermediateMode { get; set; }

        /// <summary>
        /// Get or set the Irq flags
        /// </summary>
        /// <remarks>
        /// Setting a specific flag clears the corresponding irq
        /// </remarks>
        Rfm69IrqFlags IrqFlags { get; set; }

        /// <summary>
        /// Duration of the Idle phase in Listen mode
        /// </summary>
        byte ListenCoefficentIdle { get; set; }

        /// <summary>
        /// Duration of the Idle phase in Rx phase
        /// </summary>
        byte ListenCoefficentRx { get; set; }

        /// <summary>
        /// Criteria for packet acceptance in Listen mode:
        /// false : signal strength is above RssiThreshold
        /// true : signal strength is above RssiThreshold and SyncAddress matched
        /// </summary>
        bool ListenCriteria { get; set; }

        /// <summary>
        /// Action taken after acceptance of a packet in Listen mode
        /// </summary>
        ListenEnd ListenEnd { get; set; }

        /// <summary>
        /// Enables Listen mode, should be enabled whilst in Standby mode
        /// </summary>
        bool ListenerOn { get; set; }

        /// <summary>
        /// The resolution of Listen mode Idle time
        /// </summary>
        ListenResolution ListenResolutionIdle { get; set; }

        /// <summary>
        /// The resolution of Listen mode Rx time
        /// </summary>
        ListenResolution ListenResolutionRx { get; set; }

        /// <summary>
        /// AFC offset set for low modulation index systems, used if AfcLowBetaOn = true.
        /// </summary>
        byte LowBetaAfcOffset { get; set; }

        /// <summary>
        /// Get or set the output power in dbm
        /// </summary>
        sbyte OutputPower { get; set; }

        /// <summary>
        /// RSSI trigger level for Rssi interrupt
        /// </summary>
        sbyte RssiThreshold { get; set; }

        /// <summary>
        /// High sensitivity or normal sensitivity mode
        /// </summary>
        bool SensitivityBoost { get; set; }

        /// <summary>
        /// Get or set the automatic Sequencer enable
        /// </summary>
        bool Sequencer { get; set; }

        /// <summary>
        /// Get or set the number of tolerated bit errors in Sync word
        /// </summary>
        byte SyncBitErrors { get; set; }

        /// <summary>
        /// Get or set the serial port timeout
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutRxStart*16*Tbit after switching to Rx mode
        /// if Rssi interrupt doesn’t occur(i.e. RssiValue > RssiThreshold)
        /// 0x00: TimeoutRxStart is disabled
        /// </summary>
        byte TimeoutRssiThreshold { get; set; }

        /// <summary>
        /// Timeout interrupt is generated TimeoutRxStart*16*Tbit after switching to Rx mode
        /// if Rssi interrupt doesn’t occur(i.e. RssiValue > RssiThreshold)
        /// 0x00: TimeoutRxStart is disabled
        /// </summary>
        byte TimeoutRxStart { get; set; }

        /// <summary>
        /// Clears the AfcValue if set in Rx mode
        /// </summary>
        void ExecuteAfcClear();

        /// <summary>
        /// Triggers an AFC
        /// </summary>
        void ExecuteAfcStart();

        /// <summary>
        /// Triggers a FEI measurement
        /// </summary>
        void ExecuteFeiStart();

        /// <summary>
        /// Abort listen mode
        /// </summary>
        void ExecuteListenAbort();

        /// <summary>
        /// Execute a temperature measurement
        /// </summary>
        void ExecuteMeasureTemperature();

        /// <summary>
        /// Forces the Receiver in WAIT mode, in Continuous Rx mode.
        /// </summary>
        void ExecuteRestartRx();

        /// <summary>
        /// Trigger a RSSI measurement
        /// </summary>
        void ExecuteStartRssi();

        /// <summary>
        /// Set the AES encryption key
        /// </summary>
        /// <param name="key"></param>
        void SetAesKey(IEnumerable<byte> key);

        /// <summary>
        /// Wait for a configured Irq to be signaled
        /// </summary>
        void WaitForIrq();
    }
}