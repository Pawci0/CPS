using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Window
{
    public interface IWindow
    {
        List<double> Create(int M);
    }
}
