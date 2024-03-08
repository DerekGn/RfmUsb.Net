# RfmUsb

[![Build Status](https://dev.azure.com/DerekGn/GitHub/_apis/build/status/DerekGn.RfmUsb.Net?branchName=main)](https://dev.azure.com/DerekGn/GitHub/_build/latest?definitionId=3&branchName=main)

[![NuGet Badge](https://buildstats.info/nuget/RfmUsb.Net)](https://www.nuget.org/packages/RfmUsb.Net/)

Api for the USB Rfm69 and Rfm9x serial device. The api allows for all configuration parameters of the Rfm69 and Rfm9x radio module to be configured via a command line interface.

## Installing RfmUsb

Install the RfmUsb package via nuget package manager console:

```
Install-Package RfmUsb.Net
```

## Supported .Net Runtimes

The RfmUsb.Net package is compatible with the following runtimes:

* .Net Core 7.0

## Creating Instance

An instance of an RfmUsb class can be created by providing an instance of an ILogger and a ISerialPortFactory

```csharp
var logger = GetLogger();

var serialPortFactory = GetSerialPortFactory();

var rfmUsbDevice = Rfm69(logger, serialPortFactory);
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

## Interrupts

Set the Dio pin Irq mask.

```csharp
AutoResetEvent IrqSignal;

void RfmDeviceDioInterrupt(object? sender, DioIrq e)
{
    Console.WriteLine("Dio Irq [{e}]", e);

    if ((e & DioIrq.Dio0) == DioIrq.Dio0)
    {
        IrqSignal.Set();
    }
}

// Setup the radio irq pin enables
rfmUsbDevice.DioInterruptMask = DioIrq.Dio0;

rfmUsbDevice.DioInterrupt += RfmDeviceDioInterrupt;
```

Wait for an IRQ to occur

```csharp
IrqSignal.WaitOne(timeout);

// Check the irq
if ((rfmUsbDevice.Irq & Irq.PayloadReady) == Irq.PayloadReady)
{
    // Process packet
}
```

## Buffered Read And Writes

The maximum message size supported by direct read and write of the FIFO is limited to 64 bytes. To support message transmission and reception up to a maximum size of X bytes the RfmUsb IO buffer must be used.

### Receive IO Buffer

The steps required to receive a message via the IO buffer.

* Configure the RfmUsb radio
* Set the Dio mapping for **Dio0** to **DioMapping1** to capture the **PayloadReady** Irq
* Set the Dio mapping for **Dio1** to **DioMapping2** to capture the **FifoNotEmpty** Irq
* Setup the **PacketFormat** to either fixed or variable length
* Set **PayloadLength** to 0xFF
* Enable the IO buffer via the **BufferedIoEnable** setting
* Set the radio **Mode** to **RX**
* Wait for the **PayloadReady** Irq
* Read the message bytes to the IO buffer via the **Stream**

```csharp
// Configure the radio
InitialiseRadioOpenThings(SerialPort, BaudRate);

// Attach event handlers
AttachEventHandlers(console);

// Enable DIO0 to capture payload ready IRQ 
rfmUsbDevice.SetDioMapping(Dio.Dio0, DioMapping.DioMapping1);
// Enable DIO1 to capture fifo level IRQ for buffered read
rfmUsbDevice.SetDioMapping(Dio.Dio1, DioMapping.DioMapping2);
// Set the Irq mask
rfmUsbDevice.DioInterruptMask = DioIrq.Dio0 | DioIrq.Dio1;
// Enable the Buffered Io
rfmUsbDevice.BufferedIoEnable = true;
// Set the the payload length to 0xFF
rfmUsbDevice.PayloadLength = 0xFF;
// Set the mode to rx
rfmUsbDevice.Mode = Mode.Rx;

// Wait for Irq signal
SignalSource signalSource = WaitForSignal();

if (signalSource == SignalSource.Irq)
{
    if ((rfmUsbDevice.IrqFlags & Rfm69IrqFlags.PayloadReady) == Rfm69IrqFlags.PayloadReady)
    {
        rfmUsbDevice.Mode = Mode.Standby;

        byte[] payload = new byte[rfmUsbDevice.Stream.Length];

        // Read the stream
        rfmUsbDevice.Stream.Read(payload, 0, payload.Length);

        // Process the payload
        // .............
```

### Transmit IO Buffer

The steps required to transmit a message via the IO buffer.

* Configure the RfmUsb radio
* Setup the packet format either fixed or variable length
* Enable the IO buffer via the **BufferedIoEnable** setting
* Set the Dio mapping for Dio0 to 
* Set the radio mode to TX
* Write the message bytes to the IO buffer via the **Stream**

```csharp
IrqSignal.WaitOne(timeout);

// Check the irq
if ((rfmUsbDevice.Irq & Irq.PayloadReady) == Irq.PayloadReady)
{
    // Process packet
}
```