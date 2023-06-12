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
    public class Rfm6xTests : RfmBaseTests
    {
        private readonly IRfm6x _rfm6x;

        public Rfm6xTests()
        {
            _rfm6x = _serviceProvider.GetService<IRfm6x>();
            RfmBase = _rfm6x;

            _rfm6x.Open("COM12", 230400);
        }

        [TestMethod]
        public void TestAesOn()
        {
            TestRangeBool(() => _rfm6x.AesOn, (v) => _rfm6x.AesOn = v);
        }

        [TestMethod]
        public void TestAfcLowBetaOn()
        {
            TestRangeBool(() => _rfm6x.AfcLowBetaOn, (v) => _rfm6x.AfcLowBetaOn = v);
        }

        [TestMethod]
        [DataRow(ContinuousDagc.Normal)]
        [DataRow(ContinuousDagc.ImprovedLowBeta0)]
        [DataRow(ContinuousDagc.ImprovedLowBeta1)]
        public void TestContinuousDagc(ContinuousDagc expected)
        {
            TestAssignedValue(expected, () => _rfm6x.ContinuousDagc, (v) => _rfm6x.ContinuousDagc = v);
        }

        [TestMethod]
        public void CurrentLnaGain()
        {
            Read(() => _rfm6x.CurrentLnaGain);
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
            TestAssignedValue(expected, () => _rfm6x.DccFreq, (v) => _rfm6x.DccFreq = v);
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
            TestAssignedValue(expected, () => _rfm6x.DccFreqAfc, (v) => _rfm6x.DccFreqAfc = v);
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
            TestAssignedValue(expected, () => _rfm6x.DioInterruptMask, (v) => _rfm6x.DioInterruptMask = v);
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
            TestAssignedValue(expected, () => _rfm6x.EnterCondition, (v) => _rfm6x.EnterCondition = v);
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
            TestAssignedValue(expected, () => _rfm6x.ExitCondition, (v) => _rfm6x.ExitCondition = v);
        }

        [TestMethod]
        public void TestFifoFill()
        {
            TestRangeBool(() => _rfm6x.FifoFill, (v) => _rfm6x.FifoFill = v);   
        }

        [TestMethod]
        public void TestImpedance()
        {
            TestRangeBool(() => _rfm6x.Impedance, (v) => _rfm6x.Impedance = v);
        }

        [TestMethod]
        [DataRow(IntermediateMode.Rx)]
        [DataRow(IntermediateMode.Sleep)]
        [DataRow(IntermediateMode.Standby)]
        [DataRow(IntermediateMode.Tx)]
        public void TestIntermediateMode(IntermediateMode expected)
        {
            TestAssignedValue(expected, () => _rfm6x.IntermediateMode, (v) => _rfm6x.IntermediateMode = v);
        }

        [TestMethod]
        public void TestIrq()
        {
            _rfm6x.ExecuteReset();
            _rfm6x.Irq.Should().Be(Irq.None);
        }

        [TestMethod]
        public void TestListenCoefficentIdle()
        {
            TestRange(() => _rfm6x.ListenCoefficentIdle, (v) => _rfm6x.ListenCoefficentIdle = v);
        }

        [TestMethod]
        public void TestListenCoefficentRx()
        {
            TestRange(() => _rfm6x.ListenCoefficentRx, (v) => _rfm6x.ListenCoefficentRx = v);
        }

        [TestMethod]
        public void TestListenCriteria()
        {
            TestRangeBool(() => _rfm6x.ListenCriteria, (v) => _rfm6x.ListenCriteria = v);
        }

        [TestMethod]
        [DataRow(ListenEnd.Idle)]
        [DataRow(ListenEnd.Mode)]
        [DataRow(ListenEnd.Reserved)]
        [DataRow(ListenEnd.Rx)]
        public void TestListenEnd(ListenEnd expected)
        {
            TestAssignedValue(expected, () => _rfm6x.ListenEnd, (v) => _rfm6x.ListenEnd = v);
        }

        [TestMethod]
        public void TestListenerOn()
        {
            TestRangeBool(() => _rfm6x.ListenerOn, (v) => _rfm6x.ListenerOn = v);
        }

        [TestMethod]
        [DataRow(ListenResolution.Reserved)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle262ms)]
        public void TestListenResolutionIdle(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _rfm6x.ListenResolutionIdle, (v) => _rfm6x.ListenResolutionIdle = v);
        }

        [TestMethod]
        [DataRow(ListenResolution.Reserved)]
        [DataRow(ListenResolution.Idle64us)]
        [DataRow(ListenResolution.Idle4_1ms)]
        [DataRow(ListenResolution.Idle262ms)]
        public void TestListenResolutionRx(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _rfm6x.ListenResolutionRx, (v) => _rfm6x.ListenResolutionRx = v);
        }

        [TestMethod]
        public void TestLowBetaAfcOffset()
        {
            TestRange(() => _rfm6x.LowBetaAfcOffset, (v) => _rfm6x.LowBetaAfcOffset = v);
        }

        [TestMethod]
        public void TestOutputPower()
        {
            TestRange<sbyte>(() => _rfm6x.OutputPower, (v) => _rfm6x.OutputPower = v, -2, 20);
        }

        [TestMethod]
        public void TestRadioConfig()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestRssiThreshold()
        {
            TestRange(() => _rfm6x.RssiThreshold, (v) => _rfm6x.RssiThreshold = v);
        }

        [TestMethod]
        public void TestSensitivityBoost()
        {
            TestRangeBool(() => _rfm6x.SensitivityBoost, (v) => _rfm6x.SensitivityBoost = v); 
        }

        [TestMethod]
        public void TestSequencer()
        {
            TestRangeBool(() => _rfm6x.Sequencer, (v) => _rfm6x.Sequencer = v);
        }

        [TestMethod]
        public void TestSyncBitErrors()
        {
            TestRange<byte>(() => _rfm6x.SyncBitErrors, (v) => _rfm6x.SyncBitErrors = v, 0, 7);
        }

        [TestMethod]
        [Ignore]
        public void TestTimeout()
        {
            TestRange(() => _rfm6x.Timeout, (v) => _rfm6x.Timeout = v, 0, 1000);
        }

        [TestMethod]
        public void TestPayloadLength()
        {
            TestRange<ushort>(() => _rfm6x.PayloadLength, (v) => _rfm6x.PayloadLength = v, 0, 0xFF);
        }

        [TestMethod]
        public void TestTimeoutRxStart()
        {
            TestRange(() => _rfm6x.TimeoutRxStart, (v) => _rfm6x.TimeoutRxStart = v);
        }

        [TestMethod]
        public void TestExecuteAfcClear()
        {
            _rfm6x.ExecuteAfcClear();
        }

        [TestMethod]
        public void TestExecuteAfcStart()
        {
            _rfm6x.ExecuteAfcStart();
        }

        [TestMethod]
        public void TestExecuteFeiStart()
        {
            _rfm6x.ExecuteFeiStart();
        }

        [TestMethod]
        public void TestGetRadioConfigurations()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestExecuteListenAbort()
        {
            _rfm6x.ExecuteListenAbort();
        }

        [TestMethod]
        public void TestExecuteMeasureTemperature()
        {
            _rfm6x.ExecuteMeasureTemperature();
        }

        [TestMethod]
        public void TestExecuteReset()
        {
            _rfm6x.ExecuteReset();
        }

        [TestMethod]
        public void TestExecuteRestartRx()
        {
            _rfm6x.ExecuteRestartRx();
        }

        [TestMethod]
        public void TestSetAesKey()
        {
            _rfm6x.Sync = new List<byte> { 0xAA, 0x55, 0xAA, 0x55 };
        }

        [TestMethod]
        public void TestExecuteStartRssi()
        {
            _rfm6x.ExecuteStartRssi();
        }
    }
}