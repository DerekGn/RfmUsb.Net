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

// Ignore Spelling: Aes Usb Rssi Lna Irq Fei Dcc Dagc Rfm Rx

using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace RfmUsb.Net.UnitTests
{
    public class Rfm69Tests : RfmBaseTests
    {
        private readonly Rfm69 _rfmDevice;

        public Rfm69Tests() : base()
        {
            _rfmDevice = new Rfm69(
                Mock.Of<ILogger<Rfm69>>(),
                MockSerialPortFactory.Object);

            RfmBase = _rfmDevice;

            InitialiseRfmDevice();
        }

        [Fact]
        public void TestExecuteAfcClear()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteAfcClear(); },
                Commands.ExecuteAfcClear,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestExecuteAfcStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteAfcStart(); },
                Commands.ExecuteAfcStart,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestExecuteFeiStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteFeiStart(); },
                Commands.ExecuteFeiStart,
                RfmBase.ResponseOk);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetAesOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AesOn; },
                (v) => Assert.Equal(value, v),
                Commands.GetAesOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetAfcLowBetaOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AfcLowBetaOn; },
                (v) => Assert.Equal(value, v),
                Commands.GetAfcLowBetaOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(EnterCondition.CrcOk)]
        [InlineData(EnterCondition.FifoLevel)]
        [InlineData(EnterCondition.FifoNotEmpty)]
        [InlineData(EnterCondition.Off)]
        [InlineData(EnterCondition.PacketSent)]
        [InlineData(EnterCondition.PayloadReady)]
        [InlineData(EnterCondition.SyncAddressMatch)]
        [InlineData(EnterCondition.FallingEdgeFifoNotEmpty)]
        public void TestGetAutoModeEnterCondition(EnterCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoModeEnterCondition; },
                (v) => Assert.Equal(expected, v),
                Commands.GetAutoModeEnterCondition,
                $"0x{expected:X}");
        }

        // ExitCondition
        [Theory]
        [InlineData(ExitCondition.CrcOk)]
        [InlineData(ExitCondition.FifoNotEmpty)]
        [InlineData(ExitCondition.FifoLevel)]
        [InlineData(ExitCondition.Off)]
        [InlineData(ExitCondition.PacketSent)]
        [InlineData(ExitCondition.PayloadReady)]
        [InlineData(ExitCondition.Timeout)]
        [InlineData(ExitCondition.SyncAddressMatch)]
        public void TestGetAutoModeExitCondition(ExitCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoModeExitCondition; },
                (v) => Assert.Equal(expected, v),
                Commands.GetAutoModeExitCondition,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetAutoRxRestartOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoRxRestartOn; },
                (v) => Assert.Equal(value, v),
                Commands.GetAutoRxRestartOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestGetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ContinuousDagc; },
                (v) => Assert.Equal(expected, v),
                Commands.GetContinuousDagc,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(LnaGain.Auto)]
        [InlineData(LnaGain.Max)]
        [InlineData(LnaGain.MaxMinus12db)]
        [InlineData(LnaGain.MaxMinus24db)]
        [InlineData(LnaGain.MaxMinus36db)]
        [InlineData(LnaGain.MaxMinus48db)]
        [InlineData(LnaGain.MaxMinus6db)]
        public void TestGetCurrentLnaGain(LnaGain expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.CurrentLnaGain; },
                (v) => Assert.Equal(expected, v),
                Commands.GetCurrentLnaGain,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(Rfm69DataMode.Reserved)]
        [InlineData(Rfm69DataMode.ContinousModeWithBitSync)]
        [InlineData(Rfm69DataMode.ContinousModeWithoutBitSync)]
        [InlineData(Rfm69DataMode.Packet)]
        public void TestGetDataMode(Rfm69DataMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.DataMode; },
                (v) => Assert.Equal(expected, v),
                Commands.GetDataMode,
                expected.ToString("X"));
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent16)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        public void TestGetDccFreq(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.DccFreq; },
                (v) => Assert.Equal(expected, v),
                Commands.GetDccFreq,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent16)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        public void TestGetDccFreqAfc(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.DccFreqAfc; },
                (v) => Assert.Equal(expected, v),
                Commands.GetDccFreqAfc,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetFifoFill(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoFill; },
                (v) => Assert.Equal(value, v),
                Commands.GetFifoFill,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetImpedance(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.Impedance; },
                (v) => Assert.Equal(value, v),
                Commands.GetImpedance,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestGetIntermediateMode(IntermediateMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IntermediateMode; },
                (v) => Assert.Equal(expected, v),
                Commands.GetIntermediateMode,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetIrqFlags()
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
               .Setup(_ => _.Write(It.IsAny<string>()))
               .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
                .SetupSequence(_ => _.ReadLine())
                .Returns("1:CRC_OK")
                .Returns("1:PAYLOAD_READY")
                .Returns("1:FIFO_OVERRUN")
                .Returns("1:FIFO_LEVEL")
                .Returns("1:FIFO_NOT_EMPTY")
                .Returns("1:FIFO_FULL")
                .Returns("1:ADDRESS_MATCH")
                .Returns("1:AUTO_MODE")
                .Returns("1:TIMEOUT")
                .Returns("1:RSSI")
                .Returns("1:PLL_LOCK")
                .Returns("1:TX_RDY")
                .Returns("1:MODE_RDY")
                .Returns("1:RX_RDY");

            MockSerialPort
                .SetupSequence(_ => _.BytesToRead)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = _rfmDevice.IrqFlags;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetIrqFlags}\n"));

            Assert.Equal(
                Rfm69IrqFlags.CrcOK |
                Rfm69IrqFlags.PayloadReady |
                Rfm69IrqFlags.FifoOverrun |
                Rfm69IrqFlags.FifoLevel |
                Rfm69IrqFlags.FifoNotEmpty |
                Rfm69IrqFlags.FifoFull |
                Rfm69IrqFlags.SyncAddressMatch |
                Rfm69IrqFlags.AutoMode |
                Rfm69IrqFlags.Timeout |
                Rfm69IrqFlags.Rssi |
                Rfm69IrqFlags.PllLock |
                Rfm69IrqFlags.RxReady |
                Rfm69IrqFlags.TxReady |
                Rfm69IrqFlags.ModeReady, result);
        }

        [Fact]
        public void TestGetListenCoefficientIdle()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenCoefficientIdle; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetListenCoefficientIdle,
                "0xAA");
        }

        [Fact]
        public void TestGetListenCoefficientRx()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenCoefficientRx; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetListenCoefficientRx,
                "0xAA");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetListenCriteria(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenCriteria; },
                (v) => Assert.Equal(value, v),
                Commands.GetListenCriteria,
                value ? "1" : "0");
        }

        // ListenEnd
        [Theory]
        [InlineData(ListenEnd.Idle)]
        [InlineData(ListenEnd.Mode)]
        [InlineData(ListenEnd.Reserved)]
        [InlineData(ListenEnd.Rx)]
        public void TestGetListenEnd(ListenEnd expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenEnd; },
                (v) => Assert.Equal(expected, v),
                Commands.GetListenEnd,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetListenerOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenerOn; },
                (v) => Assert.Equal(value, v),
                Commands.GetListenerOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Reserved)]
        public void TestGetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenResolutionIdle; },
                (v) => Assert.Equal(expected, v),
                Commands.GetListenResolutionIdle,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Reserved)]
        public void TestGetListenResolutionRx(ListenResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenResolutionRx; },
                (v) => Assert.Equal(expected, v),
                Commands.GetListenResolutionRx,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetLowBetaAfcOffset()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowBetaAfcOffset; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetLowBetaAfcOffset,
                "0xAA");
        }

        [Fact]
        public void TestGetOutputPower()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.OutputPower; },
                (v) => Assert.Equal(-2, v),
                Commands.GetOutputPower,
                $"0x{(sbyte)-2:X2}");
        }

        [Fact]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiThreshold; },
                (v) => Assert.Equal(-114, v),
                Commands.GetRssiThreshold,
                "0x8E");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetSensitivityBoost(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SensitivityBoost; },
                (v) => Assert.Equal(value, v),
                Commands.GetSensitivityBoost,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGetSequencer(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.Sequencer; },
                (v) => Assert.Equal(value, v),
                Commands.GetSequencer,
                value ? "1" : "0");
        }

        [Fact]
        public void TestGetSyncBitErrors()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SyncBitErrors; },
                (v) => Assert.Equal(0xA0, v),
                Commands.GetSyncBitErrors,
                "0xA0");
        }

        [Fact]
        public void TestGetTimeout()
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.ReadTimeout)
                .Returns(1000);

            // Act
            var result = _rfmDevice.Timeout;

            // Assert
            Assert.Equal(1000, result);
        }

        [Fact]
        public void TestGetTimeoutRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRssiThreshold; },
                (v) => Assert.Equal(0xA0, v),
                Commands.GetTimeoutRssiThreshold,
                "0xA0");
        }

        [Fact]
        public void TestGetTimeoutRxStart()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRxStart; },
                (v) => Assert.Equal(0xA0, v),
                Commands.GetTimeoutRxStart,
                "0xA0");
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestListenModeAbort(Mode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ExecuteListenModeAbort(expected); },
                Commands.ExecuteListenModeAbort,
                $"0x{(byte)expected:X2}");
        }

        [Fact]
        public void TestMeasureTemperature()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteMeasureTemperature(); },
                Commands.ExecuteMeasureTemperature,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestOpen()
        {
            TestOpenVersion("RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz");
        }

        [Fact]
        public void TestRestartRx()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteRestartRx(); },
                Commands.ExecuteRestartRx,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestSetAesKey()
        {
            ExecuteSetTest(
                () => { _rfmDevice.SetAesKey(new List<byte>() { 0xFF, 0xAA, 0xBB }); },
                Commands.ExecuteSetAesKey,
                "FFAABB");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAesOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AesOn = value; },
                Commands.SetAesOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAfcLowBetaOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AfcLowBetaOn = value; },
                Commands.SetAfcLowBetaOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ExitCondition.CrcOk)]
        [InlineData(ExitCondition.FifoNotEmpty)]
        [InlineData(ExitCondition.FifoLevel)]
        [InlineData(ExitCondition.Off)]
        [InlineData(ExitCondition.PacketSent)]
        [InlineData(ExitCondition.PayloadReady)]
        [InlineData(ExitCondition.Timeout)]
        [InlineData(ExitCondition.SyncAddressMatch)]
        public void TestSetAutoModeExitCondition(ExitCondition expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoModeExitCondition = expected; },
                Commands.SetAutoModeExitCondition,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAutoRxRestartOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoRxRestartOn = value; },
                Commands.SetAutoRxRestartOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestSetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ContinuousDagc = expected; },
                Commands.SetContinuousDagc,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(Rfm69DataMode.Packet)]
        [InlineData(Rfm69DataMode.Reserved)]
        [InlineData(Rfm69DataMode.ContinousModeWithBitSync)]
        [InlineData(Rfm69DataMode.ContinousModeWithoutBitSync)]
        public void TestSetDataMode(Rfm69DataMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.DataMode = expected; },
                Commands.SetDataMode,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent16)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        public void TestSetDccFreq(DccFreq expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.DccFreq = expected; },
                Commands.SetDccFreq,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent16)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        public void TestSetDccFreqAfc(DccFreq expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.DccFreqAfc = expected; },
                Commands.SetDccFreqAfc,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(EnterCondition.CrcOk)]
        [InlineData(EnterCondition.FifoLevel)]
        [InlineData(EnterCondition.FifoNotEmpty)]
        [InlineData(EnterCondition.Off)]
        [InlineData(EnterCondition.PacketSent)]
        [InlineData(EnterCondition.PayloadReady)]
        [InlineData(EnterCondition.SyncAddressMatch)]
        [InlineData(EnterCondition.FallingEdgeFifoNotEmpty)]
        public void TestSetEnterCondition(EnterCondition expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoModeEnterCondition = expected; },
                Commands.SetAutoModeEnterCondition,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetFifoFill(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoFill = value; },
                Commands.SetFifoFill,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetImpedance(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.Impedance = value; },
                Commands.SetImpedance,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestSetIntermediateMode(IntermediateMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.IntermediateMode = expected; },
                Commands.SetIntermediateMode,
                $"0x{(byte)expected:X2}");
        }

        [Fact]
        public void TestSetIrqFlags()
        {
            ExecuteSetTest(
                () => { _rfmDevice.IrqFlags = Rfm69IrqFlags.FifoOverrun | Rfm69IrqFlags.SyncAddressMatch | Rfm69IrqFlags.Rssi; },
                Commands.SetIrqFlags,
                "0x0910");
        }

        [Fact]
        public void TestSetListenCoefficientIdle()
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenCoefficientIdle = 0x60; },
                Commands.SetListenCoefficientIdle,
                "0x60");
        }

        [Fact]
        public void TestSetListenCoefficientRx()
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenCoefficientRx = 0x60; },
                Commands.SetListenCoefficientRx,
                "0x60");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetListenCriteria(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenCriteria = value; },
                Commands.SetListenCriteria,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ListenEnd.Idle)]
        [InlineData(ListenEnd.Mode)]
        [InlineData(ListenEnd.Reserved)]
        [InlineData(ListenEnd.Rx)]
        public void TestSetListenEnd(ListenEnd expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenEnd = expected; },
                Commands.SetListenEnd,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetListenerOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenerOn = value; },
                Commands.SetListenerOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Reserved)]
        public void TestSetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenResolutionIdle = expected; },
                Commands.SetListenResolutionIdle,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Reserved)]
        public void TestSetListenResolutionRx(ListenResolution expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenResolutionRx = expected; },
                Commands.SetListenResolutionRx,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestSetLowBetaAfcOffset()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowBetaAfcOffset = 0x60; },
                Commands.SetLowBetaAfcOffset,
                "0x60");
        }

        [Fact]
        public void TestSetOutputPower()
        {
            ExecuteSetTest(
                () => { _rfmDevice.OutputPower = -2; },
                Commands.SetOutputPower,
                 $"0x{(sbyte)-2:X2}");
        }

        [Fact]
        public void TestSetRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiThreshold = -114; },
                Commands.SetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetSensitivityBoost(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.SensitivityBoost = value; },
                Commands.SetSensitivityBoost,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetSequencer(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.Sequencer = value; },
                Commands.SetSequencer,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetSyncBitErrors()
        {
            ExecuteSetTest(
                () => { _rfmDevice.SyncBitErrors = 0xB0; },
                Commands.SetSyncBitErrors,
                "0xB0");
        }

        [Fact]
        public void TestSetTimeout()
        {
            // Arrange

            // Act
            _rfmDevice.Timeout = 1000;

            // Assert
            MockSerialPort.VerifySet(_ => _.ReadTimeout = 1000, Times.Once);
        }

        [Fact]
        public void TestSetTimeoutRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRssiThreshold = 0xB0; },
                Commands.SetTimeoutRssiThreshold,
                "0xB0");
        }

        [Fact]
        public void TestSetTimeoutRxStart()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRxStart = 0xB0; },
                Commands.SetTimeoutRxStart,
                "0xB0");
        }

        [Fact]
        public void TestStartRssi()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteStartRssi(); },
                Commands.ExecuteStartRssi,
                RfmBase.ResponseOk);
        }
    }
}