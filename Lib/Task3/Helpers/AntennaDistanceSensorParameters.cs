namespace Lib.Task3.Helpers
{
    public class AntennaDistanceSensorParameters
    {
        public AntennaDistanceSensorParameters(double periodOfTheProbeSignal, double samplingFrequencyOfTheProbeAndFeedbackSignal, int lengthOfBuffersOfDiscreteSignals, double reportingPeriodOfDistance)
        {
            PeriodOfTheProbeSignal = periodOfTheProbeSignal;
            SamplingFrequencyOfTheProbeAndFeedbackSignal = samplingFrequencyOfTheProbeAndFeedbackSignal;
            LengthOfBuffersOfDiscreteSignals = lengthOfBuffersOfDiscreteSignals;
            ReportingPeriodOfDistance = reportingPeriodOfDistance;
        }

        public double PeriodOfTheProbeSignal { get; }

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; }

        public int LengthOfBuffersOfDiscreteSignals { get; }

        public double ReportingPeriodOfDistance { get; }
    }
}
