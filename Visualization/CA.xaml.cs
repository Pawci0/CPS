using Lib;
using LiveCharts;
using LiveCharts.Defaults;
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
    /// <summary>
    /// Logika interakcji dla klasy CA.xaml
    /// </summary>
    public partial class CA : SignalPage
    {
        public new RealSignal Signal { get; set; }
        public SeriesCollection InterpolationCollection { get; set; }
        public SeriesCollection SincCollection { get; set; }
        public Series Interpolation { get; set; }
        public Series Sinc { get; set; }
        public CA()
        {
            InitializeComponent();
            base.DataContext = this;
            Interpolation = new LineSeries()
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 5,
                Stroke = Brushes.Blue,
                LineSmoothness = 0
            };
            Sinc = new LineSeries()
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 5,
                Stroke = Brushes.Blue
            };
            InterpolationCollection = new SeriesCollection
            {
                Interpolation
            };
            SincCollection = new SeriesCollection
            {
                Sinc
            };
        }

        public CA(RealSignal signal) : this()
        {
            Signal = signal;
            Update(signal);
        }

        public override void Update(RealSignal newSignal, bool connectPoints = false)
        {
            var interpolationPoints = new List<ObservablePoint>();
            var sincPoints = new List<ObservablePoint>();

            List<(double x, double y)> signalPoints = newSignal.ToDrawGraph();
            List<double> reconstructedPoints = ACUtils.SincReconstruction(newSignal);

            for(int i=0; i < signalPoints.Count; i++)
            {
                var x = signalPoints[i].x;
                var yInt = signalPoints[i].y;
                var ySinc = reconstructedPoints[i];
                interpolationPoints.Add(new ObservablePoint(x, yInt));
                sincPoints.Add(new ObservablePoint(x, ySinc));
            }

            Interpolation.Values = new ChartValues<ObservablePoint>(interpolationPoints);
            Sinc.Values = new ChartValues<ObservablePoint>(sincPoints);
        }


    }
}
