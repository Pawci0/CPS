using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class ACUtils
    {
        public static List<(double, double)> SincReconstruction(RealSignal signal, double frequency)
        {
            List<(double, double)> result = new List<(double, double)>();
            var duration = signal.Points.Count / signal.SamplingFrequency;
            var pointCount = duration * frequency;
            double time = signal.Begin;
            for (int i = 0; i < pointCount; i++)
            {
                result.Add((time, ReconstructPoint(signal.Points, time, signal.SamplingFrequency)));
                time += 1/frequency;
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
