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


// Ignore Spelling: Agc Io Irq Lna Lora Ook Rx Pll Rfm Rssi Tcxo Tx

using Microsoft.Extensions.Logging;
using Moq;
using RfmUsb.Net.Threading;
using Xunit;

namespace RfmUsb.Net.UnitTests
{
    public class Rfm9xTests : RfmBaseTests
    {
        private readonly Rfm9x _rfmDevice;

        public Rfm9xTests() : base()
        {
            _rfmDevice = new Rfm9x(
                Mock.Of<ILogger<Rfm9x>>(),
                MockSerialPortFactory.Object);

            RfmBase = _rfmDevice;

            InitialiseRfmDevice();
        }

        [Fact]
        public void ExecuteSequencerStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteSequencerStart(); },
                Commands.ExecuteSequencerStart,
                RfmBase.ResponseOk);
        }

        [Theory]
        [InlineData(Timer.Timer1)]
        [InlineData(Timer.Timer2)]
        public void GetTimerCoefficient(Timer timer)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.GetTimerCoefficient(timer); },
                (v) => Assert.Equal(0x10, v),
                $"{Commands.GetTimerCoefficient} {(int)timer}",
                "0x10");
        }

        [Theory]
        [InlineData(Timer.Timer1, TimerResolution.Reserved)]
        [InlineData(Timer.Timer1, TimerResolution.Resolution64us)]
        [InlineData(Timer.Timer1, TimerResolution.Resolution4_1ms)]
        [InlineData(Timer.Timer1, TimerResolution.Resolution256ms)]
        [InlineData(Timer.Timer2, TimerResolution.Reserved)]
        [InlineData(Timer.Timer2, TimerResolution.Resolution64us)]
        [InlineData(Timer.Timer2, TimerResolution.Resolution4_1ms)]
        [InlineData(Timer.Timer2, TimerResolution.Resolution256ms)]
        public void GetTimerResolution(Timer timer, TimerResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.GetTimerResolution(timer); },
                (v) => Assert.Equal(expected, v),
                $"{Commands.GetTimerResolution} {(int)timer}",
                ((int)expected).ToString());
        }

        [Fact]
        public void TestExecuteAgcStart()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteAgcStart(); },
                Commands.ExecuteAgcStart,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestExecuteImageCalibration()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteImageCalibration(); },
                Commands.ExecuteImageCalibration,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestExecuteRestartRxWithoutPllLock()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteRestartRxWithoutPllLock(); },
                Commands.ExecuteRestartRxWithoutPllLock,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestExecuteRestartRxWithPllLock()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteRestartRxWithPllLock(); },
                Commands.ExecuteRestartRxWithPllLock,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestExecuteSequencerStop()
        {
            ExecuteTest(
                () => { _rfmDevice.ExecuteSequencerStop(); },
                Commands.ExecuteSequencerStop,
                RfmBase.ResponseOk);
        }

        [Fact]
        public void TestGetAccessSharedRegisters()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AccessSharedRegisters; },
                Assert.True,
                Commands.GetAccessSharedRegisters,
                "1");
        }

        [Fact]
        public void TestGetAgcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AgcAutoOn; },
                Assert.True,
                Commands.GetAgcAutoOn,
                "1");
        }

        [Fact]
        public void TestGetAutoImageCalibrationOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoImageCalibrationOn; },
                Assert.True,
                Commands.GetAutoImageCalibrationOn,
                "1");
        }

        [Theory]
        [InlineData(AutoRestartRxMode.Off)]
        [InlineData(AutoRestartRxMode.OnWaitForPllLock)]
        [InlineData(AutoRestartRxMode.OnWithoutPllRelock)]
        [InlineData(AutoRestartRxMode.Reserved)]
        public void TestGetAutoRestartRxMode(AutoRestartRxMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.AutoRestartRxMode; },
                (v) => Assert.Equal(expected, v),
                Commands.GetAutoRestartRxMode,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetBeaconOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.BeaconOn; },
                Assert.True,
                Commands.GetBeaconOn,
                "1");
        }

        [Fact]
        public void TestGetBitRateFractional()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.BitRateFractional; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetBitRateFractional,
                "0xAA");
        }

        [Fact]
        public void TestGetBitSyncOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.BitSyncOn; },
                Assert.True,
                Commands.GetBitSyncOn,
                "1");
        }

        [Theory]
        [InlineData(ErrorCodingRate.FourFive)]
        [InlineData(ErrorCodingRate.FourSix)]
        [InlineData(ErrorCodingRate.FourSeven)]
        [InlineData(ErrorCodingRate.FourEight)]
        public void TestGetCodingRate(ErrorCodingRate expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ErrorCodingRate; },
                (v) => Assert.Equal(expected, v),
                Commands.GetErrorCodingRate,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(CrcWhiteningType.CrcCCITT)]
        [InlineData(CrcWhiteningType.CrcIbm)]
        public void TestGetCrcWhiteningType(CrcWhiteningType expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.CrcWhiteningType; },
                (v) => Assert.Equal(expected, v),
                Commands.GetCrcWhiteningType,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetFastHopOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FastHopOn; },
                Assert.True,
                Commands.GetFastHopOn,
                "1");
        }

        [Fact]
        public void TestGetFifoAddressPointer()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoAddressPointer; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetFifoAddressPointer,
                "0xAA");
        }

        [Fact]
        public void TestGetFifoRxBaseAddress()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxBaseAddress; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetFifoRxBaseAddress,
                "0xAA");
        }

        [Fact]
        public void TestGetFifoRxByteAddressPointer()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxByteAddressPointer; },
                (v) => Assert.Equal(0x20, v),
                Commands.GetFifoRxByteAddressPointer,
                "0x20");
        }

        [Fact]
        public void TestGetFifoRxBytesNumber()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxBytesNumber; },
                (v) => Assert.Equal(0x20, v),
                Commands.GetFifoRxBytesNumber,
                "0x20");
        }

        [Fact]
        public void TestGetFifoRxCurrentAddress()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoRxCurrentAddress; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetFifoRxCurrentAddress,
                "0xAA");
        }

        [Fact]
        public void TestGetFifoTxBaseAddress()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FifoTxBaseAddress; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetFifoTxBaseAddress,
                "0xAA");
        }

        [Fact]
        public void TestGetFormerTemperatureValue()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FormerTemperature; },
                (v) => Assert.Equal(0xA, v),
                Commands.GetFormerTemperatureValue,
                "0xA");
        }

        [Fact]
        public void TestGetFreqError()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FrequencyError; },
                (v) => Assert.Equal(0x2000, v),
                Commands.GetFreqError,
                "0x2000");
        }

        [Fact]
        public void TestGetFreqHoppingPeriod()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FrequencyHoppingPeriod; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetFreqHoppingPeriod,
                "0xAA");
        }

        [Fact]
        public void TestGetFromIdle()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromIdle; },
                Assert.True,
                Commands.GetFromIdle,
                "1");
        }

        [Theory]
        [InlineData(FromPacketReceived.ToLowPowerSelection)]
        [InlineData(FromPacketReceived.ToReceiveState)]
        [InlineData(FromPacketReceived.ToReceiveViaFSMode)]
        [InlineData(FromPacketReceived.ToSequencerOff)]
        [InlineData(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestGetFromPacketReceived(FromPacketReceived expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromPacketReceived; },
                (v) => Assert.Equal(expected, v),
                Commands.GetFromPacketReceived,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(FromReceive.ToSequencerOffOnPreambleDetect)]
        [InlineData(FromReceive.ToSequencerOffOnRssi)]
        [InlineData(FromReceive.ToSequencerOffOnSyncAddress)]
        [InlineData(FromReceive.ToPacketReceivedStateOnCrcOk)]
        [InlineData(FromReceive.ToLowPowerSelectionOnPayLoadReady)]
        public void TestGetFromReceived(FromReceive expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromReceive; },
                (v) => Assert.Equal(expected, v),
                Commands.GetFromReceive,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(FromRxTimeout.ToSequencerOff)]
        [InlineData(FromRxTimeout.ToLowPowerSelection)]
        [InlineData(FromRxTimeout.ToReceive)]
        [InlineData(FromRxTimeout.ToTransmit)]
        public void TestGetFromRxTimeout(FromRxTimeout expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromRxTimeout; },
                (v) => Assert.Equal(expected, v),
                Commands.GetFromRxTimeout,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(FromStart.ToLowPowerSelection)]
        [InlineData(FromStart.ToTransmitOnFifoLevel)]
        [InlineData(FromStart.ToTransmit)]
        [InlineData(FromStart.ToReceive)]
        public void TestGetFromStart(FromStart expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromStart; },
                (v) => Assert.Equal(expected, v),
                Commands.GetFromStart,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetFromTransmit()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.FromTransmit; },
                Assert.True,
                Commands.GetFromTransmit,
                "1");
        }

        [Theory]
        [InlineData(true, 0xAA)]
        [InlineData(false, 0x55)]
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

            Assert.Equal(state, result.PllTimeout);
            Assert.Equal(state, result.CrcOnPayload);
            Assert.Equal((byte)count, result.Channel);
        }

        [Fact]
        public void TestGetIdleMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IdleMode; },
                Assert.True,
                Commands.GetIdleMode,
                "1");
        }

        [Fact]
        public void TestGetImplicitHeaderModeOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ImplicitHeaderModeOn; },
                Assert.True,
                Commands.GetImplicitHeaderModeOn,
                "1");
        }

        [Fact]
        public void TestGetIoHomeOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IoHomeOn; },
                Assert.True,
                Commands.GetIoHomeOn,
                "1");
        }

        [Fact]
        public void TestGetIoHomePowerFrame()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.IoHomePowerFrame; },
                Assert.True,
                Commands.GetIoHomePowerFrame,
                "1");
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

            Assert.Equal(
                    Rfm9xIrqFlags.LowBattery |
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
                    Rfm9xIrqFlags.ModeReady, result);
        }

        [Fact]
        public void TestGetIrqFlagsMask()
        {
        }

        [Fact]
        public void TestGetLnaBoostHf()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LnaBoostHf; },
                Assert.True,
                Commands.GetLnaBoostHf,
                "1");
        }

        [Fact]
        public void TestGetLongRangeMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LongRangeMode; },
                Assert.True,
                Commands.GetLongRangeMode,
                "1");
        }

        [Fact]
        public void TestGetLoraAgcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LoraAgcAutoOn; },
                Assert.True,
                Commands.GetLoraAgcAutoOn,
                "1");
        }

        [Fact]
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

            Assert.Equal(
                LoraIrqFlags.CadDetected |
                LoraIrqFlags.FhssChangeChannel |
                LoraIrqFlags.CadDone |
                LoraIrqFlags.TxDone |
                LoraIrqFlags.ValidHeader |
                LoraIrqFlags.RxDone |
                LoraIrqFlags.RxTimeout, result);
        }

        [Fact]
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

            Assert.Equal(
                LoraIrqFlagsMask.CadDetectedMask |
                LoraIrqFlagsMask.FhssChangeChannelMask |
                LoraIrqFlagsMask.CadDoneMask |
                LoraIrqFlagsMask.TxDoneMask |
                LoraIrqFlagsMask.ValidHeaderMask |
                LoraIrqFlagsMask.RxDoneMask |
                LoraIrqFlagsMask.RxTimeoutMask, result);
        }

        [Theory]
        [InlineData(LoraMode.Cad)]
        [InlineData(LoraMode.RxContinuous)]
        [InlineData(LoraMode.RxSingle)]
        [InlineData(LoraMode.Sleep)]
        [InlineData(LoraMode.Standby)]
        [InlineData(LoraMode.SynthRx)]
        [InlineData(LoraMode.SynthTx)]
        [InlineData(LoraMode.Tx)]
        public void TestGetLoraMode(LoraMode expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LoraMode; },
                (v) => Assert.Equal(expected, v),
                Commands.GetLoraMode,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetLoraPayloadLength()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LoraPayloadLength; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetLoraPayloadLength,
                "0xAA");
        }

        [Fact]
        public void TestGetLowBatteryOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowBatteryOn; },
                Assert.True,
                Commands.GetLowBatteryOn,
                "1");
        }

        [Theory]
        [InlineData(LowBatteryTrim.Volts1_695)]
        [InlineData(LowBatteryTrim.Volts1_764)]
        [InlineData(LowBatteryTrim.Volts1_835)]
        [InlineData(LowBatteryTrim.Volts1_905)]
        [InlineData(LowBatteryTrim.Volts1_976)]
        [InlineData(LowBatteryTrim.Volts2_045)]
        [InlineData(LowBatteryTrim.Volts2_116)]
        [InlineData(LowBatteryTrim.Volts2_185)]
        public void TestGetLowBatteryTrim(LowBatteryTrim expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowBatteryTrim; },
                (v) => Assert.Equal(expected, v),
                Commands.GetLowBatteryTrim,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetLowDataRateOptimize()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowDataRateOptimize; },
                Assert.True,
                Commands.GetLowDataRateOptimize,
                "1");
        }

        [Fact]
        public void TestGetLowFrequencyMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowFrequencyMode; },
                Assert.True,
                Commands.GetLowFrequencyMode,
                "1");
        }

        [Fact]
        public void TestGetLowPowerSelection()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LowPowerSelection; },
                Assert.True,
                Commands.GetLowPowerSelection,
                "1");
        }

        [Fact]
        public void TestGetMapPreambleDetect()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.MapPreambleDetect; },
                Assert.True,
                Commands.GetMapPreambleDetect,
                "1");
        }

        [Theory]
        [InlineData(ModemBandwidth.Bandwidth10_4KHZ)]
        public void TestGetModemBandwidth(ModemBandwidth expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ModemBandwidth; },
                (v) => Assert.Equal(expected, v),
                Commands.GetModemBandwidth,
                $"0x{expected:X}");
        }

        [Fact]
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

            Assert.Equal(
                ModemStatus.SignalDetected |
                ModemStatus.SignalSynchronized |
                ModemStatus.RxOnGoing |
                ModemStatus.HeaderInfoValid |
                ModemStatus.ModemClear, result);
        }

        [Theory]
        [InlineData(OokAverageOffset.Offset0dB)]
        [InlineData(OokAverageOffset.Offset2dB)]
        [InlineData(OokAverageOffset.Offset4dB)]
        [InlineData(OokAverageOffset.Offset6dB)]
        public void TestGetOokAverageOffset(OokAverageOffset expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.OokAverageOffset; },
                (v) => Assert.Equal(expected, v),
                Commands.GetOokAverageOffset,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetOutputPower()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.OutputPower; },
                (v) => Assert.Equal(2, v),
                Commands.GetOutputPower,
                $"0x{(byte)2:X2}");
        }

        [Fact]
        public void TestGetPacketRssi()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PacketRssi; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetPacketRssi,
                "0xAA");
        }

        [Fact]
        public void TestGetPacketSnr()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.LastPacketSnr; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetLastPacketSnr,
                "0xAA");
        }

        [Fact]
        public void TestGetPayloadMaxLength()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PayloadMaxLength; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetPayloadMaxLength,
                "0xAA");
        }

        [Fact]
        public void TestGetPpmCorrection()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PpmCorrection; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetPpmCorrection,
                "0xAA");
        }

        [Fact]
        public void TestGetPreambleDetectorOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleDetectorOn; },
                Assert.True,
                Commands.GetPreambleDetectorOn,
                "1");
        }

        [Theory]
        [InlineData(PreambleDetectorSize.OneByte)]
        [InlineData(PreambleDetectorSize.TwoBytes)]
        [InlineData(PreambleDetectorSize.ThreeBytes)]
        public void TestGetPreambleDetectorSize(PreambleDetectorSize expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleDetectorSize; },
                (v) => Assert.Equal(expected, v),
                Commands.GetPreambleDetectorSize,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetPreambleDetectorTolerance()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleDetectorTolerance; },
                (v) => Assert.Equal(30, v),
                Commands.GetPreambleDetectorTolerance,
                $"0x{(sbyte)30:X2}");
        }

        [Fact]
        public void TestGetPreambleLength()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreambleLength; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetPreambleLength,
                "0xAA");
        }

        [Fact]
        public void TestGetPreamblePolarity()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.PreamblePolarity; },
                Assert.True,
                Commands.GetPreamblePolarity,
                "1");
        }

        [Fact]
        public void TestGetRestartRxOnCollision()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RestartRxOnCollision; },
                Assert.True,
                Commands.GetRestartRxOnCollision,
                "1");
        }

        [Fact]
        public void TestGetRssiCollisionThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiCollisionThreshold; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetRssiCollisionThreshold,
                "0xAA");
        }

        [Fact]
        public void TestGetRssiOffset()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiOffset; },
                (v) => Assert.Equal(-98, v),
                Commands.GetRssiOffset,
                $"0x{(sbyte)-98:X2}");
        }

        [Theory]
        [InlineData(RssiSmoothing.Samples2)]
        [InlineData(RssiSmoothing.Samples4)]
        [InlineData(RssiSmoothing.Samples8)]
        [InlineData(RssiSmoothing.Samples16)]
        [InlineData(RssiSmoothing.Samples32)]
        [InlineData(RssiSmoothing.Samples64)]
        [InlineData(RssiSmoothing.Samples128)]
        [InlineData(RssiSmoothing.Samples256)]
        public void TestGetRssiSmoothing(RssiSmoothing expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiSmoothing; },
                (v) => Assert.Equal(expected, v),
                Commands.GetRssiSmoothing,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiThreshold; },
                (v) => Assert.Equal(-114, v),
                Commands.GetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }

        [Fact]
        public void TestGetRssiWideband()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RssiWideband; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetRssiWideband,
                "0xAA");
        }

        [Fact]
        public void TestGetRxCodingRate()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RxCodingRate; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetRxCodingRate,
                "0xAA");
        }

        [Fact]
        public void TestGetRxPayloadCrcOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.RxPayloadCrcOn; },
                Assert.True,
                Commands.GetRxPayloadCrcOn,
                "1");
        }

        [Theory]
        [InlineData(SpreadingFactor.SpreadFactor64)]
        [InlineData(SpreadingFactor.SpreadFactor128)]
        [InlineData(SpreadingFactor.SpreadFactor512)]
        [InlineData(SpreadingFactor.SpreadFactor1024)]
        [InlineData(SpreadingFactor.SpreadFactor2048)]
        [InlineData(SpreadingFactor.SpreadFactor4096)]
        public void TestGetSpreadingFactor(SpreadingFactor expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SpreadingFactor; },
                (v) => Assert.Equal(expected, v),
                Commands.GetSpreadingFactor,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetSymbolTimeout()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.SymbolTimeout; },
                (v) => Assert.Equal(0x100, v),
                Commands.GetSymbolTimeout,
                "0x100");
        }

        [Fact]
        public void TestGetTcxoInputOn()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TcxoInputOn; },
                Assert.True,
                Commands.GetTcxoInputOn,
                "1");
        }

        [Theory]
        [InlineData(TemperatureThreshold.FiveDegrees)]
        [InlineData(TemperatureThreshold.TenDegrees)]
        [InlineData(TemperatureThreshold.FifteenDegrees)]
        [InlineData(TemperatureThreshold.TwentyDegrees)]
        public void TestGetTemperatureThreshold(TemperatureThreshold expected)
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TemperatureThreshold; },
                (v) => Assert.Equal(expected, v),
                Commands.GetTemperatureThreshold,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestGetTempMonitorOff()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TempMonitorOff; },
                Assert.True,
                Commands.GetTempMonitorOff,
                "1");
        }

        [Fact]
        public void TestGetTimeoutRxPreamble()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRxPreamble; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetTimeoutRxPreamble,
                "0xAA");
        }

        [Fact]
        public void TestGetTimeoutRxRssi()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutRxRssi; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetTimeoutRxRssi,
                "0xAA");
        }

        [Fact]
        public void TestGetTimeoutSignalSync()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TimeoutSignalSync; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetTimeoutSignalSync,
                "0xAA");
        }

        [Fact]
        public void TestGetTxContinuousMode()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.TxContinuousMode; },
                Assert.True,
                Commands.GetTxContinuousMode,
                "1");
        }

        [Fact]
        public void TestGetValidHeaderCount()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ValidHeaderCount; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetValidHeaderCount,
                "0xAA");
        }

        [Fact]
        public void TestGetValidPacketCount()
        {
            ExecuteGetTest(
                () => { return _rfmDevice.ValidPacketCount; },
                (v) => Assert.Equal(0xAA, v),
                Commands.GetValidPacketCount,
                "0xAA");
        }

        [Fact]
        public void TestOpen()
        {
            TestOpenVersion("RfmUsb-RFM9x FW: v3.0.3 HW: 2.0 433Mhz");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAccessSharedRegisters(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AccessSharedRegisters = value; },
                Commands.SetAccessSharedRegisters,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAesOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LoraAgcAutoOn = value; },
                Commands.SetLoraAgcAutoOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAgcAutoOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AgcAutoOn = value; },
                Commands.SetAgcAutoOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetAutoImageCalibrationOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoImageCalibrationOn = value; },
                Commands.SetAutoImageCalibrationOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(AutoRestartRxMode.Off)]
        [InlineData(AutoRestartRxMode.OnWaitForPllLock)]
        [InlineData(AutoRestartRxMode.OnWithoutPllRelock)]
        [InlineData(AutoRestartRxMode.Reserved)]
        public void TestSetAutoRestartRxMode(AutoRestartRxMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.AutoRestartRxMode = expected; },
                Commands.SetAutoRestartRxMode,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetBeaconOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.BeaconOn = value; },
                Commands.SetBeaconOn,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetBitRateFractional()
        {
            ExecuteSetTest(
                () => { _rfmDevice.BitRateFractional = 0x55; },
                Commands.SetBitRateFractional,
                "0x55");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetBitSyncOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.BitSyncOn = value; },
                Commands.SetBitSyncOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ErrorCodingRate.FourFive)]
        [InlineData(ErrorCodingRate.FourSix)]
        [InlineData(ErrorCodingRate.FourSeven)]
        [InlineData(ErrorCodingRate.FourEight)]
        public void TestSetCodingRate(ErrorCodingRate expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ErrorCodingRate = expected; },
                Commands.SetErrorCodingRate,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(CrcWhiteningType.CrcCCITT)]
        [InlineData(CrcWhiteningType.CrcIbm)]
        public void TestSetCrcWhiteningType(CrcWhiteningType expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.CrcWhiteningType = expected; },
                Commands.SetCrcWhiteningType,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetFastHopOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FastHopOn = value; },
                Commands.SetFastHopOn,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetFifoAddressPointer()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoAddressPointer = 0x55; },
                Commands.SetFifoAddressPointer,
                "0x55");
        }

        [Fact]
        public void TestSetFifoRxBaseAddress()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoRxBaseAddress = 0x55; },
                Commands.SetFifoRxBaseAddress,
                "0x55");
        }

        [Fact]
        public void TestSetFifoTxBaseAddress()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FifoTxBaseAddress = 0x55; },
                Commands.SetFifoTxBaseAddress,
                "0x55");
        }

        [Fact]
        public void TestSetFormerTemperatureValue()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FormerTemperature = 0x55; },
                Commands.SetFormerTemperatureValue,
                "0x55");
        }

        [Fact]
        public void TestSetFreqHoppingPeriod()
        {
            ExecuteSetTest(
                () => { _rfmDevice.FrequencyHoppingPeriod = 0x55; },
                Commands.SetFreqHoppingPeriod,
                "0x55");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetFromIdle(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromIdle = value; },
                Commands.SetFromIdle,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(FromPacketReceived.ToLowPowerSelection)]
        [InlineData(FromPacketReceived.ToReceiveState)]
        [InlineData(FromPacketReceived.ToReceiveViaFSMode)]
        [InlineData(FromPacketReceived.ToSequencerOff)]
        [InlineData(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestSetFromPacketReceived(FromPacketReceived expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromPacketReceived = expected; },
                Commands.SetFromPacketReceived,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(FromReceive.ToSequencerOffOnPreambleDetect)]
        [InlineData(FromReceive.ToSequencerOffOnRssi)]
        [InlineData(FromReceive.ToSequencerOffOnSyncAddress)]
        [InlineData(FromReceive.ToPacketReceivedStateOnCrcOk)]
        [InlineData(FromReceive.ToLowPowerSelectionOnPayLoadReady)]
        public void TestSetFromReceived(FromReceive expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromReceive = expected; },
                Commands.SetFromReceive,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(FromRxTimeout.ToSequencerOff)]
        [InlineData(FromRxTimeout.ToLowPowerSelection)]
        [InlineData(FromRxTimeout.ToReceive)]
        [InlineData(FromRxTimeout.ToTransmit)]
        public void TestSetFromRxTimeout(FromRxTimeout expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromRxTimeout = expected; },
                Commands.SetFromRxTimeout,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(FromStart.ToLowPowerSelection)]
        [InlineData(FromStart.ToTransmitOnFifoLevel)]
        [InlineData(FromStart.ToTransmit)]
        [InlineData(FromStart.ToReceive)]
        public void TestSetFromStart(FromStart expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromStart = expected; },
                Commands.SetFromStart,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetFromTransmit(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.FromTransmit = value; },
                Commands.SetFromTransmit,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetIdleMode(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.IdleMode = value; },
                Commands.SetIdleMode,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetImplicitHeaderModeOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ImplicitHeaderModeOn = value; },
                Commands.SetImplicitHeaderModeOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetIoHomeOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.IoHomeOn = value; },
                Commands.SetIoHomeOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetIoHomePowerFrame(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.IoHomePowerFrame = value; },
                Commands.SetIoHomePowerFrame,
                value ? "1" : "0");
        }

        [Fact]
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetLnaBoostHf(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LnaBoostHf = value; },
                Commands.SetLnaBoostHf,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetLongRangeMode(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LongRangeMode = value; },
                Commands.SetLongRangeMode,
                value ? "1" : "0");
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        public void TestSetLoraIrqFlagsMask()
        {
        }

        [Theory]
        [InlineData(LoraMode.Cad)]
        [InlineData(LoraMode.RxContinuous)]
        [InlineData(LoraMode.RxSingle)]
        [InlineData(LoraMode.Sleep)]
        [InlineData(LoraMode.Standby)]
        [InlineData(LoraMode.SynthRx)]
        [InlineData(LoraMode.SynthTx)]
        [InlineData(LoraMode.Tx)]
        public void TestSetLoraMode(LoraMode expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LoraMode = expected; },
                Commands.SetLoraMode,
                $"0x{(byte)expected:X2}");
        }

        [Fact]
        public void TestSetLoraPayloadLength()
        {
            ExecuteSetTest(
                () => { _rfmDevice.LoraPayloadLength = 0x55; },
                Commands.SetLoraPayloadLength,
                "0x55");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetLowBatteryOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowBatteryOn = value; },
                Commands.SetLowBatteryOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(LowBatteryTrim.Volts1_695)]
        [InlineData(LowBatteryTrim.Volts1_764)]
        [InlineData(LowBatteryTrim.Volts1_835)]
        [InlineData(LowBatteryTrim.Volts1_905)]
        [InlineData(LowBatteryTrim.Volts1_976)]
        [InlineData(LowBatteryTrim.Volts2_045)]
        [InlineData(LowBatteryTrim.Volts2_116)]
        [InlineData(LowBatteryTrim.Volts2_185)]
        public void TestSetLowBatteryTrim(LowBatteryTrim expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowBatteryTrim = expected; },
                Commands.SetLowBatteryTrim,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetLowDataRateOptimize(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowDataRateOptimize = value; },
                Commands.SetLowDataRateOptimize,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetLowFrequencyMode(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowFrequencyMode = value; },
                Commands.SetLowFrequencyMode,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetLowPowerSelection(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.LowPowerSelection = value; },
                Commands.SetLowPowerSelection,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetMapPreambleDetect(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.MapPreambleDetect = value; },
                Commands.SetMapPreambleDetect,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(ModemBandwidth.Bandwidth10_4KHZ)]
        public void TestSetModemBandwidth(ModemBandwidth expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.ModemBandwidth = expected; },
                Commands.SetModemBandwidth,
                $"0x{expected:X}");
        }

        [Theory]
        [InlineData(OokAverageOffset.Offset0dB)]
        [InlineData(OokAverageOffset.Offset2dB)]
        [InlineData(OokAverageOffset.Offset4dB)]
        [InlineData(OokAverageOffset.Offset6dB)]
        public void TestSetOokAverageOffset(OokAverageOffset expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.OokAverageOffset = expected; },
                Commands.SetOokAverageOffset,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestSetOutputPower()
        {
            ExecuteSetTest(
                () => { _rfmDevice.OutputPower = 2; },
                Commands.SetOutputPower,
                 $"0x{(byte)2:X2}");
        }

        [Fact]
        public void TestSetPayloadMaxLength()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PayloadMaxLength = 0x55; },
                Commands.SetPayloadMaxLength,
                "0x55");
        }

        [Fact]
        public void TestSetPpmCorrection()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PpmCorrection = 0x55; },
                Commands.SetPpmCorrection,
                "0x55");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetPreambleDetectorOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleDetectorOn = value; },
                Commands.SetPreambleDetectorOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(PreambleDetectorSize.OneByte)]
        [InlineData(PreambleDetectorSize.TwoBytes)]
        [InlineData(PreambleDetectorSize.ThreeBytes)]
        public void TestSetPreambleDetectorSize(PreambleDetectorSize expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleDetectorSize = expected; },
                Commands.SetPreambleDetectorSize,
                $"0x{(byte)expected:X2}");
        }

        [Fact]
        public void TestSetPreambleDetectorTolerance()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleDetectorTolerance = 55; },
                Commands.SetPreambleDetectorTolerance,
                $"0x{(sbyte)55:X2}");
        }

        [Fact]
        public void TestSetPreambleLength()
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreambleLength = 0xAA55; },
                Commands.SetPreambleLength,
                "0xAA55");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetPreamblePolarity(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.PreamblePolarity = value; },
                Commands.SetPreamblePolarity,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetRestartRxOnCollision(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.RestartRxOnCollision = value; },
                Commands.SetRestartRxOnCollision,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetRssiCollisionThreshold()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiCollisionThreshold = 0x55; },
                Commands.SetRssiCollisionThreshold,
                "0x55");
        }

        [Fact]
        public void TestSetRssiOffset()
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiOffset = 0x55; },
                Commands.SetRssiOffset,
                "0x55");
        }

        [Theory]
        [InlineData(RssiSmoothing.Samples2)]
        [InlineData(RssiSmoothing.Samples4)]
        [InlineData(RssiSmoothing.Samples8)]
        [InlineData(RssiSmoothing.Samples16)]
        [InlineData(RssiSmoothing.Samples32)]
        [InlineData(RssiSmoothing.Samples64)]
        [InlineData(RssiSmoothing.Samples128)]
        [InlineData(RssiSmoothing.Samples256)]
        public void TestSetRssiSmoothing(RssiSmoothing expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.RssiSmoothing = expected; },
                Commands.SetRssiSmoothing,
                $"0x{expected:X}");
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
        public void TestSetRxPayloadCrcOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.RxPayloadCrcOn = value; },
                Commands.SetRxPayloadCrcOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(SpreadingFactor.SpreadFactor64)]
        [InlineData(SpreadingFactor.SpreadFactor128)]
        [InlineData(SpreadingFactor.SpreadFactor512)]
        [InlineData(SpreadingFactor.SpreadFactor1024)]
        [InlineData(SpreadingFactor.SpreadFactor2048)]
        [InlineData(SpreadingFactor.SpreadFactor4096)]
        public void TestSetSpreadingFactor(SpreadingFactor expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.SpreadingFactor = expected; },
                Commands.SetSpreadingFactor,
                $"0x{expected:X}");
        }

        [Fact]
        public void TestSetSymbolTimeout()
        {
            ExecuteSetTest(
                () => { _rfmDevice.SymbolTimeout = 0x100; },
                Commands.SetSymbolTimeout,
                "0x100");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetTcxoInputOn(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.TcxoInputOn = value; },
                Commands.SetTcxoInputOn,
                value ? "1" : "0");
        }

        [Theory]
        [InlineData(TemperatureThreshold.FiveDegrees)]
        [InlineData(TemperatureThreshold.TenDegrees)]
        [InlineData(TemperatureThreshold.FifteenDegrees)]
        [InlineData(TemperatureThreshold.TwentyDegrees)]
        public void TestSetTemperatureThreshold(TemperatureThreshold expected)
        {
            ExecuteSetTest(
                () => { _rfmDevice.TemperatureThreshold = expected; },
                Commands.SetTemperatureThreshold,
                $"0x{(byte)expected:X2}");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetTempMonitorOff(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.TempMonitorOff = value; },
                Commands.SetTempMonitorOff,
                value ? "1" : "0");
        }

        [Fact]
        public void TestSetTimeoutRxPreamble()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRxPreamble = 0x55; },
                Commands.SetTimeoutRxPreamble,
                "0x55");
        }

        [Fact]
        public void TestSetTimeoutRxRssi()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutRxRssi = 0x55; },
                Commands.SetTimeoutRxRssi,
                "0x55");
        }

        [Fact]
        public void TestSetTimeoutSignalSync()
        {
            ExecuteSetTest(
                () => { _rfmDevice.TimeoutSignalSync = 0x55; },
                Commands.SetTimeoutSignalSync,
                "0x55");
        }

        [Theory]
        [InlineData(Timer.Timer1)]
        [InlineData(Timer.Timer2)]
        public void TestSetTimerCoefficient(Timer expected)
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

        [Theory]
        [InlineData(Timer.Timer1)]
        [InlineData(Timer.Timer2)]
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSetTxContinuousMode(bool value)
        {
            ExecuteSetTest(
                () => { _rfmDevice.TxContinuousMode = value; },
                Commands.SetTxContinuousMode,
                value ? "1" : "0");
        }
    }
}