using FluentAssertions;
using Xunit;

namespace RfmUsb.Tests
{
    public class DeviceTests : IClassFixture<DeviceTestsFixture>
    {
        private readonly DeviceTestsFixture _fixture;

        public DeviceTests(DeviceTestsFixture fixture)
        {
            _fixture = fixture;
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

        //[Theory]
        //[InlineData(ushort.MaxValue)]
        //[InlineData(ushort.MinValue)]
        //public void TestAfc(ushort value)
        //{
        //    _fixture.RfmUsbDevice.Afc = value;

        //    _fixture.RfmUsbDevice.AesOn.Should().Be(value);
        //}

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

    }
}
