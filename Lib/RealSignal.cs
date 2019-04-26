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
        public RealSignal(List<double> points)
        {
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

        public List<(double begin, double end, int value)> ToDrawHistogram(int numberOfIntervals)
        {
            List<(double, double, int)> result = new List<(double, double, int)>(numberOfIntervals);
            double max = Points.Max();
            double min = Points.Min();

            double range = max - min;
            double interval = range / numberOfIntervals;

            for (int i = 0; i < numberOfIntervals - 1; i++)
            {
                int points = Points.Count(n => n >= min + interval * i && n < min + interval * (i + 1));
                result.Add((Math.Round(min + interval * i, 2), Math.Round(min + interval * (i + 1), 2), points));
            }
            int lastPoints = Points.Count(n => n >= min + interval * (numberOfIntervals - 1) && n <= min + interval * numberOfIntervals);
            result.Add((Math.Round(min + interval * (numberOfIntervals - 1), 2), Math.Round(min + interval * numberOfIntervals, 2), lastPoints));

            return result;
        }

        public List<double> GetOnlyFullPeriods
        {
            get
            {
                if (Period == null) return Points;

                var howManyPeriods = (int)(Length / Period);
                var length = (int)(howManyPeriods * Period * SamplingFrequency);

                return Points.GetRange(0, length);
            }
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(nameof(RealSignal));
                sw.WriteLine(Begin);
                sw.WriteLine(Period);
                sw.WriteLine(SamplingFrequency);
                foreach (var y in Points)
                {
                    sw.Write($"{y} ");
                }
            }
        }
    }
}