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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RfmUsb.Net.Ports;
using Serilog;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RfmUsb.Net.IntTests
{
    public abstract class RfmBaseTests : IDisposable
    {
        internal readonly ServiceProvider _serviceProvider;
        protected IRfm RfmBase;
        private readonly IConfiguration _configuration;
        private bool disposedValue;

        public RfmBaseTests()
        {
            _configuration = SetupConfiguration();
            _serviceProvider = BuildServiceProvider();
        }

        [TestMethod]
        public void TestAddressFiltering()
        {
            
        }

        [TestMethod]
        public void TestAfc()
        {
            Read(() => RfmBase.Afc);
        }

        [TestMethod]
        public void TestAfcAutoClear()
        {
            TestRangeBool(() => RfmBase.AfcAutoClear, (v) => RfmBase.AfcAutoClear = v);
        }


        [TestMethod]
        public void TestAfcAutoOn()
        {
            TestRangeBool(() => RfmBase.AfcAutoOn, (v) => RfmBase.AfcAutoOn = v);
        }

        [TestMethod]
        public void TestBitrate()
        {
            TestRange(() => RfmBase.BitRate, (v) => RfmBase.BitRate = v);
        }

        [TestMethod]
        public void TestFei()
        {
            Read(() => RfmBase.Fei);
        }

        private static void Read<T>(Func<T> func, [CallerMemberName] string callerName = "")
        {
            Trace.WriteLine($"Function {callerName} Returned: {func()}");
        }

        private static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json")
                //.AddEnvironmentVariables()
                .Build();
        }

        private static void TestRange<T>(Func<T> read, Action<T> write) where T : IMinMaxValue<T>
        {
            write(T.MinValue);

            var min = read();

            write(T.MaxValue);

            var max = read();

            min.Should().Be(T.MinValue);

            max.Should().Be(T.MaxValue);
        }

        private static void TestRangeBool(Func<bool> read, Action<bool> write)
        {
            var current = read();

            write(!current);

            var changed = read();

            changed.Should().Be(!current);
        }

        private ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .AddSingleton(_configuration)
                .AddSingleton<IRfm9x, Rfm9x>()
                .AddSingleton<IRfm6x, Rfm6x>()
                .AddSerialPortFactory()
                .AddLogging();

            return serviceCollection.BuildServiceProvider();
        }
        #region IDisposable

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

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RfmBaseTests()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        #endregion IDisposable
    }
}