using DA_Lab_1.DTO.Base;

namespace DA_Lab_1.DTO.Specifics
{
    internal class ClassifiedData : IData
    {
        public int ClassNum { get; set; }

        public (double Min, double Max) Edges { get; set; }

        public double Frequency { get; set; }

        public double RelativeFrequency { get; set; }

        public double EmpericFunctionValue { get; set; }
    }
}
