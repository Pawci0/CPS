﻿using Lib;
using Microsoft.Win32;
using SciChart.Data.Model;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class DetailsViewModel3 : BindableObject, IPageViewModel
    {
        private string _title;
        private double _amplitude;
        private double _beginsAt;
        private double _duration;
        private double _jumpsAt;
        private double _samplingFrequency;
        private ChartDetailsEnum _chartDetailsEnum;

        #region Properties

        public PageEnum NameOfPage => PageEnum.DetailsPage3;
        public ICommand Save { get; }
        public ICommand GenerateChart { get; }
        public RealSignal Signal { get; set; }

        public ChartDetailsEnum ChartDetailName
        {
            get => _chartDetailsEnum;
            set
            {
                _chartDetailsEnum = value;

                if (_chartDetailsEnum == ChartDetailsEnum.HeavisideStep)
                {
                    Amplitude = 10;
                    BeginsAt = 0;
                    Duration = 30;
                    JumpsAt = 15;
                    SamplingFrequency = 100;
                }
                else if (_chartDetailsEnum == ChartDetailsEnum.KroneckerDelta)
                {
                    Amplitude = 10;
                    BeginsAt = 0;
                    Duration = 30;
                    JumpsAt = 15;
                    SamplingFrequency = 5;
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public double Amplitude
        {
            get => _amplitude;
            set
            {
                _amplitude = value;
                OnPropertyChanged("Amplitude");
            }
        }

        public double BeginsAt
        {
            get => _beginsAt;
            set
            {
                _beginsAt = value;
                OnPropertyChanged("BeginsAt");
            }
        }

        public double Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public double JumpsAt
        {
            get => _jumpsAt;
            set
            {
                _jumpsAt = value;
                OnPropertyChanged("JumpsAt");
            }
        }

        public double SamplingFrequency
        {
            get => _samplingFrequency;
            set
            {
                _samplingFrequency = value;
                OnPropertyChanged("SamplingFrequency");
            }
        }

        #endregion

        public DetailsViewModel3()
        {
            GenerateChart = new DelegateCommand(OnGenerateChart);
            Save = new DelegateCommand(OnSave);
        }

        public void OnGenerateChart()
        {
            var windows = new SettingsWindow();
            var settingsViewModel = new SettingsViewModel();

            windows.DataContext = settingsViewModel;
            windows.ShowDialog();

            if (ChartDetailName == ChartDetailsEnum.HeavisideStep)
            {
                Signal = SignalGenerator.HeavisideStep(Amplitude, BeginsAt, Duration, JumpsAt, SamplingFrequency);
            }
            else if (ChartDetailName == ChartDetailsEnum.KroneckerDelta)
            {
                Signal = SignalGenerator.KroneckerDelta(Amplitude, BeginsAt, Duration, JumpsAt, SamplingFrequency);
            }

            var acModel = new AcModel(Signal, SettingsData.SamplingFrequency, SettingsData.NumberOfLevels, SettingsData.NumberOfIncludedSamples);

            var window = new ChartWindow1();
            var chartViewModel = new ChartViewModel1
            {
                LineSeriesTitle = Title,
                HistogramTitle = "Histogram",
                Param1 = Signal.AbsoluteAverageValue,
                Param2 = Signal.AveragePower,
                Param3 = Signal.AverageValue,
                Param4 = Signal.RootMeanSquare,
                Param5 = Signal.Variance,
                ParamName1 = "Absolute Average Value:",
                ParamName2 = "Average Power:",
                ParamName3 = "Average Value:",
                ParamName4 = "Root Mean Square:",
                ParamName5 = "Variance:"
            };

            if (ChartDetailName == ChartDetailsEnum.HeavisideStep)
            {
                chartViewModel.GenerateChart(Signal);
            }
            else if (ChartDetailName == ChartDetailsEnum.KroneckerDelta)
            {
                chartViewModel.GeneratePointChart(Signal);
            }

            window.DataContext = chartViewModel;

            var window1 = new ChartWindow1();
            var chartViewModel1 = new ChartViewModel1
            {
                LineSeriesTitle = "Original " + Title,
                HistogramTitle = "Uniform Sampling",
                Param1 = acModel.UniformSampling.MeanSquaredError,
                Param2 = acModel.UniformSampling.SignalToNoiseRatio,
                Param3 = acModel.UniformSampling.PeakSignalToNoiseRatio,
                Param4 = acModel.UniformSampling.MaximumDifference,
                Param5 = acModel.UniformSampling.EffectiveNumberOfBits,
                ParamName1 = "Mean Squared Error:",
                ParamName2 = "Signal To Noise Ratio:",
                ParamName3 = "Peak Signal To Noise Ratio:",
                ParamName4 = "Maximum Difference:",
                ParamName5 = "Effective Number OfBits:"
            };

            chartViewModel1.GenerateUniformSamplingChart(Signal, acModel.UniformSampling.Signal);
            window1.DataContext = chartViewModel1;

            var window2 = new ChartWindow1();
            var chartViewModel2 = new ChartViewModel1
            {
                LineSeriesTitle = "Original " + Title,
                HistogramTitle = "Uniform Quantization With Truncation",
                Param1 = acModel.UniformQuantizationWithTruncation.MeanSquaredError,
                Param2 = acModel.UniformQuantizationWithTruncation.SignalToNoiseRatio,
                Param3 = acModel.UniformQuantizationWithTruncation.PeakSignalToNoiseRatio,
                Param4 = acModel.UniformQuantizationWithTruncation.MaximumDifference,
                Param5 = acModel.UniformQuantizationWithTruncation.EffectiveNumberOfBits,
                ParamName1 = "Mean Squared Error:",
                ParamName2 = "Signal To Noise Ratio:",
                ParamName3 = "Peak Signal To Noise Ratio:",
                ParamName4 = "Maximum Difference:",
                ParamName5 = "Effective Number OfBits:"
            };

            chartViewModel2.GenerateUniformQuantizationWithTruncationChart(Signal, acModel.UniformQuantizationWithTruncation.Signal);
            window2.DataContext = chartViewModel2;

            var window3 = new ChartWindow1();
            var chartViewModel3 = new ChartViewModel1
            {
                LineSeriesTitle = "Original " + Title,
                HistogramTitle = "Zero Order Hold",
                Param1 = acModel.ZeroOrderHold.MeanSquaredError,
                Param2 = acModel.ZeroOrderHold.SignalToNoiseRatio,
                Param3 = acModel.ZeroOrderHold.PeakSignalToNoiseRatio,
                Param4 = acModel.ZeroOrderHold.MaximumDifference,
                Param5 = acModel.ZeroOrderHold.EffectiveNumberOfBits,
                ParamName1 = "Mean Squared Error:",
                ParamName2 = "Signal To Noise Ratio:",
                ParamName3 = "Peak Signal To Noise Ratio:",
                ParamName4 = "Maximum Difference:",
                ParamName5 = "Effective Number OfBits:"
            };

            chartViewModel3.GenerateZeroOrderHoldChart(Signal, acModel.ZeroOrderHold.Signal);
            window3.DataContext = chartViewModel3;

            var window4 = new ChartWindow1();
            var chartViewModel4 = new ChartViewModel1
            {
                LineSeriesTitle = "Original " + Title,
                HistogramTitle = "Reconstruction Based On The Sinc Function",
                Param1 = acModel.ReconstructionBasedOnTheSincFunction.MeanSquaredError,
                Param2 = acModel.ReconstructionBasedOnTheSincFunction.SignalToNoiseRatio,
                Param3 = acModel.ReconstructionBasedOnTheSincFunction.PeakSignalToNoiseRatio,
                Param4 = acModel.ReconstructionBasedOnTheSincFunction.MaximumDifference,
                Param5 = acModel.ReconstructionBasedOnTheSincFunction.EffectiveNumberOfBits,
                ParamName1 = "Mean Squared Error:",
                ParamName2 = "Signal To Noise Ratio:",
                ParamName3 = "Peak Signal To Noise Ratio:",
                ParamName4 = "Maximum Difference:",
                ParamName5 = "Effective Number OfBits:"
            };

            chartViewModel4.GenerateReconstructionBasedOnTheSincFunctionChart(Signal, acModel.ReconstructionBasedOnTheSincFunction.Signal);
            window4.DataContext = chartViewModel4;

            //  window4.Show();
            // window3.Show();
            // window2.Show();
            //  window1.Show();
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
