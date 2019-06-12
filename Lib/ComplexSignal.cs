using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using MathNet.Numerics;

namespace Lib
{
    public class ComplexSignal : Signal<Complex>
    {
        public Complex BeginsAtComplex { get; set; }
        public ComplexSignal(Complex beginsAt, double? period, double samplingFrequency, List<Complex> points)
        {
            BeginsAtComplex = beginsAt;
            Period = period;
            SamplingFrequency = samplingFrequency;
            Points = points;
        }

        public ComplexSignal(List<Complex> points)
        {
            Points = points;
        }

        public Complex this[int index]
        {
            get => Points[index];

            set => Points[index] = value;
        }

        public List<Complex> GetOnlyFullPeriods
        {
            get
            {
                if (Period == null) return Points;

                var howManyPeriods = (int)(Length / Period);
                var length = (int)(howManyPeriods * Period * SamplingFrequency);

                return Points.GetRange(0, length);
            }
        }

        public List<(double x, double y)> ToDrawRealisGraph()
        {
            var x = BeginsAtComplex;
            double span = 1;
            if (SamplingFrequency != 0) span = 1.0 / SamplingFrequency;
            var result = new List<(double x, double y)>();

            foreach (var y in Points)
            {
                result.Add((x.Real, y.Real));
                x += span;
            }

            return result;
        }

        public List<(double x, double y)> ToDrawImaginarisGraph()
        {
            var x = BeginsAtComplex;
            double span = 1;
            if (SamplingFrequency != 0) span = 1.0 / SamplingFrequency;
            var result = new List<(double x, double y)>();

            foreach (var y in Points)
            {
                result.Add((x.Imaginary, y.Imaginary));
                x += span;
            }

            return result;
        }

        public List<(Complex x, Complex y)> ToDrawComplexGraph()
        {
            var x = BeginsAtComplex;
            double span = 1;
            if (SamplingFrequency != 0) span = 1.0 / SamplingFrequency;
            var result = new List<(Complex x, Complex y)>();

            foreach (var y in Points)
            {
                result.Add((x, y));
                x += span;
            }

            return result;
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(nameof(ComplexSignal));
                sw.WriteLine(Begin);
                sw.WriteLine(Period);
                sw.WriteLine(SamplingFrequency);
                foreach (var y in Points) sw.Write($"{y} ");
            }
        }
    }
}