using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lib
{
    public class RealSignal : Signal<double>
    {
        public RealSignal(double beginsAt, double? period, double samplingFrequency, List<double> points)
        {
            Begin = beginsAt;
            Period = period;
            SamplingFrequency = samplingFrequency;
            Points = points;
        }

        public double this[int index]
        {
            get => Points[index];

            set => Points[index] = value;
        }

        public List<(double x, double y)> ToDrawGraph()
        {
            var x = Begin;
            var span = 1.0 / SamplingFrequency;
            var result = new List<(double x, double y)>();

            foreach (var y in Points)
            {
                result.Add((x, y));
                x += span;
            }

            return result;
        }
    }
}