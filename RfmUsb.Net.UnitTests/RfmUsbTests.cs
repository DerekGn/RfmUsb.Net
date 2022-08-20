/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin
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
using Moq;
using RfmUsb.Net;
using RfmUsb.Ports;
using Serilog;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace RfmUsb.UnitTests
{
    public class RfmUsbTests
    {
        private readonly Mock<ISerialPort> _mockSerialPort;
        private readonly Mock<ISerialPortFactory> _mockSerialPortFactory;
        private readonly RfmUsb _rfmUsb;

        public RfmUsbTests(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output)
                .CreateLogger();

            _mockSerialPortFactory = new Mock<ISerialPortFactory>();
            _mockSerialPort = new Mock<ISerialPort>();

            _rfmUsb = new RfmUsb(Mock.Of<ILogger<IRfmUsb>>(), _mockSerialPortFactory.Object);
        }

        [Fact]
        public void TestGetAddressFilter()
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmUsb.ResponseOk);

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(AddressFilter.NodeBroaddcast.ToString("X"));

            // Act
            var addressFilter = _rfmUsb.AddressFiltering;

            // Assert
            addressFilter.Should().Be(AddressFilter.NodeBroaddcast);
        }

        [Fact]
        public void TestGetAesOn()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.AesOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAesOn,
                "1");
        }

        [Fact]
        public void TestOpen()
        {
            // Arrange
            _mockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(_mockSerialPort.Object);

            // Act
            _rfmUsb.Open("ComPort", 9600);

            // Assert
            _mockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            _mockSerialPort.Verify(_ => _.Open(), Times.Once);
        }

        [Fact]
        public void TestSetAddressFilter()
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmUsb.ResponseOk);

            // Act
            _rfmUsb.AddressFiltering = AddressFilter.Node;

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{Commands.SetAddressFiltering} 0x{AddressFilter.Node:X}\n"), Times.Once);
        }

        [Fact]
        public void TestSetAesOn()
        {
            ExecuteSetTest(
                () => { _rfmUsb.AesOn = true; },
                Commands.SetAesOn,
                "1");
        }

        [Fact]
        public void TestGetAfc()
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmUsb.ResponseOk);

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("0x100");

            // Act
            var afc = _rfmUsb.Afc;

            // Assert
            afc.Should().Be(0x100);
        }

        [Fact]
        public void TestGetAfcAutoClear()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.AfcAutoClear; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcAutoClear,
                "1");
        }

        [Fact]
        public void TestSetAfcAutoClear()
        {
            ExecuteSetTest(
                () => { _rfmUsb.AfcAutoClear = true; },
                Commands.SetAfcAutoClear,
                "1");
        }

        [Fact]
        public void TestGetAfcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.AfcAutoOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcAutoOn,
                "1");
        }

        [Fact]
        public void TestSetAfcAutoOn()
        {
            ExecuteSetTest(
                () => { _rfmUsb.AfcAutoOn = true; },
                Commands.SetAfcAutoOn,
                "1");
        }

        [Fact]
        public void TestGetAfcLowBetaOn()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.AfcLowBetaOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcLowBetaOn,
                "1");
        }

        [Fact]
        public void TestSetAfcLowBetaOn()
        {
            ExecuteSetTest(
                () => { _rfmUsb.AfcLowBetaOn = true; },
                Commands.SetAfcLowBetaOn,
                "1");
        }

        [Fact]
        public void TestGetAutoRxRestartOn()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.AutoRxRestartOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAutoRxRestartOn,
                "1");
        }

        [Fact]
        public void TestSetAutoRxRestartOn()
        {
            ExecuteSetTest(
                () => { _rfmUsb.AutoRxRestartOn = true; },
                Commands.SetAutoRxRestartOn,
                "1");
        }

        [Fact]
        public void TestGetBitRate()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.BitRate; },
                (v) => { v.Should().Be(0x100); },
                Commands.GetBitRate,
                "0x100");
        }

        [Fact]
        public void TestSetBitRate()
        {
            ExecuteSetTest(
                () => { _rfmUsb.BitRate = 0x100; },
                Commands.SetBitRate,
                "0x100");
        }

        [Fact]
        public void TestGetBroadcastAddress()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.BroadcastAddress; },
                (v) => { v.Should().Be(0x55); },
                Commands.GetBroadcastAddress,
                "0x55");
        }

        [Fact]
        public void TestSetBroadcastAddress()
        {
            ExecuteSetTest(
                () => { _rfmUsb.BroadcastAddress = 0xAA; },
                Commands.SetBroadcastAddress,
                "0xAA");
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestGetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ContinuousDagc; },
                (v) => { v.Should().Be(expected); },
                Commands.GetContinuousDagc,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestSetContinuousDagc(ContinuousDagc expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.ContinuousDagc = expected; },
                Commands.SetContinuousDagc,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetCrcAutoClear()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.CrcAutoClear; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcAutoClear,
                "1");
        }

        [Fact]
        public void TestSetCrcAutoClear()
        {
            ExecuteSetTest(
                () => { _rfmUsb.CrcAutoClear = true; },
                Commands.SetCrcAutoClear,
                "1");
        }

        [Fact]
        public void TestGetCrcOn()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.CrcOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcOn,
                "1");
        }

        [Fact]
        public void TestSetCrcOn()
        {
            ExecuteSetTest(
                () => { _rfmUsb.CrcOn = true; },
                Commands.SetCrcOn,
                "1");
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
                () => { return _rfmUsb.CurrentLnaGain; },
                (v) => v.Should().Be(expected),
                Commands.GetCurrentLnaGain,
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
        public void TestGetDccFreq(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.DccFreq; },
                (v) => { v.Should().Be(expected); },
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
        public void TestSetDccFreq(DccFreq expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.DccFreq = expected; },
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
        public void TestGetDccFreqAfc(DccFreq expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.DccFreqAfc; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDccFreqAfc,
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
                () => { _rfmUsb.DccFreqAfc = expected; },
                Commands.SetDccFreqAfc,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(DcFree.Manchester)]
        [InlineData(DcFree.None)]
        [InlineData(DcFree.Reserved)]
        [InlineData(DcFree.Whitening)]
        public void TestGetDcFree(DcFree expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.DcFree; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDcFree,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(DcFree.Manchester)]
        [InlineData(DcFree.None)]
        [InlineData(DcFree.Reserved)]
        [InlineData(DcFree.Whitening)]
        public void TestSetDcFree(DcFree expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.DcFree = expected; },
                Commands.SetDcFree,
                $"0x{expected:X}");
        }

        // DioInterruptMask
        public void TestDioInterruptMask()
        {
        }

        [Theory]
        [InlineData(EnterCondition.CrcOk)]
        [InlineData(EnterCondition.FifoEmpty)]
        [InlineData(EnterCondition.FifoLevel)]
        [InlineData(EnterCondition.FifoNotEmpty)]
        [InlineData(EnterCondition.Off)]
        [InlineData(EnterCondition.PacketSent)]
        [InlineData(EnterCondition.PayloadReady)]
        [InlineData(EnterCondition.SyncAddressMatch)]
        public void TestGetEnterCondition(EnterCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.EnterCondition; },
                (v) => { v.Should().Be(expected); },
                Commands.GetEnterCondition,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(EnterCondition.CrcOk)]
        [InlineData(EnterCondition.FifoEmpty)]
        [InlineData(EnterCondition.FifoLevel)]
        [InlineData(EnterCondition.FifoNotEmpty)]
        [InlineData(EnterCondition.Off)]
        [InlineData(EnterCondition.PacketSent)]
        [InlineData(EnterCondition.PayloadReady)]
        [InlineData(EnterCondition.SyncAddressMatch)]
        public void TestSetEnterCondition(EnterCondition expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.EnterCondition = expected; },
                Commands.SetEnterCondition,
                $"0x{expected:X}");
        }

        // ExitCondition
        [Theory]
        [InlineData(ExitCondition.CrcOk)]
        [InlineData(ExitCondition.FifoEmpty)]
        [InlineData(ExitCondition.FifoLevel)]
        [InlineData(ExitCondition.Off)]
        [InlineData(ExitCondition.PacketSent)]
        [InlineData(ExitCondition.PayloadReady)]
        [InlineData(ExitCondition.RxTimeout)]
        [InlineData(ExitCondition.SyncAddressMatch)]
        public void TestGetExitCondition(ExitCondition expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ExitCondition; },
                (v) => { v.Should().Be(expected); },
                Commands.GetExitCondition,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ExitCondition.CrcOk)]
        [InlineData(ExitCondition.FifoEmpty)]
        [InlineData(ExitCondition.FifoLevel)]
        [InlineData(ExitCondition.Off)]
        [InlineData(ExitCondition.PacketSent)]
        [InlineData(ExitCondition.PayloadReady)]
        [InlineData(ExitCondition.RxTimeout)]
        [InlineData(ExitCondition.SyncAddressMatch)]
        public void TestSetExitCondition(ExitCondition expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.ExitCondition = expected; },
                Commands.SetExitCondition,
                $"0x{expected:X}");
        }

        // Fei
        [Fact]
        public void TestGetFei()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Fei; },
                (v) => v.Should().Be(0x200),
                Commands.GetFei,
                "0x200");
        }

        [Fact]
        public void TestGetFifo()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Fifo; },
                (v) => v.Should().BeEquivalentTo(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }),
                Commands.GetFifo,
                "0xAA55DEAD");
        }

        [Fact]
        public void TestSetFifo()
        {
            ExecuteSetTest(
                () => { _rfmUsb.Fifo = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetFifo,
                "AA55DEAD");
        }

        [Fact]
        public void TestGetFifoFill()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.FifoFill; },
                (v) => v.Should().BeTrue(),
                Commands.GetFifoFill,
                "1");
        }

        [Fact]
        public void TestSetFifoFill()
        {
            ExecuteSetTest(
                () => { _rfmUsb.FifoFill = true; },
                Commands.SetFifoFill,
                "1");
        }

        [Fact]
        public void TestGetFifoThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.FifoThreshold; },
                (v) => v.Should().Be(0x10),
                Commands.GetFifoThreshold,
                "0x10");
        }

        [Fact]
        public void TestSetFifoThreshold()
        {
            ExecuteSetTest(
                () => { _rfmUsb.FifoThreshold = 0x10; },
                Commands.SetFifoThreshold,
                "0x10");
        }

        [Fact]
        public void TestGetFrequency()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Frequency; },
                (v) => v.Should().Be(0x100000),
                Commands.GetFrequency,
                "0x100000");
        }

        [Fact]
        public void TestSetFrequency()
        {
            ExecuteSetTest(
                () => { _rfmUsb.Frequency = 0x100000; },
                Commands.SetFrequency,
                "0x100000");
        }

        [Fact]
        public void TestGetFrequencyDeviation()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.FrequencyDeviation; },
                (v) => v.Should().Be(0xA000),
                Commands.GetFrequencyDeviation,
                "0xA000");
        }

        [Fact]
        public void TestSetFrequencyDeviation()
        {
            ExecuteSetTest(
                () => { _rfmUsb.FrequencyDeviation = 0xA000; },
                Commands.SetFrequencyDeviation,
                "0xA000");
        }

        [Theory]
        [InlineData(FskModulationShaping.GaussianBt0_3)]
        [InlineData(FskModulationShaping.GaussianBt0_5)]
        [InlineData(FskModulationShaping.GaussianBt1_0)]
        public void TestGetFskModulationShaping(FskModulationShaping expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.FskModulationShaping; },
                (v) => v.Should().Be(expected),
                Commands.GetFskModulationShaping,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(FskModulationShaping.GaussianBt0_3)]
        [InlineData(FskModulationShaping.GaussianBt0_5)]
        [InlineData(FskModulationShaping.GaussianBt1_0)]
        public void TestSetFskModulationShaping(FskModulationShaping expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.FskModulationShaping = expected; },
                Commands.SetFskModulationShaping,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetImpedance()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Impedance; },
                (v) => v.Should().BeTrue(),
                Commands.GetImpedance,
                "1");
        }

        [Fact]
        public void TestSetImpedance()
        {
            ExecuteSetTest(
                () => { _rfmUsb.Impedance = true; },
                Commands.SetImpedance,
                "1");
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestGetIntermediateMode(IntermediateMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.IntermediateMode; },
                (v) => v.Should().Be(expected),
                Commands.GetIntermediateMode,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestSetIntermediateMode(IntermediateMode expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.IntermediateMode = expected; },
                Commands.SetIntermediateMode,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetInterPacketRxDelay()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.InterPacketRxDelay; },
                (v) => v.Should().Be(0xAA),
                Commands.GetInterPacketRxDelay,
                "0xAA");
        }

        [Fact]
        public void TestSetInterPacketRxDelay()
        {
            ExecuteSetTest(
                () => { _rfmUsb.InterPacketRxDelay = 0x60; },
                Commands.SetInterPacketRxDelay,
                "0x60");
        }

        // Irq

        [Fact]
        public void TestGetListenCoefficentIdle()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ListenCoefficentIdle; },
                (v) => v.Should().Be(0xAA),
                Commands.GetListenCoefficentIdle,
                "0xAA");
        }

        [Fact]
        public void TestSetListenCoefficentIdle()
        {
            ExecuteSetTest(
                () => { _rfmUsb.ListenCoefficentIdle = 0x60; },
                Commands.SetListenCoefficentIdle,
                "0x60");
        }

        [Fact]
        public void TestGetListenCoefficentRx()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ListenCoefficentRx; },
                (v) => v.Should().Be(0xAA),
                Commands.GetListenCoefficentRx,
                "0xAA");
        }

        [Fact]
        public void TestSetListenCoefficentRx()
        {
            ExecuteSetTest(
                () => { _rfmUsb.ListenCoefficentRx = 0x60; },
                Commands.SetListenCoefficentRx,
                "0x60");
        }

        [Fact]
        public void TestGetListenCriteria()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ListenCriteria; },
                (v) => v.Should().BeTrue(),
                Commands.GetListenCriteria,
                "1");
        }

        [Fact]
        public void TestSetListenCriteria()
        {
            ExecuteSetTest(
                () => { _rfmUsb.ListenCriteria = true; },
                Commands.SetListenCriteria,
                "1");
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
                () => { return _rfmUsb.ListenEnd; },
                (v) => v.Should().Be(expected),
                Commands.GetListenEnd,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ListenEnd.Idle)]
        [InlineData(ListenEnd.Mode)]
        [InlineData(ListenEnd.Reserved)]
        [InlineData(ListenEnd.Rx)]
        public void TestSetListenEnd(ListenEnd expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.ListenEnd = expected; },
                Commands.SetListenEnd,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetListenerOn()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ListenerOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetListenerOn,
                "1");
        }

        [Fact]
        public void TestSetListenerOn()
        {
            ExecuteSetTest(
                () => { _rfmUsb.ListenerOn = true; },
                Commands.SetListenerOn,
                "1");
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Reserved)]
        public void TestGetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.ListenResolutionIdle; },
                (v) => v.Should().Be(expected),
                Commands.GetListenResolutionIdle,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Reserved)]
        public void TestSetListenResolutionIdle(ListenResolution expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.ListenResolutionIdle = expected; },
                Commands.SetListenResolutionIdle,
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
                () => { return _rfmUsb.ListenResolutionRx; },
                (v) => v.Should().Be(expected),
                Commands.GetListenResolutionRx,
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
                () => { _rfmUsb.ListenResolutionRx = expected; },
                Commands.SetListenResolutionRx,
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
        public void TestGetLnaGainSelect(LnaGain expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.LnaGainSelect; },
                (v) => v.Should().Be(expected),
                Commands.GetLnaGainSelect,
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
        public void TestSetLnaGainSelect(LnaGain expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.LnaGainSelect = expected; },
                Commands.SetLnaGainSelect,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetLowBetaAfcOffset()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.LowBetaAfcOffset; },
                (v) => v.Should().Be(0xAA),
                Commands.GetLowBetaAfcOffset,
                "0xAA");
        }

        [Fact]
        public void TestSetLowBetaAfcOffset()
        {
            ExecuteSetTest(
                () => { _rfmUsb.LowBetaAfcOffset = 0x60; },
                Commands.SetLowBetaAfcOffset,
                "0x60");
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestGetMode(Mode expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Mode; },
                (v) => v.Should().Be(expected),
                Commands.GetMode,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestSetMode(Mode expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.Mode = expected; },
                Commands.SetMode,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(Modulation.Fsk)]
        [InlineData(Modulation.Ook)]
        public void TestGetModulation(Modulation expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Modulation; },
                (v) => v.Should().Be(expected),
                Commands.GetModulation,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(Modulation.Fsk)]
        [InlineData(Modulation.Ook)]
        public void TestSetModulation(Modulation expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.Modulation = expected; },
                Commands.SetModulation,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetNodeAddress()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.NodeAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetNodeAddress,
                "0xAA");
        }

        [Fact]
        public void TestSetNodeAddress()
        {
            ExecuteSetTest(
                () => { _rfmUsb.NodeAddress = 0x60; },
                Commands.SetNodeAddress,
                "0x60");
        }

        [Fact]
        public void TestGetOcpEnable()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OcpEnable; },
                (v) => v.Should().BeTrue(),
                Commands.GetOcpEnable,
                "1");
        }

        [Fact]
        public void TestSetOcpEnable()
        {
            ExecuteSetTest(
                () => { _rfmUsb.OcpEnable = true; },
                Commands.SetOcpEnable,
                "1");
        }

        [Theory]
        [InlineData(OcpTrim.OcpTrim100)]
        [InlineData(OcpTrim.OcpTrim105)]
        [InlineData(OcpTrim.OcpTrim110)]
        [InlineData(OcpTrim.OcpTrim115)]
        [InlineData(OcpTrim.OcpTrim120)]
        [InlineData(OcpTrim.OcpTrim45)]
        [InlineData(OcpTrim.OcpTrim50)]
        [InlineData(OcpTrim.OcpTrim55)]
        [InlineData(OcpTrim.OcpTrim60)]
        [InlineData(OcpTrim.OcpTrim65)]
        [InlineData(OcpTrim.OcpTrim70)]
        [InlineData(OcpTrim.OcpTrim75)]
        [InlineData(OcpTrim.OcpTrim80)]
        [InlineData(OcpTrim.OcpTrim85)]
        [InlineData(OcpTrim.OcpTrim90)]
        [InlineData(OcpTrim.OcpTrim95)]
        public void TestGetOcpTrim(OcpTrim expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OcpTrim; },
                (v) => v.Should().Be(expected),
                Commands.GetOcpTrim,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OcpTrim.OcpTrim100)]
        [InlineData(OcpTrim.OcpTrim105)]
        [InlineData(OcpTrim.OcpTrim110)]
        [InlineData(OcpTrim.OcpTrim115)]
        [InlineData(OcpTrim.OcpTrim120)]
        [InlineData(OcpTrim.OcpTrim45)]
        [InlineData(OcpTrim.OcpTrim50)]
        [InlineData(OcpTrim.OcpTrim55)]
        [InlineData(OcpTrim.OcpTrim60)]
        [InlineData(OcpTrim.OcpTrim65)]
        [InlineData(OcpTrim.OcpTrim70)]
        [InlineData(OcpTrim.OcpTrim75)]
        [InlineData(OcpTrim.OcpTrim80)]
        [InlineData(OcpTrim.OcpTrim85)]
        [InlineData(OcpTrim.OcpTrim90)]
        [InlineData(OcpTrim.OcpTrim95)]
        public void TestSetOcpTrim(OcpTrim expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.OcpTrim = expected; },
                Commands.SetOcpTrim,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokAverageThresholdFilter.ChipRate2)]
        [InlineData(OokAverageThresholdFilter.ChipRate32)]
        [InlineData(OokAverageThresholdFilter.ChipRate4)]
        [InlineData(OokAverageThresholdFilter.ChipRate8)]
        public void TestGetOokAverageThresholdFilter(OokAverageThresholdFilter expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OokAverageThresholdFilter; },
                (v) => v.Should().Be(expected),
                Commands.GetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokAverageThresholdFilter.ChipRate2)]
        [InlineData(OokAverageThresholdFilter.ChipRate32)]
        [InlineData(OokAverageThresholdFilter.ChipRate4)]
        [InlineData(OokAverageThresholdFilter.ChipRate8)]
        public void TestSetOokAverageThresholdFilter(OokAverageThresholdFilter expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.OokAverageThresholdFilter = expected; },
                Commands.SetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetOokFixedThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OokFixedThreshold; },
                (v) => v.Should().Be(0xAA),
                Commands.GetOokFixedThreshold,
                "0xAA");
        }

        [Fact]
        public void TestSetOokFixedThreshold()
        {
            ExecuteSetTest(
                () => { _rfmUsb.OokFixedThreshold = 0x60; },
                Commands.SetOokFixedThreshold,
                "0x60");
        }

        [Theory]
        [InlineData(OokModulationShaping.Filtering2Br)]
        [InlineData(OokModulationShaping.FilteringBr)]
        [InlineData(OokModulationShaping.None)]
        [InlineData(OokModulationShaping.Reserved)]
        public void TestGetOokModulationShaping(OokModulationShaping expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OokModulationShaping; },
                (v) => v.Should().Be(expected),
                Commands.GetOokModulationShaping,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokModulationShaping.Filtering2Br)]
        [InlineData(OokModulationShaping.FilteringBr)]
        [InlineData(OokModulationShaping.None)]
        [InlineData(OokModulationShaping.Reserved)]
        public void TestSetOokModulationShaping(OokModulationShaping expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.OokModulationShaping = expected; },
                Commands.SetOokModulationShaping,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokThresholdDec.EightTimesInEachChip)]
        [InlineData(OokThresholdDec.FourTimesInEachChip)]
        [InlineData(OokThresholdDec.OnceEvery2Chips)]
        [InlineData(OokThresholdDec.OnceEvery4Chips)]
        [InlineData(OokThresholdDec.OnceEvery8Chips)]
        [InlineData(OokThresholdDec.OncePerChip)]
        [InlineData(OokThresholdDec.SixteeenTimesInEachChip)]
        [InlineData(OokThresholdDec.TwiceInEachChip)]
        public void TestGetOokPeakThresholdDec(OokThresholdDec expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OokPeakThresholdDec; },
                (v) => v.Should().Be(expected),
                Commands.GetOokPeakThresholdDec,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokThresholdDec.EightTimesInEachChip)]
        [InlineData(OokThresholdDec.FourTimesInEachChip)]
        [InlineData(OokThresholdDec.OnceEvery2Chips)]
        [InlineData(OokThresholdDec.OnceEvery4Chips)]
        [InlineData(OokThresholdDec.OnceEvery8Chips)]
        [InlineData(OokThresholdDec.OncePerChip)]
        [InlineData(OokThresholdDec.SixteeenTimesInEachChip)]
        [InlineData(OokThresholdDec.TwiceInEachChip)]
        public void TestSetOokPeakThresholdDec(OokThresholdDec expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.OokPeakThresholdDec = expected; },
                Commands.SetOokPeakThresholdDec,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokThresholdStep.Step0_5db)]
        [InlineData(OokThresholdStep.Step1db)]
        [InlineData(OokThresholdStep.Step1_5db)]
        [InlineData(OokThresholdStep.Step2db)]
        [InlineData(OokThresholdStep.Step3db)]
        [InlineData(OokThresholdStep.Step4db)]
        [InlineData(OokThresholdStep.Step5db)]
        [InlineData(OokThresholdStep.Step6db)]
        public void TestGetOokPeakThresholdStep(OokThresholdStep expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OokPeakThresholdStep; },
                (v) => v.Should().Be(expected),
                Commands.GetOokPeakThresholdStep,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokThresholdStep.Step0_5db)]
        [InlineData(OokThresholdStep.Step1db)]
        [InlineData(OokThresholdStep.Step1_5db)]
        [InlineData(OokThresholdStep.Step2db)]
        [InlineData(OokThresholdStep.Step3db)]
        [InlineData(OokThresholdStep.Step4db)]
        [InlineData(OokThresholdStep.Step5db)]
        [InlineData(OokThresholdStep.Step6db)]
        public void TestSetOokPeakThresholdStep(OokThresholdStep expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.OokPeakThresholdStep = expected; },
                Commands.SetOokPeakThresholdStep,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokThresholdType.Average)]
        [InlineData(OokThresholdType.Fixed)]
        [InlineData(OokThresholdType.Peak)]
        [InlineData(OokThresholdType.Reserved)]
        public void TestGetOokThresholdType(OokThresholdType expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OokThresholdType; },
                (v) => v.Should().Be(expected),
                Commands.GetOokThresholdType,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokThresholdType.Average)]
        [InlineData(OokThresholdType.Fixed)]
        [InlineData(OokThresholdType.Peak)]
        [InlineData(OokThresholdType.Reserved)]
        public void TestSetOokThresholdType(OokThresholdType expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.OokThresholdType = expected; },
                Commands.SetOokThresholdType,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetOutputPower()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.OutputPower; },
                (v) => v.Should().Be(0x60000),
                Commands.GetOutputPower,
                "0x60000");
        }

        [Fact]
        public void TestSetOutputPower()
        {
            ExecuteSetTest(
                () => { _rfmUsb.OutputPower = 0x60000; },
                Commands.SetOutputPower,
                "0x60000");
        }

        [Fact]
        public void TestGetPacketFormat()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.PacketFormat; },
                (v) => v.Should().BeTrue(),
                Commands.GetPacketFormat,
                "1");
        }

        [Fact]
        public void TestSetPacketFormat()
        {
            ExecuteSetTest(
                () => { _rfmUsb.PacketFormat = true; },
                Commands.SetPacketFormat,
                "1");
        }

        [Theory]
        [InlineData(PaRamp.PowerAmpRamp10)]
        [InlineData(PaRamp.PowerAmpRamp100)]
        [InlineData(PaRamp.PowerAmpRamp1000)]
        [InlineData(PaRamp.PowerAmpRamp12)]
        [InlineData(PaRamp.PowerAmpRamp125)]
        [InlineData(PaRamp.PowerAmpRamp15)]
        [InlineData(PaRamp.PowerAmpRamp20)]
        [InlineData(PaRamp.PowerAmpRamp2000)]
        [InlineData(PaRamp.PowerAmpRamp25)]
        [InlineData(PaRamp.PowerAmpRamp250)]
        [InlineData(PaRamp.PowerAmpRamp31)]
        [InlineData(PaRamp.PowerAmpRamp3400)]
        [InlineData(PaRamp.PowerAmpRamp40)]
        [InlineData(PaRamp.PowerAmpRamp50)]
        [InlineData(PaRamp.PowerAmpRamp500)]
        [InlineData(PaRamp.PowerAmpRamp62)]
        public void TestGetPaRamp(PaRamp expected)
        {
            ExecuteGetTest(
                () => { return _rfmUsb.PaRamp; },
                (v) => v.Should().Be(expected),
                Commands.GetPaRamp,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(PaRamp.PowerAmpRamp10)]
        [InlineData(PaRamp.PowerAmpRamp100)]
        [InlineData(PaRamp.PowerAmpRamp1000)]
        [InlineData(PaRamp.PowerAmpRamp12)]
        [InlineData(PaRamp.PowerAmpRamp125)]
        [InlineData(PaRamp.PowerAmpRamp15)]
        [InlineData(PaRamp.PowerAmpRamp20)]
        [InlineData(PaRamp.PowerAmpRamp2000)]
        [InlineData(PaRamp.PowerAmpRamp25)]
        [InlineData(PaRamp.PowerAmpRamp250)]
        [InlineData(PaRamp.PowerAmpRamp31)]
        [InlineData(PaRamp.PowerAmpRamp3400)]
        [InlineData(PaRamp.PowerAmpRamp40)]
        [InlineData(PaRamp.PowerAmpRamp50)]
        [InlineData(PaRamp.PowerAmpRamp500)]
        [InlineData(PaRamp.PowerAmpRamp62)]
        public void TestSetPaRamp(PaRamp expected)
        {
            ExecuteSetTest(
                () => { _rfmUsb.PaRamp = expected; },
                Commands.SetPaRamp,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetPayloadLength()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.PayloadLength; },
                (v) => v.Should().Be(0x60),
                Commands.GetPayloadLength,
                "0x60");
        }

        [Fact]
        public void TestSetPayloadLength()
        {
            ExecuteSetTest(
                () => { _rfmUsb.PayloadLength = 0x60; },
                Commands.SetPayloadLength,
                "0x60");
        }

        [Fact]
        public void TestGetPreambleSize()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.PreambleSize; },
                (v) => v.Should().Be(0x6000),
                Commands.GetPreambleSize,
                "0x6000");
        }

        [Fact]
        public void TestSetPreambleSize()
        {
            ExecuteSetTest(
                () => { _rfmUsb.PreambleSize = 0x6000; },
                Commands.SetPreambleSize,
                "0x6000");
        }

        [Fact]
        public void TestGetRadioConfig()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.RadioConfig; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRadioConfig,
                "0xA0");
        }

        [Fact]
        public void TestSetRadioConfig()
        {
            ExecuteSetTest(
                () => { _rfmUsb.RadioConfig = 0xB0; },
                Commands.SetRadioConfig,
                "0xB0");
        }

        [Fact]
        public void TestGetRssi()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Rssi; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRssi,
                "0xA0");
        }

        [Fact]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.RssiThreshold; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRssiThreshold,
                "0xA0");
        }

        [Fact]
        public void TestSetRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfmUsb.RssiThreshold = 0xB0; },
                Commands.SetRssiThreshold,
                "0xB0");
        }

        [Fact]
        public void TestGetRxBw()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.RxBw; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRxBw,
                "0xA0");
        }

        [Fact]
        public void TestSetRxBw()
        {
            ExecuteSetTest(
                () => { _rfmUsb.RxBw = 0xB0; },
                Commands.SetRxBw,
                "0xB0");
        }

        [Fact]
        public void TestGetRxBwAfc()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.RxBwAfc; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRxBwAfc,
                "0xA0");
        }

        [Fact]
        public void TestSetRxBwAfc()
        {
            ExecuteSetTest(
                () => { _rfmUsb.RxBwAfc = 0xB0; },
                Commands.SetRxBwAfc,
                "0xB0");
        }

        [Fact]
        public void TestGetSensitivityBoost()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.SensitivityBoost; },
                (v) => v.Should().BeTrue(),
                Commands.GetSensitivityBoost,
                "1");
        }

        [Fact]
        public void TestSetSensitivityBoost()
        {
            ExecuteSetTest(
                () => { _rfmUsb.SensitivityBoost = true; },
                Commands.SetSensitivityBoost,
                "1");
        }

        [Fact]
        public void TestGetSequencer()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Sequencer; },
                (v) => v.Should().BeTrue(),
                Commands.GetSequencer,
                "1");
        }

        [Fact]
        public void TestSetSequencer()
        {
            ExecuteSetTest(
                () => { _rfmUsb.Sequencer = true; },
                Commands.SetSequencer,
                "1");
        }

        // Sync
        [Fact]
        public void TestGetSync()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Sync; },
                (v) => v.Should().BeEquivalentTo(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }),
                Commands.GetSync,
                "0xAA55DEAD");
        }

        [Fact]
        public void TestSetSync()
        {
            ExecuteSetTest(
                () => { _rfmUsb.Sync = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetSync,
                "AA55DEAD");
        }

        [Fact]
        public void TestGetSyncBitErrors()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.SyncBitErrors; },
                (v) => v.Should().Be(0xA0),
                Commands.GetSyncBitErrors,
                "0xA0");
        }

        [Fact]
        public void TestSetSyncBitErrors()
        {
            ExecuteSetTest(
                () => { _rfmUsb.SyncBitErrors = 0xB0; },
                Commands.SetSyncBitErrors,
                "0xB0");
        }

        [Fact]
        public void TestGetSyncEnable()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.SyncEnable; },
                (v) => v.Should().BeTrue(),
                Commands.GetSyncEnable,
                "1");
        }

        [Fact]
        public void TestSetSyncEnable()
        {
            ExecuteSetTest(
                () => { _rfmUsb.SyncEnable = true; },
                Commands.SetSyncEnable,
                "1");
        }

        [Fact]
        public void TestGetSyncSize()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.SyncSize; },
                (v) => v.Should().Be(0xA0),
                Commands.GetSyncSize,
                "0xA0");
        }

        [Fact]
        public void TestSetSyncSize()
        {
            ExecuteSetTest(
                () => { _rfmUsb.SyncSize = 0xB0; },
                Commands.SetSyncSize,
                "0xB0");
        }

        [Fact]
        public void TestGetTemperatureValue()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.TemperatureValue; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTemperatureValue,
                "0xA0");
        }

        [Fact]
        public void TestGetTimeout()
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadTimeout)
                .Returns(1000);

            // Act
            var result = _rfmUsb.Timeout;

            // Assert
            result.Should().Be(1000);
        }

        [Fact]
        public void TestSetTimeout()
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            // Act
            _rfmUsb.Timeout = 1000;

            // Assert
            _mockSerialPort.VerifySet(_ => _.ReadTimeout = 1000, Times.Once);
        }

        [Fact]
        public void TestGetTimeoutRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.TimeoutRssiThreshold; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTimeoutRssiThreshold,
                "0xA0");
        }

        [Fact]
        public void TestSetTimeoutRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfmUsb.TimeoutRssiThreshold = 0xB0; },
                Commands.SetTimeoutRssiThreshold,
                "0xB0");
        }

        [Fact]
        public void TestGetTimeoutRxStart()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.TimeoutRxStart; },
                (v) => v.Should().Be(0xA0),
                Commands.GetTimeoutRxStart,
                "0xA0");
        }

        [Fact]
        public void TestSetTimeoutRxStart()
        {
            ExecuteSetTest(
                () => { _rfmUsb.TimeoutRxStart = 0xB0; },
                Commands.SetTimeoutRxStart,
                "0xB0");
        }

        [Fact]
        public void TestGetTxStartCondition()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.TxStartCondition; },
                (v) => v.Should().BeTrue(),
                Commands.GetTxStartCondition,
                "1");
        }

        [Fact]
        public void TestSetTxStartCondition()
        {
            ExecuteSetTest(
                () => { _rfmUsb.TxStartCondition = true; },
                Commands.SetTxStartCondition,
                "1");
        }

        [Fact]
        public void TestGetVersion()
        {
            ExecuteGetTest(
                () => { return _rfmUsb.Version; },
                (v) => v.Should().Be("1.2"),
                Commands.GetVersion,
                "1.2");
        }

        [Fact]
        public void TestAfcClear()
        {
            ExecuteTest(
                () => { _rfmUsb.AfcClear(); },
                Commands.ExecuteAfcClear);
        }

        [Fact]
        public void TestAfcStart()
        {
            ExecuteTest(
                () => { _rfmUsb.AfcStart(); },
                Commands.ExecuteAfcStart);
        }
        
        [Fact]
        public void TestFeiStart()
        {
            ExecuteTest(
                () => { _rfmUsb.FeiStart(); },
                Commands.ExecuteFeiStart);
        }

        [Fact]
        public void TestGetDioMapping()
        {
        }

        [Fact]
        public void TestGetRadioConfigurations()
        {
        }

        [Fact]
        public void TestListenAbort()
        {
            ExecuteTest(
                () => { _rfmUsb.ListenAbort(); },
                Commands.ExecuteListenAbort);
        }

        [Fact]
        public void TestMeasureTemperature()
        {
            ExecuteTest(
                () => { _rfmUsb.MeasureTemperature(); },
                Commands.ExecuteMeasureTemperature);
        }

        [Fact]
        public void TestRcCalibration()
        {
            ExecuteTest(
                () => { _rfmUsb.RcCalibration(); },
                Commands.ExecuteRcCalibration);
        }

        [Fact]
        public void TestReset()
        {
            ExecuteTest(
                () => { _rfmUsb.Reset(); },
                Commands.ExecuteReset);
        }

        [Fact]
        public void TestRestartRx()
        {
            ExecuteTest(
                () => { _rfmUsb.RestartRx(); },
                Commands.ExecuteRestartRx);
        }

        [Fact]
        public void TestSetAesKey()
        {
            ExecuteTest(
                () => { _rfmUsb.SetAesKey(new List<byte>() { 0xFF, 0xAA, 0xBB }); },
                $"{Commands.ExecuteSetAesKey} FFAABB");
        }

        [Fact]
        public void TestSetDioMapping()
        {
        }

        [Fact]
        public void TestStartRssi()
        {
            ExecuteTest(
                () => { _rfmUsb.StartRssi(); },
                Commands.ExecuteStartRssi);
        }

        [Fact]
        public void TestTransmit()
        {
            ExecuteTest(
                () => { _rfmUsb.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }); },
                $"{Commands.ExecuteTransmit} AADDFFCC");
        }

        [Fact]
        public void TestTransmitWithTimeout()
        {
            ExecuteTest(
                () => { _rfmUsb.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200); },
                $"{Commands.ExecuteTransmit} AADDFFCC 200");
        }

        [Fact]
        public void TestTransmitReceive()
        {
            //ExecuteTest(
            //    () => { _rfmUsb.TransmitReceive(); },
            //    Commands.ExecuteStartRssi);
        }

        [Fact]
        public void TestTransmitReceiveTimeout()
        {
            //ExecuteTest(
            //    () => { _rfmUsb.TransmitReceive(); },
            //    Commands.ExecuteStartRssi);
        }

        [Fact]
        public void TestWaitForIrq()
        {
        }

        private void ExecuteTest(Action action, string command)
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmUsb.ResponseOk);

            // Act
            action();

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{command}\n"), Times.Once);
        }

        private void ExecuteSetTest(Action action, string command, string value)
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmUsb.ResponseOk);

            // Act
            action();

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{command} {value}\n"), Times.Once);
        }

        private void ExecuteGetTest<T>(Func<T> action, Action<T> validation, string command, string value)
        {
            // Arrange
            _rfmUsb._serialPort = _mockSerialPort.Object;

            _mockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(value);

            // Act
            var result = action();

            // Assert
            _mockSerialPort.Verify(_ => _.Write($"{command}\n"));

            validation(result);
        }
    }
}