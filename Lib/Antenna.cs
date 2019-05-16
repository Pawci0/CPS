using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lib.AntennaParameters;

namespace Lib
{
    public class Antenna
    {
        private static readonly Random _random = new Random();

        public static List<(double, double)> CalculateAntenna(int howManyBasicSignals, double startDistance, AntennaParameters antennaParameters/*, out List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)> signalsList*/)
        {
            var result = new List<(double, double)>();
           // signalsList = new List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)>();
            var amplitudes = new List<double>();
            var periods = new List<double>();
            var k = 0;
            do
            {
                periods.Add(_random.NextDouble() * (antennaParameters.PeriodOfTheProbeSignal - 1e-10) + 1e-10);
                amplitudes.Add(_random.NextDouble() * 50.0 + 1.0);
                k++;
            } while (k < howManyBasicSignals);

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
             //   signalsList.Add((probingSignal, feedbackSignal, correlation));
                result.Add((realDistance, CalculateDistance(correlation, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal, antennaParameters.SpeedOfSignalPropagationInEnvironment)));
            }

            return result;
        }

        private static RealSignal CreateSignal(List<double> amplitudes, List<double> periods, double beginsAt, double duration, double samplingFrequency)
        {
            var signal = SignalGenerator.Sinus(amplitudes[0], periods[0], beginsAt, duration, samplingFrequency);
            for (var i = 1; i < amplitudes.Count; i++)
            {
                var second = SignalGenerator.Sinus(
                    amplitudes[i],
                    periods[i],
                    beginsAt,
                    duration,
                    samplingFrequency);
                signal = AddSignals(signal, second);
            }

            return signal;
        }

        private static double CalculateDistance(List<double> correlation, double samplingFrequencyOfTheProbeAndFeedbackSignal, double speedOfSignalPropagationInEnvironment)
        {
            var rightHalf = correlation.Skip(correlation.Count / 2).ToList();
            var maxSample = rightHalf.IndexOf(rightHalf.Max());
            var tDelay = maxSample / samplingFrequencyOfTheProbeAndFeedbackSignal;

            return (tDelay * speedOfSignalPropagationInEnvironment) / 2;
        }

        private static RealSignal AddSignals(RealSignal signal1, RealSignal signal2)
        {
            RealSignal result = null;
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

            result = new RealSignal(from, null, samplingFrequency, resultSignalPoints);

            return result;
        }
    }
}
