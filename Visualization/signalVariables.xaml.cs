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
        public double Amplitude { get; set; }

        public double BeginsAt { get; set; }

        public double Duration { get; set; }

        public double SamplingFrequency { get; set; }

        public double Period { get; set; }

        public int Interval { get; set; }

        public double FillFactor { get; set; }

        public SignalEnum SelectedSignal { get; set; }

        public SignalVariables()
        {
            DataContext = this;
            SamplingFrequency = 1.0;
            InitializeComponent();
        }

        public RealSignal GetSignal()
        {
            if(IsValid())
            {
                return EnumConverter.ConvertTo(SelectedSignal, Amplitude, BeginsAt, Duration, SamplingFrequency, Period, FillFactor);
            }
            throw new Exception("Please check signal parameters");
        }

        public bool IsValid()
        {
            if(Amplitude != 0 && Duration != 0 && SamplingFrequency != 0)
            {
                if(SelectedSignal == SignalEnum.GaussianNoise || SelectedSignal == SignalEnum.UniformNoise)
                {
                    return true;
                }
                if(Period != 0)
                {
                    if(SelectedSignal == SignalEnum.Triangular || SelectedSignal == SignalEnum.Rectangular)
                    {
                        if(Interval != 0 && FillFactor != 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
