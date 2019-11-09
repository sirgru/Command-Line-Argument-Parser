using System;
using System.Runtime.Serialization;

namespace CLAP
{
	[Serializable]
	internal class InputException : Exception
	{
		private object p;

		public InputException()
		{
		}

		public InputException(object p)
		{
			this.p = p;
		}

		public InputException(string message) : base(message)
		{
		}

		public InputException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InputException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}