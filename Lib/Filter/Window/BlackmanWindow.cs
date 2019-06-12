using System;
using System.Collections.Generic;

namespace Lib.Filter.Window
{
    public class BlackmanWindow : IWindow
    {
        public List<double> Generate(int n, int M)
        {
            var result = new List<double>();

            for (var i = 0; i < n; i++)
                result.Add(0.42 - 0.5 * Math.Cos(2 * Math.PI * i / M) + 0.08 * Math.Cos(4 * Math.PI * i / M));

            return result;
        }
    }
}