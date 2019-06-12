using System;
using System.Collections.Generic;

namespace Lib.Filter.Window
{
    public class HanningWindow : IWindow
    {
        public List<double> Generate(int n, int M)
        {
            var result = new List<double>();

            for (var i = 0; i < n; i++) result.Add(0.5 - 0.5 * Math.Cos(2 * Math.PI * i / M));

            return result;
        }
    }
}