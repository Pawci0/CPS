using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Window
{
    class HanningWindow : IWindow
    {
        public List<double> Create(int M)
        {
            var result = new List<double>();

            for (var i = 0; i < M-1; i++)
            {
                result.Add(0.5 - (0.5 * Math.Cos((2 * Math.PI * i) / M)));
            }

            return result;
        }
    }
}
