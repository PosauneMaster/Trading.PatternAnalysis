using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            var keyReversalTickers = new HashSet<string>();
            var popGunTickers = new HashSet<string>();

            var latestList = new List<StrategyResults>();
            var results = latestList.OrderBy(t => t.Symbol).ThenBy(t => t.StrategyName).ToList();


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

                    if (ts.Timestamp > DateTime.Today.AddDays(-6))
                    {
                        latestList.Add(new StrategyResults() {Symbol = ts.Symbol, StrategyDate = ts.Timestamp, StrategyName = "Key Reversal"});
                    }

                    if (!keyReversalTickers.Contains(ts.Symbol))
                    {
                        keyReversalTickers.Add(ts.Symbol);
                    }
                }

                foreach (var ts in popguns)
                {
                    var message = $"Symbol={ts.Symbol}; Date={ts.Timestamp:yyyy-MM-dd}";
                    sbPopguns.AppendLine(message);
                    Debug.WriteLine(message);

                    if (ts.Timestamp > DateTime.Today.AddDays(-6))
                    {
                        latestList.Add(new StrategyResults() { Symbol = ts.Symbol, StrategyDate = ts.Timestamp, StrategyName = "Pop Gun" });
                    }

                    if (!popGunTickers.Contains(ts.Symbol))
                    {
                        popGunTickers.Add(ts.Symbol);
                    }

                }

                sbKeyReversals.AppendLine();
                sbPopguns.AppendLine();
            }

            var sbResults = new StringBuilder();
            foreach (var strategyResults in results)
            {
                sbResults.AppendLine(strategyResults.GetLine());
            }

            var todayDirectory = Path.Combine(ConfigurationProvider.DoubleOutsideKeyReversalPath, $"{DateTime.Now:yyyyMMdd}");
            if (!Directory.Exists(todayDirectory))
            {
                Directory.CreateDirectory(todayDirectory);
            }

            var importList = new List<string>();
            importList.Add("###Double Close Key Reversals");
            foreach (var ticker in keyReversalTickers)
            {
                importList.Add(ticker);
            }

            importList.Add("###Pop Guns");
            foreach (var ticker in popGunTickers)
            {
                importList.Add(ticker);
            }

            var importFileContents = String.Join(',', importList);
            var importFilePath = Path.Combine(todayDirectory, $"StrategyList_{DateTime.Now:yyyyMMdd}.txt");
            File.WriteAllText(importFilePath, importFileContents);

            string sr = Path.Combine(todayDirectory, $"CurrentResults_{DateTime.Now:yyyyMMddHHmmss}.txt");
            string kr = Path.Combine(todayDirectory,$"KeyReversals_{DateTime.Now:yyyyMMddHHmmss}.txt");
            string pg = Path.Combine(todayDirectory,$"Popguns_{DateTime.Now:yyyyMMddHHmmss}.txt");

            File.WriteAllText(kr, sbKeyReversals.ToString());
            File.WriteAllText(pg, sbPopguns.ToString());
            File.WriteAllText(sr, sbResults.ToString());
        }
    }

    public class StrategyResults
    {
        public string Symbol { get; set; }
        public DateTime StrategyDate { get; set; }
        public string StrategyName { get; set; }

        public string GetLine()
        {
            return $"Symbol={Symbol}; Date={StrategyDate:yyyy-MM-dd}; Strategy={StrategyName}";
        }
    }
}
