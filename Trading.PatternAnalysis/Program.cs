using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Trading.Data;

namespace Trading.PatternAnalysis
{


    class Program
    {



        static void Main(string[] args)
        {
            var symbols = new StockSymbolService().GetSp500Symbols();
            new Service().Run(symbols.ToArray());
            //new Service().Run(symbols.Take(25).ToArray());
        }
    }
}
