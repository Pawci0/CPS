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

        public List<double> GetOnlyFullPeriods
        {
            get
            {
                if (Period == null) return Points;

                var howManyPeriods = (int) (Length / Period);
                var length = (int) (howManyPeriods * Period * SamplingFrequency);

                return Points.GetRange(0, length);
            }
        }

        public List<(double x, double y)> ToDrawGraph()
        {
            var x = Begin;
            double span = 1;
            if (SamplingFrequency != 0) span = 1.0 / SamplingFrequency;
            var result = new List<(double x, double y)>();

            foreach (var y in Points)
            {
                result.Add((x, y));
                x += span;
            }

            return result;
        }

        internal List<(int i, double y)> GetPointsNear(double time, int nOfPoints)
        {
            var points = ToDrawGraph();
            var left = points.Select((point, index) => (i: index, x: point.x, y: point.y))
                .Where(point => point.x <= time)
                .Reverse()
                .Take(nOfPoints)
                .Reverse()
                .ToList();

            var right = points.Select((point, index) => (i: index, x: point.x, y: point.y))
                .Where(point => point.x > time)
                .Take(nOfPoints)
                .ToList();

            return left.Concat(right)
                .Select(point => (point.i, point.y))
                .ToList();
        }

        public List<(double begin, double end, int value)> ToDrawHistogram(int numberOfIntervals)
        {
            var result = new List<(double, double, int)>(numberOfIntervals);
            var max = Points.Max();
            var min = Points.Min();

            var range = max - min;
            var interval = range / numberOfIntervals;

            for (var i = 0; i < numberOfIntervals - 1; i++)
            {
                var points = Points.Count(n => n >= min + interval * i && n < min + interval * (i + 1));
                result.Add((Math.Round(min + interval * i, 2), Math.Round(min + interval * (i + 1), 2), points));
            }

            var lastPoints = Points.Count(n =>
                n >= min + interval * (numberOfIntervals - 1) && n <= min + interval * numberOfIntervals);
            result.Add((Math.Round(min + interval * (numberOfIntervals - 1), 2),
                Math.Round(min + interval * numberOfIntervals, 2), lastPoints));

            return result;
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(nameof(RealSignal));
                sw.WriteLine(Begin);
                sw.WriteLine(Period);
                sw.WriteLine(SamplingFrequency);
                foreach (var y in Points) sw.Write($"{y} ");
            }
        }
    }
}