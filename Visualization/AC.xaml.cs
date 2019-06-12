using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Lib;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Visualization
{
    /// <summary>
    ///     Logika interakcji dla klasy AC.xaml
    /// </summary>
    public partial class AC : SignalPage
    {
        public AC()
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
            Sampling = new ScatterSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                MaxPointShapeDiameter = 5,
                Stroke = Brushes.Blue
            };
            Quantisation = new StepLineSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                PointGeometrySize = 5,
                Stroke = Brushes.Blue
            };
            SamplingCollection = new SeriesCollection
            {
                OriginalSignal,
                Sampling
            };
            QuantisationCollection = new SeriesCollection
            {
                OriginalSignal2,
                Quantisation
            };
        }

        public AC(RealSignal signal, SignalVariables sv) : this()
        {
            Signal = signal;
            Update(signal, sv);
        }

        public new RealSignal Signal { get; set; }
        public SeriesCollection SamplingCollection { get; set; }
        public SeriesCollection QuantisationCollection { get; set; }
        public Series Sampling { get; set; }
        public Series Quantisation { get; set; }
        public Series OriginalSignal { get; set; }
        public Series OriginalSignal2 { get; set; }

        public override void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints = false)
        {
            if (newSignal == null || Math.Abs(sv.SamplingFrequency % sv.QuantizationFreq) > 0.00001)
                return;
            var level = sv.QuantizationLevel;
            var qvalues = new List<double>();
            qvalues.Add(newSignal.Points.Max<double>());
            qvalues.Add(newSignal.Points.Min<double>());
            var scope = newSignal.Points.Max<double>() - newSignal.Points.Min<double>();
            if (level > 2)
            {
                var stepVal = scope / (level - 1);
                for (var i = 1; i <= level - 2; i++) qvalues.Add(newSignal.Points.Max<double>() - stepVal * i);
            }


            var step = sv.SamplingFrequency / sv.QuantizationFreq;
            var points = new List<ObservablePoint>();
            var realPoints = new List<ObservablePoint>();
            var originalPoints = new List<ObservablePoint>();
            var values = new List<double>();
            for (var i = 0; i < newSignal.Points.Count(); i += (int) step)
            {
                var xy = newSignal.ToDrawGraph()[i];
                var closest = qvalues.Aggregate((x, y) => Math.Abs(x - xy.y) < Math.Abs(y - xy.y) ? x : y);
                points.Add(new ObservablePoint(xy.x, closest));
                realPoints.Add(new ObservablePoint(xy.x, xy.y));
                values.Add(closest);
            }

            foreach (var (x, y) in newSignal.ToDrawGraph()) originalPoints.Add(new ObservablePoint(x, y));
            Signals.quantized = new RealSignal(values);
            Sampling.Values = new ChartValues<ObservablePoint>(realPoints);
            Quantisation.Values = new ChartValues<ObservablePoint>(points);
            OriginalSignal.Values = new ChartValues<ObservablePoint>(originalPoints);
            OriginalSignal2.Values = new ChartValues<ObservablePoint>(originalPoints);
        }
    }
}