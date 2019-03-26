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
            //todo dodac jakies wyjatki
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                throw new Exception("sampling frequency mismatch");

            var from = Math.Min(signal1.Begin, signal2.Begin);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
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

            var length1 = Convert.ToInt32(to - leftSignal.EndsAt);
            var length2 = Convert.ToInt32(rightSignal.Begin - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();

            var resultSignalPoints = rightSignalPoints.Select((t, i) => leftSignalPoints[i] + t).ToList();

            return new RealSignal(from, null, samplingFrequency, resultSignalPoints);
        }

        public static RealSignal SubtractSignals(RealSignal signal1, RealSignal signal2)
        {
            bool subtrahend;
            //todo dodac jakies wyjatki
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                throw new Exception();

            var from = Math.Min(signal1.Begin, signal2.Begin);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.Begin < signal2.Begin)
            {
                subtrahend = true;
                leftSignal = signal1;
                rightSignal = signal2;
            }
            else
            {
                subtrahend = false;
                leftSignal = signal2;
                rightSignal = signal1;
            }

            //todo tu cos jest zjebane
            var length1 = Convert.ToInt32(to - leftSignal.EndsAt);
            var length2 = Convert.ToInt32(rightSignal.Begin - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();

            var resultSignalPoints = subtrahend ? rightSignalPoints.Select((t, i) => leftSignalPoints[i] - t).ToList() : leftSignalPoints.Select((t, i) => rightSignalPoints[i] - t).ToList();

            return new RealSignal(from, null, samplingFrequency, resultSignalPoints);
        }

        public static RealSignal MultiplySignals(RealSignal signal1, RealSignal signal2)
        {
            //todo dodac jakies wyjatki
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                throw new Exception();

            var from = Math.Min(signal1.Begin, signal2.Begin);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
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

            var length1 = Convert.ToInt32(to - leftSignal.EndsAt);
            var length2 = Convert.ToInt32(rightSignal.Begin - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();

            var resultSignalPoints = rightSignalPoints.Select((t, i) => leftSignalPoints[i] * t).ToList();

            return new RealSignal(from, null, samplingFrequency, resultSignalPoints);
        }

        public static RealSignal DivideSignals(RealSignal signal1, RealSignal signal2)
        {
            bool dividend;
            //todo dodac jakies wyjatki
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                throw new Exception() ;

            var from = Math.Min(signal1.Begin, signal2.Begin);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.Begin < signal2.Begin)
            {
                dividend = true;
                leftSignal = signal1;
                rightSignal = signal2;
            }
            else
            {
                dividend = false;

                leftSignal = signal2;
                rightSignal = signal1;
            }

            var length1 = Convert.ToInt32(to - leftSignal.EndsAt);
            var length2 = Convert.ToInt32(rightSignal.Begin - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++)
            {
                list.Add(0.0);
            }


            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();
            var resultSignalPoints = dividend ? rightSignalPoints.Select((t, i) => (Math.Abs(leftSignalPoints[i]) < 1e-10 || Math.Abs(t) < 1e-10) ? 0 : leftSignalPoints[i] / t).ToList() : leftSignalPoints.Select((t, i) => (Math.Abs(rightSignalPoints[i]) < 1e-10 || Math.Abs(t) < 1e-10) ? 0 : rightSignalPoints[i] / t).ToList();

            return new RealSignal(from, null, samplingFrequency, resultSignalPoints);
        }
    }
}
