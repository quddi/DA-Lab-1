using System;
using System.Collections.Generic;

namespace DA_Lab_1
{
    static class MainDataConverter
    {
        private static Dictionary<(Type from, Type to), IDataConverter> _dataConverters = new()
        {
            { (typeof(RowData), typeof(GrouppedData)), new RowToGrouppedDataConverter() },
            { (typeof(GrouppedData), typeof(ClassifiedData)), new GroupedToClassifiedDataConverter() }
        };

        public static List<T2> Handle<T1, T2>(List<T1> datas, IDataConverterParameters? parameters = null) where T1 : IData where T2 : IData
        {
            var converter = _dataConverters.GetValue((typeof(T1), typeof(T2)));

            if (converter == null)
                throw new ArgumentException($"Конвертер для типу {nameof(T1)} в тип {nameof(T2)} не був знайдений!");

            if (converter.ParametersType != parameters?.GetType())
                throw new InvalidOperationException($"Конвертер для типу {nameof(T1)} в тип {nameof(T2)} не приймає параметри типу {parameters?.GetType()}");

            return converter.Handle(datas.ToGeneralDataList(), parameters).ToTemplateDataList<T2>();
        }
    }
}
