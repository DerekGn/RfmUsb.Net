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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RfmUsb.Net.Ports;
using Serilog;
using Serilog.Core;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RfmUsb.Net.IntTests
{
    public abstract class RfmBaseTests : IDisposable
    {
        internal readonly ServiceProvider _serviceProvider;
        private readonly Logger _logger;
        protected IRfm RfmBase;
        private readonly IConfiguration _configuration;
        private bool disposedValue;

        public RfmBaseTests()
        {
            _configuration = SetupConfiguration();
            _serviceProvider = BuildServiceProvider();

            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }

        //[ClassInitialize]
        //public void TestFixtureSetup(TestContext context)
        //{
        //    RfmBase.Open("COM3", 230400);
        //}

        [TestMethod]
        [DataRow(AddressFilter.None)]
        [DataRow(AddressFilter.NodeBroaddcast)]
        [DataRow(AddressFilter.Reserved)]
        public void TestAddressFiltering(AddressFilter expected)
        {
            TestAssignedValue(expected, () => RfmBase.AddressFiltering, (v) => RfmBase.AddressFiltering = v);
        }

        [TestMethod]
        public void TestAfc()
        {
            Read(() => RfmBase.Afc);
        }

        [TestMethod]
        public void TestAfcAutoClear()
        {
            TestRangeBool(() => RfmBase.AfcAutoClear, (v) => RfmBase.AfcAutoClear = v);
        }

        [TestMethod]
        public void TestAfcAutoOn()
        {
            TestRangeBool(() => RfmBase.AfcAutoOn, (v) => RfmBase.AfcAutoOn = v);
        }

        [TestMethod]
        public void TestAutoRxRestartOn()
        {
            TestRangeBool(() => RfmBase.AutoRxRestartOn, (v) => RfmBase.AutoRxRestartOn = v);
        }
        
        [TestMethod]
        public void TestBitrate()
        {
            TestRange<uint>(() => RfmBase.BitRate, (v) => RfmBase.BitRate = v, 488, 300000);
        }

        [TestMethod]
        public void TestBroadcastAddress()
        {
            TestRange(() => RfmBase.BroadcastAddress, (v) => RfmBase.BroadcastAddress = v);
        }

        [TestMethod]
        public void TestCrcAutoClear()
        {
            TestRangeBool(() => RfmBase.CrcAutoClear, (v) => RfmBase.CrcAutoClear = v);
        }

        [TestMethod]
        public void TestCrcOn()
        {
            TestRangeBool(() => RfmBase.CrcOn, (v) => RfmBase.CrcOn = v);
        }

        [TestMethod]
        [DataRow(DcFree.Manchester)]
        [DataRow(DcFree.None)]
        [DataRow(DcFree.Manchester)]
        [DataRow(DcFree.Whitening)]
        public void TestDcFree(DcFree expected)
        {
            TestAssignedValue(expected, () => RfmBase.DcFree, (v) => RfmBase.DcFree = v);
        }

        [TestMethod]
        public void TestFei()
        {
            Read(() => RfmBase.Fei);
        }

        [TestMethod]
        public void TestFifo()
        {
            var expected = new List<byte> { 0xAA, 0x55, 0xAA };
            RfmBase.Fifo = expected;

            RfmBase.Fifo.Should().StartWith(expected);
        }

        [TestMethod]
        public void TestFifoThreshold()
        {
            TestRange<byte>(() => RfmBase.FifoThreshold, (v) => RfmBase.FifoThreshold = v, 0, 127);
        }

        [TestMethod]
        public void TestFrequency()
        {
#warning Check freq max
            TestRange<uint>(() => RfmBase.Frequency, (v) => RfmBase.Frequency = v, 0, 1020000000);
        }

        [TestMethod]
        public void TestFrequencyDeviation()
        {
            TestRange(() => RfmBase.FrequencyDeviation, (v) => RfmBase.FrequencyDeviation = v, ushort.MinValue, (ushort)16383);
        }

        [TestMethod]
        [DataRow(FskModulationShaping.GaussianBt0_3)]
        [DataRow(FskModulationShaping.GaussianBt0_5)]
        [DataRow(FskModulationShaping.GaussianBt1_0)]
        [DataRow(FskModulationShaping.None)]
        public void TestFskModulationShaping(FskModulationShaping expected)
        {
            TestAssignedValue(expected, () => RfmBase.FskModulationShaping, (v) => RfmBase.FskModulationShaping = v);
        }

        [TestMethod]
        public void TestInterPacketRxDelay()
        {
            TestRange<byte>(() => RfmBase.InterPacketRxDelay, (v) => RfmBase.InterPacketRxDelay = v, 0, 15);
        }

        [TestMethod]
        [DataRow(LnaGain.Auto)]
        [DataRow(LnaGain.Max)]
        [DataRow(LnaGain.MaxMinus6db)]
        [DataRow(LnaGain.MaxMinus12db)]
        [DataRow(LnaGain.MaxMinus24db)]
        [DataRow(LnaGain.MaxMinus36db)]
        [DataRow(LnaGain.MaxMinus48db)]
        public void TestLnaGainSelect(LnaGain expected)
        {
            TestAssignedValue(expected, () => RfmBase.LnaGainSelect, (v) => RfmBase.LnaGainSelect = v);
        }

        [TestMethod]
        [DataRow(Mode.Rx)]
        [DataRow(Mode.Sleep)]
        [DataRow(Mode.Standby)]
        [DataRow(Mode.Synth)]
        [DataRow(Mode.Tx)]
        [Ignore]
        public void TestMode(Mode expected)
        {
            TestAssignedValue(expected, () => RfmBase.Mode, (v) => RfmBase.Mode = v);
        }

        [TestMethod]
        [DataRow(Modulation.Fsk)]
        [DataRow(Modulation.Ook)]
        public void TestModulation(Modulation expected)
        {
            TestAssignedValue(expected, () => RfmBase.Modulation, (v) => RfmBase.Modulation = v);
        }

        [TestMethod]
        public void TestNodeAddress()
        {
            TestRange(() => RfmBase.NodeAddress, (v) => RfmBase.NodeAddress = v);
        }

        [TestMethod]
        public void TestOcpEnable()
        {
            TestRangeBool(() => RfmBase.OcpEnable, (v) => RfmBase.OcpEnable = v);
        }

        [TestMethod]
        [DataRow(OcpTrim.OcpTrim45)]
        [DataRow(OcpTrim.OcpTrim50)]
        [DataRow(OcpTrim.OcpTrim55)]
        [DataRow(OcpTrim.OcpTrim60)]
        [DataRow(OcpTrim.OcpTrim65)]
        [DataRow(OcpTrim.OcpTrim70)]
        [DataRow(OcpTrim.OcpTrim75)]
        [DataRow(OcpTrim.OcpTrim80)]
        [DataRow(OcpTrim.OcpTrim85)]
        [DataRow(OcpTrim.OcpTrim90)]
        [DataRow(OcpTrim.OcpTrim95)]
        [DataRow(OcpTrim.OcpTrim100)]
        [DataRow(OcpTrim.OcpTrim105)]
        [DataRow(OcpTrim.OcpTrim110)]
        [DataRow(OcpTrim.OcpTrim115)]
        [DataRow(OcpTrim.OcpTrim120)]
        

        public void TestOcpTrim(OcpTrim expected)
        {
            TestAssignedValue(expected, () => RfmBase.OcpTrim, (v) => RfmBase.OcpTrim = v);
        }

        [TestMethod]
        [DataRow(OokAverageThresholdFilter.ChipRate2)]
        [DataRow(OokAverageThresholdFilter.ChipRate4)]
        [DataRow(OokAverageThresholdFilter.ChipRate8)]
        [DataRow(OokAverageThresholdFilter.ChipRate32)]
        public void TestOokAverageThresholdFilter(OokAverageThresholdFilter expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokAverageThresholdFilter, (v) => RfmBase.OokAverageThresholdFilter = v);
        }

        [TestMethod]
        public void TestOokFixedThreshold()
        {
            TestRange(() => RfmBase.OokFixedThreshold, (v) => RfmBase.OokFixedThreshold = v);
        }

        [TestMethod]
        [DataRow(OokModulationShaping.Filtering2Br)]
        [DataRow(OokModulationShaping.FilteringBr)]
        [DataRow(OokModulationShaping.None)]
        [DataRow(OokModulationShaping.Reserved)]
        public void TestOokModulationShaping(OokModulationShaping expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokModulationShaping, (v) => RfmBase.OokModulationShaping = v);
        }

        [TestMethod]
        [DataRow(OokThresholdDec.EightTimesInEachChip)]
        [DataRow(OokThresholdDec.FourTimesInEachChip)]
        [DataRow(OokThresholdDec.OnceEvery2Chips)]
        [DataRow(OokThresholdDec.OnceEvery4Chips)]
        [DataRow(OokThresholdDec.OnceEvery8Chips)]
        [DataRow(OokThresholdDec.OncePerChip)]
        [DataRow(OokThresholdDec.SixteeenTimesInEachChip)]
        [DataRow(OokThresholdDec.TwiceInEachChip)]
        public void TestOokPeakThresholdDec(OokThresholdDec expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokPeakThresholdDec, (v) => RfmBase.OokPeakThresholdDec = v);
        }

        [TestMethod]
        [DataRow(OokThresholdStep.Step0_5db)]
        [DataRow(OokThresholdStep.Step1_5db)]
        [DataRow(OokThresholdStep.Step1db)]
        [DataRow(OokThresholdStep.Step3db)]
        [DataRow(OokThresholdStep.Step4db)]
        [DataRow(OokThresholdStep.Step5db)]
        [DataRow(OokThresholdStep.Step6db)]
        public void TestOokPeakThresholdStep(OokThresholdStep expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokPeakThresholdStep, (v) => RfmBase.OokPeakThresholdStep = v);
        }

        [TestMethod]
        [DataRow(OokThresholdType.Average)]
        [DataRow(OokThresholdType.Fixed)]
        [DataRow(OokThresholdType.Peak)]
        [DataRow(OokThresholdType.Reserved)]
        public void TestOokThresholdType(OokThresholdType expected)
        {
            TestAssignedValue(expected, () => RfmBase.OokThresholdType, (v) => RfmBase.OokThresholdType = v);
        }

        [TestMethod]
        public void TestPacketFormat()
        {
            TestRangeBool(() => RfmBase.PacketFormat, (v) => RfmBase.PacketFormat = v);
        }

        [TestMethod]
        [DataRow(PaRamp.PowerAmpRamp10)]
        [DataRow(PaRamp.PowerAmpRamp12)]
        [DataRow(PaRamp.PowerAmpRamp15)]
        [DataRow(PaRamp.PowerAmpRamp20)]
        [DataRow(PaRamp.PowerAmpRamp25)]
        [DataRow(PaRamp.PowerAmpRamp50)]
        [DataRow(PaRamp.PowerAmpRamp62)]
        [DataRow(PaRamp.PowerAmpRamp31)]
        [DataRow(PaRamp.PowerAmpRamp40)]
        [DataRow(PaRamp.PowerAmpRamp100)]
        [DataRow(PaRamp.PowerAmpRamp250)]
        [DataRow(PaRamp.PowerAmpRamp500)]
        [DataRow(PaRamp.PowerAmpRamp1000)]





        public void TestPaRamp(PaRamp expected)
        {
            TestAssignedValue(expected, () => RfmBase.PaRamp, (v) => RfmBase.PaRamp = v);
        }

        
        [TestMethod]
        public void TestPreambleSize()
        {
            TestRange(() => RfmBase.PreambleSize, (v) => RfmBase.PreambleSize = v);
        }

        [TestMethod]
        public void TestRssi()
        {
            Read<byte>(() => RfmBase.Rssi);
        }

        [TestMethod]
        public void TestRxBw()
        {
            TestRange<byte>(() => RfmBase.RxBw, (v) => RfmBase.RxBw = v, 0, 23);
        }

        [TestMethod]
        public void RxBwAfc()
        {
            TestRange<byte>(() => RfmBase.RxBwAfc, (v) => RfmBase.RxBwAfc = v, 0, 23);
        }

        [TestMethod]
        public void TestSync()
        {
            RfmBase.SyncEnable = true;

            var expected = new List<byte>() { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
            RfmBase.Sync = expected;

            RfmBase.Sync.Should().BeSameAs(expected);
        }

        [TestMethod]
        public void TestSyncEnable()
        {
            TestRangeBool(() => RfmBase.SyncEnable, (v) => RfmBase.SyncEnable = v);
        }

        [TestMethod]
        [Ignore("update firmware value set/get")]
        public void TestSyncSize()
        {
            TestRange<byte>(() => RfmBase.SyncSize, (v) => RfmBase.SyncSize = v, 0, 7);
        }

        [TestMethod]
        public void TestTemperatureValue()
        {
            Read(() => RfmBase.TemperatureValue);
        }

        [TestMethod]
        public void TestTxStartCondition()
        {
            TestRangeBool(() => RfmBase.TxStartCondition, (v) => RfmBase.TxStartCondition = v);
        }

        [TestMethod]
        public void TestVersion()
        {
            Read(() => RfmBase.Version);
        }

        [TestMethod]
        [DataRow(Dio.Dio0,DioMapping.DioMapping0)]
        [DataRow(Dio.Dio0, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio0, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio0, DioMapping.DioMapping3)]
        [DataRow(Dio.Dio1, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio1, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio1, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio1, DioMapping.DioMapping3)]
        [DataRow(Dio.Dio2, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio2, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio2, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio2, DioMapping.DioMapping3)]
        [DataRow(Dio.Dio3, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio3, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio3, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio3, DioMapping.DioMapping3)]
        [DataRow(Dio.Dio4, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio4, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio4, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio4, DioMapping.DioMapping3)]
        [DataRow(Dio.Dio5, DioMapping.DioMapping0)]
        [DataRow(Dio.Dio5, DioMapping.DioMapping1)]
        [DataRow(Dio.Dio5, DioMapping.DioMapping2)]
        [DataRow(Dio.Dio5, DioMapping.DioMapping3)]
        public void TestDioMapping(Dio dio, DioMapping expected)
        {
            RfmBase.SetDioMapping(dio, expected);
            RfmBase.GetDioMapping(dio).Should().Be(expected);
        }

        [TestMethod]
        public void TestRcCalibration()
        {
            RfmBase.RcCalibration();
        }

        [TestMethod]
        public void TestTransmit()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestTransmitWithTimeout()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestTransmitReceive()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestTransmitReceiveWithTimeout()
        {
            throw new NotImplementedException();
        }

        internal static void TestAssignedValue<T>(T value, Func<T> read, Action<T> write)
        {
            write(value);
            read().Should().Be(value);
        }

        internal void Read<T>(Func<T> func, [CallerMemberName] string callerName = "")
        {
            _logger.Debug($"Function {callerName} Returned: {func()}");
        }

        private static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddEnvironmentVariables()
                .Build();
        }

        internal static void TestRange<T>(Func<T> read, Action<T> write,T expectedMin, T expectedMax) where T : IMinMaxValue<T>
        {
            write(expectedMin);

            var min = read();

            write((T)expectedMax);

            var max = read();

            min.Should().Be(expectedMin);

            max.Should().Be(expectedMax);
        }

        internal static void TestRange<T>(Func<T> read, Action<T> write) where T : IMinMaxValue<T>
        {
            TestRange<T>(read, write, T.MinValue, T.MaxValue);
        }

        internal static void TestRangeBool(Func<bool> read, Action<bool> write)
        {
            var current = read();

            write(!current);

            var changed = read();

            changed.Should().Be(!current);
        }

        private ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .AddSingleton(_configuration)
                .AddSingleton<IRfm9x, Rfm9x>()
                .AddSingleton<IRfm6x, Rfm6x>()
                .AddSerialPortFactory()
                .AddLogging();

            return serviceCollection.BuildServiceProvider();
        }
        #region IDisposable

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RfmBase.Close();
                    RfmBase.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RfmBaseTests()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        #endregion IDisposable
    }
}