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

        public void Initialize(string symbol, JProperty jproperty, Interval interval)
        {
            Interval = interval;

            DateTime.TryParse(jproperty.Name, out var dt);
            Timestamp = dt;

            Open = jproperty["1. open"].Value<decimal>();
            High = jproperty["2. high"].Value<decimal>();
            Low = jproperty["3. low"].Value<decimal>();
            Close = jproperty["4. close"].Value<decimal>();
            Close = jproperty["5. volume"].Value<int>();
        }


    }
}
