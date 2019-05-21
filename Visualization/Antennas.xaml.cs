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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for Antennas.xaml
    /// </summary>
    public partial class Antennas : Page
    {
        public SeriesCollection feedback { get; set; }
        public SeriesCollection probe { get; set; }
        public SeriesCollection conv { get; set; }

        public Antennas(RealSignal fb,RealSignal prb,RealSignal cnv)
        {
            InitializeComponent();
            var Original1 = new LineSeries()
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue,
            };
            var Original2 = new LineSeries()
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue,
            };
            var Original3 = new LineSeries()
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue,
            };
            Original1.Values = new ChartValues<ObservablePoint>(toValues(fb));
            Original2.Values = new ChartValues<ObservablePoint>(toValues(prb));
            Original3.Values = new ChartValues<ObservablePoint>(toValues(cnv));
            feedback = new SeriesCollection
            {
                Original1
            };
            probe = new SeriesCollection
            {
                Original2
            };
            conv = new SeriesCollection
            {
                Original3
            };

        }

        private static IEnumerable<ObservablePoint> toValues(RealSignal signal)
        {
            var result = new List<ObservablePoint>();
            foreach (var (x, y) in signal.ToDrawGraph())
            {
                result.Add(new ObservablePoint(x, y));
            }

            return result;
        }
    }
}
