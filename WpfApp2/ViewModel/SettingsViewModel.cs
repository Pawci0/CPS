using SciChart.Data.Model;
using System.Windows;
using WpfApp2.Helper;

namespace WpfApp2.ViewModel
{
    public class SettingsViewModel : BindableObject
    {
        private int _intervals;
        private double _samplingFrequency;
        private int _numberOfLevels;
        private int _numberOfIncludedSamples;

        #region Properties

        public RelayCommand<Window> Apply { get; }

        public int Intervals
        {
            get => _intervals;
            set
            {
                _intervals = value;
                OnPropertyChanged("Intervals");
            }
        }
        public double SamplingFrequency
        {
            get => _samplingFrequency;
            set
            {
                _samplingFrequency = value;
                OnPropertyChanged("SamplingFrequency");
            }
        }
        public int NumberOfLevels
        {
            get => _numberOfLevels;
            set
            {
                _numberOfLevels = value;
                OnPropertyChanged("NumberOfLevels");
            }
        }
        public int NumberOfIncludedSamples
        {
            get => _numberOfIncludedSamples;
            set
            {
                _numberOfIncludedSamples = value;
                OnPropertyChanged("NumberOfIncludedSamples");
            }
        }

        #endregion

        public SettingsViewModel()
        {
            Apply = new RelayCommand<Window>(OnApply);
            Intervals = SettingsData.Intervals;
            SamplingFrequency = SettingsData.SamplingFrequency;
            NumberOfLevels = SettingsData.NumberOfLevels;
            NumberOfIncludedSamples = SettingsData.NumberOfIncludedSamples;
        }

        public void OnApply(Window window)
        {
            SettingsData.Intervals = Intervals;
            SettingsData.SamplingFrequency = SamplingFrequency;
            SettingsData.NumberOfLevels = NumberOfLevels;
            SettingsData.NumberOfIncludedSamples = NumberOfIncludedSamples;
            window.Close();
        }
    }
}
