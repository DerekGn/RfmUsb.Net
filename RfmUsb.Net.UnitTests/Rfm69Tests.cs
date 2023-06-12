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
using RfmUsb.Net.Exceptions;
using System;
using System.Collections.Generic;

namespace RfmUsb.Net.UnitTests
{
    [TestClass]
    public class Rfm69Tests : RfmBaseTests
    {
        private readonly Rfm69 _rfm6x;

        public Rfm69Tests()
        {
            _rfm6x = new Rfm69(MockLogger, MockSerialPortFactory.Object);
            RfmBase = _rfm6x;
        }

        [TestMethod]
        public void TestExecuteAfcClear()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteAfcClear(); },
                Commands.ExecuteAfcClear,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteAfcStart()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteAfcStart(); },
                Commands.ExecuteAfcStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteFeiStart()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteFeiStart(); },
                Commands.ExecuteFeiStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestGetAesOn()
        {
            ExecuteGetTest(
                () => { return _rfm6x.AesOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAesOn,
                "1");
        }

        [TestMethod]
        public void TestGetAfcLowBetaOn()
        {
            ExecuteGetTest(
                () => { return _rfm6x.AfcLowBetaOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcLowBetaOn,
                "1");
        }

        [TestMethod]
        [DataRow(ContinuousDagc.Normal)]
        [DataRow(ContinuousDagc.ImprovedLowBeta0)]
        [DataRow(ContinuousDagc.ImprovedLowBeta1)]
        public void TestGetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.ContinuousDagc; },
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
                () => { return _rfm6x.CurrentLnaGain; },
                (v) => v.Should().Be(expected),
                Commands.GetCurrentLnaGain,
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
        public void TestGetDccFreq(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.DccFreq; },
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
                () => { return _rfm6x.DccFreqAfc; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDccFreqAfc,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetDioInterruptMask()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
               .SetupSequence(_ => _.ReadLine())
               .Returns("1-DIO0")
               .Returns("0-DIO1")
               .Returns("1-DIO2")
               .Returns("0-DIO3")
               .Returns("1-DIO4")
               .Returns("0-DIO5");

            // Act
            var result = _rfm6x.DioInterruptMask;

            // Assert
            result.Should().Be(DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4);

            MockSerialPort.Verify(_ => _.Write($"{Commands.GetDioInterrupt}\n"), Times.Once);
        }

        [TestMethod]
        [DataRow(EnterCondition.CrcOk)]
        [DataRow(EnterCondition.FifoEmpty)]
        [DataRow(EnterCondition.FifoLevel)]
        [DataRow(EnterCondition.FifoNotEmpty)]
        [DataRow(EnterCondition.Off)]
        [DataRow(EnterCondition.PacketSent)]
        [DataRow(EnterCondition.PayloadReady)]
        [DataRow(EnterCondition.SyncAddressMatch)]
        public void TestGetEnterCondition(EnterCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.EnterCondition; },
                (v) => { v.Should().Be(expected); },
                Commands.GetEnterCondition,
                $"0x{expected:X}");
        }

        // ExitCondition
        [TestMethod]
        [DataRow(ExitCondition.CrcOk)]
        [DataRow(ExitCondition.FifoEmpty)]
        [DataRow(ExitCondition.FifoLevel)]
        [DataRow(ExitCondition.Off)]
        [DataRow(ExitCondition.PacketSent)]
        [DataRow(ExitCondition.PayloadReady)]
        [DataRow(ExitCondition.RxTimeout)]
        [DataRow(ExitCondition.SyncAddressMatch)]
        public void TestGetExitCondition(ExitCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.ExitCondition; },
                (v) => { v.Should().Be(expected); },
                Commands.GetExitCondition,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetFifoFill()
        {
            ExecuteGetTest(
                () => { return _rfm6x.FifoFill; },
                (v) => v.Should().BeTrue(),
                Commands.GetFifoFill,
                "1");
        }

        [TestMethod]
        public void TestGetImpedance()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Impedance; },
                (v) => v.Should().BeTrue(),
                Commands.GetImpedance,
                "1");
        }

        [TestMethod]
        [DataRow(IntermediateMode.Rx)]
        [DataRow(IntermediateMode.Sleep)]
        [DataRow(IntermediateMode.Standby)]
        [DataRow(IntermediateMode.Tx)]
        public void TestGetIntermediateMode(IntermediateMode expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.IntermediateMode; },
                (v) => v.Should().Be(expected),
                Commands.GetIntermediateMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetListenCoefficentIdle()
        {
            ExecuteGetTest(
                () => { return _rfm6x.ListenCoefficentIdle; },
                (v) => v.Should().Be(0xAA),
                Commands.GetListenCoefficentIdle,
                "0xAA");
        }

        [TestMethod]
        public void TestGetListenCoefficentRx()
        {
            ExecuteGetTest(
                () => { return _rfm6x.ListenCoefficentRx; },
                (v) => v.Should().Be(0xAA),
                Commands.GetListenCoefficentRx,
                "0xAA");
        }

        [TestMethod]
        public void TestGetListenCriteria()
        {
            ExecuteGetTest(
                () => { return _rfm6x.ListenCriteria; },
                (v) => v.Should().BeTrue(),
                Commands.GetListenCriteria,
                "1");
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
                () => { return _rfm6x.ListenEnd; },
                (v) => v.Should().Be(expected),
                Commands.GetListenEnd,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetListenerOn()
        {
            ExecuteGetTest(
                () => { return _rfm6x.ListenerOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetListenerOn,
                "1");
        }

        [TestMethod]
        [DataRow(ListenResolution.Idle262ms)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Reserved)]
        public void TestGetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.ListenResolutionIdle; },
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
                () => { return _rfm6x.ListenResolutionRx; },
                (v) => v.Should().Be(expected),
                Commands.GetListenResolutionRx,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetLowBetaAfcOffset()
        {
            ExecuteGetTest(
                () => { return _rfm6x.LowBetaAfcOffset; },
                (v) => v.Should().Be(0xAA),
                Commands.GetLowBetaAfcOffset,
                "0xAA");
        }

        [TestMethod]
        public void TestGetOutputPower()
        {
            ExecuteGetTest(
                () => { return _rfm6x.OutputPower; },
                (v) => v.Should().Be(-2),
                Commands.GetOutputPower,
                $"0x{(sbyte)-2:X2}");
        }

        [TestMethod]
        public void TestGetRadioConfig()
        {
            ExecuteGetTest(
                () => { return _rfm6x.RadioConfig; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRadioConfig,
                "0xA0");
        }

        [TestMethod]
        public void TestGetRadioConfigurations()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .SetupSequence(_ => _.ReadLine())
                .Returns("A")
                .Returns("B")
                .Returns("C")
                .Throws(new TimeoutException());

            // Act
            var result = _rfm6x.GetRadioConfigurations();

            // Assert
            result.Should().NotBeEmpty();
        }

        [TestMethod]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm6x.RssiThreshold; },
                (v) => v.Should().Be(-114),
                Commands.GetRssiThreshold,
                "0x8E");
        }

        [TestMethod]
        public void TestGetSensitivityBoost()
        {
            ExecuteGetTest(
                () => { return _rfm6x.SensitivityBoost; },
                (v) => v.Should().BeTrue(),
                Commands.GetSensitivityBoost,
                "1");
        }

        [TestMethod]
        public void TestGetSequencer()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Sequencer; },
                (v) => v.Should().BeTrue(),
                Commands.GetSequencer,
                "1");
        }

        [TestMethod]
        public void TestGetSyncBitErrors()
        {
            ExecuteGetTest(
                () => { return _rfm6x.SyncBitErrors; },
                (v) => v.Should().Be(0xA0),
                Commands.GetSyncBitErrors,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTimeout()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.ReadTimeout)
                .Returns(1000);

            // Act
            var result = _rfm6x.Timeout;

            // Assert
            result.Should().Be(1000);
        }

        [TestMethod]
        public void TestGetTimeoutRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm6x.TimeoutRssiThreshold; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTimeoutRssiThreshold,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTimeoutRxStart()
        {
            ExecuteGetTest(
                () => { return _rfm6x.TimeoutRxStart; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTimeoutRxStart,
                "0xA0");
        }

        [TestMethod]
        public void TestIrq()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .SetupSequence(_ => _.ReadLine())
                .Returns("0:CRC_OK")
                .Returns("1:PAYLOAD_READY")
                .Returns("1:FIFO_OVERRUN")
                .Returns("0:FIFO_LEVEL")
                .Returns("1:FIFO_NOT_EMPTY")
                .Returns("1:FIFO_FULL")
                .Returns("0:ADDRESS_MATCH")
                .Returns("1:AUTO_MODE")
                .Returns("0:TIMEOUT")
                .Returns("1:RSSI")
                .Returns("1:PLL_LOCK")
                .Returns("0:MODE_RDY")
                .Returns("1:RX_RDY");

            // Act
            var result = _rfm6x.Irq;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetIrq}\n"));

            result.Should().Be(Irq.PayloadReady | Irq.FifoOverrun | Irq.FifoNotEmpty | Irq.FifoFull | Irq.AutoMode | Irq.Rssi | Irq.PllLock);
        }

        [TestMethod]
        public void TestListenAbort()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteListenAbort(); },
                Commands.ExecuteListenAbort,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestMeasureTemperature()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteMeasureTemperature(); },
                Commands.ExecuteMeasureTemperature,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestOpen()
        {
            // Arrange
            MockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(MockSerialPort.Object);

            MockSerialPort
            .Setup(_ => _.ReadLine())
                .Returns("RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz");

            // Act
            _rfm6x.Open("ComPort", 9600);

            // Assert
            MockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            MockSerialPort.Verify(_ => _.Open(), Times.Once);
        }

        [TestMethod]
        public void TestRestartRx()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteRestartRx(); },
                Commands.ExecuteRestartRx,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestSetAesKey()
        {
            ExecuteTest(
                () => { _rfm6x.SetAesKey(new List<byte>() { 0xFF, 0xAA, 0xBB }); },
                $"{Commands.ExecuteSetAesKey} FFAABB",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestSetAesOn()
        {
            ExecuteSetTest(
                () => { _rfm6x.AesOn = true; },
                Commands.SetAesOn,
                "1");
        }

        [TestMethod]
        public void TestSetAfcLowBetaOn()
        {
            ExecuteSetTest(
                () => { _rfm6x.AfcLowBetaOn = true; },
                Commands.SetAfcLowBetaOn,
                "1");
        }

        [TestMethod]
        [DataRow(ContinuousDagc.Normal)]
        [DataRow(ContinuousDagc.ImprovedLowBeta0)]
        [DataRow(ContinuousDagc.ImprovedLowBeta1)]
        public void TestSetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.ContinuousDagc = expected; },
                Commands.SetContinuousDagc,
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
        public void TestSetDccFreq(DccFreq expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.DccFreq = expected; },
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
                () => { _rfm6x.DccFreqAfc = expected; },
                Commands.SetDccFreqAfc,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetDioInterruptMask()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
               .Setup(_ => _.ReadLine())
               .Returns(RfmBase.ResponseOk);

            // Act
            _rfm6x.DioInterruptMask = DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4 | DioIrq.Dio5;

            // Assert
            MockSerialPort
                .Verify(_ => _.Write($"{Commands.SetDioInterrupt} 0x{(byte)(DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4 | DioIrq.Dio5) >> 1:X}\n"),
                Times.Once);
        }

        [TestMethod]
        [DataRow(EnterCondition.CrcOk)]
        [DataRow(EnterCondition.FifoEmpty)]
        [DataRow(EnterCondition.FifoLevel)]
        [DataRow(EnterCondition.FifoNotEmpty)]
        [DataRow(EnterCondition.Off)]
        [DataRow(EnterCondition.PacketSent)]
        [DataRow(EnterCondition.PayloadReady)]
        [DataRow(EnterCondition.SyncAddressMatch)]
        public void TestSetEnterCondition(EnterCondition expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.EnterCondition = expected; },
                Commands.SetEnterCondition,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(ExitCondition.CrcOk)]
        [DataRow(ExitCondition.FifoEmpty)]
        [DataRow(ExitCondition.FifoLevel)]
        [DataRow(ExitCondition.Off)]
        [DataRow(ExitCondition.PacketSent)]
        [DataRow(ExitCondition.PayloadReady)]
        [DataRow(ExitCondition.RxTimeout)]
        [DataRow(ExitCondition.SyncAddressMatch)]
        public void TestSetExitCondition(ExitCondition expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.ExitCondition = expected; },
                Commands.SetExitCondition,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetFifoFill()
        {
            ExecuteSetTest(
                () => { _rfm6x.FifoFill = true; },
                Commands.SetFifoFill,
                "1");
        }

        [TestMethod]
        public void TestSetImpedance()
        {
            ExecuteSetTest(
                () => { _rfm6x.Impedance = true; },
                Commands.SetImpedance,
                "1");
        }

        [TestMethod]
        [DataRow(IntermediateMode.Rx)]
        [DataRow(IntermediateMode.Sleep)]
        [DataRow(IntermediateMode.Standby)]
        [DataRow(IntermediateMode.Tx)]
        public void TestSetIntermediateMode(IntermediateMode expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.IntermediateMode = expected; },
                Commands.SetIntermediateMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetListenCoefficentIdle()
        {
            ExecuteSetTest(
                () => { _rfm6x.ListenCoefficentIdle = 0x60; },
                Commands.SetListenCoefficentIdle,
                "0x60");
        }

        [TestMethod]
        public void TestSetListenCoefficentRx()
        {
            ExecuteSetTest(
                () => { _rfm6x.ListenCoefficentRx = 0x60; },
                Commands.SetListenCoefficentRx,
                "0x60");
        }

        [TestMethod]
        public void TestSetListenCriteria()
        {
            ExecuteSetTest(
                () => { _rfm6x.ListenCriteria = true; },
                Commands.SetListenCriteria,
                "1");
        }

        [TestMethod]
        [DataRow(ListenEnd.Idle)]
        [DataRow(ListenEnd.Mode)]
        [DataRow(ListenEnd.Reserved)]
        [DataRow(ListenEnd.Rx)]
        public void TestSetListenEnd(ListenEnd expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.ListenEnd = expected; },
                Commands.SetListenEnd,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetListenerOn()
        {
            ExecuteSetTest(
                () => { _rfm6x.ListenerOn = true; },
                Commands.SetListenerOn,
                "1");
        }

        [TestMethod]
        [DataRow(ListenResolution.Idle262ms)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Reserved)]
        public void TestSetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.ListenResolutionIdle = expected; },
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
                () => { _rfm6x.ListenResolutionRx = expected; },
                Commands.SetListenResolutionRx,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetLowBetaAfcOffset()
        {
            ExecuteSetTest(
                () => { _rfm6x.LowBetaAfcOffset = 0x60; },
                Commands.SetLowBetaAfcOffset,
                "0x60");
        }

        [TestMethod]
        public void TestSetOutputPower()
        {
            ExecuteSetTest(
                () => { _rfm6x.OutputPower = -2; },
                Commands.SetOutputPower,
                 $"0x{(sbyte)-2:X2}");
        }
        [TestMethod]
        public void TestSetRadioConfig()
        {
            ExecuteSetTest(
                () => { _rfm6x.RadioConfig = 0xB0; },
                Commands.SetRadioConfig,
                "0xB0");
        }

        [TestMethod]
        public void TestSetRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfm6x.RssiThreshold = -114; },
                Commands.SetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }

        [TestMethod]
        public void TestSetSensitivityBoost()
        {
            ExecuteSetTest(
                () => { _rfm6x.SensitivityBoost = true; },
                Commands.SetSensitivityBoost,
                "1");
        }

        [TestMethod]
        public void TestSetSequencer()
        {
            ExecuteSetTest(
                () => { _rfm6x.Sequencer = true; },
                Commands.SetSequencer,
                "1");
        }

        [TestMethod]
        public void TestSetSyncBitErrors()
        {
            ExecuteSetTest(
                () => { _rfm6x.SyncBitErrors = 0xB0; },
                Commands.SetSyncBitErrors,
                "0xB0");
        }

        [TestMethod]
        public void TestSetTimeout()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            // Act
            _rfm6x.Timeout = 1000;

            // Assert
            MockSerialPort.VerifySet(_ => _.ReadTimeout = 1000, Times.Once);
        }

        [TestMethod]
        public void TestSetTimeoutRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfm6x.TimeoutRssiThreshold = 0xB0; },
                Commands.SetTimeoutRssiThreshold,
                "0xB0");
        }

        [TestMethod]
        public void TestSetTimeoutRxStart()
        {
            ExecuteSetTest(
                () => { _rfm6x.TimeoutRxStart = 0xB0; },
                Commands.SetTimeoutRxStart,
                "0xB0");
        }

        [TestMethod]
        public void TestStartRssi()
        {
            ExecuteTest(
                () => { _rfm6x.ExecuteStartRssi(); },
                Commands.ExecuteStartRssi,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestWaitForIrq()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("DIO PIN IRQ");

            // Act
            _rfm6x.WaitForIrq();

            // Arrange
            MockSerialPort
                .Verify(_ => _.ReadLine());
        }

        [TestMethod]
        public void TestWaitForIrqNoIrqResponse()
        {
            // Arrange
            _rfm6x.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("");

            // Act
            Action action = () => _rfm6x.WaitForIrq();

            // Arrange
            action
                .Should()
                .Throw<RfmUsbCommandExecutionException>()
                .WithMessage("Invalid response received for IRQ signal: []");
        }
    }
}