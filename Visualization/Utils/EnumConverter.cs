using Lib;
using Lib.Filter.Pass;
using Lib.Filter.Window;
using Lib.Fourier;
using System.Collections.Generic;
using System.Linq;

namespace Visualization
{
    public class EnumConverter
    {
        public static RealSignal ConvertTo(SignalEnum value, double amplitude, double beginsAt, double duration,
            double samplingFrequency, double period = 1, double fillFactor = 1, double jump = 1, double probability = 1)
        {
            switch (value)
            {
                case SignalEnum.GaussianNoise:
                    return SignalGenerator.GaussianNoise(amplitude, beginsAt, duration, samplingFrequency);
                case SignalEnum.UniformNoise:
                    return SignalGenerator.UniformNoise(amplitude, beginsAt, duration, samplingFrequency);
                case SignalEnum.Sinus:
                    return SignalGenerator.Sinus(amplitude, period, beginsAt, duration, samplingFrequency);
                case SignalEnum.HalfRectifiedSinus:
                    return SignalGenerator.HalfRectifiedSinus(amplitude, period, beginsAt, duration, samplingFrequency);
                case SignalEnum.FullRectifiedSinus:
                    return SignalGenerator.FullRectifiedSinus(amplitude, period, beginsAt, duration, samplingFrequency);
                case SignalEnum.Rectangular:
                    return SignalGenerator.Rectangular(amplitude, period, beginsAt, duration, fillFactor,
                        samplingFrequency);
                case SignalEnum.SymmetricalRectangular:
                    return SignalGenerator.SymmetricalRectangular(amplitude, period, beginsAt, duration, fillFactor,
                        samplingFrequency);
                case SignalEnum.Triangular:
                    return SignalGenerator.Triangular(amplitude, period, beginsAt, duration, fillFactor,
                        samplingFrequency);
                case SignalEnum.HeavisideStep:
                    return SignalGenerator.HeavisideStep(amplitude, beginsAt, duration, samplingFrequency, jump);
                case SignalEnum.KroneckerDelta:
                    return SignalGenerator.KroneckerDelta(amplitude, beginsAt, duration, samplingFrequency, jump);
                case SignalEnum.ImpulsiveNoise:
                    return SignalGenerator.ImpulsiveNoise(amplitude, beginsAt, duration, samplingFrequency,
                        probability);
                case SignalEnum.S1Signal:
                    return SignalGenerator.S1Signal(amplitude, beginsAt, duration, samplingFrequency, probability);
                default:
                    return null;
            }
        }

        public static RealSignal Operation(OperationEnum value, RealSignal one, RealSignal two)
        {
            switch (value)
            {
                case OperationEnum.Add:
                    return SignalOperations.AddSignals(one, two);
                case OperationEnum.Subtract:
                    return SignalOperations.SubtractSignals(one, two);
                case OperationEnum.Multiply:
                    return SignalOperations.MultiplySignals(one, two);
                case OperationEnum.Divide:
                    return SignalOperations.DivideSignals(one, two);
                case OperationEnum.Convolution:
                    return SignalOperations.Convolution(one, two);
                case OperationEnum.Correlation:
                    return new RealSignal(SignalOperations.Correlation(one, two));
                case OperationEnum.CorrelationUsingConvolution:
                    return new RealSignal(SignalOperations.CorrelationUsingConvolution(one, two));
                case OperationEnum.None:
                default:
                    return one;
            }
        }

        public static IWindow ConvertTo(WindowEnum value)
        {
            switch (value)
            {
                case WindowEnum.BlackmanWindow:
                    return new BlackmanWindow();
                case WindowEnum.HammingWindow:
                    return new HammingWindow();
                case WindowEnum.HanningWindow:
                    return new HanningWindow();
                case WindowEnum.RectangularWindow:
                    return new RectangularWindow();
                default:
                    return null;
            }
        }

        public static IPass ConvertTo(PassEnum value)
        {
            switch (value)
            {
                case PassEnum.HighPass:
                    return new HighPass();
                case PassEnum.MidPass:
                    return new MidPass();
                case PassEnum.LowPass:
                    return new LowPass();
                default:
                    return null;
            }
        }

        public static object ConvertTo(TransformationEnum value, RealSignal signal)
        {
            var pointsArray = signal.Points.ToArray();
            switch (value)
            {
                case TransformationEnum.Dct:
                    return new RealSignal(Transforms.Dct(pointsArray).ToList());
                case TransformationEnum.Dft:
                    return new ComplexSignal(signal.Begin,signal.Period, signal.SamplingFrequency, Transforms.Dft(pointsArray).ToList());
                case TransformationEnum.Fct:
                    return new RealSignal(Transforms.Fct(pointsArray).ToList());
                case TransformationEnum.Fft:
                    return new ComplexSignal(signal.Begin, signal.Period, signal.SamplingFrequency, Transforms.Fft(pointsArray).ToList());
                default:
                    return null;
                  
            }
        }
    }
}