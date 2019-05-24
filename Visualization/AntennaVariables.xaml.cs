﻿using Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Lib.Antenna;
using static Lib.Antenna.AntennaParameters;

namespace Visualization
{
    /// <summary>
    /// Logika interakcji dla klasy AntennaVariables.xaml
    /// </summary>
    public partial class AntennaVariables : Page
    {
        public int NumberOfBasicSignals { get; set; } = 5;
        public double PeriodOfTheProbeSignal { get; set; } = 1.0;

        public double SamplingFrequencyOfTheProbeAndFeedbackSignal { get; set; } = 100.0;

        public int LengthOfBuffersOfDiscreteSignals { get; set; } = 500;

        public double ReportingPeriodOfDistance { get; set; } = 2.0;

        public double SimulatorTimeUnit { get; set; } = 10;

        public double RealSpeedOfTheObject { get; set; } = 10;

        public double SpeedOfSignalPropagationInEnvironment { get; set; } = 3000;

        public int AmountOfMeasuringPoints { get; set; } = 12;

        private AntennaParameters Parameters { get; set; }

        private Frame chart;

        public AntennaVariables(ref Frame chart)
        {
            InitializeComponent();
            DataContext = this;
            this.chart = chart;
        }

        public void antennaInfo(object sender, RoutedEventArgs e)
        {
            Parameters = new AntennaParameters(PeriodOfTheProbeSignal, SamplingFrequencyOfTheProbeAndFeedbackSignal,
                LengthOfBuffersOfDiscreteSignals, ReportingPeriodOfDistance,
                SimulatorTimeUnit, RealSpeedOfTheObject, SpeedOfSignalPropagationInEnvironment, AmountOfMeasuringPoints);
            var result = Antenna.CalculateAntenna(NumberOfBasicSignals, 0, Parameters, out var realSignal, out var signal, out var correlationS);
            chart.Content = new AntennaPage(realSignal, signal, correlationS);
            string s = "Real distance \t Calculated distance \t delta\n";
            foreach (var val in result)
            {
                s += val.Item1 +"\t\t"+val.Item2+"\t\t\t"+(val.Item1-val.Item2)+ "\n";
            }
            MessageBox.Show(s, "Info");
        }
    }

}
