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
    public partial class Chart : Page
    {
        public RealSignal Signal { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        public Series Series;

        public Chart()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Chart(RealSignal signal, bool connectPoints) : this()
        {
            Signal = signal;
            if (connectPoints)
            {
                Series = new LineSeries()
                {
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 5
                };
            }
            else
            {
                Series = new ScatterSeries()
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 5
                };
            }
            var points = new List<ObservablePoint>();
            foreach (var (x, y) in Signal.ToDrawGraph())
            {
                points.Add(new ObservablePoint(x, y));
            }
            Series.Values = new ChartValues<ObservablePoint>(points);
            SeriesCollection = new SeriesCollection
            {
                Series
            };
        }
    }
}
