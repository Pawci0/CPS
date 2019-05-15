using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visualization
{
    /// <summary>
    /// Logika interakcji dla klasy signalVariables.xaml
    /// </summary>
    public partial class AntennaVariables : Page
    {
        public double PeriodOfTheProbeSignal { get; }

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; }

        public int LengthOfBuffersOfDiscreteSignals { get; }

        public double ReportingPeriodOfDistance { get; }

        public double SimulatorTimeUnit { get; }

        public double RealSpeedOfTheObject { get; }

        public double SpeedOfSignalPropagationInEnvironment { get; }

        public AntennaVariables()
        {
            DataContext = this;
    
        }

    }
}
