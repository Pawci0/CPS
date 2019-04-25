using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                {
                    return a / b;
                }
                else return Double.PositiveInfinity;
            };

            return GenericOperation(signal1, signal2, func);
        }

        private static RealSignal GenericOperation(RealSignal signal1, RealSignal signal2, Func<double, double, double> func)
        {
            //todo dodac jakies wyjatki
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                throw new Exception("sampling frequency mismatch");

            var from = Math.Max(signal1.Begin, signal2.Begin);
            var to = Math.Min(signal1.EndsAt, signal2.EndsAt);
            var samplingFrequency = signal1.SamplingFrequency;
            List<double> resultSignalPoints = new List<double>();


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

            int leftFill = Convert.ToInt32(Math.Abs(leftSignal.Begin - from) * samplingFrequency);
            int rightFill = Convert.ToInt32(Math.Abs(rightSignal.EndsAt - to) * samplingFrequency);
            int middleCount = Convert.ToInt32(to * samplingFrequency);

            for (; i < leftFill; i++)
            {
                resultSignalPoints.Add(leftSignal.Points[i]);
            }

            for (; i < middleCount; i++, j++)
            {
                resultSignalPoints.Add(func(leftSignal.Points[i], rightSignal.Points[j]));
            }

            for (; j < rightFill; j++)
            {
                resultSignalPoints.Add(leftSignal.Points[j]);
            }

            return new RealSignal(from, null, samplingFrequency, resultSignalPoints);
        }

        public static double AverageValue(RealSignal signal)
        {
            var points = signal.GetOnlyFullPeriods;
            return Math.Round(points.Sum() / points.Count, 3);
        }

        public static double AbsoluteAverateValue(RealSignal signal)
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

       /* public static double MeanSquareError(RealSignal original, RealSignal sampled)
        {
         List<double> quantizedSignal = QuantizedSignal(orignalSignal.Count(), sampledSignal);

            int N = quantizedSignal.Count;
            double fraction = 1.0 / N;
            double sum = 0;

            for (int i = 0; i < N; i++)
            {
                sum += Math.Pow((orignalSignal[i] - quantizedSignal[i]), 2);
            }

            double result = fraction * sum;

            return Math.Round(result, 4, MidpointRounding.AwayFromZero);
        }*/
    }

}
