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
        public RealSignal Signal { get; set; }

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
            InitializeComponent();
        }
    }
}
