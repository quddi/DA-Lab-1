using DA_Lab_1.Specifics.DTO;
using DA_Lab_1.Specifics.Extentions;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DA_Lab_1.Specifics.DataHandlers
{
    static class MainDataHandler
    {
        private static Dictionary<(Type from, Type to), IDataConverter> _dataHandlers = new()
        {
            { (typeof(RowData), typeof(GrouppedData)), new RowToGrouppedDataConverter() }
        };

        public static List<T2>? Handle<T1, T2>(List<T1> datas) where T1 : IData where T2 : IData
        {
            var converter = _dataHandlers.GetValue((typeof(T1), typeof(T2)));

            if (!_dataHandlers.ContainsKey(dataType)) 
            {
                MessageBox.Show($"Для типу {dataType} не знайшлося обробника!");
                return null;
            }

        }
    }
}
