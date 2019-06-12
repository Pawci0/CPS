using System.Collections.Generic;

namespace Lib.Filter.Window
{
    public interface IWindow
    {
        List<double> Generate(int n, int M);
    }
}