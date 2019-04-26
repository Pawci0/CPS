using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class ACUtils
    {
        public static List<double> SincReconstruction(RealSignal signal)
        {
            List<double> result = new List<double>();
            var pointCount = signal.Points.Count;
            double time = signal.Begin;
            for (int i = 0; i < pointCount; i++)
            {
                result.Add(ReconstructPoint(signal.Points, time, signal.SamplingFrequency));
                time += 1/signal.SamplingFrequency;
            }
            return result;
        }

        private static double ReconstructPoint(List<double> sampledY, double time, double frequency)
        {
            double result = 0;

            double T_s = 1 / frequency;

            for (int n = 0; n < sampledY.Count(); n++)
            {
                result += sampledY[n] * SinusCardinalis(time / T_s - n);
            }

            return result;
        }

        private static double SinusCardinalis(double t)
        {
            if (Math.Round(t, 6).Equals(0))
            {
                return 1;
            }

            return Math.Sin(Math.PI * t) / (Math.PI * t);
        }
    }
}
