using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();

            WindowsResponsible.Initialize(this);

            CreateGroupedDataGrid();

            PrepareClassifiedDataGrid();

            PrepareBarChart();
        
            GroupedDataGrid.IsReadOnly = true;
        }

        #region Groupped data
        private void CreateGroupedDataGrid()
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
        }

        private void UploadFileButtonClick(object sender, RoutedEventArgs e)
        {
            var rowDatas = DataLoader.LoadValues()?
                .Select(value => new RowData() { VariantValue = value })
                .OrderBy(rowData => rowData.VariantValue)
                .ToList();

            if (rowDatas == null)
            {
                MessageBox.Show("Список значень був пустий!");

                return;
            }

            _datas.AddPair(typeof(RowData), rowDatas.ToGeneralDataList());

            UpdateGroupedDataGrid();
        }

        private void UpdateGroupedDataGrid()
        {
            var rowDatas = _datas[typeof(RowData)].ToTemplateDataList<RowData>();

            Reset();

            Characteristics.SetDatas(rowDatas);

            var groupedDatas = MainDataConverter.Handle<RowData, GroupedData>(rowDatas);

            _datas.AddPair(typeof(GroupedData), groupedDatas.ToGeneralDataList());

            FillGroupedDatasGrid(groupedDatas);
        }

        private void FillGroupedDatasGrid(List<GroupedData> groupedDatas)
        {
            GroupedDataGrid.Items.Clear();

            for (int i = 0; i < groupedDatas.Count; i++)
            {
                GroupedData? groupedData = groupedDatas[i];

                GroupedDataGrid.Items.Add(groupedData);
            }
        }

        private void Reset()
        {
            Characteristics.Reset();

            WindowsResponsible.HideWindow<CharacteristicsWindow>();

            HistogramPlot.Plot.Clear();

            CumulativeProbabilityHistogram.Plot.Clear();

            CumulativeProbabilityHistogram.Refresh();

            ClassedDataGrid.Items.Clear();

            GroupedDataGrid.Items.Clear();
        }

        private void KParameterSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Characteristics.SetOutlieK(KParameterSlider.Value);

            if (KParameterTextBox?.Text != null)
            {
                KParameterTextBox.Text = string.Format($"K={Characteristics.OutlieK}");
                
                UpdateGroupedDataGrid();
            } 
        }

        #endregion

        #region Classified data
        private void PrepareClassifiedDataGrid()
        {
            ((DataGridTextColumn)ClassedDataGrid.Columns[0]).Binding = new Binding(nameof(ClassifiedData.ClassNum));
            ((DataGridTextColumn)ClassedDataGrid.Columns[1]).Binding = new Binding(nameof(ClassifiedData.FormattedEdges));
            ((DataGridTextColumn)ClassedDataGrid.Columns[2]).Binding = new Binding(nameof(ClassifiedData.Frequency));
            ((DataGridTextColumn)ClassedDataGrid.Columns[3]).Binding = new Binding(nameof(ClassifiedData.FormattedRelativeFrequency));
            ((DataGridTextColumn)ClassedDataGrid.Columns[4]).Binding = new Binding(nameof(ClassifiedData.FormattedEmpiricFunctionValue));
        }

        private void ClassifyDataButtonClick(object sender, RoutedEventArgs e)
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
        private void PrepareBarChart()
        {
            var plot = HistogramPlot.Plot;

            plot.Title("Гістограма і ядерна оцінка"); 
            plot.YAxis.Label("Відносна частота");
            plot.XAxis.Label("Значення");
        }

        private void UpdateBarChart()
        {
            var classifiedDatas = _datas[typeof(ClassifiedData)]
                .Cast<ClassifiedData>()
                .OrderBy(data => data.Edges.Min)
                .ToList();

            var plot = HistogramPlot.Plot;

            plot.Clear();

            var bar = plot.AddBar(
                values: classifiedDatas.Select(data => data.RelativeFrequency).ToArray(), 
                positions: classifiedDatas.Select(data => (data.Edges.Min + data.Edges.Max) / 2).ToArray()
                );

            var edges = classifiedDatas[0].Edges;

            bar.BarWidth = edges.Max - edges.Min;

            HistogramPlot.Refresh();
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

            var plot = HistogramPlot.Plot;

            var delta = (Characteristics.Max - Characteristics.Min) / KDEPointsAmount;

            var pointsColor = ExtensionsMethods.GetRandomColor();

            for (int i = 0; i < KDEPointsAmount + 1; i++)
            {
                var x = Characteristics.Min + i * delta;

                var y = Characteristics.GetKernelDensityEstimation(x);

                plot.AddPoint(x, y, pointsColor);
            }

            HistogramPlot.Refresh();
        }

        #endregion

        #region Comulative Probability Histogram
        private void BuildCumulativeProbabilityHistogramButtonClick(object sender, RoutedEventArgs e)
        {
            var plot = CumulativeProbabilityHistogram.Plot;

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
            plot.SetAxisLimits(yMin: 0, yMax: 1);
            plot.Title("Графік емпіричної функції розподілу");
            plot.XAxis.Label("Значення");
            plot.YAxis.Label("Вірогідність");

            CumulativeProbabilityHistogram.Refresh();
        }

        #endregion

        #region Characteristics
        private void ComputeCharacteristicsButtonClick(object sender, RoutedEventArgs e)
        {
            var rowDatas = _datas[typeof(RowData)]?.ToTemplateDataList<RowData>();

            if (rowDatas == null)
                throw new InvalidOperationException($"Для розрахунку характеристик завантажте дані!");

            var characteristicsWindow = (CharacteristicsWindow)WindowsResponsible.ShowWindow<CharacteristicsWindow>();

            characteristicsWindow.InitializeComponent(new List<RowData>(rowDatas));
        }
        #endregion
    }
}
