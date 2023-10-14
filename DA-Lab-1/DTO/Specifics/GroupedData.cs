namespace DA_Lab_1
{
    public class GroupedData : IData
    {
        public int VariantNum { get; set; }

        public double VariantValue { get; set; }

        public int Frequency { get; set; }

        public double RelativeFrequency { get; set; }

        public double EmpiricFunctionValue { get; set; }

        public bool IsOutlier { get; set; }

        public string FormattedRelativeFrequency => RelativeFrequency.ToFormattedString();


        public string FormattedEmpiricFunctionValue => EmpiricFunctionValue.ToFormattedString();
    }
}
