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
        public double Amplitude { get; set; }

        public double BeginsAt { get; set; }

        public double Duration { get; set; }

        public double SamplingFrequency { get; set; }

        LineSeries Series = new LineSeries();

        public SignalEnum SelectedSignal { get; set; }

        public void UpdateGraph(object sender, RoutedEventArgs e)
        {

            var points = new List<ObservablePoint>();
            foreach (var (x,y) in (EnumToSignalConverter.ConvertTo(SelectedSignal, Amplitude, BeginsAt, Duration, SamplingFrequency).ToDrawGraph()))
            {
                points.Add(new ObservablePoint(x, y));
            }
            Series.Values = new ChartValues<ObservablePoint>(points);
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
    
    }
}
