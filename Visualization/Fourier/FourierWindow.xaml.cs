using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Lib;
using Microsoft.Win32;
using System.Numerics;
using Visualization.Fourier;

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

        private RealSignal signal { get; set; }
        private ComplexSignal complexSignal { get; set; }

        public void load(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open the text file using a stream reader.
                var openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "sign (*.sign)|*.sign";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if ((bool)openFileDialog.ShowDialog())
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
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }


        public void loadComplex(object sender, RoutedEventArgs e)
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

                        complexSignal = new ComplexSignal(beginsComplex, period, samplingFreq, pts);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public void showResult(object sender, RoutedEventArgs e)
        {
            var enumVal = (variables.Content as FourierVariables).SelectedTransformationEnum;
            (chart.Content as TransformPage).Update(signal, enumVal);
        }

        public void saveComplex(object sender, RoutedEventArgs e)
        {
            try
            {
                if (complexSignal == null)
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
                        MessageBox.Show("No files selected");
                    else
                        complexSignal.SaveToFile(saveFileDialog.FileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: ", exception.Message);
            }
        }
        private void generateS1(object sender, RoutedEventArgs e)
        {
            signal = (variables.Content as FourierVariables).GetSignal();
        }

    }
}