//
// https://github.com/multiprogramm/WexAPI
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Wex.Utils;

namespace Wex
{
	public class Transaction
	{
		public TransType Type { get; private set; }
		public decimal Amount { get; private set; }
		public WexCurrency Currency { get; private set; }
		public string Description { get; private set; }
		public TransStatus Status { get; private set; }
		public DateTime Timestamp { get; private set; }

		public static Transaction ReadFromJObject(JObject o)
		{
			if (o == null)
				return null;
			return new Transaction()
			{
				Type = (TransType)o.Value<int>("type"),
				Amount = o.Value<decimal>("amount"),
				Currency = WexCurrencyHelper.FromString(o.Value<string>("currency")),
				Description = o.Value<string>("desc"),
				Status = (TransStatus)o.Value<int>("status"),
				Timestamp = UnixTime.ConvertToDateTime( o.Value<UInt32>("timestamp") )
			};
		}
	}

	public class TransHistoryAnswer
	{
		public Dictionary<int, Transaction> List { get; private set; }
		public static TransHistoryAnswer ReadFromJObject(JObject o)
		{
			var sel = o.OfType<JProperty>().Select(
				x => new KeyValuePair<int, Transaction>(
					int.Parse(x.Name),
					Transaction.ReadFromJObject(x.Value as JObject)
				)
			);

			return new TransHistoryAnswer()
			{
				List = sel.ToDictionary(x => x.Key, x => x.Value)
			};
		}
	}
}
