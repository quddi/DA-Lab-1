using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DA_Lab_1
{
    public partial class MainWindow : Window
    {
        private void UploadFileButtonClick(object sender, RoutedEventArgs e)
        {
            var rowDatas = DataLoader.LoadValues()?
                .Select(value => new RowData() { VariantValue = value })
                .ToList();

            if (rowDatas == null)
            {
                MessageBox.Show("Список значень був пустий!");

                return;
            }

            Reset();

            _datas.AddPair(typeof(RowData), rowDatas.ToGeneralDataList());

            Characteristics.SetDatas(rowDatas);

            UpdateAnomaliesPointsChart();

            UpdateGroupedDataGrid();
        }

        private void KParameterSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Characteristics.SetOutlieK(KParameterSlider.Value);

            if (KParameterTextBox?.Text != null)
            {
                KParameterTextBox.Text = string.Format($"K={Characteristics.OutlieK.ToFormattedString()}");

                UpdateGroupedDataGrid();
            }
        }

        private void DeleteOutlieGroupedDataButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Ви впевнені, що хочете видалити аномальні значення?",
                "Видалення аномальних значень",
                MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.No)
                return;

            var groupedDatas = _datas[typeof(GroupedData)]
                .Cast<GroupedData>()
                .Where(data => !data.IsOutlier)
                .ToGeneralDataList();

            _datas[typeof(GroupedData)] = groupedDatas;

            FillGroupedDatasGrid();
            ClassifyData();
            UpdateAnomaliesPointsChart();
        }

        private void InputKParameterButtonClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(KParameterTextBox.Text, out double result))
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

            KParameterSlider.Value = result;
            UpdateGroupedDataGrid();
        }

        private void ClassifyDataButtonClick(object sender, RoutedEventArgs e)
        {
            ClassifyData();
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

            var plot = HistogramChart.Plot;

            var delta = (Characteristics.Max - Characteristics.Min) / KDEPointsAmount;

            var pointsColor = ExtensionsMethods.GetRandomColor();

            for (int i = 0; i < KDEPointsAmount + 1; i++)
            {
                var x = Characteristics.Min + i * delta;

                var y = Characteristics.GetKernelDensityEstimation(x);

                plot.AddPoint(x, y, pointsColor);
            }

            HistogramChart.Refresh();
        }

        private void BuildCumulativeProbabilityFunctionButtonClick(object sender, RoutedEventArgs e)
        {
            var plot = CumulativeProbabilityChart.Plot;

            var key = typeof(GroupedData);

            if (!_datas.ContainsKey(key))
            {
                MessageBox.Show("Для побудови графіку емпіричної функції розподілу необхідна наявність сгрупованих даних!");
                return;
            }

            plot.Clear();

            var groupedDatas = _datas[key]
                .ToTemplateDataList<GroupedData>()
                .OrderBy(data => data.EmpiricFunctionValue);

            var xs = groupedDatas.Select(data => data.VariantValue).ToArray();
            var ys = groupedDatas.Select(data => data.EmpiricFunctionValue).ToArray();

            plot.AddScatterStep(xs, ys);

            CumulativeProbabilityChart.Refresh();
        }

        private void ComputeCharacteristicsButtonClick(object sender, RoutedEventArgs e)
        {
            var rowDatas = _datas[typeof(RowData)]?.ToTemplateDataList<RowData>();

            if (rowDatas == null)
                throw new InvalidOperationException($"Для розрахунку характеристик завантажте дані!");

            var characteristicsWindow = (CharacteristicsWindow)WindowsResponsible.ShowWindow<CharacteristicsWindow>();

            characteristicsWindow.InitializeComponent(new List<RowData>(rowDatas));
        }
    }
}
