using System;
using Trading.Data;

namespace Trading.PatternAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {

            string key = ConfigurationProvider.AlphaVantageKey;

            var query = new AvRestQuery(key);
            var result = query.GetDailyTimeSeries("MSFT").Result;


            Console.WriteLine("Hello World!");
        }
    }
}
