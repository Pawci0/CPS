using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib
{
    public static class ACUtils
    {
        public static List<(double, double)> SincReconstruction(RealSignal signal, double frequency, int nOfSamples)
        {
            var result = new List<(double, double)>();
            var duration = signal.Points.Count / signal.SamplingFrequency;
            var pointCount = duration * frequency;
            var time = signal.Begin;
            for (var i = 0; i < pointCount; i++)
            {
                var sampledPoints = signal.GetPointsNear(time, nOfSamples);
                (double time, double value) item = (time: time,
                    ReconstructPoint(sampledPoints, time, signal.SamplingFrequency));
                result.Add(item);
                time += 1 / frequency;
            }

            return result;
        }

        private static double ReconstructPoint(List<(int i, double y)> sampledPoints, double time, double frequency)
        {
            double result = 0;

            var T_s = 1 / frequency;

            foreach (var sample in sampledPoints) result += sample.y * SinusCardinalis(time / T_s - sample.i);

            return result;
        }

        private static double ReconstructPoint(List<double> allPoints, double time, double frequency)
        {
            double result = 0;

            var T_s = 1 / frequency;

            for (var n = 0; n < allPoints.Count(); n++) result += allPoints[n] * SinusCardinalis(time / T_s - n);

            return result;
        }

        private static double SinusCardinalis(double t)
        {
            if (Math.Round(t, 6).Equals(0)) return 1;

            return Math.Sin(Math.PI * t) / (Math.PI * t);
        }
    }
}