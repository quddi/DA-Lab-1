using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Binding = System.Windows.Data.Binding;

namespace DA_Lab_1
{
    public partial class MainWindow : Window
    {
        private Dictionary<Type, List<IData>> _datas = new();

        private const int MinClassifiedDatasAmount = 1;
        private const int KDEPointsAmount = 1000;
        private readonly Color OutliePointsColor = Color.Red;
        private readonly Color NormalPointsColor = Color.Black;
        private readonly Color OutlieEdgesLinesColor = Color.Purple;

        public MainWindow()
        {
            InitializeComponent();

            WindowsResponsible.Initialize(this);

            PrepareAll();
        }

        #region General
        private void Reset()
        {
            Characteristics.Reset();

            WindowsResponsible.HideWindow<CharacteristicsWindow>();

            ClassedDataGrid.Items.Clear();

            GroupedDataGrid.Items.Clear();

            BarChart.Plot.Clear();
            BarChart.Refresh();

            CumulativeProbabilityChart.Plot.Clear();
            CumulativeProbabilityChart.Refresh();

            AnomaliesPointsChart.Plot.Clear();
            AnomaliesPointsChart.Refresh();
        }

        private void SetNewDatas(List<RowData> rowDatas)
        {
            Reset();

            _datas.AddPair(typeof(RowData), rowDatas.ToGeneralDataList());

            Characteristics.SetDatas(rowDatas);

            GroupData();
        }

        private void GroupData()
        {
            var rowDatas = _datas.GetValue(typeof(RowData))?
                .Cast<RowData>()
                .OrderBy(data => data.VariantValue)
                .ToList();

            if (rowDatas == null)
                return;

            var groupedDatas = MainDataConverter.Handle<RowData, GroupedData>(rowDatas);

            _datas.AddPair(typeof(GroupedData), groupedDatas.ToGeneralDataList());

            VisualizeGroupedData(drawAnomaliesChart: false);
        }

        private void FindOutlieGroupedData()
        {
            var groupedDatas = _datas.GetValue(typeof(GroupedData))?.ToTemplateDataList<GroupedData>();

            if (groupedDatas == null) 
                return;

            foreach (var groupedData in groupedDatas)
            {
                var value = groupedData.VariantValue;

                groupedData.IsOutlier = value < Characteristics.DownOutlieEdge || value > Characteristics.UpOutlieEdge;
            }

            VisualizeGroupedData(drawAnomaliesChart: true);
        }

        private void RemoveOutlierGroupedData()
        {
            var refinedGroupedData = _datas.GetValue(typeof(GroupedData))?
                .Cast<GroupedData>()?
                .Where(data => !data.IsOutlier)?
                .ToList();

            if (refinedGroupedData == null) 
                return;

            _datas[typeof(GroupedData)] = refinedGroupedData.ToGeneralDataList();

            var oldRowDatas = _datas[typeof(RowData)].ToTemplateDataList<RowData>();

            var newRowDatas = oldRowDatas.Where(oldRowData =>
                refinedGroupedData.Any(
                    refinedGroupedData => oldRowData.VariantValue == refinedGroupedData.VariantValue
                    )
                );

            _datas[typeof(RowData)] = newRowDatas.ToGeneralDataList();

            Characteristics.SetDatas(newRowDatas.ToTemplateDataList<RowData>());
            VisualizeGroupedData(drawAnomaliesChart: true);
            ClassifyData();
            UpdateCumulativeProbabilityChart(refinedGroupedData);
            UpdateBarChartKdeFunction();
        }

