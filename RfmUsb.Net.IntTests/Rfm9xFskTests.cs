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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RfmUsb.Net.IntTests
{
    [TestClass]
    public class Rfm9xFskTests : RfmTestCommon
    {
        private readonly IRfm9x _rfm9x;

        public Rfm9xFskTests()
        {
            _rfm9x = _serviceProvider.GetService<IRfm9x>();
            RfmBase = _rfm9x;

            _rfm9x.Open("COM4", 230400);

            _rfm9x.ExecuteReset();
        }

        [TestMethod]
        public void TestAutoImageCalibrationOn()
        {
            TestRangeBool(() => _rfm9x.AutoImageCalibrationOn, (v) => _rfm9x.AutoImageCalibrationOn = v);
        }

        [TestMethod]
        public void TestBitRateFractional()
        {
            TestRange(() => _rfm9x.BitRateFractional, (v) => _rfm9x.BitRateFractional = (byte)v, 0, 0x0F);
        }

        [TestMethod]
        public void TestBitSyncOn()
        {
            TestRangeBool(() => _rfm9x.BitSyncOn, (v) => _rfm9x.BitSyncOn = v);
        }

        [TestMethod]
        [DataRow(CrcWhiteningType.CrcCCITT)]
        [DataRow(CrcWhiteningType.CrcIbm)]
        public void TestCrcWhiteningType(CrcWhiteningType expected)
        {
            TestAssignedValue(expected, () => _rfm9x.CrcWhiteningType, (v) => _rfm9x.CrcWhiteningType = v);
        }

        [TestMethod]
        public void TestExecuteAgcStart()
        {
            _rfm9x.ExecuteAgcStart();
        }

        [TestMethod]
        public void TestExecuteImageCalibration()
        {
            _rfm9x.ExecuteImageCalibration();
        }

        [TestMethod]
        public void TestExecuteRestartRxWithoutPllLock()
        {
            _rfm9x.ExecuteRestartRxWithoutPllLock();
        }

        [TestMethod]
        public void TestExecuteRestartRxWithPllLock()
        {
            _rfm9x.ExecuteRestartRxWithPllLock();
        }

        [TestMethod]
        public void TestExecuteSequencerStart()
        {
            _rfm9x.ExecuteSequencerStart();
        }

        [TestMethod]
        public void TestExecuteSequencerStop()
        {
            _rfm9x.ExecuteSequencerStop();
        }

        [TestMethod]
        public void TestFastHopOn()
        {
            TestRangeBool(() => _rfm9x.FastHopOn, (v) => _rfm9x.FastHopOn = v);
        }

        [TestMethod]
        public void TestFifo()
        {
            var expected = RandomSequence().Take(64).ToList();
            _rfm9x.Fifo = expected;

            _rfm9x.Fifo.Should().StartWith(expected);
        }

        [TestMethod]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 63);
        }

        [TestMethod]
        [Ignore("not implemented in device")]
        public void TestFormerTemperatureValue()
        {
            TestRange(() => _rfm9x.FormerTemperatureValue, (v) => _rfm9x.FormerTemperatureValue = v);
        }

        [TestMethod]
        public void TestFromIdle()
        {
            TestRangeBool(() => _rfm9x.FromIdle, (v) => _rfm9x.FromIdle = v);
        }

        [TestMethod]
        [DataRow(FromPacketReceived.ToLowPowerSelection)]
        [DataRow(FromPacketReceived.ToReceiveState)]
        [DataRow(FromPacketReceived.ToReceiveViaFSMode)]
        [DataRow(FromPacketReceived.ToSequencerOff)]
        [DataRow(FromPacketReceived.ToTransmitStateOnFifoEmpty)]
        public void TestFromPacketReceived(FromPacketReceived expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromPacketReceived, (v) => _rfm9x.FromPacketReceived = v);
        }

        [TestMethod]
        [DataRow(FromReceive.ToLowPowerSelectionOnPayLoadReady)]
        [DataRow(FromReceive.ToPacketReceivedOnPayloadReady)]
        [DataRow(FromReceive.ToPacketReceivedStateOnCrcOk)]
        [DataRow(FromReceive.ToSequencerOffOnPreambleDetect)]
        [DataRow(FromReceive.ToSequencerOffOnRssi)]
        [DataRow(FromReceive.ToSequencerOffOnSyncAddress)]
        [DataRow(FromReceive.UnusedA)]
        [DataRow(FromReceive.UnusedB)]
        public void TestFromReceive(FromReceive expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromReceive, (v) => _rfm9x.FromReceive = v);
        }

        [TestMethod]
        [DataRow(FromRxTimeout.ToLowPowerSelection)]
        [DataRow(FromRxTimeout.ToReceive)]
        [DataRow(FromRxTimeout.ToSequencerOff)]
        [DataRow(FromRxTimeout.ToTransmit)]
        public void TestFromReceive(FromRxTimeout expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromRxTimeout, (v) => _rfm9x.FromRxTimeout = v);
        }

        [TestMethod]
        [DataRow(FromStart.ToLowPowerSelection)]
        [DataRow(FromStart.ToReceive)]
        [DataRow(FromStart.ToTransmit)]
        [DataRow(FromStart.ToTransmitOnFifoLevel)]
        public void TestFromStart(FromStart expected)
        {
            TestAssignedValue(expected, () => _rfm9x.FromStart, (v) => _rfm9x.FromStart = v);
        }

        [TestMethod]
        [DataRow(OokAverageOffset.Offset0dB)]
        [DataRow(OokAverageOffset.Offset2dB)]
        [DataRow(OokAverageOffset.Offset4dB)]
        [DataRow(OokAverageOffset.Offset6dB)]
        public void TestFromStart(OokAverageOffset expected)
        {
            TestAssignedValue(expected, () => _rfm9x.OokAverageOffset, (v) => _rfm9x.OokAverageOffset = v);
        }

        [TestMethod]
        public void TestFromTransmit()
        {
            TestRangeBool(() => _rfm9x.FromTransmit, (v) => _rfm9x.FromTransmit = v);
        }

        [TestMethod]
        public void TestIoHomeOn()
        {
            TestRangeBool(() => _rfm9x.IoHomeOn, (v) => _rfm9x.IoHomeOn = v);
        }

        [TestMethod]
        public void TestIoHomePowerFrame()
        {
            TestRangeBool(() => _rfm9x.IoHomePowerFrame, (v) => _rfm9x.IoHomePowerFrame = v);
        }

        [TestMethod]
        public void TestIrq()
        {
            _rfm9x.ExecuteReset();
            _rfm9x.IrqFlags.Should().Be(Rfm9xIrqFlags.ModeReady);
        }

        [TestMethod]
        public void TestLnaBoostHf()
        {
            TestRangeBool(() => _rfm9x.LnaBoostHf, (v) => _rfm9x.LnaBoostHf = v);
        }

        [TestMethod]
        public void TestLowBatteryOn()
        {
            TestRangeBool(() => _rfm9x.LowBatteryOn, (v) => _rfm9x.LowBatteryOn = v);
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
        public void TestLowBatteryTrim(LowBatteryTrim expected)
        {
            TestAssignedValue(expected, () => _rfm9x.LowBatteryTrim, (v) => _rfm9x.LowBatteryTrim = v);
        }

        [TestMethod]
        public void TestLowFrequencyMode()
        {
            TestRangeBool(() => _rfm9x.LowFrequencyMode, (v) => _rfm9x.LowFrequencyMode = v);
        }

        [TestMethod]
        public void TestLowPowerSelection()
        {
            TestRangeBool(() => _rfm9x.LowPowerSelection, (v) => _rfm9x.LowPowerSelection = v);
        }

        [TestMethod]
        public void TestMapPreambleDetect()
        {
            TestRangeBool(() => _rfm9x.MapPreambleDetect, (v) => _rfm9x.MapPreambleDetect = v);
        }

        [TestMethod]
        public void TestPreambleDetectorOn()
        {
            TestRangeBool(() => _rfm9x.PreambleDetectorOn, (v) => _rfm9x.PreambleDetectorOn = v);
        }

        [TestMethod]
        [DataRow(PreambleDetectorSize.OneByte)]
        [DataRow(PreambleDetectorSize.ThreeBytes)]
        [DataRow(PreambleDetectorSize.TwoBytes)]
        public void TestPreambleDetectorSize(PreambleDetectorSize expected)
        {
            TestAssignedValue(expected, () => _rfm9x.PreambleDetectorSize, (v) => _rfm9x.PreambleDetectorSize = v);
        }

        [TestMethod]
        public void TestPreambleDetectorTotal()
        {
            TestRange<byte>(() => _rfm9x.PreambleDetectorTotalerance, (v) => _rfm9x.PreambleDetectorTotalerance = v, 0, 0x0F);
        }

        [TestMethod]
        public void TestPreamblePolarity()
        {
            TestRangeBool(() => _rfm9x.PreamblePolarity, (v) => _rfm9x.PreamblePolarity = v);
        }

        [TestMethod]
        public void TestRestartRxOnCollision()
        {
            TestRangeBool(() => _rfm9x.RestartRxOnCollision, (v) => _rfm9x.RestartRxOnCollision = v);
        }

        [TestMethod]
        public void TestRssiCollisionThreshold()
        {
            TestRange(() => _rfm9x.RssiCollisionThreshold, (v) => _rfm9x.RssiCollisionThreshold = v);
        }

        [TestMethod]
        public void TestRssiOffset()
        {
            TestRange(() => _rfm9x.RssiOffset, (v) => _rfm9x.RssiOffset = (sbyte)v, -16, 15);
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
        public void TestRssiSmoothing(RssiSmoothing expected)
        {
            TestAssignedValue(expected, () => _rfm9x.RssiSmoothing, (v) => _rfm9x.RssiSmoothing = v);
        }

        [TestMethod]
        public void TestRssiThreshold()
        {
            TestRange(() => _rfm9x.RssiThreshold, (v) => _rfm9x.RssiThreshold = (sbyte)v, 0, -127);
        }

        [TestMethod]
        public void TestRxBw()
        {
            TestRange<byte>(() => RfmBase.RxBw, (v) => RfmBase.RxBw = v, 0, 20);
        }

        [TestMethod]
        public void TestRxBwAfc()
        {
            TestRange<byte>(() => RfmBase.RxBwAfc, (v) => RfmBase.RxBwAfc = v, 0, 20);
        }

        [TestMethod]
        public void TestTcxoInputOn()
        {
            TestRangeBool(() => _rfm9x.TcxoInputOn, (v) => _rfm9x.TcxoInputOn = v);
        }

        [TestMethod]
        [DataRow(TemperatureThreshold.FiveDegrees)]
        [DataRow(TemperatureThreshold.TenDegrees)]
        [DataRow(TemperatureThreshold.FifteenDegrees)]
        [DataRow(TemperatureThreshold.TwentyDegrees)]
        public void TestTemperatureThreshold(TemperatureThreshold expected)
        {
            TestAssignedValue(expected, () => _rfm9x.TemperatureThreshold, (v) => _rfm9x.TemperatureThreshold = v);
        }

        [TestMethod]
        public void TestTempMonitorOff()
        {
            TestRangeBool(() => _rfm9x.TempMonitorOff, (v) => _rfm9x.TempMonitorOff = v);
        }

        [TestMethod]
        public void TestTimeoutRxPreamble()
        {
            TestRange(() => _rfm9x.TimeoutRxPreamble, (v) => _rfm9x.TimeoutRxPreamble = v);
        }

        [TestMethod]
        public void TestTimeoutRxRssi()
        {
            TestRange(() => _rfm9x.TimeoutRxRssi, (v) => _rfm9x.TimeoutRxRssi = v);
        }

        [TestMethod]
        public void TestTimeoutSignalSync()
        {
            TestRange(() => _rfm9x.TimeoutSignalSync, (v) => _rfm9x.TimeoutSignalSync = v);
        }

        [TestMethod]
        [DataRow(Timer.Timer1)]
        [DataRow(Timer.Timer2)]
        public void TestTimerCoefficient(Timer expected)
        {
            throw new NotImplementedException();
        }
    }
}