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

        private int _classesAmount;

        private const int MinClassifiedDatasAmount = 1;

        public MainWindow()
        {
            InitializeComponent();

            CreateGroupedDataGrid();

            PrepareClassifiedDataGrid();

            PrepareBarChart();
        
            GrouppedDataGrid.IsReadOnly = true;
        }

        #region Groupped data
        private void CreateGroupedDataGrid()
        {
            GrouppedDataGrid.Columns.Clear();

            DataGridTextColumn variantNumColumn = new DataGridTextColumn()
            {
                Header = "№",
                Binding = new Binding(nameof(GrouppedData.VariantNum)),
                Width = 30
            };

            DataGridTextColumn variantValueColumn = new DataGridTextColumn()
            {
                Header = "Значення",
                Binding = new Binding(nameof(GrouppedData.VariantValue)),
                Width = 70
            };

            DataGridTextColumn frequencyColumn = new DataGridTextColumn() 
            { 
                Header = "Частота",
                Binding = new Binding(nameof(GrouppedData.Frequency)),
                Width = 60
            };

            DataGridTextColumn relativeFrequencyColumn = new DataGridTextColumn() 
            { 
                Header = "Відносна частота",
                Binding = new Binding(nameof(GrouppedData.FormattedRelativeFrequency)),
                Width = 110
            };

            DataGridTextColumn empFunctionValueColumn = new DataGridTextColumn() 
            { 
                Header = "Значення емп. функції",
                Binding = new Binding(nameof(GrouppedData.FormattedEmpericFunctionValue)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            GrouppedDataGrid.Columns.Add(variantNumColumn);
            GrouppedDataGrid.Columns.Add(variantValueColumn);
            GrouppedDataGrid.Columns.Add(frequencyColumn);
            GrouppedDataGrid.Columns.Add(relativeFrequencyColumn);
            GrouppedDataGrid.Columns.Add(empFunctionValueColumn);
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

            var groupedDatas = MainDataConverter.Handle<RowData, GrouppedData>(rowDatas);

            _datas.AddPair(typeof(GrouppedData), groupedDatas.ToGeneralDataList());

            FillGroupedDatasGrid(groupedDatas);
        }

        private void FillGroupedDatasGrid(List<GrouppedData> groupedDatas)
        {
            GrouppedDataGrid.Items.Clear();

            foreach (var grouppedData in groupedDatas)
                GrouppedDataGrid.Items.Add(grouppedData);
        }
        #endregion

        #region Classified data
        private void PrepareClassifiedDataGrid()
        {
            ((DataGridTextColumn)ClassedDataGrid.Columns[0]).Binding = new Binding(nameof(ClassifiedData.ClassNum));
            ((DataGridTextColumn)ClassedDataGrid.Columns[1]).Binding = new Binding(nameof(ClassifiedData.FormattedEdges));
            ((DataGridTextColumn)ClassedDataGrid.Columns[2]).Binding = new Binding(nameof(ClassifiedData.Frequency));
            ((DataGridTextColumn)ClassedDataGrid.Columns[3]).Binding = new Binding(nameof(ClassifiedData.FormattedRelativeFrequency));
            ((DataGridTextColumn)ClassedDataGrid.Columns[4]).Binding = new Binding(nameof(ClassifiedData.FormattedEmpericFunctionValue));
        }

        private void ClassifyDataButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_datas.ContainsKey(typeof(GrouppedData)))
                throw new InvalidOperationException($"Для створення класифікованих даних необхідна наявність згрупованих даних!");

            var groupedDatasAmount = _datas[typeof(GrouppedData)].Count;

            int amount = ExtentionsMethods.GetClassesAmount(groupedDatasAmount);

            ClassesAmountTextBox.Text = "";
            ClassesAmountTextBox.Text = amount.ToString();
        }

        private void SetClassesAmount(int amount)
        {
            _classesAmount = amount;

            UpdateClassifiedDatas();
        }

        private void UpdateClassifiedDatas()
        {
            var parameters = new GrouppedToClassifiedConverterParameters() { ClassesAmount = _classesAmount };

            var groupedDatas = _datas[typeof(GrouppedData)].ToTemplateDataList<GrouppedData>();

            List<ClassifiedData> classifiedDatas = MainDataConverter.Handle<GrouppedData, ClassifiedData>(groupedDatas, parameters);
            
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

        private void ClassesAmountTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(ClassesAmountTextBox.Text, out int classesAmount)) return;

            if (MinClassifiedDatasAmount > classesAmount || classesAmount > _datas[typeof(GrouppedData)].Count) return;
        
            SetClassesAmount(classesAmount);
        }
        #endregion

        #region Bar chart
        private void PrepareBarChart()
        {
            var plot = HistogramPlot.Plot;

            plot.Title("Гістограма і ядерна оцінка"); 
            plot.YAxis.Label("Кількість");
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
                values: classifiedDatas.Select(data => data.Frequency).ToArray(), 
                positions: classifiedDatas.Select(data => data.Edges.Min).ToArray()
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

            var plot = HistogramPlot.Plot;

            //plot.AddFunction(GetKernelDensityEstimation);

            var characteristicsWindow = WindowsResponsible.GetWindow<CharacteristicsWindow>() as CharacteristicsWindow;

            var min = characteristicsWindow.Min;
            var max = characteristicsWindow.Max;

            var pointsCount = 1000;

            var delta = (max - min) / pointsCount;

            for (int i = 0; i < pointsCount + 1; i++)
            {
                var x = min + i * delta;

                var y = GetKernelDensityEstimation(x);

                plot.AddPoint(x, y, Color.Red);
            }

            HistogramPlot.Refresh();
        }

        #endregion

        #region Comulative Probability Histogram
        private void BuildCumulativeProbabilityHistogramButtonClick(object sender, RoutedEventArgs e)
        {
            var plot = CumulativeProbabilityHistogram.Plot;

            var key = typeof(GrouppedData);

            if (!_datas.ContainsKey(key))
            {
                MessageBox.Show("Для побудови графіку емпіричної функції розподілу необхідна наявність сгрупованих даних!");
                return;
            }

            plot.Clear();

            var groupedDatas = _datas[key]
                .ToTemplateDataList<GrouppedData>()
                .OrderBy(data => data.EmpericFunctionValue);

            var xs = groupedDatas.Select(data => data.VariantValue).ToArray();
            var ys = groupedDatas.Select(data => data.EmpericFunctionValue).ToArray();

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

        #region Computing methods

        private double GetBandwidthByScott(double S, int N) => S / Math.Pow(N, 0.2);

        private double GetEpanchikovCore(double u)
        {
            if (Math.Abs(u) > Math.Sqrt(5.0)) return 0;

            return ((1.0 - u * u / 5.0) * 3) / (4 * Math.Sqrt(5.0));
        }

        private double GetGaussCore(double u)
        {
            return Math.Exp(-1.0 * u * u / 2) / Math.Sqrt(2 * Math.PI);
        }

        private double GetKernelDensityEstimation(double x)
        {
            var rowDatas = _datas[typeof(RowData)].ToTemplateDataList<RowData>();

            var N = rowDatas.Count;

            var characteristicsWindow = WindowsResponsible.GetWindow<CharacteristicsWindow>() as CharacteristicsWindow;

            var S = characteristicsWindow.StandardDeviation;

            var bandwidth = GetBandwidthByScott(S, N);

            var denominator = N * bandwidth;

            var sum = rowDatas.Select(data => 
            {
                var delta = (x - data.VariantValue);

                var u = delta / bandwidth;

                var result = GetGaussCore(u);

                return result;
            }).Sum();
        
            return sum / denominator;
        }

        #endregion        
    }
}
