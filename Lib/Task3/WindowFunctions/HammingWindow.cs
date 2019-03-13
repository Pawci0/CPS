using System;
using System.Collections.Generic;

namespace Lib.Task3.WindowFunctions
{
    public class HammingWindow : IWindowFunction
    {
        public List<double> Create(int n, int m)
        {
            var result = new List<double>();

            for (var i = 0; i < n; i++)
            {
                result.Add(0.53836 - (0.46164 * Math.Cos((2 * Math.PI * i) / m)));
            }

            return result;
        }
    }
}
