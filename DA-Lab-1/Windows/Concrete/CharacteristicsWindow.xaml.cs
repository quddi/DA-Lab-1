using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DA_Lab_1
{
    public partial class CharacteristicsWindow : Window
    {
        private Dictionary<Type, List<IData>>? _datas;

        public CharacteristicsWindow() { }

        public void InitializeComponent(Dictionary<Type, List<IData>> datas)
        {
            _datas = datas;
        }

        private void ComputeCharacteristics()
        {
            ComputeMean();
            ComputeMedian();
            ComputeStandardDeviation();
            ComputeSkewnessCoefficient();
            ComputeKurtosisCoefficient();
            ComputeMin();
            ComputeMax();
        }

        private void ComputeMean()
        {
            var rowDatas = _datas?.GetValue(typeof(RowData))?.ToTemplateDataList<RowData>();

            if (rowDatas == null) return;

            var mean = rowDatas.Average(data => data.VariantValue);
            MeanGradeText.Text = mean.ToString();
        }

        private void ComputeMedian() 
        { 

        }

        private void ComputeStandardDeviation()
        {

        }

        private void ComputeSkewnessCoefficient()
        {

        }

        private void ComputeKurtosisCoefficient()
        {

        }

        private void ComputeMin()
        {

        }

        private void ComputeMax()
        {

        }
    }
}
