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

using System.Runtime.CompilerServices;
using Xunit;

namespace RfmUsb.Net.IntTests
{
    public class Rfm9xFskTests : RfmTestCommon, IClassFixture<TestFixture<IRfm9x>>
    {
        private readonly TestFixture<IRfm9x> _fixture;

        public Rfm9xFskTests(TestFixture<IRfm9x> fixture)
        {
            _fixture = fixture;
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
            _fixture.Device.SetTimerResolution(timer, expected);

            Assert.Equal(expected, _fixture.Device.GetTimerResolution(timer));
        }

        [Fact]
        public void TestAgcAutoOn()
        {
            TestRangeBool(() => _fixture.Device.AgcAutoOn, (v) => _fixture.Device.AgcAutoOn = v);
        }

        [Fact]
        public void TestAutoImageCalibrationOn()
        {
            TestRangeBool(() => _fixture.Device.AutoImageCalibrationOn, (v) => _fixture.Device.AutoImageCalibrationOn = v);
        }

        [Fact]
        public void TestBitRateFractional()
        {
            TestRange(() => _fixture.Device.BitRateFractional, (v) => _fixture.Device.BitRateFractional = (byte)v, 0, 0x0F);
        }

        [Fact]
        public void TestBitSyncOn()
        {
            TestRangeBool(() => _fixture.Device.BitSyncOn, (v) => _fixture.Device.BitSyncOn = v);
        }

        [Theory]
        [InlineData(CrcWhiteningType.CrcCCITT)]
        [InlineData(CrcWhiteningType.CrcIbm)]
        public void TestCrcWhiteningType(CrcWhiteningType expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.CrcWhiteningType, (v) => _fixture.Device.CrcWhiteningType = v);
        }

        [Fact]
        public void TestExecuteAgcStart()
        {
            _fixture.Device.ExecuteAgcStart();
        }

        [Fact]
        public void TestExecuteImageCalibration()
        {
            _fixture.Device.ExecuteImageCalibration();
        }

        [Fact]
        public void TestExecuteRestartRxWithoutPllLock()
        {
            _fixture.Device.ExecuteRestartRxWithoutPllLock();
        }

        [Fact]
        public void TestExecuteRestartRxWithPllLock()
        {
            _fixture.Device.ExecuteRestartRxWithPllLock();
        }

        [Fact]
        public void TestExecuteSequencerStart()
        {
            _fixture.Device.ExecuteSequencerStart();
        }

        [Fact]
        public void TestExecuteSequencerStop()
        {
            _fixture.Device.ExecuteSequencerStop();
        }

        [Fact]
        public void TestFastHopOn()
        {
            TestRangeBool(() => _fixture.Device.FastHopOn, (v) => _fixture.Device.FastHopOn = v);
        }

        [Fact]
        public void TestFifo()
        {
            var expected = RandomSequence().Take(64).ToList();
            _fixture.Device.Fifo = expected;

            Assert.Equal(expected, _fixture.Device.Fifo);
        }

