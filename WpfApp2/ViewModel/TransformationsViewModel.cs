using Lib;
using Lib.Task4;
using Microsoft.Win32;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class TransformationsViewModel : BindableObject, IPageViewModel
    {
        private string _title;
        private double _beginsAt;
        private double _duration;
        private double _samplingFrequency;
        private string _path;
        private ChartDetailsEnum _chartDetailsEnum;
        private SignalEnum _selectedSignal;
        private IEnumerable<SignalEnum> _signalsEnums;
        private TransformationEnum _selectedTransformation;
        private IEnumerable<TransformationEnum> _transformationsEnums;
        private Stopwatch _stopwatch;
        private long _elapsedForward;
        private long _elapsedBackward;

        #region Properties

        public PageEnum NameOfPage => PageEnum.TransformationPage;
        public ICommand Save { get; }
        public ICommand SelectFile { get; }
        public RealSignal Signal { get; set; }
        public ComplexSignal ComplexSignal { get; set; }
        public ComplexSignal ComplexSignalAfterForwardTransformation { get; set; }
        public ComplexSignal ComplexSignalAfterBackwardTransformation { get; set; }
        public ICommand GenerateChart { get; }



        public SignalEnum SelectedSignal
        {
            get => _selectedSignal;
            set
            {
                _selectedSignal = value;
                OnPropertyChanged(nameof(SelectedSignal));
            }
        }

        public IEnumerable<SignalEnum> SignalsEnums
        {
            get => _signalsEnums;
            set
            {
                _signalsEnums = value;
                OnPropertyChanged(nameof(SignalsEnums));
            }
        }

        public TransformationEnum SelectedTransformation
        {
            get => _selectedTransformation;
            set
            {
                _selectedTransformation = value;
                OnPropertyChanged(nameof(SelectedTransformation));
            }
        }

        public IEnumerable<TransformationEnum> TransformationsEnums
        {
            get => _transformationsEnums;
            set
            {
                _transformationsEnums = value;
                OnPropertyChanged(nameof(TransformationsEnums));
            }
        }

        public ChartDetailsEnum ChartDetailName
        {
            get => _chartDetailsEnum;
            set
            {
                _chartDetailsEnum = value;

            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public double BeginsAt
        {
            get => _beginsAt;
            set
            {
                _beginsAt = value;
                OnPropertyChanged(nameof(BeginsAt));
            }
        }

        public double Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public double SamplingFrequency
        {
            get => _samplingFrequency;
            set
            {
                _samplingFrequency = value;
                OnPropertyChanged(nameof(SamplingFrequency));
            }
        }

        #endregion

        public TransformationsViewModel()
        {
            GenerateChart = new DelegateCommand(OnGenerateChart);
            Save = new DelegateCommand(OnSave);
            SelectFile = new DelegateCommand(OnSelectFile);

            BeginsAt = 0;
            Duration = 64;
            SamplingFrequency = 16;

            foreach (var value in Enum.GetValues(typeof(TransformationEnum)))
            {
                
            }

            TransformationsEnums = Enum.GetValues(typeof(TransformationEnum)).Cast<TransformationEnum>();
            SelectedTransformation = TransformationEnum.FftAndIfft;
            SignalsEnums = Enum.GetValues(typeof(SignalEnum)).Cast<SignalEnum>();
            SelectedSignal = SignalEnum.S1;
        }

        public void OnSelectFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                Filter = "fortnite (*.fortnite)|*.fortnite",
                DefaultExt = "fortnite"
            };
            openFileDialog.ShowDialog();

            Path = openFileDialog.FileName;
        }

        private void SetProperties()
        {
            if (Path.IsNullOrEmpty())
            {
                switch (SelectedSignal)
                {
                    case SignalEnum.S1:
                        ComplexSignal = SignalGenerator.SignalForTask4S1(BeginsAt, Duration, SamplingFrequency);
                        break;
                    case SignalEnum.S2:
                        ComplexSignal = SignalGenerator.SignalForTask4S2(BeginsAt, Duration, SamplingFrequency);
                        break;
                    case SignalEnum.S3:
                        ComplexSignal = SignalGenerator.SignalForTask4S3(BeginsAt, Duration, SamplingFrequency);
                        break;
                    default:
                        ComplexSignal = null;
                        break;
                }
            }
            else
            {
                ComplexSignal = RealSignalHelpers.ReadFromFile(Path) as ComplexSignal;
                Path = string.Empty;
            }

            switch (SelectedTransformation)
            {
                case TransformationEnum.Fft:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Fft(ComplexSignal?.Points.ToArray()).ToList());
                    ComplexSignalAfterBackwardTransformation = null;
                    break;
                case TransformationEnum.FftAndIfft:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Fft(ComplexSignal?.Points.ToArray()).ToList());
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Ifft(ComplexSignalAfterForwardTransformation?.Points.ToArray()).ToList());
                    break;
                case TransformationEnum.Ifft:
                    ComplexSignalAfterForwardTransformation = null;
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Ifft(ComplexSignal?.Points.ToArray()).ToList());
                    break;
                case TransformationEnum.Dft:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Dft(ComplexSignal?.Points.ToArray()).ToList());
                    ComplexSignalAfterBackwardTransformation = null;
                    break;
                case TransformationEnum.DftAndIdft:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Dft(ComplexSignal?.Points.ToArray()).ToList());
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Idft(ComplexSignalAfterForwardTransformation?.Points.ToArray()).ToList());
                    break;
                case TransformationEnum.Idft:
                    ComplexSignalAfterForwardTransformation = null;
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Idft(ComplexSignal?.Points.ToArray()).ToList());
                    break;
                case TransformationEnum.Dct:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Dct(ComplexSignal?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    ComplexSignalAfterBackwardTransformation = null;
                    break;
                case TransformationEnum.DctAndIdct:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Dct(ComplexSignal?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Idct(ComplexSignalAfterForwardTransformation?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    break;
                case TransformationEnum.Idct:
                    ComplexSignalAfterForwardTransformation = null;
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Idct(ComplexSignal?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    break;
                case TransformationEnum.Fct:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Fct(ComplexSignal?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    ComplexSignalAfterBackwardTransformation = null;
                    break;
                case TransformationEnum.FctAndIfct:
                    ComplexSignalAfterForwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Fct(ComplexSignal?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Ifct(ComplexSignalAfterForwardTransformation?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    break;
                case TransformationEnum.Ifct:
                    ComplexSignalAfterForwardTransformation = null;
                    ComplexSignalAfterBackwardTransformation = new ComplexSignal(ComplexSignal.BeginsAt, ComplexSignal.SamplingFrequency, Transforms.Ifct(ComplexSignal?.Points.Select(x => x.Real).ToArray()).Select(x => new Complex(x, 0.0)).ToList());
                    break;
                default:
                    ComplexSignalAfterForwardTransformation = null;
                    ComplexSignalAfterBackwardTransformation = null;
                    break;
            }
        }

        public void OnGenerateChart()
        {
            SetProperties();

            if (ComplexSignal != null)
            {
                var window = new ChartWindow1();
                var viewModel = new ChartViewModel1
                {
                    LineSeriesTitle = "ComplexSignal - Realis",
                    HistogramTitle = "ComplexSignal - Imaginalis"
                };

                viewModel.GenerateChart(ComplexSignal);

                window.DataContext = viewModel;

                window.Show();
            }

            if (ComplexSignalAfterForwardTransformation != null)
            {
                var window = new ChartWindow1();
                var viewModel = new ChartViewModel1
                {
                    LineSeriesTitle = "ComplexSignalAfterForwardTransformation - Realis",
                    HistogramTitle = "ComplexSignalAfterForwardTransformation - Imaginalis"
                };

                viewModel.GenerateChart(ComplexSignalAfterForwardTransformation);

                window.DataContext = viewModel;

                window.Show();
            }

            if (ComplexSignalAfterBackwardTransformation != null)
            {
                var window = new ChartWindow1();
                var viewModel = new ChartViewModel1
                {
                    LineSeriesTitle = "ComplexSignalAfterBackwardTransformation - Realis",
                    HistogramTitle = "ComplexSignalAfterBackwardTransformation - Imaginalis"
                };

                viewModel.GenerateChart(ComplexSignalAfterBackwardTransformation);

                window.DataContext = viewModel;

                window.Show();
            }
        }

        public void OnSave()
        {
            if (ComplexSignal == null)
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
                    ComplexSignal.SaveToFile(saveFileDialog.FileName);
                    var extension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                    var fileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(saveFileDialog.FileName),
                        System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName));
                    ComplexSignalAfterForwardTransformation?.SaveToFile($"{fileName}AfterForwardTransformation{extension}");
                    ComplexSignalAfterBackwardTransformation?.SaveToFile($"{fileName}AfterBackwardTransformation{extension}");
                }
            }
        }
    }
}
