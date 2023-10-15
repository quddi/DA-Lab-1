using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    public class GroupedToTransformedGroupedDataConverter : DataConverter<GroupedData, TransformedGroupedData>
    {
        public override Type? ParametersType => null;

        protected override List<TransformedGroupedData> Handle(List<GroupedData> data, IDataConverterParameters? parameters = null)
        {
            var result = new List<TransformedGroupedData>();

            foreach (var groupedData in data)
            {
                var item = new TransformedGroupedData
                {
                    T = GetConvertedX(groupedData.VariantValue),
                    Z = GetConvertedY(groupedData.EmpiricFunctionValue)
                };

                if (!double.IsNaN(item.Z)) result.Add(item);
            }

            return result.OrderBy(data => data.T).ToTemplateDataList<TransformedGroupedData>();
        }

        private double GetConvertedX(double x) => x;

        private double GetConvertedY(double y) => Characteristics.GetStudentDistributionQuantile(y, Characteristics.Count-1);
    }
}
