using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace DA_Lab_1
{
    internal class GroupedToClassifiedDataConverter : DataConverter<GroupedData, ClassifiedData>
    {
        public override Type? ParametersType => typeof(GroupedToClassifiedConverterParameters);

        protected override List<ClassifiedData> Handle(List<GroupedData> data, IDataConverterParameters? parameters)
        {
            if (parameters is not GroupedToClassifiedConverterParameters concretizedParameters) 
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
                var isLast = i != concretizedParameters.ClassesAmount - 1;

                var suitableDatas = data.Where(data => 
                    IsDataSuitable(isLast, leftEdge, rightEdge, data.VariantValue))
                    .ToList();

                var frequency = suitableDatas.Sum(data => data.Frequency);

                double relativeFrequency = (double)frequency / rowDatasAmount;

                var empiricFunctionValue = (i == 0)
                    ? relativeFrequency
                    : relativeFrequency + result[i - 1].EmpiricFunctionValue;

                var classifiedData = new ClassifiedData()
                {
                    ClassNum = i + 1,
                    Edges = (leftEdge, rightEdge),
                    Frequency = frequency,
                    RelativeFrequency = relativeFrequency,
                    EmpiricFunctionValue = empiricFunctionValue
                };

                result.Add(classifiedData);
            }

            return result;
        }

        protected bool IsDataSuitable(bool isLast, double leftEdge, double rightEdge, double value)
        {
            return isLast
                ? leftEdge.IsLessOrEqual(value) && value < rightEdge
                : leftEdge.IsLessOrEqual(value) & value.IsLessOrEqual(rightEdge);
        }
    }
}
