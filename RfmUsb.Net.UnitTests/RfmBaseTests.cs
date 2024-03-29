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


// Ignore Spelling: Usb Dio Ook Ocp Bw Fei Bootloader Fsk Rssi Lna

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RfmUsb.Net.Exceptions;
using RfmUsb.Net.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Reflection;

namespace RfmUsb.Net.UnitTests
{
    public abstract class RfmBaseTests
    {
        protected readonly ILogger<IRfm> MockLogger;
        protected readonly Mock<ISerialPort> MockSerialPort;
        protected readonly Mock<ISerialPortFactory> MockSerialPortFactory;
        protected RfmBase RfmBase;

        public RfmBaseTests()
        {
            MockSerialPortFactory = new Mock<ISerialPortFactory>();

            MockSerialPort = new Mock<ISerialPort>();

            MockLogger = Mock.Of<ILogger<IRfm>>();
        }

        [TestMethod]
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

        [TestMethod]
        public void TestExecuteBootloader()
        {
            ExecuteTest(
                () => { RfmBase.EnterBootloader(); },
                Commands.ExecuteBootloader,
                "Ok");
        }

        [TestMethod]
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
            addressFilter.Should().Be(AddressFilter.NodeBroaddcast);
        }

        [TestMethod]
        public void TestGetAfc()
        {
            ExecuteGetTest(
                () => { return RfmBase.Afc; },
                (v) => v.Should().Be(0x100),
                Commands.GetAfc,
                "0x100");
        }

