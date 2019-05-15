using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Window
{
    class HammingWindow : IWindow
    {
        public List<double> Create(int M)
        {
            var result = new List<double>();

            for (var i = 0; i < M-1; i++)
            {
                result.Add(0.53836 - (0.46164 * Math.Cos((2 * Math.PI * i) / M)));
            }

            return result;
        }
    }
}
