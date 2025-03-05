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

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace RfmUsb.Net.Ports
{
    [ExcludeFromCodeCoverage]
    internal class SerialPort : ISerialPort
    {
        private readonly System.IO.Ports.SerialPort _serialPort;
        private bool disposedValue;

        public SerialPort(string serialPort)
        {
            if (string.IsNullOrWhiteSpace(serialPort))
            {
                throw new ArgumentOutOfRangeException(nameof(serialPort));
            }

            _serialPort = new System.IO.Ports.SerialPort
            {
                PortName = serialPort
            };

            _serialPort.ErrorReceived += ErrorReceivedHandler;
            _serialPort.DataReceived += DataReceivedHandler;
            _serialPort.PinChanged += PinChangedHandler;
        }

        public event SerialDataReceivedEventHandler DataReceived;

        public event SerialErrorReceivedEventHandler ErrorReceived;

        public event SerialPinChangedEventHandler PinChanged;

        public Stream BaseStream => _serialPort.BaseStream;
        public int BaudRate { get => _serialPort.BaudRate; set => _serialPort.BaudRate = value; }
        public bool BreakState { get => _serialPort.BreakState; set => _serialPort.BreakState = value; }
        public int BytesToRead => _serialPort.BytesToRead;
        public int BytesToWrite => _serialPort.BytesToWrite;
        public bool CDHolding => _serialPort.CDHolding;
        public bool CtsHolding => _serialPort.CtsHolding;
        public int DataBits { get => _serialPort.DataBits; set => _serialPort.DataBits = value; }
        public bool DiscardNull { get => _serialPort.DiscardNull; set => _serialPort.DiscardNull = value; }
        public bool DsrHolding => _serialPort.DsrHolding;
        public bool DtrEnable { get => _serialPort.DtrEnable; set => _serialPort.DtrEnable = value; }
        public Encoding Encoding { get => _serialPort.Encoding; set => _serialPort.Encoding = value; }
        public Handshake Handshake { get => _serialPort.Handshake; set => _serialPort.Handshake = value; }
        public bool IsOpen => _serialPort.IsOpen;
        public string NewLine { get => _serialPort.NewLine; set => _serialPort.NewLine = value; }
        public Parity Parity { get => _serialPort.Parity; set => _serialPort.Parity = value; }
        public byte ParityReplace { get => _serialPort.ParityReplace; set => _serialPort.ParityReplace = value; }
        public string PortName { get => _serialPort.PortName; set => _serialPort.PortName = value; }
        public int ReadBufferSize { get => _serialPort.ReadBufferSize; set => _serialPort.ReadBufferSize = value; }
        public int ReadTimeout { get => _serialPort.ReadTimeout; set => _serialPort.ReadTimeout = value; }
        public int ReceivedBytesThreshold { get => _serialPort.ReceivedBytesThreshold; set => _serialPort.ReceivedBytesThreshold = value; }
        public bool RtsEnable { get => _serialPort.RtsEnable; set => _serialPort.RtsEnable = value; }
        public StopBits StopBits { get => _serialPort.StopBits; set => _serialPort.StopBits = value; }
        public int WriteBufferSize { get => _serialPort.WriteBufferSize; set => _serialPort.WriteBufferSize = value; }
        public int WriteTimeout { get => _serialPort.WriteTimeout; set => _serialPort.WriteTimeout = value; }

        public void Close()
        {
            _serialPort.Close();
        }

        public void DiscardInBuffer()
        {
            _serialPort.DiscardInBuffer();
        }

        public void DiscardOutBuffer()
        {
            _serialPort.DiscardOutBuffer();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Open()
        {
            _serialPort.Open();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _serialPort.Read(buffer, offset, count);
        }

        public int Read(char[] buffer, int offset, int count)
        {
            return _serialPort.Read(buffer, offset, count);
        }

        public int ReadByte()
        {
            return _serialPort.ReadByte();
        }

        public int ReadChar()
        {
            return _serialPort.ReadChar();
        }

        public string ReadExisting()
        {
            return _serialPort.ReadExisting();
        }

        public string ReadLine()
        {
            return _serialPort.ReadLine();
        }

        public string ReadTo(string value)
        {
            return _serialPort.ReadTo(value);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _serialPort.Write(buffer, offset, count);
        }

        public void Write(string text)
        {
            _serialPort.Write(text);
        }

        public void Write(char[] buffer, int offset, int count)
        {
            _serialPort.Write(buffer, offset, count);
        }

        public void WriteLine(string text)
        {
            _serialPort.WriteLine(text);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _serialPort != null)
                {
                    _serialPort.ErrorReceived -= ErrorReceivedHandler;
                    _serialPort.DataReceived -= DataReceivedHandler;
                    _serialPort.PinChanged -= PinChangedHandler;
                    _serialPort.Dispose();
                }

                disposedValue = true;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialDataReceivedEventHandler handler = DataReceived;
            handler?.Invoke(this, e);
        }

        private void ErrorReceivedHandler(object sender, SerialErrorReceivedEventArgs e)
        {
            SerialErrorReceivedEventHandler handler = ErrorReceived;
            handler?.Invoke(this, e);
        }

        private void PinChangedHandler(object sender, SerialPinChangedEventArgs e)
        {
            SerialPinChangedEventHandler handler = PinChanged;
            handler?.Invoke(this, e);
        }
    }
}