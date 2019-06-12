using System.Windows;

namespace Visualization
{
    /// <summary>
    ///     Logika interakcji dla klasy AntenaWindow.xaml
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