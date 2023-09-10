using DA_Lab_1.DataConverters;
using DA_Lab_1.DTO.Base;
using DA_Lab_1.DTO.Specifics;
using DA_Lab_1.Extentions;
using DA_Lab_1.Other;
using DA_Lab_1.Specifics.DataHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DA_Lab_1
{
    public partial class MainWindow : Window
    {
        private Dictionary<Type, List<IData>> _datas = new();

        public MainWindow()
        {
            InitializeComponent();

            CreateGrouppedDataGrid();
        
            FirstDataGrid.IsReadOnly = true;
        }

        private void CreateGrouppedDataGrid()
        {
            FirstDataGrid.Columns.Clear();

            DataGridTextColumn variantNumColumn = new DataGridTextColumn()
            {
                Header = "№ Варіанти",
                Binding = new Binding(nameof(GrouppedData.VariantNum)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn variantValueColumn = new DataGridTextColumn()
            {
                Header = "Значення варіанти",
                Binding = new Binding(nameof(GrouppedData.VariantValue)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn frequencyColumn = new DataGridTextColumn() 
            { 
                Header = "Частота",
                Binding = new Binding(nameof(GrouppedData.Frequency)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn relativeFrequencyColumn = new DataGridTextColumn() 
            { 
                Header = "Відносна частота",
                Binding = new Binding(nameof(GrouppedData.RelativeFrequency)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn empFunctionValueColumn = new DataGridTextColumn() 
            { 
                Header = "Значення емпіричної функції розподілу",
                Binding = new Binding(nameof(GrouppedData.EmpericFunctionValue)),
                Width = 300
            };

            FirstDataGrid.Columns.Add(variantNumColumn);
            FirstDataGrid.Columns.Add(variantValueColumn);
            FirstDataGrid.Columns.Add(frequencyColumn);
            FirstDataGrid.Columns.Add(relativeFrequencyColumn);
            FirstDataGrid.Columns.Add(empFunctionValueColumn);
        }

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

            _datas.AddPair(typeof(RowData), rowDatas.ToGeneralDataList());

            var groupedDatas = MainDataConverter.Handle<RowData, GrouppedData>(rowDatas);
            
            _datas.AddPair(typeof(GrouppedData), groupedDatas.ToGeneralDataList());

            foreach (var grouppedData in groupedDatas) 
            { 
                FirstDataGrid.Items.Add(grouppedData);
            }
        }
    }
}
