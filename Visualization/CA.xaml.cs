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
        public Series OriginalSignal { get; set; }
        public Series OriginalSignal2 { get; set; }


        public CA()
        {
            InitializeComponent();
            base.DataContext = this;
            OriginalSignal = new LineSeries()
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue,
            };
            OriginalSignal2 = new LineSeries()
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue,
            };
            Interpolation = new LineSeries()
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                Stroke = Brushes.Blue,
                LineSmoothness = 0
            };
            Sinc = new LineSeries()
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                Stroke = Brushes.Blue
            };
            InterpolationCollection = new SeriesCollection
            {
                Interpolation,
                OriginalSignal
            };
            SincCollection = new SeriesCollection
            {
                Sinc,
                OriginalSignal2
        };
        }

        public CA(RealSignal signal) : this()
        {
            Signal = signal;
            Update(signal);
        }

        public override void Update(RealSignal newSignal, bool connectPoints = false)
        {
            //if (newSignal == null || SignalVariables.SamplingFrequency % SignalVariables.RecFreq != 0)
            //    return;
            //var step = SignalVariables.RecFreq;
            var originalPoints = new List<ObservablePoint>();
            var interpolationPoints = new List<ObservablePoint>();
            var sincPoints = new List<ObservablePoint>();

            List<(double x, double y)> signalPoints = newSignal.ToDrawGraph();
            List<(double x, double y)> reconstructedPoints = ACUtils.SincReconstruction(newSignal, SignalVariables.RecFreq);

            foreach(var (x, y) in signalPoints)
            {
                originalPoints.Add(new ObservablePoint(x, y));
                interpolationPoints.Add(new ObservablePoint(x, y));
            }

            foreach (var (x, y) in reconstructedPoints)
            {
                sincPoints.Add(new ObservablePoint(x, y));
            }

            Interpolation.Values = new ChartValues<ObservablePoint>(interpolationPoints);
            Sinc.Values = new ChartValues<ObservablePoint>(sincPoints);
            OriginalSignal.Values = new ChartValues<ObservablePoint>(originalPoints);
            OriginalSignal2.Values = new ChartValues<ObservablePoint>(originalPoints);
        }


    }
}
