using Lib;
using Lib.Task3;
using Lib.Task3.FilterImpulseResponses;
using Lib.Task3.WindowFunctions;
using Microsoft.Win32;
using SciChart.Data.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class LoadViewModel : BindableObject, IPageViewModel
    {
        private string _title;
        private double _cutOffFrequency;
        private int _m;
        private ChartDetailsEnum _chartDetailsEnum;
        private ObservableCollection<ImpulseResponseModel> _impulseResponses;
        private ImpulseResponseModel _impulseResponse;
        private ObservableCollection<WindowFunctionModel> _windowFunctions;
        private WindowFunctionModel _windowFunction;

        #region Properties

        public PageEnum NameOfPage => PageEnum.LoadPage;
        public ICommand Save { get; }
        public RealSignal Signal { get; set; }
        public ICommand GenerateChart { get; }

        public ObservableCollection<ImpulseResponseModel> ImpulseResponses
        {
            get => _impulseResponses;
            set
            {
                _impulseResponses = value;
                OnPropertyChanged("ImpulseResponses");
            }
        }

        public ImpulseResponseModel ImpulseResponse
        {
            get => _impulseResponse;
            set
            {
                _impulseResponse = value;
                OnPropertyChanged("ImpulseResponse");
            }
        }

        public ObservableCollection<WindowFunctionModel> WindowFunctions
        {
            get => _windowFunctions;
            set
            {
                _windowFunctions = value;
                OnPropertyChanged("WindowFunctions");
            }
        }

        public WindowFunctionModel WindowFunction
        {
            get => _windowFunction;
            set
            {
                _windowFunction = value;
                OnPropertyChanged("WindowFunction");
            }
        }

        public ChartDetailsEnum ChartDetailName
        {
            get => _chartDetailsEnum;
            set
            {
                _chartDetailsEnum = value;

            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public double CutOffFrequency
        {
            get => _cutOffFrequency;
            set
            {
                _cutOffFrequency = value;
                OnPropertyChanged("CutOffFrequency");
            }
        }

        public int M
        {
            get => _m;
            set
            {
                _m = value;
                OnPropertyChanged("M");
            }
        }

        #endregion

        public LoadViewModel()
        {
            GenerateChart = new DelegateCommand(OnGenerateChart);
            Save = new DelegateCommand(OnSave);

            _impulseResponses = new ObservableCollection<ImpulseResponseModel>();
            ImpulseResponses.Add(new ImpulseResponseModel() { Name = "Low pass impulse", ImpulseResponse = new LowPassImpulseResponse() });
            ImpulseResponses.Add(new ImpulseResponseModel() { Name = "Band dass impulse", ImpulseResponse = new BandPassImpulseResponse() });
            ImpulseResponses.Add(new ImpulseResponseModel() { Name = "High pass impulse", ImpulseResponse = new HighPassImpulseResponse() });

            ImpulseResponse = new ImpulseResponseModel() { Name = "Band dass impulse", ImpulseResponse = new BandPassImpulseResponse() };

            _windowFunctions = new ObservableCollection<WindowFunctionModel>();
            WindowFunctions.Add(new WindowFunctionModel() { Name = "Blackman window", WindowFunction = new BlackmanWindow() });
            WindowFunctions.Add(new WindowFunctionModel() { Name = "Hamming window", WindowFunction = new HammingWindow() });
            WindowFunctions.Add(new WindowFunctionModel() { Name = "Hanning window", WindowFunction = new HanningWindow() });
            WindowFunctions.Add(new WindowFunctionModel() { Name = "Rectangular window", WindowFunction = new RectangularWindow() });

            WindowFunction = new WindowFunctionModel() { Name = "Rectangular window", WindowFunction = new RectangularWindow() };
        }

        public void OnGenerateChart()
        {
            var filter = new Filter().ImpulseResponse(ImpulseResponse.ImpulseResponse).WindowFunction(WindowFunction.WindowFunction).FilterOperation(Signal.Points, M, CutOffFrequency, Signal.SamplingFrequency);
            var filter2 = new Filter().ImpulseResponse(ImpulseResponse.ImpulseResponse).WindowFunction(WindowFunction.WindowFunction).FilterOperation2(Signal.Points, M, CutOffFrequency, Signal.SamplingFrequency);

            var window1 = new ChartWindow1();
            var chartViewModel1 = new ChartViewModel1
            {
                LineSeriesTitle = "Original " + Title,
                HistogramTitle = "Filter",
            };

            chartViewModel1.GenerateFilterChart(filter2, Signal);
            window1.DataContext = chartViewModel1;

            var window2 = new ChartWindow1();
            var chartViewModel2 = new ChartViewModel1
            {
                LineSeriesTitle = "Original " + Title,
                HistogramTitle = "Filtered Signal",
            };

            chartViewModel2.GenerateFilterChart(filter, Signal);
            window2.DataContext = chartViewModel2;

            window2.Show();
            window1.Show();
        }

        public void OnSave()
        {
            if (Signal == null)
            {
                MessageBox.Show("First you have to generate chart");
            }
            else
            {
                var saveFileDialog = new SaveFileDialog
                {
                    AddExtension = true,
                    Filter = "fortnite (*.fortnite)|*.fortnite",
                    DefaultExt = "fortnite",
                    FileName = Title
                };
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName.Length == 0)
                {
                    MessageBox.Show("No files selected");
                }
                else
                {
                    Signal.SaveToFile(saveFileDialog.FileName);
                }
            }
        }
    }
}
