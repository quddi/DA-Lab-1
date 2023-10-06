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
        private double _secondSkewnessCoefficient;
        private double _secondKurtosisCoefficient;
        private double _min;
        private double _max;

        private const double _alpha = 0.05;
        private const double _c0 = 2.515517;
        private const double _c1 = 0.802853;
        private const double _c2 = 0.010328;
        private const double _d1 = 1.432788;
        private const double _d2 = 0.1892659;
        private const double _d3 = 0.001308;

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

            var uP = GetNormalDistributionQuantile(1 - _alpha / 2);

            var j = (int)(count / 2 - uP * Math.Sqrt(count) / 2);
            var k = (int)(count / 2 + 1 + uP * Math.Sqrt(count) / 2);

            MedianTrustIntervalText.Text = string.Format($"[{rowDatas[j].VariantValue}; {rowDatas[k].VariantValue}]");
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

            _secondSkewnessCoefficient = (firstSkewnessCoeficient * Math.Sqrt(count * (count - 1)) / (count - 2));

            SkewnessCoefficientGradeText.Text = _secondSkewnessCoefficient.ToFormattedString();
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

            _secondKurtosisCoefficient = (firstKurtosisCoeficient + 6.0 / (count + 1)) * ((count * count - 1) / ((count - 2) * (count - 3)));

            KurtosisCoefficientGradeText.Text = _secondKurtosisCoefficient.ToFormattedString();
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

        #region Computing methods

        private double GetQuantileT(double a) => Math.Sqrt(-2 * Math.Log2(a));

        private double GetQuantilePhi(double a)
        {
            var t = GetQuantileT(a);

            var numerator = _c0 + _c1 * t + _c2 * t * t;

            var denominator = 1 + _d1 * t + _d2 * t * t + _d3 * t * t * t;

            return t - numerator / denominator;
        }

        private double GetNormalDistributionQuantile(double p)
        {
            return p <= 0.5
                ? -1 * GetQuantilePhi(p)
                : GetQuantilePhi(1 - p);
        }

        private double GetG1(double uP) => (uP * uP * uP + uP) / 4;

        private double GetG2(double uP) => (5 * Math.Pow(uP, 5) + 16 * Math.Pow(uP, 3) + 3 * uP) / 96;

        private double GetG3(double uP) => (3 * Math.Pow(uP, 7) + 19 * Math.Pow(uP, 5) + 17 * Math.Pow(uP, 3) - 15 * uP) / 384;

        private double GetG4(double uP) => (79 * Math.Pow(uP, 9) + 779 * Math.Pow(uP, 7) + 1482 * Math.Pow(uP, 5) - 1920 * Math.Pow(uP, 3) - 945 * uP) / 92160;

        private double GetStudentDistributionQuantile(double p, double v)
        {
            var uP = GetNormalDistributionQuantile(p);

            var g1 = GetG1(uP);
            var g2 = GetG2(uP);
            var g3 = GetG3(uP);
            var g4 = GetG4(uP);

            return uP + (g1 / v) + (g2 / Math.Pow(uP, 2)) + (g3 / Math.Pow(uP, 3)) + (g4 / Math.Pow(uP, 4));
        }

        #endregion
    }
}
