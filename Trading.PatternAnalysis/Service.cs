using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Trading.Data;

namespace Trading.PatternAnalysis
{
    public class Service
    {
        private readonly IAvRestQuery _query;
        private readonly ITradingDataApi _api;
        private readonly IBarchartService _barchartService;

        public Service()
        {
            _barchartService = new BarchartService();
            _query = new AvRestQuery(ConfigurationProvider.AlphaVantageKey);
            _api = new TradingDataApi(_query);

        }

        public void Run(string[] symbols)
        {
            var sbKeyReversals = new StringBuilder();

            foreach (var symbol in symbols)
            {
                Thread.Sleep(2000);

                var result = _api.GetDailyTimeSeries(symbol);

                if (result == null)
                {
                    continue;
                }

                var keyReversals = _barchartService.FindOutsideDoubleKeyReversal(result);

                foreach (var ts in keyReversals)
                {
                    var message = $"Symbol={ts.Symbol}; Date={ts.Timestamp:yyyy-MM-dd}";
                    sbKeyReversals.AppendLine(message);
                    Debug.WriteLine(message);
                }

                sbKeyReversals.AppendLine();

            }

            string filename = Path.Combine(ConfigurationProvider.DoubleOutsideKeyReversalPath,
                $"KeyReversals_{DateTime.Now:yyyyMMddHHmmss}.txt");

            File.WriteAllText(filename, sbKeyReversals.ToString());

        }
    }
}
