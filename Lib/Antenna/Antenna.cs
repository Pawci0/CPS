using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Antenna
{
    public class Antenna
    {
        private static readonly Random _random = new Random();

        public static List<(double, double)> CalculateAntenna(int howManyBasicSignals, double startDistance, AntennaParameters antennaParameters, out RealSignal probing, out RealSignal feedback, out RealSignal correlationS )
        {
            var result = new List<(double, double)>();
            var amplitudes = new List<double>();
            var periods = new List<double>();

            for(var k = 0; k<howManyBasicSignals; k++)
            {
                periods.Add(_random.NextDouble() * (antennaParameters.PeriodOfTheProbeSignal - 1e-10) + 1e-10);
                amplitudes.Add(_random.NextDouble() * 50.0 + 1.0);
            } 

            periods[0] = antennaParameters.PeriodOfTheProbeSignal;
            var duration = antennaParameters.LengthOfBuffersOfDiscreteSignals /
                           antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal;

            for (var i = 0.0; i < 12.0 * antennaParameters.ReportingPeriodOfDistance; i += antennaParameters.ReportingPeriodOfDistance)
            {
                var realDistance = startDistance + i * antennaParameters.RealSpeedOfTheObject;
                var propagationTimeToAndFromObject = 2 * (realDistance / antennaParameters.SpeedOfSignalPropagationInEnvironment);
                var probingSignal = CreateSignal(amplitudes, periods, i - duration, duration, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
                var feedbackSignal = CreateSignal(amplitudes, periods, i - propagationTimeToAndFromObject - duration, duration, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
                var correlation =
                    SignalOperations.CorrelationUsingConvolution(probingSignal, feedbackSignal);
                result.Add((realDistance, CalculateDistance(correlation, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal, antennaParameters.SpeedOfSignalPropagationInEnvironment)));
            }

            probing = CreateSignal(amplitudes, periods, 0 - duration, duration,
                antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
            feedback = CreateSignal(amplitudes, periods, 0 - duration, duration, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
            correlationS = new RealSignal(0 - duration, null,
                antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal,
                SignalOperations.CorrelationUsingConvolution(probing, feedback));
            return result;
        }

        private static RealSignal CreateSignal(IReadOnlyList<double> amplitudes, IReadOnlyList<double> periods, double beginsAt, double duration, double samplingFrequency)
        {
            var signal = SinusForAntenna(amplitudes[0], periods[0], beginsAt, duration, samplingFrequency);
            for (var i = 1; i < amplitudes.Count; i++)
            {
                var second = SinusForAntenna(
                    amplitudes[i],
                    periods[i],
                    beginsAt,
                    duration,
                    samplingFrequency);
                signal = AddSignals(signal, second);
            }

            return signal;
        }

        private static double CalculateDistance(IReadOnlyCollection<double> correlation, double samplingFrequencyOfTheProbeAndFeedbackSignal, double speedOfSignalPropagationInEnvironment)
        {
            var rightHalf = correlation.Skip(correlation.Count / 2).ToList();
            var maxSample = rightHalf.IndexOf(rightHalf.Max());
            var tDelay = maxSample / samplingFrequencyOfTheProbeAndFeedbackSignal;

            return (tDelay * speedOfSignalPropagationInEnvironment) / 2;
        }

        private static RealSignal AddSignals(RealSignal signal1, RealSignal signal2)
        {
            if (Math.Abs(signal1.SamplingFrequency - signal2.SamplingFrequency) > 1e-6)
                return null;

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

            var result = new RealSignal(@from, null, samplingFrequency, resultSignalPoints);

            return result;
        }

        private static RealSignal SinusForAntenna(double amplitude, double period, double begin, double duration, double samplingFrequency)
        {
            var points = new List<double>();
            var howManyPoints = duration * samplingFrequency;
            var span = 1.0 / samplingFrequency;

            var i = begin;
            for (var j = 0; j < howManyPoints; i += span, j++)
            {
                points.Add(amplitude * Math.Sin(((Math.PI * 2.0) / period) * (i)));
            }

            return new RealSignal(begin, period, samplingFrequency, points);
        }
    }
}
