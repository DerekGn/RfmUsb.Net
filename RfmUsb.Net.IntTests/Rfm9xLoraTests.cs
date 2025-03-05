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

// Ignore Spelling: Agc Irq Lora Rfm Rssi Rx Tx

using System.Runtime.CompilerServices;
using Xunit;

namespace RfmUsb.Net.IntTests
{
    public class Rfm9xLoraTests : RfmTestCommon, IClassFixture<TestFixture<IRfm9x>>
    {
        private readonly TestFixture<IRfm9x> _fixture;

        public Rfm9xLoraTests(TestFixture<IRfm9x> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestAccessSharedRegisters()
        {
            TestRangeBool(() => _fixture.Device.AccessSharedRegisters, (v) => _fixture.Device.AccessSharedRegisters = v);
        }

        [Theory]
        [InlineData(AutoRestartRxMode.Off)]
        [InlineData(AutoRestartRxMode.OnWaitForPllLock)]
        [InlineData(AutoRestartRxMode.OnWithoutPllRelock)]
        [InlineData(AutoRestartRxMode.Reserved)]
        public void TestAutoRestartRxMode(AutoRestartRxMode expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.AutoRestartRxMode, (v) => _fixture.Device.AutoRestartRxMode = v);
        }

        [Theory]
        [InlineData(ModemBandwidth.Bandwidth10_4KHZ)]
        [InlineData(ModemBandwidth.Bandwidth125KHZ)]
        [InlineData(ModemBandwidth.Bandwidth15_6KHZ)]
        [InlineData(ModemBandwidth.Bandwidth20_8KHZ)]
        [InlineData(ModemBandwidth.Bandwidth250KHZ)]
        [InlineData(ModemBandwidth.Bandwidth31_25KHZ)]
        [InlineData(ModemBandwidth.Bandwidth41_7KHZ)]
        [InlineData(ModemBandwidth.Bandwidth500KHZ)]
        [InlineData(ModemBandwidth.Bandwidth62_5KHZ)]
        [InlineData(ModemBandwidth.Bandwidth7_8KHZ)]
        public void TestModemBandwidth(ModemBandwidth expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.ModemBandwidth, (v) => _fixture.Device.ModemBandwidth = v);
        }

