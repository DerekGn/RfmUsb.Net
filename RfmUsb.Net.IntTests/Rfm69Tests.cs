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

// Ignore Spelling: Lna Bw Aes Rssi Dagc Dcc Dio Fei Irq Initalise Rfm Rx

using Xunit;

namespace RfmUsb.Net.IntTests
{
    public class Rfm69Tests : RfmTestCommon, IClassFixture<TestFixture<IRfm69>>
    {
        private readonly TestFixture<IRfm69> _fixture;

        public Rfm69Tests(TestFixture<IRfm69> fixture) : base()
        {
            _fixture = fixture;
            RfmBase = fixture.BaseDevice;
        }

        [Fact]
        public void CurrentLnaGain()
        {
            Assert.Equal(LnaGain.Max, _fixture.Device.CurrentLnaGain);
        }

        [Fact]
        public void RxBwAfc()
        {
            TestRange<byte>(() => RfmBase.RxBwAfc, (v) => RfmBase.RxBwAfc = v, 0, 23);
        }

        [Fact]
        public void TestAesOn()
        {
            TestRangeBool(() => _fixture.Device.AesOn, (v) => _fixture.Device.AesOn = v);
        }

        [Fact(Skip = "Cant be set in isolation")]
        public void TestAfcLowBetaOn()
        {
            TestRangeBool(() => _fixture.Device.AfcLowBetaOn, (v) => _fixture.Device.AfcLowBetaOn = v);
        }

        [Fact]
        public void TestAutoRxRestartOn()
        {
            TestRangeBool(() => _fixture.Device.AutoRxRestartOn, (v) => _fixture.Device.AutoRxRestartOn = v);
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestContinuousDagc(ContinuousDagc expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.ContinuousDagc, (v) => _fixture.Device.ContinuousDagc = v);
        }

        [Theory]
        [InlineData(Rfm69DataMode.Reserved)]
        [InlineData(Rfm69DataMode.ContinousModeWithBitSync, Skip = "Not Supported")]
        [InlineData(Rfm69DataMode.ContinousModeWithoutBitSync)]
        [InlineData(Rfm69DataMode.Packet)]
        public void TestDataMode(Rfm69DataMode expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.DataMode, (v) => _fixture.Device.DataMode = v);
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        [InlineData(DccFreq.FreqPercent16)]
        public void TestDccFreq(DccFreq expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.DccFreq, (v) => _fixture.Device.DccFreq = v);
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        [InlineData(DccFreq.FreqPercent16)]
        public void TestDccFreqAfc(DccFreq expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.DccFreqAfc, (v) => _fixture.Device.DccFreqAfc = v);
        }

