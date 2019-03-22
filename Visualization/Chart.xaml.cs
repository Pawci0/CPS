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

        public LineSeries Series = new LineSeries()
        {
            Fill = Brushes.Transparent,
            PointGeometrySize = 5
        };

        public Chart()
        {
            InitializeComponent();
            DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                Series
            };
        }

        public Chart(RealSignal signal) : this()
        {
            Signal = signal;
            var points = new List<ObservablePoint>();
            foreach (var (x, y) in Signal.ToDrawGraph())
            {
                points.Add(new ObservablePoint(x, y));
            }
            Series.Values = new ChartValues<ObservablePoint>(points);
        }
    }
}
