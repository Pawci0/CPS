using Lib;
using LiveCharts;
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
    public partial class MainWindow : Window
    {

        LineSeries Series = new LineSeries();

        public SignalEnum SelectedSignal
        {
            get { return SignalEnum.GaussianNoise; }
            set
            {
                Series.Values = UpdateGraph(value);
            }
        }

        private IChartValues UpdateGraph(SignalEnum value)
        {
            return new ChartValues<double>(EnumToSignalConverter.ConvertTo(value));
            if (value.Equals(SignalEnum.GaussianNoise))
            {
                return new ChartValues<double>(SignalGenerator.GaussianNoise(2, 0, 5, 20).Points);
            }
            else if (value.Equals(SignalEnum.UniformNoise))
            {
                return new ChartValues<double>(SignalGenerator.UniformNoise(2, 0, 5, 20).Points);
            }
            else return new ChartValues<double>();
        }

        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                Series
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    
    }
}
