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
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RfmUsb.Exceptions;
using RfmUsb.Net;
using RfmUsb.Ports;
using System;
using System.Collections.Generic;
using System.IO;

namespace RfmUsb.UnitTests
{
    [TestClass]
    public class Rfm6xTests
    {
        private readonly ILogger<IRfm> _logger;
        private readonly Mock<ISerialPort> _mockSerialPort;
        private readonly Mock<ISerialPortFactory> _mockSerialPortFactory;
        private readonly Rfm6x _rfm6x;

        public Rfm6xTests()
        {
            _mockSerialPortFactory = new Mock<ISerialPortFactory>();

            _mockSerialPort = new Mock<ISerialPort>();

            _logger = Mock.Of<ILogger<IRfm>>();

            _rfm6x = new Rfm6x(_logger, _mockSerialPortFactory.Object);
        }

        [TestMethod]
        public void TestAfcClear()
        {
            ExecuteTest(
                () => { _rfm6x.AfcClear(); },
                Commands.ExecuteAfcClear,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestAfcStart()
        {
            ExecuteTest(
                () => { _rfm6x.AfcStart(); },
                Commands.ExecuteAfcStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestClose()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            // Act
            _rfm6x.Close();

            // Assert
            _mockSerialPort.Verify(_ => _.Close());
        }

        [TestMethod]
        public void TestFeiStart()
        {
            ExecuteTest(
                () => { _rfm6x.FeiStart(); },
                Commands.ExecuteFeiStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestGetAddressFilter()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(AddressFilter.NodeBroaddcast.ToString("X"));

            // Act
            var addressFilter = _rfm6x.AddressFiltering;

            // Assert
            addressFilter.Should().Be(AddressFilter.NodeBroaddcast);
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
        public void TestGetAfc()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("0x100");

            // Act
            var afc = _rfm6x.Afc;

            // Assert
            afc.Should().Be(0x100);
        }

        [TestMethod]
        public void TestGetAfcAutoClear()
        {
            ExecuteGetTest(
                () => { return _rfm6x.AfcAutoClear; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcAutoClear,
                "1");
        }

        [TestMethod]
        public void TestGetAfcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfm6x.AfcAutoOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcAutoOn,
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
        public void TestGetAutoRxRestartOn()
        {
            ExecuteGetTest(
                () => { return _rfm6x.AutoRxRestartOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAutoRxRestartOn,
                "1");
        }

        [TestMethod]
        public void TestGetBitRate()
        {
            ExecuteGetTest(
                () => { return _rfm6x.BitRate; },
                (v) => { v.Should().Be(0x100); },
                Commands.GetBitRate,
                "0x100");
        }

        [TestMethod]
        public void TestGetBroadcastAddress()
        {
            ExecuteGetTest(
                () => { return _rfm6x.BroadcastAddress; },
                (v) => { v.Should().Be(0x55); },
                Commands.GetBroadcastAddress,
                "0x55");
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
        public void TestGetCrcAutoClear()
        {
            ExecuteGetTest(
                () => { return _rfm6x.CrcAutoClear; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcAutoClear,
                "1");
        }

        [TestMethod]
        public void TestGetCrcOn()
        {
            ExecuteGetTest(
                () => { return _rfm6x.CrcOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcOn,
                "1");
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
        [DataRow(DcFree.Manchester)]
        [DataRow(DcFree.None)]
        [DataRow(DcFree.Reserved)]
        [DataRow(DcFree.Whitening)]
        public void TestGetDcFree(DcFree expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.DcFree; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDcFree,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetDioInterruptMask()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
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

            _mockSerialPort.Verify(_ => _.Write($"{Commands.GetDioInterrupt}\n"), Times.Once);
        }

        [TestMethod]
        [DataRow(Dio.Dio0)]
        [DataRow(Dio.Dio1)]
        [DataRow(Dio.Dio2)]
        [DataRow(Dio.Dio3)]
        [DataRow(Dio.Dio4)]
        [DataRow(Dio.Dio5)]
        public void TestGetDioMapping(Dio dio)
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("0x0000-Map 00");

            // Act
            var result = _rfm6x.GetDioMapping(dio);

            // Assert
            result.Should().Be(DioMapping.DioMapping0);

            _mockSerialPort.Verify(_ => _.Write($"{Commands.GetDioMapping} 0x{(byte)dio:X}\n"));
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

        // Fei
        [TestMethod]
        public void TestGetFei()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Fei; },
                (v) => v.Should().Be(0x200),
                Commands.GetFei,
                "0x200");
        }

        [TestMethod]
        public void TestGetFifo()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Fifo; },
                (v) => v.Should().BeEquivalentTo(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }),
                Commands.GetFifo,
                "0xAA55DEAD");
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
        public void TestGetFifoThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm6x.FifoThreshold; },
                (v) => v.Should().Be(0x10),
                Commands.GetFifoThreshold,
                "0x10");
        }

        [TestMethod]
        public void TestGetFrequency()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Frequency; },
                (v) => v.Should().Be(0x100000),
                Commands.GetFrequency,
                "0x100000");
        }

        [TestMethod]
        public void TestGetFrequencyDeviation()
        {
            ExecuteGetTest(
                () => { return _rfm6x.FrequencyDeviation; },
                (v) => v.Should().Be(0xA000),
                Commands.GetFrequencyDeviation,
                "0xA000");
        }

        [TestMethod]
        [DataRow(FskModulationShaping.GaussianBt0_3)]
        [DataRow(FskModulationShaping.GaussianBt0_5)]
        [DataRow(FskModulationShaping.GaussianBt1_0)]
        public void TestGetFskModulationShaping(FskModulationShaping expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.FskModulationShaping; },
                (v) => v.Should().Be(expected),
                Commands.GetFskModulationShaping,
                $"0x{expected:X}");
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
        public void TestGetInterPacketRxDelay()
        {
            ExecuteGetTest(
                () => { return _rfm6x.InterPacketRxDelay; },
                (v) => v.Should().Be(0xAA),
                Commands.GetInterPacketRxDelay,
                "0xAA");
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
        [DataRow(LnaGain.Auto)]
        [DataRow(LnaGain.Max)]
        [DataRow(LnaGain.MaxMinus12db)]
        [DataRow(LnaGain.MaxMinus24db)]
        [DataRow(LnaGain.MaxMinus36db)]
        [DataRow(LnaGain.MaxMinus48db)]
        [DataRow(LnaGain.MaxMinus6db)]
        public void TestGetLnaGainSelect(LnaGain expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.LnaGainSelect; },
                (v) => v.Should().Be(expected),
                Commands.GetLnaGainSelect,
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
        [DataRow(Mode.Rx)]
        [DataRow(Mode.Sleep)]
        [DataRow(Mode.Standby)]
        [DataRow(Mode.Synth)]
        [DataRow(Mode.Tx)]
        public void TestGetMode(Mode expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.Mode; },
                (v) => v.Should().Be(expected),
                Commands.GetMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(Modulation.Fsk)]
        [DataRow(Modulation.Ook)]
        public void TestGetModulation(Modulation expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.Modulation; },
                (v) => v.Should().Be(expected),
                Commands.GetModulation,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetNodeAddress()
        {
            ExecuteGetTest(
                () => { return _rfm6x.NodeAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetNodeAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetOcpEnable()
        {
            ExecuteGetTest(
                () => { return _rfm6x.OcpEnable; },
                (v) => v.Should().BeTrue(),
                Commands.GetOcpEnable,
                "1");
        }

        [TestMethod]
        [DataRow(OcpTrim.OcpTrim100)]
        [DataRow(OcpTrim.OcpTrim105)]
        [DataRow(OcpTrim.OcpTrim110)]
        [DataRow(OcpTrim.OcpTrim115)]
        [DataRow(OcpTrim.OcpTrim120)]
        [DataRow(OcpTrim.OcpTrim45)]
        [DataRow(OcpTrim.OcpTrim50)]
        [DataRow(OcpTrim.OcpTrim55)]
        [DataRow(OcpTrim.OcpTrim60)]
        [DataRow(OcpTrim.OcpTrim65)]
        [DataRow(OcpTrim.OcpTrim70)]
        [DataRow(OcpTrim.OcpTrim75)]
        [DataRow(OcpTrim.OcpTrim80)]
        [DataRow(OcpTrim.OcpTrim85)]
        [DataRow(OcpTrim.OcpTrim90)]
        [DataRow(OcpTrim.OcpTrim95)]
        public void TestGetOcpTrim(OcpTrim expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.OcpTrim; },
                (v) => v.Should().Be(expected),
                Commands.GetOcpTrim,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokAverageThresholdFilter.ChipRate2)]
        [DataRow(OokAverageThresholdFilter.ChipRate32)]
        [DataRow(OokAverageThresholdFilter.ChipRate4)]
        [DataRow(OokAverageThresholdFilter.ChipRate8)]
        public void TestGetOokAverageThresholdFilter(OokAverageThresholdFilter expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.OokAverageThresholdFilter; },
                (v) => v.Should().Be(expected),
                Commands.GetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetOokFixedThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm6x.OokFixedThreshold; },
                (v) => v.Should().Be(0xAA),
                Commands.GetOokFixedThreshold,
                "0xAA");
        }

        [TestMethod]
        [DataRow(OokModulationShaping.Filtering2Br)]
        [DataRow(OokModulationShaping.FilteringBr)]
        [DataRow(OokModulationShaping.None)]
        [DataRow(OokModulationShaping.Reserved)]
        public void TestGetOokModulationShaping(OokModulationShaping expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.OokModulationShaping; },
                (v) => v.Should().Be(expected),
                Commands.GetOokModulationShaping,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokThresholdDec.EightTimesInEachChip)]
        [DataRow(OokThresholdDec.FourTimesInEachChip)]
        [DataRow(OokThresholdDec.OnceEvery2Chips)]
        [DataRow(OokThresholdDec.OnceEvery4Chips)]
        [DataRow(OokThresholdDec.OnceEvery8Chips)]
        [DataRow(OokThresholdDec.OncePerChip)]
        [DataRow(OokThresholdDec.SixteeenTimesInEachChip)]
        [DataRow(OokThresholdDec.TwiceInEachChip)]
        public void TestGetOokPeakThresholdDec(OokThresholdDec expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.OokPeakThresholdDec; },
                (v) => v.Should().Be(expected),
                Commands.GetOokPeakThresholdDec,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokThresholdStep.Step0_5db)]
        [DataRow(OokThresholdStep.Step1db)]
        [DataRow(OokThresholdStep.Step1_5db)]
        [DataRow(OokThresholdStep.Step2db)]
        [DataRow(OokThresholdStep.Step3db)]
        [DataRow(OokThresholdStep.Step4db)]
        [DataRow(OokThresholdStep.Step5db)]
        [DataRow(OokThresholdStep.Step6db)]
        public void TestGetOokPeakThresholdStep(OokThresholdStep expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.OokPeakThresholdStep; },
                (v) => v.Should().Be(expected),
                Commands.GetOokPeakThresholdStep,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokThresholdType.Average)]
        [DataRow(OokThresholdType.Fixed)]
        [DataRow(OokThresholdType.Peak)]
        [DataRow(OokThresholdType.Reserved)]
        public void TestGetOokThresholdType(OokThresholdType expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.OokThresholdType; },
                (v) => v.Should().Be(expected),
                Commands.GetOokThresholdType,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetOutputPower()
        {
            ExecuteGetTest(
                () => { return _rfm6x.OutputPower; },
                (v) => v.Should().Be(0x60000),
                Commands.GetOutputPower,
                "0x60000");
        }

        [TestMethod]
        public void TestGetPacketFormat()
        {
            ExecuteGetTest(
                () => { return _rfm6x.PacketFormat; },
                (v) => v.Should().BeTrue(),
                Commands.GetPacketFormat,
                "1");
        }

        [TestMethod]
        [DataRow(PaRamp.PowerAmpRamp10)]
        [DataRow(PaRamp.PowerAmpRamp100)]
        [DataRow(PaRamp.PowerAmpRamp1000)]
        [DataRow(PaRamp.PowerAmpRamp12)]
        [DataRow(PaRamp.PowerAmpRamp125)]
        [DataRow(PaRamp.PowerAmpRamp15)]
        [DataRow(PaRamp.PowerAmpRamp20)]
        [DataRow(PaRamp.PowerAmpRamp2000)]
        [DataRow(PaRamp.PowerAmpRamp25)]
        [DataRow(PaRamp.PowerAmpRamp250)]
        [DataRow(PaRamp.PowerAmpRamp31)]
        [DataRow(PaRamp.PowerAmpRamp3400)]
        [DataRow(PaRamp.PowerAmpRamp40)]
        [DataRow(PaRamp.PowerAmpRamp50)]
        [DataRow(PaRamp.PowerAmpRamp500)]
        [DataRow(PaRamp.PowerAmpRamp62)]
        public void TestGetPaRamp(PaRamp expected)
        {
            ExecuteGetTest(
                () => { return _rfm6x.PaRamp; },
                (v) => v.Should().Be(expected),
                Commands.GetPaRamp,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPayloadLength()
        {
            ExecuteGetTest(
                () => { return _rfm6x.PayloadLength; },
                (v) => v.Should().Be(0x60),
                Commands.GetPayloadLength,
                "0x60");
        }

        [TestMethod]
        public void TestGetPreambleSize()
        {
            ExecuteGetTest(
                () => { return _rfm6x.PreambleSize; },
                (v) => v.Should().Be(0x6000),
                Commands.GetPreambleSize,
                "0x6000");
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
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
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
        public void TestGetRssi()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Rssi; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRssi,
                "0xA0");
        }

        [TestMethod]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm6x.RssiThreshold; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRssiThreshold,
                "0xA0");
        }

        [TestMethod]
        public void TestGetRxBw()
        {
            ExecuteGetTest(
                () => { return _rfm6x.RxBw; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRxBw,
                "0xA0");
        }

        [TestMethod]
        public void TestGetRxBwAfc()
        {
            ExecuteGetTest(
                () => { return _rfm6x.RxBwAfc; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRxBwAfc,
                "0xA0");
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

        // Sync
        [TestMethod]
        public void TestGetSync()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Sync; },
                (v) => v.Should().BeEquivalentTo(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }),
                Commands.GetSync,
                "0xAA55DEAD");
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
        public void TestGetSyncEnable()
        {
            ExecuteGetTest(
                () => { return _rfm6x.SyncEnable; },
                (v) => v.Should().BeTrue(),
                Commands.GetSyncEnable,
                "1");
        }

        [TestMethod]
        public void TestGetSyncSize()
        {
            ExecuteGetTest(
                () => { return _rfm6x.SyncSize; },
                (v) => v.Should().Be(0xA0),
                Commands.GetSyncSize,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTemperatureValue()
        {
            ExecuteGetTest(
                () => { return _rfm6x.TemperatureValue; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTemperatureValue,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTimeout()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
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
        public void TestGetTxStartCondition()
        {
            ExecuteGetTest(
                () => { return _rfm6x.TxStartCondition; },
                (v) => v.Should().BeTrue(),
                Commands.GetTxStartCondition,
                "1");
        }

        [TestMethod]
        public void TestGetVersion()
        {
            ExecuteGetTest(
                () => { return _rfm6x.Version; },
                (v) => v.Should().Be("1.2"),
                Commands.GetVersion,
                "1.2");
        }

        [TestMethod]
        public void TestIrq()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
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
            _mockSerialPort.Verify(_ => _.Write($"{Commands.GetIrq}\n"));

            result.Should().Be(Irq.PayloadReady | Irq.FifoOverrun | Irq.FifoNotEmpty | Irq.FifoFull | Irq.AutoMode | Irq.Rssi | Irq.PllLock);
        }

        [TestMethod]
        public void TestListenAbort()
        {
            ExecuteTest(
                () => { _rfm6x.ListenAbort(); },
                Commands.ExecuteListenAbort,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestMeasureTemperature()
        {
            ExecuteTest(
                () => { _rfm6x.MeasureTemperature(); },
                Commands.ExecuteMeasureTemperature,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestOpen()
        {
            // Arrange
            _mockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(_mockSerialPort.Object);

            // Act
            _rfm6x.Open("ComPort", 9600);

            // Assert
            _mockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            _mockSerialPort.Verify(_ => _.Open(), Times.Once);
        }

        [TestMethod]
        public void TestOpenNotFound()
        {
            // Arrange
            _mockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(_mockSerialPort.Object);

            _mockSerialPortFactory
                .Setup(_ => _.GetSerialPorts())
                .Returns(new List<string>() { });

            _mockSerialPort
                .Setup(_ => _.Open())
                .Throws<FileNotFoundException>();

            // Act
            Action action = () => _rfm6x.Open("ComPort", 9600);

            // Assert
            action.Should().Throw<RfmUsbSerialPortNotFoundException>();
        }

        [TestMethod]
        public void TestRcCalibration()
        {
            ExecuteTest(
                () => { _rfm6x.RcCalibration(); },
                Commands.ExecuteRcCalibration,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestReset()
        {
            ExecuteTest(
                () => { _rfm6x.Reset(); },
                Commands.ExecuteReset,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestRestartRx()
        {
            ExecuteTest(
                () => { _rfm6x.RestartRx(); },
                Commands.ExecuteRestartRx,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestSetAddressFilter()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            // Act
            _rfm6x.AddressFiltering = AddressFilter.Node;

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{Commands.SetAddressFiltering} 0x{AddressFilter.Node:X}\n"), Times.Once);
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
        public void TestSetAfcAutoClear()
        {
            ExecuteSetTest(
                () => { _rfm6x.AfcAutoClear = true; },
                Commands.SetAfcAutoClear,
                "1");
        }

        [TestMethod]
        public void TestSetAfcAutoOn()
        {
            ExecuteSetTest(
                () => { _rfm6x.AfcAutoOn = true; },
                Commands.SetAfcAutoOn,
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
        public void TestSetAutoRxRestartOn()
        {
            ExecuteSetTest(
                () => { _rfm6x.AutoRxRestartOn = true; },
                Commands.SetAutoRxRestartOn,
                "1");
        }

        [TestMethod]
        public void TestSetBitRate()
        {
            ExecuteSetTest(
                () => { _rfm6x.BitRate = 0x100; },
                Commands.SetBitRate,
                "0x100");
        }

        [TestMethod]
        public void TestSetBroadcastAddress()
        {
            ExecuteSetTest(
                () => { _rfm6x.BroadcastAddress = 0xAA; },
                Commands.SetBroadcastAddress,
                "0xAA");
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
        public void TestSetCrcAutoClear()
        {
            ExecuteSetTest(
                () => { _rfm6x.CrcAutoClear = true; },
                Commands.SetCrcAutoClear,
                "1");
        }

        [TestMethod]
        public void TestSetCrcOn()
        {
            ExecuteSetTest(
                () => { _rfm6x.CrcOn = true; },
                Commands.SetCrcOn,
                "1");
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
        [DataRow(DcFree.Manchester)]
        [DataRow(DcFree.None)]
        [DataRow(DcFree.Reserved)]
        [DataRow(DcFree.Whitening)]
        public void TestSetDcFree(DcFree expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.DcFree = expected; },
                Commands.SetDcFree,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetDioInterruptMask()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
               .Setup(_ => _.ReadLine())
               .Returns(RfmBase.ResponseOk);

            // Act
            _rfm6x.DioInterruptMask = DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4 | DioIrq.Dio5;

            // Assert
            _mockSerialPort
                .Verify(_ => _.Write($"{Commands.SetDioInterrupt} 0x{(byte)(DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4 | DioIrq.Dio5) >> 1:X}\n"),
                Times.Once);
        }

        [TestMethod]
        [DataRow(Dio.Dio0, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio1, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio2, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio3, DioMapping.DioMapping3)]
        [DataRow(Dio.Dio4, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio5, DioMapping.DioMapping1)]
        public void TestSetDioMapping(Dio dio, DioMapping mapping)
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
               .Setup(_ => _.ReadLine())
               .Returns(RfmBase.ResponseOk);

            // Act
            _rfm6x.SetDioMapping(dio, mapping);

            // Assert
            _mockSerialPort
                .Verify(_ => _.Write($"{Commands.SetDioMapping} {(byte)dio} {(byte)mapping}\n"),
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
        public void TestSetFifo()
        {
            ExecuteSetTest(
                () => { _rfm6x.Fifo = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetFifo,
                "AA55DEAD");
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
        public void TestSetFifoThreshold()
        {
            ExecuteSetTest(
                () => { _rfm6x.FifoThreshold = 0x10; },
                Commands.SetFifoThreshold,
                "0x10");
        }

        [TestMethod]
        public void TestSetFrequency()
        {
            ExecuteSetTest(
                () => { _rfm6x.Frequency = 0x100000; },
                Commands.SetFrequency,
                "0x100000");
        }

        [TestMethod]
        public void TestSetFrequencyDeviation()
        {
            ExecuteSetTest(
                () => { _rfm6x.FrequencyDeviation = 0xA000; },
                Commands.SetFrequencyDeviation,
                "0xA000");
        }

        [TestMethod]
        [DataRow(FskModulationShaping.GaussianBt0_3)]
        [DataRow(FskModulationShaping.GaussianBt0_5)]
        [DataRow(FskModulationShaping.GaussianBt1_0)]
        public void TestSetFskModulationShaping(FskModulationShaping expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.FskModulationShaping = expected; },
                Commands.SetFskModulationShaping,
                $"0x{expected:X}");
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
        public void TestSetInterPacketRxDelay()
        {
            ExecuteSetTest(
                () => { _rfm6x.InterPacketRxDelay = 0x60; },
                Commands.SetInterPacketRxDelay,
                "0x60");
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
        [DataRow(LnaGain.Auto)]
        [DataRow(LnaGain.Max)]
        [DataRow(LnaGain.MaxMinus12db)]
        [DataRow(LnaGain.MaxMinus24db)]
        [DataRow(LnaGain.MaxMinus36db)]
        [DataRow(LnaGain.MaxMinus48db)]
        [DataRow(LnaGain.MaxMinus6db)]
        public void TestSetLnaGainSelect(LnaGain expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.LnaGainSelect = expected; },
                Commands.SetLnaGainSelect,
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
        [DataRow(Mode.Rx)]
        [DataRow(Mode.Sleep)]
        [DataRow(Mode.Standby)]
        [DataRow(Mode.Synth)]
        [DataRow(Mode.Tx)]
        public void TestSetMode(Mode expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.Mode = expected; },
                Commands.SetMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(Modulation.Fsk)]
        [DataRow(Modulation.Ook)]
        public void TestSetModulation(Modulation expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.Modulation = expected; },
                Commands.SetModulation,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetNodeAddress()
        {
            ExecuteSetTest(
                () => { _rfm6x.NodeAddress = 0x60; },
                Commands.SetNodeAddress,
                "0x60");
        }

        [TestMethod]
        public void TestSetOcpEnable()
        {
            ExecuteSetTest(
                () => { _rfm6x.OcpEnable = true; },
                Commands.SetOcpEnable,
                "1");
        }

        [TestMethod]
        [DataRow(OcpTrim.OcpTrim100)]
        [DataRow(OcpTrim.OcpTrim105)]
        [DataRow(OcpTrim.OcpTrim110)]
        [DataRow(OcpTrim.OcpTrim115)]
        [DataRow(OcpTrim.OcpTrim120)]
        [DataRow(OcpTrim.OcpTrim45)]
        [DataRow(OcpTrim.OcpTrim50)]
        [DataRow(OcpTrim.OcpTrim55)]
        [DataRow(OcpTrim.OcpTrim60)]
        [DataRow(OcpTrim.OcpTrim65)]
        [DataRow(OcpTrim.OcpTrim70)]
        [DataRow(OcpTrim.OcpTrim75)]
        [DataRow(OcpTrim.OcpTrim80)]
        [DataRow(OcpTrim.OcpTrim85)]
        [DataRow(OcpTrim.OcpTrim90)]
        [DataRow(OcpTrim.OcpTrim95)]
        public void TestSetOcpTrim(OcpTrim expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.OcpTrim = expected; },
                Commands.SetOcpTrim,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokAverageThresholdFilter.ChipRate2)]
        [DataRow(OokAverageThresholdFilter.ChipRate32)]
        [DataRow(OokAverageThresholdFilter.ChipRate4)]
        [DataRow(OokAverageThresholdFilter.ChipRate8)]
        public void TestSetOokAverageThresholdFilter(OokAverageThresholdFilter expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.OokAverageThresholdFilter = expected; },
                Commands.SetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetOokFixedThreshold()
        {
            ExecuteSetTest(
                () => { _rfm6x.OokFixedThreshold = 0x60; },
                Commands.SetOokFixedThreshold,
                "0x60");
        }

        [TestMethod]
        [DataRow(OokModulationShaping.Filtering2Br)]
        [DataRow(OokModulationShaping.FilteringBr)]
        [DataRow(OokModulationShaping.None)]
        [DataRow(OokModulationShaping.Reserved)]
        public void TestSetOokModulationShaping(OokModulationShaping expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.OokModulationShaping = expected; },
                Commands.SetOokModulationShaping,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokThresholdDec.EightTimesInEachChip)]
        [DataRow(OokThresholdDec.FourTimesInEachChip)]
        [DataRow(OokThresholdDec.OnceEvery2Chips)]
        [DataRow(OokThresholdDec.OnceEvery4Chips)]
        [DataRow(OokThresholdDec.OnceEvery8Chips)]
        [DataRow(OokThresholdDec.OncePerChip)]
        [DataRow(OokThresholdDec.SixteeenTimesInEachChip)]
        [DataRow(OokThresholdDec.TwiceInEachChip)]
        public void TestSetOokPeakThresholdDec(OokThresholdDec expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.OokPeakThresholdDec = expected; },
                Commands.SetOokPeakThresholdDec,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokThresholdStep.Step0_5db)]
        [DataRow(OokThresholdStep.Step1db)]
        [DataRow(OokThresholdStep.Step1_5db)]
        [DataRow(OokThresholdStep.Step2db)]
        [DataRow(OokThresholdStep.Step3db)]
        [DataRow(OokThresholdStep.Step4db)]
        [DataRow(OokThresholdStep.Step5db)]
        [DataRow(OokThresholdStep.Step6db)]
        public void TestSetOokPeakThresholdStep(OokThresholdStep expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.OokPeakThresholdStep = expected; },
                Commands.SetOokPeakThresholdStep,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokThresholdType.Average)]
        [DataRow(OokThresholdType.Fixed)]
        [DataRow(OokThresholdType.Peak)]
        [DataRow(OokThresholdType.Reserved)]
        public void TestSetOokThresholdType(OokThresholdType expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.OokThresholdType = expected; },
                Commands.SetOokThresholdType,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetOutputPower()
        {
            ExecuteSetTest(
                () => { _rfm6x.OutputPower = 0x60000; },
                Commands.SetOutputPower,
                "0x60000");
        }

        [TestMethod]
        public void TestSetPacketFormat()
        {
            ExecuteSetTest(
                () => { _rfm6x.PacketFormat = true; },
                Commands.SetPacketFormat,
                "1");
        }

        [TestMethod]
        [DataRow(PaRamp.PowerAmpRamp10)]
        [DataRow(PaRamp.PowerAmpRamp100)]
        [DataRow(PaRamp.PowerAmpRamp1000)]
        [DataRow(PaRamp.PowerAmpRamp12)]
        [DataRow(PaRamp.PowerAmpRamp125)]
        [DataRow(PaRamp.PowerAmpRamp15)]
        [DataRow(PaRamp.PowerAmpRamp20)]
        [DataRow(PaRamp.PowerAmpRamp2000)]
        [DataRow(PaRamp.PowerAmpRamp25)]
        [DataRow(PaRamp.PowerAmpRamp250)]
        [DataRow(PaRamp.PowerAmpRamp31)]
        [DataRow(PaRamp.PowerAmpRamp3400)]
        [DataRow(PaRamp.PowerAmpRamp40)]
        [DataRow(PaRamp.PowerAmpRamp50)]
        [DataRow(PaRamp.PowerAmpRamp500)]
        [DataRow(PaRamp.PowerAmpRamp62)]
        public void TestSetPaRamp(PaRamp expected)
        {
            ExecuteSetTest(
                () => { _rfm6x.PaRamp = expected; },
                Commands.SetPaRamp,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetPayloadLength()
        {
            ExecuteSetTest(
                () => { _rfm6x.PayloadLength = 0x60; },
                Commands.SetPayloadLength,
                "0x60");
        }

        [TestMethod]
        public void TestSetPreambleSize()
        {
            ExecuteSetTest(
                () => { _rfm6x.PreambleSize = 0x6000; },
                Commands.SetPreambleSize,
                "0x6000");
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
                () => { _rfm6x.RssiThreshold = 0xB0; },
                Commands.SetRssiThreshold,
                "0xB0");
        }

        [TestMethod]
        public void TestSetRxBw()
        {
            ExecuteSetTest(
                () => { _rfm6x.RxBw = 0xB0; },
                Commands.SetRxBw,
                "0xB0");
        }

        [TestMethod]
        public void TestSetRxBwAfc()
        {
            ExecuteSetTest(
                () => { _rfm6x.RxBwAfc = 0xB0; },
                Commands.SetRxBwAfc,
                "0xB0");
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
        public void TestSetSync()
        {
            ExecuteSetTest(
                () => { _rfm6x.Sync = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetSync,
                "AA55DEAD");
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
        public void TestSetSyncEnable()
        {
            ExecuteSetTest(
                () => { _rfm6x.SyncEnable = true; },
                Commands.SetSyncEnable,
                "1");
        }

        [TestMethod]
        public void TestSetSyncSize()
        {
            ExecuteSetTest(
                () => { _rfm6x.SyncSize = 0xB0; },
                Commands.SetSyncSize,
                "0xB0");
        }

        [TestMethod]
        public void TestSetTimeout()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            // Act
            _rfm6x.Timeout = 1000;

            // Assert
            _mockSerialPort.VerifySet(_ => _.ReadTimeout = 1000, Times.Once);
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
        public void TestSetTxStartCondition()
        {
            ExecuteSetTest(
                () => { _rfm6x.TxStartCondition = true; },
                Commands.SetTxStartCondition,
                "1");
        }

        [TestMethod]
        public void TestStartRssi()
        {
            ExecuteTest(
                () => { _rfm6x.StartRssi(); },
                Commands.ExecuteStartRssi,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestTransmit()
        {
            ExecuteTest(
                () => { _rfm6x.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }); },
                $"{Commands.ExecuteTransmit} AADDFFCC",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestTransmitReceive()
        {
            ExecuteTest(
                () => { _rfm6x.TransmitReceive(new List<byte>() { 0xAA, 0x55 }); },
                $"{Commands.ExecuteTransmitReceive} AA55",
                "0xFEED");
        }

        [TestMethod]
        public void TestTransmitReceiveTimeout()
        {
            ExecuteTest(
                () => { _rfm6x.TransmitReceive(new List<byte>() { 0xAA, 0x55 }, 100); },
                $"{Commands.ExecuteTransmitReceive} AA55 100",
                "0xFEED");
        }

        [TestMethod]
        public void TestTransmitReceiveTransmitTimeout()
        {
            ExecuteTest(
                () => { _rfm6x.TransmitReceive(new List<byte>() { 0xAA, 0x55 }, 100, 200); },
                $"{Commands.ExecuteTransmitReceive} AA55 100 200",
                "0xFEED");
        }

        [TestMethod]
        public void TestTransmitWithTimeout()
        {
            ExecuteTest(
                () => { _rfm6x.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200); },
                $"{Commands.ExecuteTransmit} AADDFFCC 200",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestWaitForIrq()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("DIO PIN IRQ");

            // Act
            _rfm6x.WaitForIrq();

            // Arrange
            _mockSerialPort
                .Verify(_ => _.ReadLine());
        }

        [TestMethod]
        public void TestWaitForIrqNoIrqResponse()
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
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

        private void ExecuteGetTest<T>(Func<T> action, Action<T> validation, string command, string value)
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(value);

            // Act
            var result = action();

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{command}\n"));

            validation(result);
        }

        private void ExecuteSetTest(Action action, string command, string value)
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            // Act
            action();

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{command} {value}\n"), Times.Once);
        }

        private void ExecuteTest(Action action, string command, string response)
        {
            // Arrange
            _rfm6x.SerialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(response);

            // Act
            action();

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{command}\n"), Times.Once);
        }
    }
}