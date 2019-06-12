using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Lib;
using Microsoft.Win32;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for FourierWindow.xaml
    /// </summary>
    public partial class FourierWindow : Window
    {
        private RealSignal signal { get; set; }

        public FourierWindow()
        {
            InitializeComponent();
        }

        private void load(object sender, RoutedEventArgs e)
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
                            if (point != String.Empty)
                                pts.Add(Convert.ToDouble(point));
                        }

                        signal = new RealSignal(begins, period, samplingFreq, pts);
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
            throw new NotImplementedException();
        }
    }
}
