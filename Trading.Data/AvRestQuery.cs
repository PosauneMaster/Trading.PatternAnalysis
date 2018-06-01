using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Trading.Data
{
    public class AvRestQuery
    {
        private readonly HttpClient _client;
        private readonly string _apikey;

        public AvRestQuery(string apikey)
        {
            _apikey = apikey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://www.alphavantage.co/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetDailyTimeSeries(string symbol, bool isCompact = true )
        {
            var outputsize = "compact";
            if (!isCompact)
            {
                outputsize = "full";
            }
            var method = 
                $"query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&outputsize={outputsize}&apikey={_apikey}";
            var response = await _client.GetStringAsync(method);

            return response;
        }

    }
}
