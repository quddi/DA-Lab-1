using System.Windows.Markup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    public static class Characteristics
    {
        public static double? Mean {  get; set; }
        public static double? MeanSigma { get; set; }
        public static (double LeftEdge, double RightEdge)? MeanTrustInterval { get; set; }

        public static double? Median { get; set; }
        public static (double LeftEdge, double RightEdge)? MedianTrustInterval { get; set; }

        public static double? StandardDeviation { get; set; }
        public static double? StandardDeviationSigma { get; set; }
        public static (double LeftEdge, double RightEdge)? StandardDeviationTrustInterval { get; set; }

        public static double? SecondSkewnessCoefficient { get; set; }
        public static double? SecondSkewnessCoefficientSigma { get; set; }
        public static (double LeftEdge, double RightEdge)? SecondSkewnessCoefficientTrustInterval { get; set; }

        public static double? SecondKurtosisCoefficient { get; set; }
        public static double? SecondKurtosisCoefficientSigma { get; set; }
        public static (double LeftEdge, double RightEdge)? SecondKurtosisCoefficientTrustInterval { get; set; }

        public static double? Variance { get; set; }
        public static double? Min {  get; set; }
        public static double? Max { get; set; }


        private const double _alpha = 0.05;
        private const double _c0 = 2.515517;
        private const double _c1 = 0.802853;
        private const double _c2 = 0.010328;
        private const double _d1 = 1.432788;
        private const double _d2 = 0.1892659;
        private const double _d3 = 0.001308;

        public static void Compute(List<RowData> datas)
        {
            ComputeMean(datas);
            ComputeMedian(datas);
            ComputeStandardDeviation(datas);
            ComputeSkewnessCoefficient(datas);
            ComputeKurtosisCoefficient(datas);
            ComputeMin(datas);
            ComputeMax(datas);
        }

        private static void ComputeMean(List<RowData> datas)
        {
            if (datas == null) return;

            Mean = datas.Average(data => data.VariantValue);

            var count = datas.Count;

            var studentQuantile = GetStudentDistributionQuantile(1 - _alpha / 2, count - 1);

            var sum = datas.Sum(data =>
            {
                var delta = data.VariantValue - Mean;
                return delta * delta;
            });

            Variance = sum / (datas.Count - 1); //S^2

            StandardDeviation = Math.Sqrt(Variance.Value);

            MeanSigma = GetMeanSquaredStandardDeviation(StandardDeviation.Value, count);

            MeanTrustInterval = (Mean.Value - studentQuantile * MeanSigma.Value, Mean.Value + studentQuantile * MeanSigma.Value);
        }

        private static void ComputeMedian(List<RowData> datas)
        {
            var rowDatas = datas?
                .OrderBy(data => data.VariantValue)?
                .ToList();

            var count = rowDatas.Count;

            Median = count % 2 == 0
                ? (rowDatas[count / 2].VariantValue + rowDatas[count / 2 - 1].VariantValue) / 2f
                : rowDatas[count / 2].VariantValue;

            var uP = GetNormalDistributionQuantile(1 - _alpha / 2);

            var j = (int)(count / 2 - uP * Math.Sqrt(count) / 2);
            var k = (int)(count / 2 + 1 + uP * Math.Sqrt(count) / 2);

            MedianTrustInterval = (rowDatas[j].VariantValue, rowDatas[k].VariantValue);
        }

        private static void ComputeStandardDeviation(List<RowData> datas)
        {
            var sum = datas.Sum(data =>
            {
                var delta = data.VariantValue - Mean;

                return delta * delta;
            });

            Variance = sum / (datas.Count - 1); //S^2

            StandardDeviation = Math.Sqrt(Variance.Value);

            var count = datas.Count;

            var studentQuantile = GetStudentDistributionQuantile(1 - _alpha / 2, count - 1);

            StandardDeviationSigma = GetSampleMeanSquaredStandardDeviation(StandardDeviation.Value, count);

            StandardDeviationTrustInterval = (StandardDeviation.Value - studentQuantile * StandardDeviationSigma.Value, StandardDeviation.Value + studentQuantile * StandardDeviationSigma.Value);
        }

        private static void ComputeSkewnessCoefficient(List<RowData> datas)
        {
            var count = datas.Count;

            var shiftedVarianceSqrt = Math.Sqrt(Variance.Value * (count - 1) / count);

            var sum = datas.Sum(data =>
            {
                var delta = data.VariantValue - Mean.Value;

                return delta * delta * delta;
            });

            var firstSkewnessCoefficient = sum / (count * shiftedVarianceSqrt * shiftedVarianceSqrt * shiftedVarianceSqrt);

            SecondSkewnessCoefficient = (firstSkewnessCoefficient * Math.Sqrt(count * (count - 1)) / (count - 2));

            var studentQuantile = GetStudentDistributionQuantile(1 - _alpha / 2, count - 1);

            SecondSkewnessCoefficientSigma = GetSkewnessCoefficientRootMeanSquareDeviation(count);

            SecondSkewnessCoefficientTrustInterval = (
                SecondSkewnessCoefficient.Value - studentQuantile * SecondSkewnessCoefficientSigma.Value, 
                SecondSkewnessCoefficient.Value + studentQuantile * SecondSkewnessCoefficientSigma.Value
                );
        }

        private static void ComputeKurtosisCoefficient(List<RowData> datas)
        {
            var count = datas.Count;

            var shiftedVariance = Variance.Value * (count - 1) / count;

            var sum = datas.Sum(data =>
            {
                var delta = data.VariantValue - Mean.Value;

                return delta * delta * delta * delta;
            });

            var firstKurtosisCoefficient = (sum / (count * shiftedVariance * shiftedVariance)) - 3;

            SecondKurtosisCoefficient = (firstKurtosisCoefficient + 6.0 / (count + 1)) * ((count * count - 1) / ((count - 2) * (count - 3)));

            var studentQuantile = GetStudentDistributionQuantile(1 - _alpha / 2, count - 1);

            SecondKurtosisCoefficientSigma = GetKurtosisCoefficientRootMeanSquareDeviation(count);

            SecondKurtosisCoefficientTrustInterval = (
                SecondKurtosisCoefficient.Value - studentQuantile * SecondKurtosisCoefficientSigma.Value,
                SecondKurtosisCoefficient.Value + studentQuantile * SecondKurtosisCoefficientSigma.Value
                );
        }

        private static void ComputeMin(List<RowData> datas)
        {
            Min = datas.Min(data => data.VariantValue);
        }

        private static void ComputeMax(List<RowData> datas)
        {
            Max = datas.Max(data => data.VariantValue);
        }

        #region Computing methods
        private static double GetQuantileT(double a) => Math.Sqrt(-2 * Math.Log2(a));

        private static double GetQuantilePhi(double a)
        {
            var t = GetQuantileT(a);

            var numerator = _c0 + _c1 * t + _c2 * t * t;

            var denominator = 1 + _d1 * t + _d2 * t * t + _d3 * t * t * t;

            return t - numerator / denominator;
        }

        private static double GetNormalDistributionQuantile(double p)
        {
            return p <= 0.5
                ? -1 * GetQuantilePhi(p)
                : GetQuantilePhi(1 - p);
        }

        private static double GetG1(double uP) => (uP * uP * uP + uP) / 4;

        private static double GetG2(double uP) => (5 * Math.Pow(uP, 5) + 16 * Math.Pow(uP, 3) + 3 * uP) / 96;

        private static double GetG3(double uP) => (3 * Math.Pow(uP, 7) + 19 * Math.Pow(uP, 5) + 17 * Math.Pow(uP, 3) - 15 * uP) / 384;

        private static double GetG4(double uP) => (79 * Math.Pow(uP, 9) + 779 * Math.Pow(uP, 7) + 1482 * Math.Pow(uP, 5) - 1920 * Math.Pow(uP, 3) - 945 * uP) / 92160;

        private static double GetStudentDistributionQuantile(double p, double v)
        {
            var uP = GetNormalDistributionQuantile(p);

            var g1 = GetG1(uP);
            var g2 = GetG2(uP);
            var g3 = GetG3(uP);
            var g4 = GetG4(uP);

            return uP + (g1 / v) + (g2 / Math.Pow(uP, 2)) + (g3 / Math.Pow(uP, 3)) + (g4 / Math.Pow(uP, 4));
        }

        //Sigma
        private static double GetMeanSquaredStandardDeviation(double S, int N) => S / Math.Sqrt(N);

        private static double GetSampleMeanSquaredStandardDeviation(double S, int N) => S / Math.Sqrt(2.0 * N);

        private static double GetSkewnessCoefficientRootMeanSquareDeviation(int N) => Math.Sqrt(
            (6.0 * N * (N - 1))
            /
            (((double)N - 2) * (N + 1) * (N + 3))
            );

        private static double GetKurtosisCoefficientRootMeanSquareDeviation(int N) => Math.Sqrt(
            (24.0 * N * (N - 1) * (N - 1))
            /
            (((double)N - 3) * (N - 2) * (N + 3) * (N + 5))
            );
        #endregion
    }
}
