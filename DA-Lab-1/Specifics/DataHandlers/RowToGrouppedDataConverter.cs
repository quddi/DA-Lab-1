using DA_Lab_1.Specifics.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Lab_1.Specifics.DataHandlers
{
    class RowToGrouppedDataConverter : DataConverter<RowData, GrouppedData>
    {
        protected override List<GrouppedData> Handle(List<RowData> datas)
        {
            var result = new List<GrouppedData>();

            var uniqueValues = datas.Select(rowData => rowData.VariantValue).Distinct().ToList();

            for (int i = 0; i < uniqueValues.Count; i++)
            {
                double currentValue = uniqueValues[i];

                var valuesAmount = datas.Count(rowData => rowData.VariantValue == currentValue);

                var previousRelativeFrequenciesSum = result.Sum(groupedData => groupedData.RelativeFrequency);

                int currentRelativeFrequency = valuesAmount / datas.Count;

                var grouppedData = new GrouppedData()
                {
                    VariantNum = i + 1,
                    VariantValue = currentValue,
                    Frequency = valuesAmount,
                    RelativeFrequency = currentRelativeFrequency,
                    EmpericFunctionValue = previousRelativeFrequenciesSum + currentRelativeFrequency
                };

                result.Add(grouppedData);
            }

            return result;
        }
    }
}
