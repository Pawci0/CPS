using Lib;
using System.Windows;

namespace Visualization
{
    public partial class MainWindow : Window
    {
        private RealSignal signalOne;
        private bool chartSwitch = true;

        public MainWindow()
        {
            InitializeComponent();
            signalOneVariables.Content = new SignalVariables();
            signalTwoVariables.Content = new SignalVariables();
            chart.Content = new Chart();
            DataContext = this;
        }

        public void toChart(object sender, RoutedEventArgs e)
        {
            chartSwitch = true;
            chart.Content = new Chart(signalOne);
        }

        public void toHistogram(object sender, RoutedEventArgs e)
        {
            var s = (signalOneVariables.Content as SignalVariables);
            chartSwitch = false;
            chart.Content = new Histogram(signalOne, s.Interval);
        }

        public void UpdateGraph(object sender, RoutedEventArgs e)
        {
            var s = (signalOneVariables.Content as SignalVariables);
            signalOne = EnumToSignalConverter.ConvertTo(s.SelectedSignal, s.Amplitude, s.BeginsAt, s.Duration, s.SamplingFrequency, s.Period, s.FillFactor);
            if (chartSwitch)
            {
                chart.Content = new Chart(signalOne);
            }
            else
            {
                chart.Content = new Histogram(signalOne, s.Interval);
            }
        }

    }
}
