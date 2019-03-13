using Lib;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SciChart.Data.Model;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Helper;

namespace WpfApp2.ViewModel
{
    public class ChartViewModel1 : BindableObject
    {
        private string _lineSeriesTitle;
        private string _histogramTitle;
        private string _xAxisLineTitle;
        private string _yAxisLineTitle;
        private string _xAxisHistogramTitle;
        private string _yAxisHistogramTitle;
        private double _param1;
        private double _param2;
        private double _param3;
        private double _param4;
        private double _param5;
        private string _paramName1;
        private string _paramName2;
        private string _paramName3;
        private string _paramName4;
        private string _paramName5;
        private PlotModel _lineSeries;
        private PlotModel _histogramSeries;

        #region Properties

        public double Param1
        {
            get => _param1;
            set
            {
                _param1 = value;
                OnPropertyChanged("Param1");
            }
        }
        public double Param2
        {
            get => _param2;
            set
            {
                _param2 = value;
                OnPropertyChanged("Param2");
            }
        }
        public double Param3
        {
            get => _param3;
            set
            {
                _param3 = value;
                OnPropertyChanged("Param3");
            }
        }
        public double Param4
        {
            get => _param4;
            set
            {
                _param4 = value;
                OnPropertyChanged("Param4");
            }
        }
        public double Param5
        {
            get => _param5;
            set
            {
                _param5 = value;
                OnPropertyChanged("Param5");
            }
        }
        public string ParamName1
        {
            get => _paramName1;
            set
            {
                _paramName1 = value;
                OnPropertyChanged("ParamName1");
            }
        }
        public string ParamName2
        {
            get => _paramName2;
            set
            {
                _paramName2 = value;
                OnPropertyChanged("ParamName2");
            }
        }
        public string ParamName3
        {
            get => _paramName3;
            set
            {
                _paramName3 = value;
                OnPropertyChanged("ParamName3");
            }
        }
        public string ParamName4
        {
            get => _paramName4;
            set
            {
                _paramName4 = value;
                OnPropertyChanged("ParamName4");
            }
        }
        public string ParamName5
        {
            get => _paramName5;
            set
            {
                _paramName5 = value;
                OnPropertyChanged("ParamName5");
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

        public PlotModel LineSeries
        {
            get => _lineSeries;
            set
            {
                _lineSeries = value;
                OnPropertyChanged("LineSeries");
            }
        }

        public PlotModel HistogramSeries
        {
            get => _histogramSeries;
            set
            {
                _histogramSeries = value;
                OnPropertyChanged("HistogramSeries");
            }
        }

        #endregion

        public ChartViewModel1()
        {
            _xAxisLineTitle = "t[s]";
            _yAxisLineTitle = "Amplitude";

            _xAxisHistogramTitle = "t[s]";
            _yAxisHistogramTitle = "Amplitude";
        }

        public void GenerateChart(RealSignal signal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            var histogramData = new ColumnSeries()
            {
                FillColor = OxyColor.FromRgb(38, 152, 191)
            };

            foreach (var (_, y) in signal.ToDrawHistogram(SettingsData.Intervals <= default(int) ? 10 : SettingsData.Intervals))
            {
                histogramData.Items.Add(new ColumnItem(y));
            }

            HistogramSeries.Series.Add(histogramData);
        }

        public void GenerateFilterChart(List<double> points, RealSignal signal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);
            

            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData1 = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191)
            };

            double xData = 0;
            foreach (double y in points)
            {
                lineData1.Points.Add(new DataPoint(xData, y));
                xData++;
            }

            HistogramSeries.Series.Add(lineData1);
        }

        public void GenerateUniformSamplingChart(RealSignal signal, RealSignal convertedSignal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var pointData = new ScatterSeries()
            {
                MarkerFill = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in convertedSignal.ToDrawGraph())
            {
                pointData.Points.Add(new ScatterPoint(x, y));
            }

            HistogramSeries.Series.Add(pointData);
        }

