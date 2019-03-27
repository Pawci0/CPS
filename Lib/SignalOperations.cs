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
    }
}
