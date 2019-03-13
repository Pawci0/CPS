using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib
{
    public static class AnalogAndDigitalConverter
    {
        public static RealSignal UniformSampling(RealSignal signal, double samplingFrequency)
        {
            var rawFrequency = (signal.SamplingFrequency / samplingFrequency);
            var frequency = (int)rawFrequency;
            if (Math.Abs(rawFrequency - frequency) > 1e-10)
            {
                throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
            }
            var result = new List<double>();

            for (var i = 0; i < signal.Points.Count / frequency; i++)
            {
                result.Add(signal[i * frequency]);
            }

            return new RealSignal(signal.BeginsAt, signal.Period, samplingFrequency, result);
        }

        public static RealSignal UniformQuantizationWithTruncation(RealSignal signal, int numberOfLevels)
        {
            var points = new List<double>();

            foreach (var y in Transform(signal.Points, numberOfLevels))
            {
                points.Add(Math.Floor(y));
            }

            return new RealSignal(signal.BeginsAt, signal.Period, signal.SamplingFrequency, Transform1(points, signal.Points.Min(), signal.Points.Max()));
        }

        public static RealSignal ZeroOrderHold(RealSignal signal)
        {
            var frequencyMultiplier = 10;
            var frequency = signal.SamplingFrequency * frequencyMultiplier;
            var result = new List<double>();

            foreach (var y in signal.Points)
            {
                result.AddRange(Enumerable.Repeat(y, frequencyMultiplier));
            }

            return new RealSignal(signal.BeginsAt, signal.Period, frequency, result);
        }

        public static RealSignal ReconstructionBasedOnTheSincFunction(RealSignal signal, int numberOfIncludedSamples)
        {
            var frequencyMultiplier = 10;
            var frequency = signal.SamplingFrequency * frequencyMultiplier;
            var samplingPeriod = 1 / frequency;
            var result = new List<double>();
            var t = signal.BeginsAt;

            for (var i = 0; i < signal.Points.Count * frequencyMultiplier; i++)
            {
                result.Add(signal.GetPointsNear(t, numberOfIncludedSamples).Sum(x => x.y * SincFunction(Math.PI * ((t / signal.SamplingPeriod) - x.n))));
                t += samplingPeriod;
            }

            //for (var i = 0; i < signal.Points.Count; i++)
            //{
            //    var startNn = n - (numberOfIncludedSamples / 2);
            //    var startN = startNn >= 0 ? startNn : 0;
            //    result.Add(signal.Points.Skip(startN).Take(i == 0 ? 3 : i == 1 ? 4 : 5).Sum(x => x * SincFunction((t / signal.SamplingPeriod) - startN++)));
            //    t += samplingPeriod;
            //    n++;
            //}

            return new RealSignal(signal.BeginsAt, signal.Period, frequency, result);
        }

        private static double SincFunction(double x)
        {
            return Math.Abs(x) > 1e-10 ? Math.Sin(x) / x : 1;
        }

        private static List<double> Transform(List<double> list, int numberOfLevels)
        {
            var result = new List<double>();
            var min = list.Min();
            var max = list.Max();
            var span = max - min;

            foreach (var d in list)
            {
                result.Add(((d - min) / span) * (numberOfLevels - 1));
            }

            return result;
        }

        private static List<double> Transform1(List<double> list, double newMin, double newMax)
        {
            var result = new List<double>();
            var min = list.Min();
            var max = list.Max();
            var span = max - min;

            foreach (var d in list)
            {
                result.Add(((d - min) / span) * (newMax - newMin) + newMin);
            }

            return result;
        }

        public static double MeanSquaredError(RealSignal originalSignal, RealSignal convertedSignal)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (originalSignal.SamplingFrequency == convertedSignal.SamplingFrequency || originalSignal.Points.Count == convertedSignal.Points.Count)
                return originalSignal.Points.Zip(convertedSignal.Points, (s1, s2) => s1 - s2).Average(x => x * x);
            if (originalSignal.SamplingFrequency > convertedSignal.SamplingFrequency)
            {
                var rawFrequency = (originalSignal.SamplingFrequency / convertedSignal.SamplingFrequency);
                var frequency = (int)rawFrequency;
                if (Math.Abs(rawFrequency - frequency) > 1e-10)
                {
                    throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
                }

                return convertedSignal.Points.Select((x, i) => x - originalSignal[i * frequency]).Average(x => x * x);
            }
            else
            {
                var rawFrequency = (convertedSignal.SamplingFrequency / originalSignal.SamplingFrequency);
                var frequency = (int)rawFrequency;
                if (Math.Abs(rawFrequency - frequency) > 1e-10)
                {
                    throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
                }

                return originalSignal.Points.Select((x, i) => x - convertedSignal[i * frequency]).Average(x => x * x);
            }
        }

        public static double SignalToNoiseRatio(RealSignal originalSignal, RealSignal convertedSignal)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (originalSignal.SamplingFrequency == convertedSignal.SamplingFrequency || originalSignal.Points.Count == convertedSignal.Points.Count)
            {
                var nominator = originalSignal.Points.Sum(x => x * x);
                var denominator = originalSignal.Points.Zip(convertedSignal.Points, (s1, s2) => s1 - s2).Sum(x => x * x);

                return 10.0 * Math.Log10(nominator / denominator);
            }

            if (originalSignal.SamplingFrequency > convertedSignal.SamplingFrequency)
            {
                var rawFrequency = (originalSignal.SamplingFrequency / convertedSignal.SamplingFrequency);
                var frequency = (int)rawFrequency;
                if (Math.Abs(rawFrequency - frequency) > 1e-10)
                {
                    throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
                }

                var nominator = originalSignal.Points.Sum(x => x * x);
                var denominator = convertedSignal.Points.Select((x, i) => x - originalSignal[i * frequency]).Sum(x => x * x);

                return 10.0 * Math.Log10(nominator / denominator);
            }
            else
            {
                var rawFrequency = (convertedSignal.SamplingFrequency / originalSignal.SamplingFrequency);
                var frequency = (int)rawFrequency;
                if (Math.Abs(rawFrequency - frequency) > 1e-10)
                {
                    throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
                }

                var nominator = originalSignal.Points.Sum(x => x * x);
                var denominator = originalSignal.Points.Select((x, i) => x - convertedSignal[i * frequency]).Sum(x => x * x);

                return 10.0 * Math.Log10(nominator / denominator);
            }
        }

        public static double PeakSignalToNoiseRatio(RealSignal originalSignal, RealSignal convertedSignal)
        {
            return 10.0 * Math.Log10(originalSignal.Points.Max() / MeanSquaredError(originalSignal, convertedSignal));
        }

        public static double MaximumDifference(RealSignal originalSignal, RealSignal convertedSignal)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (originalSignal.SamplingFrequency == convertedSignal.SamplingFrequency || originalSignal.Points.Count == convertedSignal.Points.Count)
                return originalSignal.Points.Zip(convertedSignal.Points, (s1, s2) => s1 - s2).Max(x => Math.Abs(x));
            if (originalSignal.SamplingFrequency > convertedSignal.SamplingFrequency)
            {
                var rawFrequency = (originalSignal.SamplingFrequency / convertedSignal.SamplingFrequency);
                var frequency = (int)rawFrequency;
                if (Math.Abs(rawFrequency - frequency) > 1e-10)
                {
                    throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
                }

                return convertedSignal.Points.Select((x, i) => x - originalSignal[i * frequency]).Max(x => Math.Abs(x));
            }
            else
            {
                var rawFrequency = (convertedSignal.SamplingFrequency / originalSignal.SamplingFrequency);
                var frequency = (int)rawFrequency;
                if (Math.Abs(rawFrequency - frequency) > 1e-10)
                {
                    throw new Exception("Częstotliwość próbkowania musi być całkowitą wielokrotnością częstotliwości sygnału (chyba xD)");
                }

                return originalSignal.Points.Select((x, i) => x - convertedSignal[i * frequency]).Max(x => Math.Abs(x));
            }
        }

        public static double EffectiveNumberOfBits(RealSignal signal1, RealSignal signal2)
        {
            return (SignalToNoiseRatio(signal1, signal2) - 1.76) / 6.02;
        }
    }
}
