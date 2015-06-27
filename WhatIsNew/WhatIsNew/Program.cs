using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatIsNew
{
    internal class Program
    {
        private const string A_OLD_FILE = "A_TestFile.csv";
        private const string A_KEY_COLS = "2;3";
        private const string B_NEW_FILE = "B_TestFile.csv";
        private const string B_KEY_COLS = "1;2";
        private const Char SEP = ';';

        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to test an argument parser hello world application");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Read: {0}", A_OLD_FILE);
            var aDict = ReadDictionaryFromFile(A_OLD_FILE, A_KEY_COLS.Split(SEP).Select(n => Convert.ToInt32(n)).ToArray());
            Console.WriteLine("Read: {0}", B_NEW_FILE);
            var bDict = ReadDictionaryFromFile(B_NEW_FILE, B_KEY_COLS.Split(SEP).Select(n => Convert.ToInt32(n)).ToArray());
            var outPutDict = bDict.Except(aDict, new DictCompareOnKeyOnly()).ToDictionary(od => od.Key, od => od.Value);
            Console.WriteLine("OutPut new {0} rows that are not in  but in {1}: {2}", outPutDict.Count, A_OLD_FILE, B_NEW_FILE);
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var row in outPutDict)
                Console.WriteLine(row.Value);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Hit any key to Exit");
            Console.ReadKey();
        }


        private static Dictionary<string, string> ReadDictionaryFromFile(string path, int[] colKeys)
        {
            StreamReader reader = new StreamReader(File.OpenRead(path));
            var dict = new Dictionary<string, string>();
            string line;
            // I am reusing the rowKey StringBuilder for every row. Maybe that is stupid optimisation.
            var rowKey = new StringBuilder();
            while ((line = reader.ReadLine()) != null)
            {
                BuildRowKey(ref rowKey, line, SEP, colKeys);
                dict.Add(rowKey.ToString(), line);
                rowKey.Clear();
            }
            return dict;
        }

        private static void BuildRowKey(ref StringBuilder rowKey, string line, char splitChar, int[] keyColumns)
        {
            var fieldArr = line.Split(splitChar);
            for (int index = 0; index < keyColumns.Length - 1; index++)
                rowKey.Append(fieldArr[keyColumns[index] - 1]).Append("|");
            rowKey.Append(fieldArr[keyColumns.Last() - 1]);
        }

        public class DictCompareOnKeyOnly : IEqualityComparer<KeyValuePair<string, string>>
        {
            public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                return x.Key.Equals(y.Key);
            }

            public int GetHashCode(KeyValuePair<string, string> obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}
