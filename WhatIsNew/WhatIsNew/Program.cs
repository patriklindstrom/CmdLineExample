using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

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
               var options = new Options();
            //Console.ReadKey();

            if (Parser.Default.ParseArguments(args, options))

            {
                if (options.Verbose)
                {
                    Console.WriteLine("Read: {0}", options.FileA);
                }
                var aDict = ReadDictionaryFromFile(options.FileA,
                    A_KEY_COLS.Split(options.Fieldseparator).Select(n => Convert.ToInt32(n)).ToArray());
                if (options.Verbose)
                {
                    Console.WriteLine("Read: {0}", options.FileB); 
                }
                var bDict = ReadDictionaryFromFile(options.FileB,
                    B_KEY_COLS.Split(options.Fieldseparator).Select(n => Convert.ToInt32(n)).ToArray());
                var outPutDict = bDict.Except(aDict, new DictCompareOnKeyOnly())
                    .ToDictionary(od => od.Key, od => od.Value);
                if (options.Verbose)
                {
                    Console.WriteLine("OutPut new {0} rows that are not in  but in {1}: {2}", outPutDict.Count, options.FileA,options.FileB);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var row in outPutDict)
                    Console.WriteLine(row.Value);
             
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error Cannot Parse the arguments !");
            }
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

    /// <summary>
    /// The CommandLine parser is the stable version of:  https://github.com/gsscoder/commandline  https://www.nuget.org/packages/CommandLineParser/  Install-Package Install-Package CommandLineParser -Version 1.9.71
    /// </summary>
    public class Options
    {
        [Option('o', "fileA", Required = true, HelpText = "Input A csv file to read.")]
        public string FileA { get; set; }
        [Option('n', "fileb", Required = true, HelpText = "Input B csv file to read.")]
        public string FileB { get; set; }
        [Option('v', null, Required = false, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
        [Option('s', "fieldseparator", Required = false, DefaultValue = ';',
            HelpText = "Char that separates every column")]
        public char Fieldseparator { get; set; }
        [Option('e', "version", Required = false, HelpText = "Prints version number of program.")]
        public string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }

        }
    }
    class CommitSubOptions
    {
        [Option('k', "columns", HelpText = "Sets what column that are the compound key, First column is. Eg: -k12 means column 1 and 2 combined is the key")]
        public bool All { get; set; }
        // Remainder omitted
    }
}
