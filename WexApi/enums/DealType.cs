//
// https://github.com/multiprogramm/WexAPI
//

using System;

namespace Wex
{
	public enum DealType
	{
		Ask,
		Bid
	}

	public class DealTypeHelper
	{
		public static DealType FromString(string s)
		{
			switch (s.ToLowerInvariant())
			{
				case "ask":
					return DealType.Ask;
				case "bid":
					return DealType.Bid;
				default:
					throw new ArgumentException();
			}
		}

		public static string ToString(DealType deal_type)
		{
			return Enum.GetName(typeof(DealType), deal_type);
		}
	}
}
