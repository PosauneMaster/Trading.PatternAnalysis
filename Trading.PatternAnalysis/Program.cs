using System;
using Trading.Data;

namespace Trading.PatternAnalysis
{


    class Program
    {



        static void Main(string[] args)
        {
            var symbols = new StockSymbolService().GetSp500Symbols();
            new Service().Run(symbols);
        }
    }
}
