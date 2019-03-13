using System;
using System.Collections.Generic;

namespace Lib.Task3.FilterImpulseResponses
{
    public class LowPassImpulseResponse : IImpulseResponse
    {
        public List<double> Create(int n, int m, double fo, double fp)
        {
            var k = fp / fo;
            var result = new List<double>();

            for (var i = 0; i < n; i++)
            {
                if (i == (m - 1) / 2)
                    result.Add(2.0 / k);
                else
                    result.Add(Math.Sin((2 * Math.PI * (i - ((m - 1) / 2))) / k) / (Math.PI * (i - ((m - 1) / 2))));
            }

            return result;
        }
    }
}
