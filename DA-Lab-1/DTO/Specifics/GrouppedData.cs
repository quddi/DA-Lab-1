using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Lab_1.DTO.Base;

namespace DA_Lab_1.DTO.Specifics
{
    internal class GrouppedData : IData
    {
        public int VariantNum { get; set; }

        public double VariantValue { get; set; }

        public double Frequency { get; set; }

        public double RelativeFrequency { get; set; }

        public double EmpericFunctionValue { get; set; }
    }
}
