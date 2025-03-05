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


// Ignore Spelling: Agc Bw Fsk Io Irq Lna Ook Pll Rfm Rssi Rx Tcxo

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RfmUsb.Net.IntTests
{
    public class Rfm9xFskTests : RfmTestCommon
    {
        private readonly IRfm9x _rfm9x;

        public Rfm9xFskTests()
        {
            _rfm9x = ServiceProvider.GetService<IRfm9x>() ?? throw new NullReferenceException($"Unable to resolve {nameof(IRfm9x)}");
            RfmBase = _rfm9x;

#warning TODO
            //_rfm9x.Open((string)TestContext.Properties["Rfm9xComPort"], int.Parse((string)TestContext.Properties["BaudRate"]));

            _rfm9x.ExecuteReset();
        }

        [Theory]
        [InlineData(Timer.Timer1, TimerResolution.Reserved)]
        [InlineData(Timer.Timer2, TimerResolution.Reserved)]
        [InlineData(Timer.Timer1, TimerResolution.Resolution64us)]
        [InlineData(Timer.Timer2, TimerResolution.Resolution64us)]
        [InlineData(Timer.Timer1, TimerResolution.Resolution4_1ms)]
        [InlineData(Timer.Timer2, TimerResolution.Resolution4_1ms)]
        [InlineData(Timer.Timer1, TimerResolution.Resolution256ms)]
        [InlineData(Timer.Timer2, TimerResolution.Resolution256ms)]
        public void SetTimerResolution(Timer timer, TimerResolution expected)
        {
            _rfm9x.SetTimerResolution(timer, expected);

            Assert.Equal(expected, _rfm9x.GetTimerResolution(timer));
        }

        [Fact]
        public void TestAgcAutoOn()
        {
            TestRangeBool(() => _rfm9x.AgcAutoOn, (v) => _rfm9x.AgcAutoOn = v);
        }

        [Fact]
        public void TestAutoImageCalibrationOn()
        {
            TestRangeBool(() => _rfm9x.AutoImageCalibrationOn, (v) => _rfm9x.AutoImageCalibrationOn = v);
        }

        [Fact]
        public void TestBitRateFractional()
        {
            TestRange(() => _rfm9x.BitRateFractional, (v) => _rfm9x.BitRateFractional = (byte)v, 0, 0x0F);
        }

        [Fact]
        public void TestBitSyncOn()
        {
            TestRangeBool(() => _rfm9x.BitSyncOn, (v) => _rfm9x.BitSyncOn = v);
        }

        [Theory]
        [InlineData(CrcWhiteningType.CrcCCITT)]
        [InlineData(CrcWhiteningType.CrcIbm)]
        public void TestCrcWhiteningType(CrcWhiteningType expected)
        {
            TestAssignedValue(expected, () => _rfm9x.CrcWhiteningType, (v) => _rfm9x.CrcWhiteningType = v);
        }

        [Fact]
        public void TestExecuteAgcStart()
        {
            _rfm9x.ExecuteAgcStart();
        }

        [Fact]
        public void TestExecuteImageCalibration()
        {
            _rfm9x.ExecuteImageCalibration();
        }

        [Fact]
        public void TestExecuteRestartRxWithoutPllLock()
        {
            _rfm9x.ExecuteRestartRxWithoutPllLock();
        }

        [Fact]
        public void TestExecuteRestartRxWithPllLock()
        {
            _rfm9x.ExecuteRestartRxWithPllLock();
        }

        [Fact]
        public void TestExecuteSequencerStart()
        {
            _rfm9x.ExecuteSequencerStart();
        }

        [Fact]
        public void TestExecuteSequencerStop()
        {
            _rfm9x.ExecuteSequencerStop();
        }

        [Fact]
        public void TestFastHopOn()
        {
            TestRangeBool(() => _rfm9x.FastHopOn, (v) => _rfm9x.FastHopOn = v);
        }

        [Fact]
        public void TestFifo()
        {
            var expected = RandomSequence().Take(64).ToList();
            _rfm9x.Fifo = expected;

            Assert.Equal(expected, _rfm9x.Fifo);
        }

        [Fact]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 63);
        }

        [Fact(Skip = "not implemented in device")]
        public void TestFormerTemperatureValue()
        {
            TestRange(() => _rfm9x.FormerTemperature, (v) => _rfm9x.FormerTemperature = v);
        }

        [Fact]
        public void TestFromIdle()
        {
            TestRangeBool(() => _rfm9x.FromIdle, (v) => _rfm9x.FromIdle = v);
        }

        [Theory]
        [InlineData(FromPacketReceived.ToLowPowerSelection)]
        [InlineData(FromPacketReceived.ToReceiveState)]
        [InlineData(FromPacketReceived.ToReceiveViaFSMode)]
        [InlineData(FromPacketReceived.ToSequencerOff)]
        [InlineData(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestFromPacketReceived(FromPacketReceived expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromPacketReceived, (v) => _rfm9x.FromPacketReceived = v);
        }

        [Theory]
        [InlineData(FromReceive.ToLowPowerSelectionOnPayLoadReady)]
        [InlineData(FromReceive.ToPacketReceivedOnPayloadReady)]
        [InlineData(FromReceive.ToPacketReceivedStateOnCrcOk)]
        [InlineData(FromReceive.ToSequencerOffOnPreambleDetect)]
        [InlineData(FromReceive.ToSequencerOffOnRssi)]
        [InlineData(FromReceive.ToSequencerOffOnSyncAddress)]
        [InlineData(FromReceive.UnusedA)]
        [InlineData(FromReceive.UnusedB)]
        public void TestFromReceive(FromReceive expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromReceive, (v) => _rfm9x.FromReceive = v);
        }

        [Theory]
        [InlineData(FromRxTimeout.ToLowPowerSelection)]
        [InlineData(FromRxTimeout.ToReceive)]
        [InlineData(FromRxTimeout.ToSequencerOff)]
        [InlineData(FromRxTimeout.ToTransmit)]
        public void TestFromRxTimeout(FromRxTimeout expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromRxTimeout, (v) => _rfm9x.FromRxTimeout = v);
        }

        [Theory]
        [InlineData(FromStart.ToLowPowerSelection)]
        [InlineData(FromStart.ToReceive)]
        [InlineData(FromStart.ToTransmit)]
        [InlineData(FromStart.ToTransmitOnFifoLevel)]
        public void TestFromStart(FromStart expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromStart, (v) => _rfm9x.FromStart = v);
        }

        [Theory]
        [InlineData(OokAverageOffset.Offset0dB)]
        [InlineData(OokAverageOffset.Offset2dB)]
        [InlineData(OokAverageOffset.Offset4dB)]
        [InlineData(OokAverageOffset.Offset6dB)]
        public void TestOokAverageOffset(OokAverageOffset expected)
        {
            TestAssignedValue(expected, () => _rfm9x.OokAverageOffset, (v) => _rfm9x.OokAverageOffset = v);
        }

        [Fact]
        public void TestFromTransmit()
        {
            TestRangeBool(() => _rfm9x.FromTransmit, (v) => _rfm9x.FromTransmit = v);
        }

        [Fact]
        public void TestGetIrq()
        {
            _rfm9x.ExecuteReset();
            Assert.Equal(Rfm9xIrqFlags.ModeReady, _rfm9x.IrqFlags);
        }

        [Fact]
        public void TestIdleMode()
        {
            TestRangeBool(() => _rfm9x.IdleMode, (v) => _rfm9x.IdleMode = v);
        }

        [Fact]
        public void TestIoHomeOn()
        {
            TestRangeBool(() => _rfm9x.IoHomeOn, (v) => _rfm9x.IoHomeOn = v);
        }

        [Fact]
        public void TestIoHomePowerFrame()
        {
            TestRangeBool(() => _rfm9x.IoHomePowerFrame, (v) => _rfm9x.IoHomePowerFrame = v);
        }

        [Fact]
        public void TestLnaBoostHf()
        {
            TestRangeBool(() => _rfm9x.LnaBoostHf, (v) => _rfm9x.LnaBoostHf = v);
        }

        [Fact]
        public void TestLowBatteryOn()
        {
            TestRangeBool(() => _rfm9x.LowBatteryOn, (v) => _rfm9x.LowBatteryOn = v);
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
        public void TestLowBatteryTrim(LowBatteryTrim expected)
        {
            TestAssignedValue(expected, () => _rfm9x.LowBatteryTrim, (v) => _rfm9x.LowBatteryTrim = v);
        }

        [Fact]
        public void TestLowFrequencyMode()
        {
            TestRangeBool(() => _rfm9x.LowFrequencyMode, (v) => _rfm9x.LowFrequencyMode = v);
        }

        [Fact]
        public void TestLowPowerSelection()
        {
            TestRangeBool(() => _rfm9x.LowPowerSelection, (v) => _rfm9x.LowPowerSelection = v);
        }

        [Fact]
        public void TestMapPreambleDetect()
        {
            TestRangeBool(() => _rfm9x.MapPreambleDetect, (v) => _rfm9x.MapPreambleDetect = v);
        }

        [Fact]
        public void TestOutputPower()
        {
            TestRange<byte>(() => _rfm9x.OutputPower, (v) => _rfm9x.OutputPower = v, 2, 20);
        }
        [Fact]
        public void TestPreambleDetectorOn()
        {
            TestRangeBool(() => _rfm9x.PreambleDetectorOn, (v) => _rfm9x.PreambleDetectorOn = v);
        }

        [Theory]
        [InlineData(PreambleDetectorSize.OneByte)]
        [InlineData(PreambleDetectorSize.ThreeBytes)]
        [InlineData(PreambleDetectorSize.TwoBytes)]
        public void TestPreambleDetectorSize(PreambleDetectorSize expected)
        {
            TestAssignedValue(expected, () => _rfm9x.PreambleDetectorSize, (v) => _rfm9x.PreambleDetectorSize = v);
        }

        [Fact]
        public void TestPreambleDetectorTotal()
        {
            TestRange<byte>(() => _rfm9x.PreambleDetectorTolerance, (v) => _rfm9x.PreambleDetectorTolerance = v, 0, 0x0F);
        }

        [Fact]
        public void TestPreamblePolarity()
        {
            TestRangeBool(() => _rfm9x.PreamblePolarity, (v) => _rfm9x.PreamblePolarity = v);
        }

        [Fact]
        public void TestRestartRxOnCollision()
        {
            TestRangeBool(() => _rfm9x.RestartRxOnCollision, (v) => _rfm9x.RestartRxOnCollision = v);
        }

        [Fact]
        public void TestRssiCollisionThreshold()
        {
            TestRange(() => _rfm9x.RssiCollisionThreshold, (v) => _rfm9x.RssiCollisionThreshold = v);
        }

        [Fact]
        public void TestRssiOffset()
        {
            TestRange(() => _rfm9x.RssiOffset, (v) => _rfm9x.RssiOffset = (sbyte)v, -16, 15);
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
        public void TestRssiSmoothing(RssiSmoothing expected)
        {
            TestAssignedValue(expected, () => _rfm9x.RssiSmoothing, (v) => _rfm9x.RssiSmoothing = v);
        }

        [Fact]
        public void TestRssiThreshold()
        {
            TestRange(() => _rfm9x.RssiThreshold, (v) => _rfm9x.RssiThreshold = (sbyte)v, 0, -127);
        }

        [Fact]
        public void TestRxBw()
        {
            TestRange<byte>(() => RfmBase.RxBw, (v) => RfmBase.RxBw = v, 0, 20);
        }

        [Fact]
        public void TestRxBwAfc()
        {
            TestRange<byte>(() => RfmBase.RxBwAfc, (v) => RfmBase.RxBwAfc = v, 0, 20);
        }

        [Fact]
        public void TestSetIrq()
        {
            _rfm9x.ExecuteReset();
            _rfm9x.IrqFlags = Rfm9xIrqFlags.LowBattery | Rfm9xIrqFlags.FifoOverrun | Rfm9xIrqFlags.SyncAddressMatch | Rfm9xIrqFlags.PreambleDetect | Rfm9xIrqFlags.Rssi;
        }

        [Fact]
        public void TestTcxoInputOn()
        {
            TestRangeBool(() => _rfm9x.TcxoInputOn, (v) => _rfm9x.TcxoInputOn = v);
        }

        [Theory]
        [InlineData(TemperatureThreshold.FiveDegrees)]
        [InlineData(TemperatureThreshold.TenDegrees)]
        [InlineData(TemperatureThreshold.FifteenDegrees)]
        [InlineData(TemperatureThreshold.TwentyDegrees)]
        public void TestTemperatureThreshold(TemperatureThreshold expected)
        {
            TestAssignedValue(expected, () => _rfm9x.TemperatureThreshold, (v) => _rfm9x.TemperatureThreshold = v);
        }

        [Fact]
        public void TestTempMonitorOff()
        {
            TestRangeBool(() => _rfm9x.TempMonitorOff, (v) => _rfm9x.TempMonitorOff = v);
        }

        [Fact]
        public void TestTimeoutRxPreamble()
        {
            TestRange(() => _rfm9x.TimeoutRxPreamble, (v) => _rfm9x.TimeoutRxPreamble = v);
        }

        [Fact]
        public void TestTimeoutRxRssi()
        {
            TestRange(() => _rfm9x.TimeoutRxRssi, (v) => _rfm9x.TimeoutRxRssi = v);
        }

        [Fact]
        public void TestTimeoutSignalSync()
        {
            TestRange(() => _rfm9x.TimeoutSignalSync, (v) => _rfm9x.TimeoutSignalSync = v);
        }

        [Theory]
        [InlineData(Timer.Timer1, 0x00)]
        [InlineData(Timer.Timer2, 0x00)]
        [InlineData(Timer.Timer1, 0xFF)]
        [InlineData(Timer.Timer2, 0xFF)]
        public void TestTimerCoefficient(Timer timer, int expected)
        {
            _rfm9x.SetTimerCoefficient(timer, (byte)expected);

            Assert.Equal(expected, _rfm9x.GetTimerCoefficient(timer));
        }
    }
}