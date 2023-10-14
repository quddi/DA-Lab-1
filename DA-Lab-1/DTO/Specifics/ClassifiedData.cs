using DA_Lab_1;

namespace DA_Lab_1
{
    public class ClassifiedData : IData
    {
        public int ClassNum { get; set; }

        public (double Min, double Max) Edges { get; set; }

        public double Frequency { get; set; }

        public double RelativeFrequency { get; set; }

        public double EmpiricFunctionValue { get; set; }

        public string FormattedEdges => string.Format($"[{Edges.Min.ToFormattedString()}, {Edges.Max.ToFormattedString()}]");

        public string FormattedRelativeFrequency => RelativeFrequency.ToFormattedString();

        public string FormattedEmpiricFunctionValue => EmpiricFunctionValue.ToFormattedString();
    }
}
