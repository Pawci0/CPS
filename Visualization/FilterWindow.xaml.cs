﻿using System;
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
    /// Logika interakcji dla klasy FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();
            signalVariables.Content = new SignalVariables();
            filterVariables.Content = new FilterVariables();
            chart.Content = new FilterPage();
            DataContext = this;
        }

        private void showResult(object sender, RoutedEventArgs e)
        {
            var signal = (signalVariables.Content as SignalVariables).GetSignal();

            var filter = (filterVariables.Content as FilterVariables).GetFilter();

            (chart.Content as FilterPage).Update(signal, filter);
        }
    }
}
