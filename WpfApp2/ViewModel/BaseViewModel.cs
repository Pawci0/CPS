using SciChart.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class BaseViewModel : BindableObject
    {
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        #region Properties

        public ICommand GoToMenuPage { get; }
        public ICommand GoToAntennaPage { get; }
        public ICommand GoToSettings { get; }
        public ICommand GoToTransformationPage { get; }
        public List<IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new List<IPageViewModel>());

        public IPageViewModel CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        #endregion

        public BaseViewModel()
        {
            GoToMenuPage = new DelegateCommand(OnGoToMenuPage);
            GoToAntennaPage = new DelegateCommand(OnGoToAntennaPage);
            GoToSettings = new DelegateCommand(OnGoToSettings);
            GoToTransformationPage = new DelegateCommand(OnGoToTransformationPage);

            PageViewModels.Add(new DetailsViewModel());
            PageViewModels.Add(new DetailsViewModel1());
            PageViewModels.Add(new DetailsViewModel2());
            PageViewModels.Add(new DetailsViewModel3());
            PageViewModels.Add(new DetailsViewModel4());
            PageViewModels.Add(new OperationsDetailsViewModel());

            PageViewModels.Add(new MenuViewModel(this));

            CurrentPageViewModel = PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.MenuPage);
        }

        public void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        private void OnGoToMenuPage()
        {
            ChangeViewModel(PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.MenuPage));
        }

        private void OnGoToAntennaPage()
        {
            ChangeViewModel(PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.AntennaPage));
            CurrentPageViewModel.Title = "Antenna";
        }

        private void OnGoToTransformationPage()
        {
            ChangeViewModel(PageViewModels.FirstOrDefault(p => p.NameOfPage == PageEnum.TransformationPage));
            CurrentPageViewModel.Title = "Transformation";
        }

        public void OnGoToSettings()
        {
            var window = new SettingsWindow();
            var settingsViewModel = new SettingsViewModel();

            window.DataContext = settingsViewModel;
            window.Show();
        }
    }
}