        [Theory]
        [InlineData(DioIrq.None)]
        [InlineData(DioIrq.Dio0)]
        [InlineData(DioIrq.Dio1)]
        [InlineData(DioIrq.Dio2)]
        [InlineData(DioIrq.Dio3)]
        [InlineData(DioIrq.Dio4)]
        [InlineData(DioIrq.Dio5)]
        public void TestDioInterruptMask(DioIrq expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.DioInterruptMask, (v) => _fixture.Device.DioInterruptMask = v);
        }

        [Theory]
        [InlineData(EnterCondition.Off)]
        [InlineData(EnterCondition.CrcOk)]
        [InlineData(EnterCondition.FifoLevel)]
        [InlineData(EnterCondition.FifoNotEmpty)]
        [InlineData(EnterCondition.PacketSent)]
        [InlineData(EnterCondition.PayloadReady)]
        [InlineData(EnterCondition.SyncAddressMatch)]
        [InlineData(EnterCondition.FallingEdgeFifoNotEmpty)]
        public void TestEnterCondition(EnterCondition expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.AutoModeEnterCondition, (v) => _fixture.Device.AutoModeEnterCondition = v);
        }

        [Fact]
        public void TestExecuteAfcClear()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteAfcClear()));
        }

        [Fact]
        public void TestExecuteAfcStart()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteAfcStart()));
        }

        [Fact]
        public void TestExecuteFeiStart()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteFeiStart()));
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestExecuteListenAbort(Mode mode)
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteListenModeAbort(mode)));
        }

        [Fact]
        public void TestExecuteMeasureTemperature()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteMeasureTemperature()));
        }

        [Fact]
        public void TestExecuteReset()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteReset()));
        }

        [Fact]
        public void TestExecuteRestartRx()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.ExecuteRestartRx()));
        }

        [Fact(Skip = "Causes device hang")]
        public void TestExecuteStartRssi()
        {
            _fixture.Device.ExecuteStartRssi();
        }

        [Theory]
        [InlineData(ExitCondition.CrcOk)]
        [InlineData(ExitCondition.FifoNotEmpty)]
        [InlineData(ExitCondition.FifoLevel)]
        [InlineData(ExitCondition.Off)]
        [InlineData(ExitCondition.PacketSent)]
        [InlineData(ExitCondition.PayloadReady)]
        [InlineData(ExitCondition.Timeout)]
        [InlineData(ExitCondition.SyncAddressMatch)]
        public void TestExitCondition(ExitCondition expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.AutoModeExitCondition, (v) => _fixture.Device.AutoModeExitCondition = v);
        }

        [Fact]
        public void TestFifo()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.Fifo));
        }

        [Fact]
        public void TestFifoFill()
        {
            TestRangeBool(() => _fixture.Device.FifoFill, (v) => _fixture.Device.FifoFill = v);
        }

        [Fact]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 63);
        }

        [Fact]
        public void TestGetIrqFlags()
        {
            _fixture.Device.ExecuteReset();
            Assert.Equal(Rfm69IrqFlags.ModeReady, _fixture.Device.IrqFlags);
        }

        [Fact]
        public void TestImpedance()
        {
            TestRangeBool(() => _fixture.Device.Impedance, (v) => _fixture.Device.Impedance = v);
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestIntermediateMode(IntermediateMode expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.IntermediateMode, (v) => _fixture.Device.IntermediateMode = v);
        }

        [Fact]
        public void TestListenCoefficientIdle()
        {
            TestRange(() => _fixture.Device.ListenCoefficientIdle, (v) => _fixture.Device.ListenCoefficientIdle = v);
        }

        [Fact]
        public void TestListenCoefficientRx()
        {
            TestRange(() => _fixture.Device.ListenCoefficientRx, (v) => _fixture.Device.ListenCoefficientRx = v);
        }

        [Fact]
        public void TestListenCriteria()
        {
            TestRangeBool(() => _fixture.Device.ListenCriteria, (v) => _fixture.Device.ListenCriteria = v);
        }

        [Theory]
        [InlineData(ListenEnd.Idle)]
        [InlineData(ListenEnd.Mode)]
        [InlineData(ListenEnd.Reserved)]
        [InlineData(ListenEnd.Rx)]
        public void TestListenEnd(ListenEnd expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.ListenEnd, (v) => _fixture.Device.ListenEnd = v);
        }

        [Fact]
        public void TestListenerOn()
        {
            TestRangeBool(() => _fixture.Device.ListenerOn, (v) => _fixture.Device.ListenerOn = v);
        }

        [Theory]
        [InlineData(ListenResolution.Reserved)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle262ms)]
        public void TestListenResolutionIdle(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.ListenResolutionIdle, (v) => _fixture.Device.ListenResolutionIdle = v);
        }

        [Theory]
        [InlineData(ListenResolution.Reserved)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle262ms)]
        public void TestListenResolutionRx(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.ListenResolutionRx, (v) => _fixture.Device.ListenResolutionRx = v);
        }

        [Fact]
        public void TestLowBetaAfcOffset()
        {
            TestRange(() => _fixture.Device.LowBetaAfcOffset, (v) => _fixture.Device.LowBetaAfcOffset = v);
        }

        [Fact]
        public void TestOutputPower()
        {
            TestRange<sbyte>(() => _fixture.Device.OutputPower, (v) => _fixture.Device.OutputPower = v, -2, 20);
        }

        [Fact]
        public void TestPayloadLength()
        {
            TestRange<ushort>(() => _fixture.Device.PayloadLength, (v) => _fixture.Device.PayloadLength = v, 0, 0xFF);
        }

        [Fact]
        public void TestRssiThreshold()
        {
            TestRange<sbyte>(() => _fixture.Device.RssiThreshold, (v) => _fixture.Device.RssiThreshold = v, -115, 0);
        }

        [Fact]
        public void TestRxBw()
        {
            TestRange<byte>(() => RfmBase.RxBw, (v) => RfmBase.RxBw = v, 0, 23);
        }

        [Fact]
        public void TestSensitivityBoost()
        {
            TestRangeBool(() => _fixture.Device.SensitivityBoost, (v) => _fixture.Device.SensitivityBoost = v);
        }

        [Fact]
        public void TestSequencer()
        {
            TestRangeBool(() => _fixture.Device.Sequencer, (v) => _fixture.Device.Sequencer = v);
        }

        [Fact]
        public void TestSetAesKey()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.SetAesKey(new List<byte> { 0xAA, 0x55, 0xAA, 0x55 })));
        }

        [Fact]
        public void TestSetIrqFlags()
        {
            Assert.Null(Record.Exception(() => _fixture.Device.IrqFlags));
        }

        [Fact]
        public void TestSyncBitErrors()
        {
            TestRange<byte>(() => _fixture.Device.SyncBitErrors, (v) => _fixture.Device.SyncBitErrors = v, 0, 7);
        }

        [Fact]
        public void TestTimeoutRxStart()
        {
            TestRange(() => _fixture.Device.TimeoutRxStart, (v) => _fixture.Device.TimeoutRxStart = v);
        }
    }
}