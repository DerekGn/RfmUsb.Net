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


// Ignore Spelling: Rx Bw Dio Fei Fsk Io Lna Ocp Ook Rc Rfm Rssi Tx

using Microsoft.Extensions.Logging;
using Moq;
using RfmUsb.Net.Exceptions;
using RfmUsb.Net.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using Xunit;

namespace RfmUsb.Net.UnitTests
{
    public abstract class RfmBaseTests
    {
        protected readonly ILogger<IRfm> MockLogger;
        protected readonly Mock<ISerialPort> MockSerialPort;
        protected readonly Mock<ISerialPortFactory> MockSerialPortFactory;
        protected RfmBase RfmBase;

        protected RfmBaseTests()
        {
            MockSerialPortFactory = new Mock<ISerialPortFactory>();

            MockSerialPort = new Mock<ISerialPort>();

            MockLogger = Mock.Of<ILogger<IRfm>>();
        }

        [Fact]
        public void TestClose()
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            // Act
            RfmBase.Close();

            // Assert
            MockSerialPort.Verify(_ => _.Close());
        }

        [Fact]
        public void TestExecuteBootloader()
        {
            ExecuteTest(
                () => { RfmBase.EnterBootloader(); },
                Commands.ExecuteBootloader,
                "Ok");
        }

        [Fact]
        public void TestGetAddressFilter()
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(AddressFilter.NodeBroaddcast.ToString("X"));

            // Act
            var addressFilter = RfmBase.AddressFiltering;

