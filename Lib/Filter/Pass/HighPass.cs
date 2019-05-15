using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Pass
{
    class HighPass : IPass
    {
        public List<double> Generate(int M, double K)
        {
            return new LowPass().Generate(M, K)
                                .Select((x, i) => x * Math.Sin(Math.PI * i / 2.0))
                                .ToList();
        }
    }
}
