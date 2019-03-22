using Lib;
using System;
using System.Collections.Generic;
using Visualization;

namespace Visualization
{
    public class EnumToSignalConverter
    {
        public static RealSignal ConvertTo(SignalEnum value, double amplitude, double beginsAt, double duration, double samplingFrequency, double period=1, double fillFactor=1)
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
                    return SignalGenerator.Rectangular(amplitude, period, beginsAt, duration, fillFactor, samplingFrequency);
                case SignalEnum.SymmetricalRectangular:
                    return SignalGenerator.SymmetricalRectangular(amplitude, period, beginsAt, duration, fillFactor, samplingFrequency);
                case SignalEnum.Triangular:
                    return SignalGenerator.Triangular(amplitude, period, beginsAt, duration, fillFactor, samplingFrequency);
                default:
                    return null;
            }
        }
    }
}