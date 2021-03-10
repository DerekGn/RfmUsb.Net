using Microsoft.Extensions.DependencyInjection;
using RfmUsb.Ports;
using System;

namespace RfmUsb.Tests
{
    public class DeviceTestsFixture : IDisposable
    {
        private readonly string ComPort = "COM4";

        private readonly ServiceProvider _serviceProvider;
        public readonly IRfmUsb RfmUsbDevice;

        public DeviceTestsFixture()
        {
            _serviceProvider = BuildServiceProvider();

            RfmUsbDevice = _serviceProvider.GetService<IRfmUsb>();

            RfmUsbDevice.Open(ComPort, 115200);

            RfmUsbDevice.Timeout = 1000;
        }

        public void Dispose()
        {
            if (RfmUsbDevice != null)
            {
                RfmUsbDevice.Close();

                RfmUsbDevice.Dispose();
            }
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                .AddSerialPortFactory()
                .AddLogging()
                .AddRfmUsb();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
