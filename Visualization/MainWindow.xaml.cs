using Lib;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Visualization
{
    public partial class MainWindow : Window
    {
        private RealSignal Signal { get; set; }
        private bool chartSwitch = true;
        private int Interval { get; set; }
        public OperationEnum SelectedOperation { get; set; }

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
            chart.Content = new Chart(Signal);
        }

        public void toHistogram(object sender, RoutedEventArgs e)
        {
            chartSwitch = false;
            chart.Content = new Histogram(Signal, Interval);
        }

        public void UpdateGraph(object sender, RoutedEventArgs e)
        {
            if (PrepateSignal())
            {
                if (chartSwitch)
                {
                    chart.Content = new Chart(Signal);
                }
                else
                {
                    chart.Content = new Histogram(Signal, Interval);
                }
            }
        }

        private bool PrepateSignal()
        {
            var s1 = (signalOneVariables.Content as SignalVariables);
            var s2 = (signalTwoVariables.Content as SignalVariables);
            try
            {
                Signal = s1.GetSignal();
                if (s2.IsValid())
                {
                    Signal = EnumConverter.Operation(SelectedOperation, Signal, s2.GetSignal());
                }
                Interval = s1.Interval;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
