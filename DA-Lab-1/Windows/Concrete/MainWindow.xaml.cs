using DA_Lab_1;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

            CreateGrouppedDataGrid();

            PrepareClassifiedDataGrid();

            PrepareBarChart();
        
            GrouppedDataGrid.IsReadOnly = true;
        }

        #region Groupped data
        private void CreateGrouppedDataGrid()
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

            FillGrouppedDatasGrid(groupedDatas);
        }

        private void FillGrouppedDatasGrid(List<GrouppedData> groupedDatas)
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
                throw new InvalidOperationException($"Для створення классифікованих данних необхідна наявність згрупованих даних!");

            var grouppedDatasAmount = _datas[typeof(GrouppedData)].Count;

            int amount = ExtentionsMethods.GetClassesAmount(grouppedDatasAmount);

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

            var grouppedDatas = _datas[typeof(GrouppedData)].ToTemplateDataList<GrouppedData>();

            List<ClassifiedData> classifiedDatas = MainDataConverter.Handle<GrouppedData, ClassifiedData>(grouppedDatas, parameters);
            
            _datas[typeof(ClassifiedData)] = classifiedDatas.ToGeneralDataList();

            FillClassifiedDatasGrid(classifiedDatas);

            UpdateBarChart();
        }

        private void FillClassifiedDatasGrid(List<ClassifiedData> classifiedDatas)
        {
            ClassedDataGrid.Items.Clear();

            foreach (var grouppedData in classifiedDatas)
                ClassedDataGrid.Items.Add(grouppedData);
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
            HistogramPlot.Configuration.Pan = false;
            HistogramPlot.Configuration.Zoom = false;

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

            var scottPlot = HistogramPlot.Plot;

            scottPlot.Clear();

            var bar = scottPlot.AddBar(
                values: classifiedDatas.Select(data => data.Frequency).ToArray(), 
                positions: classifiedDatas.Select(data => data.Edges.Min).ToArray()
                );

            var edges = classifiedDatas[0].Edges;

            bar.BarWidth = edges.Max - edges.Min;

            HistogramPlot.Refresh();
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
