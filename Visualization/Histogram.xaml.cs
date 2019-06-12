using System.Linq;
using Lib;
using LiveCharts;
using LiveCharts.Wpf;

namespace Visualization
{
    public partial class Histogram : SignalPage
    {
        public ColumnSeries Series = new ColumnSeries();

        public Histogram()
        {
            InitializeComponent();
            DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                Series
            };
        }

        public Histogram(RealSignal signal) : this()
        {
            if (signal != null)
            {
                Signal = signal;
                Update(signal, null);
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        public override void Update(RealSignal newSignal, SignalVariables sv, bool connectPoints = false)
        {
            var points = newSignal.ToDrawHistogram(newSignal.Interval);
            Series.Values = new ChartValues<int>(points.Select(x => x.value));
            Labels = points.Select(n => n.begin + ", " + n.end).ToArray();
        }
    }
}