        [Theory]
        [InlineData(SpreadingFactor.SpreadFactor1024)]
        [InlineData(SpreadingFactor.SpreadFactor128)]
        [InlineData(SpreadingFactor.SpreadFactor2048)]
        [InlineData(SpreadingFactor.SpreadFactor256)]
        [InlineData(SpreadingFactor.SpreadFactor4096)]
        [InlineData(SpreadingFactor.SpreadFactor512)]
        [InlineData(SpreadingFactor.SpreadFactor64)]
        public void TestSpreadingFactor(SpreadingFactor expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.SpreadingFactor, (v) => _fixture.Device.SpreadingFactor = v);
        }

        [Fact]
        public void TestBeaconOn()
        {
            TestRangeBool(() => _fixture.Device.BeaconOn, (v) => _fixture.Device.BeaconOn = v);
        }

        [Theory]
        [InlineData(ErrorCodingRate.FourEight)]
        [InlineData(ErrorCodingRate.FourFive)]
        [InlineData(ErrorCodingRate.FourSeven)]
        [InlineData(ErrorCodingRate.FourSix)]
        public void TestErrorCodingRate(ErrorCodingRate expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.ErrorCodingRate, (v) => _fixture.Device.ErrorCodingRate = v);
        }

        [Fact]
        public void TestFifoAddressPointer()
        {
            TestRange(() => _fixture.Device.FifoAddressPointer, (v) => _fixture.Device.FifoAddressPointer = v);
        }

        [Fact]
        public void TestFifoRxBaseAddress()
        {
            TestRange(() => _fixture.Device.FifoRxBaseAddress, (v) => _fixture.Device.FifoRxBaseAddress = v);
        }

        [Fact]
        public void TestFifoRxBytesNumber()
        {
            Assert.Equal(0, _fixture.Device.FifoRxBytesNumber);
        }

        [Fact]
        public void TestFifoRxCurrentAddress()
        {
            Assert.Equal(0, _fixture.Device.FifoRxCurrentAddress);
        }

        [Fact]
        public void TestFifoTxBaseAddress()
        {
            TestRange(() => _fixture.Device.FifoTxBaseAddress, (v) => _fixture.Device.FifoTxBaseAddress = v);
        }

        [Fact]
        public void TestFrequencyHoppingPeriod()
        {
            TestRange(() => _fixture.Device.FrequencyHoppingPeriod, (v) => _fixture.Device.FrequencyHoppingPeriod = v);
        }

        [Fact]
        public void TestGetFrequencyError()
        {
            Assert.Equal(0, _fixture.Device.FrequencyError);
        }

        [Fact]
        public void TestGetHopChannel()
        {
            var hopChannel = _fixture.Device.HopChannel;

            Assert.Equal(0, hopChannel.Channel);
        }

        [Fact]
        public void TestGetLoraIrqFlags()
        {
            _fixture.Device.ExecuteReset();

            Assert.Equal(
                    LoraIrqFlags.CadDetected |
                    LoraIrqFlags.CadDone |
                    LoraIrqFlags.ValidHeader, _fixture.Device.LoraIrqFlags);
        }

        [Fact]
        public void TestGetLoraIrqFlagsMask()
        {
            _fixture.Device.ExecuteReset();

            Assert.Equal(
                LoraIrqFlagsMask.ValidHeaderMask | LoraIrqFlagsMask.RxDoneMask,
                _fixture.Device.LoraIrqFlagsMask & (LoraIrqFlagsMask.ValidHeaderMask | LoraIrqFlagsMask.RxDoneMask));
        }

        [Fact]
        public void TestImplicitHeaderModeOn()
        {
            TestRangeBool(() => _fixture.Device.ImplicitHeaderModeOn, (v) => _fixture.Device.ImplicitHeaderModeOn = v);
        }

        [Fact]
        public void TestLongRangeMode()
        {
            TestRangeBool(() => _fixture.Device.LongRangeMode, (v) => _fixture.Device.LongRangeMode = v);
        }

        [Fact]
        public void TestLoraAgcAutoOn()
        {
            TestRangeBool(() => _fixture.Device.LoraAgcAutoOn, (v) => _fixture.Device.LoraAgcAutoOn = v);
        }

        [Theory]
        [InlineData(LoraMode.RxContinuous)]
        [InlineData(LoraMode.RxSingle)]
        [InlineData(LoraMode.Sleep)]
        [InlineData(LoraMode.Standby)]
        [InlineData(LoraMode.SynthRx)]
        [InlineData(LoraMode.SynthTx)]
        [InlineData(LoraMode.Tx)]
        public void TestLoraMode(LoraMode expected)
        {
            TestAssignedValue(expected, () => _fixture.Device.LoraMode, (v) => _fixture.Device.LoraMode = v);
        }

        [Fact]
        public void TestLoraModeCad()
        {
            // Cad auto transitions to standby
            _fixture.Device.LoraMode = LoraMode.Cad;
        }

        [Fact]
        public void TestLoraPayloadLength()
        {
            _fixture.Device.ImplicitHeaderModeOn = true;

            TestRange<byte>(() => _fixture.Device.LoraPayloadLength, (v) => _fixture.Device.LoraPayloadLength = v, 1, 0xFF);
        }

        [Fact]
        public void TestLowDataRateOptimize()
        {
            TestRangeBool(() => _fixture.Device.LowDataRateOptimize, (v) => _fixture.Device.LowDataRateOptimize = v);
        }

        [Fact]
        public void TestModemStatus()
        {
            Assert.Equal(ModemStatus.None, _fixture.Device.ModemStatus);
        }

        [Fact]
        public void TestPacketRssi()
        {
            Assert.Equal(0, _fixture.Device.PacketRssi);
        }

        [Fact]
        public void TestPacketSnr()
        {
            Assert.Equal(0, _fixture.Device.LastPacketSnr);
        }

        [Fact]
        public void TestPayloadMaxLength()
        {
            TestRange(() => _fixture.Device.PayloadMaxLength, (v) => _fixture.Device.PayloadMaxLength = v);
        }

        [Fact]
        public void TestPpmCorrection()
        {
            TestRange(() => _fixture.Device.PpmCorrection, (v) => _fixture.Device.PpmCorrection = v);
        }

        [Fact]
        public void TestPreambleLength()
        {
            TestRange(() => _fixture.Device.PreambleLength, (v) => _fixture.Device.PreambleLength = v);
        }

        [Fact]
        public void TestRssiWideband()
        {
            Assert.Equal(0, _fixture.Device.RssiWideband);
        }

        [Fact]
        public void TestRxCodingRate()
        {
            Assert.Equal(0, _fixture.Device.RxCodingRate);
        }

        [Fact]
        public void TestRxPayloadCrcOn()
        {
            TestRangeBool(() => _fixture.Device.RxPayloadCrcOn, (v) => _fixture.Device.RxPayloadCrcOn = v);
        }

        [Fact]
        public void TestSetLoraIrqFlags()
        {
            _fixture.Device.ExecuteReset();
            _fixture.Device.LoraIrqFlags =
                LoraIrqFlags.CadDetected |
                LoraIrqFlags.FhssChangeChannel |
                LoraIrqFlags.CadDone |
                LoraIrqFlags.TxDone |
                LoraIrqFlags.ValidHeader |
                LoraIrqFlags.PayloadCrcError |
                LoraIrqFlags.CadDetected |
                LoraIrqFlags.RxTimeout;
        }

        [Fact]
        public void TestSetLoraIrqFlagsMask()
        {
            _fixture.Device.ExecuteReset();
            _fixture.Device.LoraIrqFlagsMask =
                LoraIrqFlagsMask.CadDetectedMask |
                LoraIrqFlagsMask.FhssChangeChannelMask |
                LoraIrqFlagsMask.CadDoneMask |
                LoraIrqFlagsMask.TxDoneMask |
                LoraIrqFlagsMask.ValidHeaderMask |
                LoraIrqFlagsMask.PayloadCrcErrorMask |
                LoraIrqFlagsMask.CadDetectedMask |
                LoraIrqFlagsMask.RxTimeoutMask;
        }

        [Fact]
        public void TestSymbolTimeout()
        {
            TestRange<ushort>(() => _fixture.Device.SymbolTimeout, (v) => _fixture.Device.SymbolTimeout = v, 0, 0x3FF);
        }

        [Fact]
        public void TestTxContinuousMode()
        {
            TestRangeBool(() => _fixture.Device.TxContinuousMode, (v) => _fixture.Device.TxContinuousMode = v);
        }

        [Fact]
        public void TestValidHeaderCount()
        {
            Assert.Equal(0, _fixture.Device.ValidHeaderCount);
        }

        [Fact]
        public void TestValidPacketCount()
        {
            Assert.Equal(0, _fixture.Device.ValidPacketCount);
        }
    }
}