        public void GenerateChart(ComplexSignal complexSignal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };
            if (LineSeriesTitle == "ComplexSignal - Realis")
            {
                var i = complexSignal.BeginsAt;
                foreach (var y in complexSignal.Points.Select(x => x.Real))
                {
                    lineData.Points.Add(new DataPoint(i, y));
                    i += complexSignal.SamplingPeriod;
                }
            }
            else
            {
                var i = 0;
                foreach (var y in complexSignal.Points.Select(x => x.Real))
                {
                    lineData.Points.Add(new DataPoint(i * complexSignal.SamplingFrequency / complexSignal.Points.Count, y));
                    i++;
                }
            }
            

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var pointData = new ScatterSeries()
            {
                MarkerFill = OxyColor.FromRgb(38, 152, 191)
            };
            if (HistogramTitle == "ComplexSignal - Imaginalis")
            {
                var i = complexSignal.BeginsAt;
                foreach (var y in complexSignal.Points.Select(x => x.Imaginary))
                {
                    pointData.Points.Add(new ScatterPoint(i, y));
                    i += complexSignal.SamplingPeriod;
                }
            }
            else
            {
                var i = 0;
                foreach (var y in complexSignal.Points.Select(x => x.Imaginary))
                {
                    pointData.Points.Add(new ScatterPoint(i * complexSignal.SamplingFrequency / complexSignal.Points.Count, y));
                    i++;
                }
            }

            HistogramSeries.Series.Add(pointData);
        }

        public void GenerateUniformQuantizationWithTruncationChart(RealSignal signal, RealSignal convertedSignal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData1 = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in convertedSignal.ToDrawGraph())
            {
                lineData1.Points.Add(new DataPoint(x, y));
            }

            HistogramSeries.Series.Add(lineData1);
        }

        public void GenerateCorrelationChart(List<double> correlation)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };

            var x = 0;
            foreach (var y in correlation)
            {
                lineData.Points.Add(new DataPoint(x, y));
                x++;
            }

            LineSeries.Series.Add(lineData);
        }

        public void GenerateAntennaChart(RealSignal probingSignal, RealSignal feedbackSignal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };

            foreach (var (x, y) in probingSignal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);

            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData1 = new LineSeries()
            {
                Color = OxyColor.FromRgb(255, 255, 255)
            };

            foreach (var (x, y) in feedbackSignal.ToDrawGraph())
            {
                lineData1.Points.Add(new DataPoint(x, y));
            }

            HistogramSeries.Series.Add(lineData1);
        }

        public void GenerateZeroOrderHoldChart(RealSignal signal, RealSignal convertedSignal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };
            

            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData1 = new LineSeries()
            {
                Color = OxyColor.FromRgb(255, 255, 255)
            };


            foreach (var (x, y) in convertedSignal.ToDrawGraph())
            {
                lineData1.Points.Add(new DataPoint(x, y));
            }

            var lineData2 = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData2.Points.Add(new DataPoint(x, y));
            }

            HistogramSeries.Series.Add(lineData1);
            HistogramSeries.Series.Add(lineData2);
        }

        public void GenerateReconstructionBasedOnTheSincFunctionChart(RealSignal signal, RealSignal convertedSignal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };



            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new DataPoint(x, y));
            }

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData1 = new LineSeries()
            {
                Color = OxyColor.FromRgb(255, 255, 255)
            };



            foreach (var (x, y) in convertedSignal.ToDrawGraph())
            {
                lineData1.Points.Add(new DataPoint(x, y));
            }

            var lineData2 = new LineSeries()
            {
                Color = OxyColor.FromRgb(38, 152, 191),
                TextColor = OxyColor.FromRgb(38, 152, 191)
            };



            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData2.Points.Add(new DataPoint(x, y));
            }

            HistogramSeries.Series.Add(lineData1);
            HistogramSeries.Series.Add(lineData2);
        }

        public void GeneratePointChart(RealSignal signal)
        {
            LineSeries = new PlotModel()
            {
                Title = LineSeriesTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            LineSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var lineData = new ScatterSeries()
            {
                MarkerFill = OxyColor.FromRgb(38, 152, 191)
            };


            foreach (var (x, y) in signal.ToDrawGraph())
            {
                lineData.Points.Add(new ScatterPoint(x, y));
            }

            LineSeries.Series.Add(lineData);



            HistogramSeries = new PlotModel()
            {
                Title = HistogramTitle,
                TextColor = OxyColor.FromRgb(151, 144, 149),
                PlotAreaBorderColor = OxyColor.FromRgb(151, 144, 149)
            };

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            HistogramSeries.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColor.FromRgb(151, 144, 149)
            });

            var histogramData = new ColumnSeries()
            {
                FillColor = OxyColor.FromRgb(38, 152, 191)
            };



            foreach (var (_, y) in signal.ToDrawHistogram(SettingsData.Intervals <= default(int) ? 10 : SettingsData.Intervals))
            {
                histogramData.Items.Add(new ColumnItem(y));
            }

            HistogramSeries.Series.Add(histogramData);
        }
    }
}
