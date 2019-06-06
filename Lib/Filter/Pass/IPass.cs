using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Pass
{
    public interface IPass
    {
        List<double> Generate(int M, double K);

        double CalculateK(double f0, double fp);
    }
}
