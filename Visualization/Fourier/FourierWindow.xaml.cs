using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Lib;
using Microsoft.Win32;
using System.Numerics;

namespace Visualization
{
    /// <summary>
    ///     Interaction logic for FourierWindow.xaml
    /// </summary>
    public partial class FourierWindow : Window
    {
        public FourierWindow()
        {
            InitializeComponent();
            DataContext = this;
            variables.Content = new FourierVariables();
            chart.Content = new FourierPage();
        }

        private RealSignal s1Signal { get; set; }
        private ComplexSignal signal { get; set; }

        private void load(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open the text file using a stream reader.
                var openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "sign (*.sign)|*.sign";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if ((bool) openFileDialog.ShowDialog())
                {
                    //Get the path of specified file

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (var reader = new StreamReader(fileStream))
                    {
                        reader.ReadLine();
                        var begins = reader.ReadLine().Split(',');
                        var beginsComplex = new Complex(Convert.ToDouble(begins[0]), Convert.ToDouble(begins[1]));
                        var periodStr = reader.ReadLine();
                        double? period = null;
                        if (periodStr != string.Empty)
                            period = Convert.ToDouble(periodStr);
                        var samplingFreq = Convert.ToDouble(reader.ReadLine());
                        var pointsLine = reader.ReadLine();
                        var allPoints = pointsLine.Split(' ');
                        var pts = new List<Complex>();
                        foreach (var point in allPoints)
                            if (point != string.Empty)
                                foreach (var value in point.Split(','))                                
                                    pts.Add(new Complex(Convert.ToDouble(value[0]), Convert.ToDouble(value[1])));

                        signal = new ComplexSignal(beginsComplex, period, samplingFreq, pts);
                        //(chart.Content as FilterPage).UpdateSignal(signal);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void showResult(object sender, RoutedEventArgs e)
        {
            var s1sig = (variables.Content as FourierVariables).GetSignal();

            (chart.Content as FourierPage).Update(s1sig);
        }
    }
}