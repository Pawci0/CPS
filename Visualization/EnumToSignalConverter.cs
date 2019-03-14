using Lib;
using System;
using System.Collections.Generic;
using Visualization;

namespace Visualization
{
    public class EnumToSignalConverter
    {
        public static IEnumerable<double> ConvertTo(SignalEnum value, double amplitude, double beginsAt, double duration, double samplingFrequency)
        {
            switch (value)
            {
                case SignalEnum.GaussianNoise:
                    return SignalGenerator.GaussianNoise(amplitude, beginsAt, duration, samplingFrequency).Points;
                case SignalEnum.UniformNoise:
                    return SignalGenerator.UniformNoise(amplitude, beginsAt, duration, samplingFrequency).Points;
                case SignalEnum.Sinus:
                    return SignalGenerator.Sinus(amplitude, 1,beginsAt, duration, samplingFrequency).Points;
                default:
                    return new List<double>();

            }
        }
    }
}