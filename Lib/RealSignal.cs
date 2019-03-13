using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lib
{
    public class RealSignal
    {
        public RealSignal(double beginsAt, double? period, double samplingFrequency, List<double> points)
        {
            BeginsAt = beginsAt;
            Period = period;
            SamplingFrequency = samplingFrequency;
            Points = points;
        }

        public double BeginsAt { get; }

        public double SamplingFrequency { get; }

        public List<double> Points { get; }

        public double? Period { get; }

        public double SamplingPeriod => 1.0 / SamplingFrequency;

        public double Length => EndsAt - BeginsAt;

        public double EndsAt => BeginsAt + (SamplingPeriod * Points.Count);

        public double AverageValue => GetOnlyFullPeriods.Sum() / GetOnlyFullPeriods.Count;

        public double AbsoluteAverageValue => GetOnlyFullPeriods.Sum(x => Math.Abs(x)) / GetOnlyFullPeriods.Count;

        public double RootMeanSquare => Math.Sqrt(AveragePower);

        public double Variance => GetOnlyFullPeriods.Sum(x => Math.Pow(x - AverageValue, 2)) / GetOnlyFullPeriods.Count;

        public double AveragePower => GetOnlyFullPeriods.Sum(x => x * x) / GetOnlyFullPeriods.Count;

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

        public double this[int index]
        {
            get => Points[index];

            set => Points[index] = value;
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(nameof(RealSignal));
                sw.WriteLine(BeginsAt);
                sw.WriteLine(Period);
                sw.WriteLine(SamplingFrequency);
                foreach (var y in Points)
                {
                    sw.Write($"{y} ");
                }
            }
        }

        public List<(double x, double y)> ToDrawGraph()
        {
            var x = BeginsAt;
            var span = 1.0 / SamplingFrequency;
            var result = new List<(double x, double y)>();

            foreach (var y in Points)
            {
                result.Add((x, y));
                x += span;
            }

            return result;
        }

        public List<(double interval, int number)> ToDrawHistogram(int numberOfIntervals)
        {
            var result = new List<(double interval, int number)>();

            var max = GetOnlyFullPeriods.Max();
            var min = GetOnlyFullPeriods.Min();

            var difference = Math.Abs(max - min);
            var span = difference / numberOfIntervals;

            var from = min;

            for (var i = 0; i < numberOfIntervals - 1; i++)
            {
                var to = from + span;
                var number = GetOnlyFullPeriods.Count(x => x >= from && x < to);
                result.Add((from + (span / 2.0), number));
                from = to;
            }

            result.Add((from + (span / 2.0), GetOnlyFullPeriods.Count(x => x >= from)));

            return result;
        }

        public List<(int n, double y)> GetPointsNear(double t, int numberOfPointsNearT)
        {
            var result = new List<(int n, double y)>();
            var xyPoints = Points.Select((x, i) => (t: i * SamplingPeriod, y: x, n: i)).ToList();

            var leftPoints = xyPoints.Where(x => x.t < t).OrderByDescending(x => x.t).Take(numberOfPointsNearT / 2).ToList();
            var rightPoints = xyPoints.Where(x => x.t >= t).Take(numberOfPointsNearT / 2).ToList();

            result.AddRange(leftPoints.OrderBy(x => x.n).Select(x => (x.n, x.y)));
            result.AddRange(rightPoints.Select(x => (x.n, x.y)));

            return result;
        }
    }
}
