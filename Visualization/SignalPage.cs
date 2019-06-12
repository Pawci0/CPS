using System.Windows.Controls;
using Lib;

namespace Visualization
{
    public abstract class SignalPage : Page
    {
        public RealSignal Signal { get; set; }
        public abstract void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints = false);
    }
}