using Lib;
using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class AntennaWindowViewModel : BindableObject
    {
        private ObservableCollection<double> _calculatedDistance;
        private ObservableCollection<double> _orginalDistance;
        private ObservableCollection<double> _diffreceBetweenDistances;
        private int _chartIndex;
        private List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)> _signalsList;

        #region properties

        public ICommand GenerateChart { get; }

        public int ChartIndex
        {
            get => _chartIndex;
            set
            {
                _chartIndex = value;
                OnPropertyChanged(nameof(ChartIndex));
            }
        }

        public ObservableCollection<double> CalculatedDistance
        {
            get
            {
                if(_calculatedDistance == null)
                {
                    _calculatedDistance = new ObservableCollection<double>();
                }

                return _calculatedDistance;
            }
            set
            {
                _calculatedDistance = value;
                OnPropertyChanged(nameof(CalculatedDistance));
            }
        }

        public ObservableCollection<double> OriginalDistance
        {
            get
            {
                if (_orginalDistance == null)
                {
                    _orginalDistance = new ObservableCollection<double>();
                }

                return _orginalDistance;
            }
            set
            {
                _orginalDistance = value;
                OnPropertyChanged(nameof(OriginalDistance));
            }
        }

        public ObservableCollection<double> DiffrenceBetweenDistances
        {
            get
            {
                if (_diffreceBetweenDistances == null)
                {
                    _diffreceBetweenDistances = new ObservableCollection<double>();
                }

                return _diffreceBetweenDistances;
            }
            set
            {
                _diffreceBetweenDistances = value;
                OnPropertyChanged(nameof(DiffrenceBetweenDistances));
            }
        }

        #endregion

        public AntennaWindowViewModel()
        {
            GenerateChart = new DelegateCommand(OnGenerateChart);
        }

        public void OnGenerateChart()
        {
            if (ChartIndex < 0 || ChartIndex >= _signalsList.Count)
                return;

            var window1 = new ChartWindow1();
            var viewModel1 = new ChartViewModel1
            {
                LineSeriesTitle = "Probing Signal",
                HistogramTitle = "Feedback Signal"
            };

            viewModel1.GenerateAntennaChart(_signalsList[ChartIndex].probingSignal, _signalsList[ChartIndex].feedbackSignal);

            window1.DataContext = viewModel1;

            var window2 = new ChartWindow1();
            var viewModel2 = new ChartViewModel1
            {
                LineSeriesTitle = "Correlation"
            };

            viewModel2.GenerateCorrelationChart(_signalsList[ChartIndex].correlation);

            window2.DataContext = viewModel2;

            window2.Show();
            window1.Show();
        }

        public void OnStartAntenna(List<(double originalDistance, double calculatedDistance)> data, List<(RealSignal probingSignal, RealSignal feedbackSignal, List<double> correlation)> signalsList)
        {
            _signalsList = signalsList;
            foreach (var item in data)
            {
                CalculatedDistance.Add(item.calculatedDistance);
                OriginalDistance.Add(item.originalDistance);
                DiffrenceBetweenDistances.Add(Math.Abs(item.originalDistance - item.calculatedDistance));
            }
        }
    }
}
