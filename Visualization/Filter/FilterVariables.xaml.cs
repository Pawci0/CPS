using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Lib.Filter;

namespace Visualization
{
    /// <summary>
    ///     Logika interakcji dla klasy FilterVariables.xaml
    /// </summary>
    public partial class FilterVariables : Page, INotifyPropertyChanged
    {
        public FilterVariables()
        {
            InitializeComponent();
            DataContext = this;
        }

        public WindowEnum SelectedWindow { get; set; }

        public PassEnum SelectedPass { get; set; }

        public int M { get; set; }

        public double K { get; set; }

        public double F0 { get; set; }

        public double Fp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public Filter GetFilter()
        {
            var pass = EnumConverter.ConvertTo(SelectedPass);
            var window = EnumConverter.ConvertTo(SelectedWindow);
            return new Filter(pass, window, M, K);
        }

        private void CalculateK(object sender, RoutedEventArgs e)
        {
            var pass = EnumConverter.ConvertTo(SelectedPass);
            K = pass.CalculateK(F0, Fp);
            OnPropertyChanged("K");
        }
    }
}