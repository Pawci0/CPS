namespace Lib
{
    public class AcModel
    {
        public AcModel(RealSignal signal, double samplingFrequency, int numberOfLevels, int numberOfIncludedSamples)
        {
            OriginalSignal = signal;

            var convertedSignal = AnalogAndDigitalConverter.UniformSampling(OriginalSignal, samplingFrequency);
            UniformSampling = new ConvertedSignalModel
            {
                Signal = convertedSignal,
                MeanSquaredError = AnalogAndDigitalConverter.MeanSquaredError(OriginalSignal, convertedSignal),
                SignalToNoiseRatio = AnalogAndDigitalConverter.SignalToNoiseRatio(OriginalSignal, convertedSignal),
                PeakSignalToNoiseRatio =
                    AnalogAndDigitalConverter.PeakSignalToNoiseRatio(OriginalSignal, convertedSignal),
                MaximumDifference = AnalogAndDigitalConverter.MaximumDifference(OriginalSignal, convertedSignal),
                EffectiveNumberOfBits = AnalogAndDigitalConverter.EffectiveNumberOfBits(OriginalSignal, convertedSignal)
            };

            convertedSignal =
                AnalogAndDigitalConverter.UniformQuantizationWithTruncation(UniformSampling.Signal, numberOfLevels);
            UniformQuantizationWithTruncation = new ConvertedSignalModel
            {
                Signal = convertedSignal,
                MeanSquaredError = AnalogAndDigitalConverter.MeanSquaredError(OriginalSignal, convertedSignal),
                SignalToNoiseRatio = AnalogAndDigitalConverter.SignalToNoiseRatio(OriginalSignal, convertedSignal),
                PeakSignalToNoiseRatio =
                    AnalogAndDigitalConverter.PeakSignalToNoiseRatio(OriginalSignal, convertedSignal),
                MaximumDifference = AnalogAndDigitalConverter.MaximumDifference(OriginalSignal, convertedSignal),
                EffectiveNumberOfBits = AnalogAndDigitalConverter.EffectiveNumberOfBits(OriginalSignal, convertedSignal)
            };

            convertedSignal = AnalogAndDigitalConverter.ZeroOrderHold(UniformSampling.Signal);
            ZeroOrderHold = new ConvertedSignalModel
            {
                Signal = convertedSignal,
                MeanSquaredError =
                    AnalogAndDigitalConverter.MeanSquaredError(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                SignalToNoiseRatio =
                    AnalogAndDigitalConverter.SignalToNoiseRatio(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                PeakSignalToNoiseRatio =
                    AnalogAndDigitalConverter.PeakSignalToNoiseRatio(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                MaximumDifference =
                    AnalogAndDigitalConverter.MaximumDifference(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                EffectiveNumberOfBits =
                    AnalogAndDigitalConverter.EffectiveNumberOfBits(UniformQuantizationWithTruncation.Signal,
                        convertedSignal)
            };

            convertedSignal =
                AnalogAndDigitalConverter.ReconstructionBasedOnTheSincFunction(UniformSampling.Signal,
                    numberOfIncludedSamples);
            ReconstructionBasedOnTheSincFunction = new ConvertedSignalModel
            {
                Signal = convertedSignal,
                MeanSquaredError =
                    AnalogAndDigitalConverter.MeanSquaredError(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                SignalToNoiseRatio =
                    AnalogAndDigitalConverter.SignalToNoiseRatio(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                PeakSignalToNoiseRatio =
                    AnalogAndDigitalConverter.PeakSignalToNoiseRatio(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                MaximumDifference =
                    AnalogAndDigitalConverter.MaximumDifference(UniformQuantizationWithTruncation.Signal,
                        convertedSignal),
                EffectiveNumberOfBits =
                    AnalogAndDigitalConverter.EffectiveNumberOfBits(UniformQuantizationWithTruncation.Signal,
                        convertedSignal)
            };
        }

        public RealSignal OriginalSignal { get; }
        public ConvertedSignalModel UniformSampling { get; }
        public ConvertedSignalModel UniformQuantizationWithTruncation { get; }
        public ConvertedSignalModel ZeroOrderHold { get; }
        public ConvertedSignalModel ReconstructionBasedOnTheSincFunction { get; }
    }
}