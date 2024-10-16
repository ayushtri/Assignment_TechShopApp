using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLibrary
{

	[Serializable]
	public class InsufficientStockException : Exception
	{
		public InsufficientStockException() { }
		public InsufficientStockException(string message) : base(message) { }
		public InsufficientStockException(string message, Exception inner) : base(message, inner) { }
		protected InsufficientStockException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
