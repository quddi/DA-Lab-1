using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DA_Lab_1
{
    internal static class ExtensionsMethods
    {
        private static readonly Color[] RandomColorsPool = new Color[]
        {
            Color.Red,
            Color.Green, 
            Color.Magenta,
            Color.Cyan,
            Color.Bisque,
            Color.Fuchsia,
            Color.Firebrick,
            Color.Coral,
            Color.IndianRed,
            Color.HotPink
        };

        private static readonly Random Random = new();

        public static Color GetRandomColor()
        {
            return RandomColorsPool[Random.Next(0, RandomColorsPool.Length)];
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
