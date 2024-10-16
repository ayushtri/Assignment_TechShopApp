using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLibrary
{

	[Serializable]
	public class IncompleteOrderException : Exception
	{
		public IncompleteOrderException() { }
		public IncompleteOrderException(string message) : base(message) { }
		public IncompleteOrderException(string message, Exception inner) : base(message, inner) { }
		protected IncompleteOrderException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
