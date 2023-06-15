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
        private readonly Rfm9x _rfm9x;

        public Rfm9xTests() : base()
        {
            _rfm9x = new Rfm9x(MockLogger, MockSerialPortFactory.Object);
            RfmBase = _rfm9x;
        }

        [TestMethod]
        public void ExecuteSequencerStart()
        {
            ExecuteTest(
                () => { _rfm9x.ExecuteSequencerStart(); },
                Commands.ExecuteSequencerStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        [DataRow(Timer.Timer1)]
        [DataRow(Timer.Timer2)]
        public void GetTimerCoefficient(Timer timer)
        {
            ExecuteGetTest(
                () => { return _rfm9x.GetTimerCoefficient(timer); },
                (v) => v.Should().Be(0x10),
                $"{Commands.GetTimerCoefficient} {(int)timer}",
                "0x10");
        }

        [TestMethod]
        [DataRow(Timer.Timer1, TimerResolution.Disabled)]
        [DataRow(Timer.Timer1, TimerResolution.Resolution64us)]
        [DataRow(Timer.Timer1, TimerResolution.Resolution4_1ms)]
        [DataRow(Timer.Timer1, TimerResolution.Resolution256ms)]
        [DataRow(Timer.Timer2, TimerResolution.Disabled)]
        [DataRow(Timer.Timer2, TimerResolution.Resolution64us)]
        [DataRow(Timer.Timer2, TimerResolution.Resolution4_1ms)]
        [DataRow(Timer.Timer2, TimerResolution.Resolution256ms)]
        public void GetTimerResolution(Timer timer, TimerResolution expected)
        {
            ExecuteGetTest(
                () => { return _rfm9x.GetTimerResolution(timer); },
                (v) => v.Should().Be(expected),
                $"{Commands.GetTimerResolution} {(int)timer}",
                ((int)expected).ToString());
        }

        [TestMethod]
        public void TestClearFifoOverrun()
        {
            ExecuteTest(
                () => { _rfm9x.ClearFifoOverrun(); },
                Commands.ExecuteClearFifoOverrun,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestClearLowBattery()
        {
            ExecuteTest(
                () => { _rfm9x.ClearLowBattery(); },
                Commands.ExecuteClearLowBattery,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestClearPreambleDetect()
        {
            ExecuteTest(
                () => { _rfm9x.ClearPreambleDetect(); },
                Commands.ExecuteClearPreambleDetect,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestClearSyncAddressMatch()
        {
            ExecuteTest(
                () => { _rfm9x.ClearSyncAddressMatch(); },
                Commands.ExecuteClearSyncAddressMatch,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteAgcStart()
        {
            ExecuteTest(
                () => { _rfm9x.ExecuteAgcStart(); },
                Commands.ExecuteAgcStart,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteImageCalibration()
        {
            ExecuteTest(
                () => { _rfm9x.ExecuteImageCalibration(); },
                Commands.ExecuteImageCalibration,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteRestartRxWithoutPllLock()
        {
            ExecuteTest(
                () => { _rfm9x.ExecuteRestartRxWithoutPllLock(); },
                Commands.ExecuteRestartRxWithoutPllLock,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteRestartRxWithPllLock()
        {
            ExecuteTest(
                () => { _rfm9x.ExecuteRestartRxWithPllLock(); },
                Commands.ExecuteRestartRxWithPllLock,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestExecuteSequencerStop()
        {
            ExecuteTest(
                () => { _rfm9x.ExecuteSequencerStop(); },
                Commands.ExecuteSequencerStop,
                RfmBase.ResponseOk);
        }

        [TestMethod]
        public void TestGetAutoImageCalibrationOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.AutoImageCalibrationOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetAutoImageCalibrationOn,
                "1");
        }

        [TestMethod]
        public void TestGetBeaconOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.BeaconOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetBeaconOn,
                "1");
        }

        [TestMethod]
        public void TestGetBitRateFractional()
        {
            ExecuteGetTest(
                () => { return _rfm9x.BitRateFractional; },
                (v) => v.Should().Be(0xAA),
                Commands.GetBitRateFractional,
                "0xAA");
        }

        [TestMethod]
        public void TestGetBitSyncOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.BitSyncOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetBitSyncOn,
                "1");
        }

        [TestMethod]
        [DataRow(CodingRate.FourFive)]
        [DataRow(CodingRate.FourSix)]
        [DataRow(CodingRate.FourSeven)]
        [DataRow(CodingRate.FourEight)]
        public void TestGetCodingRate(CodingRate expected)
        {
            ExecuteGetTest(
                () => { return _rfm9x.CodingRate; },
                (v) => { v.Should().Be(expected); },
                Commands.GetCodingRate,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetCrcWhiteningType()
        {
            ExecuteGetTest(
                () => { return _rfm9x.CrcWhiteningType; },
                (v) => v.Should().BeTrue(),
                Commands.GetCrcWhiteningType,
                "1");
        }

        [TestMethod]
        public void TestGetFastHopOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FastHopOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetFastHopOn,
                "1");
        }

        [TestMethod]
        public void TestGetFifoAddressPointer()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FifoAddressPointer; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoAddressPointer,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFifoRxBaseAddress()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FifoRxBaseAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoRxBaseAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFifoRxByteAddressPointer()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FifoRxByteAddressPointer; },
                (v) => v.Should().Be(0x20),
                Commands.GetFifoRxByteAddressPointer,
                "0x20");
        }

        [TestMethod]
        public void TestGetFifoRxBytesNumber()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FifoRxBytesNumber; },
                (v) => v.Should().Be(0x20),
                Commands.GetFifoRxBytesNumber,
                "0x20");
        }

        [TestMethod]
        public void TestGetFifoTxBaseAddress()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FifoTxBaseAddress; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFifoTxBaseAddress,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFormerTemperatureValue()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FormerTemperatureValue; },
                (v) => v.Should().Be((sbyte)0xA),
                Commands.GetFormerTemperatureValue,
                "0xA");
        }

        [TestMethod]
        public void TestGetFreqError()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FrequencyError; },
                (v) => v.Should().Be(0x2000),
                Commands.GetFreqError,
                "0x2000");
        }

        [TestMethod]
        public void TestGetFreqHoppingPeriod()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FrequencyHoppingPeriod; },
                (v) => v.Should().Be(0xAA),
                Commands.GetFreqHoppingPeriod,
                "0xAA");
        }

        [TestMethod]
        public void TestGetFromIdle()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FromIdle; },
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
                () => { return _rfm9x.FromPacketReceived; },
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
                () => { return _rfm9x.FromReceive; },
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
                () => { return _rfm9x.FromRxTimeout; },
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
                () => { return _rfm9x.FromStart; },
                (v) => { v.Should().Be(expected); },
                Commands.GetFromStart,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetFromTransmit()
        {
            ExecuteGetTest(
                () => { return _rfm9x.FromTransmit; },
                (v) => v.Should().BeTrue(),
                Commands.GetFromTransmit,
                "1");
        }

        [TestMethod]
        public void TestGetHopChannel()
        {
            ExecuteGetTest(
                () => { return _rfm9x.HopChannel; },
                (v) => v.Should().Be(0xAA),
                Commands.GetHopChannel,
                "0xAA");
        }

        [TestMethod]
        public void TestGetIdleMode()
        {
            ExecuteGetTest(
                () => { return _rfm9x.IdleMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetIdleMode,
                "1");
        }

        [TestMethod]
        public void TestGetImplicitHeaderModeOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.ImplicitHeaderModeOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetImplicitHeaderModeOn,
                "1");
        }

        [TestMethod]
        public void TestGetIoHomeOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.IoHomeOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetIoHomeOn,
                "1");
        }

        [TestMethod]
        public void TestGetIoHomePowerFrame()
        {
            ExecuteGetTest(
                () => { return _rfm9x.IoHomePowerFrame; },
                (v) => v.Should().BeTrue(),
                Commands.GetIoHomePowerFrame,
                "1");
        }

        [TestMethod]
        public void TestGetLnaBoostHf()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LnaBoostHf; },
                (v) => v.Should().BeTrue(),
                Commands.GetLnaBoostHf,
                "1");
        }

        [TestMethod]
        public void TestGetLongRangeMode()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LongRangeMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetLongRangeMode,
                "1");
        }

        [TestMethod]
        public void TestGetLoraAgcAutoOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LoraAgcAutoOn; },
                (v) => v.Should().BeTrue(),
                Commands.GetLoraAgcAutoOn,
                "1");
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
                () => { return _rfm9x.LoraMode; },
                (v) => { v.Should().Be(expected); },
                Commands.GetLoraMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetLowBatteryOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LowBatteryOn; },
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
                () => { return _rfm9x.LowBatteryTrim; },
                (v) => { v.Should().Be(expected); },
                Commands.GetLowBatteryTrim,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetLowDataRateOptimize()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LowDataRateOptimize; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowDataRateOptimize,
                "1");
        }

        [TestMethod]
        public void TestGetLowFrequencyMode()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LowFrequencyMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowFrequencyMode,
                "1");
        }

        [TestMethod]
        public void TestGetLowPowerSelection()
        {
            ExecuteGetTest(
                () => { return _rfm9x.LowPowerSelection; },
                (v) => v.Should().BeTrue(),
                Commands.GetLowPowerSelection,
                "1");
        }

        [TestMethod]
        public void TestGetMapPreambleDetect()
        {
            ExecuteGetTest(
                () => { return _rfm9x.MapPreambleDetect; },
                (v) => v.Should().BeTrue(),
                Commands.GetMapPreambleDetect,
                "1");
        }

        [TestMethod]
        [DataRow(ModemBandwidth.Bandwidth10_4KHZ)]
        public void TestGetModemBandwidth(ModemBandwidth expected)
        {
            ExecuteGetTest(
                () => { return _rfm9x.ModemBandwidth; },
                (v) => { v.Should().Be(expected); },
                Commands.GetModemBandwidth,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(ModemStatus.SignalDetected)]
        [DataRow(ModemStatus.SignalSynchronized)]
        [DataRow(ModemStatus.HeaderInfoValid)]
        [DataRow(ModemStatus.StatusRxOnGoing)]
        [DataRow(ModemStatus.ModemClear)]
        public void TestGetModemStatus(ModemStatus expected)
        {
            ExecuteGetTest(
                 () => { return _rfm9x.ModemStatus; },
                 (v) => { v.Should().Be(expected); },
                 Commands.GetModemStatus,
                 $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(OokAverageOffset.Offset0dB)]
        [DataRow(OokAverageOffset.Offset2dB)]
        [DataRow(OokAverageOffset.Offset4dB)]
        [DataRow(OokAverageOffset.Offset6dB)]
        public void TestGetOokAverageOffset(OokAverageOffset expected)
        {
            ExecuteGetTest(
                () => { return _rfm9x.OokAverageOffset; },
                (v) => { v.Should().Be(expected); },
                Commands.GetOokAverageOffset,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPacketRssi()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PacketRssi; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPacketRssi,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPacketSnr()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PacketSnr; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPacketSnr,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPayloadMaxLength()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PayloadMaxLength; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPayloadMaxLength,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPpmCorrection()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PpmCorrection; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPpmCorrection,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPreambleDetectorOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PreambleDetectorOn; },
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
                () => { return _rfm9x.PreambleDetectorSize; },
                (v) => { v.Should().Be(expected); },
                Commands.GetPreambleDetectorSize,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetPreambleDetectorTotal()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PreambleDetectorTotal; },
                (v) => v.Should().Be(0xAA),
                Commands.GetPreambleDetectorTotal,
                "0xAA");
        }

        [TestMethod]
        public void TestGetPreambleLength()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PreambleSize; },
                (v) => v.Should().Be(0x100),
                Commands.GetPreambleLength,
                "0x100");
        }

        [TestMethod]
        public void TestGetPreamblePolarity()
        {
            ExecuteGetTest(
                () => { return _rfm9x.PreamblePolarity; },
                (v) => v.Should().BeTrue(),
                Commands.GetPreamblePolarity,
                "1");
        }

        [TestMethod]
        public void TestGetRestartRxOnCollision()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RestartRxOnCollision; },
                (v) => v.Should().BeTrue(),
                Commands.GetRestartRxOnCollision,
                "1");
        }

        [TestMethod]
        public void TestGetRssiCollisionThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RssiCollisionThreshold; },
                (v) => v.Should().Be(0xAA),
                Commands.GetRssiCollisionThreshold,
                "0xAA");
        }

        [TestMethod]
        public void TestGetRssiOffset()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RssiOffset; },
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
                () => { return _rfm9x.RssiSmoothing; },
                (v) => { v.Should().Be(expected); },
                Commands.GetRssiSmoothing,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetRssiThreshold()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RssiThreshold; },
                (v) => v.Should().Be(-114),
                Commands.GetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }

        [TestMethod]
        public void TestGetRssiWideband()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RssiWideband; },
                (v) => v.Should().Be(0xAA),
                Commands.GetRssiWideband,
                "0xAA");
        }

        [TestMethod]
        public void TestGetRxCodingRate()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RxCodingRate; },
                (v) => v.Should().Be(0xAA),
                Commands.GetRxCodingRate,
                "0xAA");
        }

        [TestMethod]
        public void TestGetRxPayloadCrcOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.RxPayloadCrcOn; },
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
                () => { return _rfm9x.SpreadingFactor; },
                (v) => { v.Should().Be(expected); },
                Commands.GetSpreadingFactor,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetSymbolTimeout()
        {
            ExecuteGetTest(
                () => { return _rfm9x.SymbolTimeout; },
                (v) => v.Should().Be(0x100),
                Commands.GetSymbolTimeout,
                "0x100");
        }

        [TestMethod]
        public void TestGetTcxoInputOn()
        {
            ExecuteGetTest(
                () => { return _rfm9x.TcxoInputOn; },
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
                () => { return _rfm9x.TemperatureThreshold; },
                (v) => { v.Should().Be(expected); },
                Commands.GetTemperatureThreshold,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestGetTempMonitorOff()
        {
            ExecuteGetTest(
                () => { return _rfm9x.TempMonitorOff; },
                (v) => v.Should().BeTrue(),
                Commands.GetTempMonitorOff,
                "1");
        }

        [TestMethod]
        public void TestGetTimeoutRxPreamble()
        {
            ExecuteGetTest(
                () => { return _rfm9x.TimeoutRxPreamble; },
                (v) => v.Should().Be(0xAA),
                Commands.GetTimeoutRxPreamble,
                "0xAA");
        }

        [TestMethod]
        public void TestGetTimeoutRxRssi()
        {
            ExecuteGetTest(
                () => { return _rfm9x.TimeoutRxRssi; },
                (v) => v.Should().Be(0xAA),
                Commands.GetTimeoutRxRssi,
                "0xAA");
        }

        [TestMethod]
        public void TestGetTimeoutSignalSync()
        {
            ExecuteGetTest(
                () => { return _rfm9x.TimeoutSignalSync; },
                (v) => v.Should().Be(0xAA),
                Commands.GetTimeoutSignalSync,
                "0xAA");
        }

        [TestMethod]
        public void TestGetTxContinuousMode()
        {
            ExecuteGetTest(
                () => { return _rfm9x.TxContinuousMode; },
                (v) => v.Should().BeTrue(),
                Commands.GetTxContinuousMode,
                "1");
        }

        [TestMethod]
        public void TestGetValidHeaderCount()
        {
            ExecuteGetTest(
                () => { return _rfm9x.ValidHeaderCount; },
                (v) => v.Should().Be(0xAA),
                Commands.GetValidHeaderCount,
                "0xAA");
        }

        [TestMethod]
        public void TestGetValidPacketCount()
        {
            ExecuteGetTest(
                () => { return _rfm9x.ValidPacketCount; },
                (v) => v.Should().Be(0xAA),
                Commands.GetValidPacketCount,
                "0xAA");
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
                .Returns("RfmUsb-RFM9x FW: v3.0.3 HW: 2.0 433Mhz");

            // Act
            _rfm9x.Open("ComPort", 9600);

            // Assert
            MockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            MockSerialPort.Verify(_ => _.Open(), Times.Once);
        }

        [TestMethod]
        public void TestSetAesOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.LoraAgcAutoOn = true; },
                Commands.SetLoraAgcAutoOn,
                "1");
        }

        [TestMethod]
        public void TestSetAutoImageCalibrationOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.AutoImageCalibrationOn = true; },
                Commands.SetAutoImageCalibrationOn,
                "1");
        }

        [TestMethod]
        public void TestSetBeaconOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.BeaconOn = true; },
                Commands.SetBeaconOn,
                "1");
        }

        [TestMethod]
        public void TestSetBitRateFractional()
        {
            ExecuteSetTest(
                () => { _rfm9x.BitRateFractional = 0x55; },
                Commands.SetBitRateFractional,
                "0x55");
        }

        [TestMethod]
        public void TestSetBitSyncOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.BitSyncOn = true; },
                Commands.SetBitSyncOn,
                "1");
        }

        [TestMethod]
        public void TestSetCrcWhiteningType()
        {
            ExecuteSetTest(
                () => { _rfm9x.CrcWhiteningType = true; },
                Commands.SetCrcWhiteningType,
                "1");
        }

        [TestMethod]
        public void TestSetFastHopOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.FastHopOn = true; },
                Commands.SetFastHopOn,
                "1");
        }

        [TestMethod]
        public void TestSetFifoAddressPointer()
        {
            ExecuteSetTest(
                () => { _rfm9x.FifoAddressPointer = 0x55; },
                Commands.SetFifoAddressPointer,
                "0x55");
        }

        [TestMethod]
        public void TestSetFifoRxBaseAddress()
        {
            ExecuteSetTest(
                () => { _rfm9x.FifoRxBaseAddress = 0x55; },
                Commands.SetFifoRxBaseAddress,
                "0x55");
        }

        [TestMethod]
        public void TestSetFifoTxBaseAddress()
        {
            ExecuteSetTest(
                () => { _rfm9x.FifoTxBaseAddress = 0x55; },
                Commands.SetFifoTxBaseAddress,
                "0x55");
        }

        [TestMethod]
        public void TestSetFormerTemperatureValue()
        {
            ExecuteSetTest(
                () => { _rfm9x.FormerTemperatureValue = 0x55; },
                Commands.SetFormerTemperatureValue,
                "0x55");
        }

        [TestMethod]
        public void TestSetFreqHoppingPeriod()
        {
            ExecuteSetTest(
                () => { _rfm9x.FrequencyHoppingPeriod = 0x55; },
                Commands.SetFreqHoppingPeriod,
                "0x55");
        }

        [TestMethod]
        public void TestSetFromIdle()
        {
            ExecuteSetTest(
                () => { _rfm9x.FromIdle = true; },
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
                () => { _rfm9x.FromPacketReceived = expected; },
                Commands.SetFromPacketReceived,
                $"0x{expected:X}");
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
                () => { _rfm9x.FromReceive = expected; },
                Commands.SetFromReceive,
                $"0x{expected:X}");
        }

        [TestMethod]
        [DataRow(FromRxTimeout.ToSequencerOff)]
        [DataRow(FromRxTimeout.ToLowPowerSelection)]
        [DataRow(FromRxTimeout.ToReceive)]
        [DataRow(FromRxTimeout.ToTransmit)]
        public void TestSetFromRxTimeout(FromRxTimeout expected)
        {
            ExecuteSetTest(
                () => { _rfm9x.FromRxTimeout = expected; },
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
                () => { _rfm9x.FromStart = expected; },
                Commands.SetFromStart,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetFromTransmit()
        {
            ExecuteSetTest(
                () => { _rfm9x.FromTransmit = true; },
                Commands.SetFromTransmit,
                "1");
        }

        [TestMethod]
        public void TestSetIdleMode()
        {
            ExecuteSetTest(
                () => { _rfm9x.IdleMode = true; },
                Commands.SetIdleMode,
                "1");
        }

        [TestMethod]
        public void TestSetImplicitHeaderModeOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.ImplicitHeaderModeOn = true; },
                Commands.SetImplicitHeaderModeOn,
                "1");
        }

        [TestMethod]
        public void TestSetIoHomeOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.IoHomeOn = true; },
                Commands.SetIoHomeOn,
                "1");
        }

        [TestMethod]
        public void TestSetIoHomePowerFrame()
        {
            ExecuteSetTest(
                () => { _rfm9x.IoHomePowerFrame = true; },
                Commands.SetIoHomePowerFrame,
                "1");
        }

        [TestMethod]
        public void TestSetLnaBoostHf()
        {
            ExecuteSetTest(
                () => { _rfm9x.LnaBoostHf = true; },
                Commands.SetLnaBoostHf,
                "1");
        }

        [TestMethod]
        public void TestSetLongRangeMode()
        {
            ExecuteSetTest(
                () => { _rfm9x.LongRangeMode = true; },
                Commands.SetLongRangeMode,
                "1");
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
                () => { _rfm9x.LoraMode = expected; },
                Commands.SetLoraMode,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetLowBatteryOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.LowBatteryOn = true; },
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
                () => { _rfm9x.LowBatteryTrim = expected; },
                Commands.SetLowBatteryTrim,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetLowDataRateOptimize()
        {
            ExecuteSetTest(
                () => { _rfm9x.LowDataRateOptimize = true; },
                Commands.SetLowDataRateOptimize,
                "1");
        }

        [TestMethod]
        public void TestSetLowFrequencyMode()
        {
            ExecuteSetTest(
                () => { _rfm9x.LowFrequencyMode = true; },
                Commands.SetLowFrequencyMode,
                "1");
        }

        [TestMethod]
        public void TestSetLowPowerSelection()
        {
            ExecuteSetTest(
                () => { _rfm9x.LowPowerSelection = true; },
                Commands.SetLowPowerSelection,
                "1");
        }

        [TestMethod]
        public void TestSetMapPreambleDetect()
        {
            ExecuteSetTest(
                () => { _rfm9x.MapPreambleDetect = true; },
                Commands.SetMapPreambleDetect,
                "1");
        }

        [TestMethod]
        [DataRow(ModemBandwidth.Bandwidth10_4KHZ)]
        public void TestSetModemBandwidth(ModemBandwidth expected)
        {
            ExecuteSetTest(
    () => { _rfm9x.ModemBandwidth = expected; },
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
                () => { _rfm9x.OokAverageOffset = expected; },
                Commands.SetOokAverageOffset,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetPayloadMaxLength()
        {
            ExecuteSetTest(
                () => { _rfm9x.PayloadMaxLength = 0x55; },
                Commands.SetPayloadMaxLength,
                "0x55");
        }

        [TestMethod]
        public void TestSetPpmCorrection()
        {
            ExecuteSetTest(
                () => { _rfm9x.PpmCorrection = 0x55; },
                Commands.SetPpmCorrection,
                "0x55");
        }

        [TestMethod]
        public void TestSetPreambleDetectorOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.PreambleDetectorOn = true; },
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
                () => { _rfm9x.PreambleDetectorSize = expected; },
                Commands.SetPreambleDetectorSize,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetPreambleDetectorTotal()
        {
            ExecuteSetTest(
                () => { _rfm9x.PreambleDetectorTotal = 0x55; },
                Commands.SetPreambleDetectorTotal,
                "0x55");
        }

        [TestMethod]
        public void TestSetPreambleLength()
        {
            ExecuteSetTest(
                () => { _rfm9x.PreambleSize = 0x100; },
                Commands.SetPreambleLength,
                "0x100");
        }

        [TestMethod]
        public void TestSetPreamblePolarity()
        {
            ExecuteSetTest(
                () => { _rfm9x.PreamblePolarity = true; },
                Commands.SetPreamblePolarity,
                "1");
        }

        [TestMethod]
        public void TestSetRestartRxOnCollision()
        {
            ExecuteSetTest(
                () => { _rfm9x.RestartRxOnCollision = true; },
                Commands.SetRestartRxOnCollision,
                "1");
        }

        [TestMethod]
        public void TestSetRssiCollisionThreshold()
        {
            ExecuteSetTest(
                () => { _rfm9x.RssiCollisionThreshold = 0x55; },
                Commands.SetRssiCollisionThreshold,
                "0x55");
        }

        [TestMethod]
        public void TestSetRssiOffset()
        {
            ExecuteSetTest(
                () => { _rfm9x.RssiOffset = 0x55; },
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
                () => { _rfm9x.RssiSmoothing = expected; },
                Commands.SetRssiSmoothing,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetRssiThreshold()
        {
            ExecuteSetTest(
                () => { _rfm9x.RssiThreshold = -114; },
                Commands.SetRssiThreshold,
                $"0x{(sbyte)-114:X2}");
        }
        [TestMethod]
        public void TestSetRxPayloadCrcOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.RxPayloadCrcOn = true; },
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
                () => { _rfm9x.SpreadingFactor = expected; },
                Commands.SetSpreadingFactor,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetSymbolTimeout()
        {
            ExecuteSetTest(
                () => { _rfm9x.SymbolTimeout = 0x100; },
                Commands.SetSymbolTimeout,
                "0x100");
        }

        [TestMethod]
        public void TestSetTcxoInputOn()
        {
            ExecuteSetTest(
                () => { _rfm9x.TcxoInputOn = true; },
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
                () => { _rfm9x.TemperatureThreshold = expected; },
                Commands.SetTemperatureThreshold,
                $"0x{expected:X}");
        }

        [TestMethod]
        public void TestSetTempMonitorOff()
        {
            ExecuteSetTest(
                () => { _rfm9x.TempMonitorOff = true; },
                Commands.SetTempMonitorOff,
                "1");
        }

        [TestMethod]
        public void TestSetTimeoutRxPreamble()
        {
            ExecuteSetTest(
                () => { _rfm9x.TimeoutRxPreamble = 0x55; },
                Commands.SetTimeoutRxPreamble,
                "0x55");
        }

        [TestMethod]
        public void TestSetTimeoutRxRssi()
        {
            ExecuteSetTest(
                () => { _rfm9x.TimeoutRxRssi = 0x55; },
                Commands.SetTimeoutRxRssi,
                "0x55");
        }

        [TestMethod]
        public void TestSetTimeoutSignalSync()
        {
            ExecuteSetTest(
                () => { _rfm9x.TimeoutSignalSync = 0x55; },
                Commands.SetTimeoutSignalSync,
                "0x55");
        }

        [TestMethod]
        public void TestSetTxContinuousMode()
        {
            ExecuteSetTest(
                () => { _rfm9x.TxContinuousMode = true; },
                Commands.SetTxContinuousMode,
                "1");
        }
    }
}
