namespace Lib
{
    public class ConvertedSignalModel
    {
        public RealSignal Signal { get; set; }

        public double MeanSquaredError { get; set; }
        public double SignalToNoiseRatio { get; set; }
        public double PeakSignalToNoiseRatio { get; set; }
        public double MaximumDifference { get; set; }
        public double EffectiveNumberOfBits { get; set; }
    }
}
