using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Visualization.Fourier
{
    public abstract class TransformPage : Page
    {
        public abstract void Update(RealSignal signal);
    }
}
