using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLibrary
{

	[Serializable]
	public class OrderNotFoundException : Exception
	{
		public OrderNotFoundException() { }
		public OrderNotFoundException(string message) : base(message) { }
		public OrderNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected OrderNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
