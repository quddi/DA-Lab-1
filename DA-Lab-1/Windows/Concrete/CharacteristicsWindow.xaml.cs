using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace DA_Lab_1
{
    public partial class CharacteristicsWindow : Window
    {
        private List<RowData> _rowDatas;
        private List<GroupedData> _groupedDatas;

        private readonly Color ProbabilityPaperPointsColor = Color.Red;

        public CharacteristicsWindow() 
        { 
            InitializeComponent();

            ResizeMode = ResizeMode.NoResize;

            PrepareProbabilityPaperChart();
        }

        public void InitializeComponent(List<RowData> rowDatas, List<GroupedData> groupedDatas)
        {
            _rowDatas = rowDatas;
            _groupedDatas = groupedDatas.OrderBy(data => data.VariantValue).ToList();

            ComputeCharacteristics();
        }
      
        private void ComputeCharacteristics()
        {
            if (_rowDatas == null)
                throw new InvalidOperationException($"Для розрахунку необхідні row datas!");
            
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

        private void PrepareProbabilityPaperChart()
        {
            var plot = ProbabilityPaperChart.Plot;

            plot.Title("Ймовірнісний папір");
            plot.XLabel("t");
            plot.YLabel("z");
        }

        #region Buttons handlers
        private void IdentifyBySkewnessButtonClick(object sender, RoutedEventArgs e)
        {
            var identified = Math.Abs(Characteristics.SkewnessStatistics) <= Characteristics.StudentQuantile;

            MessageBox.Show(identified ?
                string.Format($"Нормальний розподіл ідентифікується.\n|{Characteristics.SkewnessStatistics}| <= {Characteristics.StudentQuantile}") :
                string.Format($"Нормальний розподіл не ідентифікується.\n|{Characteristics.SkewnessStatistics}| <= {Characteristics.StudentQuantile}"));
        }

        private void IdentifyByKurtosisButtonClick(object sender, RoutedEventArgs e)
        {
            var identified = Math.Abs(Characteristics.KurtosisStatistics) <= Characteristics.StudentQuantile;

            MessageBox.Show(identified ?
                string.Format($"Нормальний розподіл ідентифікується.\n|{Characteristics.KurtosisStatistics}| < {Characteristics.StudentQuantile}") :
                string.Format($"Нормальний розподіл не ідентифікується.\n|{Characteristics.KurtosisStatistics}| < {Characteristics.StudentQuantile}"));
        }

        private void BuildProbabilityPaperButtonClick(object sender, RoutedEventArgs e)
        {
            var plot = ProbabilityPaperChart.Plot;

            plot.Clear();

            var transformedDatas = MainDataConverter.Handle<GroupedData, TransformedGroupedData>(_groupedDatas);

            foreach (var transformedData in transformedDatas)
            {
                plot.AddPoint(transformedData.T, transformedData.Z, ProbabilityPaperPointsColor);
            }

            ProbabilityPaperChart.Refresh();
        }
        #endregion
    }
}
