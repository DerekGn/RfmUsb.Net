# RfmUsb

[![Build Status](https://dev.azure.com/DerekGn/GitHub/_apis/build/status/DerekGn.RfmUsb?branchName=main)](https://dev.azure.com/DerekGn/GitHub/_build/latest?definitionId=3&branchName=main)

Api for the usb rfm69 usb serial device. The api allows for all configuration parameters of the Rfm69HW to be configured via a command line interface.

## Installing RfmUsb

Install the RfmUsb package via nuget package manager console:

```
Install-Package RfmUsb
```

## Supported .Net Runtimes

The RfmUsb package is compatible with the following runtimes:

* .NET Standard 2.0

## Creating Instance

An instance of an RfmUsb class can be created by providing an instance of an ILogger and a ISerialPortFactory

```csharp
var logger = GetLogger();

var serialPortFactory = GetSerialPortFactory();

var rfmUsbDevice = RfmUsb(logger, serialPortFactory);
```

Alternatively an instance can be configured using the standard microsoft DI framework.

```csharp
var serviceCollection = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .AddLogging()
                .AddRfmUsb(); // Add the RfmUsb
```

## Open

The RfmUsb instance must be opened prior to usage.

```csharp
 rfmUsbDevice.Open(comPort, 115200);
```

## Configuration

All of the configuration settings are assigned via properties of the IRfmUsb interface.

```csharp
_rfmUsb.Open(serialPort, baudrate);

_rfmUsb.Reset();

_rfmUsb.Timeout = 5000;

_rfmUsb.Modulation = Modulation.Fsk;
_rfmUsb.FrequencyDeviation = 0x01EC;
_rfmUsb.Frequency = 434300000;
_rfmUsb.RxBw = 14;
_rfmUsb.BitRate = 4800;
_rfmUsb.Sync = new List<byte>() { 0x2D, 0xD4 };
_rfmUsb.SyncSize = 1;
_rfmUsb.SyncEnable = true;
_rfmUsb.SyncBitErrors = 0;
_rfmUsb.PacketFormat = true;
_rfmUsb.DcFree = DcFree.Manchester;
_rfmUsb.CrcOn = false;
_rfmUsb.CrcAutoClear = false;
_rfmUsb.AddressFiltering = AddressFilter.None;
_rfmUsb.PayloadLength = 66;
```

## Transmit

Transmit a packet of data with the default transmit timeout. The transmit control registers need to be configured to allow transmission of the packet data.

```csharp
// Transmit data with default transmission timeouts
rfmUsbDevice.Transmit(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA });
```

Transmit a packet of data with a specific transmit timeout. The transmit control registers need to be configured to allow transmission of the packet data.

```csharp
// Transmit data with a timeout
rfmUsbDevice.Transmit(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA }, 1000);
```

## Transmit And Receive

Transmit a packet and wait for a response with the default transmit and receive timeouts.

```csharp
// Transmit data and wait for a response with the default 
var response = rfmUsbDevice.TransmitReceive(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA });
```

Transmit a packet and wait for a response with the default receive timeouts and a specified transmit timeout.

```csharp
// Transmit data and wait for
var response = rfmUsbDevice.TransmitReceive(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA }, 1000);
```

Transmit a packet and wait for a response with a specified transmit and receive timeouts.

```csharp
// Transmit data and wait for
var response = rfmUsbDevice.TransmitReceive(new List<byte>() { 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA }, 1000, 1000 );
```

## Radio Configurations

The RfmUsb device has a number of predefined radio configurations that can be assigned to the Rfm device.

```csharp
// Get the list of radio configurations
var radioConfigs = rfmUsbDevice.GetRadioConfigurations();
```

```csharp
// Set the radio configuration
rfmUsbDevice.RadioConfig = 20;
```

## Irqs

Set the Dio pin Irq mask.

```csharp
// Setup the radio irq pin enables
rfmUsbDevice.DioInterruptMask = DioIrq.Dio0;
```

Wait for an IRQ to occur

```csharp
// Wait for an Irq
rfmUsbDevice.WaitForIrq();

// Check the irq
if ((rfmUsbDevice.Irq & Irq.PayloadReady) == Irq.PayloadReady)
{
    // Process packet
}
```
