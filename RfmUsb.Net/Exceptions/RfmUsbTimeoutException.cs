using System;

namespace RfmUsb.Net.Exceptions
{
    /// <summary>
    /// Exception thrown when the serial port receive timeout
    /// </summary>
    public class RfmUsbTimeoutException : Exception
    {
        /// <summary>
        /// Create an instance of <see cref="RfmUsbTimeoutException"/>
        /// </summary>
        public RfmUsbTimeoutException()
        { }

        /// <summary>
        /// Create an instance of <see cref="RfmUsbTimeoutException"/>
        /// </summary>
        /// <param name="message">The exception message</param>
        public RfmUsbTimeoutException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="RfmUsbTimeoutException"/>
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="inner">The inner <see cref="Exception"/></param>
        public RfmUsbTimeoutException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}