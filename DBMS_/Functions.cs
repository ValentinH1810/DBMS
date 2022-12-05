using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS_
{
    class Functions
    {
        public static List<string> split(string text, char splitChar)
        {
            List<string> splitText = new List<string>();
            int currentIndex = 0;
            int startIndex = 0;
            foreach (char character in text.ToCharArray())
            {
                if (character == splitChar)
                {
                    splitText.Add(substring(text, startIndex, currentIndex));
                    startIndex = currentIndex + 1;
                }
                currentIndex++;
            }

            //has split -> add last text
            if (startIndex != 0)
            {
                splitText.Add(substring(text, startIndex, text.Length));
            }
            return splitText;
        }

        public static List<string> SplitString(string text, char[] regex)
        {
            List<string> splitText = new List<string>();
            int currentIndex = 0;
            int startIndex = 0;
            bool hasEntered = false;
            foreach (char character in text.ToCharArray())
            {
                foreach (char regexCharacter in regex)
                {
                    if (regexCharacter == character && !hasEntered)
                    {
                        splitText.Add(substring(text, startIndex, currentIndex));
                        startIndex = currentIndex + 1;
                        hasEntered = true;
                    }
                }
                hasEntered = false;
                currentIndex++;
            }

            //has split -> add last text
            if (startIndex != 0)
            {
                splitText.Add(substring(text, startIndex, text.Length));
            }
            return splitText;
        }

        public static string SubString(string text, int index)
        {
            char[] substringArray = new char[text.Length - index];
            int counter = 0;
            for (int i = index; i < text.Length; i++, counter++)
            {
                substringArray[counter] = charAt(text, i);
            }

            return new string(substringArray);
        }

        public static string substring(string text, int start, int end)
        {
            char[] substringArray = new char[end - start];
            int counter = 0;
            for (int i = start; i < end; i++, counter++)
            {
                substringArray[counter] = charAt(text, i);
            }

            return new string(substringArray);
        }

        public static char charAt(string text, int index)
        {
            return text.ToCharArray()[index];
        }

        public static string Join(List<string> list, string delimiter)
        { //Make StringBuilder...
            string concatText = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                concatText += list[i] + delimiter;
            }
            return concatText;
        }

        public static string toLowerCase(string text)
        {
            char[] textToLowerCase = new char[text.Length];
            int counter = 0;
            foreach (char letter in text.ToCharArray())
            {
                //check if upper case...
                if ((int)Functions.charAt(text, counter) >= 65 && (int)Functions.charAt(text, counter) <= 90)
                {
                    textToLowerCase[counter] = (char)((int)Functions.charAt(text, counter) + 32);
                }
                else
                {
                    textToLowerCase[counter] = Functions.charAt(text, counter);
                }
                counter++;
            }
            return new string(textToLowerCase);
        }
        public static void Create(string tableName, List<string> columnsNames)
        {
            string fileName = $@"D:\{tableName}.txt";

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            FileStream stream = new FileStream(fileName, FileMode.CreateNew);

            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write("Table name: ");
                writer.WriteLine(tableName);
                writer.WriteLine();
                foreach (var column in columnsNames)
                {
                    writer.Write(column);
                    writer.Write('\t');
                }
                writer.WriteLine();
            }

            Console.WriteLine("Table created.\n\n");
        }

        public static void Drop(string tableName)
        {
            string fileName = $@"D:\{tableName}.txt";

            if (!File.Exists(fileName))
            {
                Console.WriteLine("This table doesn't exist!\n\n");
                return;
            }

            File.Delete(fileName);
            Console.WriteLine($"Table {tableName} is removed.\n\n");
        }

        public static void Insert(string tableName, List<List<object>> valueLines)
        {
            string fileName = $@"D:\{tableName}.txt";

            using (StreamWriter writer = new StreamWriter($"{tableName}.txt"))
            {
                foreach (List<object> values in valueLines)
                {
                    foreach(object value in values)
                    {
                        writer.WriteLine(value);
                        writer.WriteLine("\t");
                    }                   
                }
            }
        }

        public static int TableInfo(string tableName)
        {
            string fileName = $@"D:\{tableName}.txt";

            return Directory.GetFiles(fileName).Length;
        }
    }
}
