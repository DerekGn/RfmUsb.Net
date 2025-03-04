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

using Xunit;

namespace RfmUsb.Net.IntTests
{
    public abstract class RfmTestCommon : RfmTestBase
    {
        [Theory]
        [InlineData(AddressFilter.None)]
        [InlineData(AddressFilter.NodeBroaddcast)]
        [InlineData(AddressFilter.Reserved)]
        public void TestAddressFiltering(AddressFilter expected)
        {
            TestAssignedValue(expected, () => RfmBase.AddressFiltering, (v) => RfmBase.AddressFiltering = v);
        }

        [Fact]
        public void TestAfc()
        {
            Read(() => RfmBase.Afc);
        }

        [Fact]
        public void TestAfcAutoClear()
        {
            TestRangeBool(() => RfmBase.AfcAutoClear, (v) => RfmBase.AfcAutoClear = v);
        }

        [Fact]
        public void TestAfcAutoOn()
        {
            TestRangeBool(() => RfmBase.AfcAutoOn, (v) => RfmBase.AfcAutoOn = v);
        }

        [Fact]
        public void TestBitrate()
        {
            TestRange<uint>(() => RfmBase.BitRate, (v) => RfmBase.BitRate = v, 1200, 299065);
        }

        [Fact]
        public void TestBroadcastAddress()
        {
            TestRange(() => RfmBase.BroadcastAddress, (v) => RfmBase.BroadcastAddress = v);
        }

        [Fact]
        public void TestBufferedIoEnable()
        {
            TestRangeBool(() => RfmBase.BufferedIoEnable, (v) => RfmBase.BufferedIoEnable = v);
        }

        [Fact]
        public void TestCrcAutoClearOff()
        {
            TestRangeBool(() => RfmBase.CrcAutoClearOff, (v) => RfmBase.CrcAutoClearOff = v);
        }

        [Fact]
        public void TestCrcOn()
        {
            TestRangeBool(() => RfmBase.CrcOn, (v) => RfmBase.CrcOn = v);
        }

