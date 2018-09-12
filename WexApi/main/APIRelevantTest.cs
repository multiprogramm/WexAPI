//
// https://github.com/multiprogramm/WexAPI
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Wex
{
	public static class APIRelevantTest
	{
		/// <summary>
		/// Check that code is relevant. Error if code does not contain new pairs or currencies,
		///		or contain deleted pairs or currencies
		/// </summary>
		public static void Check()
		{
			var info_json = WexPublicApi.InfoRequestWithoutParse();
			var pairs_json = info_json.Value<JObject>("pairs");

			var resp_pairs = pairs_json.OfType<JProperty>().Select(
				x => x.Name
			).ToList();

			var added_pairs = GetAddedPairs(resp_pairs);
			var added_currencies = GetAddedCurrencies(resp_pairs);
			var deleted_pairs = GetDeletedPairs(resp_pairs);
			var deleted_currencies = GetDeletedCurrencies(resp_pairs);

			if (added_pairs.Count() == 0
				&& added_currencies.Count() == 0
				&& deleted_pairs.Count() == 0
				&& deleted_currencies.Count() == 0
				)
				return; // ok

			StringBuilder err_descr_builder = new StringBuilder();
			BuildPairsErr(err_descr_builder, added_pairs, deleted_pairs);
			BuildCurrenciesErr(err_descr_builder, added_currencies, deleted_currencies);
			throw new Exception(err_descr_builder.ToString());
		}

		//PRIVATE:

		static SortedSet<string> GetAddedPairs(List<string> resp_pairs)
		{
			SortedSet<string> added_pairs = new SortedSet<string>();

			foreach (string s_pair in resp_pairs)
			{
				WexPair wex_pair = WexPairHelper.FromString(s_pair);
				if (wex_pair == WexPair.Unknown)
					added_pairs.Add(s_pair.ToLowerInvariant());
			}

			return added_pairs;
		}

		static SortedSet<string> GetAddedCurrencies(List<string> resp_pairs)
		{
			SortedSet<string> added_currencies = new SortedSet<string>();

			foreach (string s_pair in resp_pairs)
			{
				string[] two_currs_arr = s_pair.Split(WexPairHelper.PAIR_TO_CURR_SPLIT_CHARS);
				if (two_currs_arr.Count() != 2)
					throw new Exception("Strange pair " + s_pair);

				foreach (string s_curr in two_currs_arr)
				{
					WexCurrency wex_curr = WexCurrencyHelper.FromString(s_curr);
					if (wex_curr == WexCurrency.Unknown)
						added_currencies.Add(s_curr.ToLowerInvariant());
				}
			}

			return added_currencies;
		}

		static SortedSet<string> GetDeletedPairs(IEnumerable<string> resp_pairs)
		{
			var our_pairs = WexPairHelper.GetAllPairs();
			foreach (string s_pair in resp_pairs)
			{
				WexPair wex_pair = WexPairHelper.FromString(s_pair);
				if (wex_pair != WexPair.Unknown)
					our_pairs.Remove(wex_pair);
			}

			SortedSet<string> deleted_pairs = new SortedSet<string>();
			foreach (WexPair pair in our_pairs)
				deleted_pairs.Add(WexPairHelper.ToString(pair));
			return deleted_pairs;
		}

		static SortedSet<string> GetDeletedCurrencies(IEnumerable<string> resp_pairs)
		{
			var our_currencies = WexCurrencyHelper.GetAllCurrencies();
			foreach (string s_pair in resp_pairs)
			{
				string[] two_currs_arr = s_pair.Split(WexPairHelper.PAIR_TO_CURR_SPLIT_CHARS);
				if (two_currs_arr.Count() != 2)
					throw new Exception("Strange pair " + s_pair);

				foreach (string s_curr in two_currs_arr)
				{
					WexCurrency wex_curr = WexCurrencyHelper.FromString(s_curr);
					if (wex_curr != WexCurrency.Unknown)
						our_currencies.Remove(wex_curr);
				}
			}

			SortedSet<string> deleted_currencies = new SortedSet<string>();
			foreach (WexCurrency curr in our_currencies)
				deleted_currencies.Add(WexCurrencyHelper.ToString(curr));
			return deleted_currencies;
		}



		static void BuildPairsErr(
			StringBuilder err_descr_builder,
			SortedSet<string> added_pairs,
			SortedSet<string> deleted_pairs)
		{
			PrintSet(err_descr_builder,
				"In enum WexPair need to add items: ", ".\n", added_pairs);
			PrintSet(err_descr_builder,
				"From enum WexPair need to delete items: ", ".\n", deleted_pairs);
		}

		static void BuildCurrenciesErr(
			StringBuilder err_descr_builder,
			SortedSet<string> added_currencies,
			SortedSet<string> deleted_currencies)
		{
			PrintSet(err_descr_builder,
				"In enum WexCurrency need to add items: ", ".\n", added_currencies);
			PrintSet(err_descr_builder,
				"From enum WexCurrency need to delete items: ", ".\n", deleted_currencies);
		}

		static void PrintSet(
			StringBuilder err_descr_builder,
			string header,
			string ending,
			SortedSet<string> set )
		{
			bool is_first = true;
			foreach (string elem in set)
			{
				if (is_first)
				{
					err_descr_builder.Append(header);
					is_first = false;
				}
				else
					err_descr_builder.Append(", ");

				err_descr_builder.Append(elem);
			}

			if (!is_first)
				err_descr_builder.Append(ending);
		}
	}
}
