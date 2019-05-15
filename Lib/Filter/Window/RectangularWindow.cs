using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Window
{
    public class RectangularWindow : IWindow
    {
        public List<double> Generate(int n, int M)
        {
            return Enumerable.Repeat(1.0, n).ToList();
        }
    }
}
