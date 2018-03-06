using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wex
{
	public enum TransType
	{
		Input = 1,
		Output = 2,
		Income = 4,
		Expense = 5
	}

	public class TransTypeHelper
	{
		public static string ToString(TransType v)
		{
			return Enum.GetName(typeof(TransType), v);
		}
	}
}
