using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace RfmUsb.Tests
{
    public class DeviceTests : IClassFixture<DeviceTestsFixture>
    {
        private readonly DeviceTestsFixture _fixture;

        public DeviceTests(DeviceTestsFixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;

            _fixture.RfmUsbDevice.Reset();
        }

        [Theory]
        [InlineData(AddressFilter.Node)]
        [InlineData(AddressFilter.None)]
        [InlineData(AddressFilter.NodeBroaddcast)]
        public void TestAddressFilter(AddressFilter addressFilter)
        {
            _fixture.RfmUsbDevice.AddressFiltering = addressFilter;

            _fixture.RfmUsbDevice.AddressFiltering.Should().Be(addressFilter);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestAesOn(bool value)
        {
            _fixture.RfmUsbDevice.AesOn = value;

            _fixture.RfmUsbDevice.AesOn.Should().Be(value);
        }

        [Theory]
        [InlineData(ushort.MinValue)]
        public void TestAfc(ushort value)
        {
            _fixture.RfmUsbDevice.Afc.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestAfcAutoClear(bool value)
        {
            _fixture.RfmUsbDevice.AfcAutoClear = value;

            _fixture.RfmUsbDevice.AfcAutoClear.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestAfcAutoOn(bool value)
        {
            _fixture.RfmUsbDevice.AfcAutoOn = value;

            _fixture.RfmUsbDevice.AfcAutoOn.Should().Be(value);
        }

        [Fact]
        public void TestAfcClear()
        {
            _fixture.RfmUsbDevice.AfcClear();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestAfcLowBetaOn(bool value)
        {
            _fixture.RfmUsbDevice.AfcLowBetaOn = value;

            _fixture.RfmUsbDevice.AfcLowBetaOn.Should().Be(value);
        }

        [Fact]
        public void TestAfcStart()
        {
            _fixture.RfmUsbDevice.AfcStart();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestAutoRxRestartOn(bool value)
        {
            _fixture.RfmUsbDevice.AutoRxRestartOn = value;

            _fixture.RfmUsbDevice.AutoRxRestartOn.Should().Be(value);
        }

        [Theory]
        [InlineData(9600)]
        [InlineData(32000)]
        public void TestBitRate(ushort value)
        {
            _fixture.RfmUsbDevice.BitRate = value;

            _fixture.RfmUsbDevice.BitRate.Should().BeCloseTo(value, 10);
        }

        [Theory]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        public void TestBroadcastAddress(byte value)
        {
            _fixture.RfmUsbDevice.BroadcastAddress = value;

            _fixture.RfmUsbDevice.BroadcastAddress.Should().Be(value);
        }

        [Theory]
        [InlineData(ContinuousDagc.Normal)]
        [InlineData(ContinuousDagc.ImprovedLowBeta0)]
        [InlineData(ContinuousDagc.ImprovedLowBeta1)]
        public void TestContinuousDagc(ContinuousDagc value)
        {
            _fixture.RfmUsbDevice.ContinuousDagc = value;

            _fixture.RfmUsbDevice.ContinuousDagc.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestCrcAutoClear(bool value)
        {
            _fixture.RfmUsbDevice.CrcAutoClear = value;

            _fixture.RfmUsbDevice.CrcAutoClear.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestCrcOn(bool value)
        {
            _fixture.RfmUsbDevice.CrcOn = value;

            _fixture.RfmUsbDevice.CrcOn.Should().Be(value);
        }

        [Fact]
        public void TestCurrentLnaGain()
        {
            _fixture.RfmUsbDevice.CurrentLnaGain.Should().Be(LnaGain.Max);
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent16)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        public void TestDccFreq(DccFreq value)
        {
            _fixture.RfmUsbDevice.DccFreq = value;

            _fixture.RfmUsbDevice.DccFreq.Should().Be(value);
        }

        [Theory]
        [InlineData(DccFreq.FreqPercent0_125)]
        [InlineData(DccFreq.FreqPercent0_25)]
        [InlineData(DccFreq.FreqPercent0_5)]
        [InlineData(DccFreq.FreqPercent1)]
        [InlineData(DccFreq.FreqPercent16)]
        [InlineData(DccFreq.FreqPercent2)]
        [InlineData(DccFreq.FreqPercent4)]
        [InlineData(DccFreq.FreqPercent8)]
        public void TestDccFreqAfc(DccFreq value)
        {
            _fixture.RfmUsbDevice.DccFreqAfc = value;

            _fixture.RfmUsbDevice.DccFreqAfc.Should().Be(value);
        }

        [Theory]
        [InlineData(DcFree.Manchester)]
        [InlineData(DcFree.None)]
        [InlineData(DcFree.Reserved)]
        [InlineData(DcFree.Whitening)]
        public void TestDcFree(DcFree value)
        {
            _fixture.RfmUsbDevice.DcFree = value;

            _fixture.RfmUsbDevice.DcFree.Should().Be(value);
        }

        [Theory]
        [InlineData(DioIrq.None)]
        [InlineData(DioIrq.Dio0 | DioIrq.Dio1 | DioIrq.Dio2 | DioIrq.Dio3 | DioIrq.Dio4 | DioIrq.Dio5)]
        public void TestDioInterruptMask(DioIrq value)
        {
            _fixture.RfmUsbDevice.DioInterruptMask = value;

            _fixture.RfmUsbDevice.DioInterruptMask.Should().Be(value);
        }

        [Theory]
        [InlineData(EnterCondition.CrcOk)]
        [InlineData(EnterCondition.FifoEmpty)]
        [InlineData(EnterCondition.FifoLevel)]
        [InlineData(EnterCondition.FifoNotEmpty)]
        [InlineData(EnterCondition.Off)]
        [InlineData(EnterCondition.PacketSent)]
        [InlineData(EnterCondition.PayloadReady)]
        [InlineData(EnterCondition.SyncAddressMatch)]
        public void TestEnterCondition(EnterCondition value)
        {
            _fixture.RfmUsbDevice.EnterCondition = value;

            _fixture.RfmUsbDevice.EnterCondition.Should().Be(value);
        }

        [Theory]
        [InlineData(ExitCondition.CrcOk)]
        [InlineData(ExitCondition.FifoEmpty)]
        [InlineData(ExitCondition.FifoLevel)]
        [InlineData(ExitCondition.Off)]
        [InlineData(ExitCondition.PacketSent)]
        [InlineData(ExitCondition.PayloadReady)]
        [InlineData(ExitCondition.SyncAddressMatch)]
        public void TestExitCondition(ExitCondition value)
        {
            _fixture.RfmUsbDevice.ExitCondition = value;

            _fixture.RfmUsbDevice.ExitCondition.Should().Be(value);
        }

        [Fact]
        public void TestFei()
        {
            _fixture.RfmUsbDevice.Fei.Should().Be(0);
        }

        [Fact]
        public void TestFeiStart()
        {
            _fixture.RfmUsbDevice.FeiStart();
        }

        [Fact]
        public void TestFifo()
        {
            _fixture.RfmUsbDevice.Fifo = new List<byte>() { 0xAA, 0x55 };

            _fixture.RfmUsbDevice.Fifo.Should().StartWith(new List<byte>() { 0xAA, 0x55 });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestFifoFill(bool value)
        {
            _fixture.RfmUsbDevice.FifoFill = value;

            _fixture.RfmUsbDevice.FifoFill.Should().Be(value);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(byte.MinValue)]
        public void TestFifoThreshold(byte value)
        {
            _fixture.RfmUsbDevice.FifoThreshold = value;

            _fixture.RfmUsbDevice.FifoThreshold.Should().Be(value);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(ushort.MinValue)]
        public void TestFreqencyDeviation(ushort value)
        {
            _fixture.RfmUsbDevice.FreqencyDeviation = value;

            _fixture.RfmUsbDevice.FreqencyDeviation.Should().Be(value);
        }

        [Theory]
        [InlineData(433000000u)]
        [InlineData(868000000u)]
        public void TestFreqency(uint value)
        {
            _fixture.RfmUsbDevice.Frequency = value;

            _fixture.RfmUsbDevice.Frequency.Should().Be(value);
        }

        [Theory]
        [InlineData(FskModulationShaping.None)]
        [InlineData(FskModulationShaping.GaussianBt1_0)]
        [InlineData(FskModulationShaping.GaussianBt0_5)]
        [InlineData(FskModulationShaping.GaussianBt0_3)]
        public void TestFskModulationShaping(FskModulationShaping value)
        {
            _fixture.RfmUsbDevice.FskModulationShaping = value;

            _fixture.RfmUsbDevice.FskModulationShaping.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestImpedance(bool value)
        {
            _fixture.RfmUsbDevice.Impedance = value;

            _fixture.RfmUsbDevice.Impedance.Should().Be(value);
        }

        [Theory]
        [InlineData(IntermediateMode.Rx)]
        [InlineData(IntermediateMode.Sleep)]
        [InlineData(IntermediateMode.Standby)]
        [InlineData(IntermediateMode.Tx)]
        public void TestIntermediateMode(IntermediateMode value)
        {
            _fixture.RfmUsbDevice.IntermediateMode = value;

            _fixture.RfmUsbDevice.IntermediateMode.Should().Be(value);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(byte.MinValue)]
        public void TestInterPacketRxDelay(byte value)
        {
            _fixture.RfmUsbDevice.InterPacketRxDelay = value;

            _fixture.RfmUsbDevice.InterPacketRxDelay.Should().Be(value);
        }

        [Fact()]
        public void TestIrq()
        {
            _fixture.RfmUsbDevice.Irq.Should().Be(0);
        }

        [Fact]
        public void TestListenAbort()
        {
            _fixture.RfmUsbDevice.ListenAbort();
        }

        [Theory]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        public void TestListenCoefficentIdle(byte value)
        {
            _fixture.RfmUsbDevice.ListenCoefficentIdle = value;

            _fixture.RfmUsbDevice.ListenCoefficentIdle.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        public void TestListenCoefficentRx(byte value)
        {
            _fixture.RfmUsbDevice.ListenCoefficentRx = value;

            _fixture.RfmUsbDevice.ListenCoefficentRx.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestListenCriteria(bool value)
        {
            _fixture.RfmUsbDevice.ListenCriteria = value;

            _fixture.RfmUsbDevice.ListenCriteria.Should().Be(value);
        }

        [Theory]
        [InlineData(ListenEnd.Idle)]
        [InlineData(ListenEnd.Mode)]
        [InlineData(ListenEnd.Reserved)]
        [InlineData(ListenEnd.Rx)]
        public void TestListenEnd(ListenEnd value)
        {
            _fixture.RfmUsbDevice.ListenEnd = value;

            _fixture.RfmUsbDevice.ListenEnd.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestListenOn(bool value)
        {
            _fixture.RfmUsbDevice.ListenerOn = value;

            _fixture.RfmUsbDevice.ListenerOn.Should().Be(value);
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        public void TestListenResolutionIdle(ListenResolution value)
        {
            _fixture.RfmUsbDevice.ListenResolutionIdle = value;

            _fixture.RfmUsbDevice.ListenResolutionIdle.Should().Be(value);
        }

        [Theory]
        [InlineData(ListenResolution.Idle262ms)]
        [InlineData(ListenResolution.Idle4_1ms)]
        [InlineData(ListenResolution.Idle64us)]
        public void TestListenResolutionRx(ListenResolution value)
        {
            _fixture.RfmUsbDevice.ListenResolutionRx = value;

            _fixture.RfmUsbDevice.ListenResolutionRx.Should().Be(value);
        }

        [Theory]
        [InlineData(LnaGain.Auto)]
        [InlineData(LnaGain.Max)]
        [InlineData(LnaGain.MaxMinus12db)]
        [InlineData(LnaGain.MaxMinus24db)]
        [InlineData(LnaGain.MaxMinus36db)]
        [InlineData(LnaGain.MaxMinus48db)]
        [InlineData(LnaGain.MaxMinus6db)]
        public void TestLnaGain(LnaGain value)
        {
            _fixture.RfmUsbDevice.LnaGainSelect = value;

            _fixture.RfmUsbDevice.LnaGainSelect.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        public void TestLowBetaAfcOffset(byte value)
        {
            _fixture.RfmUsbDevice.LowBetaAfcOffset = value;

            _fixture.RfmUsbDevice.LowBetaAfcOffset.Should().Be(value);
        }


        [Fact]
        public void TestMeasureTemperature()
        {
            _fixture.RfmUsbDevice.MeasureTemperature();
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestMode(Mode value)
        {
            _fixture.RfmUsbDevice.Mode = value;

            _fixture.RfmUsbDevice.Mode.Should().Be(value);
        }

        [Theory]
        [InlineData(Modulation.Fsk)]
        [InlineData(Modulation.Ook)]
        public void TestModulation(Modulation value)
        {
            _fixture.RfmUsbDevice.Modulation = value;

            _fixture.RfmUsbDevice.Modulation.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        public void TestNodeAddress(byte value)
        {
            _fixture.RfmUsbDevice.NodeAddress = value;

            _fixture.RfmUsbDevice.NodeAddress.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOcpEnable(bool value)
        {
            _fixture.RfmUsbDevice.OcpEnable = value;

            _fixture.RfmUsbDevice.OcpEnable.Should().Be(value);
        }

        [Theory]
        [InlineData(OcpTrim.OcpTrim100)]
        [InlineData(OcpTrim.OcpTrim95)]
        public void TestOcpTrim(OcpTrim value)
        {
            _fixture.RfmUsbDevice.OcpTrim = value;

            _fixture.RfmUsbDevice.OcpTrim.Should().Be(value);
        }

        [Theory]
        [InlineData(OokAverageThresholdFilter.ChipRate2)]
        [InlineData(OokAverageThresholdFilter.ChipRate32)]
        [InlineData(OokAverageThresholdFilter.ChipRate4)]
        [InlineData(OokAverageThresholdFilter.ChipRate8)]
        public void TestOokAverageThresholdFilter(OokAverageThresholdFilter value)
        {
            _fixture.RfmUsbDevice.OokAverageThresholdFilter = value;

            _fixture.RfmUsbDevice.OokAverageThresholdFilter.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        public void TestOokFixedThreshold(byte value)
        {
            _fixture.RfmUsbDevice.OokFixedThreshold = value;

            _fixture.RfmUsbDevice.OokFixedThreshold.Should().Be(value);
        }

        [Theory]
        [InlineData(OokModulationShaping.Filtering2Br)]
        [InlineData(OokModulationShaping.FilteringBr)]
        [InlineData(OokModulationShaping.None)]
        [InlineData(OokModulationShaping.Reserved)]
        public void TestOokModulationShaping(OokModulationShaping value)
        {
            _fixture.RfmUsbDevice.OokModulationShaping = value;

            _fixture.RfmUsbDevice.OokModulationShaping.Should().Be(value);
        }

        [Theory]
        [InlineData(OokThresholdDec.EightTimesInEachChip)]
        [InlineData(OokThresholdDec.FourTimesInEachChip)]
        [InlineData(OokThresholdDec.OnceEvery2Chips)]
        [InlineData(OokThresholdDec.OnceEvery4Chips)]
        [InlineData(OokThresholdDec.OnceEvery8Chips)]
        [InlineData(OokThresholdDec.OncePerChip)]
        [InlineData(OokThresholdDec.SixteeenTimesInEachChip)]
        [InlineData(OokThresholdDec.TwiceInEachChip)]
        public void TestOokPeakThresholdDec(OokThresholdDec value)
        {
            _fixture.RfmUsbDevice.OokPeakThresholdDec = value;

            _fixture.RfmUsbDevice.OokPeakThresholdDec.Should().Be(value);
        }

        [Theory]
        [InlineData(OokThresholdStep.Step0_5db)]
        [InlineData(OokThresholdStep.Step1_0db)]
        [InlineData(OokThresholdStep.Step1_5db)]
        [InlineData(OokThresholdStep.Step2_0db)]
        [InlineData(OokThresholdStep.Step3_0db)]
        [InlineData(OokThresholdStep.Step4_0db)]
        [InlineData(OokThresholdStep.Step5_0db)]
        [InlineData(OokThresholdStep.Step6_0db)]
        public void TestOokPeakThresholdStep(OokThresholdStep value)
        {
            _fixture.RfmUsbDevice.OokPeakThresholdStep = value;

            _fixture.RfmUsbDevice.OokPeakThresholdStep.Should().Be(value);
        }

        [Theory]
        [InlineData(OokThresholdType.Average)]
        [InlineData(OokThresholdType.Fixed)]
        [InlineData(OokThresholdType.Peak)]
        [InlineData(OokThresholdType.Reserved)]
        public void TestOokThresholdType(OokThresholdType value)
        {
            _fixture.RfmUsbDevice.OokThresholdType = value;

            _fixture.RfmUsbDevice.OokThresholdType.Should().Be(value);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(byte.MinValue)]
        public void TestOutputPower(byte value)
        {
            _fixture.RfmUsbDevice.OutputPower = value;

            _fixture.RfmUsbDevice.OutputPower.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestPacketFormat(bool value)
        {
            _fixture.RfmUsbDevice.PacketFormat = value;

            _fixture.RfmUsbDevice.PacketFormat.Should().Be(value);
        }

        [Theory]
        [InlineData(PaRamp.PowerAmpRamp10)]
        [InlineData(PaRamp.PowerAmpRamp62)]
        public void TestPaRamp(PaRamp value)
        {
            _fixture.RfmUsbDevice.PaRamp = value;

            _fixture.RfmUsbDevice.PaRamp.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(50)]
        public void TestPayloadLength(byte value)
        {
            _fixture.RfmUsbDevice.PayloadLength = value;

            _fixture.RfmUsbDevice.PayloadLength.Should().Be(value);
        }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        public void TestPreambleSize(ushort value)
        {
            _fixture.RfmUsbDevice.PreambleSize = value;

            _fixture.RfmUsbDevice.PreambleSize.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(10)]
        public void TestRadioConfig(byte value)
        {
            _fixture.RfmUsbDevice.RadioConfig = value;

            _fixture.RfmUsbDevice.RadioConfig.Should().Be(value);
        }

        [Fact]
        public void TestRcCalibration()
        {
            _fixture.RfmUsbDevice.RcCalibration();
        }

        [Fact]
        public void TestReset()
        {
            _fixture.RfmUsbDevice.Reset();
        }

        [Fact]
        public void TestRestartRx()
        {
            _fixture.RfmUsbDevice.RestartRx();
        }

        [Fact]
        public void TestRssi()
        {
            _fixture.RfmUsbDevice.Rssi.Should().Be(0xFF);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        public void TestRssiThreshold(byte value)
        {
            _fixture.RfmUsbDevice.RssiThreshold = value;

            _fixture.RfmUsbDevice.RssiThreshold.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(23)]
        public void TestRxBw(byte value)
        {
            _fixture.RfmUsbDevice.RxBw = value;

            _fixture.RfmUsbDevice.RxBw.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(23)]
        public void TestRxBwAfc(byte value)
        {
            _fixture.RfmUsbDevice.RxBwAfc = value;

            _fixture.RfmUsbDevice.RxBwAfc.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSensitivityBoost(bool value)
        {
            _fixture.RfmUsbDevice.SensitivityBoost = value;

            _fixture.RfmUsbDevice.SensitivityBoost.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSequencer(bool value)
        {
            _fixture.RfmUsbDevice.Sequencer = value;

            _fixture.RfmUsbDevice.Sequencer.Should().Be(value);
        }

        [Fact]
        public void TestSetAesKey()
        {
            _fixture.RfmUsbDevice.SetAesKey(new List<byte>() { 0xAA, 0x55 });
        }

        [Theory]
        [InlineData(Dio.Dio0, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio0, DioMapping.DioMapping1)]
        [InlineData(Dio.Dio0, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio0, DioMapping.DioMapping3)]
        [InlineData(Dio.Dio1, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio1, DioMapping.DioMapping1)]
        [InlineData(Dio.Dio1, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio1, DioMapping.DioMapping3)]
        [InlineData(Dio.Dio2, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio2, DioMapping.DioMapping1)]
        [InlineData(Dio.Dio2, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio2, DioMapping.DioMapping3)]
        [InlineData(Dio.Dio3, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio3, DioMapping.DioMapping1)]
        [InlineData(Dio.Dio3, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio3, DioMapping.DioMapping3)]
        [InlineData(Dio.Dio4, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio4, DioMapping.DioMapping1)]
        public void TestSetDioMapping(Dio dio, DioMapping dioMapping)
        {
            _fixture.RfmUsbDevice.SetDioMapping(dio, dioMapping);

            _fixture.RfmUsbDevice.GetDioMapping(dio, out var dioMappingValue);

            dioMappingValue.Should().Be(dioMapping);
        }

        [Fact]
        public void TestStartRssi()
        {
            _fixture.RfmUsbDevice.StartRssi();
        }

        [Fact]
        public void TestSync()
        {
            var sync = new List<byte>() { 0x55, 0xAA };

            _fixture.RfmUsbDevice.Sync = sync;
            _fixture.RfmUsbDevice.Sync.Should().StartWith(sync);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(7)]
        public void TestSyncBitErrors(byte value)
        {
            _fixture.RfmUsbDevice.SyncBitErrors = value;

            _fixture.RfmUsbDevice.SyncBitErrors.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSyncEnable(bool value)
        {
            _fixture.RfmUsbDevice.SyncEnable = value;

            _fixture.RfmUsbDevice.SyncEnable.Should().Be(value);
        }

        [Fact]
        public void TestTemperatureValue()
        {
            _fixture.RfmUsbDevice.TemperatureValue.Should().Be(0);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        public void TestTimeoutRssiThreshold(byte value)
        {
            _fixture.RfmUsbDevice.TimeoutRssiThreshold = value;

            _fixture.RfmUsbDevice.TimeoutRssiThreshold.Should().Be(value);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        public void TestTimeoutRxStart(byte value)
        {
            _fixture.RfmUsbDevice.TimeoutRxStart = value;

            _fixture.RfmUsbDevice.TimeoutRxStart.Should().Be(value);
        }

        //[Fact]
        //public void TestTransmit()
        //{
        //    _fixture.RfmUsbDevice.Transmit(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA });
        //}

        //[Fact]
        //public void TestTransmitReceive()
        //{
        //    _fixture.RfmUsbDevice.TransmitReceive(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA }, 1000);
        //}

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestTxStartCondition(bool value)
        {
            _fixture.RfmUsbDevice.TxStartCondition = value;

            _fixture.RfmUsbDevice.TxStartCondition.Should().Be(value);
        }

        [Fact]
        public void TestVersion()
        {
            _fixture.RfmUsbDevice.Version.Should().NotBeNullOrWhiteSpace();
        }
    }
}
