﻿using Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using Microsoft.Win32;

namespace Visualization
{
    public partial class MainWindow : Window
    {
        private RealSignal Signal { get; set; }
        private bool chartSwitch = true;
        private int Interval { get; set; }
        public OperationEnum SelectedOperation { get; set; }

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
            chart.Content = new Chart(Signal);
        }

        public void toHistogram(object sender, RoutedEventArgs e)
        {
            chartSwitch = false;
            chart.Content = new Histogram(Signal, Interval);
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

        public void load(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

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
                    filePath = openFileDialog.FileName;

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
                                chart.Content = new Chart(Signal);
                            }
                            else
                            {
                                chart.Content = new Histogram(Signal, Interval);
                            }
                      }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public void UpdateGraph(object sender, RoutedEventArgs e)
        {
            if (PrepateSignal())
            {
                if (chartSwitch)
                {
                    chart.Content = new Chart(Signal);
                }
                else
                {
                    chart.Content = new Histogram(Signal, Interval);
                }
            }
        }

        private bool PrepateSignal()
        {
            var s1 = (signalOneVariables.Content as SignalVariables);
            var s2 = (signalTwoVariables.Content as SignalVariables);
            try
            {
                Signal = s1.GetSignal();
                if (s2.IsValid())
                {
                    Signal = EnumConverter.Operation(SelectedOperation, Signal, s2.GetSignal());
                }
                Interval = s1.Interval;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
