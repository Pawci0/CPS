using System.Collections.Generic;
using System.Linq;

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