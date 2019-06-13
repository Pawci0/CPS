using System.Collections.Generic;
using System.Diagnostics;
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
            realS = new ScatterSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                MaxPointShapeDiameter = 5,
                Stroke = Brushes.Blue
            };
            firstS = new ScatterSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                MaxPointShapeDiameter = 5,
                Stroke = Brushes.Blue
            };
            secondS = new ScatterSeries
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
                MaxPointShapeDiameter = 5,
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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var output = EnumConverter.ConvertTo(enumValue, newSignal);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Trace.WriteLine("Elapsed Time " + enumValue.ToString() + ": " + elapsedMs);
            realS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(signalPoints));

            if (output is ComplexSignal)
            {
                switchView.IsEnabled = true;
                complex = (output as ComplexSignal); 
                firstS.Values = ToFrequency(complex.ToDrawRealisGraph(), newSignal);
                secondS.Values = ToFrequency(complex.ToDrawImaginarisGraph(), newSignal);
            }
            else if (output is RealSignal)
            {
                switchView.IsEnabled = false;
                var real = (output as RealSignal);
                firstS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(real.ToDrawGraph()));
            }

        }

        public void SwitchView(object sender, RoutedEventArgs e)
        {
            if (_switch) //W1
            {
                firstS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawRealisGraph()));
                secondS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawImaginarisGraph()));
                _switch = false;
            }
            else  //W2
            {
                firstS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawMagnitudeGraph()));
                secondS.Values = new ChartValues<ObservablePoint>(ViewUtils.ToValues(complex?.ToDrawPhaseGraph()));
                _switch = true;
            }
        }
        private ChartValues<ObservablePoint> ToFrequency(List<(double x, double y)> points, RealSignal signal)
        {
            var values = new ChartValues<ObservablePoint>();
            {
                var i = signal.Begin;
                foreach (var p in points)
                {
                    values.Add(new ObservablePoint(i * signal.SamplingFrequency / signal.Points.Count, p.y));
                    i++;
                }
            }

            return values;
        }
    }
}