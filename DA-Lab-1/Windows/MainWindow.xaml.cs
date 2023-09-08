using DA_Lab_1.Base;
using DA_Lab_1.Specifics.DTO;
using DA_Lab_1.Specifics.Other;
using DA_Lab_1.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DA_Lab_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateRowDataGrid();

            FirstDataGrid.Items.Add(new RowData { VariantNum = 1, VariantValue = 2.31, Frequency = 4, RelativeFrequency = 0.6, EmpericFunctionValue = -117.3 });
            FirstDataGrid.Items.Add(new RowData { VariantNum = 2, VariantValue = 421, Frequency = 1, RelativeFrequency = 0.2, EmpericFunctionValue = 37.2 });
            FirstDataGrid.Items.Add(new RowData { VariantNum = 3, VariantValue = 421, Frequency = 1, RelativeFrequency = 0.2, EmpericFunctionValue = 37.2 });
            FirstDataGrid.Items.Add(new RowData { VariantNum = 4, VariantValue = 421, Frequency = 1, RelativeFrequency = 0.2, EmpericFunctionValue = 37.2 });
        
            FirstDataGrid.IsReadOnly = true;
        }

        private void CreateRowDataGrid()
        {
            FirstDataGrid.Columns.Clear();

            DataGridTextColumn variantNumColumn = new DataGridTextColumn()
            {
                Header = "№ Варіанти",
                Binding = new Binding(nameof(RowData.VariantNum)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn variantValueColumn = new DataGridTextColumn()
            {
                Header = "Значення варіанти",
                Binding = new Binding(nameof(RowData.VariantValue)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn frequencyColumn = new DataGridTextColumn() 
            { 
                Header = "Частота",
                Binding = new Binding(nameof(RowData.Frequency)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn relativeFrequencyColumn = new DataGridTextColumn() 
            { 
                Header = "Відносна частота",
                Binding = new Binding(nameof(RowData.RelativeFrequency)),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            DataGridTextColumn empFunctionValueColumn = new DataGridTextColumn() 
            { 
                Header = "Значення емпіричної функції розподілу",
                Binding = new Binding(nameof(RowData.EmpericFunctionValue)),
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
            var values = DataLoader.LoadValues();

            if (values == null)
            {
                MessageBox.Show("Список значень був пустий!");

                return;
            }

            var rowDatas = new List<RowData>();

            for (int i = 1; i < values.Count + 1; i++)
            {
                double value = values[i];

                rowDatas.Add(new RowData()
                {
                    VariantNum = i,
                    VariantValue = value,
                });
            }
        }
    }
}
