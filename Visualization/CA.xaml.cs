using System.Collections.Generic;
using System.Windows.Media;
using Lib;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Visualization
{
    /// <summary>
    ///     Logika interakcji dla klasy CA.xaml
    /// </summary>
    public partial class CA : SignalPage
    {
        public CA()
        {
            InitializeComponent();
            DataContext = this;
            OriginalSignal = new LineSeries
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue
            };
            OriginalSignal2 = new LineSeries
            {
                PointGeometry = null,
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                Stroke = Brushes.LightBlue
            };
            Interpolation = new LineSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                Stroke = Brushes.Blue,
                LineSmoothness = 0
            };
            Sinc = new LineSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                Stroke = Brushes.Blue
            };
            InterpolationCollection = new SeriesCollection
            {
                OriginalSignal,
                Interpolation
            };
            SincCollection = new SeriesCollection
            {
                OriginalSignal2,
                Sinc
            };
        }

        public CA(RealSignal signal, SignalVariables sv) : this()
        {
            Signal = signal;
            Update(signal, sv);
        }

        public new RealSignal Signal { get; set; }
        public SeriesCollection InterpolationCollection { get; set; }
        public SeriesCollection SincCollection { get; set; }
        public Series Interpolation { get; set; }
        public Series Sinc { get; set; }
        public Series OriginalSignal { get; set; }
        public Series OriginalSignal2 { get; set; }

        public override void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints = false)
        {
            //if (newSignal == null || SignalVariables.SamplingFrequency % SignalVariables.RecFreq != 0)
            //    return;
            //var step = SignalVariables.RecFreq;
            var originalPoints = new List<ObservablePoint>();
            var interpolationPoints = new List<ObservablePoint>();
            var sincPoints = new List<ObservablePoint>();

            var signalPoints = newSignal.ToDrawGraph();
            var signal = EnumConverter.ConvertTo(sv.SelectedSignal, sv.Amplitude, newSignal.Begin,
                sv.Duration, sv.RecFreq, sv.Period,
                sv.FillFactor, sv.Jump, sv.Probability);
            List<(double x, double y)> reconstructedPoints =
                ACUtils.SincReconstruction(newSignal, sv.RecFreq, sv.NOfSamples);

            foreach (var (x, y) in signalPoints) originalPoints.Add(new ObservablePoint(x, y));
            foreach (var (x, y) in signal.ToDrawGraph()) interpolationPoints.Add(new ObservablePoint(x, y));
            foreach (var (x, y) in reconstructedPoints) sincPoints.Add(new ObservablePoint(x, y));

            Interpolation.Values = new ChartValues<ObservablePoint>(interpolationPoints);
            Sinc.Values = new ChartValues<ObservablePoint>(sincPoints);
            OriginalSignal.Values = new ChartValues<ObservablePoint>(originalPoints);
            OriginalSignal2.Values = new ChartValues<ObservablePoint>(originalPoints);
        }
    }
}