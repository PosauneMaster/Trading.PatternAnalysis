using Newtonsoft.Json.Linq;
using System;

namespace Trading.Data
{
    public enum Interval
    {
        Min1,
        Min5,
        Min15,
        Min30,
        Min60,
        Daily,
        Weekly,
        Monthly       

    }
    public class TimeSeries
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public Interval Interval { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }

        public TimeSeries()
        {
        }

        public TimeSeries(string symbol, JProperty jproperty, Interval interval)
        {
            Initialize(symbol, jproperty, interval);
        }

        public void Initialize(string symbol, JProperty jproperty, Interval interval)
        {
            Symbol = symbol;
            Interval = interval;

            DateTime.TryParse(jproperty.Name, out var dt);
            Timestamp = dt;

            Open = jproperty.Value["1. open"].Value<decimal>();
            High = jproperty.Value["2. high"].Value<decimal>();
            Low = jproperty.Value["3. low"].Value<decimal>();
            Close = jproperty.Value["4. close"].Value<decimal>();
            Volume = jproperty.Value["6. volume"].Value<int>();
        }


    }
}
