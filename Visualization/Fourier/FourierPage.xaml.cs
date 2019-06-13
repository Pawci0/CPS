using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lib;
using Lib.Filter;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Visualization.Fourier;

namespace Visualization
{
    /// <summary>
    ///     Interaction logic for FourierPage.xaml
    /// </summary>
    public partial class FourierPage : TransformPage
    {
        public SeriesCollection firstChart { get; set; }
        public SeriesCollection secondChart { get; set; }
        public SeriesCollection realSignal { get; set; }

        public string firstTitle { get; set; }
        public string secondTtitle { get; set; }

        public FourierPage()
        {
            firstTitle = "";
            secondTtitle = "";
            InitializeComponent();
            DataContext = this;
            realS = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };
            firstS = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };
            secondS = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };

            firstChart = new SeriesCollection
            {
                firstS
            };
            secondChart = new SeriesCollection
            {
                secondS
            };
            realSignal = new SeriesCollection
            {
                realS
            };
        }

        public Series realS { get; set; }
        public Series firstS { get; set; }
        public Series secondS { get; set; }

        private bool _switch;
        private ComplexSignal complex;

        public override void Update(RealSignal newSignal, TransformationEnum enumValue)
        {
            _switch = false;
            var signalPoints = newSignal.ToDrawGraph();

            var output = EnumConverter.ConvertTo(enumValue, newSignal);
            complex = (output as ComplexSignal);

            firstS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex.ToDrawRealisGraph()));
            secondS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex.ToDrawImaginarisGraph()));

            realS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(signalPoints));
        }

        public void SwitchView(object sender, RoutedEventArgs e)
        {
            if (_switch)
            {
                firstS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawRealisGraph()));
                secondS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawImaginarisGraph()));
                _switch = false;
            }
            else
            {
                firstS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawMagnitudeGraph()));
                secondS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawPhaseGraph()));
                _switch = true;
            }
        }

    }
}