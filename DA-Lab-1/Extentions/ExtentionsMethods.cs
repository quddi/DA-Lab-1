using System;
using System.Collections.Generic;
using System.Linq;

namespace DA_Lab_1
{
    internal static class ExtentionsMethods
    {
        public static int GetClassesAmount(int elementsAmount)
        {
            if (elementsAmount < 100)
            {
                var sqrt = (int)Math.Sqrt(elementsAmount);

                return sqrt % 2 == 0 ? sqrt - 1 : sqrt;
            }
            else
            {
                var sqrt = (int)Math.Pow(elementsAmount, 1.0/3.0);

                return sqrt % 2 == 0 ? sqrt - 1 : sqrt;
            }
        }

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

        public static string ToFormattedString(this double value)
        {
            return value.ToString("0.0000");
        }
    }
}
