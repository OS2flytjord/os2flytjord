using System;

namespace Niras.Jordflytning.Infrastructure.Common
{
	public class DecimalUtil
	{

		/// <summary>
		/// Makes a decimal to a database numeric
		/// So we dont get an out of range error
		/// </summary>
		public static string ToNumericString(decimal value, int digitsBefore, int digitsAfter)
		{
			var value16 = Math.Truncate(value * (decimal)Math.Pow(10, digitsAfter));
			return value16.ToString(new String('0', digitsBefore + digitsAfter));
		}


	}
}
