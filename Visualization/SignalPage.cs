using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Visualization
{
    public abstract class SignalPage : Page
    {
        public RealSignal Signal { get; set; }
        public abstract void Update(RealSignal newSignal, bool connectPoints=false);

    }
}
