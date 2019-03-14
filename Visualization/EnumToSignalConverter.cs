using Lib;
using System;
using System.Collections.Generic;
using Visualization;

namespace Visualization
{
    public class EnumToSignalConverter
    {
        public static IEnumerable<double> ConvertTo(SignalEnum value)
        {
            switch (value)
            {
                case SignalEnum.GaussianNoise:
                    return SignalGenerator.GaussianNoise(2, 0, 5, 20).Points;
                case SignalEnum.UniformNoise:
                    return SignalGenerator.UniformNoise(2, 0, 5, 20).Points;
                case SignalEnum.Sinus:
                    return SignalGenerator.Sinus(2, 1, 0, 5, 20).Points;
                default:
                    return new List<double>();

            }
        }
    }
}