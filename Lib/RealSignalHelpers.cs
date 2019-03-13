using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Lib
{
    public class RealSignalHelpers
    {
        public static object ReadFromFile(string path)
        {
            object signal;
            using (var sr = new StreamReader(path))
            {
                var type = sr.ReadLine();
                switch (type)
                {
                    case nameof(RealSignal):
                        double.TryParse(sr.ReadLine(), out var realBeginsAt);
                        double? period;
                        if (!double.TryParse(sr.ReadLine(), out var p))
                            period = null;
                        else
                            period = p;

                        double.TryParse(sr.ReadLine(), out var realSamplingFrequency);
                        var realPoints = sr.ReadLine()?.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Select(double.Parse).ToList();
                        signal = new RealSignal(realBeginsAt, period, realSamplingFrequency, realPoints);
                        break;
                    case nameof(ComplexSignal):
                        double.TryParse(sr.ReadLine(), out var complexBeginsAt);
                        double.TryParse(sr.ReadLine(), out var complexSamplingFrequency);
                        var complexPoints = sr.ReadLine()?.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries))
                            .Select(x => new Complex(double.Parse(x[0]), double.Parse(x[1]))).ToList();
                        signal = new ComplexSignal(complexBeginsAt, complexSamplingFrequency, complexPoints);
                        break;
                    default:
                        signal = null;
                        break;
                }
            }

            return signal;
        }

        public static bool AddSignals(RealSignal signal1, RealSignal signal2, out RealSignal result)
        {
            result = null;
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                return false;

            var from = Math.Min(signal1.BeginsAt, signal2.BeginsAt);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.BeginsAt < signal2.BeginsAt)
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
            var length2 = Convert.ToInt32(rightSignal.BeginsAt - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++) list.Add(0.0);

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++) list.Add(0.0);

            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();

            var resultSignalPoints = rightSignalPoints.Select((t, i) => leftSignalPoints[i] + t).ToList();

            result = new RealSignal(from, null, samplingFrequency, resultSignalPoints);

            return true;
        }

        public static bool SubtractSignals(RealSignal signal1, RealSignal signal2, out RealSignal result)
        {
            bool subtrahend;
            result = null;
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                return false;

            var from = Math.Min(signal1.BeginsAt, signal2.BeginsAt);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.BeginsAt < signal2.BeginsAt)
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

            var length1 = Convert.ToInt32(to - leftSignal.EndsAt);
            var length2 = Convert.ToInt32(rightSignal.BeginsAt - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++) list.Add(0.0);

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++) list.Add(0.0);

            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();

            var resultSignalPoints = subtrahend
                ? rightSignalPoints.Select((t, i) => leftSignalPoints[i] - t).ToList()
                : leftSignalPoints.Select((t, i) => rightSignalPoints[i] - t).ToList();

            result = new RealSignal(from, null, samplingFrequency, resultSignalPoints);

            return true;
        }

        public static bool MultiplySignals(RealSignal signal1, RealSignal signal2, out RealSignal result)
        {
            result = null;
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                return false;

            var from = Math.Min(signal1.BeginsAt, signal2.BeginsAt);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.BeginsAt < signal2.BeginsAt)
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
            var length2 = Convert.ToInt32(rightSignal.BeginsAt - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++) list.Add(0.0);

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++) list.Add(0.0);

            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();

            var resultSignalPoints = rightSignalPoints.Select((t, i) => leftSignalPoints[i] * t).ToList();

            result = new RealSignal(from, null, samplingFrequency, resultSignalPoints);

            return true;
        }

        public static bool DivideSignals(RealSignal signal1, RealSignal signal2, out RealSignal result)
        {
            bool dividend;
            result = null;
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                return false;

            var from = Math.Min(signal1.BeginsAt, signal2.BeginsAt);
            var to = Math.Max(signal1.EndsAt, signal2.EndsAt);

            var samplingFrequency = signal1.SamplingFrequency;
            RealSignal leftSignal;
            RealSignal rightSignal;

            if (signal1.BeginsAt < signal2.BeginsAt)
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
            var length2 = Convert.ToInt32(rightSignal.BeginsAt - from);

            var list = new List<double>();

            for (var i = 0; i < length1 * samplingFrequency; i++) list.Add(0.0);

            var leftSignalPoints = leftSignal.Points.Concat(list).ToList();

            list = new List<double>();

            for (var i = 0; i < length2 * samplingFrequency; i++) list.Add(0.0);


            var rightSignalPoints = list.Concat(rightSignal.Points).ToList();
            var resultSignalPoints = dividend
                ? rightSignalPoints.Select((t, i) =>
                    Math.Abs(leftSignalPoints[i]) < 1e-10 || Math.Abs(t) < 1e-10 ? 0 : leftSignalPoints[i] / t).ToList()
                : leftSignalPoints.Select((t, i) =>
                        Math.Abs(rightSignalPoints[i]) < 1e-10 || Math.Abs(t) < 1e-10 ? 0 : rightSignalPoints[i] / t)
                    .ToList();

            result = new RealSignal(from, null, samplingFrequency, resultSignalPoints);

            return true;
        }
    }
}