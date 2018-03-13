using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wex
{
	public enum TransStatus
	{
		Rejected = 0,
		WaitAccept = 1,
		Successful = 2,
		Confirmed = 3
	}

	public class TransStatusHelper
	{
		public static string ToString(TransStatus v)
		{
			return Enum.GetName(typeof(TransStatus), v);
		}
	}
}
