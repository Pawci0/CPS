using Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using static Lib.SignalOperations;
using static Lib.Signals;
using System.Threading.Tasks;

namespace Visualization
{
    public partial class MainWindow : Window
    {
        private RealSignal Signal { get; set; }
        private bool chartSwitch = true;
        private int Interval { get; set; }
        public OperationEnum SelectedOperation { get; set; }
        public bool ConnectPoints { get; set; } = false;

        private bool antennaSwitch = false;

        public MainWindow()
        {
            InitializeComponent();
            signalOneVariables.Content = new SignalVariables();
            signalTwoVariables.Content = new SignalVariables();
            chart.Content = new Chart();
            DataContext = this;
        }

        public void toChart(object sender, RoutedEventArgs e)
        {
            chartSwitch = true;
            chart.Content = new Chart(Signal, ConnectPoints);
        }

        public void toHistogram(object sender, RoutedEventArgs e)
        {
            chartSwitch = false;
            chart.Content = new Histogram(Signal);
        }

        public void toAC(object sender, RoutedEventArgs e)
        {
            chart.Content = new AC(Signal, (signalOneVariables.Content as SignalVariables));
        }

        public void toCA(object sender, RoutedEventArgs e)
        {
            chart.Content = new CA(Signal, (signalOneVariables.Content as SignalVariables));
        }

        public void save(object sender, RoutedEventArgs e)
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
                    Filter = "sign (*.sign)|*.sign",
                    DefaultExt = "sign",
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

        public void antenna(Object sender, RoutedEventArgs e)
        {
            Window window = new AntennaWindow();
            window.Show();
            //if (!antennaSwitch)
            //{
            //    antennaSwitch = true;
            //    signalOneVariables.Content = new AntennaVariables(ref chart);
            //    signalTwoVariables.Content = null;
            //}
        }
        public void load(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open the text file using a stream reader.
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "sign (*.sign)|*.sign";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if ((bool) openFileDialog.ShowDialog())
                {
                    //Get the path of specified file

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        reader.ReadLine();
                        var begins = Convert.ToDouble(reader.ReadLine());
                        var periodStr = reader.ReadLine();
                        double? period = null;
                        if (periodStr != String.Empty)
                            period = Convert.ToDouble(periodStr);
                        var samplingFreq = Convert.ToDouble(reader.ReadLine());
                        var pointsLine = reader.ReadLine();
                        var points = pointsLine.Split(' ');
                        List<double> pts = new List<double>();
                        foreach (var point in points)
                        {
                            if(point != String.Empty)
                                pts.Add(Convert.ToDouble(point));
                        }

                        Signal = new RealSignal(begins, period, samplingFreq, pts);
                            if (chartSwitch)
                            {
                                chart.Content = new Chart(Signal, ConnectPoints);
                            }
                            else
                            {
                                chart.Content = new Histogram(Signal);
                            }
                      }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public void ShowFirst(object sender, RoutedEventArgs e)
        {
            try
            {
                var s1 = (signalOneVariables.Content as SignalVariables);
                ShowSignal(s1.GetSignal(), s1);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
        public void ShowSecond(object sender, RoutedEventArgs e)
        {
            var s2 = (signalTwoVariables.Content as SignalVariables);
            ShowSignal(s2?.GetSignal(), s2);
        }

        public void UpdateGraph(SignalVariables sv)
        {
            (chart.Content as SignalPage)?.Update(Signal, sv, ConnectPoints);
        }

        private void ShowResult(object sender, RoutedEventArgs e)
        {
            var s1 = (signalOneVariables.Content as SignalVariables);
            var s2 = (signalTwoVariables.Content as SignalVariables);
            try
            {
                Signal = s1?.GetSignal();
                if (s2.IsValid())
                {
                    Signal = EnumConverter.Operation(SelectedOperation, Signal, s2.GetSignal());
                }
                ShowSignal(Signal, s1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ShowSignal(RealSignal signal, SignalVariables sv)
        {
            Signal = signal;
            real = Signal;
            ConnectPoints = (sv.SelectedSignal != SignalEnum.KroneckerDelta && sv.SelectedSignal != SignalEnum.ImpulsiveNoise);
            UpdateGraph(sv);
        }

        public void moreInfo(object sender, RoutedEventArgs e)
        {
            if (Signal != null)
            {
                real = Signal;
                string s = String.Format("Average value: {0} \n" +
                                         "Absolute average value: {1} \n" +
                                         "Root mean square: {2} \n" +
                                         "Variance: {3} \n" +
                                         "Average power: {4} \n\n" +
                                         "Mean squared error: {5} \n" +
                                         "Signal to noise ratio: {6} \n" +
                                         "Peak signal to noise ratio: {7} \n" +
                                         "Maximum Difference: {8} \n" +
                                         "Effective number of bits: {9}"
                    , AverageValue(Signal), AbsoluteAverageValue(Signal), RootMeanSquare(Signal), Variance(Signal),
                    AveragePower(Signal), MeanSquaredError(Signal, quantized), SignalToNoiseRatio(Signal, quantized),
                    PeakSignalToNoiseRatio(Signal, quantized), MaximumDifference(Signal, quantized), EffectiveNumberOfBits(Signal, quantized));
                MessageBox.Show(s, "Info");
            }
        }

        public void filters(object sender, RoutedEventArgs e)
        {
            Window window = new FilterWindow();
            window.Show();
        }
    }
}
