﻿/*
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

// Ignore Spelling: Rfm Rx

using Microsoft.Extensions.Logging;
using RfmUsb.Net.Extensions;
using RfmUsb.Net.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RfmUsb.Net
{
    /// <summary>
    /// An implementation of the <see cref="IRfm69"/> interface
    /// </summary>
    public class Rfm69 : RfmBase, IRfm69
    {
        /// <summary>
        /// Create an instance of a <see cref="RfmUsb"/>
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{T}"/> for logging</param>
        /// <param name="serialPortFactory">The <see cref="ISerialPortFactory"/> instance for creating and querying serial port instances</param>
        public Rfm69(ILogger<Rfm69> logger, ISerialPortFactory serialPortFactory) : base(logger, serialPortFactory)
        {
        }

        ///<inheritdoc/>
        public bool AesOn
        {
            get => SendCommand(Commands.GetAesOn)[..1] == "1";
            set => SendCommandWithCheck($"{Commands.SetAesOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AfcLowBetaOn
        {
            get => SendCommand(Commands.GetAfcLowBetaOn).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetAfcLowBetaOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public EnterCondition AutoModeEnterCondition
        {
            get => (EnterCondition)SendCommand(Commands.GetAutoModeEnterCondition).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetAutoModeEnterCondition} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public ExitCondition AutoModeExitCondition
        {
            get => (ExitCondition)SendCommand(Commands.GetAutoModeExitCondition).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetAutoModeExitCondition} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool AutoRxRestartOn
        {
            get => SendCommand(Commands.GetAutoRxRestartOn).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetAutoRxRestartOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ContinuousDagc ContinuousDagc
        {
            get => (ContinuousDagc)SendCommand(Commands.GetContinuousDagc).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetContinuousDagc} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public LnaGain CurrentLnaGain => (LnaGain)SendCommand(Commands.GetCurrentLnaGain).ConvertToInt32();

        ///<inheritdoc/>
        public Rfm69DataMode DataMode
        {
            get => (Rfm69DataMode)SendCommand(Commands.GetDataMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDataMode} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public DccFreq DccFreq
        {
            get => (DccFreq)SendCommand(Commands.GetDccFreq).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDccFreq} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public DccFreq DccFreqAfc
        {
            get => (DccFreq)SendCommand(Commands.GetDccFreqAfc).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetDccFreqAfc} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool FifoFill
        {
            get => SendCommand(Commands.GetFifoFill).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetFifoFill} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool Impedance
        {
            get => SendCommand(Commands.GetImpedance).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetImpedance} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public IntermediateMode IntermediateMode
        {
            get => (IntermediateMode)SendCommand(Commands.GetIntermediateMode).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetIntermediateMode} 0x{(byte)value:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public Rfm69IrqFlags IrqFlags
        {
            get => GetIrqInternal();
            set => SendCommandWithCheck($"{Commands.SetIrqFlags} 0x{(ushort)value:X4}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte ListenCoefficientIdle
        {
            get => SendCommand(Commands.GetListenCoefficientIdle).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetListenCoefficientIdle} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte ListenCoefficientRx
        {
            get => SendCommand(Commands.GetListenCoefficientRx).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetListenCoefficientRx} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool ListenCriteria
        {
            get => SendCommand(Commands.GetListenCriteria).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetListenCriteria} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ListenEnd ListenEnd
        {
            get => (ListenEnd)SendCommand(Commands.GetListenEnd).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetListenEnd} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool ListenerOn
        {
            get => SendCommand(Commands.GetListenerOn).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetListenerOn} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public ListenResolution ListenResolutionIdle
        {
            get => (ListenResolution)SendCommand(Commands.GetListenResolutionIdle).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetListenResolutionIdle} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public ListenResolution ListenResolutionRx
        {
            get => (ListenResolution)SendCommand(Commands.GetListenResolutionRx).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetListenResolutionRx} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte LowBetaAfcOffset
        {
            get => SendCommand(Commands.GetLowBetaAfcOffset).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetLowBetaAfcOffset} 0x{value:x}", ResponseOk);
        }

        ///<inheritdoc/>
        public sbyte OutputPower
        {
            get => (sbyte)SendCommand(Commands.GetOutputPower).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetOutputPower} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public sbyte RssiThreshold
        {
            get => (sbyte)SendCommand(Commands.GetRssiThreshold).ConvertToInt32();
            set => SendCommandWithCheck($"{Commands.SetRssiThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool SensitivityBoost
        {
            get => SendCommand(Commands.GetSensitivityBoost).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetSensitivityBoost} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public bool Sequencer
        {
            get => SendCommand(Commands.GetSequencer).StartsWith('1');
            set => SendCommandWithCheck($"{Commands.SetSequencer} {(value ? "1" : "0")}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte SyncBitErrors
        {
            get => SendCommand(Commands.GetSyncBitErrors).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetSyncBitErrors} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TimeoutRssiThreshold
        {
            get => SendCommand(Commands.GetTimeoutRssiThreshold).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutRssiThreshold} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public byte TimeoutRxStart
        {
            get => SendCommand(Commands.GetTimeoutRxStart).ConvertToByte();
            set => SendCommandWithCheck($"{Commands.SetTimeoutRxStart} 0x{value:X}", ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteAfcClear()
        {
            SendCommandWithCheck(Commands.ExecuteAfcClear, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteAfcStart()
        {
            SendCommandWithCheck(Commands.ExecuteAfcStart, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteFeiStart()
        {
            SendCommandWithCheck(Commands.ExecuteFeiStart, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteListenModeAbort(Mode mode)
        {
            SendCommandWithCheck($"{Commands.ExecuteListenModeAbort} 0x{(byte)mode:X2}", ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteMeasureTemperature()
        {
            SendCommandWithCheck(Commands.ExecuteMeasureTemperature, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteRestartRx()
        {
            SendCommandWithCheck(Commands.ExecuteRestartRx, ResponseOk);
        }

        ///<inheritdoc/>
        public void ExecuteStartRssi()
        {
            SendCommandWithCheck(Commands.ExecuteStartRssi, ResponseOk);
        }

        ///<inheritdoc/>
        public void SetAesKey(IEnumerable<byte> key)
        {
            SendCommandWithCheck($"{Commands.ExecuteSetAesKey} {BitConverter.ToString(key.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        }

        private Rfm69IrqFlags GetIrqInternal()
        {
            Rfm69IrqFlags irq = Rfm69IrqFlags.None;

            var lines = SendCommandListResponse(Commands.GetIrqFlags);

            lines.ForEach(_ =>
            {
                var parts = _.Split(':');

                switch (parts[1])
                {
                    case "CRC_OK":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.CrcOK;
                        }
                        break;

                    case "PAYLOAD_READY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.PayloadReady;
                        }
                        break;

                    case "PACKET_SENT":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.PacketSent;
                        }
                        break;

                    case "FIFO_OVERRUN":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.FifoOverrun;
                        }
                        break;

                    case "FIFO_LEVEL":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.FifoLevel;
                        }
                        break;

                    case "FIFO_NOT_EMPTY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.FifoNotEmpty;
                        }
                        break;

                    case "FIFO_FULL":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.FifoFull;
                        }
                        break;

                    case "ADDRESS_MATCH":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.SyncAddressMatch;
                        }
                        break;

                    case "AUTO_MODE":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.AutoMode;
                        }
                        break;

                    case "TIMEOUT":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.Timeout;
                        }
                        break;

                    case "RSSI":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.Rssi;
                        }
                        break;

                    case "PLL_LOCK":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.PllLock;
                        }
                        break;

                    case "TX_RDY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.TxReady;
                        }
                        break;

                    case "RX_RDY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.RxReady;
                        }
                        break;

                    case "MODE_RDY":
                        if (parts[0] == "1")
                        {
                            irq |= Rfm69IrqFlags.ModeReady;
                        }
                        break;
                }
            });

            return irq;
        }
    }
}