        [Theory]
        [InlineData(DcFreeEncoding.Manchester)]
        [InlineData(DcFreeEncoding.None)]
        [InlineData(DcFreeEncoding.Reserved)]
        [InlineData(DcFreeEncoding.Whitening)]
        public void TestDcFreeEncoding(DcFreeEncoding expected)
        {
            TestAssignedValue(expected, () => RfmBase.DcFreeEncoding, (v) => RfmBase.DcFreeEncoding = v);
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
        [InlineData(Dio.Dio4, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio4, DioMapping.DioMapping3)]
        [InlineData(Dio.Dio5, DioMapping.DioMapping0)]
        [InlineData(Dio.Dio5, DioMapping.DioMapping1)]
        [InlineData(Dio.Dio5, DioMapping.DioMapping2)]
        [InlineData(Dio.Dio5, DioMapping.DioMapping3)]
        public void TestDioMapping(Dio dio, DioMapping expected)
        {
            RfmBase.SetDioMapping(dio, expected);
            Assert.Equal(expected, RfmBase.GetDioMapping(dio));
        }

        [Fact]
        public void TestFei()
        {
            Read(() => RfmBase.Fei);
        }

        [Fact]
        public void TestFirmwareVersion()
        {
            Read(() => RfmBase.FirmwareVersion);
        }

        [Fact]
        public void TestFrequency()
        {
            TestRange<uint>(() => RfmBase.Frequency, (v) => RfmBase.Frequency = v, 0, 1020000000);
        }

        [Fact]
        public void TestFrequencyDeviation()
        {
            TestRange<ushort>(() => RfmBase.FrequencyDeviation, (v) => RfmBase.FrequencyDeviation = v, 0, 0x3FFF);
        }

        [Theory]
        [InlineData(FskModulationShaping.GaussianBt0_3)]
        [InlineData(FskModulationShaping.GaussianBt0_5)]
        [InlineData(FskModulationShaping.GaussianBt1_0)]
        [InlineData(FskModulationShaping.None)]
        public void TestFskModulationShaping(FskModulationShaping expected)
        {
            TestAssignedValue(expected, () => RfmBase.FskModulationShaping, (v) => RfmBase.FskModulationShaping = v);
        }

        [Fact]
        public void TestInterPacketRxDelay()
        {
            TestRange<byte>(() => RfmBase.InterPacketRxDelay, (v) => RfmBase.InterPacketRxDelay = v, 0, 15);
        }

        [Theory]
        [InlineData(LnaGain.Auto)]
        [InlineData(LnaGain.Max)]
        [InlineData(LnaGain.MaxMinus6db)]
        [InlineData(LnaGain.MaxMinus12db)]
        [InlineData(LnaGain.MaxMinus24db)]
        [InlineData(LnaGain.MaxMinus36db)]
        [InlineData(LnaGain.MaxMinus48db)]
        public void TestLnaGainSelect(LnaGain expected)
        {
            TestAssignedValue(expected, () => RfmBase.LnaGainSelect, (v) => RfmBase.LnaGainSelect = v);
        }

        [Theory]
        [InlineData(Mode.Rx)]
        [InlineData(Mode.Sleep)]
        [InlineData(Mode.Standby)]
        [InlineData(Mode.Synth)]
        [InlineData(Mode.Tx)]
        public void TestMode(Mode expected)
        {
            TestAssignedValue(expected, () => RfmBase.Mode, (v) => RfmBase.Mode = v);

            // Revert to sleep mode.
            RfmBase.Mode = Mode.Sleep;
        }

        [Theory]
        [InlineData(ModulationType.Fsk)]
        [InlineData(ModulationType.Ook)]
        public void TestModulation(ModulationType expected)
        {
            TestAssignedValue(expected, () => RfmBase.ModulationType, (v) => RfmBase.ModulationType = v);
        }

        [Fact]
        public void TestNodeAddress()
        {
            TestRange(() => RfmBase.NodeAddress, (v) => RfmBase.NodeAddress = v);
        }

        [Fact]
        public void TestOcpEnable()
        {
            TestRangeBool(() => RfmBase.OcpEnable, (v) => RfmBase.OcpEnable = v);
        }

        [Theory]
        [InlineData(OcpTrim.OcpTrim45)]
        [InlineData(OcpTrim.OcpTrim50)]
        [InlineData(OcpTrim.OcpTrim55)]
        [InlineData(OcpTrim.OcpTrim60)]
        [InlineData(OcpTrim.OcpTrim65)]
        [InlineData(OcpTrim.OcpTrim70)]
        [InlineData(OcpTrim.OcpTrim75)]
        [InlineData(OcpTrim.OcpTrim80)]
        [InlineData(OcpTrim.OcpTrim85)]
        [InlineData(OcpTrim.OcpTrim90)]
        [InlineData(OcpTrim.OcpTrim95)]
        [InlineData(OcpTrim.OcpTrim100)]
        [InlineData(OcpTrim.OcpTrim105)]
        [InlineData(OcpTrim.OcpTrim110)]
        [InlineData(OcpTrim.OcpTrim115)]
        [InlineData(OcpTrim.OcpTrim120)]
        public void TestOcpTrim(OcpTrim expected)
        {
            TestAssignedValue(expected, () => RfmBase.OcpTrim, (v) => RfmBase.OcpTrim = v);
        }

        [Theory]
        [InlineData(OokAverageThresholdFilter.ChipRate2)]
        [InlineData(OokAverageThresholdFilter.ChipRate4)]
        [InlineData(OokAverageThresholdFilter.ChipRate8)]
        [InlineData(OokAverageThresholdFilter.ChipRate32)]
        public void TestOokAverageThresholdFilter(OokAverageThresholdFilter expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokAverageThresholdFilter, (v) => RfmBase.OokAverageThresholdFilter = v);
        }

        [Fact]
        public void TestOokFixedThreshold()
        {
            TestRange(() => RfmBase.OokFixedThreshold, (v) => RfmBase.OokFixedThreshold = v);
        }

