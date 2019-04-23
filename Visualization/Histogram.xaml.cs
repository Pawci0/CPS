using Lib;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using System.Windows.Controls;

namespace Visualization
{
    public partial class Histogram : SignalPage
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        public ColumnSeries Series = new ColumnSeries()
        {
        };

        public Histogram()
        {
            InitializeComponent();
            base.DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                Series
            };
        }

        public Histogram(RealSignal signal) : this()
        {
            if (signal != null)
            {
                base.Signal = signal;
                Update(signal);
            }
        }

        public override void Update(RealSignal newSignal, bool connectPoints=false)
        {
            var points = newSignal.ToDrawHistogram(newSignal.Interval);
            Series.Values = new ChartValues<int>(points.Select(x => x.value));
            Labels = points.Select(n => n.begin + ", " + n.end).ToArray();
        }
    }
}
