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

            Characteristics.SetDatas(_datas);
            
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
            MeanGradeText.Text = Characteristics.Mean.ToFormattedString();

            MeanMSDEText.Text = Characteristics.MeanSigma.ToFormattedString();

            var edges = Characteristics.MeanTrustInterval;

            MeanTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayMedian() 
        {
            MedianGradeText.Text = Characteristics.Median.ToFormattedString();

            var edges = Characteristics.MedianTrustInterval;

            MedianTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayStandardDeviation()
        {
            StandardDeviationGradeText.Text = Characteristics.StandardDeviation.ToFormattedString();

            StandardDeviationMSDEText.Text = Characteristics.StandardDeviationSigma.ToFormattedString();

            var edges = Characteristics.StandardDeviationTrustInterval;

            StandardDeviationTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplaySkewnessCoefficient()
        {
            SkewnessCoefficientGradeText.Text = Characteristics.SecondSkewnessCoefficient.ToFormattedString();

            SkewnessCoefficientMSDEText.Text = Characteristics.SecondSkewnessCoefficientSigma.ToFormattedString();

            var edges = Characteristics.SecondSkewnessCoefficientTrustInterval;

            SkewnessCoefficientTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayKurtosisCoefficient()
        {
            KurtosisCoefficientGradeText.Text = Characteristics.SecondKurtosisCoefficient.ToFormattedString();

            KurtosisCoefficientMSDEText.Text = Characteristics.SecondKurtosisCoefficientSigma.ToFormattedString();

            var edges = Characteristics.SecondKurtosisCoefficientTrustInterval;

            KurtosisCoefficientTrustIntervalText.Text = string.Format($"[{edges.LeftEdge.ToFormattedString()}; {edges.RightEdge.ToFormattedString()}]");
        }

        private void DisplayMin()
        {
            MinGradeText.Text = Characteristics.Min.ToFormattedString();
        }

        private void DisplayMax()
        {
            MaxGradeText.Text = Characteristics.Max.ToFormattedString();
        }
    }
}
