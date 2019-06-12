using System.Collections.Generic;

namespace Lib.Filter.Pass
{
    public interface IPass
    {
        List<double> Generate(int M, double K);

        double CalculateK(double f0, double fp);
    }
}