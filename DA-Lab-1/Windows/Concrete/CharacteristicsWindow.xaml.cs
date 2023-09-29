using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DA_Lab_1
{
    public partial class CharacteristicsWindow : Window
    {
        private List<RowData>? _datas;

        private double _mean;
        private double _median;
        private double _variance;
        private double _standardDeviation;
        private double _secondSkewnessCoeficient;
        private double _secondKurtosisCoeficient;
        private double _min;
        private double _max;

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
            ComputeMean();
            ComputeMedian();
            ComputeStandardDeviation();
            ComputeSkewnessCoefficient();
            ComputeKurtosisCoefficient();
            ComputeMin();
            ComputeMax();
        }

        private void ComputeMean()
        {
            if (_datas == null) return;

            _mean = _datas.Average(data => data.VariantValue);

            MeanGradeText.Text = _mean.ToFormattedString();
        }

        private void ComputeMedian() 
        {
            var rowDatas = _datas?
                .OrderBy(data => data.VariantValue)?
                .ToList();

            if (rowDatas == null) return;

            var count = rowDatas.Count;

            _median = count % 2 == 0
                ? (rowDatas[count / 2].VariantValue + rowDatas[count / 2 - 1].VariantValue) / 2f
                : rowDatas[count / 2].VariantValue;

            MedianGradeText.Text = _median.ToFormattedString();
        }

        private void ComputeStandardDeviation()
        {
            if (_datas == null) return;

            var sum = _datas.Sum(data =>
            {
                var delta = data.VariantValue - _mean;

                return delta * delta;
            });

            _variance = sum / (_datas.Count - 1); //S^2

            _standardDeviation = Math.Sqrt(_variance);

            StandardDeviationGradeText.Text = _standardDeviation.ToFormattedString();
        }

        private void ComputeSkewnessCoefficient()
        {
            if (_datas == null) return;

            var count = _datas.Count;

            var shiftedVarianceSqrt = Math.Sqrt(_variance * (count - 1) / count);

            var sum = _datas.Sum(data =>
            {
                var delta = data.VariantValue - _mean;

                return delta * delta * delta;
            });

            var firstSkewnessCoeficient = sum / (count * shiftedVarianceSqrt * shiftedVarianceSqrt * shiftedVarianceSqrt);

            _secondSkewnessCoeficient = (firstSkewnessCoeficient * Math.Sqrt(count * (count - 1)) / (count - 2));

            SkewnessCoefficientGradeText.Text = _secondSkewnessCoeficient.ToFormattedString();
        }

        private void ComputeKurtosisCoefficient()
        {
            if (_datas == null) return;

            var count = _datas.Count;

            var shiftedVariance = _variance * (count - 1) / count;

            var sum = _datas.Sum(data =>
            {
                var delta = data.VariantValue - _mean;

                return delta * delta * delta * delta;
            });

            var firstKurtosisCoeficient = (sum / (count * shiftedVariance * shiftedVariance)) - 3;

            _secondKurtosisCoeficient = (firstKurtosisCoeficient + 6.0 / (count + 1)) * ((count * count - 1) / ((count - 2) * (count - 3)));

            KurtosisCoefficientGradeText.Text = _secondKurtosisCoeficient.ToFormattedString();
        }

        private void ComputeMin()
        {
            if (_datas == null) return;

            _min = _datas.Min(data => data.VariantValue);

            MinGradeText.Text = _min.ToFormattedString();
        }

        private void ComputeMax()
        {
            if (_datas == null) return;

            _max = _datas.Max(data => data.VariantValue);

            MaxGradeText.Text = _max.ToFormattedString();
        }
    }
}
