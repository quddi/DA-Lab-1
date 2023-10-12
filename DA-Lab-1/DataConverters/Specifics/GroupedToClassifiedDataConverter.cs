using DA_Lab_1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    internal class GroupedToClassifiedDataConverter : DataConverter<GrouppedData, ClassifiedData>
    {
        public override Type? ParametersType => typeof(GrouppedToClassifiedConverterParameters);

        protected override List<ClassifiedData> Handle(List<GrouppedData> data, IDataConverterParameters? parameters)
        {
            if (parameters is not GrouppedToClassifiedConverterParameters concretizedParameters) 
                throw new ArgumentNullException($"Тип параметрів мав бути {ParametersType} але був {parameters?.GetType()}");

            var minValue = data.Min(data => data.VariantValue);
            var maxValue = data.Max(data => data.VariantValue);

            var classWidth = (maxValue - minValue) / concretizedParameters.ClassesAmount;

            Characteristics.SetClassWidth(classWidth);

            var rowDatasAmount = data.Sum(data => data.Frequency);

            var result = new List<ClassifiedData>();

            for (int i = 0; i < concretizedParameters.ClassesAmount; i++)
            {
                var leftEdge = minValue + Characteristics.ClassWidth * i;
                var rightEdge = minValue + Characteristics.ClassWidth * (i + 1);

                var suitableDatas = (i == result.Count - 1)
                    ? data.Where(data => (leftEdge <= data.VariantValue && data.VariantValue < rightEdge))
                    : data.Where(data => (leftEdge <= data.VariantValue && data.VariantValue <= rightEdge))
                    .ToList();

                var frequency = suitableDatas.Sum(data => data.Frequency);

                double relativeFrequency = (double)frequency / rowDatasAmount;

                var empericFunctionValue = (i == 0)
                    ? relativeFrequency
                    : relativeFrequency + result[i - 1].EmpericFunctionValue;

                var classifiedData = new ClassifiedData()
                {
                    ClassNum = i + 1,
                    Edges = (leftEdge, rightEdge),
                    Frequency = frequency,
                    RelativeFrequency = relativeFrequency,
                    EmpericFunctionValue = empericFunctionValue
                };

                result.Add(classifiedData);
            }

            return result;
        }
    }
}
