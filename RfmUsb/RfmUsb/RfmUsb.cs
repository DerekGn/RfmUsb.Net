/*
* MIT License
*
* Copyright (c) 2021 Derek Goslin
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

using Microsoft.Extensions.Logging;
using RfmUsb.Ports;
using RfmUsb.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RfmUsb
{
    public class RfmUsb : IRfmUsb
    {
        //private const string ResponseOk = "OK";

        //private readonly ISerialPortFactory _serialPortFactory;
        //private readonly ILogger<IRfmUsb> _logger;

        //private ISerialPort _serialPort;

        //public RfmUsb(ILogger<IRfmUsb> logger, ISerialPortFactory serialPortFactory)
        //{
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //    _serialPortFactory = serialPortFactory ?? throw new ArgumentNullException(nameof(serialPortFactory));
        //}
        /////<inheritdoc/>
        //public string Version => SendCommand("g-fv");
        /////<inheritdoc/>
        //public byte FifoThreshold
        //{
        //    get => SendCommand("g-ft").ToBytes().First();
        //    set => SendCommandWithCheck($"s-ft 0x{value:X}", ResponseOk);
        //}
        /////<inheritdoc/>
        //public byte DioInterruptMask
        //{
        //    get => SendCommand("g-di").ToBytes().First();
        //    set => SendCommandWithCheck($"s-di 0x{value:X}", ResponseOk);
        //}
        /////<inheritdoc/>
        //public int Timeout
        //{
        //    get => _serialPort.ReadTimeout;
        //    set
        //    {
        //        _serialPort.ReadTimeout = value;
        //        _serialPort.WriteTimeout = value;
        //    }
        //}
        /////<inheritdoc/>
        //public bool TransmissionStartCondition
        //{
        //    get => SendCommand("g-tsc").ToBytes().First() == 1;
        //    set => SendCommandWithCheck($"s-tsc 0x0{(value ? 0x01 : 0x00):X}", ResponseOk);
        //}
        /////<inheritdoc/>
        //public byte RadioConfig
        //{
        //    get => SendCommand($"g-rc").ToBytes().First();
        //    set => SendCommandWithCheck($"s-rc {value}", ResponseOk);
        //}
        /////<inheritdoc/>
        //public IEnumerable<byte> Sync
        //{
        //    get => SendCommand($"g-sync").ToBytes();
        //    set
        //    {
        //        SendCommandWithCheck($"s-sync {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        //        SendCommandWithCheck($"s-ss {value.Count() - 1}", ResponseOk);
        //    }
        //}
        /////<inheritdoc/>
        //public int OutputPower
        //{
        //    get => int.Parse(SendCommand($"g-op"));
        //    set => SendCommandWithCheck($"s-op {value}", ResponseOk);
        //}
        /////<inheritdoc/>
        //public Modulation Modulation
        //{
        //    get => (Modulation)Enum.Parse(typeof(Modulation), SendCommand("g-mt"));
        //    set => SendCommand($"s-mt 0x{(int)value:X}");
        //}
        /////<inheritdoc/>
        //public int FreqencyDeviation
        //{
        //    get => int.Parse(SendCommand($"g-fd"));
        //    set => SendCommand($"s-fd {value}");
        //}
        /////<inheritdoc/>
        //public int Frequency
        //{
        //    get => int.Parse(SendCommand($"g-f"));
        //    set => SendCommand($"s-f {value}");
        //}
        /////<inheritdoc/>
        //public int ReceiveBandwidth
        //{
        //    get => int.Parse(SendCommand("g-rxbw"));
        //    set => SendCommand($"s-rxbw 0x{value:X}");
        //}
        /////<inheritdoc/>
        //public int RadioBaudRate
        //{
        //    get => int.Parse(SendCommand($"g-br"));
        //    set => SendCommand($"s-br 0x{value:X}");
        //}
        /////<inheritdoc/>
        //public bool SyncEnable
        //{
        //    get => bool.Parse(SendCommand("g-se"));
        //    set => SendCommand($"s-se {(value ? 1 : 0)}");
        //}
        /////<inheritdoc/>
        //public int SyncBitErrors
        //{
        //    get => int.Parse(SendCommand("g-sbe"));
        //    set => SendCommand($"s-sbe 0x{value:X}");
        //}
        /////<inheritdoc/>
        //public bool VariableLengthPacket
        //{
        //    get => bool.Parse(SendCommand("g-pf"));
        //    set => SendCommand($"s-pf {(value ? 1 : 0)}");
        //}
        /////<inheritdoc/>
        //public bool DcFreeEncoding
        //{
        //    get => bool.Parse(SendCommand("g-dfe"));
        //    set => SendCommand($"s-dfe {(value ? 1 : 0)}");
        //}
        /////<inheritdoc/>
        //public bool CrcCheck
        //{
        //    get => bool.Parse(SendCommand("g-cc"));
        //    set => SendCommand($"s-cc {(value ? 1 : 0)}");
        //}
        /////<inheritdoc/>
        //public bool CrcAutoClear
        //{
        //    get => bool.Parse(SendCommand("g-caco"));
        //    set => SendCommand($"s-caco {(value ? 1 : 0)}");
        //}
        /////<inheritdoc/>
        //public bool AddressFilter
        //{
        //    get => bool.Parse(SendCommand("g-af"));
        //    set => SendCommand($"s-af {(value ? 1 : 0)}");
        //}
        /////<inheritdoc/>
        //public int PacketLength
        //{
        //    get => SendCommand("g-pl").ToBytes().First();
        //    set => SendCommandWithCheck($"s-pl 0x{value:X}", ResponseOk);
        //}
        /////<inheritdoc/>
        //public Mode Mode
        //{
        //    get => (Mode)Enum.Parse(typeof(Mode), SendCommand("g-om"), true);
        //    set => SendCommand($"s-om 0x{(int)value:X}");
        //}
        /////<inheritdoc/>
        //public IEnumerable<byte> Fifo
        //{
        //    get => SendCommand($"g-fifo").ToBytes();
        //    set
        //    {
        //        SendCommandWithCheck($"s-fifo {BitConverter.ToString(value.ToArray()).Replace("-", string.Empty)}", ResponseOk);
        //    }
        //}
        /////<inheritdoc/>
        //public Irq Irq => GetIrqInternal();

        //public bool Sequencer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public bool ListenerOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /////<inheritdoc/>
        //public void Open(string serialPort, int baudRate)
        //{
        //    try
        //    {
        //        if (_serialPort == null)
        //        {
        //            _serialPort = _serialPortFactory.CreateSerialPortInstance(serialPort);

        //            _serialPort.BaudRate = baudRate;
        //            _serialPort.NewLine = "\r\n";
        //            _serialPort.DtrEnable = true;
        //            _serialPort.RtsEnable = true;
        //            _serialPort.ReadTimeout = 500;
        //            _serialPort.WriteTimeout = 500;
        //            _serialPort.Open();
        //        }
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        _logger.LogDebug(ex, "Exception occurred opening serial port");

        //        throw new RfmUsbSerialPortNotFoundException(
        //            $"Unable to open serial port [{serialPort}] Reason: [{ex.Message}]. " +
        //            $"Available Serial Ports: [{string.Join(", ", _serialPortFactory.GetSerialPorts())}]");
        //    }
        //}
        /////<inheritdoc/>
        //public void Close()
        //{
        //    if (_serialPort != null && _serialPort.IsOpen)
        //    {
        //        _serialPort.Close();
        //    }
        //}
        /////<inheritdoc/>
        //public void Reset()
        //{
        //    SendCommandWithCheck($"e-r", ResponseOk);
        //}
        /////<inheritdoc/>
        //public IList<byte> TransmitReceive(IList<byte> data, int timeout)
        //{
        //    var response = SendCommand($"e-txrx {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)} {timeout}");

        //    if (response.Contains("TX") || response.Contains("RX"))
        //    {
        //        throw new RfmUsbTransmitException($"Packet transmission failed: [{response}]");
        //    }

        //    return response.ToBytes();
        //}
        /////<inheritdoc/>
        //public void Transmit(IList<byte> data)
        //{
        //    var response = SendCommand($"e-tx {BitConverter.ToString(data.ToArray()).Replace("-", string.Empty)}");

        //    if (response.Contains("TX") || response.Contains("RX"))
        //    {
        //        throw new RfmUsbTransmitException($"Packet transmission failed: [{response}]");
        //    }
        //}
        /////<inheritdoc/>
        //public void SetDioMapping(Dio dio, DioMapping mapping)
        //{
        //    SendCommandWithCheck($"s-dio {(int)dio} {(int)mapping}", $"[0x{(int)mapping:X4}]-Map {(int)mapping:D2}");
        //}
        /////<inheritdoc/>
        //public void WaitForIrq()
        //{
        //    var irq = _serialPort.ReadLine();

        //    if (!irq.StartsWith("DIO PIN IRQ"))
        //    {
        //        throw new RfmUsbCommandExecutionException($"Invalid response received for IRQ signal: [{irq}]");
        //    }
        //}

        //private Irq GetIrqInternal()
        //{
        //    Irq irq = Irq.None;

        //    var result = SendCommand("g-irq");

        //    while (!result.Contains("RX_RDY"))
        //    {
        //        var parts = result.Split(':');

        //        switch (parts[1])
        //        {
        //            case "CRC_OK":
        //                if(parts[0] == "1")
        //                {
        //                    irq |= Irq.CrcOK;
        //                }
        //                break;
        //            case "PAYLOAD_READY":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.PayloadReady;
        //                }
        //                break;
        //            case "FIFO_OVERRUN":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.FifoOverrun;
        //                }
        //                break;
        //            case "FIFO_LEVEL":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.FifoLevel;
        //                }
        //                break;
        //            case "FIFO_NOT_EMPTY":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.FifoNotEmpty;
        //                }
        //                break;
        //            case "FIFO_FULL":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.FifoFull;
        //                }
        //                break;
        //            case "ADDRESS_MATCH":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.AddressMatch;
        //                }
        //                break;
        //            case "AUTO_MODE":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.AutoMode;
        //                }
        //                break;
        //            case "TIMEOUT":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.Timeout;
        //                }
        //                break;
        //            case "RSSI":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.Rssi;
        //                }
        //                break;
        //            case "PLL_LOCK":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.PllLock;
        //                }
        //                break;
        //            case "TX_RDY":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.TxReady;
        //                }
        //                break;
        //            case "RX_RDY":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.RxReady;
        //                }
        //                break;
        //            case "MODE_RDY":
        //                if (parts[0] == "1")
        //                {
        //                    irq |= Irq.ModeReady;
        //                }
        //                break;
        //        }

        //        result = _serialPort.ReadLine();
        //    };

        //    return irq;
        //}

        //private string SendCommand(string command)
        //{
        //    _serialPort.Write($"{command}\n");

        //    var response = _serialPort.ReadLine();

        //    _logger.LogDebug($"Command: [{command}] Result: [{response}]");

        //    return response;
        //}
        //private void SendCommandWithCheck(string command, string response)
        //{
        //    var result = SendCommand(command);

        //    if (!result.StartsWith(response))
        //    {
        //        throw new RfmUsbCommandExecutionException($"Command: [{command}] Execution Failed Reason: [{result}]");
        //    }
        //}

        #region IDisposible
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_serialPort != null)
                    {
                        _serialPort.Close();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RfmUsb()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        public void LeistenAbort()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
