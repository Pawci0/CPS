using Lib;
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
    /// <summary>
    /// Logika interakcji dla klasy signalVariables.xaml
    /// </summary>
    public partial class SignalVariables : Page
    {
        public double Amplitude { get; set; } = 1.0;

        public double BeginsAt { get; set; }

        public double Duration { get; set; } = 2.0;

        public double SamplingFrequency { get; set; }

        public double Period { get; set; } = 0.5;

        public int Interval { get; set; }

        public double FillFactor { get; set; }

        public double Jump { get; set; }

        public double Probability { get; set; }

        public double QuantizationFreq { get; set; } = 20.0;

        public double RecFreq { get; set; } = 20.0;

        public int QuantizationLevel { get; set; } = 4;

        public SignalEnum SelectedSignal { get; set; }

        public int NOfSamples { set; get; } = 4;

        public SignalVariables()
        {
            InitializeComponent();
            DataContext = this;
            SamplingFrequency = 100.0;
        }

        public RealSignal GetSignal()
        {
            if (!IsValid()) throw new Exception("Please check signal parameters");
            var signal = EnumConverter.ConvertTo(SelectedSignal, Amplitude, BeginsAt, Duration, SamplingFrequency, Period, FillFactor, Jump, Probability);
            signal.Interval = Interval;
            return signal;
        }

        public bool IsValid()
        {
            if (Amplitude == 0 || Duration == 0 || SamplingFrequency == 0) return false;
            if(SelectedSignal == SignalEnum.GaussianNoise || SelectedSignal == SignalEnum.UniformNoise 
                                                          || SelectedSignal == SignalEnum.HeavisideStep || SelectedSignal == SignalEnum.KroneckerDelta
                                                          || SelectedSignal == SignalEnum.ImpulsiveNoise)
            {
                return true;
            }

            if (Period == 0) return false;
            if (SelectedSignal != SignalEnum.Triangular && SelectedSignal != SignalEnum.Rectangular) return true;
            return FillFactor != 0;
        }
    }
}