            // Assert
            Assert.Equal(AddressFilter.NodeBroaddcast, addressFilter);
        }

        [Fact]
        public void TestGetAfc()
        {
            ExecuteGetTest(
                () => { return RfmBase.Afc; },
                (v) => Assert.Equal(0x100, v),
                Commands.GetAfc,
                "0x100");
        }

        [Fact]
        public void TestGetAfcAutoClear()
        {
            ExecuteGetTest(
                () => { return RfmBase.AfcAutoClear; },
                Assert.True,
                Commands.GetAfcAutoClear,
                "1");
        }

        [Fact]
        public void TestGetAfcAutoOn()
        {
            ExecuteGetTest(
                () => { return RfmBase.AfcAutoOn; },
                Assert.True,
                Commands.GetAfcAutoOn,
                "1");
        }

        [Fact]
        public void TestGetBitRate()
        {
            ExecuteGetTest(
                () => { return RfmBase.BitRate; },
                (v) => Assert.Equal<uint>(0x100, v),
                Commands.GetBitRate,
                "0x100");
        }

        [Fact]
        public void TestGetBroadcastAddress()
        {
            ExecuteGetTest(
                () => { return RfmBase.BroadcastAddress; },
                (v) => Assert.Equal(0x55, v),
                Commands.GetBroadcastAddress,
                "0x55");
        }

        [Fact]
        public void TestGetBufferEnable()
        {
            ExecuteGetTest(
                () => { return RfmBase.BufferedIoEnable; },
                Assert.True,
                Commands.GetBufferEnable,
                "1");
        }

        [Fact]
        public void TestGetBufferIoInfo()
        {
            ExecuteGetTest(
                () => { return RfmBase.BufferedIoInfo; },
                (v) =>
                {
                    Assert.Equal(160, v.Capacity);
                    Assert.Equal(16, v.Count);
                },
                Commands.GetIoBufferInfo,
                new List<string>()
                {
                    "CAPACITY:0xA0",
                    "COUNT:0x10"
                });
        }

        [Fact]
        public void TestGetCrcAutoClear()
        {
            ExecuteGetTest(
                () => { return RfmBase.CrcAutoClearOff; },
                Assert.True,
                Commands.GetCrcAutoClearOff,
                "1");
        }

        [Fact]
        public void TestGetCrcOn()
        {
            ExecuteGetTest(
                () => { return RfmBase.CrcOn; },
                Assert.True,
                Commands.GetCrcOn,
                "1");
        }

        [Theory]
        [InlineData(DcFreeEncoding.Manchester)]
        [InlineData(DcFreeEncoding.None)]
        [InlineData(DcFreeEncoding.Reserved)]
        [InlineData(DcFreeEncoding.Whitening)]
        public void TestGetDcFreeEncoding(DcFreeEncoding expected)
        {
            ExecuteGetTest(
                () => { return RfmBase.DcFreeEncoding; },
                (v) => Assert.Equal(expected, v),
                Commands.GetDcFree,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetDioInterruptMask()
        {
            // Arrange
            MockSerialPort
               .Setup(_ => _.IsOpen)
               .Returns(true);

            MockSerialPort
               .Setup(_ => _.Write($"{Commands.GetDioInterruptMask}\n"))
               .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
               .SetupSequence(_ => _.ReadLine())
               .Returns("1-DIO0")
               .Returns("0-DIO1")
               .Returns("1-DIO2")
               .Returns("0-DIO3")
               .Returns("1-DIO4")
               .Returns("0-DIO5");

            MockSerialPort
                .SetupSequence(_ => _.BytesToRead)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = RfmBase.DioInterruptMask;

            // Assert
            Assert.Equal(DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4, result);

            MockSerialPort.Verify(_ => _.Write($"{Commands.GetDioInterruptMask}\n"), Times.Once);
        }

        [Theory]
        [InlineData(Dio.Dio0)]
        [InlineData(Dio.Dio1)]
        [InlineData(Dio.Dio2)]
        [InlineData(Dio.Dio3)]
        [InlineData(Dio.Dio4)]
        [InlineData(Dio.Dio5)]
        public void TestGetDioMapping(Dio dio)
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns("0x0000-Map 00");

            // Act
            var result = RfmBase.GetDioMapping(dio);

            // Assert
            Assert.Equal(DioMapping.DioMapping0, result);

            MockSerialPort.Verify(_ => _.Write($"{Commands.GetDioMapping} 0x{(byte)dio:X}\n"));
        }

        [Fact]
        public void TestGetFei()
        {
            ExecuteGetTest(
                () => { return RfmBase.Fei; },
                (v) => Assert.Equal(0x200, v),
                Commands.GetFei,
                "0x200");
        }

        [Fact]
        public void TestGetFifo()
        {
            ExecuteGetTest(
                () => { return RfmBase.Fifo; },
                (v) => Assert.Equal(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }, v),
                Commands.GetFifo,
                "0xAA55DEAD");
        }

        [Fact]
        public void TestGetFifoThreshold()
        {
            ExecuteGetTest(
                () => { return RfmBase.FifoThreshold; },
                (v) => Assert.Equal(0x10, v),
                Commands.GetFifoThreshold,
                "0x10");
        }

        [Fact]
        public void TestGetFirmwareVersion()
        {
            ExecuteGetTest(
                () => { return RfmBase.FirmwareVersion; },
                (v) => Assert.Equal("RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz",v),
                Commands.GetFirmwareVersion,
                "RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz");
        }

        [Fact]
        public void TestGetFrequency()
        {
            ExecuteGetTest(
                () => { return RfmBase.Frequency; },
                (v) => Assert.Equal<uint>(0x100000, v),
                Commands.GetFrequency,
                "0x100000");
        }

        [Fact]
        public void TestGetFrequencyDeviation()
        {
            ExecuteGetTest(
                () => { return RfmBase.FrequencyDeviation; },
                (v) => Assert.Equal(0xA000, v),
                Commands.GetFrequencyDeviation,
                "0xA000");
        }

        [Theory]
        [InlineData(FskModulationShaping.GaussianBt0_3)]
        [InlineData(FskModulationShaping.GaussianBt0_5)]
        [InlineData(FskModulationShaping.GaussianBt1_0)]
        public void TestGetFskModulationShaping(FskModulationShaping expected)
        {
            ExecuteGetTest(
                () => { return RfmBase.FskModulationShaping; },
                (v) => Assert.Equal(expected, v),
                Commands.GetFskModulationShaping,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetInterPacketRxDelay()
        {
            ExecuteGetTest(
                () => { return RfmBase.InterPacketRxDelay; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetInterPacketRxDelay,
                "0xAA");
        }

        [Fact]
        public void TestGetLastRssi()
        {
            ExecuteGetTest(
                () => { return RfmBase.LastRssi; },
                (v) => Assert.Equal(-18, v),
                Commands.GetLastRssi,
                "0xFFFFFFEE");
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
                () => { return RfmBase.LnaGainSelect; },
                (v) => Assert.Equal(expected, v),
                Commands.GetLnaGainSelect,
                $"0x{expected:X}");
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
                () => { return RfmBase.Mode; },
                (v) => Assert.Equal(expected, v),
                Commands.GetMode,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(ModulationType.Fsk)]
        [InlineData(ModulationType.Ook)]
        public void TestGetModulation(ModulationType expected)
        {
            ExecuteGetTest(
                () => { return RfmBase.ModulationType; },
                (v) => Assert.Equal(expected, v),
                Commands.GetModulationType,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetNodeAddress()
        {
            ExecuteGetTest(
                () => { return RfmBase.NodeAddress; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetNodeAddress,
                "0xAA");
        }

        [Fact]
        public void TestGetOcpEnable()
        {
            ExecuteGetTest(
                () => { return RfmBase.OcpEnable; },
                Assert.True,
                Commands.GetOcpEnable,
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
                () => { return RfmBase.OcpTrim; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOcpTrim,
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
                () => { return RfmBase.OokAverageThresholdFilter; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetOokFixedThreshold()
        {
            ExecuteGetTest(
                () => { return RfmBase.OokFixedThreshold; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetOokFixedThreshold,
                "0xAA");
        }

        [Theory]
        [InlineData(OokModulationShaping.Filtering2Br)]
        [InlineData(OokModulationShaping.FilteringBr)]
        [InlineData(OokModulationShaping.None)]
        [InlineData(OokModulationShaping.Reserved)]
        public void TestGetOokModulationShaping(OokModulationShaping expected)
        {
            ExecuteGetTest(
                () => { return RfmBase.OokModulationShaping; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOokModulationShaping,
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
                () => { return RfmBase.OokPeakThresholdDec; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOokPeakThresholdDec,
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
                () => { return RfmBase.OokPeakThresholdStep; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOokPeakThresholdStep,
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
                () => { return RfmBase.OokThresholdType; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOokThresholdType,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetPacketFormat()
        {
            ExecuteGetTest(
                () => { return RfmBase.PacketFormat; },
                Assert.True,
                Commands.GetPacketFormat,
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
                () => { return RfmBase.PaRamp; },
                (v) => Assert.Equal(expected, v),
                Commands.GetPaRamp,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetPayloadLength()
        {
            ExecuteGetTest(
                () => { return RfmBase.PayloadLength; },
                (v) => Assert.Equal(0x60, v),
                Commands.GetPayloadLength,
                "0x60");
        }

        [Fact]
        public void TestGetPreambleSize()
        {
            ExecuteGetTest(
                () => { return RfmBase.PreambleSize; },
                (v) => Assert.Equal(0x6000, v),
                Commands.GetPreambleSize,
                "0x6000");
        }

        [Fact]
        public void TestGetRadioVersion()
        {
            ExecuteGetTest(
                () => { return RfmBase.RadioVersion; },
                (v) => Assert.Equal(0x24, v),
                Commands.GetRadioVersion,
                "0x24");
        }

        [Fact]
        public void TestGetRssi()
        {
            ExecuteGetTest(
                () => { return RfmBase.Rssi; },
                (v) => Assert.Equal(-66, v),
                Commands.GetRssi,
                $"0x{(sbyte)-66:X2}");
        }

        [Fact]
        public void TestGetRxBw()
        {
            ExecuteGetTest(
                () => { return RfmBase.RxBw; },
                (v) => Assert.Equal(0xA0, v),
                Commands.GetRxBw,
                "0xA0");
        }

        [Fact]
        public void TestGetRxBwAfc()
        {
            ExecuteGetTest(
                () => { return RfmBase.RxBwAfc; },
                (v) => Assert.Equal(0xA0, v),
                Commands.GetRxBwAfc,
                "0xA0");
        }

        [Fact]
        public void TestGetSerialNumber()
        {
            ExecuteGetTest(
                () => { return RfmBase.SerialNumber; },
                (v) => Assert.Equal("SERIALNUMBER", v),
                Commands.GetSerialNumber,
                "SERIALNUMBER");
        }

        [Fact]
        public void TestGetSync()
        {
            ExecuteGetTest(
                () => { return RfmBase.Sync; },
                (v) => Assert.Equal(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }, v),
                Commands.GetSync,
                "0xAA55DEAD");
        }

        [Fact]
        public void TestGetSyncEnable()
        {
            ExecuteGetTest(
                () => { return RfmBase.SyncEnable; },
                Assert.True,
                Commands.GetSyncEnable,
                "1");
        }

        [Fact]
        public void TestGetSyncSize()
        {
            ExecuteGetTest(
                () => { return RfmBase.SyncSize; },
                (v) => Assert.Equal(0xA0, v),
                Commands.GetSyncSize,
                "0xA0");
        }

        [Fact]
        public void TestGetTemperatureValue()
        {
            ExecuteGetTest(
                () => { return RfmBase.Temperature; },
                (v) => Assert.Equal(-100, v),
                Commands.GetTemperatureValue,
                "0x9C");
        }

        [Fact]
        public void TestGetTxStartCondition()
        {
            ExecuteGetTest(
                () => { return RfmBase.TxStartCondition; },
                Assert.True,
                Commands.GetTxStartCondition,
                "1");
        }

        [Fact]
        public void TestOpenNotFound()
        {
            // Arrange
            MockSerialPortFactory
                .Setup(_ => _.GetSerialPorts())
                .Returns(new List<string>() { });

            MockSerialPort
                .Setup(_ => _.Open())
                .Throws<FileNotFoundException>();

            RfmBase.ResetSerialPort(MockSerialPort.Object);

            // Act
            Action action = () => RfmBase.Open("ComPort", 9600);

            // Assert
            Assert.Throws<RfmUsbSerialPortOpenFailedException>(action);
        }

        [Fact]
        public void TestRcCalibration()
        {
            ExecuteTest(
                () => { RfmBase.RcCalibration(); },
                Commands.ExecuteRcCalibration,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestReadFromBuffer()
        {
            ExecuteTest(
                () => { RfmBase.ReadFromBuffer(10); },
                $"{Commands.ReadIoBuffer} 0x0A",
                "AA55AA");
        }

        [Fact]
        public void TestReset()
        {
            ExecuteTest(
                () => { RfmBase.ExecuteReset(); },
                Commands.ExecuteReset,
                RfmBase.ResponseOk);
        }

        [Theory]
        [InlineData(AddressFilter.None)]
        [InlineData(AddressFilter.Node)]
        [InlineData(AddressFilter.Reserved)]
        [InlineData(AddressFilter.NodeBroaddcast)]
        public void TestSetAddressFilter(AddressFilter expected)
        {
            ExecuteSetTest(
                () => { RfmBase.AddressFiltering = expected; },
                Commands.SetAddressFiltering,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAfcAutoClear(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.AfcAutoClear = value; },
                Commands.SetAfcAutoClear,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAfcAutoOn(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.AfcAutoOn = value; },
                Commands.SetAfcAutoOn,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetBitRate()
        {
            ExecuteSetTest(
                () => { RfmBase.BitRate = 0x100; },
                Commands.SetBitRate,
                "0x100");
        }

        [Fact]
        public void TestSetBroadcastAddress()
        {
            ExecuteSetTest(
                () => { RfmBase.BroadcastAddress = 0xAA; },
                Commands.SetBroadcastAddress,
                "0xAA");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetBufferedIoEnable(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.BufferedIoEnable = value; },
                Commands.SetBufferEnable,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetCrcAutoClear(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.CrcAutoClearOff = value; },
                Commands.SetCrcAutoClearOff,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetCrcOn(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.CrcOn = value; },
                Commands.SetCrcOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(DcFreeEncoding.Manchester)]
        [InlineData(DcFreeEncoding.None)]
        [InlineData(DcFreeEncoding.Reserved)]
        [InlineData(DcFreeEncoding.Whitening)]
        public void TestSetDcFree(DcFreeEncoding expected)
        {
            ExecuteSetTest(
                () => { RfmBase.DcFreeEncoding = expected; },
                Commands.SetDcFree,
                $"0x{(byte)expected:X2}");
        }

        [Fact]
        public void TestSetDioInterruptMask()
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
               .Setup(_ => _.ReadLine())
               .Returns(RfmBase.ResponseOk);

            // Act
            RfmBase.DioInterruptMask = DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4 | DioIrq.Dio5;

            // Assert
            MockSerialPort
                .Verify(_ => _.Write($"{Commands.SetDioInterruptMask} 0x{(byte)(DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4 | DioIrq.Dio5):X}\n"),
                Times.Once);
        }

        [Theory]
        [InlineData(Dio.Dio0, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio1, DioMapping.DioMapping1)]
        [InlineData(Dio.Dio2, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio3, DioMapping.DioMapping3)]
        [InlineData(Dio.Dio4, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio5, DioMapping.DioMapping1)]
        public void TestSetDioMapping(Dio dio, DioMapping mapping)
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
               .Setup(_ => _.ReadLine())
               .Returns(RfmBase.ResponseOk);

            // Act
            RfmBase.SetDioMapping(dio, mapping);

            // Assert
            MockSerialPort
                .Verify(_ => _.Write($"{Commands.SetDioMapping} {(byte)dio} {(byte)mapping}\n"),
                Times.Once);
        }

        [Fact]
        public void TestSetFifo()
        {
            ExecuteSetTest(
                () => { RfmBase.Fifo = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetFifo,
                "AA55DEAD");
        }

        [Fact]
        public void TestSetFifoThreshold()
        {
            ExecuteSetTest(
                () => { RfmBase.FifoThreshold = 0x10; },
                Commands.SetFifoThreshold,
                "0x10");
        }

        [Fact]
        public void TestSetFrequency()
        {
            ExecuteSetTest(
                () => { RfmBase.Frequency = 0x100000; },
                Commands.SetFrequency,
                "0x100000");
        }

        [Fact]
        public void TestSetFrequencyDeviation()
        {
            ExecuteSetTest(
                () => { RfmBase.FrequencyDeviation = 0xA000; },
                Commands.SetFrequencyDeviation,
                "0xA000");
        }

        [Theory]
        [InlineData(FskModulationShaping.GaussianBt0_3)]
        [InlineData(FskModulationShaping.GaussianBt0_5)]
        [InlineData(FskModulationShaping.GaussianBt1_0)]
        public void TestSetFskModulationShaping(FskModulationShaping expected)
        {
            ExecuteSetTest(
                () => { RfmBase.FskModulationShaping = expected; },
                Commands.SetFskModulationShaping,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestSetInterPacketRxDelay()
        {
            ExecuteSetTest(
                () => { RfmBase.InterPacketRxDelay = 0x60; },
                Commands.SetInterPacketRxDelay,
                "0x60");
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
                () => { RfmBase.LnaGainSelect = expected; },
                Commands.SetLnaGainSelect,
                $"0x{(byte)expected:X2}");
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
                () => { RfmBase.Mode = expected; },
                Commands.SetMode,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(ModulationType.Fsk)]
        [InlineData(ModulationType.Ook)]
        public void TestSetModulation(ModulationType expected)
        {
            ExecuteSetTest(
                () => { RfmBase.ModulationType = expected; },
                Commands.SetModulationType,
                $"0x{(byte)expected:X2}");
        }

        [Fact]
        public void TestSetNodeAddress()
        {
            ExecuteSetTest(
                () => { RfmBase.NodeAddress = 0x60; },
                Commands.SetNodeAddress,
                "0x60");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetOcpEnable(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.OcpEnable = value; },
                Commands.SetOcpEnable,
                value ? "1" : "0");
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
                () => { RfmBase.OcpTrim = expected; },
                Commands.SetOcpTrim,
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
                () => { RfmBase.OokAverageThresholdFilter = expected; },
                Commands.SetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestSetOokFixedThreshold()
        {
            ExecuteSetTest(
                () => { RfmBase.OokFixedThreshold = 0x60; },
                Commands.SetOokFixedThreshold,
                "0x60");
        }

        [Theory]
        [InlineData(OokModulationShaping.Filtering2Br)]
        [InlineData(OokModulationShaping.FilteringBr)]
        [InlineData(OokModulationShaping.None)]
        [InlineData(OokModulationShaping.Reserved)]
        public void TestSetOokModulationShaping(OokModulationShaping expected)
        {
            ExecuteSetTest(
                () => { RfmBase.OokModulationShaping = expected; },
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
        public void TestSetOokPeakThresholdDec(OokThresholdDec expected)
        {
            ExecuteSetTest(
                () => { RfmBase.OokPeakThresholdDec = expected; },
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
        public void TestSetOokPeakThresholdStep(OokThresholdStep expected)
        {
            ExecuteSetTest(
                () => { RfmBase.OokPeakThresholdStep = expected; },
                Commands.SetOokPeakThresholdStep,
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
                () => { RfmBase.OokThresholdType = expected; },
                Commands.SetOokThresholdType,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetPacketFormat(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.PacketFormat = value; },
                Commands.SetPacketFormat,
                value ? "1" : "0");
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
                () => { RfmBase.PaRamp = expected; },
                Commands.SetPaRamp,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestSetPayloadLength()
        {
            ExecuteSetTest(
                () => { RfmBase.PayloadLength = 0x60; },
                Commands.SetPayloadLength,
                "0x60");
        }

        [Fact]
        public void TestSetPreambleSize()
        {
            ExecuteSetTest(
                () => { RfmBase.PreambleSize = 0x6000; },
                Commands.SetPreambleSize,
                "0x6000");
        }

        [Fact]
        public void TestSetRxBw()
        {
            ExecuteSetTest(
                () => { RfmBase.RxBw = 0xB0; },
                Commands.SetRxBw,
                "0xB0");
        }

        [Fact]
        public void TestSetRxBwAfc()
        {
            ExecuteSetTest(
                () => { RfmBase.RxBwAfc = 0xB0; },
                Commands.SetRxBwAfc,
                "0xB0");
        }

        [Fact]
        public void TestSetSync()
        {
            ExecuteSetTest(
                () => { RfmBase.Sync = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetSync,
                "AA55DEAD");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetSyncEnable(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.SyncEnable = value; },
                Commands.SetSyncEnable,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetSyncSize()
        {
            ExecuteSetTest(
                () => { RfmBase.SyncSize = 0xB0; },
                Commands.SetSyncSize,
                "0xB0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetTxStartCondition(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.TxStartCondition = value; },
                Commands.SetTxStartCondition,
                value ? "1" : "0");
        }

        [Fact]
        public void TestTransmit()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }); },
                $"{Commands.ExecuteTransmit} AADDFFCC",
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestTransmitBufferOk()
        {
            ExecuteTest(
                () => { RfmBase.TransmitBuffer(); },
                $"{Commands.ExecuteTransmitBuffer}",
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestTransmitReceive()
        {
            ExecuteTest(
                () => { RfmBase.TransmitReceive(new List<byte>() { 0xAA, 0x55 }); },
                $"{Commands.ExecuteTransmitReceive} AA55",
                "0xFEED");
        }

        [Fact]
        public void TestTransmitReceiveTimeout()
        {
            ExecuteTest(
                () => { RfmBase.TransmitReceive(new List<byte>() { 0xAA, 0x55 }, 100); },
                $"{Commands.ExecuteTransmitReceive} AA55 100",
                "0xFEED");
        }

        [Fact]
        public void TestTransmitReceiveTransmitTimeout()
        {
            ExecuteTest(
                () => { RfmBase.TransmitReceive(new List<byte>() { 0xAA, 0x55 }, 100, 200); },
                $"{Commands.ExecuteTransmitReceive} AA55 100 200",
                "0xFEED");
        }

        [Fact]
        public void TestTransmitWithTimeout()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200); },
                $"{Commands.ExecuteTransmit} AADDFFCC 0xC8",
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestTransmitWithTimeoutAndInterval()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200, 100); },
                $"{Commands.ExecuteTransmit} AADDFFCC 0xC8 0x64",
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestTransmitWithTimeoutAndIntervalAndTimeout()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200, 100, 50); },
                $"{Commands.ExecuteTransmit} AADDFFCC 0xC8 0x64 0x32",
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestWriteBuffer()
        {
            ExecuteSetTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "AA55DEAD");
        }

        [Fact]
        public void TestWriteBufferNotEnabled()
        {
            Action action = () => ExecuteTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "ERROR:BUFFERED_IO_NOT_ENABLED");

            Assert.Throws<RfmUsbBufferedIoNotEnabledException>(action);
        }

        [Fact]
        public void TestWriteBufferNotInTx()
        {
            Action action = () => ExecuteTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "ERROR:NOT_TX");

            Assert.Throws<RfmUsbTransmitNotEnabledException>(action);
        }

        [Fact]
        public void TestWriteBufferOverflow()
        {
            Action action = () => ExecuteTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "ERROR:OVERFLOW");

            Assert.Throws<RfmUsbBufferedIoOverflowException>(action);
        }

        internal static EventArgs CreateSerialDataReceivedEventArgs()
        {
            ConstructorInfo? constructor = typeof(SerialDataReceivedEventArgs)
                .GetConstructor(
                BindingFlags.NonPublic |
                BindingFlags.Instance, null, new[] { typeof(SerialData) }, null);

            return constructor?.Invoke(new object[] { SerialData.Chars }) as SerialDataReceivedEventArgs;
        }

        internal void ExecuteGetTest<T>(Func<T> action, Action<T> validation, string command, string value)
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(value);

            // Act
            var result = action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command}\n"));

            validation(result);
        }

        internal void ExecuteGetTest<T>(Func<T> action, Action<T> validation, string command, IList<string> values)
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            var serialPortBytesToReadSequence = MockSerialPort
                    .SetupSequence(_ => _.BytesToRead);

            var serialPortReadLineSequence = MockSerialPort
                .SetupSequence(_ => _.ReadLine());

            for (int i = 0; i < values.Count; i++)
            {
                serialPortReadLineSequence = serialPortReadLineSequence.Returns(values[i]);
                if (i > 0)
                {
                    serialPortBytesToReadSequence = serialPortBytesToReadSequence.Returns(values[i].Length);
                }
            }

            // Act
            var result = action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command}\n"));

            validation(result);
        }

        internal void ExecuteSetTest(Action action, string command, string value)
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            // Act
            action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command} {value}\n"), Times.Once);
        }

        internal void ExecuteTest(Action action, string command, string? response = null)
        {
            // Arrange
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            if (response != null)
            {
                MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(response);
            }

            // Act
            action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command}\n"), Times.Once);
        }

        internal void InitialiseRfmDevice()
        {
            MockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(MockSerialPort.Object);

            RfmBase.InitializeSerialPort(string.Empty, 0);
        }

        internal void TestOpenVersion(string version)
        {
            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            MockSerialPort
                .Setup(_ => _.Write(It.IsAny<string>()))
                .Raises(_ => _.DataReceived += null, CreateSerialDataReceivedEventArgs());

            MockSerialPort
            .SetupSequence(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk)
                .Returns(version)
                .Returns(RfmBase.ResponseOk);

            RfmBase.ResetSerialPort(MockSerialPort.Object);

            // Act
            RfmBase.Open("ComPort", 9600);

            // Assert
            MockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.AtLeastOnce);

            MockSerialPort.Verify(_ => _.Open(), Times.AtLeastOnce);
        }
    }
}