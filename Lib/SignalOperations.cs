using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib
{
    public class SignalOperations
    {
        public static RealSignal AddSignals(RealSignal signal1, RealSignal signal2)
        {
            return GenericOperation(signal1, signal2, (a, b) => a + b);
        }

        public static RealSignal SubtractSignals(RealSignal signal1, RealSignal signal2)
        {
            return GenericOperation(signal1, signal2, (a, b) => a - b);
        }

        public static RealSignal MultiplySignals(RealSignal signal1, RealSignal signal2)
        {
            return GenericOperation(signal1, signal2, (a, b) => a * b);
        }

        public static RealSignal DivideSignals(RealSignal signal1, RealSignal signal2)
        {
            Func<double, double, double> func = (a, b) =>
            {
                if (b != 0)
                    return a / b;
                return double.PositiveInfinity;
            };

            return GenericOperation(signal1, signal2, func);
        }

        private static RealSignal GenericOperation(RealSignal signal1, RealSignal signal2,
            Func<double, double, double> func)
        {
            //todo dodac jakies wyjatki
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                throw new Exception("sampling frequency mismatch");

            var from = Math.Max(signal1.Begin, signal2.Begin);
            var to = Math.Min(signal1.EndsAt, signal2.EndsAt);
            var samplingFrequency = signal1.SamplingFrequency;
            var resultSignalPoints = new List<double>();


            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.Begin < signal2.Begin)
            {
                leftSignal = signal1;
                rightSignal = signal2;
            }
            else
            {
                leftSignal = signal2;
                rightSignal = signal1;
            }

            int i = 0, j = 0;

            var leftFill = Convert.ToInt32(Math.Abs(leftSignal.Begin - from) * samplingFrequency);
            var rightFill = Convert.ToInt32(Math.Abs(rightSignal.EndsAt - to) * samplingFrequency);
            var middleCount = Convert.ToInt32(to * samplingFrequency);

            for (; i < leftFill; i++) resultSignalPoints.Add(leftSignal.Points[i]);

            for (; i < middleCount; i++, j++) resultSignalPoints.Add(func(leftSignal.Points[i], rightSignal.Points[j]));

            for (; j < rightFill; j++) resultSignalPoints.Add(leftSignal.Points[j]);

            return new RealSignal(from, null, samplingFrequency, resultSignalPoints);
        }

        public static double AverageValue(RealSignal signal)
        {
            var points = signal.GetOnlyFullPeriods;
            return Math.Round(points.Sum() / points.Count, 3);
        }

        public static double AbsoluteAverageValue(RealSignal signal)
        {
            var points = signal.GetOnlyFullPeriods;
            return Math.Round(points.Sum(x => Math.Abs(x)) / points.Count, 3);
        }

        public static double AveragePower(RealSignal signal)
        {
            var points = signal.GetOnlyFullPeriods;
            return Math.Round(points.Sum(x => x * x) / points.Count, 3);
        }

        public static double Variance(RealSignal signal)
        {
            var points = signal.GetOnlyFullPeriods;
            var average = AverageValue(signal);
            return Math.Round(points.Sum(x => (x - average) * (x - average)) / points.Count, 3);
        }

        public static double RootMeanSquare(RealSignal signal)
        {
            return Math.Round(Math.Sqrt(AveragePower(signal)), 3);
        }

        public static double MeanSquaredError(RealSignal original, RealSignal sampled)
        {
            if (original == null || sampled == null)
                return 0;
            var quantizedSignal = QuantizedSignal(original.Points.Count(), sampled.Points);

            var N = quantizedSignal.Count;
            var fraction = 1.0 / N;
            double sum = 0;

            for (var i = 0; i < N; i++) sum += Math.Pow(original.Points[i] - quantizedSignal[i], 2);

            var result = fraction * sum;

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }

        private static double MeanSquaredError(RealSignal original, List<double> sampled)
        {
            var quantizedSignal = QuantizedSignal(original.Points.Count(), sampled);

            var N = quantizedSignal.Count;
            var fraction = 1.0 / N;
            double sum = 0;

            for (var i = 0; i < N; i++) sum += Math.Pow(original.Points[i] - quantizedSignal[i], 2);

            var result = fraction * sum;

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }


        public static double SignalToNoiseRatio(RealSignal orignalSignal, RealSignal sampledSignal)
        {
            if (orignalSignal == null || sampledSignal == null)
                return 0;
            var quantizedSignal = QuantizedSignal(orignalSignal.Points.Count(), sampledSignal.Points);

            double numerator = 0;
            double denominator = 0;
            var N = quantizedSignal.Count;

            for (var i = 0; i < N; i++) numerator += Math.Pow(orignalSignal.Points[i], 2);

            for (var i = 0; i < N; i++) denominator += Math.Pow(orignalSignal.Points[i] - quantizedSignal[i], 2);

            var result = 10 * Math.Log10(numerator / denominator);

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }

        private static double SignalToNoiseRatio(RealSignal orignalSignal, List<double> sampledSignal)
        {
            var quantizedSignal = QuantizedSignal(orignalSignal.Points.Count(), sampledSignal);

            double numerator = 0;
            double denominator = 0;
            var N = quantizedSignal.Count;

            for (var i = 0; i < N; i++) numerator += Math.Pow(orignalSignal.Points[i], 2);

            for (var i = 0; i < N; i++) denominator += Math.Pow(orignalSignal.Points[i] - quantizedSignal[i], 2);

            var result = 10 * Math.Log10(numerator / denominator);

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }

        public static double PeakSignalToNoiseRatio(RealSignal orignalSignal, RealSignal sampledSignal)
        {
            if (orignalSignal == null || sampledSignal == null)
                return 0;
            var quantizedSignal = QuantizedSignal(orignalSignal.Points.Count(), sampledSignal.Points);

            var mse = MeanSquaredError(orignalSignal, quantizedSignal);
            var numerator = quantizedSignal.Max();

            var result = 10 * Math.Log10(numerator / mse);

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }

        public static double MaximumDifference(RealSignal orignalSignal, RealSignal sampledSignal)
        {
            if (orignalSignal == null || sampledSignal == null)
                return 0;
            var quantizedSignal = QuantizedSignal(orignalSignal.Points.Count(), sampledSignal.Points);

            var N = quantizedSignal.Count;
            var differences = new List<double>(N);

            for (var i = 0; i < N; i++) differences.Add(Math.Abs(orignalSignal.Points[i] - quantizedSignal[i]));

            var result = differences.Max();

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }

        public static double EffectiveNumberOfBits(RealSignal orignalSignal, RealSignal sampledSignal)
        {
            if (orignalSignal == null || sampledSignal == null)
                return 0;
            var quantizedSignal = QuantizedSignal(orignalSignal.Points.Count(), sampledSignal.Points);

            var snr = SignalToNoiseRatio(orignalSignal, quantizedSignal);

            var result = (snr - 1.76) / 6.02;

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }


        private static List<double> QuantizedSignal(int orignalSignalCount, List<double> sampledSignal)
        {
            var result = new List<double>();

            for (var i = 0; i < sampledSignal.Count(); i++)
            for (var j = 0; j < orignalSignalCount / sampledSignal.Count(); j++)
                result.Add(sampledSignal[i]);

            return result;
        }

        public static List<double> Convolution(List<double> one, List<double> two)
        {
            var resultPoints = new List<double>();
            var resultLength = one.Count + two.Count - 1;

            for (var n = 0; n < resultLength; n++)
            {
                double sum = 0;
                var kmin = n >= two.Count - 1 ? n - (two.Count - 1) : 0;
                var kmax = n < one.Count - 1 ? n : one.Count - 1;

                for (var k = kmin; k <= kmax; k++) sum += one[k] * two[n - k];
                resultPoints.Add(sum);
            }

            return resultPoints;
        }

        public static RealSignal Convolution(RealSignal one, RealSignal two)
        {
            var first = one.Points;
            var second = two.Points;

            var resultPoints = Convolution(first, second);

            var result = new RealSignal(resultPoints);

            return result;
        }

        public static List<double> Correlation(RealSignal one, RealSignal two)
        {
            var first = one.Points;
            var second = two.Points;

            var result = new List<double>();
            var resultLength = first.Count + second.Count - 1;

            for (var n = 0; n < resultLength; n++)
            {
                var sum = 0d;
                var k1Min = n >= second.Count - 1 ? n - (second.Count - 1) : 0;
                var k1Max = n < first.Count - 1 ? n : first.Count - 1;
                var k2Min = n <= second.Count - 1 ? second.Count - 1 - n : 0;

                for (int k1 = k1Min, k2 = k2Min; k1 <= k1Max; k1++, k2++) sum += first[k1] * second[k2];
                result.Add(sum);
            }

            return result;
        }

        public static List<double> CorrelationUsingConvolution(RealSignal one, RealSignal two)
        {
            one.Points.Reverse();
            return Convolution(one, two).Points;
        }
    }
}