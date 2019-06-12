using System.Windows.Controls;
using LiveCharts;

namespace Visualization
{
    /// <summary>
    ///     Interaction logic for FourierPage.xaml
    /// </summary>
    public partial class FourierPage : Page
    {
        public SeriesCollection firstChart { get; set; }
        public SeriesCollection secondChart { get; set; }

        public string firstTitle { get; set; }
        public string secondTtitle { get; set; }

        public FourierPage()
        {
            firstTitle = "";
            secondTtitle = "";
            InitializeComponent();
        }
    }
}