namespace Lib.Antenna
{
    public class AntennaParameters
    {
        public AntennaParameters(double periodOfTheProbeSignal, double samplingFrequencyOfTheProbeAndFeedbackSignal,
            int lengthOfBuffersOfDiscreteSignals, double reportingPeriodOfDistance, double simulatorTimeUnit,
            double realSpeedOfTheObject, double speedOfSignalPropagationInEnvironment, int amountOfMeasuringPoints)
        {
            PeriodOfTheProbeSignal = periodOfTheProbeSignal;
            SamplingFrequencyOfTheProbeAndFeedbackSignal = samplingFrequencyOfTheProbeAndFeedbackSignal;
            LengthOfBuffersOfDiscreteSignals = lengthOfBuffersOfDiscreteSignals;
            ReportingPeriodOfDistance = reportingPeriodOfDistance;
            SimulatorTimeUnit = simulatorTimeUnit;
            RealSpeedOfTheObject = realSpeedOfTheObject;
            SpeedOfSignalPropagationInEnvironment = speedOfSignalPropagationInEnvironment;
            if (amountOfMeasuringPoints > 0)
                AmountOfMeasuringPoints = amountOfMeasuringPoints;
        }

        public double PeriodOfTheProbeSignal { get; }

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; }

        public int LengthOfBuffersOfDiscreteSignals { get; }

        public double ReportingPeriodOfDistance { get; }

        public double SimulatorTimeUnit { get; }

        public double RealSpeedOfTheObject { get; }

        public double SpeedOfSignalPropagationInEnvironment { get; }

        public int AmountOfMeasuringPoints { get; }
    }
}