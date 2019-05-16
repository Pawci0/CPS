using Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static Lib.AntennaParameters;

namespace Visualization
{
    /// <summary>
    /// Logika interakcji dla klasy AntennaVariables.xaml
    /// </summary>
    public partial class AntennaVariables : Page
    {
        public int NumberOfBasicSignals { get; set; } = 5;
        public double PeriodOfTheProbeSignal { get; set; } = 1.0;

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; set; } = 100.0;

        public int LengthOfBuffersOfDiscreteSignals { get; set; } = 500;

        public double ReportingPeriodOfDistance { get; set; } = 2.0;

        public double SimulatorTimeUnit { get; set; } = 10;

        public double RealSpeedOfTheObject { get; set; } = 10;

        public double SpeedOfSignalPropagationInEnvironment { get; set; } = 3000;

        private AntennaParameters parameters { get; set; }

        public AntennaVariables()
        {
            InitializeComponent();
            DataContext = this;


        }

        public void antennaInfo(object sender, RoutedEventArgs e)
        {
            parameters = new AntennaParameters(PeriodOfTheProbeSignal, SamplingFrequencyOfTheProbeAndFeedbackSignal,
                LengthOfBuffersOfDiscreteSignals, ReportingPeriodOfDistance,
                SimulatorTimeUnit, RealSpeedOfTheObject, SpeedOfSignalPropagationInEnvironment);
            var result = Antenna.CalculateAntenna(NumberOfBasicSignals, 0, parameters);
            string s = "";
            foreach (var val in result)
            {
                s += val +"\n";

            }

            MessageBox.Show(s, "Info");
        }
    }

}
