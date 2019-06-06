using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Pass
{
    public class MidPass : IPass
    {
        public double CalculateK(double f0, double fp)
        {
            return fp / (fp / 4 - f0);
        }

        public List<double> Generate(int M, double K)
        {
            return new LowPass().Generate(M, K)
                                .Select((x, i) => x * Math.Sin(Math.PI * i / 2.0))
                                .ToList();
        }
    }
}
