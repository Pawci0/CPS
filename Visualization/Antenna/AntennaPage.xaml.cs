using System.Windows.Controls;
using System.Windows.Media;
using Lib;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Visualization
{
    /// <summary>
    ///     Interaction logic for Antennas.xaml
    /// </summary>
    public partial class AntennaPage : Page
    {
        public AntennaPage()
        {
        }

        public AntennaPage(RealSignal fb, RealSignal prb, RealSignal cnv)
        {
            InitializeComponent();
            DataContext = this;
            var _feedback = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue,
                Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(fb))
            };
            var _probe = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue,
                Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(prb))
            };
            var _conv = new LineSeries
            {
                Fill = Brushes.Transparent,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue,
                Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(cnv))
            };

            feedback = new SeriesCollection
            {
                _feedback
            };
            probe = new SeriesCollection
            {
                _probe
            };
            conv = new SeriesCollection
            {
                _conv
            };
        }

        public SeriesCollection feedback { get; set; }
        public SeriesCollection probe { get; set; }
        public SeriesCollection conv { get; set; }
    }
}