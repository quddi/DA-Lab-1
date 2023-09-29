using DA_Lab_1;

namespace DA_Lab_1
{
    internal class ClassifiedData : IData
    {
        public int ClassNum { get; set; }

        public (double Min, double Max) Edges { get; set; }

        public double Frequency { get; set; }

        public double RelativeFrequency { get; set; }

        public double EmpericFunctionValue { get; set; }

        public string FormattedEdges => string.Format($"[{Edges.Min.ToFormattedString()}, {Edges.Max.ToFormattedString()}]");

        public string FormattedRelativeFrequency => RelativeFrequency.ToFormattedString();

        public string FormattedEmpericFunctionValue => EmpericFunctionValue.ToFormattedString();
    }
}
