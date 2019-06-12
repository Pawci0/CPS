using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Filter.Pass
{
    public class HighPass : IPass
    {
        public double CalculateK(double f0, double fp)
        {
            return fp / (fp / 2 - f0);
        }

        public List<double> Generate(int M, double K)
        {
            return new LowPass().Generate(M, K)
                .Select((x, i) => x * Math.Pow(-1, i))
                .ToList();
        }
    }
}