using System;
using System.Runtime.Serialization;

namespace CLAP
{
	[Serializable]
	public class InvalidInput : Exception
	{
		public InvalidInput()
		{
		}

		public InvalidInput(string message) : base(message)
		{
		}

		public InvalidInput(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidInput(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}