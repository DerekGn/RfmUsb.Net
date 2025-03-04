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

// Ignore Spelling: Lna Bw Aes Rssi Dagc Dcc Dio Fei Irq Initalise

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RfmUsb.Net.IntTests
{
    public class Rfm69Tests : RfmTestCommon, IDisposable
    {
        private readonly IRfm69 _rfm69;

        public Rfm69Tests()
        {
            _rfm69 = _serviceProvider.GetService<IRfm69>() ?? throw new NullReferenceException($"Unable to resolve {nameof(IRfm69)}");

            _rfm69.Open((string)TestContext.Properties["Rfm69ComPort"]!, int.Parse((string)TestContext.Properties["BaudRate"]!));

            RfmBase = _rfm69;
        }

        [Fact]
        public void CurrentLnaGain()
        {
            Read(() => _rfm69.CurrentLnaGain);
        }

        [Fact]
        public void RxBwAfc()
        {
            TestRange<byte>(() => RfmBase.RxBwAfc, (v) => RfmBase.RxBwAfc = v, 0, 23);
        }

        [Fact]
        public void TestAesOn()
        {
            TestRangeBool(() => _rfm69.AesOn, (v) => _rfm69.AesOn = v);
        }

        [Fact]
        public void TestAfcLowBetaOn()
        {
            TestRangeBool(() => _rfm69.AfcLowBetaOn, (v) => _rfm69.AfcLowBetaOn = v);
        }

        [Fact]
        public void TestAutoRxRestartOn()
        {
            TestRangeBool(() => _rfm69.AutoRxRestartOn, (v) => _rfm69.AutoRxRestartOn = v);
        }

        public void Dispose()
        {
            _rfm69?.Close();
            _rfm69?.Dispose();
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestContinuousDagc(ContinuousDagc expected)
        {
            TestAssignedValue(expected, () => _rfm69.ContinuousDagc, (v) => _rfm69.ContinuousDagc = v);
        }

        [Theory]
        [InlineData(Rfm69DataMode.Reserved)]
        //[InlineData(Rfm69DataMode.ContinousModeWithBitSync)]
        [InlineData(Rfm69DataMode.ContinousModeWithoutBitSync)]
        [InlineData(Rfm69DataMode.Packet)]
        public void TestDataMode(Rfm69DataMode expected)
        {
            TestAssignedValue(expected, () => _rfm69.DataMode, (v) => _rfm69.DataMode = v);
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
            TestAssignedValue(expected, () => _rfm69.DccFreq, (v) => _rfm69.DccFreq = v);
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
            TestAssignedValue(expected, () => _rfm69.DccFreqAfc, (v) => _rfm69.DccFreqAfc = v);
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
            TestAssignedValue(expected, () => _rfm69.DioInterruptMask, (v) => _rfm69.DioInterruptMask = v);
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
            TestAssignedValue(expected, () => _rfm69.AutoModeEnterCondition, (v) => _rfm69.AutoModeEnterCondition = v);
        }

        [Fact]
        public void TestExecuteAfcClear()
        {
            _rfm69.ExecuteAfcClear();
        }

        [Fact]
        public void TestExecuteAfcStart()
        {
            _rfm69.ExecuteAfcStart();
        }

        [Fact]
        public void TestExecuteFeiStart()
        {
            _rfm69.ExecuteFeiStart();
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestExecuteListenAbort(Mode mode)
        {
            _rfm69.ExecuteListenModeAbort(mode);
        }

        [Fact(Skip = "Ignore")]
        public void TestExecuteMeasureTemperature()
        {
            _rfm69.ExecuteMeasureTemperature();
        }

        [Fact]
        public void TestExecuteReset()
        {
            _rfm69.ExecuteReset();
        }

        [Fact]
        public void TestExecuteRestartRx()
        {
            _rfm69.ExecuteRestartRx();
        }

        [Fact]
        public void TestExecuteStartRssi()
        {
            _rfm69.ExecuteStartRssi();
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
            TestAssignedValue(expected, () => _rfm69.AutoModeExitCondition, (v) => _rfm69.AutoModeExitCondition = v);
        }

        [Fact]
        public void TestFifo()
        {
            var expected = RandomSequence().Take(66);
            _rfm69.Fifo = expected;

            Assert.Equal(expected, _rfm69.Fifo);
        }

        [Fact]
        public void TestFifoFill()
        {
            TestRangeBool(() => _rfm69.FifoFill, (v) => _rfm69.FifoFill = v);
        }

        [Fact]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 63);
        }

        [Fact]
        public void TestGetIrqFlags()
        {
            _rfm69.ExecuteReset();
            Assert.Equal(Rfm69IrqFlags.ModeReady, _rfm69.IrqFlags);
        }

        [Fact]
        public void TestImpedance()
        {
            TestRangeBool(() => _rfm69.Impedance, (v) => _rfm69.Impedance = v);
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestIntermediateMode(IntermediateMode expected)
        {
            TestAssignedValue(expected, () => _rfm69.IntermediateMode, (v) => _rfm69.IntermediateMode = v);
        }

        [Fact]
        public void TestListenCoefficientIdle()
        {
            TestRange(() => _rfm69.ListenCoefficientIdle, (v) => _rfm69.ListenCoefficientIdle = v);
        }

        [Fact]
        public void TestListenCoefficientRx()
        {
            TestRange(() => _rfm69.ListenCoefficientRx, (v) => _rfm69.ListenCoefficientRx = v);
        }

        [Fact]
        public void TestListenCriteria()
        {
            TestRangeBool(() => _rfm69.ListenCriteria, (v) => _rfm69.ListenCriteria = v);
        }

        [Theory]
        [InlineData(ListenEnd.Idle)]
        [InlineData(ListenEnd.Mode)]
        [InlineData(ListenEnd.Reserved)]
        [InlineData(ListenEnd.Rx)]
        public void TestListenEnd(ListenEnd expected)
        {
            TestAssignedValue(expected, () => _rfm69.ListenEnd, (v) => _rfm69.ListenEnd = v);
        }

        [Fact]
        public void TestListenerOn()
        {
            TestRangeBool(() => _rfm69.ListenerOn, (v) => _rfm69.ListenerOn = v);
        }

        [Theory]
        [InlineData(ListenResolution.Reserved)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle262ms)]
        public void TestListenResolutionIdle(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _rfm69.ListenResolutionIdle, (v) => _rfm69.ListenResolutionIdle = v);
        }

        [Theory]
        [InlineData(ListenResolution.Reserved)]
        [InlineData(ListenResolution.Idle64us)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle262ms)]
        public void TestListenResolutionRx(ListenResolution expected)
        {
            TestAssignedValue(expected, () => _rfm69.ListenResolutionRx, (v) => _rfm69.ListenResolutionRx = v);
        }

        [Fact]
        public void TestLowBetaAfcOffset()
        {
            TestRange(() => _rfm69.LowBetaAfcOffset, (v) => _rfm69.LowBetaAfcOffset = v);
        }

        [Fact]
        public void TestOutputPower()
        {
            TestRange<sbyte>(() => _rfm69.OutputPower, (v) => _rfm69.OutputPower = v, -2, 20);
        }

        [Fact]
        public void TestPayloadLength()
        {
            TestRange<ushort>(() => _rfm69.PayloadLength, (v) => _rfm69.PayloadLength = v, 0, 0xFF);
        }

        [Fact]
        public void TestRssiThreshold()
        {
            TestRange<sbyte>(() => _rfm69.RssiThreshold, (v) => _rfm69.RssiThreshold = v, -115, 0);
        }

        [Fact]
        public void TestRxBw()
        {
            TestRange<byte>(() => RfmBase.RxBw, (v) => RfmBase.RxBw = v, 0, 23);
        }

        [Fact]
        public void TestSensitivityBoost()
        {
            TestRangeBool(() => _rfm69.SensitivityBoost, (v) => _rfm69.SensitivityBoost = v);
        }

        [Fact]
        public void TestSequencer()
        {
            TestRangeBool(() => _rfm69.Sequencer, (v) => _rfm69.Sequencer = v);
        }

        [Fact]
        public void TestSetAesKey()
        {
            _rfm69.SetAesKey(new List<byte> { 0xAA, 0x55, 0xAA, 0x55 });
        }

        [Fact]
        public void TestSetIrqFlags()
        {
            _rfm69.ExecuteReset();
            _rfm69.IrqFlags = Rfm69IrqFlags.FifoOverrun | Rfm69IrqFlags.SyncAddressMatch | Rfm69IrqFlags.Rssi;
        }

        [Fact]
        public void TestSyncBitErrors()
        {
            TestRange<byte>(() => _rfm69.SyncBitErrors, (v) => _rfm69.SyncBitErrors = v, 0, 7);
        }

        [Fact]
        public void TestTimeoutRxStart()
        {
            TestRange(() => _rfm69.TimeoutRxStart, (v) => _rfm69.TimeoutRxStart = v);
        }
    }
}