        private void ClassifyData()
        {
            if (!_datas.ContainsKey(typeof(GroupedData)))
                throw new InvalidOperationException($"Для створення класифікованих даних необхідна наявність згрупованих даних!");

            var parsed = int.TryParse(ClassesAmountTextBox.Text, out int classesCount);

            var parsedIsValid = MinClassifiedDatasAmount < classesCount && classesCount < _datas[typeof(GroupedData)].Count;

            if (!parsed || !parsedIsValid)
            {
                MessageBox.Show("Кількість класів або не було введено, або було введено некоректно, тому значення розраховано автоматично!");

                classesCount = Characteristics.ClassesCount;

                ClassesAmountTextBox.Text = classesCount.ToString();
            }
            else
            {
                Characteristics.SetClassesCount(classesCount);
            }

            var parameters = new GroupedToClassifiedConverterParameters() { ClassesAmount = Characteristics.ClassesCount };

            var groupedDatas = _datas[typeof(GroupedData)].ToTemplateDataList<GroupedData>();

            List<ClassifiedData> classifiedDatas = MainDataConverter.Handle<GroupedData, ClassifiedData>(groupedDatas, parameters);

            _datas[typeof(ClassifiedData)] = classifiedDatas.ToGeneralDataList();

            VisualizeClassifiedData();
        }
        #endregion

        #region Visualization
        private void VisualizeGroupedData(bool drawAnomaliesChart)
        {
            var groupedDatas = _datas[typeof(GroupedData)].ToTemplateDataList<GroupedData>();

            UpdateGroupedDatasGrid(groupedDatas);

            if (drawAnomaliesChart)
                UpdateAnomaliesPointsChart(_datas[typeof(RowData)].ToTemplateDataList<RowData>());
        }

        private void VisualizeClassifiedData()
        {
            var classifiedDatas = _datas[typeof(ClassifiedData)]
                .Cast<ClassifiedData>()
                .OrderBy(data => data.Edges.Min)
                .ToList();

            UpdateClassifiedDatasGrid(classifiedDatas);

            UpdateBarChart(classifiedDatas);
        }
        #endregion

        #region Updations
        private void UpdateGroupedDatasGrid(List<GroupedData> groupedDatas)
        {
            GroupedDataGrid.Items.Clear();

            for (int i = 0; i < groupedDatas.Count; i++)
            {
                GroupedData? groupedData = groupedDatas[i];

                GroupedDataGrid.Items.Add(groupedData);
            }
        }

        private void UpdateBarChart(List<ClassifiedData> classifiedDatas)
        {
            var plot = BarChart.Plot;

            plot.Clear();

            var bar = plot.AddBar(
                values: classifiedDatas.Select(data => data.RelativeFrequency).ToArray(),
                positions: classifiedDatas.Select(data => (data.Edges.Min + data.Edges.Max) / 2).ToArray()
                );

            var edges = classifiedDatas[0].Edges;

            bar.BarWidth = edges.Max - edges.Min;

            BarChart.Refresh();
        }

        private void UpdateClassifiedDatasGrid(List<ClassifiedData> classifiedDatas)
        {
            ClassedDataGrid.Items.Clear();

            foreach (var groupedData in classifiedDatas)
                ClassedDataGrid.Items.Add(groupedData);
        }

        private void UpdateAnomaliesPointsChart(List<RowData> rowDatas)
        {
            var plot = AnomaliesPointsChart.Plot;

            plot.Clear();

            for (int i = 0; i < rowDatas.Count; i++)
            {
                var pointValue = rowDatas[i].VariantValue;

                var pointColor = Characteristics.DownOutlieEdge < pointValue && pointValue < Characteristics.UpOutlieEdge
                    ? NormalPointsColor : OutliePointsColor;

                plot.AddPoint(i, pointValue, pointColor);
            }

            plot.AddFunction(x => Characteristics.DownOutlieEdge, OutlieEdgesLinesColor);
            plot.AddFunction(x => Characteristics.UpOutlieEdge, OutlieEdgesLinesColor);

            AnomaliesPointsChart.Refresh();
        }

        private void UpdateBarChartKdeFunction()
        {
            var plot = BarChart.Plot;

            var delta = (Characteristics.Max - Characteristics.Min) / KDEPointsAmount;

            var pointsColor = ExtensionsMethods.GetRandomColor();

            for (int i = 0; i < KDEPointsAmount + 1; i++)
            {
                var x = Characteristics.Min + i * delta;

                var y = Characteristics.GetKernelDensityEstimation(x);

                plot.AddPoint(x, y, pointsColor);
            }

            BarChart.Refresh();
        }

