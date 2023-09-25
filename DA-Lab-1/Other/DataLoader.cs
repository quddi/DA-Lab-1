using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace DA_Lab_1
{
    static class DataLoader
    {
        public static List<double>? LoadValues()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    return ReadNumbersFromFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при читанні файлу: {ex.Message}");
                }
            }

            return null;
        }

        private static List<double> ReadNumbersFromFile(string filePath)
        {
            List<double> numbers = new List<double>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string token in tokens)
                    {
                        if (double.TryParse(token, out double number))
                            numbers.Add(number);
                        else
                            MessageBox.Show($"Помилка при зчитуванні числа: {token}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зчитуванні файлу: {ex.Message}");
            }

            return numbers;
        }
    }
}
