using System;
using System.Runtime.Serialization;

namespace RfmUsb.Net.Exceptions
{
    /// <summary>
    /// Exception thrown when the serial port receive timeout
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Create an instance of <see cref="RfmUsbTimeoutException"/>
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/></param>
        /// <param name="context">The <see cref="StreamingContext"/></param>
        protected RfmUsbTimeoutException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}