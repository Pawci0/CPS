using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lib
{
    public static class SignalGenerator
    {
        public static RealSignal UniformNoise(double amplitude, double beginsAt, 
                                              double duration, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var generator = new Random();

            for (var i = 0; i <= howManyPoints; i++)
            {
                points.Add(amplitude * (generator.NextDouble() * 2.0 - 1.0));
            }

            return new RealSignal(beginsAt, null, samplingFrequency, points);
        }

        public static RealSignal GaussianNoise(double amplitude, double beginsAt, 
                                               double duration, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            
            for(var i = 0; i <= howManyPoints; i++)
            {
                points.Add(amplitude * (Normal.Sample(0.5, 0.1) * 2.0 - 1.0));
            }
            return new RealSignal(beginsAt, null, samplingFrequency, points);
        }

        public static RealSignal Sinus(double amplitude, double period, double beginsAt, double duration,
            double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
                points.Add(amplitude * Math.Sin(Math.PI * 2.0 / period * (i - beginsAt)));

            return new RealSignal(beginsAt, period, samplingFrequency, points);
        }

        public static RealSignal SinusOneSideStraightened(double amplitude, double period, double beginsAt,
            double duration, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
                points.Add(0.5 * amplitude * (Math.Sin(Math.PI * 2.0 / period * (i - beginsAt)) +
                                              Math.Abs(Math.Sin(Math.PI * 2.0 / period * (i - beginsAt)))));

            return new RealSignal(beginsAt, period, samplingFrequency, points);
        }

        public static RealSignal SinusBothSideStraightened(double amplitude, double period, double beginsAt,
            double duration, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
                points.Add(amplitude * Math.Abs(Math.Sin(Math.PI * 2.0 / period * (i - beginsAt))));

            return new RealSignal(beginsAt, period, samplingFrequency, points);
        }

        public static RealSignal Rectangular(double amplitude, double period, double beginsAt, double duration,
            double fillFactor, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;
            var k = 0;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
            {
                if (i >= beginsAt + (k + 1) * period)
                    k++;
                if (i >= k * period + beginsAt && i < fillFactor * period + k * period + beginsAt)
                    points.Add(amplitude);
                else if (i >= fillFactor * period + k * period + beginsAt && i < period + k * period + beginsAt)
                    points.Add(0.0);
            }

            return new RealSignal(beginsAt, period, samplingFrequency, points);
        }

        public static RealSignal RectangularSymmetrical(double amplitude, double period, double beginsAt,
            double duration, double fillFactor, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;
            var k = 0;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
            {
                if (i >= beginsAt + (k + 1) * period)
                    k++;
                if (i >= k * period + beginsAt && i < fillFactor * period + k * period + beginsAt)
                    points.Add(amplitude);
                if (i >= fillFactor * period + k * period + beginsAt && i < period + k * period + beginsAt)
                    points.Add(-amplitude);
            }

            return new RealSignal(beginsAt, period, samplingFrequency, points);
        }

        public static RealSignal Triangular(double amplitude, double period, double beginsAt, double duration,
            double fillFactor, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;
            var k = 0;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
            {
                if (i >= beginsAt + (k + 1) * period)
                    k++;
                if (i >= k * period + beginsAt && i < fillFactor * period + k * period + beginsAt)
                    points.Add(amplitude / (fillFactor * period) * (i - k * period - beginsAt));
                if (i >= fillFactor * period + k * period + beginsAt && i < period + k * period + beginsAt)
                    points.Add(-amplitude / (period * (1 - fillFactor)) * (i - k * period - beginsAt) +
                               amplitude / (1 - fillFactor));
            }

            return new RealSignal(beginsAt, period, samplingFrequency, points);
        }

        public static RealSignal HeavisideStep(double amplitude, double beginsAt, double duration, double jumpsAt,
            double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++)
                if (Math.Abs(i - jumpsAt) < 1e-6)
                    points.Add(0.5);
                else if (i < jumpsAt)
                    points.Add(0.0);
                else
                    points.Add(amplitude);

            return new RealSignal(beginsAt, null, samplingFrequency, points);
        }

        public static RealSignal KroneckerDelta(double amplitude, double beginsAt, double duration, double jumpsAt,
            double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;

            var i = beginsAt;
            var j = 0;
            for (; j < howManyPoints; i += span, j++) points.Add(Math.Abs(i - jumpsAt) > 1e-6 ? 0.0 : amplitude);

            return new RealSignal(beginsAt, null, samplingFrequency, points);
        }

        public static RealSignal ImpulsiveNoise(double amplitude, double beginsAt, double duration, double probability,
            double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var r = new Random();

            var j = 0;
            for (; j < howManyPoints; j++) points.Add(r.NextDouble() < probability ? amplitude : 0.0);

            return new RealSignal(beginsAt, null, samplingFrequency, points);
        }
    }
}