using Lib;
using LiveCharts;
using LiveCharts.Wpf;
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
    public partial class MainWindow : Window
    {
        public double Amplitude { get; set; }

        public double BeginsAt { get; set; }

        public double Duration { get; set; }

        public double SamplingFrequency { get; set; }

        LineSeries Series = new LineSeries();

        public SignalEnum SelectedSignal { get; set; }

        public void UpdateGraph(object sender, RoutedEventArgs e)
        {
            Series.Values = new ChartValues<double>(EnumToSignalConverter.ConvertTo(SelectedSignal, Amplitude, BeginsAt, Duration, SamplingFrequency));
        }

        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                Series
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    
    }
}
