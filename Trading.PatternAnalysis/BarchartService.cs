using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trading.Data;

namespace Trading.PatternAnalysis
{

    public interface IBarchartService
    {
        TimeSeries[] FindOutsideDoubleKeyReversal(IEnumerable<TimeSeries> series);
    }
    public class BarchartService : IBarchartService
    {
        public TimeSeries[] FindOutsideDoubleKeyReversal(IEnumerable<TimeSeries> series)
        {
            var resultsList = new List<TimeSeries>();
            var sorted = series.OrderByDescending(s => s.Timestamp);
            var linked = new LinkedList<TimeSeries>(sorted);

            var currentSeriesNode = linked.First;
            while (currentSeriesNode?.Next?.Next != null)
            {
                if (IsOutsideBar(currentSeriesNode.Value, currentSeriesNode.Next.Value))
                {
                    if (IsBearReversal(currentSeriesNode.Value, currentSeriesNode.Next.Value, currentSeriesNode.Next.Next.Value))
                    {
                        resultsList.Add(currentSeriesNode.Value);
                    }

                    if (IsBullReversal(currentSeriesNode.Value, currentSeriesNode.Next.Value, currentSeriesNode.Next.Next.Value))
                    {
                        resultsList.Add(currentSeriesNode.Value);
                    }
                }

                currentSeriesNode = currentSeriesNode.Next;
            }
            return resultsList.ToArray();
        }

        private bool IsOutsideBar(TimeSeries timeSeries1, TimeSeries timeSeries2)
        {
            return timeSeries1.High > timeSeries2.High && timeSeries1.Low < timeSeries2.Low;
        }

        private bool IsBullReversal(TimeSeries timeSeries1, TimeSeries timeSeries2, TimeSeries timeSeries3)
        {
            var closeCondition = timeSeries1.Close > timeSeries2.Close && timeSeries1.Close > timeSeries3.Close;
            var rangeCondition = timeSeries1.Low < timeSeries2.Low && timeSeries1.Low < timeSeries3.Low;
            return closeCondition && rangeCondition;

        }

        private bool IsBearReversal(TimeSeries timeSeries1, TimeSeries timeSeries2, TimeSeries timeSeries3)
        {
            var closeCondition = timeSeries1.Close < timeSeries2.Close && timeSeries1.Close < timeSeries3.Close;
            var rangeCondition = timeSeries1.High > timeSeries2.High && timeSeries1.High  > timeSeries3.High;
            return closeCondition && rangeCondition;
        }
    }
}
