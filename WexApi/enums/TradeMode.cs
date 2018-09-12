//
// https://github.com/multiprogramm/WexAPI
//

using System;

namespace Wex
{
	public enum TradeMode
	{
		Limit,
		Market
	}

	public class TradeModeHelper
	{
		public static TradeMode FromString(string s)
		{
			switch (s.ToLowerInvariant())
			{
				case "limit":
					return TradeMode.Limit;

				case "market":
					return TradeMode.Market;

				default:
					throw new ArgumentException();
			}
		}

		public static string ToString(TradeMode v)
		{
			return Enum.GetName(typeof(TradeMode), v).ToLowerInvariant();
		}
	}
}
