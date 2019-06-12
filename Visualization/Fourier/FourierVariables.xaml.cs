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
using Lib;
using Lib.Filter;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for FourierVariables.xaml
    /// </summary>
    public partial class FourierVariables : Page
    {
        public double begins { get; set; } = 0;
        public double duration { get; set; } = 64;
        public double samplingFrequency { get; set; } = 16;
        public TransformationEnum SelectedTransformationEnum { get; set; }

        public FourierVariables()
        {
            InitializeComponent();
            DataContext = this;
        }

        public RealSignal GetSignal()
        {

            return SignalGenerator.S1Signal(0, begins, duration, samplingFrequency, 0);
        }
    }
}