        private void UpdateCumulativeProbabilityChart(List<GroupedData> groupedDatas)
        {
            var plot = CumulativeProbabilityChart.Plot;

            plot.Clear();

            var xs = groupedDatas.Select(data => data.VariantValue).ToArray();
            var ys = groupedDatas.Select(data => data.EmpiricFunctionValue).ToArray();

            plot.AddScatterStep(xs, ys);

            CumulativeProbabilityChart.Refresh();
        }
        #endregion

        #region Preparations
        private void PrepareAll()
        {
            PrepareGroupedDataGrid();
            PrepareClassifiedDataGrid();
            PrepareAnomaliesPointsChart();
            PrepareBarChart();
            PrepareCumulativeProbabilityChart();
        }

        private void PrepareGroupedDataGrid()
        {
            GroupedDataGrid.Columns.Clear();

            DataGridTextColumn variantNumColumn = new DataGridTextColumn()
            {
                Header = "№",
                Binding = new Binding(nameof(GroupedData.VariantNum)),
                Width = 30
            };

            DataGridTextColumn variantValueColumn = new DataGridTextColumn()
            {
                Header = "Значення",
                Binding = new Binding(nameof(GroupedData.VariantValue)),
                Width = 70
            };

            DataGridTextColumn frequencyColumn = new DataGridTextColumn()
            {
                Header = "Частота",
                Binding = new Binding(nameof(GroupedData.Frequency)),
                Width = 60
            };

            DataGridTextColumn relativeFrequencyColumn = new DataGridTextColumn()
            {
                Header = "Відносна частота",
                Binding = new Binding(nameof(GroupedData.FormattedRelativeFrequency)),
                Width = 110
            };

            DataGridTextColumn empFunctionValueColumn = new DataGridTextColumn()
            {
                Header = "Значення емп. ф-ї",
                Binding = new Binding(nameof(GroupedData.FormattedEmpiricFunctionValue)),
                Width = 110
            };

            DataGridCheckBoxColumn isOutlieColumn = new DataGridCheckBoxColumn()
            {
                Header = "Аномальне",
                Binding = new Binding(nameof(GroupedData.IsOutlier)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            GroupedDataGrid.Columns.Add(variantNumColumn);
            GroupedDataGrid.Columns.Add(variantValueColumn);
            GroupedDataGrid.Columns.Add(frequencyColumn);
            GroupedDataGrid.Columns.Add(relativeFrequencyColumn);
            GroupedDataGrid.Columns.Add(empFunctionValueColumn);
            GroupedDataGrid.Columns.Add(isOutlieColumn);

            GroupedDataGrid.IsReadOnly = true;
        }

        private void PrepareClassifiedDataGrid()
        {
            ((DataGridTextColumn)ClassedDataGrid.Columns[0]).Binding = new Binding(nameof(ClassifiedData.ClassNum));
            ((DataGridTextColumn)ClassedDataGrid.Columns[1]).Binding = new Binding(nameof(ClassifiedData.FormattedEdges));
            ((DataGridTextColumn)ClassedDataGrid.Columns[2]).Binding = new Binding(nameof(ClassifiedData.Frequency));
            ((DataGridTextColumn)ClassedDataGrid.Columns[3]).Binding = new Binding(nameof(ClassifiedData.FormattedRelativeFrequency));
            ((DataGridTextColumn)ClassedDataGrid.Columns[4]).Binding = new Binding(nameof(ClassifiedData.FormattedEmpiricFunctionValue));

            ClassedDataGrid.IsReadOnly = true;
        }

        private void PrepareAnomaliesPointsChart()
        {
            var plot = AnomaliesPointsChart.Plot;

            plot.Title("Аномальні значення");
            plot.YAxis.Label("Значення елементу");
            plot.XAxis.Label("Індекс елементу");
        }

        private void PrepareBarChart()
        {
            var plot = BarChart.Plot;

            plot.Title("Гістограма і ядерна оцінка");
            plot.YAxis.Label("Відносна частота");
            plot.XAxis.Label("Значення");
        }

        private void PrepareCumulativeProbabilityChart()
        {
            var plot = CumulativeProbabilityChart.Plot;

            plot.SetAxisLimits(yMin: 0, yMax: 1);
            plot.Title("Графік емпіричної функції розподілу");
            plot.XAxis.Label("Значення");
            plot.YAxis.Label("Вірогідність");
        }
        #endregion
    }
}
