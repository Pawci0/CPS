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
using System.Windows.Shapes;

namespace Visualization
{
    /// <summary>
    /// Logika interakcji dla klasy AntenaWindow.xaml
    /// </summary>
    public partial class AntennaWindow : Window
    {
        public AntennaWindow()
        {
            InitializeComponent();
            antenaVariables.Content = new AntennaVariables(ref chart);
            DataContext = this;
        }

        private void showResult(object sender, RoutedEventArgs e)
        {
            (antenaVariables.Content as AntennaVariables).antennaInfo(sender, e);
        }
    }
}
