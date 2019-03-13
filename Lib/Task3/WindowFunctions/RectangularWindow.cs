using System.Collections.Generic;
using System.Linq;

namespace Lib.Task3.WindowFunctions
{
    public class RectangularWindow : IWindowFunction
    {
        public List<double> Create(int n, int m)
        {
            return Enumerable.Repeat(1.0, n).ToList();
        }
    }
}
