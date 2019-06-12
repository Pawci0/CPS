using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Lib;
using Microsoft.Win32;

namespace Visualization
{
    /// <summary>
    ///     Logika interakcji dla klasy FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();
            filterVariables.Content = new FilterVariables();
            chart.Content = new FilterPage();
            DataContext = this;
        }

        private RealSignal signal { get; set; }

        private void showResult(object sender, RoutedEventArgs e)
        {
            //   var _signal = (signalVariables.Content as SignalVariables).GetSignal();

            var filter = (filterVariables.Content as FilterVariables).GetFilter();

            (chart.Content as FilterPage).Update(signal, filter);
        }

        public void load(object sender, RoutedEventArgs e)
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
                        var begins = Convert.ToDouble(reader.ReadLine());
                        var periodStr = reader.ReadLine();
                        double? period = null;
                        if (periodStr != string.Empty)
                            period = Convert.ToDouble(periodStr);
                        var samplingFreq = Convert.ToDouble(reader.ReadLine());
                        var pointsLine = reader.ReadLine();
                        var points = pointsLine.Split(' ');
                        var pts = new List<double>();
                        foreach (var point in points)
                            if (point != string.Empty)
                                pts.Add(Convert.ToDouble(point));

                        signal = new RealSignal(begins, period, samplingFreq, pts);
                        (chart.Content as FilterPage).UpdateSignal(signal);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}