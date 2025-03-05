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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RfmUsb.Net.IntTests.Options;

namespace RfmUsb.Net.IntTests
{
    public class TestFixture<T> : BaseFixture, IDisposable where T : IRfm
    {
        private readonly T _device;
        private bool disposedValue;

        public TestFixture()
        {
            var options = ServiceProvider.GetService<IOptions<DeviceConfigurationOptions>>();

            _device = ServiceProvider.GetService<T>() ?? throw new NullReferenceException($"Unable to resolve {nameof(T)}");
            
            _device.Timeout = 1000;

            _device.Open(options!.Value.Rfm69ComPort!, options.Value.BaudRate);

            BaseDevice = _device;
        }

        public IRfm BaseDevice { get; internal set; }

        public T Device => _device;

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
                    _device?.Close();
                    _device?.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}