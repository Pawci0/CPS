using Lib;
using Microsoft.Win32;
using SciChart.Data.Model;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class OperationsDetailsViewModel : BindableObject, IPageViewModel
    {
        private string _title;
        private string _operation;
        private string _path1;
        private string _path2;
        private RealSignal _realSignal;

        #region Properties

        public ICommand GenerateChart { get; }
        public ICommand Save { get; }
        public ICommand SelectFile1 { get; }
        public ICommand SelectFile2 { get; }
        public ChartDetailsEnum ChartDetailName { get; set; }
        public PageEnum NameOfPage => PageEnum.OperationPage;
        public RealSignal Signal { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;

                if (ChartDetailName == ChartDetailsEnum.AddSignals)
                    Operation = "+";
                else if (ChartDetailName == ChartDetailsEnum.SubtractSignals)
                    Operation = "-";
                else if (ChartDetailName == ChartDetailsEnum.MultiplySignals)
                    Operation = "*";
                else if (ChartDetailName == ChartDetailsEnum.DivideSignals)
                    Operation = "/";

                OnPropertyChanged("Title");
            }
        }

        public string Operation
        {
            get => _operation;
            set
            {
                _operation = value;
                OnPropertyChanged("Operation");
            }
        }
        public string Path1
        {
            get => _path1;
            set
            {
                _path1 = value;
                OnPropertyChanged("Path1");
            }
        }
        public string Path2
        {
            get => _path2;
            set
            {
                _path2 = value;
                OnPropertyChanged("Path2");
            }
        }

        #endregion

        public OperationsDetailsViewModel()
        {
            GenerateChart = new DelegateCommand(OnGenerateChart);
            Save = new DelegateCommand(OnSave);
            SelectFile1 = new DelegateCommand(OnSelectFile1);
            SelectFile2 = new DelegateCommand(OnSelectFile2);
        }

        public void OnSelectFile1()
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                Filter = "fortnite (*.fortnite)|*.fortnite",
                DefaultExt = "fortnite"
            };
            openFileDialog.ShowDialog();

            Path1 = openFileDialog.FileName;
        }

        public void OnSelectFile2()
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                Filter = "fortnite (*.fortnite)|*.fortnite",
                DefaultExt = "fortnite"
            };
            openFileDialog.ShowDialog();

            Path2 = openFileDialog.FileName;
        }

        public void OnGenerateChart()
        {
            var window = new ChartWindow1();
            var chartViewModel = new ChartViewModel1();

            if (ChartDetailName == ChartDetailsEnum.AddSignals)
            {
                if (!RealSignalHelpers.AddSignals(RealSignalHelpers.ReadFromFile(Path1) as RealSignal, RealSignalHelpers.ReadFromFile(Path2) as RealSignal, out _realSignal))
                    MessageBox.Show("Somethings goes wrong :/");
            }
            else if (ChartDetailName == ChartDetailsEnum.SubtractSignals)
            {
                if (!RealSignalHelpers.SubtractSignals(RealSignalHelpers.ReadFromFile(Path1) as RealSignal, RealSignalHelpers.ReadFromFile(Path2) as RealSignal, out _realSignal))
                    MessageBox.Show("Somethings goes wrong :/");
            }
            else if (ChartDetailName == ChartDetailsEnum.MultiplySignals)
            {
                if (!RealSignalHelpers.MultiplySignals(RealSignalHelpers.ReadFromFile(Path1) as RealSignal, RealSignalHelpers.ReadFromFile(Path2) as RealSignal, out _realSignal))
                    MessageBox.Show("Somethings goes wrong :/");
            }
            else if (ChartDetailName == ChartDetailsEnum.DivideSignals)
            {
                if (!RealSignalHelpers.DivideSignals(RealSignalHelpers.ReadFromFile(Path1) as RealSignal, RealSignalHelpers.ReadFromFile(Path2) as RealSignal, out _realSignal))
                    MessageBox.Show("Somethings goes wrong :/");
            }

            Signal = _realSignal;
            chartViewModel.LineSeriesTitle = Title;
            chartViewModel.GenerateChart(_realSignal);
            window.DataContext = chartViewModel;
            window.Show();
        }

        public void OnSave()
        {
            if (Signal == null)
            {
                MessageBox.Show("First you have to generate chart");
            }
            else
            {
                var saveFileDialog = new SaveFileDialog
                {
                    AddExtension = true,
                    Filter = "fortnite (*.fortnite)|*.fortnite",
                    DefaultExt = "fortnite",
                    FileName = Title
                };
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName.Length == 0)
                {
                    MessageBox.Show("No files selected");
                }
                else
                {
                    Signal.SaveToFile(saveFileDialog.FileName);
                }
            }
        }
    }
}
