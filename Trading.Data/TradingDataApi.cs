using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Trading.Data
{
    public interface ITradingDataApi
    {
        TimeSeries[] GetDailyTimeSeries(string symbol);
    }
    public class TradingDataApi : ITradingDataApi
    {
        public IAvRestQuery _avRestQuery;
        public TradingDataApi()
        {
        }

        public TradingDataApi(IAvRestQuery avRestQuery)
        {
            _avRestQuery = avRestQuery;
        }

        public TimeSeries[] GetDailyTimeSeries(string symbol)
        {
            var list = new List<TimeSeries>();

            var result = _avRestQuery.GetDailyTimeSeries(symbol).Result;
            var jObject = JObject.Parse(result);
            var dataToken = jObject["Time Series (Daily)"];

            if (dataToken == null)
            {
                return null;
            }

            foreach (var token in dataToken)
            {
                if (token is JProperty prop)
                {
                    list.Add(new TimeSeries(symbol, prop, Interval.Daily));
                }
            }

            return list.ToArray();
        }
    }
}
