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
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RfmUsb.Net.Exceptions;
using RfmUsb.Net.Ports;
using System;
using System.Collections.Generic;
using System.IO;

namespace RfmUsb.Net.UnitTests
{
    public abstract class RfmBaseTests
    {
        protected readonly ILogger<IRfm> MockLogger;
        protected readonly Mock<ISerialPort> MockSerialPort;
        protected readonly Mock<ISerialPortFactory> MockSerialPortFactory;
        protected RfmBase RfmBase;

        public RfmBaseTests()
        {
            MockSerialPortFactory = new Mock<ISerialPortFactory>();

            MockSerialPort = new Mock<ISerialPort>();

            MockLogger = Mock.Of<ILogger<IRfm>>();
        }

        [TestMethod]
        public void TestClose()
        {
            // Arrange
            RfmBase.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.IsOpen)
                .Returns(true);

            // Act
            RfmBase.Close();

            // Assert
            MockSerialPort.Verify(_ => _.Close());
        }

        [TestMethod]
        public void TestGetBitRate()
        {
            ExecuteGetTest(
                () => { return RfmBase.BitRate; },
                (v) => { v.Should().Be(0x100); },
                Commands.GetBitRate,
                "0x100");
        }

        [TestMethod]
        public void TestOpen()
        {
            // Arrange
            MockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(MockSerialPort.Object);

            // Act
            RfmBase.Open("ComPort", 9600);

            // Assert
            MockSerialPortFactory
                .Verify(_ => _.CreateSerialPortInstance(It.IsAny<string>()), Times.Once);

            MockSerialPort.Verify(_ => _.Open(), Times.Once);
        }

        [TestMethod]
        public void TestOpenNotFound()
        {
            // Arrange
            MockSerialPortFactory
                .Setup(_ => _.CreateSerialPortInstance(It.IsAny<string>()))
                .Returns(MockSerialPort.Object);

            MockSerialPortFactory
                .Setup(_ => _.GetSerialPorts())
                .Returns(new List<string>() { });

            MockSerialPort
                .Setup(_ => _.Open())
                .Throws<FileNotFoundException>();

            // Act
            Action action = () => RfmBase.Open("ComPort", 9600);

            // Assert
            action.Should().Throw<RfmUsbSerialPortNotFoundException>();
        }

        [TestMethod]
        public void TestReset()
        {
            ExecuteTest(
                () => { RfmBase.Reset(); },
                Commands.ExecuteReset,
                RfmBase.ResponseOk);
        }

        internal void ExecuteGetTest<T>(Func<T> action, Action<T> validation, string command, string value)
        {
            // Arrange
            RfmBase.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(value);

            // Act
            var result = action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command}\n"));

            validation(result);
        }

        internal void ExecuteSetTest(Action action, string command, string value)
        {
            // Arrange
            RfmBase.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(RfmBase.ResponseOk);

            // Act
            action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command} {value}\n"), Times.Once);
        }

        internal void ExecuteTest(Action action, string command, string response)
        {
            // Arrange
            RfmBase.SerialPort = MockSerialPort.Object;

            MockSerialPort
                .Setup(_ => _.ReadLine())
                .Returns(response);

            // Act
            action();

            // Assert
            MockSerialPort.Verify(_ => _.Write($"{command}\n"), Times.Once);
        }
    }
}
