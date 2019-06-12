using System;
using System.Collections.Generic;

namespace Lib.Filter.Pass
{
    public class LowPass : IPass
    {
        public double CalculateK(double f0, double fp)
        {
            return fp / f0;
        }

        public List<double> Generate(int M, double K)
        {
            var result = new List<double>();
            var center = (M - 1) / 2;

            for (var i = 0; i < M; i++)
            {
                double value;
                if (i == center)
                    value = 2.0 / K;
                else
                    value = Math.Sin(2 * Math.PI * (i - center) / K) / (Math.PI * (i - center));
                result.Add(value);
            }

            return result;
        }
    }
}