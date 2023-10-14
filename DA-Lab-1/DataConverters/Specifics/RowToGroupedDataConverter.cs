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

            var delta = Characteristics.ThirdQuartile - Characteristics.FirstQuartile;

            var downOutlieEdge = Characteristics.FirstQuartile - Characteristics.OutlieK  * delta;
            var upOutlieEdge = Characteristics.ThirdQuartile + Characteristics.OutlieK * delta;

            for (int i = 0; i < uniqueValues.Count; i++)
            {
                double currentValue = uniqueValues[i];

                int valuesAmount = datas.Count(rowData => rowData.VariantValue == currentValue);

                double currentRelativeFrequency = (double)valuesAmount / datas.Count;

                double empiricFunctionValue = (double)datas.Count(rowData => rowData.VariantValue <= currentValue) / datas.Count;

                bool isOutlie = currentValue < downOutlieEdge || currentValue > upOutlieEdge;

                var groupedData = new GroupedData()
                {
                    VariantNum = i + 1,
                    VariantValue = currentValue,
                    Frequency = valuesAmount,
                    RelativeFrequency = currentRelativeFrequency,
                    EmpiricFunctionValue = empiricFunctionValue,
                    IsOutlier = isOutlie,
                };

                result.Add(groupedData);
            }

            return result;
        }
    }
}
