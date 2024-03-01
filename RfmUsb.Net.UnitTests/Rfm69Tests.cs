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

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace RfmUsb.Net.UnitTests
{
    [TestClass]
    public class Rfm69Tests : RfmBaseTests
    {
        private readonly Rfm69 _rfmDevice;

        public Rfm69Tests()
        {
            _rfmDevice = new Rfm69(MockLogger, MockSerialPortFactory.Object);
            RfmBase = _rfmDevice;
        }

        [TestMethod]
        public void TestExecuteAfcClear()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteAfcClear(); },
                Commands.ExecuteAfcClear,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteAfcStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteAfcStart(); },
                Commands.ExecuteAfcStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteFeiStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteFeiStart(); },
                Commands.ExecuteFeiStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetAesOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AesOn; },
                (v) => v.Should().Be(value),
                Commands.GetAesOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetAfcLowBetaOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AfcLowBetaOn; },
                (v) => v.Should().Be(value),
                Commands.GetAfcLowBetaOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(EnterCondition.CrcOk)]
        [DataRow(EnterCondition.FifoLevel)]
        [DataRow(EnterCondition.FifoNotEmpty)]
        [DataRow(EnterCondition.Off)]
        [DataRow(EnterCondition.PacketSent)]
        [DataRow(EnterCondition.PayloadReady)]
        [DataRow(EnterCondition.SyncAddressMatch)]
        [DataRow(EnterCondition.FallingEdgeFifoNotEmpty)]
        public void TestGetAutoModeEnterCondition(EnterCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoModeEnterCondition; },
                (v) => { v.Should().Be(expected); },
                Commands.GetAutoModeEnterCondition,
                $"0x{expected:X}");
        }

        // ExitCondition
        [TestMethod]
        [DataRow(ExitCondition.CrcOk)]
        [DataRow(ExitCondition.FifoNotEmpty)]
        [DataRow(ExitCondition.FifoLevel)]
        [DataRow(ExitCondition.Off)]
        [DataRow(ExitCondition.PacketSent)]
        [DataRow(ExitCondition.PayloadReady)]
        [DataRow(ExitCondition.Timeout)]
        [DataRow(ExitCondition.SyncAddressMatch)]
        public void TestGetAutoModeExitCondition(ExitCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoModeExitCondition; },
                (v) => { v.Should().Be(expected); },
                Commands.GetAutoModeExitCondition,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetAutoRxRestartOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoRxRestartOn; },
                (v) => v.Should().Be(value),
                Commands.GetAutoRxRestartOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(ContinuousDagc.Normal)]
        [DataRow(ContinuousDagc.ImprovedLowBeta0)]
        [DataRow(ContinuousDagc.ImprovedLowBeta1)]
        public void TestGetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ContinuousDagc; },
                (v) => { v.Should().Be(expected); },
                Commands.GetContinuousDagc,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(LnaGain.Auto)]
        [DataRow(LnaGain.Max)]
        [DataRow(LnaGain.MaxMinus12db)]
        [DataRow(LnaGain.MaxMinus24db)]
        [DataRow(LnaGain.MaxMinus36db)]
        [DataRow(LnaGain.MaxMinus48db)]
        [DataRow(LnaGain.MaxMinus6db)]
        public void TestGetCurrentLnaGain(LnaGain expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.CurrentLnaGain; },
                (v) => v.Should().Be(expected),
                Commands.GetCurrentLnaGain,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(Rfm69DataMode.Reserved)]
        [DataRow(Rfm69DataMode.ContinousModeWithBitSync)]
        [DataRow(Rfm69DataMode.ContinousModeWithoutBitSync)]
        [DataRow(Rfm69DataMode.Packet)]
        public void TestGetDataMode(Rfm69DataMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.DataMode; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDataMode,
                expected.ToString("X"));
        }

        [TestMethod]
        [DataRow(DccFreq.FreqPercent0_125)]
        [DataRow(DccFreq.FreqPercent0_25)]
        [DataRow(DccFreq.FreqPercent0_5)]
        [DataRow(DccFreq.FreqPercent1)]
        [DataRow(DccFreq.FreqPercent16)]
        [DataRow(DccFreq.FreqPercent2)]
        [DataRow(DccFreq.FreqPercent4)]
        [DataRow(DccFreq.FreqPercent8)]
        public void TestGetDccFreq(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.DccFreq; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDccFreq,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(DccFreq.FreqPercent0_125)]
        [DataRow(DccFreq.FreqPercent0_25)]
        [DataRow(DccFreq.FreqPercent0_5)]
        [DataRow(DccFreq.FreqPercent1)]
        [DataRow(DccFreq.FreqPercent16)]
        [DataRow(DccFreq.FreqPercent2)]
        [DataRow(DccFreq.FreqPercent4)]
        [DataRow(DccFreq.FreqPercent8)]
        public void TestGetDccFreqAfc(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.DccFreqAfc; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDccFreqAfc,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetFifoFill(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoFill; },
                (v) => v.Should().Be(value),
                Commands.GetFifoFill,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetImpedance(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.Impedance; },
                (v) => v.Should().Be(value),
                Commands.GetImpedance,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(IntermediateMode.Rx)]
        [DataRow(IntermediateMode.Sleep)]
        [DataRow(IntermediateMode.Standby)]
        [DataRow(IntermediateMode.Tx)]
        public void TestGetIntermediateMode(IntermediateMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IntermediateMode; },
                (v) => v.Should().Be(expected),
                Commands.GetIntermediateMode,
                $"0x{expected:X}");
        }

        [TestMethod]
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

            result.Should().Be(
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
                Rfm69IrqFlags.ModeReady);
        }

        [TestMethod]
        public void TestGetListenCoefficentIdle()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenCoefficentIdle; },
                (v) => v.Should().Be(0xAA),
                Commands.GetListenCoefficentIdle,
                "0xAA");
        }

        [TestMethod]
        public void TestGetListenCoefficentRx()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenCoefficentRx; },
                (v) => v.Should().Be(0xAA),
                Commands.GetListenCoefficentRx,
                "0xAA");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetListenCriteria(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenCriteria; },
                (v) => v.Should().Be(value),
                Commands.GetListenCriteria,
                value ? "1" : "0");
        }

        // ListenEnd
        [TestMethod]
        [DataRow(ListenEnd.Idle)]
        [DataRow(ListenEnd.Mode)]
        [DataRow(ListenEnd.Reserved)]
        [DataRow(ListenEnd.Rx)]
        public void TestGetListenEnd(ListenEnd expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenEnd; },
                (v) => v.Should().Be(expected),
                Commands.GetListenEnd,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetListenerOn(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenerOn; },
                (v) => v.Should().Be(value),
                Commands.GetListenerOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(ListenResolution.Idle262ms)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Reserved)]
        public void TestGetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenResolutionIdle; },
                (v) => v.Should().Be(expected),
                Commands.GetListenResolutionIdle,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(ListenResolution.Idle262ms)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Reserved)]
        public void TestGetListenResolutionRx(ListenResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ListenResolutionRx; },
                (v) => v.Should().Be(expected),
                Commands.GetListenResolutionRx,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetLowBetaAfcOffset()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowBetaAfcOffset; },
                (v) => v.Should().Be(0xAA),
                Commands.GetLowBetaAfcOffset,
                "0xAA");
        }

        [TestMethod]
        public void TestGetOutputPower()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.OutputPower; },
                (v) => v.Should().Be(-2),
                Commands.GetOutputPower,
                $"0x{(sbyte)-2:X2}");
        }

        [TestMethod]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiThreshold; },
                (v) => v.Should().Be(-114),
                Commands.GetRssiThreshold,
                "0x8E");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetSensitivityBoost(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SensitivityBoost; },
                (v) => v.Should().Be(value),
                Commands.GetSensitivityBoost,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestGetSequencer(bool value)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.Sequencer; },
                (v) => v.Should().Be(value),
                Commands.GetSequencer,
                value ? "1" : "0");
        }

        [TestMethod]
        public void TestGetSyncBitErrors()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SyncBitErrors; },
                (v) => v.Should().Be(0xA0),
                Commands.GetSyncBitErrors,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTimeout()
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.ReadTimeout)
                .Returns(1000);

            // Act
            var result = _rfmDevice.Timeout;

            // Assert
            result.Should().Be(1000);
        }

        [TestMethod]
        public void TestGetTimeoutRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRssiThreshold; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTimeoutRssiThreshold,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTimeoutRxStart()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRxStart; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTimeoutRxStart,
                "0xA0");
        }

        [TestMethod]
        [DataRow(Mode.Rx)]
        [DataRow(Mode.Sleep)]
        [DataRow(Mode.Standby)]
        [DataRow(Mode.Synth)]
        [DataRow(Mode.Tx)]
        public void TestListenModeAbort(Mode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ExecuteListenModeAbort(expected); },
                Commands.ExecuteListenModeAbort,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestMeasureTemperature()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteMeasureTemperature(); },
                Commands.ExecuteMeasureTemperature,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestOpen()
        {
            TestOpen("RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz");
        }

        [TestMethod]
        public void TestRestartRx()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteRestartRx(); },
                Commands.ExecuteRestartRx,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestSetAesKey()
        {
            ExecuteSetTest(
                () => { _rfmDevice.SetAesKey(new List<byte>() { 0xFF, 0xAA, 0xBB }); },
                Commands.ExecuteSetAesKey,
                "FFAABB");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetAesOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AesOn = value; },
                Commands.SetAesOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetAfcLowBetaOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AfcLowBetaOn = value; },
                Commands.SetAfcLowBetaOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(ExitCondition.CrcOk)]
        [DataRow(ExitCondition.FifoNotEmpty)]
        [DataRow(ExitCondition.FifoLevel)]
        [DataRow(ExitCondition.Off)]
        [DataRow(ExitCondition.PacketSent)]
        [DataRow(ExitCondition.PayloadReady)]
        [DataRow(ExitCondition.Timeout)]
        [DataRow(ExitCondition.SyncAddressMatch)]
        public void TestSetAutoModeExitCondition(ExitCondition expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoModeExitCondition = expected; },
                Commands.SetAutoModeExitCondition,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetAutoRxRestartOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoRxRestartOn = value; },
                Commands.SetAutoRxRestartOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(ContinuousDagc.Normal)]
        [DataRow(ContinuousDagc.ImprovedLowBeta0)]
        [DataRow(ContinuousDagc.ImprovedLowBeta1)]
        public void TestSetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ContinuousDagc = expected; },
                Commands.SetContinuousDagc,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(Rfm69DataMode.Packet)]
        [DataRow(Rfm69DataMode.Reserved)]
        [DataRow(Rfm69DataMode.ContinousModeWithBitSync)]
        [DataRow(Rfm69DataMode.ContinousModeWithoutBitSync)]
        public void TestSetDataMode(Rfm69DataMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.DataMode = expected; },
                Commands.SetDataMode,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(DccFreq.FreqPercent0_125)]
        [DataRow(DccFreq.FreqPercent0_25)]
        [DataRow(DccFreq.FreqPercent0_5)]
        [DataRow(DccFreq.FreqPercent1)]
        [DataRow(DccFreq.FreqPercent16)]
        [DataRow(DccFreq.FreqPercent2)]
        [DataRow(DccFreq.FreqPercent4)]
        [DataRow(DccFreq.FreqPercent8)]
        public void TestSetDccFreq(DccFreq expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.DccFreq = expected; },
                Commands.SetDccFreq,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(DccFreq.FreqPercent0_125)]
        [DataRow(DccFreq.FreqPercent0_25)]
        [DataRow(DccFreq.FreqPercent0_5)]
        [DataRow(DccFreq.FreqPercent1)]
        [DataRow(DccFreq.FreqPercent16)]
        [DataRow(DccFreq.FreqPercent2)]
        [DataRow(DccFreq.FreqPercent4)]
        [DataRow(DccFreq.FreqPercent8)]
        public void TestSetDccFreqAfc(DccFreq expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.DccFreqAfc = expected; },
                Commands.SetDccFreqAfc,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(EnterCondition.CrcOk)]
        [DataRow(EnterCondition.FifoLevel)]
        [DataRow(EnterCondition.FifoNotEmpty)]
        [DataRow(EnterCondition.Off)]
        [DataRow(EnterCondition.PacketSent)]
        [DataRow(EnterCondition.PayloadReady)]
        [DataRow(EnterCondition.SyncAddressMatch)]
        [DataRow(EnterCondition.FallingEdgeFifoNotEmpty)]
        public void TestSetEnterCondition(EnterCondition expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoModeEnterCondition = expected; },
                Commands.SetAutoModeEnterCondition,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetFifoFill(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoFill = value; },
                Commands.SetFifoFill,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetImpedance(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.Impedance = value; },
                Commands.SetImpedance,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(IntermediateMode.Rx)]
        [DataRow(IntermediateMode.Sleep)]
        [DataRow(IntermediateMode.Standby)]
        [DataRow(IntermediateMode.Tx)]
        public void TestSetIntermediateMode(IntermediateMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.IntermediateMode = expected; },
                Commands.SetIntermediateMode,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetIrqFlags()
        {
            ExecuteSetTest(
                () => { _rfmDevice.IrqFlags = Rfm69IrqFlags.FifoOverrun | Rfm69IrqFlags.SyncAddressMatch | Rfm69IrqFlags.Rssi; },
                Commands.SetIrqFlags,
                "0x0910");
        }

        [TestMethod]
        public void TestSetListenCoefficentIdle()
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenCoefficentIdle = 0x60; },
                Commands.SetListenCoefficentIdle,
                "0x60");
        }

        [TestMethod]
        public void TestSetListenCoefficentRx()
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenCoefficentRx = 0x60; },
                Commands.SetListenCoefficentRx,
                "0x60");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetListenCriteria(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenCriteria = value; },
                Commands.SetListenCriteria,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(ListenEnd.Idle)]
        [DataRow(ListenEnd.Mode)]
        [DataRow(ListenEnd.Reserved)]
        [DataRow(ListenEnd.Rx)]
        public void TestSetListenEnd(ListenEnd expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenEnd = expected; },
                Commands.SetListenEnd,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetListenerOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenerOn = value; },
                Commands.SetListenerOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(ListenResolution.Idle262ms)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Reserved)]
        public void TestSetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenResolutionIdle = expected; },
                Commands.SetListenResolutionIdle,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(ListenResolution.Idle262ms)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Reserved)]
        public void TestSetListenResolutionRx(ListenResolution expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ListenResolutionRx = expected; },
                Commands.SetListenResolutionRx,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetLowBetaAfcOffset()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowBetaAfcOffset = 0x60; },
                Commands.SetLowBetaAfcOffset,
                "0x60");
        }

        [TestMethod]
        public void TestSetOutputPower()
        {
            ExecuteSetTest(
                () => { _rfmDevice.OutputPower = -2; },
                Commands.SetOutputPower,
                 $"0x{(sbyte)-2:X2}");
        }

        [TestMethod]
        public void TestSetRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiThreshold = -114; },
                Commands.SetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetSensitivityBoost(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.SensitivityBoost = value; },
                Commands.SetSensitivityBoost,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetSequencer(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.Sequencer = value; },
                Commands.SetSequencer,
                value ? "1" : "0");
        }

        [TestMethod]
        public void TestSetSyncBitErrors()
        {
            ExecuteSetTest(
                () => { _rfmDevice.SyncBitErrors = 0xB0; },
                Commands.SetSyncBitErrors,
                "0xB0");
        }

        [TestMethod]
        public void TestSetTimeout()
        {
            // Arrange

            // Act
            _rfmDevice.Timeout = 1000;

            // Assert
            MockSerialPort.VerifySet(_ => _.ReadTimeout = 1000, Times.Once);
        }

        [TestMethod]
        public void TestSetTimeoutRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRssiThreshold = 0xB0; },
                Commands.SetTimeoutRssiThreshold,
                "0xB0");
        }

        [TestMethod]
        public void TestSetTimeoutRxStart()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRxStart = 0xB0; },
                Commands.SetTimeoutRxStart,
                "0xB0");
        }

        [TestMethod]
        public void TestStartRssi()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteStartRssi(); },
                Commands.ExecuteStartRssi,
                RfmBase.ResponseOk);
        }
    }
}