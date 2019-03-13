using Lib.Task3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Task3
{
    public class Antenna
    {
        private readonly Random _random = new Random();

        public List<(double, double)> Start(int howManyBasicSignals, double startDistance, AntennaObjectAndEnvironmentParameters objectAndEnvironmentParameters, AntennaDistanceSensorParameters distanceSensorParameters, out List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)> signalsList)
        {
            var result = new List<(double, double)>();
            signalsList = new List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)>();
            //var _random = new Random();
            var amplitudes = new List<double>();
            var periods = new List<double>();
            var k = 0;
            do
            {
                periods.Add(_random.NextDouble() * (distanceSensorParameters.PeriodOfTheProbeSignal - 1e-10) + 1e-10);
                amplitudes.Add(_random.NextDouble() * 50.0 + 1.0);
                k++;
            } while (k < howManyBasicSignals);

            periods[0] = distanceSensorParameters.PeriodOfTheProbeSignal;
            var duration = distanceSensorParameters.LengthOfBuffersOfDiscreteSignals /
                           distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal;

            for (var i = 0.0; i < 12.0 * distanceSensorParameters.ReportingPeriodOfDistance; i += distanceSensorParameters.ReportingPeriodOfDistance)
            {
                var realDistance = startDistance + i * objectAndEnvironmentParameters.RealSpeedOfTheObject;
                var propagationTimeToAndFromObject = 2 * (realDistance / objectAndEnvironmentParameters.SpeedOfSignalPropagationInEnvironment);
                var probingSignal = CreateSignal(amplitudes, periods, i - duration, duration, distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
                var feedbackSignal = CreateSignal(amplitudes, periods, i - propagationTimeToAndFromObject - duration, duration, distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);
                var correlation =
                    OperationsHelper.CorrelationUsingConvolution(probingSignal.Points, feedbackSignal.Points);
                signalsList.Add((probingSignal, feedbackSignal, correlation));
                result.Add((realDistance, CalculateDistance(correlation, distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal, objectAndEnvironmentParameters.SpeedOfSignalPropagationInEnvironment)));
            }

            return result;
        }

        private static double CalculateDistance(List<double> correlation, double samplingFrequencyOfTheProbeAndFeedbackSignal, double speedOfSignalPropagationInEnvironment)
        {
            var rightHalf = correlation.Skip(correlation.Count / 2).ToList();
            var maxSample = rightHalf.IndexOf(rightHalf.Max());
            var tDelay = maxSample / samplingFrequencyOfTheProbeAndFeedbackSignal;

            return (tDelay * speedOfSignalPropagationInEnvironment) / 2;
        }

        private static RealSignal CreateSignal(List<double> amplitudes, List<double> periods, double beginsAt, double duration, double samplingFrequency)
        {
            var signal = SignalGenerator.SinusForAntenna(amplitudes[0], periods[0], beginsAt, duration, samplingFrequency);
            for (var i = 1; i < amplitudes.Count; i++)
            {
                RealSignalHelpers.AddSignals(signal,
                    SignalGenerator.SinusForAntenna(
                        amplitudes[i],
                        periods[i],
                        beginsAt,
                        duration,
                        samplingFrequency
                        ),
                    out signal);
            }

            return signal;
        }

        public List<(double, double)> ReceiveSendSignal(int howManyBasicSignals, double startDistance, AntennaObjectAndEnvironmentParameters objectAndEnvironmentParameters, AntennaDistanceSensorParameters distanceSensorParameters)
        {
            var result = new List<(double, double)>();
            var random = new Random();
            var parameters = new List<double>();
            var k = 0;
            do
            {
                parameters.Add(random.NextDouble() * 50 + 1);
                k++;
            } while (k < howManyBasicSignals);

            for (var i = distanceSensorParameters.ReportingPeriodOfDistance; i < distanceSensorParameters.ReportingPeriodOfDistance * 12; i += distanceSensorParameters.ReportingPeriodOfDistance)
            {
                var realDistance = startDistance + i * objectAndEnvironmentParameters.RealSpeedOfTheObject;
                var cos = (realDistance * 2) / objectAndEnvironmentParameters.SpeedOfSignalPropagationInEnvironment;
                var skip = (int)(cos * distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);

                var signal = SignalGenerator.SinusForAntenna(
                    parameters[0],
                    distanceSensorParameters.PeriodOfTheProbeSignal,
                    i,
                    500,
                    distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal
                );
                for (var j = 1; j < parameters.Count; j++)
                {
                    RealSignalHelpers.AddSignals(
                        signal,
                        SignalGenerator.SinusForAntenna(
                            parameters[j],
                            distanceSensorParameters.PeriodOfTheProbeSignal,
                            i,
                            500,
                            distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal
                        ),
                        out signal);
                }

                var probingSignal = signal.Points;
                var feedbackSignal = probingSignal.Skip(skip).Skip(Math.Max(0, probingSignal.Count - distanceSensorParameters.LengthOfBuffersOfDiscreteSignals - skip)).ToList();

                var correlation = OperationsHelper.CorrelationUsingConvolution(probingSignal.Skip(Math.Max(0, probingSignal.Count - distanceSensorParameters.LengthOfBuffersOfDiscreteSignals)).ToList(), feedbackSignal);
                var rightHalf = correlation.Skip(correlation.Count / 2).ToList();
                var maxSample = rightHalf.IndexOf(rightHalf.Max());
                var tDelay = maxSample / distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal;

                var distanceFromRadarToObjectAndFromObjectToRadar =
                    objectAndEnvironmentParameters.SpeedOfSignalPropagationInEnvironment * tDelay;
                var distanceFromRadarToObject = distanceFromRadarToObjectAndFromObjectToRadar / 2;

                result.Add((realDistance, distanceFromRadarToObject));
            }

            return result;
        }

        public List<(double, double)> ReceiveSendSignal2(int howManyBasicSignals, double startDistance, AntennaObjectAndEnvironmentParameters objectAndEnvironmentParameters, AntennaDistanceSensorParameters distanceSensorParameters)
        {
            var result = new List<(double, double)>();
            var random = new Random();
            var parameters = new List<double>();
            var k = 0;
            List<double> probingSignal = new List<double>();

            do
            {
                parameters.Add(random.NextDouble() * 50 + 1);
                k++;
            } while (k < howManyBasicSignals);

            for (var i = distanceSensorParameters.ReportingPeriodOfDistance; i < distanceSensorParameters.ReportingPeriodOfDistance * 12; i += distanceSensorParameters.ReportingPeriodOfDistance)
            {
                var signal = SignalGenerator.SinusForAntenna(
                    parameters[0],
                    distanceSensorParameters.PeriodOfTheProbeSignal,
                    i,
                    50,
                    distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal
                );
                for (var j = 1; j < parameters.Count; j++)
                {
                    RealSignalHelpers.AddSignals(
                        signal,
                        SignalGenerator.SinusForAntenna(
                            parameters[j],
                            distanceSensorParameters.PeriodOfTheProbeSignal,
                            i,
                            50,
                            distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal
                        ),
                        out signal);
                }

                probingSignal = probingSignal.Concat(signal.Points).ToList();
            }

            for (var i = distanceSensorParameters.ReportingPeriodOfDistance; i < distanceSensorParameters.ReportingPeriodOfDistance * 12; i += distanceSensorParameters.ReportingPeriodOfDistance)
            {
                var realDistance = startDistance + i * objectAndEnvironmentParameters.RealSpeedOfTheObject;
                var cos = (realDistance * 2) / objectAndEnvironmentParameters.SpeedOfSignalPropagationInEnvironment;
                var skip = (int)(cos * distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal);

                var response = probingSignal.Skip(skip).Take(distanceSensorParameters.LengthOfBuffersOfDiscreteSignals).ToList();
                var signal = probingSignal.Take(distanceSensorParameters.LengthOfBuffersOfDiscreteSignals).ToList();

                var correlation = OperationsHelper.CorrelationUsingConvolution(response, signal);
                var rightHalf = correlation.Skip(correlation.Count / 2).ToList();
                var maxSample = rightHalf.IndexOf(rightHalf.Max());
                var tDelay = maxSample / distanceSensorParameters.SamplingFrequencyOfTheProbeAndFeedbackSignal;

                var distanceFromRadarToObjectAndFromObjectToRadar =
                    objectAndEnvironmentParameters.SpeedOfSignalPropagationInEnvironment * tDelay;
                var distanceFromRadarToObject = distanceFromRadarToObjectAndFromObjectToRadar / 2;


                result.Add((realDistance, distanceFromRadarToObject));
            }

            return result;
        }
    }
}
