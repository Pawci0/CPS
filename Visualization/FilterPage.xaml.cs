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
using Lib;
using Lib.Filter;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Visualization
{
    /// <summary>
    /// Logika interakcji dla klasy FilterPage.xaml
    /// </summary>
    public partial class FilterPage : SignalPage
    {
        public SeriesCollection SignalCollection { get; set; }
        public SeriesCollection FilterCollection { get; set; }
        public SeriesCollection ResultCollection { get; set; }

        public Series Signal { get; set; }
        public Series Filter { get; set; }
        public Series Result { get; set; }



        public FilterPage()
        {
            InitializeComponent();
            DataContext = this;
            Signal = new LineSeries()
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue,
            };
            Filter = new LineSeries()
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue,
            };
            Result = new LineSeries()
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue,
            };

            SignalCollection = new SeriesCollection()
            {
                Signal
            };
            FilterCollection = new SeriesCollection()
            {
                Filter
            };
            ResultCollection = new SeriesCollection()
            {
                Result
            };
        }

        public override void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints = false)
        {
        }

        public void Update(RealSignal newSignal, Filter newFilter)
        {
            var filterOutput = newFilter.GenerateOutput();

            var filterPoints = filterOutput.Select((value, i) => ((double)i, value))
                                           .ToList();

            var signalPoints = newSignal.ToDrawGraph();

            var resultPoints = SignalOperations.Convolution(newSignal.Points, filterOutput)
                                               .Select((value, i) => ((double)i, value))
                                               .ToList();

            Signal.Values = toChartValues(signalPoints);
            Filter.Values = toChartValues(filterPoints);
            Result.Values = toChartValues(resultPoints);
        }

        private static ChartValues<ObservablePoint> toChartValues(List<(double x, double y)> list)
        {
            var result = new List<ObservablePoint>();
            foreach (var (x, y) in list)
            {
                result.Add(new ObservablePoint(x, y));
            }

            return new ChartValues<ObservablePoint>(result);
        }
    }
}
