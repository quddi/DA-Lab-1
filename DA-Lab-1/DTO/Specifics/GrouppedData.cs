namespace DA_Lab_1
{
    internal class GrouppedData : IData
    {
        public int VariantNum { get; set; }

        public double VariantValue { get; set; }

        public int Frequency { get; set; }

        public double RelativeFrequency { get; set; }

        public double EmpericFunctionValue { get; set; }

        public string FormattedRelativeFrequency => RelativeFrequency.ToFormattedString();


        public string FormattedEmpericFunctionValue => EmpericFunctionValue.ToFormattedString();
    }
}