        [Theory]
        [InlineData(OokModulationShaping.Filtering2Br)]
        [InlineData(OokModulationShaping.FilteringBr)]
        [InlineData(OokModulationShaping.None)]
        [InlineData(OokModulationShaping.Reserved)]
        public void TestOokModulationShaping(OokModulationShaping expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokModulationShaping, (v) => RfmBase.OokModulationShaping = v);
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
        public void TestOokPeakThresholdDec(OokThresholdDec expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokPeakThresholdDec, (v) => RfmBase.OokPeakThresholdDec = v);
        }

        [Theory]
        [InlineData(OokThresholdStep.Step0_5db)]
        [InlineData(OokThresholdStep.Step1_5db)]
        [InlineData(OokThresholdStep.Step1db)]
        [InlineData(OokThresholdStep.Step3db)]
        [InlineData(OokThresholdStep.Step4db)]
        [InlineData(OokThresholdStep.Step5db)]
        [InlineData(OokThresholdStep.Step6db)]
        public void TestOokPeakThresholdStep(OokThresholdStep expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokPeakThresholdStep, (v) => RfmBase.OokPeakThresholdStep = v);
        }

        [Theory]
        [InlineData(OokThresholdType.Average)]
        [InlineData(OokThresholdType.Fixed)]
        [InlineData(OokThresholdType.Peak)]
        [InlineData(OokThresholdType.Reserved)]
        public void TestOokThresholdType(OokThresholdType expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokThresholdType, (v) => RfmBase.OokThresholdType = v);
        }

        [Fact]
        public void TestPacketFormat()
        {
            TestRangeBool(() => RfmBase.PacketFormat, (v) => RfmBase.PacketFormat = v);
        }

        [Theory]
        [InlineData(PaRamp.PowerAmpRamp10)]
        [InlineData(PaRamp.PowerAmpRamp12)]
        [InlineData(PaRamp.PowerAmpRamp15)]
        [InlineData(PaRamp.PowerAmpRamp20)]
        [InlineData(PaRamp.PowerAmpRamp25)]
        [InlineData(PaRamp.PowerAmpRamp50)]
        [InlineData(PaRamp.PowerAmpRamp62)]
        [InlineData(PaRamp.PowerAmpRamp31)]
        [InlineData(PaRamp.PowerAmpRamp40)]
        [InlineData(PaRamp.PowerAmpRamp100)]
        [InlineData(PaRamp.PowerAmpRamp250)]
        [InlineData(PaRamp.PowerAmpRamp500)]
        [InlineData(PaRamp.PowerAmpRamp1000)]
        public void TestPaRamp(PaRamp expected)
        {
            TestAssignedValue(expected, () => RfmBase.PaRamp, (v) => RfmBase.PaRamp = v);
        }

        [Fact]
        public void TestPreambleSize()
        {
            TestRange(() => RfmBase.PreambleSize, (v) => RfmBase.PreambleSize = v);
        }

        [Fact]
        public void TestRadioVersion()
        {
            Read(() => RfmBase.RadioVersion);
        }

        [Fact]
        public void TestRcCalibration()
        {
            RfmBase.RcCalibration();
        }

        [Fact]
        public void TestRssi()
        {
            Read<sbyte>(() => RfmBase.Rssi);
        }

        [Fact]
        public void TestSerialNumber()
        {
            Read(() => RfmBase.SerialNumber);
        }

        [Fact]
        public void TestSync()
        {
            var expected = new List<byte>() { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
            RfmBase.Sync = expected;

            Assert.Equal(expected, RfmBase.Sync);
        }

        [Fact]
        public void TestSyncEnable()
        {
            TestRangeBool(() => RfmBase.SyncEnable, (v) => RfmBase.SyncEnable = v);
        }

        [Fact]
        public void TestSyncSize()
        {
            TestRange<byte>(() => RfmBase.SyncSize, (v) => RfmBase.SyncSize = v, 0, 7);
        }

        [Fact]
        public void TestTemperatureValue()
        {
            Read(() => RfmBase.Temperature);
        }

        [Fact]
        public void TestTxStartCondition()
        {
            TestRangeBool(() => RfmBase.TxStartCondition, (v) => RfmBase.TxStartCondition = v);
        }
    }
}