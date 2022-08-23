using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace RfmUsb.Net.UnitTests.Logging
{
    public class XUnitLogger<T> : ILogger<T>, IDisposable
    {
        private readonly ITestOutputHelper _output;
        private bool disposedValue;

        public XUnitLogger(ITestOutputHelper output)
        { 
            _output = output;
        }

        public IDisposable BeginScope<TState>(TState state)
        { 
            return this;
        }

        public bool IsEnabled(LogLevel logLevel)
        { 
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _output.WriteLine(state.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}