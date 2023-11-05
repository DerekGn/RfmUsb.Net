using System;

namespace RfmUsb.Net.Exceptions
{

	[Serializable]
	public class RfmUsbTimeoutException : Exception
	{
		public RfmUsbTimeoutException() { }
		public RfmUsbTimeoutException(string message) : base(message) { }
		public RfmUsbTimeoutException(string message, Exception inner) : base(message, inner) { }
		protected RfmUsbTimeoutException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
