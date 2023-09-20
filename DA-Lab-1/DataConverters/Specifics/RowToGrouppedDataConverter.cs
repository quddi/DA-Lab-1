using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    class RowToGrouppedDataConverter : DataConverter<RowData, GrouppedData>
    {
        public override Type? ParametersType => null;

        protected override List<GrouppedData> Handle(List<RowData> datas, IDataConverterParameters? parameters = null)
        {
            var result = new List<GrouppedData>();

            var uniqueValues = datas.Select(rowData => rowData.VariantValue).Distinct().ToList();

            for (int i = 0; i < uniqueValues.Count; i++)
            {
                double currentValue = uniqueValues[i];

                int valuesAmount = datas.Count(rowData => rowData.VariantValue == currentValue);

                double currentRelativeFrequency = (double)valuesAmount / datas.Count;

                double empericFunctionValue = (double)datas.Count(rowData => rowData.VariantValue <= currentValue) / datas.Count;

                var grouppedData = new GrouppedData()
                {
                    VariantNum = i + 1,
                    VariantValue = currentValue,
                    Frequency = valuesAmount,
                    RelativeFrequency = currentRelativeFrequency,
                    EmpericFunctionValue = empericFunctionValue
                };

                result.Add(grouppedData);
            }

            return result;
        }
    }
}
