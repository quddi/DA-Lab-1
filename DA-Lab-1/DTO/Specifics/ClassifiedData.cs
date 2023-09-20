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

        public string FormattedEdges => string.Format($"[{Edges.Min:0.0000}, {Edges.Max:0.0000}]");

        public string FormattedRelativeFrequency => RelativeFrequency.ToString("0.0000");

        public string FormattedEmpericFunctionValue => EmpericFunctionValue.ToString("0.0000");
    }
}
