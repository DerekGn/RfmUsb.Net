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

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RfmUsb.Net.IntTests
{
    public class Rfm9xLoraTests : RfmTestBase
    {
        private readonly IRfm9x _rfm9x;

        public Rfm9xLoraTests()
        {
            _rfm9x = _serviceProvider.GetService<IRfm9x>() ?? throw new NullReferenceException($"Unable to resolve {nameof(IRfm9x)}");
            RfmBase = _rfm9x;

            _rfm9x.Open("COM3", 230400);

            _rfm9x.ExecuteReset();

            _rfm9x.Mode = Mode.Sleep;

            _rfm9x.LongRangeMode = true;
        }

        [Fact]
        public void TestAccessSharedRegisters()
        {
            TestRangeBool(() => _rfm9x.AccessSharedRegisters, (v) => _rfm9x.AccessSharedRegisters = v);
        }

        [Theory]
        [InlineData(AutoRestartRxMode.Off)]
        [InlineData(AutoRestartRxMode.OnWaitForPllLock)]
        [InlineData(AutoRestartRxMode.OnWithoutPllRelock)]
        [InlineData(AutoRestartRxMode.Reserved)]
        public void TestAutoRestartRxMode(AutoRestartRxMode expected)
        {
            TestAssignedValue(expected, () => _rfm9x.AutoRestartRxMode, (v) => _rfm9x.AutoRestartRxMode = v);
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
            TestAssignedValue(expected, () => _rfm9x.ModemBandwidth, (v) => _rfm9x.ModemBandwidth = v);
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
            TestAssignedValue(expected, () => _rfm9x.SpreadingFactor, (v) => _rfm9x.SpreadingFactor = v);
        }

        [Fact]
        public void TestBeaconOn()
        {
            TestRangeBool(() => _rfm9x.BeaconOn, (v) => _rfm9x.BeaconOn = v);
        }

        [Theory]
        [InlineData(ErrorCodingRate.FourEight)]
        [InlineData(ErrorCodingRate.FourFive)]
        [InlineData(ErrorCodingRate.FourSeven)]
        [InlineData(ErrorCodingRate.FourSix)]
        public void TestErrorCodingRate(ErrorCodingRate expected)
        {
            TestAssignedValue(expected, () => _rfm9x.ErrorCodingRate, (v) => _rfm9x.ErrorCodingRate = v);
        }

        [Fact]
        public void TestFifoAddressPointer()
        {
            TestRange(() => _rfm9x.FifoAddressPointer, (v) => _rfm9x.FifoAddressPointer = v);
        }

        [Fact]
        public void TestFifoRxBaseAddress()
        {
            TestRange(() => _rfm9x.FifoRxBaseAddress, (v) => _rfm9x.FifoRxBaseAddress = v);
        }

        [Fact]
        public void TestFifoRxBytesNumber()
        {
            Assert.Equal(0, _rfm9x.FifoRxBytesNumber);
        }

        [Fact]
        public void TestFifoRxCurrentAddress()
        {
            Assert.Equal(0, _rfm9x.FifoRxCurrentAddress);
        }

        [Fact]
        public void TestFifoTxBaseAddress()
        {
            TestRange(() => _rfm9x.FifoTxBaseAddress, (v) => _rfm9x.FifoTxBaseAddress = v);
        }

        [Fact]
        public void TestFrequencyHoppingPeriod()
        {
            TestRange(() => _rfm9x.FrequencyHoppingPeriod, (v) => _rfm9x.FrequencyHoppingPeriod = v);
        }

        [Fact]
        public void TestGetFrequencyError()
        {
            Assert.Equal(0, _rfm9x.FrequencyError);
        }

        [Fact]
        public void TestGetHopChannel()
        {
            var hopChannel = _rfm9x.HopChannel;

            Assert.Equal(0, hopChannel.Channel);
        }

        [Fact]
        public void TestGetLoraIrqFlags()
        {
            _rfm9x.ExecuteReset();

            Assert.Equal(
                    LoraIrqFlags.CadDetected |
                    LoraIrqFlags.CadDone |
                    LoraIrqFlags.ValidHeader, _rfm9x.LoraIrqFlags);
        }

        [Fact]
        public void TestGetLoraIrqFlagsMask()
        {
            _rfm9x.ExecuteReset();

            Assert.Equal(
                LoraIrqFlagsMask.ValidHeaderMask | LoraIrqFlagsMask.RxDoneMask,
                _rfm9x.LoraIrqFlagsMask & (LoraIrqFlagsMask.ValidHeaderMask | LoraIrqFlagsMask.RxDoneMask));
        }

        [Fact]
        public void TestImplicitHeaderModeOn()
        {
            TestRangeBool(() => _rfm9x.ImplicitHeaderModeOn, (v) => _rfm9x.ImplicitHeaderModeOn = v);
        }

        [Fact]
        public void TestLongRangeMode()
        {
            TestRangeBool(() => _rfm9x.LongRangeMode, (v) => _rfm9x.LongRangeMode = v);
        }

        [Fact]
        public void TestLoraAgcAutoOn()
        {
            TestRangeBool(() => _rfm9x.LoraAgcAutoOn, (v) => _rfm9x.LoraAgcAutoOn = v);
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
            TestAssignedValue(expected, () => _rfm9x.LoraMode, (v) => _rfm9x.LoraMode = v);
        }

        [Fact]
        public void TestLoraModeCad()
        {
            // Cad auto transitions to standby
            _rfm9x.LoraMode = LoraMode.Cad;
        }

        [Fact]
        public void TestLoraPayloadLength()
        {
            _rfm9x.ImplicitHeaderModeOn = true;

            TestRange<byte>(() => _rfm9x.LoraPayloadLength, (v) => _rfm9x.LoraPayloadLength = v, 1, 0xFF);
        }

        [Fact]
        public void TestLowDataRateOptimize()
        {
            TestRangeBool(() => _rfm9x.LowDataRateOptimize, (v) => _rfm9x.LowDataRateOptimize = v);
        }

        [Fact]
        public void TestModemStatus()
        {
            Assert.Equal(ModemStatus.None, _rfm9x.ModemStatus);
        }

        [Fact]
        public void TestPacketRssi()
        {
            Assert.Equal(0, _rfm9x.PacketRssi);
        }

        [Fact]
        public void TestPacketSnr()
        {
            Assert.Equal(0, _rfm9x.LastPacketSnr);
        }

        [Fact]
        public void TestPayloadMaxLength()
        {
            TestRange(() => _rfm9x.PayloadMaxLength, (v) => _rfm9x.PayloadMaxLength = v);
        }

        [Fact]
        public void TestPpmCorrection()
        {
            TestRange(() => _rfm9x.PpmCorrection, (v) => _rfm9x.PpmCorrection = v);
        }

        [Fact]
        public void TestPreambleLength()
        {
            TestRange(() => _rfm9x.PreambleLength, (v) => _rfm9x.PreambleLength = v);
        }

        [Fact]
        public void TestRssiWideband()
        {
            Assert.Equal(0, _rfm9x.RssiWideband);
        }

        [Fact]
        public void TestRxCodingRate()
        {
            Assert.Equal(0, _rfm9x.RxCodingRate);
        }

        [Fact]
        public void TestRxPayloadCrcOn()
        {
            TestRangeBool(() => _rfm9x.RxPayloadCrcOn, (v) => _rfm9x.RxPayloadCrcOn = v);
        }

        [Fact]
        public void TestSetLoraIrqFlags()
        {
            _rfm9x.ExecuteReset();
            _rfm9x.LoraIrqFlags =
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
            _rfm9x.ExecuteReset();
            _rfm9x.LoraIrqFlagsMask =
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
            TestRange<ushort>(() => _rfm9x.SymbolTimeout, (v) => _rfm9x.SymbolTimeout = v, 0, 0x3FF);
        }

        [Fact]
        public void TestTxContinuousMode()
        {
            TestRangeBool(() => _rfm9x.TxContinuousMode, (v) => _rfm9x.TxContinuousMode = v);
        }

        [Fact]
        public void TestValidHeaderCount()
        {
            Assert.Equal(0, _rfm9x.ValidHeaderCount);
        }

        [Fact]
        public void TestValidPacketCount()
        {
            Assert.Equal(0, _rfm9x.ValidPacketCount);
        }
    }
}