using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Lab_1.Specifics.Extentions
{
    internal static class ExtentionsMethods
    {
        public static T2 GetValue<T1, T2>(this IDictionary<T1, T2> dictionary, T1 key)
        {
            if (!dictionary.ContainsKey(key)) return default;

            return dictionary[key];
        }
    }
}
