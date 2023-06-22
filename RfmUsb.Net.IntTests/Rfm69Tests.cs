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
    public class Rfm69Tests : RfmTestCommon
    {
        private readonly IRfm69 _rfm69;

        public Rfm69Tests()
        {
            _rfm69 = _serviceProvider.GetService<IRfm69>();
            RfmBase = _rfm69;

            _rfm69.Open("COM3", 230400);
        }

        [TestMethod]
        public void CurrentLnaGain()
        {
            Read(() => _rfm69.CurrentLnaGain);
        }

        [TestMethod]
        public void RxBwAfc()
        {
            TestRange<byte>(() => RfmBase.RxBwAfc, (v) => RfmBase.RxBwAfc = v, 0, 23);
        }

        [TestMethod]
        public void TestAesOn()
        {
            TestRangeBool(() => _rfm69.AesOn, (v) => _rfm69.AesOn = v);
        }

        [TestMethod]
        public void TestAfcLowBetaOn()
        {
            TestRangeBool(() => _rfm69.AfcLowBetaOn, (v) => _rfm69.AfcLowBetaOn = v);
        }

        [TestMethod]
        public void TestAutoRxRestartOn()
        {
            TestRangeBool(() => _rfm69.AutoRxRestartOn, (v) => _rfm69.AutoRxRestartOn = v);
        }

        [TestMethod]
        [DataRow(ContinuousDagc.Normal)]
        [DataRow(ContinuousDagc.ImprovedLowBeta0)]
        [DataRow(ContinuousDagc.ImprovedLowBeta1)]
        public void TestContinuousDagc(ContinuousDagc expected)
        {
            TestAssignedValue(expected, () => _rfm69.ContinuousDagc, (v) => _rfm69.ContinuousDagc = v);
        }

        [TestMethod]
        [DataRow(DccFreq.FreqPercent0_125)]
        [DataRow(DccFreq.FreqPercent0_25)]
        [DataRow(DccFreq.FreqPercent0_5)]
        [DataRow(DccFreq.FreqPercent1)]
        [DataRow(DccFreq.FreqPercent2)]
        [DataRow(DccFreq.FreqPercent4)]
        [DataRow(DccFreq.FreqPercent8)]
        [DataRow(DccFreq.FreqPercent16)]
        public void TestDccFreq(DccFreq expected)
        {
            TestAssignedValue(expected, () => _rfm69.DccFreq, (v) => _rfm69.DccFreq = v);
        }

        [TestMethod]
        [DataRow(DccFreq.FreqPercent0_125)]
        [DataRow(DccFreq.FreqPercent0_25)]
        [DataRow(DccFreq.FreqPercent0_5)]
        [DataRow(DccFreq.FreqPercent1)]
        [DataRow(DccFreq.FreqPercent2)]
        [DataRow(DccFreq.FreqPercent4)]
        [DataRow(DccFreq.FreqPercent8)]
        [DataRow(DccFreq.FreqPercent16)]
        public void TestDccFreqAfc(DccFreq expected)
        {
            TestAssignedValue(expected, () => _rfm69.DccFreqAfc, (v) => _rfm69.DccFreqAfc = v);
        }

        [TestMethod]
        [DataRow(DioIrq.None)]
        [DataRow(DioIrq.Dio0)]
        [DataRow(DioIrq.Dio1)]
        [DataRow(DioIrq.Dio2)]
        [DataRow(DioIrq.Dio3)]
        [DataRow(DioIrq.Dio4)]
        [DataRow(DioIrq.Dio5)]
        public void TestDioInterrupMask(DioIrq expected)
        {
            TestAssignedValue(expected, () => _rfm69.DioInterruptMask, (v) => _rfm69.DioInterruptMask = v);
        }

        [TestMethod]
        [DataRow(EnterCondition.Off)]
        [DataRow(EnterCondition.CrcOk)]
        [DataRow(EnterCondition.FifoEmpty)]
        [DataRow(EnterCondition.FifoLevel)]
        [DataRow(EnterCondition.FifoNotEmpty)]
        [DataRow(EnterCondition.PacketSent)]
        [DataRow(EnterCondition.PayloadReady)]
        [DataRow(EnterCondition.SyncAddressMatch)]
        public void TestEnterCondition(EnterCondition expected)
        {
            TestAssignedValue(expected, () => _rfm69.EnterCondition, (v) => _rfm69.EnterCondition = v);
        }

        [TestMethod]
        public void TestExecuteAfcClear()
        {
            _rfm69.ExecuteAfcClear();
        }

        [TestMethod]
        public void TestExecuteAfcStart()
        {
            _rfm69.ExecuteAfcStart();
        }

        [TestMethod]
        public void TestExecuteFeiStart()
        {
            _rfm69.ExecuteFeiStart();
        }

        [TestMethod]
        public void TestExecuteListenAbort()
        {
            _rfm69.ExecuteListenAbort();
        }

        [TestMethod]
        public void TestExecuteMeasureTemperature()
        {
            _rfm69.ExecuteMeasureTemperature();
        }

        [TestMethod]
        public void TestExecuteReset()
        {
            _rfm69.ExecuteReset();
        }

        [TestMethod]
        public void TestExecuteRestartRx()
        {
            _rfm69.ExecuteRestartRx();
        }

        [TestMethod]
        public void TestExecuteStartRssi()
        {
            _rfm69.ExecuteStartRssi();
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
        public void TestExitCondition(ExitCondition expected)
        {
            TestAssignedValue(expected, () => _rfm69.ExitCondition, (v) => _rfm69.ExitCondition = v);
        }

        [TestMethod]
        public void TestFifo()
        {
            var expected = RandomSequence().Take(66).ToList();
            _rfm69.Fifo = expected;

            _rfm69.Fifo.Should().StartWith(expected);
        }

        [TestMethod]
        public void TestFifoFill()
        {
            TestRangeBool(() => _rfm69.FifoFill, (v) => _rfm69.FifoFill = v);
        }

        [TestMethod]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 63);
        }

        [TestMethod]
        public void TestImpedance()
        {
            TestRangeBool(() => _rfm69.Impedance, (v) => _rfm69.Impedance = v);
        }

        [TestMethod]
        [DataRow(IntermediateMode.Rx)]
        [DataRow(IntermediateMode.Sleep)]
        [DataRow(IntermediateMode.Standby)]
        [DataRow(IntermediateMode.Tx)]
        public void TestIntermediateMode(IntermediateMode expected)
        {
            TestAssignedValue(expected, () => _rfm69.IntermediateMode, (v) => _rfm69.IntermediateMode = v);
        }

        [TestMethod]
        public void TestIrq()
        {
            _rfm69.ExecuteReset();
            _rfm69.Irq.Should().Be(Irq.None);
        }

        [TestMethod]
        public void TestListenCoefficentIdle()
        {
            TestRange(() => _rfm69.ListenCoefficentIdle, (v) => _rfm69.ListenCoefficentIdle = v);
        }

        [TestMethod]
        public void TestListenCoefficentRx()
        {
            TestRange(() => _rfm69.ListenCoefficentRx, (v) => _rfm69.ListenCoefficentRx = v);
        }

        [TestMethod]
        public void TestListenCriteria()
        {
            TestRangeBool(() => _rfm69.ListenCriteria, (v) => _rfm69.ListenCriteria = v);
        }

        [TestMethod]
        [DataRow(ListenEnd.Idle)]
        [DataRow(ListenEnd.Mode)]
        [DataRow(ListenEnd.Reserved)]
        [DataRow(ListenEnd.Rx)]
        public void TestListenEnd(ListenEnd expected)
        {
            TestAssignedValue(expected, () => _rfm69.ListenEnd, (v) => _rfm69.ListenEnd = v);
        }

        [TestMethod]
        public void TestListenerOn()
        {
            TestRangeBool(() => _rfm69.ListenerOn, (v) => _rfm69.ListenerOn = v);
        }

        [TestMethod]
        [DataRow(ListenResolution.Reserved)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle262ms)]
        public void TestListenResolutionIdle(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _rfm69.ListenResolutionIdle, (v) => _rfm69.ListenResolutionIdle = v);
        }

        [TestMethod]
        [DataRow(ListenResolution.Reserved)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle262ms)]
        public void TestListenResolutionRx(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _rfm69.ListenResolutionRx, (v) => _rfm69.ListenResolutionRx = v);
        }

        [TestMethod]
        public void TestLowBetaAfcOffset()
        {
            TestRange(() => _rfm69.LowBetaAfcOffset, (v) => _rfm69.LowBetaAfcOffset = v);
        }

        [TestMethod]
        public void TestOutputPower()
        {
            TestRange<sbyte>(() => _rfm69.OutputPower, (v) => _rfm69.OutputPower = v, -2, 20);
        }

        [TestMethod]
        public void TestPayloadLength()
        {
            TestRange<ushort>(() => _rfm69.PayloadLength, (v) => _rfm69.PayloadLength = v, 0, 0xFF);
        }

        [TestMethod]
        public void TestRadioConfig()
        {
            TestRange<byte>(() => _rfm69.RadioConfig, (v) => _rfm69.RadioConfig = v, 0,26);
        }

        [TestMethod]
        public void TestRssiThreshold()
        {
            TestRange<sbyte>(() => _rfm69.RssiThreshold, (v) => _rfm69.RssiThreshold = v, -115, 0);
        }

        [TestMethod]
        public void TestRxBw()
        {
            TestRange<byte>(() => RfmBase.RxBw, (v) => RfmBase.RxBw = v, 0, 23);
        }

        [TestMethod]
        public void TestSensitivityBoost()
        {
            TestRangeBool(() => _rfm69.SensitivityBoost, (v) => _rfm69.SensitivityBoost = v);
        }

        [TestMethod]
        public void TestSequencer()
        {
            TestRangeBool(() => _rfm69.Sequencer, (v) => _rfm69.Sequencer = v);
        }

        [TestMethod]
        public void TestSetAesKey()
        {
            _rfm69.Sync = new List<byte> { 0xAA, 0x55, 0xAA, 0x55 };
        }

        [TestMethod]
        public void TestSyncBitErrors()
        {
            TestRange<byte>(() => _rfm69.SyncBitErrors, (v) => _rfm69.SyncBitErrors = v, 0, 7);
        }

        [TestMethod]
        [Ignore]
        public void TestTimeout()
        {
            TestRange(() => _rfm69.Timeout, (v) => _rfm69.Timeout = v, 0, 1000);
        }

        [TestMethod]
        public void TestTimeoutRxStart()
        {
            TestRange(() => _rfm69.TimeoutRxStart, (v) => _rfm69.TimeoutRxStart = v);
        }


        private static IEnumerable<byte> RandomSequence()
        {
            Random r = new Random();
            while (true)
                yield return (byte)r.Next(0, 0xFF);
        }
    }
}