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
using System.IO;
using System.IO.Ports;
using System.Text;

namespace RfmUsb.Ports
{
    /// <summary>
    /// Defines a serial port interface
    /// </summary>
    public interface ISerialPort : IDisposable
    {
        /// <summary>
        /// Gets or sets the handshaking protocol for serial port transmission of data using a value from System.IO.Ports.Handshake.
        /// </summary>
        
        public Handshake Handshake { get; set; }
        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal during serial communication.
        /// </summary>
        public bool DtrEnable { get; set; }
        /// <summary>
        /// Gets the state of the Clear-to-Send line.
        /// </summary>
        public bool CtsHolding { get; }
        /// <summary>
        /// Gets or sets a value indicating whether null bytes are ignored when transmitted between the port and the receive buffer.
        /// </summary>
        public bool DiscardNull { get; set; }
        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        public int DataBits { get; set; }
        /// <summary>
        /// Gets a value indicating the open or closed status of the <see cref="ISerialPort"/> object.
        /// </summary>
        public bool IsOpen { get; }
        /// <summary>
        /// Gets the state of the Data Set Ready (DSR) signal.
        /// </summary>
        public bool DsrHolding { get; }
        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the <see cref="ISerialPort"/>.ReadLine
        /// and <see cref="ISerialPort"/>.WriteLine(System.String) methods.
        /// </summary>
        public string NewLine { get; set; }
        /// <summary>
        /// Gets or sets the size of the <see cref="ISerialPort"/> input buffer.
        /// </summary>
        public int ReadBufferSize { get; set; }
        /// <summary>
        /// Gets or sets the byte that replaces invalid bytes in a data stream when a parity error occurs.
        /// </summary>
        public byte ParityReplace { get; set; }
        /// <summary>
        /// Gets or sets the port for communications, including but not limited to all available COM ports.
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// Gets the state of the Carrier Detect line for the port.
        /// </summary>
        public bool CDHolding { get; }
        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a read operation does not finish.
        /// </summary>
        public int ReadTimeout { get; set; }
        /// <summary>
        /// Gets or sets the number of bytes in the internal input buffer before a <see cref="ISerialPort"/>.DataReceived event occurs.
        /// </summary>
        public int ReceivedBytesThreshold { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is enabled during serial communication.
        /// </summary>
        public bool RtsEnable { get; set; }
        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// </summary>
        public StopBits StopBits { get; set; }
        /// <summary>
        /// Gets or sets the size of the serial port output buffer.
        /// </summary>
        public int WriteBufferSize { get; set; }
        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a write operation does not finish.
        /// </summary>
        public int WriteTimeout { get; set; }
        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// </summary>
        public Parity Parity { get; set; }
        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        public int BytesToWrite { get; }
        /// <summary>
        /// Gets or sets the serial baud rate.
        /// </summary>
        public int BaudRate { get; set; }
        /// <summary>
        /// Gets or sets the break signal state.
        /// </summary>
        public bool BreakState { get; set; }
        /// <summary>
        /// Gets the underlying System.IO.Stream object for a <see cref="ISerialPort"/> object.
        /// </summary>
        public Stream BaseStream { get; }
        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        public int BytesToRead { get; }
        /// <summary>
        /// Indicates that data has been received through a port represented by the <see cref="ISerialPort"/> object.
        /// </summary>
        public event SerialDataReceivedEventHandler DataReceived;
        /// <summary>
        /// Indicates that a non-data signal event has occurred on the port represented by the <see cref="ISerialPort"/> object.
        /// </summary>
        public event SerialPinChangedEventHandler PinChanged;
        /// <summary>
        /// Indicates that an error has occurred with a port represented by a <see cref="ISerialPort"/> object.
        /// </summary>
        public event SerialErrorReceivedEventHandler ErrorReceived;
        /// <summary>
        /// Closes the port connection, sets the <see cref="ISerialPort"/>.IsOpen property
        /// to false, and disposes of the internal <see cref="Stream"/> object.
        /// </summary>
        public void Close();
        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        public void DiscardInBuffer();
        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        public void DiscardOutBuffer();
        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        public void Open();
        /// <summary>
        /// Reads a number of bytes from the <see cref="ISerialPort"/> input buffer and
        /// writes those bytes into a byte array at the specified offset.
        /// </summary>
        public int Read(byte[] buffer, int offset, int count);
        /// <summary>
        /// Reads a number of characters from the <see cref="ISerialPort"/> input buffer and writes them into an array of characters at a given offset.
        /// </summary>
        public int Read(char[] buffer, int offset, int count);
        /// <summary>
        /// Synchronously reads one byte from the <see cref="ISerialPort"/> input buffer.
        /// </summary>
        public int ReadByte();
        /// <summary>
        /// Synchronously reads one character from the <see cref="ISerialPort"/> input buffer.
        /// </summary>
        public int ReadChar();
        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in both the stream
        /// and the input buffer of the <see cref="ISerialPort"/> object.
        /// </summary>
        public string ReadExisting();
        /// <summary>
        /// Reads up to the <see cref="ISerialPort"/>.NewLine value in the input buffer.
        /// </summary>
        public string ReadLine();
        /// <summary>Reads a string up to the specified value in the input buffer.
        /// </summary>
        public string ReadTo(string value);
        /// <summary>
        /// Writes a specified number of bytes to the serial port using data from a buffer.
        /// </summary>        
        public void Write(byte[] buffer, int offset, int count);
        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        public void Write(string text);
        /// <summary>
        /// Writes a specified number of characters to the serial port using data from a buffer.
        /// </summary>
        public void Write(char[] buffer, int offset, int count);
        /// <summary>
        /// Writes the specified string and the <see cref="NewLine"/> value to the output buffer.
        /// </summary>
        public void WriteLine(string text);
    }
}
