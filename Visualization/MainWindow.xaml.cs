using Lib;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections;
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
using LiveCharts.Defaults;

namespace Visualization
{
    public partial class MainWindow : Window
    {
        private RealSignal signal;
        private bool chartSwitch = true;
        public double Amplitude { get; set; }

        public double BeginsAt { get; set; }

        public double Duration { get; set; }

        public double SamplingFrequency { get; set; }

        public double Period { get; set; }

        public int Interval { get; set; }

        public double FillFactor { get; set; }

        public SignalEnum SelectedSignal { get; set; }

        public void toChart(object sender, RoutedEventArgs e)
        {
            chartSwitch = true;
            chart.Content = new Chart(signal);
        }

        public void toHistogram(object sender, RoutedEventArgs e)
        {
            chartSwitch = false;
            chart.Content = new Histogram(signal, Interval);
        }

        public void UpdateGraph(object sender, RoutedEventArgs e)
        {
            signal = EnumToSignalConverter.ConvertTo(SelectedSignal, Amplitude, BeginsAt, Duration, SamplingFrequency, Period, FillFactor);
            if (chartSwitch)
            {
                chart.Content = new Chart(signal);
            }
            else
            {
                chart.Content = new Histogram(signal, Interval);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
       //     chart.Content = new Chart();
            DataContext = this;
        }
    }
}
