using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DA_Lab_1
{
    public partial class CharacteristicsWindow : Window
    {
        private List<RowData>? _datas;

        public CharacteristicsWindow() 
        { 
            InitializeComponent(); 
        }

        public void InitializeComponent(List<RowData>? datas)
        {
            _datas = datas;

            ComputeCharacteristics();
        }
      
        private void ComputeCharacteristics()
        {
            if (_datas == null)
                throw new InvalidOperationException($"Для розрахунку необхідні row datas!");

            Characteristics.Compute(_datas);
            
            DisplayCharacteristics();
        }

        private void DisplayCharacteristics()
        {
            DisplayMean();
            DisplayMedian();
            DisplayStandardDeviation();
            DisplaySkewnessCoefficient();
            DisplayKurtosisCoefficient();
            DisplayMin();
            DisplayMax();
        }

        private void DisplayMean()
        {
            MeanGradeText.Text = Characteristics.Mean.Value.ToFormattedString();

            MeanMSDEText.Text = Characteristics.MeanSigma.Value.ToFormattedString();

            var edges = Characteristics.MeanTrustInterval.Value;

            MeanTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayMedian() 
        {
            MedianGradeText.Text = Characteristics.Median.Value.ToFormattedString();

            var edges = Characteristics.MedianTrustInterval.Value;

            MedianTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayStandardDeviation()
        {
            StandardDeviationGradeText.Text = Characteristics.StandardDeviation.Value.ToFormattedString();

            StandardDeviationMSDEText.Text = Characteristics.StandardDeviationSigma.Value.ToFormattedString();

            var edges = Characteristics.StandardDeviationTrustInterval.Value;

            StandardDeviationTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplaySkewnessCoefficient()
        {
            SkewnessCoefficientGradeText.Text = Characteristics.SecondSkewnessCoefficient.Value.ToFormattedString();

            SkewnessCoefficientMSDEText.Text = Characteristics.SecondSkewnessCoefficientSigma.Value.ToFormattedString();

            var edges = Characteristics.SecondSkewnessCoefficientTrustInterval.Value;

            SkewnessCoefficientTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayKurtosisCoefficient()
        {
            KurtosisCoefficientGradeText.Text = Characteristics.SecondKurtosisCoefficient.Value.ToFormattedString();

            KurtosisCoefficientMSDEText.Text = Characteristics.SecondKurtosisCoefficientSigma.Value.ToFormattedString();

            var edges = Characteristics.SecondKurtosisCoefficientTrustInterval.Value;

            KurtosisCoefficientTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayMin()
        {
            MinGradeText.Text = Characteristics.Min.Value.ToFormattedString();
        }

        private void DisplayMax()
        {
            MaxGradeText.Text = Characteristics.Max.Value.ToFormattedString();
        }
    }
}
