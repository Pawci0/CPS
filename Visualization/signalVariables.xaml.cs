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
        public static double Amplitude { get; set; }

        public static double BeginsAt { get; set; }

        public static double Duration { get; set; }

        public static double SamplingFrequency { get; set; }

        public static double Period { get; set; }

        public static int Interval { get; set; }

        public static double FillFactor { get; set; }

        public static double Jump { get; set; }

        public static double Probability { get; set; }

        public static double QuantizationFreq { get; set; }

        public static double RecFreq { get; set; }

        public SignalEnum SelectedSignal { get; set; }

        public SignalVariables()
        {
            DataContext = this;
            SamplingFrequency = 1.0;
            InitializeComponent();
     
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
