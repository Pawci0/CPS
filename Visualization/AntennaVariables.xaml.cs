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
    /// Logika interakcji dla klasy AntennaVariables.xaml
    /// </summary>
    public partial class AntennaVariables : Page
    {
        public double PeriodOfTheProbeSignal { get; set; }

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; set; }

        public int LengthOfBuffersOfDiscreteSignals { get; set; }

        public double ReportingPeriodOfDistance { get; set; }

        public double SimulatorTimeUnit { get; set; }

        public double RealSpeedOfTheObject { get; set; }

        public double SpeedOfSignalPropagationInEnvironment { get; set; }

        public AntennaVariables()
        {
            InitializeComponent();
            DataContext = this;
    
        }

    }
}
