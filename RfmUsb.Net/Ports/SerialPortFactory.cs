﻿/*
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

using RfmUsb.Net.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RfmUsb.Net.Ports
{
    [ExcludeFromCodeCoverage]
    internal class SerialPortFactory : ISerialPortFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="ISerialPort"/>
        /// </summary>
        /// <param name="serialPort">The port to use (for example, COM1).</param>
        /// <returns>An instance of the <see cref="ISerialPort"/></returns>
        public ISerialPort CreateSerialPortInstance(string serialPort)
        {
            if(!GetSerialPorts().Any(x => x == serialPort))
            {
                throw new RfmUsbSerialPortNotFoundException(
                    $"Serial port [{serialPort}] not found. " +
                    $"Available Serial Ports: [{string.Join(", ", GetSerialPorts())}]");
            }

            return new SerialPort(serialPort);
        }

        /// <summary>
        /// Get the list of serial ports from the OD
        /// </summary>
        /// <returns></returns>
        public IList<string> GetSerialPorts()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }
    }
}