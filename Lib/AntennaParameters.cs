using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class AntennaParameters
    {
        public double PeriodOfTheProbeSignal { get; }

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; }

        public int LengthOfBuffersOfDiscreteSignals { get; }

        public double ReportingPeriodOfDistance { get; }

        public double SimulatorTimeUnit { get; }

        public double RealSpeedOfTheObject { get; }

        public double SpeedOfSignalPropagationInEnvironment { get; }

        public AntennaParameters(double periodOfTheProbeSignal, double samplingFrequencyOfTheProbeAndFeedbackSignal, int lengthOfBuffersOfDiscreteSignals, double reportingPeriodOfDistance, double simulatorTimeUnit, double realSpeedOfTheObject, double speedOfSignalPropagationInEnvironment)
        {
            PeriodOfTheProbeSignal = periodOfTheProbeSignal;
            SamplingFrequencyOfTheProbeAndFeedbackSignal = samplingFrequencyOfTheProbeAndFeedbackSignal;
            LengthOfBuffersOfDiscreteSignals = lengthOfBuffersOfDiscreteSignals;
            ReportingPeriodOfDistance = reportingPeriodOfDistance;
            SimulatorTimeUnit = simulatorTimeUnit;
            RealSpeedOfTheObject = realSpeedOfTheObject;
            SpeedOfSignalPropagationInEnvironment = speedOfSignalPropagationInEnvironment;
        }
    }
}