        [TestMethod]
        public void TestGetAfcAutoClear()
        {
            ExecuteGetTest(
                () => { return RfmBase.AfcAutoClear; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcAutoClear,
                "1");
        }

        [TestMethod]
        public void TestGetAfcAutoOn()
        {
            ExecuteGetTest(
                () => { return RfmBase.AfcAutoOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAfcAutoOn,
                "1");
        }

        [TestMethod]
        public void TestGetBitRate()
        {
            ExecuteGetTest(
                () => { return RfmBase.BitRate; },
                (v) => { v.Should().Be(0x100); },
                Commands.GetBitRate,
                "0x100");
        }

        [TestMethod]
        public void TestGetBroadcastAddress()
        {
            ExecuteGetTest(
                () => { return RfmBase.BroadcastAddress; },
                (v) => { v.Should().Be(0x55); },
                Commands.GetBroadcastAddress,
                "0x55");
        }

        [TestMethod]
        public void TestGetBufferEnable()
        {
            ExecuteGetTest(
                () => { return RfmBase.BufferedIoEnable; },
                (v) => v.Should().BeTrue(),
                Commands.GetBufferEnable,
                "1");
        }

        [TestMethod]
        public void TestGetBufferIoInfo()
        {
            ExecuteGetTest(
                () => { return RfmBase.BufferedIoInfo; },
                (v) =>
                {
                    v.Capacity.Should().Be(160);
                    v.Count.Should().Be(16);
                },
                Commands.GetIoBufferInfo,
                new List<string>()
                {
                    "CAPACITY:0xA0",
                    "COUNT:0x10"
                });
        }

        [TestMethod]
        public void TestGetCrcAutoClear()
        {
            ExecuteGetTest(
                () => { return RfmBase.CrcAutoClearOff; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcAutoClearOff,
                "1");
        }

        [TestMethod]
        public void TestGetCrcOn()
        {
            ExecuteGetTest(
                () => { return RfmBase.CrcOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcOn,
                "1");
        }

        [TestMethod]
        [DataRow(DcFreeEncoding.Manchester)]
        [DataRow(DcFreeEncoding.None)]
        [DataRow(DcFreeEncoding.Reserved)]
        [DataRow(DcFreeEncoding.Whitening)]
        public void TestGetDcFreeEncoding(DcFreeEncoding expected)
        {
            ExecuteGetTest(
                () => { return RfmBase.DcFreeEncoding; },
                (v) => { v.Should().Be(expected); },
                Commands.GetDcFree,
                $"0x{expected:X}");
        }

        [TestMethod]
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
            result.Should().Be(DioIrq.Dio0 | DioIrq.Dio2 | DioIrq.Dio4);

            MockSerialPort.Verify(_ => _.Write($"{Commands.GetDioInterruptMask}\n"), Times.Once);
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
            result.Should().Be(DioMapping.DioMapping0);

            MockSerialPort.Verify(_ => _.Write($"{Commands.GetDioMapping} 0x{(byte)dio:X}\n"));
        }

        [TestMethod]
        public void TestGetFei()
        {
            ExecuteGetTest(
                () => { return RfmBase.Fei; },
                (v) => v.Should().Be(0x200),
                Commands.GetFei,
                "0x200");
        }

        [TestMethod]
        public void TestGetFifo()
        {
            ExecuteGetTest(
                () => { return RfmBase.Fifo; },
                (v) => v.Should().BeEquivalentTo(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }),
                Commands.GetFifo,
                "0xAA55DEAD");
        }

        [TestMethod]
        public void TestGetFifoThreshold()
        {
            ExecuteGetTest(
                () => { return RfmBase.FifoThreshold; },
                (v) => v.Should().Be(0x10),
                Commands.GetFifoThreshold,
                "0x10");
        }

        [TestMethod]
        public void TestGetFirmwareVersion()
        {
            ExecuteGetTest(
                () => { return RfmBase.FirmwareVersion; },
                (v) => v.Should().Be("RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz"),
                Commands.GetFirmwareVersion,
                "RfmUsb-RFM69 FW: v3.0.3 HW: 2.0 433Mhz");
        }

        [TestMethod]
        public void TestGetFrequency()
        {
            ExecuteGetTest(
                () => { return RfmBase.Frequency; },
                (v) => v.Should().Be(0x100000),
                Commands.GetFrequency,
                "0x100000");
        }

        [TestMethod]
        public void TestGetFrequencyDeviation()
        {
            ExecuteGetTest(
                () => { return RfmBase.FrequencyDeviation; },
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
                () => { return RfmBase.FskModulationShaping; },
                (v) => v.Should().Be(expected),
                Commands.GetFskModulationShaping,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetInterPacketRxDelay()
        {
            ExecuteGetTest(
                () => { return RfmBase.InterPacketRxDelay; },
                (v) => v.Should().Be(0xAA),
                Commands.GetInterPacketRxDelay,
                "0xAA");
        }

        [TestMethod]
        public void TestGetLastRssi()
        {
            ExecuteGetTest(
                () => { return RfmBase.LastRssi; },
                (v) => v.Should().Be(-18),
                Commands.GetLastRssi,
                "0xFFFFFFEE");
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
                () => { return RfmBase.LnaGainSelect; },
                (v) => v.Should().Be(expected),
                Commands.GetLnaGainSelect,
                $"0x{expected:X}");
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
                () => { return RfmBase.Mode; },
                (v) => v.Should().Be(expected),
                Commands.GetMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(ModulationType.Fsk)]
        [DataRow(ModulationType.Ook)]
        public void TestGetModulation(ModulationType expected)
        {
            ExecuteGetTest(
                () => { return RfmBase.ModulationType; },
                (v) => v.Should().Be(expected),
                Commands.GetModulationType,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetNodeAddress()
        {
            ExecuteGetTest(
                () => { return RfmBase.NodeAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetNodeAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetOcpEnable()
        {
            ExecuteGetTest(
                () => { return RfmBase.OcpEnable; },
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
                () => { return RfmBase.OcpTrim; },
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
                () => { return RfmBase.OokAverageThresholdFilter; },
                (v) => v.Should().Be(expected),
                Commands.GetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetOokFixedThreshold()
        {
            ExecuteGetTest(
                () => { return RfmBase.OokFixedThreshold; },
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
                () => { return RfmBase.OokModulationShaping; },
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
                () => { return RfmBase.OokPeakThresholdDec; },
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
                () => { return RfmBase.OokPeakThresholdStep; },
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
                () => { return RfmBase.OokThresholdType; },
                (v) => v.Should().Be(expected),
                Commands.GetOokThresholdType,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPacketFormat()
        {
            ExecuteGetTest(
                () => { return RfmBase.PacketFormat; },
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
                () => { return RfmBase.PaRamp; },
                (v) => v.Should().Be(expected),
                Commands.GetPaRamp,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPayloadLength()
        {
            ExecuteGetTest(
                () => { return RfmBase.PayloadLength; },
                (v) => v.Should().Be(0x60),
                Commands.GetPayloadLength,
                "0x60");
        }

        [TestMethod]
        public void TestGetPreambleSize()
        {
            ExecuteGetTest(
                () => { return RfmBase.PreambleSize; },
                (v) => v.Should().Be(0x6000),
                Commands.GetPreambleSize,
                "0x6000");
        }

        [TestMethod]
        public void TestGetRadioVersion()
        {
            ExecuteGetTest(
                () => { return RfmBase.RadioVersion; },
                (v) => v.Should().Be(0x24),
                Commands.GetRadioVersion,
                "0x24");
        }

        [TestMethod]
        public void TestGetRssi()
        {
            ExecuteGetTest(
                () => { return RfmBase.Rssi; },
                (v) => v.Should().Be(-66),
                Commands.GetRssi,
                $"0x{(sbyte)-66:X2}");
        }

        [TestMethod]
        public void TestGetRxBw()
        {
            ExecuteGetTest(
                () => { return RfmBase.RxBw; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRxBw,
                "0xA0");
        }

        [TestMethod]
        public void TestGetRxBwAfc()
        {
            ExecuteGetTest(
                () => { return RfmBase.RxBwAfc; },
                (v) => v.Should().Be(0xA0),
                Commands.GetRxBwAfc,
                "0xA0");
        }

        [TestMethod]
        public void TestGetSerialNumber()
        {
            ExecuteGetTest(
                () => { return RfmBase.SerialNumber; },
                (v) => v.Should().Be("SERIALNUMBER"),
                Commands.GetSerialNumber,
                "SERIALNUMBER");
        }

        [TestMethod]
        public void TestGetSync()
        {
            ExecuteGetTest(
                () => { return RfmBase.Sync; },
                (v) => v.Should().BeEquivalentTo(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }),
                Commands.GetSync,
                "0xAA55DEAD");
        }

        [TestMethod]
        public void TestGetSyncEnable()
        {
            ExecuteGetTest(
                () => { return RfmBase.SyncEnable; },
                (v) => v.Should().BeTrue(),
                Commands.GetSyncEnable,
                "1");
        }

        [TestMethod]
        public void TestGetSyncSize()
        {
            ExecuteGetTest(
                () => { return RfmBase.SyncSize; },
                (v) => v.Should().Be(0xA0),
                Commands.GetSyncSize,
                "0xA0");
        }

        [TestMethod]
        public void TestGetTemperatureValue()
        {
            ExecuteGetTest(
                () => { return RfmBase.Temperature; },
                (v) => v.Should().Be(-100),
                Commands.GetTemperatureValue,
                "0x9C");
        }

        [TestMethod]
        public void TestGetTxStartCondition()
        {
            ExecuteGetTest(
                () => { return RfmBase.TxStartCondition; },
                (v) => v.Should().BeTrue(),
                Commands.GetTxStartCondition,
                "1");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            RfmBase.SetupSerialPort(MockSerialPort.Object);
        }

        [TestMethod]
        public void TestOpenNotFound()
        {
            // Arrange
            MockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(MockSerialPort.Object);

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
            action.Should().Throw<RfmUsbSerialPortOpenFailedException>();
        }

        [TestMethod]
        public void TestRcCalibration()
        {
            ExecuteTest(
                () => { RfmBase.RcCalibration(); },
                Commands.ExecuteRcCalibration,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestReadFromBuffer()
        {
            ExecuteTest(
                () => { RfmBase.ReadFromBuffer(10); },
                $"{Commands.ReadIoBuffer} 0x0A",
                "AA55AA");
        }

        [TestMethod]
        public void TestReset()
        {
            ExecuteTest(
                () => { RfmBase.ExecuteReset(); },
                Commands.ExecuteReset,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        [DataRow(AddressFilter.None)]
        [DataRow(AddressFilter.Node)]
        [DataRow(AddressFilter.Reserved)]
        [DataRow(AddressFilter.NodeBroaddcast)]
        public void TestSetAddressFilter(AddressFilter expected)
        {
            ExecuteSetTest(
                () => { RfmBase.AddressFiltering = expected; },
                Commands.SetAddressFiltering,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetAfcAutoClear(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.AfcAutoClear = value; },
                Commands.SetAfcAutoClear,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetAfcAutoOn(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.AfcAutoOn = value; },
                Commands.SetAfcAutoOn,
                value ? "1" : "0");
        }

        [TestMethod]
        public void TestSetBitRate()
        {
            ExecuteSetTest(
                () => { RfmBase.BitRate = 0x100; },
                Commands.SetBitRate,
                "0x100");
        }

        [TestMethod]
        public void TestSetBroadcastAddress()
        {
            ExecuteSetTest(
                () => { RfmBase.BroadcastAddress = 0xAA; },
                Commands.SetBroadcastAddress,
                "0xAA");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetBufferedIoEnable(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.BufferedIoEnable = value; },
                Commands.SetBufferEnable,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetCrcAutoClear(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.CrcAutoClearOff = value; },
                Commands.SetCrcAutoClearOff,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetCrcOn(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.CrcOn = value; },
                Commands.SetCrcOn,
                value ? "1" : "0");
        }

        [TestMethod]
        [DataRow(DcFreeEncoding.Manchester)]
        [DataRow(DcFreeEncoding.None)]
        [DataRow(DcFreeEncoding.Reserved)]
        [DataRow(DcFreeEncoding.Whitening)]
        public void TestSetDcFree(DcFreeEncoding expected)
        {
            ExecuteSetTest(
                () => { RfmBase.DcFreeEncoding = expected; },
                Commands.SetDcFree,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
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

        [TestMethod]
        public void TestSetFifo()
        {
            ExecuteSetTest(
                () => { RfmBase.Fifo = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetFifo,
                "AA55DEAD");
        }

        [TestMethod]
        public void TestSetFifoThreshold()
        {
            ExecuteSetTest(
                () => { RfmBase.FifoThreshold = 0x10; },
                Commands.SetFifoThreshold,
                "0x10");
        }

        [TestMethod]
        public void TestSetFrequency()
        {
            ExecuteSetTest(
                () => { RfmBase.Frequency = 0x100000; },
                Commands.SetFrequency,
                "0x100000");
        }

        [TestMethod]
        public void TestSetFrequencyDeviation()
        {
            ExecuteSetTest(
                () => { RfmBase.FrequencyDeviation = 0xA000; },
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
                () => { RfmBase.FskModulationShaping = expected; },
                Commands.SetFskModulationShaping,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetInterPacketRxDelay()
        {
            ExecuteSetTest(
                () => { RfmBase.InterPacketRxDelay = 0x60; },
                Commands.SetInterPacketRxDelay,
                "0x60");
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
                () => { RfmBase.LnaGainSelect = expected; },
                Commands.SetLnaGainSelect,
                $"0x{(byte)expected:X2}");
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
                () => { RfmBase.Mode = expected; },
                Commands.SetMode,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(ModulationType.Fsk)]
        [DataRow(ModulationType.Ook)]
        public void TestSetModulation(ModulationType expected)
        {
            ExecuteSetTest(
                () => { RfmBase.ModulationType = expected; },
                Commands.SetModulationType,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetNodeAddress()
        {
            ExecuteSetTest(
                () => { RfmBase.NodeAddress = 0x60; },
                Commands.SetNodeAddress,
                "0x60");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetOcpEnable(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.OcpEnable = value; },
                Commands.SetOcpEnable,
                value ? "1" : "0");
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
                () => { RfmBase.OcpTrim = expected; },
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
                () => { RfmBase.OokAverageThresholdFilter = expected; },
                Commands.SetOokAverageThresholdFilter,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetOokFixedThreshold()
        {
            ExecuteSetTest(
                () => { RfmBase.OokFixedThreshold = 0x60; },
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
                () => { RfmBase.OokModulationShaping = expected; },
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
                () => { RfmBase.OokPeakThresholdDec = expected; },
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
                () => { RfmBase.OokPeakThresholdStep = expected; },
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
                () => { RfmBase.OokThresholdType = expected; },
                Commands.SetOokThresholdType,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetPacketFormat(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.PacketFormat = value; },
                Commands.SetPacketFormat,
                value ? "1" : "0");
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
                () => { RfmBase.PaRamp = expected; },
                Commands.SetPaRamp,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetPayloadLength()
        {
            ExecuteSetTest(
                () => { RfmBase.PayloadLength = 0x60; },
                Commands.SetPayloadLength,
                "0x60");
        }

        [TestMethod]
        public void TestSetPreambleSize()
        {
            ExecuteSetTest(
                () => { RfmBase.PreambleSize = 0x6000; },
                Commands.SetPreambleSize,
                "0x6000");
        }

        [TestMethod]
        public void TestSetRxBw()
        {
            ExecuteSetTest(
                () => { RfmBase.RxBw = 0xB0; },
                Commands.SetRxBw,
                "0xB0");
        }

        [TestMethod]
        public void TestSetRxBwAfc()
        {
            ExecuteSetTest(
                () => { RfmBase.RxBwAfc = 0xB0; },
                Commands.SetRxBwAfc,
                "0xB0");
        }

        [TestMethod]
        public void TestSetSync()
        {
            ExecuteSetTest(
                () => { RfmBase.Sync = new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }; },
                Commands.SetSync,
                "AA55DEAD");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetSyncEnable(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.SyncEnable = value; },
                Commands.SetSyncEnable,
                value ? "1" : "0");
        }

        [TestMethod]
        public void TestSetSyncSize()
        {
            ExecuteSetTest(
                () => { RfmBase.SyncSize = 0xB0; },
                Commands.SetSyncSize,
                "0xB0");
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestSetTxStartCondition(bool value)
        {
            ExecuteSetTest(
                () => { RfmBase.TxStartCondition = value; },
                Commands.SetTxStartCondition,
                value ? "1" : "0");
        }

        [TestMethod]
        public void TestTransmit()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }); },
                $"{Commands.ExecuteTransmit} AADDFFCC",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestTransmitBufferOk()
        {
            ExecuteTest(
                () => { RfmBase.TransmitBuffer(); },
                $"{Commands.ExecuteTransmitBuffer}",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestTransmitWithTimeout()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200); },
                $"{Commands.ExecuteTransmit} AADDFFCC 0xC8",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestTransmitWithTimeoutAndInterval()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200, 100); },
                $"{Commands.ExecuteTransmit} AADDFFCC 0xC8 0x64",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestTransmitWithTimeoutAndIntervalAndTimeout()
        {
            ExecuteTest(
                () => { RfmBase.Transmit(new List<byte>() { 0xAA, 0xDD, 0xFF, 0xCC }, 200, 100, 50); },
                $"{Commands.ExecuteTransmit} AADDFFCC 0xC8 0x64 0x32",
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestWriteBuffer()
        {
            ExecuteSetTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "AA55DEAD");
        }

        [TestMethod]
        public void TestWriteBufferNotEnabled()
        {
            Action action = () => ExecuteTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "ERROR:BUFFERED_IO_NOT_ENABLED");

            action.Should().Throw<RfmUsbBufferedIoNotEnabledException>();
        }

        [TestMethod]
        public void TestWriteBufferNotInTx()
        {
            Action action = () => ExecuteTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "ERROR:NOT_TX");

            action.Should().Throw<RfmUsbTransmitNotEnabledException>();
        }

        [TestMethod]
        public void TestWriteBufferOverflow()
        {
            Action action = () => ExecuteTest(
                () => { RfmBase.WriteToBuffer(new List<byte>() { 0xAA, 0x55, 0xDE, 0xAD }); },
                Commands.WriteIoBuffer,
                "ERROR:OVERFLOW");

            action.Should().Throw<RfmUsbBufferedIoOverflowException>();
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

            serialPortBytesToReadSequence = serialPortBytesToReadSequence.Returns(0);

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

        internal void TestOpen(string version)
        {
            // Arrange
            MockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(MockSerialPort.Object);

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
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            MockSerialPort.Verify(_ => _.Open(), Times.Once);
        }
    }
}