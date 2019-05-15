using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lib.AntennaParameters;

namespace Lib
{
    class Antenna
    {
        private readonly Random _random = new Random();

       /* public List<(double, double)> Start(RealSignal probingSignal, RealSignal feedbackSignal, double startDistance, AntennaParameters antennaParameters, out List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)> signalsList)
        {
            var result = new List<(double, double)>();
            signalsList = new List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)>();
            //var _random = new Random();
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
                var probingSignal = new RealSignal(amplitudes, periods, i - duration, duration, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
                var feedbackSignal = new RealSignal(amplitudes, periods, i - propagationTimeToAndFromObject - duration, duration, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
                var correlation =
                    SignalOperations.CorrelationUsingConvolution(probingSignal.Points, feedbackSignal.Points);
                signalsList.Add((probingSignal, feedbackSignal, correlation));
                result.Add((realDistance, CalculateDistance(correlation, antennaParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal, antennaParameters.SpeedOfSignalPropagationInEnvironment)));
            }

            return result;
        }*/

        private static double CalculateDistance(List<double> correlation, double samplingFrequencyOfTheProbeAndFeedbackSignal, double speedOfSignalPropagationInEnvironment)
        {
            var rightHalf = correlation.Skip(correlation.Count / 2).ToList();
            var maxSample = rightHalf.IndexOf(rightHalf.Max());
            var tDelay = maxSample / samplingFrequencyOfTheProbeAndFeedbackSignal;

            return (tDelay * speedOfSignalPropagationInEnvironment) / 2;
        }
    }
}
