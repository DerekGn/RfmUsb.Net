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

// Ignore Spelling: Rfm

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RfmUsb.Net.Exceptions;
using RfmUsb.Net.Ports;
using Serilog;
using Serilog.Core;
using System.Numerics;
using System.Runtime.CompilerServices;
using Xunit;

namespace RfmUsb.Net.IntTests
{
    public abstract class RfmTestBase : IDisposable
    {
        internal readonly ServiceProvider _serviceProvider;
        internal readonly Logger Logger;

        protected IRfm RfmBase;
        private readonly IConfiguration _configuration;

        protected RfmTestBase()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();

            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddSerilog(Logger);
                })
                .AddSingleton(_configuration)
                .AddSingleton<IRfm9x, Rfm9x>()
                .AddSingleton<IRfm69, Rfm69>()
                .AddSerialPortFactory();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void TestReadStreamDisabled()
        {
            // Arrange
            var bytes = new byte[1];

            RfmBase.BufferedIoEnable = false;

            // Act
            Action action = () => RfmBase.Stream.Read(bytes);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestReadStreamEnabled()
        {
            // Arrange
            var bytes = new byte[10];

            RfmBase.BufferedIoEnable = true;

            // Act
            RfmBase.Stream.Read(bytes);
        }

        [Fact]
        public void TestWriteStreamDisabled()
        {
            // Arrange
            var bytes = new byte[1];
            var rand = new Random();

            rand.NextBytes(bytes);

            RfmBase.BufferedIoEnable = false;

            // Act
            Action action = () => RfmBase.Stream.Write(bytes);

            // Assert
            Assert.Throws<RfmUsbBufferedIoNotEnabledException>(action);
        }

        [Fact(Skip = "ignore")]
        public void TestWriteStreamEnabled()
        {
            // Arrange
            var bytes = new byte[100];
            var rand = new Random();

            rand.NextBytes(bytes);

            RfmBase.BufferedIoEnable = true;

            // Act
            RfmBase.Stream.Write(bytes);
        }

        internal static IEnumerable<byte> RandomSequence()
        {
            Random r = new();
            while (true)
                yield return (byte)r.Next(0, 0xFF);
        }

        internal static void TestAssignedValue<T>(T value, Func<T> read, Action<T> write)
        {
            write(value);
            Assert.Equal(value, read());
        }

        internal static void TestRange<T>(Func<T> read, Action<T> write) where T : IMinMaxValue<T>
        {
            TestRange<T>(read, write, T.MinValue, T.MaxValue);
        }

        internal static void TestRange<T>(Func<T> read, Action<T> write, T expectedMin, T expectedMax) where T : IMinMaxValue<T>
        {
            write(expectedMin);

            var min = read();

            write(expectedMax);

            var max = read();

            Assert.Equal(expectedMin, min);

            Assert.Equal(expectedMax, max);
        }

        internal static void TestRangeBool(Func<bool> read, Action<bool> write)
        {
            var current = read();

            write(!current);

            var changed = read();

            Assert.Equal(!current, changed);
        }

        internal void Read<T>(Func<T> func, [CallerMemberName] string callerName = "")
        {
            Logger.Debug($"Function {callerName} Returned: {func()}");
        }

        #region IDisposable

        private bool disposedValue;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RfmBase.Close();
                    RfmBase.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion IDisposable
    }
}