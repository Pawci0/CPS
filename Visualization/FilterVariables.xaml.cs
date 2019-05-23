using Lib.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Visualization
{
    /// <summary>
    /// Logika interakcji dla klasy FilterVariables.xaml
    /// </summary>
    public partial class FilterVariables : Page, INotifyPropertyChanged
    {
        public WindowEnum SelectedWindow { get; set; }

        public PassEnum SelectedPass { get; set; }

        public int M { get; set; }

        public double K { get; set; }

        public double F0 { get; set; }

        public double Fp { get; set; }

        public FilterVariables()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }

        }

        public Filter GetFilter()
        {
            var pass = EnumConverter.ConvertTo(SelectedPass);
            var window = EnumConverter.ConvertTo(SelectedWindow);
            return new Filter(pass, window, M, K);
        }

        private void calculateK(object sender, RoutedEventArgs e)
        {
            var pass = EnumConverter.ConvertTo(SelectedPass);
            K = pass.CalculateK(F0, Fp);
            OnPropertyChanged("K");
        }
    }
}
