using Lib;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using System.Windows.Controls;

namespace Visualization
{
    public partial class Histogram : Page
    {
        public RealSignal Signal { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; }

        public ColumnSeries Series = new ColumnSeries()
        {
        };

        public Histogram()
        {
            InitializeComponent();
            DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                Series
            };
        }

        public Histogram(RealSignal signal, int intervals) : this()
        {
            Signal = signal;
            var points = Signal.ToDrawHistogram(intervals);
            Series.Values = new ChartValues<int>(points.Select(x => x.value));
            Labels = points.Select(n => n.begin + ", " + n.end).ToArray();
        }
    }
}
