using System.Collections.Generic;

namespace Lib.Task3.Helpers
{
    public static class OperationsHelper
    {
        public static List<double> Convolution(List<double> h, List<double> x)
        {
            var result = new List<double>();
            var resultLength = h.Count + x.Count - 1;

            for (var n = 0; n < resultLength; n++)
            {
                var sum = 0d;
                var kmin = (n >= x.Count - 1) ? n - (x.Count - 1) : 0;
                var kmax = (n < h.Count - 1) ? n : h.Count - 1;

                for (var k = kmin; k <= kmax; k++)
                {
                    sum += h[k] * x[n - k];
                }
                result.Add(sum);
            }

            return result;
        }

        public static List<double> Correlation(List<double> h, List<double> x)
        {
            var result = new List<double>();
            var resultLength = h.Count + x.Count - 1;

            for (var n = 0; n < resultLength; n++)
            {
                var sum = 0d;
                var k1Min = (n >= x.Count - 1) ? n - (x.Count - 1) : 0;
                var k1Max = (n < h.Count - 1) ? n : h.Count - 1;
                var k2Min = (n <= x.Count - 1) ? x.Count - 1 - n : 0;

                for (int k1 = k1Min, k2 = k2Min; k1 <= k1Max; k1++, k2++)
                {
                    sum += h[k1] * x[k2];
                }
                result.Add(sum);
            }

            return result;
        }

        public static List<double> CorrelationUsingConvolution(List<double> h, List<double> x)
        {
            h.Reverse();
            return Convolution(h, x);
        }
    }
}
