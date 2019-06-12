using System.Linq;
using System.Windows.Media;
using Lib;
using Lib.Filter;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Visualization
{
    /// <summary>
    ///     Logika interakcji dla klasy FilterPage.xaml
    /// </summary>
    public partial class FilterPage : SignalPage
    {
        public FilterPage()
        {
            InitializeComponent();
            DataContext = this;
            Signal = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };
            Filter = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };
            Result = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };

            SignalCollection = new SeriesCollection
            {
                Signal
            };
            FilterCollection = new SeriesCollection
            {
                Filter
            };
            ResultCollection = new SeriesCollection
            {
                Result
            };
        }

        public SeriesCollection SignalCollection { get; set; }
        public SeriesCollection FilterCollection { get; set; }
        public SeriesCollection ResultCollection { get; set; }

        public Series Signal { get; set; }
        public Series Filter { get; set; }
        public Series Result { get; set; }

        public override void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints = false)
        {
        }

        public void Update(RealSignal newSignal, Filter newFilter)
        {
            var filterOutput = newFilter.GenerateOutput();

            var filterPoints = filterOutput.Select((value, i) => ((double) i, value))
                .ToList();

            var signalPoints = newSignal.ToDrawGraph();

            var resultPoints = SignalOperations.Convolution(newSignal.Points, filterOutput)
                .Select((value, i) => ((double) i, value))
                .ToList();

            Signal.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(signalPoints));
            Filter.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(filterPoints));
            Result.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(resultPoints));
        }

        public void UpdateSignal(RealSignal signal)
        {
            Signal.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(signal.ToDrawGraph()));
        }
    }
}