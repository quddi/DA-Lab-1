using DA_Lab_1.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Lab_1.Extentions
{
    internal static class ExtentionsMethods
    {
        public static T2 GetValue<T1, T2>(this IDictionary<T1, T2> dictionary, T1 key)
        {
            if (!dictionary.ContainsKey(key)) return default;

            return dictionary[key];
        }

        public static void AddPair<T1, T2>(this IDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key)) dictionary[key] = value;
            else dictionary.Add(key, value);
        }

        public static List<IData> ToGeneralDataList<T>(this IEnumerable<T> originEnumerable) where T : IData
        {
            return originEnumerable.Cast<IData>().ToList();
        }

        public static List<T> ToTemplateDataList<T>(this IEnumerable<IData> originEnumerable) where T : IData
        {
            return originEnumerable.Cast<T>().ToList();
        }
    }
}
