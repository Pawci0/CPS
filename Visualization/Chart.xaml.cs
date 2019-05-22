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
    public partial class Chart : SignalPage
    {
        public new RealSignal Signal { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        public Series Series = new LineSeries();

        public Chart()
        {
            InitializeComponent();
            base.DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                Series
            };
        }

        public Chart(RealSignal signal, bool connectPoints) : this()
        {
            Signal = signal;
            Update(signal, null, connectPoints);
        }

        public override void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints)
        {
            if (newSignal == null)
                return;;
            TruncateSeries();
            if (connectPoints)
            {
                Series = new LineSeries()
                {
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 5,
                    Stroke = Brushes.Blue
                };
            }
            else
            {
                Series = new ScatterSeries()
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 5,
                    Stroke = Brushes.Blue,
                    MaxPointShapeDiameter = 5
                };
            }
            var points = new List<ObservablePoint>();
            foreach (var (x, y) in newSignal.ToDrawGraph())
            {
                points.Add(new ObservablePoint(x, y));
            }
            Series.Values = new ChartValues<ObservablePoint>(points);
            SeriesCollection.Add(Series);
        }

        private void TruncateSeries()
        {
            try
            {
                SeriesCollection.Remove(Series);
            }
            catch (Exception e) { }

        }
    }
}
