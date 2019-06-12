using System.Collections.Generic;
using Lib;
using LiveCharts.Defaults;

namespace Visualization
{
    public class ViewUtils
    {
        public static IEnumerable<ObservablePoint> ToValues(RealSignal signal)
        {
            var result = new List<ObservablePoint>();
            foreach (var (x, y) in signal.ToDrawGraph()) result.Add(new ObservablePoint(x, y));

            return result;
        }

        public static IEnumerable<ObservablePoint> ToValues(List<(double x, double y)> list)
        {
            var result = new List<ObservablePoint>();
            foreach (var (x, y) in list) result.Add(new ObservablePoint(x, y));

            return result;
        }
    }
}