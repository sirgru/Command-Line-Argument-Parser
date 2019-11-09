using System;
using System.Runtime.Serialization;

namespace CLAP
{
	[Serializable]
	internal class Termination : Exception
	{
		public Termination()
		{
		}

		public Termination(string message) : base(message)
		{
		}

		public Termination(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected Termination(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}