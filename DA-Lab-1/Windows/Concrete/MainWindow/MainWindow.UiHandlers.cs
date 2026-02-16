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
                MessageBox.Show("Для побудови функції ядерної оцінки відкрийте вікно характеристик і натисніть ще раз!");
                return;
            }

            var parsed = double.TryParse(BandWidthTextBox.Text, out double bandwidth);

            if (!parsed)
            {
                MessageBox.Show("Ширину вікна або не було введено, або було введено некоректно, тому значення розраховано автоматично!");

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
                MessageBox.Show("Для побудови графіку емпіричної функції розподілу необхідна наявність згрупованих даних!");
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
    }
}
