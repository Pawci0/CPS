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
    }
}