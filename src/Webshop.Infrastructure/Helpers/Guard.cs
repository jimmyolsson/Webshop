using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Infrastructure.Helpers
{
	public class Guard
	{
		public static void ArgumentNotNull(object argumentValue)
		{
			if (argumentValue == null)
			{
				throw new ArgumentNullException(nameof(argumentValue));
			}
		}

		public static void ArgumentNotNullOrWhiteSpace(string argumentValue)
		{
			if (string.IsNullOrWhiteSpace(argumentValue))
			{
				throw new ArgumentException($"{nameof(argumentValue)} cannot be null, empty, or only whitespace.");
			}
		}
	}
}
