using System;
using System.CodeDom;
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

        #region Groupped data
        

        private void UpdateGroupedDataGrid()
        {
            var rowDatas = _datas.GetValue(typeof(RowData))?
                .Cast<RowData>()
                .OrderBy(data => data.VariantValue)
                .ToList();

            if (rowDatas == null)
                return;

            var groupedDatas = MainDataConverter.Handle<RowData, GroupedData>(rowDatas);

            _datas.AddPair(typeof(GroupedData), groupedDatas.ToGeneralDataList());

            FillGroupedDatasGrid();
        }

        private void FillGroupedDatasGrid()
        {
            var groupedDatas = _datas[typeof(GroupedData)].ToTemplateDataList<GroupedData>();

            GroupedDataGrid.Items.Clear();

            for (int i = 0; i < groupedDatas.Count; i++)
            {
                GroupedData? groupedData = groupedDatas[i];

                GroupedDataGrid.Items.Add(groupedData);
            }
        }

        private void UpdateAnomaliesPointsChart()
        {
            var rowDatas = _datas[typeof(RowData)].ToTemplateDataList<RowData>();

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

        private void Reset()
        {
            Characteristics.Reset();

            WindowsResponsible.HideWindow<CharacteristicsWindow>();

            HistogramChart.Plot.Clear();

            CumulativeProbabilityChart.Plot.Clear();

            CumulativeProbabilityChart.Refresh();

            ClassedDataGrid.Items.Clear();

            GroupedDataGrid.Items.Clear();

            AnomaliesPointsChart.Plot.Clear();
        }

        

        #endregion

        #region Classified data
        private void ClassifyData()
        {
            if (!_datas.ContainsKey(typeof(GroupedData)))
                throw new InvalidOperationException($"Для створення класифікованих даних необхідна наявність згрупованих даних!");

            var groupedDatasAmount = _datas[typeof(GroupedData)].Count;

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

            SetClassesAmount();
        }

        private void SetClassesAmount()
        {
            UpdateClassifiedDatas();
        }

        private void UpdateClassifiedDatas()
        {
            var parameters = new GroupedToClassifiedConverterParameters() { ClassesAmount = Characteristics.ClassesCount };

            var groupedDatas = _datas[typeof(GroupedData)].ToTemplateDataList<GroupedData>();

            List<ClassifiedData> classifiedDatas = MainDataConverter.Handle<GroupedData, ClassifiedData>(groupedDatas, parameters);
            
            _datas[typeof(ClassifiedData)] = classifiedDatas.ToGeneralDataList();

            FillClassifiedDatasGrid(classifiedDatas);

            UpdateBarChart();
        }

        private void FillClassifiedDatasGrid(List<ClassifiedData> classifiedDatas)
        {
            ClassedDataGrid.Items.Clear();

            foreach (var groupedData in classifiedDatas)
                ClassedDataGrid.Items.Add(groupedData);
        }
        #endregion

        #region Bar chart
        

        private void UpdateBarChart()
        {
            var classifiedDatas = _datas[typeof(ClassifiedData)]
                .Cast<ClassifiedData>()
                .OrderBy(data => data.Edges.Min)
                .ToList();

            var plot = HistogramChart.Plot;

            plot.Clear();

            var bar = plot.AddBar(
                values: classifiedDatas.Select(data => data.RelativeFrequency).ToArray(), 
                positions: classifiedDatas.Select(data => (data.Edges.Min + data.Edges.Max) / 2).ToArray()
                );

            var edges = classifiedDatas[0].Edges;

            bar.BarWidth = edges.Max - edges.Min;

            HistogramChart.Refresh();
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
            var plot = HistogramChart.Plot;

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
