using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace Lib
{
    public class ComplexSignal
    {
        public ComplexSignal(double beginsAt, double samplingFrequency, List<Complex> points)
        {
            BeginsAt = beginsAt;
            SamplingFrequency = samplingFrequency;
            Points = points;
        }

        public double BeginsAt { get; }

        public double SamplingFrequency { get; }

        public List<Complex> Points { get; }

        public double SamplingPeriod => 1.0 / SamplingFrequency;

        public double Length => EndsAt - BeginsAt;

        public double EndsAt => BeginsAt + SamplingPeriod * Points.Count;

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(nameof(ComplexSignal));
                sw.WriteLine(BeginsAt);
                sw.WriteLine(SamplingFrequency);
                foreach (var complex in Points) sw.Write($"{complex.Real}/{complex.Imaginary} ");
            }
        }
    }
}