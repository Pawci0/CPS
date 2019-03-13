using Lib;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using System.Collections.ObjectModel;
using System.Windows.Media;
using WpfApp2.Helper;

namespace WpfApp2.ViewModel
{
    public class ChartViewModel : BindableObject
    {
        private string _lineSeriesTitle;
        private string _histogramTitle;
        private string _xAxisLineTitle;
        private string _yAxisLineTitle;
        private string _xAxisHistogramTitle;
        private string _yAxisHistogramTitle;
        private DoubleRange _xAxisLineRange;
        private DoubleRange _yAxisLineRange;
        private DoubleRange _xAxisHistogramRange;
        private DoubleRange _yAxisHistogramRange;
        private bool _enablePan;
        private bool _enableZoom = true;
        private ObservableCollection<IRenderableSeriesViewModel> _lineSeries;
        private ObservableCollection<IRenderableSeriesViewModel> _histogram;
        private double _averageValue;
        private double _absoluteAverageValue;
        private double _rootMeanSquare;
        private double _variance;
        private double _averagePower;

        #region Properties

        public double AverageValue
        {
            get => _averageValue;
            set
            {
                _averageValue = value;
                OnPropertyChanged("AverageValue");
            }
        }
        public double AbsoluteAverageValue
        {
            get => _absoluteAverageValue;
            set
            {
                _absoluteAverageValue = value;
                OnPropertyChanged("AbsoluteAverageValue");
            }
        }
        public double RootMeanSquare
        {
            get => _rootMeanSquare;
            set
            {
                _rootMeanSquare = value;
                OnPropertyChanged("RootMeanSquare");
            }
        }
        public double Variance
        {
            get => _variance;
            set
            {
                _variance = value;
                OnPropertyChanged("Variance");
            }
        }
        public double AveragePower
        {
            get => _averagePower;
            set
            {
                _averagePower = value;
                OnPropertyChanged("AveragePower");
            }
        }
        public bool EnablePan
        {
            get => _enablePan;
            set
            {
                if (_enablePan != value)
                {
                    _enablePan = value;
                    OnPropertyChanged("EnablePan");
                    if (_enablePan) EnableZoom = false;
                }
            }
        }

        public bool EnableZoom
        {
            get => _enableZoom;
            set
            {
                if (_enableZoom != value)
                {
                    _enableZoom = value;
                    OnPropertyChanged("EnableZoom");
                    if (_enableZoom) EnablePan = false;
                }
            }
        }

        public ObservableCollection<IRenderableSeriesViewModel> LineSeries
        {
            get => _lineSeries;
            set
            {
                _lineSeries = value;
                OnPropertyChanged("LineSeries");
            }
        }

        public ObservableCollection<IRenderableSeriesViewModel> Histogram
        {
            get => _histogram;
            set
            {
                _histogram = value;
                OnPropertyChanged("Histogram");
            }
        }

        public string LineSeriesTitle
        {
            get => _lineSeriesTitle;
            set
            {
                _lineSeriesTitle = value;
                OnPropertyChanged("LineSeriesTitle");
            }
        }
        public string HistogramTitle
        {
            get => _histogramTitle;
            set
            {
                _histogramTitle = value;
                OnPropertyChanged("HistogramTitle");
            }
        }
        public DoubleRange YAxisLineRange
        {
            get => _yAxisLineRange;
            set
            {
                _yAxisLineRange = value;
                OnPropertyChanged("YAxisLineRange");
            }
        }
        public DoubleRange XAxisLineRange
        {
            get => _xAxisLineRange;
            set
            {
                _xAxisLineRange = value;
                OnPropertyChanged("XAxisLineRange");
            }
        }
        public DoubleRange YAxisHistogramRange
        {
            get => _yAxisHistogramRange;
            set
            {
                _yAxisHistogramRange = value;
                OnPropertyChanged("YAxisHistogramRange");
            }
        }
        public DoubleRange XAxisHistogramRange
        {
            get => _xAxisHistogramRange;
            set
            {
                _xAxisHistogramRange = value;
                OnPropertyChanged("XAxisHistogramRange");
            }
        }
        public string XAxisLineTitle
        {
            get => _xAxisLineTitle;
            set
            {
                _xAxisLineTitle = value;
                OnPropertyChanged("XAxisLineTitle");
            }
        }
        public string YAxisLineTitle
        {
            get => _yAxisLineTitle;
            set
            {
                _yAxisLineTitle = value;
                OnPropertyChanged("YAxisLineTitle");
            }
        }
        public string XAxisHistogramTitle
        {
            get => _xAxisHistogramTitle;
            set
            {
                _xAxisHistogramTitle = value;
                OnPropertyChanged("XAxisHistogramTitle");
            }
        }
        public string YAxisHistogramTitle
        {
            get => _yAxisHistogramTitle;
            set
            {
                _yAxisHistogramTitle = value;
                OnPropertyChanged("YAxisHistogramTitle");
            }
        }

        #endregion

        public ChartViewModel()
        {
            _xAxisLineRange = new DoubleRange(0, 10);
            _yAxisLineRange = new DoubleRange(0, 10);
            _xAxisHistogramRange = new DoubleRange(0, 10);
            _yAxisHistogramRange = new DoubleRange(0, 10);

            _xAxisLineTitle = "t[s]";
            _yAxisLineTitle = "Amplitude";

            _xAxisHistogramTitle = "t[s]";
            _yAxisHistogramTitle = "Amplitude";
        }

        public void GenerateChart(RealSignal signal)
        {
            AverageValue = signal.AverageValue;
            AbsoluteAverageValue = signal.AbsoluteAverageValue;
            RootMeanSquare = signal.RootMeanSquare;
            Variance = signal.Variance;
            AveragePower = signal.AveragePower;

            var lineData = new XyDataSeries<double, double>() { SeriesName = LineSeriesTitle };

            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Append(x, y);
            }

            XAxisLineRange = new DoubleRange(lineData.XMin.ToDouble() - 1, lineData.XMax.ToDouble() + 1);
            YAxisLineRange = new DoubleRange(lineData.YMin.ToDouble() - 1, lineData.YMax.ToDouble() + 1);


            _lineSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            LineSeries.Add(new LineRenderableSeriesViewModel
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = lineData
            });

            var histogramData = new XyDataSeries<double, double>() { SeriesName = HistogramTitle };

            foreach (var (x, y) in signal.ToDrawHistogram(SettingsData.Intervals <= default(int) ? 10 : SettingsData.Intervals))
            {
                histogramData.Append(x, y);
            }

            XAxisHistogramRange = new DoubleRange(histogramData.XMin.ToDouble() - 1, histogramData.XMax.ToDouble() + 1);
            YAxisHistogramRange = new DoubleRange(histogramData.YMin.ToDouble() - 1, histogramData.YMax.ToDouble() + 1);


            _histogram = new ObservableCollection<IRenderableSeriesViewModel>();
            Histogram.Add(new ColumnRenderableSeriesViewModel
            {
                StrokeThickness = 2,
                DataPointWidth = 1,
                Stroke = Colors.DarkBlue,
                Fill = new SolidColorBrush(Colors.SteelBlue),
                DataSeries = histogramData
            });
        }
    }
}
