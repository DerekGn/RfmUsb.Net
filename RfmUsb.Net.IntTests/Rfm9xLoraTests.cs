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
    public class Rfm9xLoraTests : RfmTestBase
    {
        private readonly IRfm9x _rfm9x;

        public Rfm9xLoraTests()
        {
            _rfm9x = _serviceProvider.GetService<IRfm9x>();
            RfmBase = _rfm9x;

            _rfm9x.Open((string)TestContext.Properties["Rfm9x"], int.Parse((string)TestContext.Properties["BaudRate"]));

            _rfm9x.ExecuteReset();

            _rfm9x.Mode = Mode.Sleep;

            _rfm9x.LongRangeMode = true;
        }

        [TestMethod]
        public void TestAccessSharedRegisters()
        {
            TestRangeBool(() => _rfm9x.AccessSharedRegisters, (v) => _rfm9x.AccessSharedRegisters = v);
        }

        [TestMethod]
        [DataRow(AutoRestartRxMode.Off)]
        [DataRow(AutoRestartRxMode.OnWaitForPllLock)]
        [DataRow(AutoRestartRxMode.OnWithoutPllRelock)]
        [DataRow(AutoRestartRxMode.Reserved)]
        public void TestAutoRestartRxMode(AutoRestartRxMode expected)
        {
            TestAssignedValue(expected, () => _rfm9x.AutoRestartRxMode, (v) => _rfm9x.AutoRestartRxMode = v);
        }

        [TestMethod]
        [DataRow(ModemBandwidth.Bandwidth10_4KHZ)]
        [DataRow(ModemBandwidth.Bandwidth125KHZ)]
        [DataRow(ModemBandwidth.Bandwidth15_6KHZ)]
        [DataRow(ModemBandwidth.Bandwidth20_8KHZ)]
        [DataRow(ModemBandwidth.Bandwidth250KHZ)]
        [DataRow(ModemBandwidth.Bandwidth31_25KHZ)]
        [DataRow(ModemBandwidth.Bandwidth41_7KHZ)]
        [DataRow(ModemBandwidth.Bandwidth500KHZ)]
        [DataRow(ModemBandwidth.Bandwidth62_5KHZ)]
        [DataRow(ModemBandwidth.Bandwidth7_8KHZ)]
        public void TestAutoRestartRxMode(ModemBandwidth expected)
        {
            TestAssignedValue(expected, () => _rfm9x.ModemBandwidth, (v) => _rfm9x.ModemBandwidth = v);
        }

        [TestMethod]
        [DataRow(SpreadingFactor.SpreadFactor1024)]
        [DataRow(SpreadingFactor.SpreadFactor128)]
        [DataRow(SpreadingFactor.SpreadFactor2048)]
        [DataRow(SpreadingFactor.SpreadFactor256)]
        [DataRow(SpreadingFactor.SpreadFactor4096)]
        [DataRow(SpreadingFactor.SpreadFactor512)]
        [DataRow(SpreadingFactor.SpreadFactor64)]
        public void TestAutoRestartRxMode(SpreadingFactor expected)
        {
            TestAssignedValue(expected, () => _rfm9x.SpreadingFactor, (v) => _rfm9x.SpreadingFactor = v);
        }

        [TestMethod]
        public void TestBeaconOn()
        {
            TestRangeBool(() => _rfm9x.BeaconOn, (v) => _rfm9x.BeaconOn = v);
        }

        [TestMethod]
        [DataRow(ErrorCodingRate.FourEight)]
        [DataRow(ErrorCodingRate.FourFive)]
        [DataRow(ErrorCodingRate.FourSeven)]
        [DataRow(ErrorCodingRate.FourSix)]
        public void TestErrorCodingRate(ErrorCodingRate expected)
        {
            TestAssignedValue(expected, () => _rfm9x.ErrorCodingRate, (v) => _rfm9x.ErrorCodingRate = v);
        }

        [TestMethod]
        public void TestFifoAddressPointer()
        {
            TestRange(() => _rfm9x.FifoAddressPointer, (v) => _rfm9x.FifoAddressPointer = v);
        }

        [TestMethod]
        public void TestFifoRxBaseAddress()
        {
            TestRange(() => _rfm9x.FifoRxBaseAddress, (v) => _rfm9x.FifoRxBaseAddress = v);
        }

        [TestMethod]
        public void TestFifoRxBytesNumber()
        {
            _rfm9x.FifoRxBytesNumber.Should().Be(0);
        }

        [TestMethod]
        public void TestFifoRxCurrentAddress()
        {
            _rfm9x.FifoRxCurrentAddress.Should().Be(0);
        }

        [TestMethod]
        public void TestFifoTxBaseAddress()
        {
            TestRange(() => _rfm9x.FifoTxBaseAddress, (v) => _rfm9x.FifoTxBaseAddress = v);
        }

        [TestMethod]
        public void TestFrequencyHoppingPeriod()
        {
            TestRange(() => _rfm9x.FrequencyHoppingPeriod, (v) => _rfm9x.FrequencyHoppingPeriod = v);
        }

        [TestMethod]
        public void TestGetFrequencyError()
        {
            _rfm9x.FrequencyError.Should().Be(0);
        }

        [TestMethod]
        public void TestGetHopChannel()
        {
            var hopChannel = _rfm9x.HopChannel;

            hopChannel.Should().NotBeNull();
        }

        [TestMethod]
        public void TestGetLoraIrqFlags()
        {
            _rfm9x.ExecuteReset();
            _rfm9x.LoraIrqFlags.Should()
                .Be(
                    LoraIrqFlags.CadDetected |
                    LoraIrqFlags.CadDone |
                    LoraIrqFlags.ValidHeader);
        }

        [TestMethod]
        public void TestGetLoraIrqFlagsMask()
        {
            _rfm9x.ExecuteReset();
            _rfm9x.LoraIrqFlagsMask.Should().HaveFlag(
                LoraIrqFlagsMask.ValidHeaderMask |
                LoraIrqFlagsMask.RxDoneMask);
        }

        [TestMethod]
        public void TestImplicitHeaderModeOn()
        {
            TestRangeBool(() => _rfm9x.ImplicitHeaderModeOn, (v) => _rfm9x.ImplicitHeaderModeOn = v);
        }

        [TestMethod]
        public void TestLongRangeMode()
        {
            TestRangeBool(() => _rfm9x.LongRangeMode, (v) => _rfm9x.LongRangeMode = v);
        }

        [TestMethod]
        public void TestLoraAgcAutoOn()
        {
            TestRangeBool(() => _rfm9x.LoraAgcAutoOn, (v) => _rfm9x.LoraAgcAutoOn = v);
        }

        [TestMethod]
        [DataRow(LoraMode.RxContinuous)]
        [DataRow(LoraMode.RxSingle)]
        [DataRow(LoraMode.Sleep)]
        [DataRow(LoraMode.Standby)]
        [DataRow(LoraMode.SynthRx)]
        [DataRow(LoraMode.SynthTx)]
        [DataRow(LoraMode.Tx)]
        public void TestLoraMode(LoraMode expected)
        {
            TestAssignedValue(expected, () => _rfm9x.LoraMode, (v) => _rfm9x.LoraMode = v);
        }

        [TestMethod]
        public void TestLoraModeCad()
        {
            // Cad auto transitions to standby
            _rfm9x.LoraMode = LoraMode.Cad;
        }

        [TestMethod]
        public void TestLoraPayloadLength()
        {
            _rfm9x.ImplicitHeaderModeOn = true;

            TestRange<byte>(() => _rfm9x.LoraPayloadLength, (v) => _rfm9x.LoraPayloadLength = v, 1, 0xFF);
        }

        [TestMethod]
        public void TestLowDataRateOptimize()
        {
            TestRangeBool(() => _rfm9x.LowDataRateOptimize, (v) => _rfm9x.LowDataRateOptimize = v);
        }

        [TestMethod]
        public void TestModemStatus()
        {
            _rfm9x.ModemStatus.Should().Be(ModemStatus.None);
        }

        [TestMethod]
        public void TestPacketRssi()
        {
            var x = _rfm9x.PacketRssi;
        }

        [TestMethod]
        public void TestPacketSnr()
        {
            var x = _rfm9x.LastPacketSnr;
        }

        [TestMethod]
        public void TestPayloadMaxLength()
        {
            TestRange(() => _rfm9x.PayloadMaxLength, (v) => _rfm9x.PayloadMaxLength = v);
        }

        [TestMethod]
        public void TestPpmCorrection()
        {
            TestRange(() => _rfm9x.PpmCorrection, (v) => _rfm9x.PpmCorrection = v);
        }

        [TestMethod]
        public void TestPreambleLength()
        {
            TestRange(() => _rfm9x.PreambleLength, (v) => _rfm9x.PreambleLength = v);
        }

        [TestMethod]
        public void TestRssiWideband()
        {
            var x = _rfm9x.RssiWideband;
        }

        [TestMethod]
        public void TestRxCodingRate()
        {
            var x = _rfm9x.RxCodingRate;
        }

        [TestMethod]
        public void TestRxPayloadCrcOn()
        {
            TestRangeBool(() => _rfm9x.RxPayloadCrcOn, (v) => _rfm9x.RxPayloadCrcOn = v);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestSymbolTimeout()
        {
            TestRange<ushort>(() => _rfm9x.SymbolTimeout, (v) => _rfm9x.SymbolTimeout = v, 0, 0x3FF);
        }

        [TestMethod]
        public void TestTxContinuousMode()
        {
            TestRangeBool(() => _rfm9x.TxContinuousMode, (v) => _rfm9x.TxContinuousMode = v);
        }

        [TestMethod]
        public void TestValidHeaderCount()
        {
            var x = _rfm9x.ValidHeaderCount;
        }

        [TestMethod]
        public void TestValidPacketCount()
        {
            var x = _rfm9x.ValidPacketCount;
        }
    }
}