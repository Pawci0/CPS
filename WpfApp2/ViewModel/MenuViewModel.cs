using Lib;
using Microsoft.Win32;
using SciChart.Data.Model;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Helper;

namespace WpfApp2.ViewModel
{
    public class MenuViewModel : BindableObject, IPageViewModel
    {
        public ChartDetailsEnum ChartDetailName { get; set; }
        public string Title { get; set; }
        PageEnum IPageViewModel.NameOfPage => PageEnum.MenuPage;
        public RealSignal Signal { get; set; }

        public ICommand GoNoiseWithUniformDistributionPage { get; }
        public ICommand GoNoiseWithGaussianDistributionPage { get; }
        public ICommand GoSinusPage { get; }
        public ICommand GoSinusOneSideStraightenedPage { get; }
        public ICommand GoSinusBothSideStraightenedPage { get; }
        public ICommand GoRectangularPage { get; }
        public ICommand GoRectangularSymmetricalPage { get; }
        public ICommand GoTriangularPage { get; }
        public ICommand GoHeavisideStepPage { get; }
        public ICommand GoKroneckerDeltaPage { get; }
        public ICommand GoImpulsiveNoisePage { get; }
        public ICommand GoAddSignalsPage { get; }
        public ICommand GoSubtractSignalsPage { get; }
        public ICommand GoMultiplySignalsPage { get; }
        public ICommand GoDivideSignalsPage { get; }
        public ICommand GoReadFromFilePage { get; }

        private readonly BaseViewModel _baseViewModel;

        public MenuViewModel(BaseViewModel baseViewModel)
        {
            _baseViewModel = baseViewModel;

            GoNoiseWithUniformDistributionPage = new DelegateCommand(OnGoNoiseWithUniformDistributionPage);
            GoNoiseWithGaussianDistributionPage = new DelegateCommand(OnGoNoiseWithGaussianDistributionPage);
            GoSinusPage = new DelegateCommand(OnGoSinusPage);
            GoSinusOneSideStraightenedPage = new DelegateCommand(OnGoSinusOneSideStraightenedPage);
            GoSinusBothSideStraightenedPage = new DelegateCommand(OnGoSinusBothSideStraightenedPage);
            GoRectangularPage = new DelegateCommand(OnGoRectangularPage);
            GoRectangularSymmetricalPage = new DelegateCommand(OnGoRectangularSymmetricalPage);
            GoTriangularPage = new DelegateCommand(OnGoTriangularPage);
            GoHeavisideStepPage = new DelegateCommand(OnGoHeavisideStepPage);
            GoKroneckerDeltaPage = new DelegateCommand(OnGoKroneckerDeltaPage);
            GoImpulsiveNoisePage = new DelegateCommand(OnGoImpulsiveNoisePage);
            GoAddSignalsPage = new DelegateCommand(OnGoAddSignalsPage);
            GoSubtractSignalsPage = new DelegateCommand(OnGoSubtractSignalsPage);
            GoMultiplySignalsPage = new DelegateCommand(OnGoMultiplySignalsPage);
            GoDivideSignalsPage = new DelegateCommand(OnGoDivideSignalsPage);
            GoReadFromFilePage = new DelegateCommand(OnGoReadFromFilePage);
        }

        private void OnGoNoiseWithUniformDistributionPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage));

            _baseViewModel.CurrentPageViewModel.Title = "Noise With Uniform Distribution";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.NoiseWithUniformDistribution;
        }

        private void OnGoNoiseWithGaussianDistributionPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage));

            _baseViewModel.CurrentPageViewModel.Title = "Noise With Gaussian Distribution";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.NoiseWithGaussianDistribution;
        }

        private void OnGoSinusPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage1));

            _baseViewModel.CurrentPageViewModel.Title = "Sinus";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.Sinus;
        }

        private void OnGoSinusOneSideStraightenedPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage1));

            _baseViewModel.CurrentPageViewModel.Title = "Sinus One Side Straightened";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.SinusOneSideStraightened;
        }

        private void OnGoSinusBothSideStraightenedPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage1));

            _baseViewModel.CurrentPageViewModel.Title = "Sinus Both Side Straightened";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.SinusBothSideStraightened;
        }

        private void OnGoRectangularPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage2));

            _baseViewModel.CurrentPageViewModel.Title = "Rectangular";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.Rectangular;
        }

        private void OnGoRectangularSymmetricalPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage2));

            _baseViewModel.CurrentPageViewModel.Title = "Rectangular Symmetrical";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.RectangularSymmetrical;
        }

        private void OnGoTriangularPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage2));

            _baseViewModel.CurrentPageViewModel.Title = "Triangular";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.Triangular;
        }
        private void OnGoHeavisideStepPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage3));

            _baseViewModel.CurrentPageViewModel.Title = "Heaviside Step";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.HeavisideStep;
        }

        private void OnGoKroneckerDeltaPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage3));

            _baseViewModel.CurrentPageViewModel.Title = "Kronecker Delta";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.KroneckerDelta;
        }

        private void OnGoImpulsiveNoisePage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.DetailsPage4));

            _baseViewModel.CurrentPageViewModel.Title = "Impulsive Noise";

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.ImpulsiveNoise;
        }

        private void OnGoAddSignalsPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.OperationPage));

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.AddSignals;

            _baseViewModel.CurrentPageViewModel.Title = "Add Signals";
        }

        private void OnGoSubtractSignalsPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.OperationPage));

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.SubtractSignals;

            _baseViewModel.CurrentPageViewModel.Title = "Subtract Signals";
        }

        private void OnGoMultiplySignalsPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.OperationPage));

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.MultiplySignals;

            _baseViewModel.CurrentPageViewModel.Title = "Multiply Signals";
        }

        private void OnGoDivideSignalsPage()
        {
            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.OperationPage));

            _baseViewModel.CurrentPageViewModel.ChartDetailName = ChartDetailsEnum.DivideSignals;

            _baseViewModel.CurrentPageViewModel.Title = "Divide Signals";
        }

        private void OnGoReadFromFilePage()
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                Filter = "fortnite (*.fortnite)|*.fortnite",
                DefaultExt = "fortnite"
            };
            openFileDialog.ShowDialog();

            var signal = RealSignalHelpers.ReadFromFile(openFileDialog.FileName);

            _baseViewModel.ChangeViewModel(_baseViewModel.PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.LoadPage));

            
            _baseViewModel.CurrentPageViewModel.Signal = signal as RealSignal;
            _baseViewModel.CurrentPageViewModel.Title = "Filter signal";
        }
    }
}
