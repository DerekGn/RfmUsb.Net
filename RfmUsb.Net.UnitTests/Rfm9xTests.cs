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

namespace RfmUsb.Net.UnitTests
{
    [TestClass]
    public class Rfm9xTests : RfmBaseTests
    {
        private readonly Rfm9x _rfmDevice;

        public Rfm9xTests() : base()
        {
            _rfmDevice = new Rfm9x(MockLogger, MockSerialPortFactory.Object);
            RfmBase = _rfmDevice;
        }

        [TestMethod]
        public void ExecuteSequencerStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteSequencerStart(); },
                Commands.ExecuteSequencerStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        [DataRow(Timer.Timer1)]
        [DataRow(Timer.Timer2)]
        public void GetTimerCoefficient(Timer timer)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.GetTimerCoefficient(timer); },
                (v) => v.Should().Be(0x10),
                $"{Commands.GetTimerCoefficient} {(int)timer}",
                "0x10");
        }

        [TestMethod]
        [DataRow(Timer.Timer1, TimerResolution.Reserved)]
        [DataRow(Timer.Timer1, TimerResolution.Resolution64us)]
        [DataRow(Timer.Timer1, TimerResolution.Resolution4_1ms)]
        [DataRow(Timer.Timer1, TimerResolution.Resolution256ms)]
        [DataRow(Timer.Timer2, TimerResolution.Reserved)]
        [DataRow(Timer.Timer2, TimerResolution.Resolution64us)]
        [DataRow(Timer.Timer2, TimerResolution.Resolution4_1ms)]
        [DataRow(Timer.Timer2, TimerResolution.Resolution256ms)]
        public void GetTimerResolution(Timer timer, TimerResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.GetTimerResolution(timer); },
                (v) => v.Should().Be(expected),
                $"{Commands.GetTimerResolution} {(int)timer}",
                ((int)expected).ToString());
        }

        [TestMethod]
        public void TestExecuteAgcStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteAgcStart(); },
                Commands.ExecuteAgcStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteImageCalibration()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteImageCalibration(); },
                Commands.ExecuteImageCalibration,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteRestartRxWithoutPllLock()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteRestartRxWithoutPllLock(); },
                Commands.ExecuteRestartRxWithoutPllLock,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteRestartRxWithPllLock()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteRestartRxWithPllLock(); },
                Commands.ExecuteRestartRxWithPllLock,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteSequencerStop()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteSequencerStop(); },
                Commands.ExecuteSequencerStop,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestGetAccessSharedRegisters()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AccessSharedRegisters; },
                (v) => v.Should().BeTrue(),
                Commands.GetAccessSharedRegisters,
                "1");
        }

        [TestMethod]
        public void TestGetAgcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AgcAutoOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAgcAutoOn,
                "1");
        }

        [TestMethod]
        public void TestGetAutoImageCalibrationOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoImageCalibrationOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAutoImageCalibrationOn,
                "1");
        }

        [TestMethod]
        [DataRow(AutoRestartRxMode.Off)]
        [DataRow(AutoRestartRxMode.OnWaitForPllLock)]
        [DataRow(AutoRestartRxMode.OnWithoutPllRelock)]
        [DataRow(AutoRestartRxMode.Reserved)]
        public void TestGetAutoRestartRxMode(AutoRestartRxMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoRestartRxMode; },
                (v) => { v.Should().Be(expected); },
                Commands.GetAutoRestartRxMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetBeaconOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.BeaconOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetBeaconOn,
                "1");
        }

        [TestMethod]
        public void TestGetBitRateFractional()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.BitRateFractional; },
                (v) => v.Should().Be(0xAA),
                Commands.GetBitRateFractional,
                "0xAA");
        }

        [TestMethod]
        public void TestGetBitSyncOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.BitSyncOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetBitSyncOn,
                "1");
        }

        [TestMethod]
        [DataRow(ErrorCodingRate.FourFive)]
        [DataRow(ErrorCodingRate.FourSix)]
        [DataRow(ErrorCodingRate.FourSeven)]
        [DataRow(ErrorCodingRate.FourEight)]
        public void TestGetCodingRate(ErrorCodingRate expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ErrorCodingRate; },
                (v) => { v.Should().Be(expected); },
                Commands.GetErrorCodingRate,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(CrcWhiteningType.CrcCCITT)]
        [DataRow(CrcWhiteningType.CrcIbm)]
        public void TestGetCrcWhiteningType(CrcWhiteningType expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.CrcWhiteningType; },
                (v) => { v.Should().Be(expected); },
                Commands.GetCrcWhiteningType,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetFastHopOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FastHopOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetFastHopOn,
                "1");
        }

        [TestMethod]
        public void TestGetFifoAddressPointer()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoAddressPointer; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoAddressPointer,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFifoRxBaseAddress()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxBaseAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoRxBaseAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFifoRxByteAddressPointer()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxByteAddressPointer; },
                (v) => v.Should().Be(0x20),
                Commands.GetFifoRxByteAddressPointer,
                "0x20");
        }

        [TestMethod]
        public void TestGetFifoRxBytesNumber()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxBytesNumber; },
                (v) => v.Should().Be(0x20),
                Commands.GetFifoRxBytesNumber,
                "0x20");
        }

        [TestMethod]
        public void TestGetFifoRxCurrentAddress()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxCurrentAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoRxCurrentAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFifoTxBaseAddress()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoTxBaseAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoTxBaseAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFormerTemperatureValue()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FormerTemperature; },
                (v) => v.Should().Be((sbyte)0xA),
                Commands.GetFormerTemperatureValue,
                "0xA");
        }

        [TestMethod]
        public void TestGetFreqError()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FrequencyError; },
                (v) => v.Should().Be(0x2000),
                Commands.GetFreqError,
                "0x2000");
        }

        [TestMethod]
        public void TestGetFreqHoppingPeriod()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FrequencyHoppingPeriod; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFreqHoppingPeriod,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFromIdle()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromIdle; },
                (v) => v.Should().BeTrue(),
                Commands.GetFromIdle,
                "1");
        }

        [TestMethod]
        [DataRow(FromPacketReceived.ToLowPowerSelection)]
        [DataRow(FromPacketReceived.ToReceiveState)]
        [DataRow(FromPacketReceived.ToReceiveViaFSMode)]
        [DataRow(FromPacketReceived.ToSequencerOff)]
        [DataRow(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestGetFromPacketReceived(FromPacketReceived expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromPacketReceived; },
                (v) => { v.Should().Be(expected); },
                Commands.GetFromPacketReceived,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(FromReceive.ToSequencerOffOnPreambleDetect)]
        [DataRow(FromReceive.ToSequencerOffOnRssi)]
        [DataRow(FromReceive.ToSequencerOffOnSyncAddress)]
        [DataRow(FromReceive.ToPacketReceivedStateOnCrcOk)]
        [DataRow(FromReceive.ToLowPowerSelectionOnPayLoadReady)]
        public void TestGetFromReceived(FromReceive expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromReceive; },
                (v) => { v.Should().Be(expected); },
                Commands.GetFromReceive,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(FromRxTimeout.ToSequencerOff)]
        [DataRow(FromRxTimeout.ToLowPowerSelection)]
        [DataRow(FromRxTimeout.ToReceive)]
        [DataRow(FromRxTimeout.ToTransmit)]
        public void TestGetFromRxTimeout(FromRxTimeout expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromRxTimeout; },
                (v) => { v.Should().Be(expected); },
                Commands.GetFromRxTimeout,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(FromStart.ToLowPowerSelection)]
        [DataRow(FromStart.ToTransmitOnFifoLevel)]
        [DataRow(FromStart.ToTransmit)]
        [DataRow(FromStart.ToReceive)]
        public void TestGetFromStart(FromStart expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromStart; },
                (v) => { v.Should().Be(expected); },
                Commands.GetFromStart,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetFromTransmit()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromTransmit; },
                (v) => v.Should().BeTrue(),
                Commands.GetFromTransmit,
                "1");
        }

        [TestMethod]
        [DataRow(true, 0xAA)]
        [DataRow(false, 0x55)]
        public void TestGetHopChannel(bool state, int count)
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
                .Returns($"{(state ? "1:PLL_TIMEOUT" : "0:PLL_TIMEOUT")}")
                .Returns($"{(state ? "1:CRC_ON_PAYLOAD" : "0:CRC_ON_PAYLOAD")}")
                .Returns($"0x{count:X2}:FHSS_PRESENT_CHANNEL");

            MockSerialPort
                .SetupSequence(_ => _.BytesToRead)
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = _rfmDevice.HopChannel;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetHopChannel}\n"));

            result.Should().NotBeNull();
            result.PllTimeout.Should().Be(state);
            result.CrcOnPayload.Should().Be(state);
            result.Channel.Should().Be((byte)count);
        }

        [TestMethod]
        public void TestGetIdleMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IdleMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetIdleMode,
                "1");
        }

        [TestMethod]
        public void TestGetImplicitHeaderModeOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ImplicitHeaderModeOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetImplicitHeaderModeOn,
                "1");
        }

        [TestMethod]
        public void TestGetIoHomeOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IoHomeOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetIoHomeOn,
                "1");
        }

        [TestMethod]
        public void TestGetIoHomePowerFrame()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IoHomePowerFrame; },
                (v) => v.Should().BeTrue(),
                Commands.GetIoHomePowerFrame,
                "1");
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
                .Returns("1:LOW_BATTERY")
                .Returns("1:CRC_OK")
                .Returns("1:PAYLOAD_READY")
                .Returns("1:PACKET_SENT")
                .Returns("1:FIFO_OVERRUN")
                .Returns("1:FIFO_LEVEL")
                .Returns("1:FIFO_NOT_EMPTY")
                .Returns("1:FIFO_FULL")
                .Returns("1:ADDRESS_MATCH")
                .Returns("1:PREAMBLE_DETECT")
                .Returns("1:TIMEOUT")
                .Returns("1:RSSI")
                .Returns("1:PLL_LOCK")
                .Returns("1:TX_RDY")
                .Returns("1:RX_RDY")
                .Returns("1:MODE_RDY");

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
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = _rfmDevice.IrqFlags;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetIrqFlags}\n"));

            result.Should()
                .Be(Rfm9xIrqFlags.LowBattery |
                    Rfm9xIrqFlags.CrcOK |
                    Rfm9xIrqFlags.PayloadReady |
                    Rfm9xIrqFlags.PacketSent |
                    Rfm9xIrqFlags.FifoOverrun |
                    Rfm9xIrqFlags.FifoLevel |
                    Rfm9xIrqFlags.FifoNotEmpty |
                    Rfm9xIrqFlags.FifoFull |
                    Rfm9xIrqFlags.SyncAddressMatch |
                    Rfm9xIrqFlags.PreambleDetect |
                    Rfm9xIrqFlags.Timeout |
                    Rfm9xIrqFlags.Rssi |
                    Rfm9xIrqFlags.PllLock |
                    Rfm9xIrqFlags.TxReady |
                    Rfm9xIrqFlags.RxReady |
                    Rfm9xIrqFlags.ModeReady);
        }

        [TestMethod]
        public void TestGetIrqFlagsMask()
        {
        }

        [TestMethod]
        public void TestGetLnaBoostHf()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LnaBoostHf; },
                (v) => v.Should().BeTrue(),
                Commands.GetLnaBoostHf,
                "1");
        }

        [TestMethod]
        public void TestGetLongRangeMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LongRangeMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetLongRangeMode,
                "1");
        }

        [TestMethod]
        public void TestGetLoraAgcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LoraAgcAutoOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetLoraAgcAutoOn,
                "1");
        }

        [TestMethod]
        public void TestGetLoraIrqFlags()
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
                .Returns("1:CAD_DETECTED")
                .Returns("1:FHSS_CHANGE_CHANNEL")
                .Returns("1:CAD_DONE")
                .Returns("1:TX_DONE")
                .Returns("1:VALID_HEADER")
                .Returns("1:PAYLOAD_CRC_ERROR")
                .Returns("1:RX_DONE")
                .Returns("1:RX_TIMEOUT");

            MockSerialPort
                .SetupSequence(_ => _.BytesToRead)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = _rfmDevice.LoraIrqFlags;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetLoraIrqFlags}\n"));

            result.Should().Be(
                LoraIrqFlags.CadDetected |
                LoraIrqFlags.FhssChangeChannel |
                LoraIrqFlags.CadDone |
                LoraIrqFlags.TxDone |
                LoraIrqFlags.ValidHeader |
                LoraIrqFlags.RxDone |
                LoraIrqFlags.RxTimeout);
        }

        [TestMethod]
        public void TestGetLoraIrqFlagsMask()
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
                .Returns("1:CAD_DETECTED_MASK")
                .Returns("1:FHSS_CHANGE_CHANNEL_MASK")
                .Returns("1:CAD_DONE_MASK")
                .Returns("1:TX_DONE_MASK")
                .Returns("1:VALID_HEADER_MASK")
                .Returns("1:PAYLOAD_CRC_ERROR_MASK")
                .Returns("1:RX_DONE_MASK")
                .Returns("1:RX_TIMEOUT_MASK");

            MockSerialPort
                .SetupSequence(_ => _.BytesToRead)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = _rfmDevice.LoraIrqFlagsMask;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetLoraIrqFlagsMask}\n"));

            result.Should().Be(
                LoraIrqFlagsMask.CadDetectedMask |
                LoraIrqFlagsMask.FhssChangeChannelMask |
                LoraIrqFlagsMask.CadDoneMask |
                LoraIrqFlagsMask.TxDoneMask |
                LoraIrqFlagsMask.ValidHeaderMask |
                LoraIrqFlagsMask.RxDoneMask |
                LoraIrqFlagsMask.RxTimeoutMask);
        }

        [TestMethod]
        [DataRow(LoraMode.Standby)]
        [DataRow(LoraMode.RxSingle)]
        [DataRow(LoraMode.RxContinuous)]
        [DataRow(LoraMode.Standby)]
        [DataRow(LoraMode.Sleep)]
        [DataRow(LoraMode.RxContinuous)]
        [DataRow(LoraMode.Cad)]
        public void TestGetLoraMode(LoraMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LoraMode; },
                (v) => { v.Should().Be(expected); },
                Commands.GetLoraMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetLoraPayloadLength()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LoraPayloadLength; },
                (v) => v.Should().Be(0xAA),
                Commands.GetLoraPayloadLength,
                "0xAA");
        }

        [TestMethod]
        public void TestGetLowBatteryOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowBatteryOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowBatteryOn,
                "1");
        }

        [TestMethod]
        [DataRow(LowBatteryTrim.Volts1_695)]
        [DataRow(LowBatteryTrim.Volts1_764)]
        [DataRow(LowBatteryTrim.Volts1_835)]
        [DataRow(LowBatteryTrim.Volts1_905)]
        [DataRow(LowBatteryTrim.Volts1_976)]
        [DataRow(LowBatteryTrim.Volts2_045)]
        [DataRow(LowBatteryTrim.Volts2_116)]
        [DataRow(LowBatteryTrim.Volts2_185)]
        public void TestGetLowBatteryTrim(LowBatteryTrim expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowBatteryTrim; },
                (v) => { v.Should().Be(expected); },
                Commands.GetLowBatteryTrim,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetLowDataRateOptimize()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowDataRateOptimize; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowDataRateOptimize,
                "1");
        }

        [TestMethod]
        public void TestGetLowFrequencyMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowFrequencyMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowFrequencyMode,
                "1");
        }

        [TestMethod]
        public void TestGetLowPowerSelection()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowPowerSelection; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowPowerSelection,
                "1");
        }

        [TestMethod]
        public void TestGetMapPreambleDetect()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.MapPreambleDetect; },
                (v) => v.Should().BeTrue(),
                Commands.GetMapPreambleDetect,
                "1");
        }

        [TestMethod]
        [DataRow(ModemBandwidth.Bandwidth10_4KHZ)]
        public void TestGetModemBandwidth(ModemBandwidth expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ModemBandwidth; },
                (v) => { v.Should().Be(expected); },
                Commands.GetModemBandwidth,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetModemStatus()
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
                .Returns("1:SIGNAL_DETECTED")
                .Returns("1:SIGNAL_SYNCHRONIZED")
                .Returns("1:RX_ONGOING")
                .Returns("1:HEADER_INFO_VALID")
                .Returns("1:MODEM_CLEAR");

            MockSerialPort
                .SetupSequence(_ => _.BytesToRead)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(0);

            // Act
            var result = _rfmDevice.ModemStatus;

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.GetModemStatus}\n"));

            result.Should().Be(
                ModemStatus.SignalDetected |
                ModemStatus.SignalSynchronized |
                ModemStatus.RxOnGoing |
                ModemStatus.HeaderInfoValid |
                ModemStatus.ModemClear);
        }

        [TestMethod]
        [DataRow(OokAverageOffset.Offset0dB)]
        [DataRow(OokAverageOffset.Offset2dB)]
        [DataRow(OokAverageOffset.Offset4dB)]
        [DataRow(OokAverageOffset.Offset6dB)]
        public void TestGetOokAverageOffset(OokAverageOffset expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.OokAverageOffset; },
                (v) => { v.Should().Be(expected); },
                Commands.GetOokAverageOffset,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPacketRssi()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PacketRssi; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPacketRssi,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPacketSnr()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LastPacketSnr; },
                (v) => v.Should().Be(0xAA),
                Commands.GetLastPacketSnr,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPayloadMaxLength()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PayloadMaxLength; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPayloadMaxLength,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPpmCorrection()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PpmCorrection; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPpmCorrection,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPreambleDetectorOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleDetectorOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetPreambleDetectorOn,
                "1");
        }

        [TestMethod]
        [DataRow(PreambleDetectorSize.OneByte)]
        [DataRow(PreambleDetectorSize.TwoBytes)]
        [DataRow(PreambleDetectorSize.ThreeBytes)]
        public void TestGetPreambleDetectorSize(PreambleDetectorSize expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleDetectorSize; },
                (v) => { v.Should().Be(expected); },
                Commands.GetPreambleDetectorSize,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPreambleDetectorTotalerance()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleDetectorTotalerance; },
                (v) => v.Should().Be(30),
                Commands.GetPreambleDetectorTotalerance,
                $"0x{(sbyte)30:X2}");
        }

        [TestMethod]
        public void TestGetPreambleLength()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleLength; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPreambleLength,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPreamblePolarity()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreamblePolarity; },
                (v) => v.Should().BeTrue(),
                Commands.GetPreamblePolarity,
                "1");
        }

        [TestMethod]
        public void TestGetRestartRxOnCollision()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RestartRxOnCollision; },
                (v) => v.Should().BeTrue(),
                Commands.GetRestartRxOnCollision,
                "1");
        }

        [TestMethod]
        public void TestGetRssiCollisionThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiCollisionThreshold; },
                (v) => v.Should().Be(0xAA),
                Commands.GetRssiCollisionThreshold,
                "0xAA");
        }

        [TestMethod]
        public void TestGetRssiOffset()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiOffset; },
                (v) => v.Should().Be(-98),
                Commands.GetRssiOffset,
                $"0x{(sbyte)-98:X2}");
        }

        [TestMethod]
        [DataRow(RssiSmoothing.Samples2)]
        [DataRow(RssiSmoothing.Samples4)]
        [DataRow(RssiSmoothing.Samples8)]
        [DataRow(RssiSmoothing.Samples16)]
        [DataRow(RssiSmoothing.Samples32)]
        [DataRow(RssiSmoothing.Samples64)]
        [DataRow(RssiSmoothing.Samples128)]
        [DataRow(RssiSmoothing.Samples256)]
        public void TestGetRssiSmoothing(RssiSmoothing expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiSmoothing; },
                (v) => { v.Should().Be(expected); },
                Commands.GetRssiSmoothing,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiThreshold; },
                (v) => v.Should().Be(-114),
                Commands.GetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }

        [TestMethod]
        public void TestGetRssiWideband()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiWideband; },
                (v) => v.Should().Be(0xAA),
                Commands.GetRssiWideband,
                "0xAA");
        }

        [TestMethod]
        public void TestGetRxCodingRate()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RxCodingRate; },
                (v) => v.Should().Be(0xAA),
                Commands.GetRxCodingRate,
                "0xAA");
        }

        [TestMethod]
        public void TestGetRxPayloadCrcOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RxPayloadCrcOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetRxPayloadCrcOn,
                "1");
        }

        [TestMethod]
        [DataRow(SpreadingFactor.SpreadFactor64)]
        [DataRow(SpreadingFactor.SpreadFactor128)]
        [DataRow(SpreadingFactor.SpreadFactor512)]
        [DataRow(SpreadingFactor.SpreadFactor1024)]
        [DataRow(SpreadingFactor.SpreadFactor2048)]
        [DataRow(SpreadingFactor.SpreadFactor4096)]
        public void TestGetSpreadingFactor(SpreadingFactor expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SpreadingFactor; },
                (v) => { v.Should().Be(expected); },
                Commands.GetSpreadingFactor,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetSymbolTimeout()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SymbolTimeout; },
                (v) => v.Should().Be(0x100),
                Commands.GetSymbolTimeout,
                "0x100");
        }

        [TestMethod]
        public void TestGetTcxoInputOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TcxoInputOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetTcxoInputOn,
                "1");
        }

        [TestMethod]
        [DataRow(TemperatureThreshold.FiveDegrees)]
        [DataRow(TemperatureThreshold.TenDegrees)]
        [DataRow(TemperatureThreshold.FifteenDegrees)]
        [DataRow(TemperatureThreshold.TwentyDegrees)]
        public void TestGetTemperatureThreshold(TemperatureThreshold expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TemperatureThreshold; },
                (v) => { v.Should().Be(expected); },
                Commands.GetTemperatureThreshold,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetTempMonitorOff()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TempMonitorOff; },
                (v) => v.Should().BeTrue(),
                Commands.GetTempMonitorOff,
                "1");
        }

        [TestMethod]
        public void TestGetTimeoutRxPreamble()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRxPreamble; },
                (v) => v.Should().Be(0xAA),
                Commands.GetTimeoutRxPreamble,
                "0xAA");
        }

        [TestMethod]
        public void TestGetTimeoutRxRssi()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRxRssi; },
                (v) => v.Should().Be(0xAA),
                Commands.GetTimeoutRxRssi,
                "0xAA");
        }

        [TestMethod]
        public void TestGetTimeoutSignalSync()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutSignalSync; },
                (v) => v.Should().Be(0xAA),
                Commands.GetTimeoutSignalSync,
                "0xAA");
        }

        [TestMethod]
        public void TestGetTxContinuousMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TxContinuousMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetTxContinuousMode,
                "1");
        }

        [TestMethod]
        public void TestGetValidHeaderCount()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ValidHeaderCount; },
                (v) => v.Should().Be(0xAA),
                Commands.GetValidHeaderCount,
                "0xAA");
        }

        [TestMethod]
        public void TestGetValidPacketCount()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ValidPacketCount; },
                (v) => v.Should().Be(0xAA),
                Commands.GetValidPacketCount,
                "0xAA");
        }

        [TestMethod]
        public void TestOpen()
        {
            TestOpen("RfmUsb-RFM9x FW: v3.0.3 HW: 2.0 433Mhz");
        }

        //[TestMethod]
        //public void TestLoraFlags()
        //{
        //    var x = _rfmDevice.LoraIrqFlags;
        //}
        [TestMethod]
        public void TestSetAccessSharedRegisters()
        {
            ExecuteSetTest(
                () => { _rfmDevice.AccessSharedRegisters = true; },
                Commands.SetAccessSharedRegisters,
                "1");
        }

        [TestMethod]
        public void TestSetAesOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LoraAgcAutoOn = true; },
                Commands.SetLoraAgcAutoOn,
                "1");
        }

        [TestMethod]
        public void TestSetAgcAutoOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.AgcAutoOn = true; },
                Commands.SetAgcAutoOn,
                "1");
        }

        [TestMethod]
        public void TestSetAutoImageCalibrationOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoImageCalibrationOn = true; },
                Commands.SetAutoImageCalibrationOn,
                "1");
        }

        [TestMethod]
        [DataRow(AutoRestartRxMode.Off)]
        [DataRow(AutoRestartRxMode.OnWaitForPllLock)]
        [DataRow(AutoRestartRxMode.OnWithoutPllRelock)]
        [DataRow(AutoRestartRxMode.Reserved)]
        public void TestSetAutoRestartRxMode(AutoRestartRxMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoRestartRxMode = expected; },
                Commands.SetAutoRestartRxMode,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetBeaconOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.BeaconOn = true; },
                Commands.SetBeaconOn,
                "1");
        }

        [TestMethod]
        public void TestSetBitRateFractional()
        {
            ExecuteSetTest(
                () => { _rfmDevice.BitRateFractional = 0x55; },
                Commands.SetBitRateFractional,
                "0x55");
        }

        [TestMethod]
        public void TestSetBitSyncOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.BitSyncOn = true; },
                Commands.SetBitSyncOn,
                "1");
        }

        [TestMethod]
        [DataRow(ErrorCodingRate.FourFive)]
        [DataRow(ErrorCodingRate.FourSix)]
        [DataRow(ErrorCodingRate.FourSeven)]
        [DataRow(ErrorCodingRate.FourEight)]
        public void TestSetCodingRate(ErrorCodingRate expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ErrorCodingRate = expected; },
                Commands.SetErrorCodingRate,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(CrcWhiteningType.CrcCCITT)]
        [DataRow(CrcWhiteningType.CrcIbm)]
        public void TestSetCrcWhiteningType(CrcWhiteningType expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.CrcWhiteningType = expected; },
                Commands.SetCrcWhiteningType,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetFastHopOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FastHopOn = true; },
                Commands.SetFastHopOn,
                "1");
        }

        [TestMethod]
        public void TestSetFifoAddressPointer()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoAddressPointer = 0x55; },
                Commands.SetFifoAddressPointer,
                "0x55");
        }

        [TestMethod]
        public void TestSetFifoRxBaseAddress()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoRxBaseAddress = 0x55; },
                Commands.SetFifoRxBaseAddress,
                "0x55");
        }

        [TestMethod]
        public void TestSetFifoTxBaseAddress()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoTxBaseAddress = 0x55; },
                Commands.SetFifoTxBaseAddress,
                "0x55");
        }

        [TestMethod]
        public void TestSetFormerTemperatureValue()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FormerTemperature = 0x55; },
                Commands.SetFormerTemperatureValue,
                "0x55");
        }

        [TestMethod]
        public void TestSetFreqHoppingPeriod()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FrequencyHoppingPeriod = 0x55; },
                Commands.SetFreqHoppingPeriod,
                "0x55");
        }

        [TestMethod]
        public void TestSetFromIdle()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromIdle = true; },
                Commands.SetFromIdle,
                "1");
        }

        [TestMethod]
        [DataRow(FromPacketReceived.ToLowPowerSelection)]
        [DataRow(FromPacketReceived.ToReceiveState)]
        [DataRow(FromPacketReceived.ToReceiveViaFSMode)]
        [DataRow(FromPacketReceived.ToSequencerOff)]
        [DataRow(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestSetFromPacketReceived(FromPacketReceived expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromPacketReceived = expected; },
                Commands.SetFromPacketReceived,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(FromReceive.ToSequencerOffOnPreambleDetect)]
        [DataRow(FromReceive.ToSequencerOffOnRssi)]
        [DataRow(FromReceive.ToSequencerOffOnSyncAddress)]
        [DataRow(FromReceive.ToPacketReceivedStateOnCrcOk)]
        [DataRow(FromReceive.ToLowPowerSelectionOnPayLoadReady)]
        public void TestSetFromReceived(FromReceive expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromReceive = expected; },
                Commands.SetFromReceive,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        [DataRow(FromRxTimeout.ToSequencerOff)]
        [DataRow(FromRxTimeout.ToLowPowerSelection)]
        [DataRow(FromRxTimeout.ToReceive)]
        [DataRow(FromRxTimeout.ToTransmit)]
        public void TestSetFromRxTimeout(FromRxTimeout expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromRxTimeout = expected; },
                Commands.SetFromRxTimeout,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(FromStart.ToLowPowerSelection)]
        [DataRow(FromStart.ToTransmitOnFifoLevel)]
        [DataRow(FromStart.ToTransmit)]
        [DataRow(FromStart.ToReceive)]
        public void TestSetFromStart(FromStart expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromStart = expected; },
                Commands.SetFromStart,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetFromTransmit()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromTransmit = true; },
                Commands.SetFromTransmit,
                "1");
        }

        [TestMethod]
        public void TestSetIdleMode()
        {
            ExecuteSetTest(
                () => { _rfmDevice.IdleMode = true; },
                Commands.SetIdleMode,
                "1");
        }

        [TestMethod]
        public void TestSetImplicitHeaderModeOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.ImplicitHeaderModeOn = true; },
                Commands.SetImplicitHeaderModeOn,
                "1");
        }

        [TestMethod]
        public void TestSetIoHomeOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.IoHomeOn = true; },
                Commands.SetIoHomeOn,
                "1");
        }

        [TestMethod]
        public void TestSetIoHomePowerFrame()
        {
            ExecuteSetTest(
                () => { _rfmDevice.IoHomePowerFrame = true; },
                Commands.SetIoHomePowerFrame,
                "1");
        }

        [TestMethod]
        public void TestSetIrqFlags()
        {
            ExecuteSetTest(
                () =>
                {
                    _rfmDevice.IrqFlags =
                    Rfm9xIrqFlags.LowBattery |
                    Rfm9xIrqFlags.FifoOverrun |
                    Rfm9xIrqFlags.SyncAddressMatch |
                    Rfm9xIrqFlags.PreambleDetect |
                    Rfm9xIrqFlags.Rssi;
                },
                Commands.SetIrqFlags,
                "0x0B11");
        }

        [TestMethod]
        public void TestSetLnaBoostHf()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LnaBoostHf = true; },
                Commands.SetLnaBoostHf,
                "1");
        }

        [TestMethod]
        public void TestSetLongRangeMode()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LongRangeMode = true; },
                Commands.SetLongRangeMode,
                "1");
        }

        [TestMethod]
        public void TestSetLoraIrqFlagMask()
        {
            ExecuteSetTest(() =>
            {
                _rfmDevice.LoraIrqFlagsMask =
                    LoraIrqFlagsMask.CadDetectedMask |
                    LoraIrqFlagsMask.FhssChangeChannelMask |
                    LoraIrqFlagsMask.CadDoneMask |
                    LoraIrqFlagsMask.TxDoneMask |
                    LoraIrqFlagsMask.ValidHeaderMask |
                    LoraIrqFlagsMask.PayloadCrcErrorMask |
                    LoraIrqFlagsMask.RxDoneMask |
                    LoraIrqFlagsMask.RxTimeoutMask;
            },
            Commands.SetLoraIrqFlagsMask,
            "0xFF");
        }

        [TestMethod]
        public void TestSetLoraIrqFlags()
        {
            ExecuteSetTest(() =>
            {
                _rfmDevice.LoraIrqFlags =
                    LoraIrqFlags.CadDetected |
                    LoraIrqFlags.FhssChangeChannel |
                    LoraIrqFlags.CadDone |
                    LoraIrqFlags.TxDone |
                    LoraIrqFlags.ValidHeader |
                    LoraIrqFlags.PayloadCrcError |
                    LoraIrqFlags.RxDone |
                    LoraIrqFlags.RxTimeout;
            },
            Commands.SetLoraIrqFlags,
            "0xFF");
        }

        [TestMethod]
        public void TestSetLoraIrqFlagsMask()
        {
        }

        [TestMethod]
        [DataRow(LoraMode.Standby)]
        [DataRow(LoraMode.RxSingle)]
        [DataRow(LoraMode.RxContinuous)]
        [DataRow(LoraMode.Standby)]
        [DataRow(LoraMode.Sleep)]
        [DataRow(LoraMode.RxContinuous)]
        [DataRow(LoraMode.Cad)]
        public void TestSetLoraMode(LoraMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LoraMode = expected; },
                Commands.SetLoraMode,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetLoraPayloadLength()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LoraPayloadLength = 0x55; },
                Commands.SetLoraPayloadLength,
                "0x55");
        }

        [TestMethod]
        public void TestSetLowBatteryOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowBatteryOn = true; },
                Commands.SetLowBatteryOn,
                "1");
        }

        [TestMethod]
        [DataRow(LowBatteryTrim.Volts1_695)]
        [DataRow(LowBatteryTrim.Volts1_764)]
        [DataRow(LowBatteryTrim.Volts1_835)]
        [DataRow(LowBatteryTrim.Volts1_905)]
        [DataRow(LowBatteryTrim.Volts1_976)]
        [DataRow(LowBatteryTrim.Volts2_045)]
        [DataRow(LowBatteryTrim.Volts2_116)]
        [DataRow(LowBatteryTrim.Volts2_185)]
        public void TestSetLowBatteryTrim(LowBatteryTrim expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowBatteryTrim = expected; },
                Commands.SetLowBatteryTrim,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetLowDataRateOptimize()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowDataRateOptimize = true; },
                Commands.SetLowDataRateOptimize,
                "1");
        }

        [TestMethod]
        public void TestSetLowFrequencyMode()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowFrequencyMode = true; },
                Commands.SetLowFrequencyMode,
                "1");
        }

        [TestMethod]
        public void TestSetLowPowerSelection()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowPowerSelection = true; },
                Commands.SetLowPowerSelection,
                "1");
        }

        [TestMethod]
        public void TestSetMapPreambleDetect()
        {
            ExecuteSetTest(
                () => { _rfmDevice.MapPreambleDetect = true; },
                Commands.SetMapPreambleDetect,
                "1");
        }

        [TestMethod]
        [DataRow(ModemBandwidth.Bandwidth10_4KHZ)]
        public void TestSetModemBandwidth(ModemBandwidth expected)
        {
            ExecuteSetTest(
    () => { _rfmDevice.ModemBandwidth = expected; },
    Commands.SetModemBandwidth,
    $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokAverageOffset.Offset0dB)]
        [DataRow(OokAverageOffset.Offset2dB)]
        [DataRow(OokAverageOffset.Offset4dB)]
        [DataRow(OokAverageOffset.Offset6dB)]
        public void TestSetOokAverageOffset(OokAverageOffset expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.OokAverageOffset = expected; },
                Commands.SetOokAverageOffset,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetPayloadMaxLength()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PayloadMaxLength = 0x55; },
                Commands.SetPayloadMaxLength,
                "0x55");
        }

        [TestMethod]
        public void TestSetPpmCorrection()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PpmCorrection = 0x55; },
                Commands.SetPpmCorrection,
                "0x55");
        }

        [TestMethod]
        public void TestSetPreambleDetectorOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleDetectorOn = true; },
                Commands.SetPreambleDetectorOn,
                "1");
        }

        [TestMethod]
        [DataRow(PreambleDetectorSize.OneByte)]
        [DataRow(PreambleDetectorSize.TwoBytes)]
        [DataRow(PreambleDetectorSize.ThreeBytes)]
        public void TestSetPreambleDetectorSize(PreambleDetectorSize expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleDetectorSize = expected; },
                Commands.SetPreambleDetectorSize,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetPreambleDetectorTotalerance()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleDetectorTotalerance = 55; },
                Commands.SetPreambleDetectorTotalerance,
                $"0x{(sbyte)55:X2}");
        }

        [TestMethod]
        public void TestSetPreambleLength()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleLength = 0xAA55; },
                Commands.SetPreambleLength,
                "0xAA55");
        }

        [TestMethod]
        public void TestSetPreamblePolarity()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreamblePolarity = true; },
                Commands.SetPreamblePolarity,
                "1");
        }

        [TestMethod]
        public void TestSetRestartRxOnCollision()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RestartRxOnCollision = true; },
                Commands.SetRestartRxOnCollision,
                "1");
        }

        [TestMethod]
        public void TestSetRssiCollisionThreshold()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiCollisionThreshold = 0x55; },
                Commands.SetRssiCollisionThreshold,
                "0x55");
        }

        [TestMethod]
        public void TestSetRssiOffset()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiOffset = 0x55; },
                Commands.SetRssiOffset,
                "0x55");
        }

        [TestMethod]
        [DataRow(RssiSmoothing.Samples2)]
        [DataRow(RssiSmoothing.Samples4)]
        [DataRow(RssiSmoothing.Samples8)]
        [DataRow(RssiSmoothing.Samples16)]
        [DataRow(RssiSmoothing.Samples32)]
        [DataRow(RssiSmoothing.Samples64)]
        [DataRow(RssiSmoothing.Samples128)]
        [DataRow(RssiSmoothing.Samples256)]
        public void TestSetRssiSmoothing(RssiSmoothing expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiSmoothing = expected; },
                Commands.SetRssiSmoothing,
                $"0x{expected:X}");
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
        public void TestSetRxPayloadCrcOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RxPayloadCrcOn = true; },
                Commands.SetRxPayloadCrcOn,
                "1");
        }

        [TestMethod]
        [DataRow(SpreadingFactor.SpreadFactor64)]
        [DataRow(SpreadingFactor.SpreadFactor128)]
        [DataRow(SpreadingFactor.SpreadFactor512)]
        [DataRow(SpreadingFactor.SpreadFactor1024)]
        [DataRow(SpreadingFactor.SpreadFactor2048)]
        [DataRow(SpreadingFactor.SpreadFactor4096)]
        public void TestSetSpreadingFactor(SpreadingFactor expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.SpreadingFactor = expected; },
                Commands.SetSpreadingFactor,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetSymbolTimeout()
        {
            ExecuteSetTest(
                () => { _rfmDevice.SymbolTimeout = 0x100; },
                Commands.SetSymbolTimeout,
                "0x100");
        }

        [TestMethod]
        public void TestSetTcxoInputOn()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TcxoInputOn = true; },
                Commands.SetTcxoInputOn,
                "1");
        }

        [TestMethod]
        [DataRow(TemperatureThreshold.FiveDegrees)]
        [DataRow(TemperatureThreshold.TenDegrees)]
        [DataRow(TemperatureThreshold.FifteenDegrees)]
        [DataRow(TemperatureThreshold.TwentyDegrees)]
        public void TestSetTemperatureThreshold(TemperatureThreshold expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.TemperatureThreshold = expected; },
                Commands.SetTemperatureThreshold,
                $"0x{(byte)expected:X2}");
        }

        [TestMethod]
        public void TestSetTempMonitorOff()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TempMonitorOff = true; },
                Commands.SetTempMonitorOff,
                "1");
        }

        [TestMethod]
        public void TestSetTimeoutRxPreamble()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRxPreamble = 0x55; },
                Commands.SetTimeoutRxPreamble,
                "0x55");
        }

        [TestMethod]
        public void TestSetTimeoutRxRssi()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRxRssi = 0x55; },
                Commands.SetTimeoutRxRssi,
                "0x55");
        }

        [TestMethod]
        public void TestSetTimeoutSignalSync()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutSignalSync = 0x55; },
                Commands.SetTimeoutSignalSync,
                "0x55");
        }

        [TestMethod]
        [DataRow(Timer.Timer1)]
        [DataRow(Timer.Timer2)]
        public void TestSetTimerCoefficent(Timer expected)
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
            _rfmDevice.SetTimerCoefficient(expected, 10);

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.SetTimerCoefficient} {(int)expected} 0x0A\n"), Times.Once);
        }

        [TestMethod]
        [DataRow(Timer.Timer1)]
        [DataRow(Timer.Timer2)]
        public void TestSetTimerResolution(Timer expected)
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
            _rfmDevice.SetTimerResolution(expected, TimerResolution.Resolution64us);

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{Commands.SetTimerResolution} {(int)expected} 0x{(byte)TimerResolution.Resolution64us:X2}\n"), Times.Once);
        }

        [TestMethod]
        public void TestSetTxContinuousMode()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TxContinuousMode = true; },
                Commands.SetTxContinuousMode,
                "1");
        }
    }
}