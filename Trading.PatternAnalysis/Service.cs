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
        private readonly ITradingDataApi _api;
        private readonly IBarchartService _barchartService;

        public Service()
        {
            _barchartService = new BarchartService();
            IAvRestQuery query = new AvRestQuery(ConfigurationProvider.AlphaVantageKey);
            _api = new TradingDataApi(query);
        }

        public void Run(string[] symbols)
        {
            var sbKeyReversals = new StringBuilder();
            var sbPopguns = new StringBuilder();

            foreach (var symbol in symbols)
            {
                Console.WriteLine($"Evaluating Symbol {symbol}");

                Thread.Sleep(2000);

                var result = _api.GetDailyTimeSeries(symbol);

                if (result == null)
                {
                    continue;
                }

                var keyReversals = _barchartService.FindOutsideDoubleKeyReversal(result);
                Console.WriteLine($"Found {keyReversals.Length} Key Reversals for Symbol {symbol}");

                var popguns = _barchartService.FindPopgun(result);
                Console.WriteLine($"Found {popguns.Length} Popguns for Symbol {symbol}");

                foreach (var ts in keyReversals)
                {
                    var message = $"Symbol={ts.Symbol}; Date={ts.Timestamp:yyyy-MM-dd}";
                    sbKeyReversals.AppendLine(message);
                    Debug.WriteLine(message);
                }

                foreach (var ts in popguns)
                {
                    var message = $"Symbol={ts.Symbol}; Date={ts.Timestamp:yyyy-MM-dd}";
                    sbPopguns.AppendLine(message);
                    Debug.WriteLine(message);
                }

                sbKeyReversals.AppendLine();
                sbPopguns.AppendLine();
            }

            string kr = Path.Combine(ConfigurationProvider.DoubleOutsideKeyReversalPath,
                $"KeyReversals_{DateTime.Now:yyyyMMddHHmmss}.txt");

            string pg = Path.Combine(ConfigurationProvider.DoubleOutsideKeyReversalPath,
                $"Popguns_{DateTime.Now:yyyyMMddHHmmss}.txt");

            File.WriteAllText(kr, sbKeyReversals.ToString());
            File.WriteAllText(pg, sbPopguns.ToString());

        }
    }
}
