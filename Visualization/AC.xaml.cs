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
    /// Logika interakcji dla klasy AC.xaml
    /// </summary>
    public partial class AC : SignalPage
    {
        public new RealSignal Signal { get; set; }
        public SeriesCollection SamplingCollection { get; set; }
        public SeriesCollection QuantisationCollection { get; set; }
        public Series Sampling { get; set; }
        public Series Quantisation { get; set; }

        public AC()
        {
            InitializeComponent();
            base.DataContext = this;
            Sampling = new ScatterSeries()
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 5,
                Stroke = Brushes.Blue
            };
            Quantisation = new StepLineSeries()
            {
                Fill = Brushes.Transparent,
                StrokeThickness = 5,
                Stroke = Brushes.Blue
            };
            SamplingCollection = new SeriesCollection
            {
                Sampling
            };
            QuantisationCollection = new SeriesCollection
            {
                Quantisation
            };
        }

        public AC(RealSignal signal) : this()
        {
            Signal = signal;
            Update(signal);
        }

        public override void Update(RealSignal newSignal, bool connectPoints = false)
        {
            if (newSignal == null ||   SignalVariables.SamplingFrequency % SignalVariables.QuantizationFreq != 0)
                return;
            var step = SignalVariables.SamplingFrequency / SignalVariables.QuantizationFreq;
            var points = new List<ObservablePoint>();
            for (int i = 0; i<SignalVariables.SamplingFrequency; i+= (int)step)
            {
                    var xy = newSignal.ToDrawGraph()[i];
                    points.Add(new ObservablePoint(xy.x, xy.y));
                
            }
            Sampling.Values = new ChartValues<ObservablePoint>(points);
            Quantisation.Values = new ChartValues<ObservablePoint>(points);
        }


    }
}
