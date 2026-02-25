using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DA_Lab_1
{
    public partial class MainWindow : Window
    {
        private void GenerateDataButtonClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(LambdaTextBox.Text, out double lambda) || Math.Abs(lambda) < double.Epsilon)
            {
                MessageBox.Show("Введіть коректне значення lambda (не 0)!");
                return;
            }

            var random = new Random();
            var rowDatas = new List<RowData>(10000);

            for (int i = 0; i < 10000; i++)
            {
                double epsilon = random.NextDouble();
                // Ensure epsilon is not 0 to avoid Log(0)
                if (epsilon == 0) epsilon = double.Epsilon;

                double x = -Math.Log(epsilon) / lambda;
                rowDatas.Add(new RowData() { VariantValue = x });
            }

            SetNewDatas(rowDatas);
        }

        private void ClassifyDataButtonClick(object sender, RoutedEventArgs e)
        {
            ClassifyData();
        }

        private void ComputeCharacteristicsButtonClick(object sender, RoutedEventArgs e)
        {
            var rowDatas = _datas[typeof(RowData)]?.ToTemplateDataList<RowData>();
            var groupedDatas = _datas[typeof(GroupedData)]?.ToTemplateDataList<GroupedData>();

            if (rowDatas == null || groupedDatas == null)
                throw new InvalidOperationException($"Для розрахунку характеристик завантажте дані!");

            var characteristicsWindow = (CharacteristicsWindow)WindowsResponsible.ShowWindow<CharacteristicsWindow>();

            characteristicsWindow.InitializeComponent(new List<RowData>(rowDatas), new List<GroupedData>(groupedDatas));
        }

        private void KParameterSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (KParameterTextBox?.Text != null)
                KParameterTextBox.Text = string.Format($"K={KParameterSlider.Value.ToFormattedString()}");
        }

        private void DeleteOutlieGroupedDataButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Ви впевнені, що хочете видалити аномальні значення?",
                "Видалення аномальних значень",
                MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.No)
                return;

            RemoveOutlierGroupedData();
        }

        private void BuildKernelDensityEstimationFunctionButtonClick(object sender, RoutedEventArgs e)
        {
            if (!WindowsResponsible.IsWindowOpened<CharacteristicsWindow>())
            {
                MessageBox.Show(
                    "Для побудови функції ядерної оцінки відкрийте вікно характеристик і натисніть ще раз!");
                return;
            }

            var parsed = double.TryParse(BandWidthTextBox.Text, out double bandwidth);

            if (!parsed)
            {
                MessageBox.Show(
                    "Ширину вікна або не було введено, або було введено некоректно, тому значення розраховано автоматично!");

                Characteristics.SetBandwidth(null);

                BandWidthTextBox.Text = Characteristics.Bandwidth.ToFormattedString();
            }
            else
            {
                Characteristics.SetBandwidth(bandwidth);
            }

            UpdateBarChartKdeFunction();
        }

        private void BuildCumulativeProbabilityFunctionButtonClick(object sender, RoutedEventArgs e)
        {
            var key = typeof(GroupedData);

            if (!_datas.ContainsKey(key))
            {
                MessageBox.Show(
                    "Для побудови графіку емпіричної функції розподілу необхідна наявність згрупованих даних!");
                return;
            }

            var groupedDatas = _datas[key]
                .ToTemplateDataList<GroupedData>()
                .OrderBy(data => data.EmpiricFunctionValue)
                .ToList();

            UpdateCumulativeProbabilityChart(groupedDatas);
        }

        private void FindAnomaliesButtonClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(KParameterTextBox.Text.Remove(0, 2), out double result))
                return;

            try
            {
                Characteristics.SetOutlieK(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при спробі призначення К: {ex.Message}");
                return;
            }

            FindOutlieGroupedData();
        }

        private void CheckExponentialLawButtonClick(object sender, RoutedEventArgs e)
        {
            var key = typeof(ClassifiedData);

            if (!_datas.ContainsKey(key))
            {
                MessageBox.Show("Для перевірки на експоненціальний закон необхідно виконати класифікацію даних!");
                return;
            }

            var classifiedDatas = _datas[key]
                .ToTemplateDataList<ClassifiedData>()
                .OrderBy(data => data.Edges.Min)
                .ToList();

            double mean = Characteristics.Mean;
            double lambda = 1.0 / mean;
            int n = Characteristics.Count;

            double chiSquareObserved = 0;

            foreach (var data in classifiedDatas)
            {
                double lower = data.Edges.Min;
                double upper = data.Edges.Max;

                // F(x) = 1 - e^(-lambda * x)
                // P_i = F(upper) - F(lower) = (1 - e^(-lambda * upper)) - (1 - e^(-lambda * lower))
                // P_i = e^(-lambda * lower) - e^(-lambda * upper)
                double p_i = Math.Exp(-lambda * lower) - Math.Exp(-lambda * upper);

                // If upper is effectively infinity (last class sometimes), handled naturally by integration limits, 
                // but here we use strict bin edges.
                // Note: Last bin theoretically goes to infinity for the distribution, but we use sample max.
                // For proper test, the last bin should statistically include everything > max, 
                // but usually P_i based on edges is close enough if distribution fits.

                double expectedFrequency = n * p_i;
                double observedFrequency = data.Frequency;

                if (expectedFrequency > 0)
                {
                    chiSquareObserved += Math.Pow(observedFrequency - expectedFrequency, 2) / expectedFrequency;
                }
            }

            // Degrees of freedom: k - 1 - number_of_estimated_parameters
            // We estimated lambda, so parameters = 1.
            // df = k - 2
            int k = classifiedDatas.Count;
            int df = k - 2;

            if (df <= 0)
            {
                MessageBox.Show($"Неможливо провести тест: замало класів (k={k}, df={df}). Потрібно мінімум 3 класи.");
                return;
            }

            // Critical value for alpha = 0.05
            // Approximation for df > 0: Wilson-Hilferty
            // Chi^2 = df * (1 - 2/(9*df) + z * sqrt(2/(9*df)))^3
            // z for 0.05 is 1.645

            double criticalValue = GetChiSquareCriticalValue(df);

            string resultMessage = chiSquareObserved < criticalValue
                ? $"Дані узгоджуються з експоненціальним законом розподілу.\nChi^2 сп. = {chiSquareObserved:F4} < Chi^2 кр. = {criticalValue:F4}"
                : $"Дані НЕ узгоджуються з експоненціальним законом розподілу.\nChi^2 сп. = {chiSquareObserved:F4} >= Chi^2 кр. = {criticalValue:F4}";

            MessageBox.Show(resultMessage, "Результат перевірки");
        }

        private double GetChiSquareCriticalValue(int df)
        {
            // Lookup for small df (alpha = 0.05)
            double[] lookup = new double[]
            {
                0, 3.841, 5.991, 7.815, 9.488, 11.070, 12.592, 14.067, 15.507, 16.919,
                18.307, 19.675, 21.026, 22.362, 23.685, 25.000, 26.296, 27.587, 28.869, 30.144, 31.410
            };

            if (df <= 20) return lookup[df];

            // Approximation for larger df
            // Wilson-Hilferty approximation for Chi-Square:
            // X^2 ~ df * (1 - 2/(9df) + z * sqrt(2/(9df)))^3
            // For alpha = 0.05, z_0.95 = 1.645
            double z = 1.645;
            double term = 2.0 / (9.0 * df);
            return df * Math.Pow(1 - term + z * Math.Sqrt(term), 3);
        }
    }
}
