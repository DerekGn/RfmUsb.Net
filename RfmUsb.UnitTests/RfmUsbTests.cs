using Microsoft.Extensions.Logging;
using Moq;
using RfmUsb.Ports;
using Xunit;
using Xunit.Abstractions;

namespace RfmUsb.UnitTests
{
    public class RfmUsbTests
    {
        private readonly Mock<ISerialPortFactory> _mockSerialPortFactory;
        private readonly Mock<ISerialPort> _mockSerialPort;
        private readonly IRfmUsb _rfmUsb;

        public RfmUsbTests(ITestOutputHelper output)
        {
            _mockSerialPortFactory = new Mock<ISerialPortFactory>();
            _mockSerialPort = new Mock<ISerialPort>();

            _rfmUsb = new RfmUsb(Mock.Of<ILogger<IRfmUsb>>(), _mockSerialPortFactory.Object);
        }

        [Fact]
        public void TestOpen()
        {
            // Arrange
            _mockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(_mockSerialPort.Object);

            // Act
            _rfmUsb.Open("ComPort", 9600);

            // Assert
            _mockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            _mockSerialPort.Verify(_ => _.Open(), Times.Once);
        }

        [Fact]
        public void TestAddressFilter()
        {
            // Arrange

            // Act
            _rfmUsb.AddressFiltering = AddressFilter.Node;

            // Assert
        }
    }
}
