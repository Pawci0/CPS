using System;
using System.Collections.Generic;

namespace Lib.Task3.WindowFunctions
{
    public class BlackmanWindow : IWindowFunction
    {
        public List<double> Create(int n, int m)
        {
            var result = new List<double>();

            for (var i = 0; i < n; i++)
            {
                result.Add(0.42 - (0.5 * Math.Cos((2 * Math.PI * i) / m)) + (0.08 * Math.Cos((4 * Math.PI * i) / m)));
            }

            return result;
        }
    }
}