        [Fact]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 63);
        }

        [Fact(Skip = "not implemented in device")]
        public void TestFormerTemperatureValue()
        {
            TestRange(() => _fixture.Device.FormerTemperature, (v) => _fixture.Device.FormerTemperature = v);
        }

        [Fact]
        public void TestFromIdle()
        {
            TestRangeBool(() => _fixture.Device.FromIdle, (v) => _fixture.Device.FromIdle = v);
        }

        [Theory]
        [InlineData(FromPacketReceived.ToLowPowerSelection)]
        [InlineData(FromPacketReceived.ToReceiveState)]
        [InlineData(FromPacketReceived.ToReceiveViaFSMode)]
        [InlineData(FromPacketReceived.ToSequencerOff)]
        [InlineData(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestFromPacketReceived(FromPacketReceived expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.FromPacketReceived, (v) => _fixture.Device.FromPacketReceived = v);
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
            TestAssignedValue(expected, () => _fixture.Device.FromReceive, (v) => _fixture.Device.FromReceive = v);
        }

        [Theory]
        [InlineData(FromRxTimeout.ToLowPowerSelection)]
        [InlineData(FromRxTimeout.ToReceive)]
        [InlineData(FromRxTimeout.ToSequencerOff)]
        [InlineData(FromRxTimeout.ToTransmit)]
        public void TestFromRxTimeout(FromRxTimeout expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.FromRxTimeout, (v) => _fixture.Device.FromRxTimeout = v);
        }

        [Theory]
        [InlineData(FromStart.ToLowPowerSelection)]
        [InlineData(FromStart.ToReceive)]
        [InlineData(FromStart.ToTransmit)]
        [InlineData(FromStart.ToTransmitOnFifoLevel)]
        public void TestFromStart(FromStart expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.FromStart, (v) => _fixture.Device.FromStart = v);
        }

        [Theory]
        [InlineData(OokAverageOffset.Offset0dB)]
        [InlineData(OokAverageOffset.Offset2dB)]
        [InlineData(OokAverageOffset.Offset4dB)]
        [InlineData(OokAverageOffset.Offset6dB)]
        public void TestOokAverageOffset(OokAverageOffset expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.OokAverageOffset, (v) => _fixture.Device.OokAverageOffset = v);
        }

        [Fact]
        public void TestFromTransmit()
        {
            TestRangeBool(() => _fixture.Device.FromTransmit, (v) => _fixture.Device.FromTransmit = v);
        }

        [Fact]
        public void TestGetIrq()
        {
            _fixture.Device.ExecuteReset();
            Assert.Equal(Rfm9xIrqFlags.ModeReady, _fixture.Device.IrqFlags);
        }

        [Fact]
        public void TestIdleMode()
        {
            TestRangeBool(() => _fixture.Device.IdleMode, (v) => _fixture.Device.IdleMode = v);
        }

        [Fact]
        public void TestIoHomeOn()
        {
            TestRangeBool(() => _fixture.Device.IoHomeOn, (v) => _fixture.Device.IoHomeOn = v);
        }

        [Fact]
        public void TestIoHomePowerFrame()
        {
            TestRangeBool(() => _fixture.Device.IoHomePowerFrame, (v) => _fixture.Device.IoHomePowerFrame = v);
        }

        [Fact]
        public void TestLnaBoostHf()
        {
            TestRangeBool(() => _fixture.Device.LnaBoostHf, (v) => _fixture.Device.LnaBoostHf = v);
        }

        [Fact]
        public void TestLowBatteryOn()
        {
            TestRangeBool(() => _fixture.Device.LowBatteryOn, (v) => _fixture.Device.LowBatteryOn = v);
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
            TestAssignedValue(expected, () => _fixture.Device.LowBatteryTrim, (v) => _fixture.Device.LowBatteryTrim = v);
        }

        [Fact]
        public void TestLowFrequencyMode()
        {
            TestRangeBool(() => _fixture.Device.LowFrequencyMode, (v) => _fixture.Device.LowFrequencyMode = v);
        }

        [Fact]
        public void TestLowPowerSelection()
        {
            TestRangeBool(() => _fixture.Device.LowPowerSelection, (v) => _fixture.Device.LowPowerSelection = v);
        }

        [Fact]
        public void TestMapPreambleDetect()
        {
            TestRangeBool(() => _fixture.Device.MapPreambleDetect, (v) => _fixture.Device.MapPreambleDetect = v);
        }

        [Fact]
        public void TestOutputPower()
        {
            TestRange<byte>(() => _fixture.Device.OutputPower, (v) => _fixture.Device.OutputPower = v, 2, 20);
        }
        [Fact]
        public void TestPreambleDetectorOn()
        {
            TestRangeBool(() => _fixture.Device.PreambleDetectorOn, (v) => _fixture.Device.PreambleDetectorOn = v);
        }

        [Theory]
        [InlineData(PreambleDetectorSize.OneByte)]
        [InlineData(PreambleDetectorSize.ThreeBytes)]
        [InlineData(PreambleDetectorSize.TwoBytes)]
        public void TestPreambleDetectorSize(PreambleDetectorSize expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.PreambleDetectorSize, (v) => _fixture.Device.PreambleDetectorSize = v);
        }

        [Fact]
        public void TestPreambleDetectorTotal()
        {
            TestRange<byte>(() => _fixture.Device.PreambleDetectorTolerance, (v) => _fixture.Device.PreambleDetectorTolerance = v, 0, 0x0F);
        }

        [Fact]
        public void TestPreamblePolarity()
        {
            TestRangeBool(() => _fixture.Device.PreamblePolarity, (v) => _fixture.Device.PreamblePolarity = v);
        }

        [Fact]
        public void TestRestartRxOnCollision()
        {
            TestRangeBool(() => _fixture.Device.RestartRxOnCollision, (v) => _fixture.Device.RestartRxOnCollision = v);
        }

        [Fact]
        public void TestRssiCollisionThreshold()
        {
            TestRange(() => _fixture.Device.RssiCollisionThreshold, (v) => _fixture.Device.RssiCollisionThreshold = v);
        }

        [Fact]
        public void TestRssiOffset()
        {
            TestRange(() => _fixture.Device.RssiOffset, (v) => _fixture.Device.RssiOffset = (sbyte)v, -16, 15);
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
            TestAssignedValue(expected, () => _fixture.Device.RssiSmoothing, (v) => _fixture.Device.RssiSmoothing = v);
        }

        [Fact]
        public void TestRssiThreshold()
        {
            TestRange(() => _fixture.Device.RssiThreshold, (v) => _fixture.Device.RssiThreshold = (sbyte)v, 0, -127);
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
            _fixture.Device.ExecuteReset();
            _fixture.Device.IrqFlags = Rfm9xIrqFlags.LowBattery | Rfm9xIrqFlags.FifoOverrun | Rfm9xIrqFlags.SyncAddressMatch | Rfm9xIrqFlags.PreambleDetect | Rfm9xIrqFlags.Rssi;
        }

        [Fact]
        public void TestTcxoInputOn()
        {
            TestRangeBool(() => _fixture.Device.TcxoInputOn, (v) => _fixture.Device.TcxoInputOn = v);
        }

        [Theory]
        [InlineData(TemperatureThreshold.FiveDegrees)]
        [InlineData(TemperatureThreshold.TenDegrees)]
        [InlineData(TemperatureThreshold.FifteenDegrees)]
        [InlineData(TemperatureThreshold.TwentyDegrees)]
        public void TestTemperatureThreshold(TemperatureThreshold expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.TemperatureThreshold, (v) => _fixture.Device.TemperatureThreshold = v);
        }

        [Fact]
        public void TestTempMonitorOff()
        {
            TestRangeBool(() => _fixture.Device.TempMonitorOff, (v) => _fixture.Device.TempMonitorOff = v);
        }

        [Fact]
        public void TestTimeoutRxPreamble()
        {
            TestRange(() => _fixture.Device.TimeoutRxPreamble, (v) => _fixture.Device.TimeoutRxPreamble = v);
        }

        [Fact]
        public void TestTimeoutRxRssi()
        {
            TestRange(() => _fixture.Device.TimeoutRxRssi, (v) => _fixture.Device.TimeoutRxRssi = v);
        }

        [Fact]
        public void TestTimeoutSignalSync()
        {
            TestRange(() => _fixture.Device.TimeoutSignalSync, (v) => _fixture.Device.TimeoutSignalSync = v);
        }

        [Theory]
        [InlineData(Timer.Timer1, 0x00)]
        [InlineData(Timer.Timer2, 0x00)]
        [InlineData(Timer.Timer1, 0xFF)]
        [InlineData(Timer.Timer2, 0xFF)]
        public void TestTimerCoefficient(Timer timer, int expected)
        {
            _fixture.Device.SetTimerCoefficient(timer, (byte)expected);

            Assert.Equal(expected, _fixture.Device.GetTimerCoefficient(timer));
        }
    }
}