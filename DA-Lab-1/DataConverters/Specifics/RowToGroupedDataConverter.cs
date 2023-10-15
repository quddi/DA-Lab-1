using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    class RowToGroupedDataConverter : DataConverter<RowData, GroupedData>
    {
        public override Type? ParametersType => null;

        protected override List<GroupedData> Handle(List<RowData> datas, IDataConverterParameters? parameters = null)
        {
            var result = new List<GroupedData>();

            var uniqueValues = datas.Select(rowData => rowData.VariantValue).Distinct().ToList();

            for (int i = 0; i < uniqueValues.Count; i++)
            {
                double currentValue = uniqueValues[i];

                int valuesAmount = datas.Count(rowData => rowData.VariantValue == currentValue);

                double currentRelativeFrequency = (double)valuesAmount / datas.Count;

                double empiricFunctionValue = (double)datas.Count(rowData => rowData.VariantValue <= currentValue) / datas.Count;

                var groupedData = new GroupedData()
                {
                    VariantNum = i + 1,
                    VariantValue = currentValue,
                    Frequency = valuesAmount,
                    RelativeFrequency = currentRelativeFrequency,
                    EmpiricFunctionValue = empiricFunctionValue,
                };

                result.Add(groupedData);
            }

            return result;
        }
    }
}
