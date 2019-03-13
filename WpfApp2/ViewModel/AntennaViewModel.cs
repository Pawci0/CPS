using Lib;
using Lib.Task3;
using Lib.Task3.Helpers;
using SciChart.Data.Model;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class AntennaViewModel : BindableObject, IPageViewModel
    {
        private string _title;
        private int _howManyBasicSignals;
        private double _startDistance;
        private double _simulatorTimeUnit;
        private double _realSpeedOfTheObject;
        private double _speedOfSignalPropagationInEnvironment;
        private double _periodOfTheProbeSignal;
        private double _samplingFrequencyOfTheProbeAndFeedbackSignal;
        private int _lengthOfBuffersOfDiscreteSignals;
        private double _reportingPeriodOfDistance;

        #region Properties

        public PageEnum NameOfPage => PageEnum.AntennaPage;
        public ICommand GenerateChart { get; }
        public RealSignal Signal { get; set; }
        public ChartDetailsEnum ChartDetailName { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public int HowManyBasicSignals
        {
            get => _howManyBasicSignals;
            set
            {
                _howManyBasicSignals = value;
                OnPropertyChanged(nameof(HowManyBasicSignals));
            }
        }

        public double StartDistance
        {
            get => _startDistance;
            set
            {
                _startDistance = value;
                OnPropertyChanged(nameof(StartDistance));
            }
        }

        public double SimulatorTimeUnit
        {
            get => _simulatorTimeUnit;
            set
            {
                _simulatorTimeUnit = value;
                OnPropertyChanged(nameof(SimulatorTimeUnit));
            }
        }

        public double RealSpeedOfTheObject
        {
            get => _realSpeedOfTheObject;
            set
            {
                _realSpeedOfTheObject = value;
                OnPropertyChanged(nameof(RealSpeedOfTheObject));
            }
        }

        public double SpeedOfSignalPropagationInEnvironment
        {
            get => _speedOfSignalPropagationInEnvironment;
            set
            {
                _speedOfSignalPropagationInEnvironment = value;
                OnPropertyChanged(nameof(SpeedOfSignalPropagationInEnvironment));
            }
        }

        public double PeriodOfTheProbeSignal
        {
            get => _periodOfTheProbeSignal;
            set
            {
                _periodOfTheProbeSignal = value;
                OnPropertyChanged(nameof(PeriodOfTheProbeSignal));
            }
        }

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal
        {
            get => _samplingFrequencyOfTheProbeAndFeedbackSignal;
            set
            {
                _samplingFrequencyOfTheProbeAndFeedbackSignal = value;
                OnPropertyChanged(nameof(SamplingFrequencyOfTheProbeAndFeedbackSignal));
            }
        }

        public int LengthOfBuffersOfDiscreteSignals
        {
            get => _lengthOfBuffersOfDiscreteSignals;
            set
            {
                _lengthOfBuffersOfDiscreteSignals = value;
                OnPropertyChanged(nameof(LengthOfBuffersOfDiscreteSignals));
            }
        }

        public double ReportingPeriodOfDistance
        {
            get => _reportingPeriodOfDistance;
            set
            {
                _reportingPeriodOfDistance = value;
                OnPropertyChanged(nameof(ReportingPeriodOfDistance));
            }
        }


        #endregion

        public AntennaViewModel()
        {
            GenerateChart = new DelegateCommand(OnGenerateChart);
            HowManyBasicSignals = 5;
            StartDistance = 5;
            SimulatorTimeUnit = 10;
            RealSpeedOfTheObject = 10;
            SpeedOfSignalPropagationInEnvironment = 3000;
            PeriodOfTheProbeSignal = 1;
            SamplingFrequencyOfTheProbeAndFeedbackSignal = 100;
            LengthOfBuffersOfDiscreteSignals = 500;
            ReportingPeriodOfDistance = 2;
        }

        public void OnGenerateChart()
        {
            var window = new AntennaWindow();
            var antennaViewModel = new AntennaWindowViewModel();

            var objectAndEnvironmentParameters = new AntennaObjectAndEnvironmentParameters(SimulatorTimeUnit, RealSpeedOfTheObject, SpeedOfSignalPropagationInEnvironment);
            var distanceSensorParameters = new AntennaDistanceSensorParameters(PeriodOfTheProbeSignal, SamplingFrequencyOfTheProbeAndFeedbackSignal, LengthOfBuffersOfDiscreteSignals, ReportingPeriodOfDistance);
            var antenna = new Antenna().Start(HowManyBasicSignals, StartDistance, objectAndEnvironmentParameters, distanceSensorParameters, out var signalsList);
            //var antenna2 = new Antenna().ReceiveSendSignal2(HowManyBasicSignals, StartDistance, objectAndEnvironmentParameters, distanceSensorParameters);

            antennaViewModel.OnStartAntenna(antenna, signalsList);

            window.DataContext = antennaViewModel;

            window.Show();
        }
    }
}
