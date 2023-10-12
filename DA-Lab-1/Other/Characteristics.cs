using System.Windows.Markup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    public static class Characteristics
    {
        private static List<RowData>? _datas;

        private static double? _mean;
        private static double? _meanSigma;
        private static (double LeftEdge, double RightEdge)? _meanTrustInterval;

        private static double? _median;
        private static (double LeftEdge, double RightEdge)? _medianTrustInterval;

        private static double? _standardDeviation;  
        private static double? _standardDeviationSigma;
        private static (double LeftEdge, double RightEdge)? _standardDeviationTrustInterval;

        private static double? _secondSkewnessCoefficient;
        private static double? _secondSkewnessCoefficientSigma;
        private static (double LeftEdge, double RightEdge)? _secondSkewnessCoefficientTrustInterval;

        private static double? _secondKurtosisCoefficient;
        private static double? _secondKurtosisCoefficientSigma;
        private static (double LeftEdge, double RightEdge)? _secondKurtosisCoefficientTrustInterval;

        private static double? _variance;
        private static double? _min;
        private static double? _max;
        private static double? _studentQuantile;
        private static double? _shiftedVariance;
        private static double? _firstSkewnessCoefficient;
        private static double? _firstKurtosisCoefficient;
        private static double? _bandwidth;
        private static int? _count;

        public static double Mean
        {
            get
            {
                if (_mean == null) ComputeMean();

                return _mean.Value;
            }
        }
        public static double MeanSigma
        {
            get
            {
                if (_meanSigma == null) ComputeMean();

                return _meanSigma.Value;
            }
        }
        public static (double LeftEdge, double RightEdge) MeanTrustInterval
        {
            get
            {
                if (_meanTrustInterval == null) ComputeMean();

                return _meanTrustInterval.Value;
            }
        }

        public static double Median
        {
            get
            {
                if (_median == null) ComputeMedian();

                return _median.Value;
            }
        }
        public static (double LeftEdge, double RightEdge) MedianTrustInterval
        {
            get
            {
                if (_medianTrustInterval == null) ComputeMedian();

                return _medianTrustInterval.Value;
            }
        }

        public static double StandardDeviation
        {
            get
            {
                if (_standardDeviation == null) ComputeStandardDeviation();

                return _standardDeviation.Value;
            }
        }
        public static double StandardDeviationSigma
        {
            get
            {
                if (_standardDeviationSigma == null) ComputeStandardDeviation();

                return _standardDeviationSigma.Value;
            }
        }
        public static (double LeftEdge, double RightEdge) StandardDeviationTrustInterval
        {
            get
            {
                if (_standardDeviationTrustInterval == null) ComputeStandardDeviation();

                return _standardDeviationTrustInterval.Value;
            }
        }

        public static double SecondSkewnessCoefficient
        {
            get
            {
                if (_secondSkewnessCoefficient == null) ComputeSecondSkewnessCoefficient();

                return _secondSkewnessCoefficient.Value;
            }
        }
        public static double SecondSkewnessCoefficientSigma
        {
            get
            {
                if (_secondSkewnessCoefficientSigma == null) ComputeSecondSkewnessCoefficient();

                return _secondSkewnessCoefficientSigma.Value;
            }
        }
        public static (double LeftEdge, double RightEdge) SecondSkewnessCoefficientTrustInterval
        {
            get
            {
                if (_secondSkewnessCoefficientTrustInterval == null) ComputeSecondSkewnessCoefficient();

                return _secondSkewnessCoefficientTrustInterval.Value;
            }
        }

        public static double SecondKurtosisCoefficient
        {
            get
            {
                if (_secondKurtosisCoefficient == null) ComputeSecondKurtosisCoefficient();

                return _secondKurtosisCoefficient.Value;
            }
        }
        public static double SecondKurtosisCoefficientSigma
        {
            get
            {
                if (_secondKurtosisCoefficientSigma == null) ComputeSecondKurtosisCoefficient();

                return _secondKurtosisCoefficientSigma.Value;
            }
        }
        public static (double LeftEdge, double RightEdge) SecondKurtosisCoefficientTrustInterval
        {
            get
            {
                if (_secondKurtosisCoefficientTrustInterval == null) ComputeSecondKurtosisCoefficient();

                return _secondKurtosisCoefficientTrustInterval.Value;
            }
        }

        public static double Variance
        {
            get
            {
                if (_variance == null) ComputeVariance();

                return _variance.Value;
            }
        }

        public static double Min
        {
            get
            {
                if (_min == null) ComputeMin();

                return _min.Value;
            }
        }

        public static double Max
        {
            get
            {
                if (_max == null) ComputeMax();

                return _max.Value;
            }
        }

        public static int Count
        {
            get
            {
                if (_count == null) ComputeCount();

                return _count.Value;
            }
        }

        public static double StudentQuantile
        {
            get
            {
                if (_studentQuantile == null) ComputeStudentQuantile();

                return _studentQuantile.Value;
            }
        }

        public static double ShiftedVariance
        {
            get
            {
                if (_shiftedVariance == null) ComputeShiftedVariance();

                return _shiftedVariance.Value;
            }
        }

        public static double FirstSkewnessCoefficient
        {
            get
            {
                if (_firstSkewnessCoefficient == null) ComputeFirstSkewnessCoefficient();

                return _firstSkewnessCoefficient.Value;
            }
        }

        public static double FirstKurtosisCoefficient
        {
            get
            {
                if (_firstKurtosisCoefficient == null) ComputeFirstKurtosisCoefficient();

                return _firstKurtosisCoefficient.Value;
            }
        }

        public static double Bandwidth
        {
            get
            {
                if (_bandwidth == null) ComputeBandwidth();

                return _bandwidth.Value;
            }
        }

        private const double Alpha = 0.05;
        private const double C0 = 2.515517;
        private const double C1 = 0.802853;
        private const double C2 = 0.010328;
        private const double D1 = 1.432788;
        private const double D2 = 0.1892659;
        private const double D3 = 0.001308;

        public static void SetDatas(List<RowData>? datas)
        {
            _datas = datas;
        }

        public static void Reset()
        {
            _datas = null;
            _mean = null;
            _meanSigma = null;
            _meanTrustInterval = null;
            _median = null;
            _medianTrustInterval = null;
            _standardDeviation = null;
            _standardDeviationSigma = null;
            _standardDeviationTrustInterval = null;
            _secondSkewnessCoefficient = null;
            _secondSkewnessCoefficientSigma = null;
            _secondSkewnessCoefficientTrustInterval = null;
            _secondKurtosisCoefficient = null;
            _secondKurtosisCoefficientSigma = null;
            _secondKurtosisCoefficientTrustInterval = null;
            _variance = null;
            _min = null;
            _max = null;
            _studentQuantile = null;
            _shiftedVariance = null;
            _firstSkewnessCoefficient = null;
            _firstKurtosisCoefficient = null;
            _bandwidth = null;
            _count = null;
        }

        #region Computing methods

        private static void ComputeMean()
        {
            _mean = _datas.Average(data => data.VariantValue);

            _meanSigma = GetMeanSquaredStandardDeviation(StandardDeviation, Count);

            _meanTrustInterval = (Mean - StudentQuantile * MeanSigma, Mean + StudentQuantile * MeanSigma);
        }

        private static void ComputeMedian()
        {
            var rowDatas = _datas?
                .OrderBy(data => data.VariantValue)?
                .ToList();

            _median = Count % 2 == 0
                ? (rowDatas[Count / 2].VariantValue + rowDatas[Count / 2 - 1].VariantValue) / 2f
                : rowDatas[Count / 2].VariantValue;

            var uP = GetNormalDistributionQuantile(1 - Alpha / 2);

            var j = (int)(Count / 2 - uP * Math.Sqrt(Count) / 2);
            var k = (int)(Count / 2 + 1 + uP * Math.Sqrt(Count) / 2);

            _medianTrustInterval = (rowDatas[j].VariantValue, rowDatas[k].VariantValue);
        }

        private static void ComputeStandardDeviation()
        {
            _standardDeviation = Math.Sqrt(Variance);

            _standardDeviationSigma = GetSampleMeanSquaredStandardDeviation(StandardDeviation, Count);

            _standardDeviationTrustInterval = (
                StandardDeviation - StudentQuantile * StandardDeviationSigma, 
                StandardDeviation + StudentQuantile * StandardDeviationSigma);
        }

        private static void ComputeFirstSkewnessCoefficient()
        {
            var sum = _datas.Sum(data =>
            {
                var delta = data.VariantValue - Mean;

                return delta * delta * delta;
            });

            var shiftedVarianceSqrt = Math.Sqrt(ShiftedVariance);

            _firstSkewnessCoefficient = sum / (Count * shiftedVarianceSqrt * shiftedVarianceSqrt * shiftedVarianceSqrt);
        }

        private static void ComputeSecondSkewnessCoefficient()
        {
            _secondSkewnessCoefficient = (FirstSkewnessCoefficient * Math.Sqrt(Count * (Count - 1)) / (Count - 2));

            _secondSkewnessCoefficientSigma = GetSkewnessCoefficientRootMeanSquareDeviation(Count);

            _secondSkewnessCoefficientTrustInterval = (
                SecondSkewnessCoefficient - StudentQuantile * SecondSkewnessCoefficientSigma, 
                SecondSkewnessCoefficient + StudentQuantile * SecondSkewnessCoefficientSigma);
        }

        private static void ComputeFirstKurtosisCoefficient()
        {
            var sum = _datas.Sum(data =>
            {
                var delta = data.VariantValue - Mean;
                return delta * delta * delta * delta;
            });

            _firstKurtosisCoefficient = (sum / (Count * ShiftedVariance * ShiftedVariance)) - 3;
        }

        private static void ComputeSecondKurtosisCoefficient()
        {
            _secondKurtosisCoefficient = (FirstKurtosisCoefficient + 6.0 / (Count + 1)) * ((Count * Count - 1) / ((Count - 2) * (Count - 3)));

            _secondKurtosisCoefficientSigma = GetKurtosisCoefficientRootMeanSquareDeviation(Count);

            _secondKurtosisCoefficientTrustInterval = (
                SecondKurtosisCoefficient - StudentQuantile * SecondKurtosisCoefficientSigma,
                SecondKurtosisCoefficient + StudentQuantile * SecondKurtosisCoefficientSigma);
        }

        private static void ComputeMin()
        {
            _min = _datas.Min(data => data.VariantValue);
        }

        private static void ComputeMax()
        {
            _max = _datas.Max(data => data.VariantValue);
        }

        private static void ComputeVariance()
        {
            var sum = _datas.Sum(data =>
            {
                var delta = data.VariantValue - _mean;
                return delta * delta;
            });

            _variance = sum / (_datas.Count - 1); //S^2
        }

        private static void ComputeCount()
        {
            _count = _datas.Count;
        }

        private static void ComputeStudentQuantile()
        {
            _studentQuantile = GetStudentDistributionQuantile(1 - Alpha / 2, Count - 1);
        }

        private static void ComputeShiftedVariance()
        {
            _shiftedVariance = Variance * (Count - 1) / Count;
        }

        private static void ComputeBandwidth()
        {
            _bandwidth = StandardDeviation / Math.Pow(Count, 0.2);
        }

        public static double GetGaussCore(double u)
        {
            return Math.Exp(-1.0 * u * u / 2) / Math.Sqrt(2 * Math.PI);
        }

        public static double GetKernelDensityEstimation(double x)
        {
            var rowDatas = _datas.ToTemplateDataList<RowData>();

            var characteristicsWindow = WindowsResponsible.GetWindow<CharacteristicsWindow>() as CharacteristicsWindow;

            var denominator = Count * Bandwidth;

            var sum = rowDatas.Select(data =>
            {
                var delta = (x - data.VariantValue);

                var u = delta / Bandwidth;

                var result = GetGaussCore(u);

                return result;
            }).Sum();

            return sum / denominator;
        }

        private static double GetQuantileT(double a) => Math.Sqrt(-2 * Math.Log2(a));

        private static double GetQuantilePhi(double a)
        {
            var t = GetQuantileT(a);

            var numerator = C0 + C1 * t + C2 * t * t;

            var denominator = 1 + D1 * t + D2 * t * t + D3 * t * t * t;

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
