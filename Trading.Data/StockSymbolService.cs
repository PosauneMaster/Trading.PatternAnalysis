using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace Trading.Data
{
    public class StockSymbolService
    {
        private const string jsonUrl = "https://datahub.io/core/s-and-p-500-companies/r/constituents.json";

        public string[] GetSp500Symbols()
        {
            string result;
            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(jsonUrl).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }

            var stockData = JsonConvert.DeserializeObject<StockData[]>(result);

            if (stockData != null)
            {
                var symbols = from stock in stockData select stock.Symbol;
                return symbols.ToArray();
            }

            return null;
        }

    }
}
