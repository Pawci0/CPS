using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

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

        public static RealSignal FullRectifiedSinus(double amplitude, double period, double beginsAt, double duration,
            double samplingFrequency)
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

        public static RealSignal HalfRectifiedSinus(double amplitude, double period, double beginsAt, double duration,
            double samplingFrequency)
        {
            var signal = Sinus(amplitude, period, beginsAt, duration, samplingFrequency);

            for (int i = 0; i < signal.Points.Count; i++)
            {
                if (signal.Points[i] < 0)
                {
                    signal.Points[i] = 0;
                }
            }
            return signal;
        }

        public static RealSignal Rectangular(double amplitude, double period, double beginsAt, double duration,
            double samplingFrequency)
        {
            var signal = Sinus(amplitude, period, beginsAt, duration, samplingFrequency);

            for (int i = 0; i < signal.Points.Count; i++)
            {
                if (signal.Points[i] < 0)
                {
                    signal.Points[i] = 0;
                }
                else
                {
                    signal.Points[i] = amplitude;
                }
            }
            return signal;
        }
        public static RealSignal SymetricalRectangular(double amplitude, double period, double beginsAt, double duration,
            double samplingFrequency)
        {
            var signal = Sinus(amplitude, period, beginsAt, duration, samplingFrequency);

            for (int i = 0; i < signal.Points.Count; i++)
            {
                if (signal.Points[i] < 0)
                {
                    signal.Points[i] = -amplitude;
                }
                else
                {
                    signal.Points[i] = amplitude;
                }
            }
            return signal;
        }
    }
}