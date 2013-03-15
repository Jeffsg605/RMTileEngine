using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMTileEngine
{
	internal class RMTEException : Exception
	{
		private static readonly string ExceptionHeader = "TILE ENGINE EXCEPTION: ";

		internal RMTEException(string message)
			: base(ExceptionHeader + message)
		{

		}

		internal RMTEException(string message, Exception inner)
			: base(ExceptionHeader + message, inner)
		{

		}
	}
}
