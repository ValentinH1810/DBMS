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
        public static string path = @"..\..\..\Tables\";

        public static void Create(string tableName, List<string> items)
        {
            string filePath = path + $"{tableName}.txt";

            if (File.Exists(filePath))
            {
                //File.Delete(filePath);

                FileStream stream = new FileStream(filePath, FileMode.CreateNew);

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"Table name: {tableName}");
                    writer.WriteLine();

                    for (int i = 1; i < items.Count; i++)
                    {
                        var elements = HelpFuncs.SplitString(items[i], new char[] { ':', ' ' });

                        if(elements.Count > 2)
                        {
                            writer.Write(elements[0] + "(" + elements[1] + " " + elements[2] + " " + elements[3] + ")" + "\t");
                        }
                        else
                        {
                            writer.Write(elements[0] + "(" + elements[1] + ")" + "\t");
                        }
                    }
                    writer.WriteLine();
                }

                Console.WriteLine("Table created.\n\n\n");
            }
            else
            {
                Console.WriteLine("This table already exists. Do you want to replace it?");
                Console.WriteLine("Yes / No");

                string option = Console.ReadLine();

                while (HelpFuncs.ToLower(option) != "yes" && HelpFuncs.ToLower(option) != "no")
                {
                    Console.WriteLine("Invalid answer!");
                    Console.WriteLine("Yes / No");
                    option = Console.ReadLine();
                }

                if (HelpFuncs.ToLower(option) == "yes")
                {
                    File.Delete(filePath);

                    FileStream stream = new FileStream(filePath, FileMode.CreateNew);

                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine($"Table name: {tableName}");
                        writer.WriteLine();

                        for (int i = 1; i < items.Count; i++)
                        {
                            var elements = HelpFuncs.SplitString(items[i], new char[] { ':', ' ' });

                            if (elements.Count > 2)
                            {
                                writer.Write(elements[0] + "(" + elements[1] + " " + elements[2] + " " + elements[3] + ")" + "\t");
                            }
                            else
                            {
                                writer.Write(elements[0] + "(" + elements[1] + ")" + "\t");
                            }
                        }

                        writer.WriteLine();
                    }

                    Console.WriteLine("Table replaced.\n\n\n");
                }
                else if (HelpFuncs.ToLower(option) == "no")
                {
                    Console.WriteLine("\n\n\n");
                }
            }
        }

        public static void Drop(string tableName)
        {
            string filePath = path + $"{tableName}.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("This table doesn't exist!\n\n\n");
                return;
            }

            File.Delete(filePath);
            Console.WriteLine($"Table {tableName} is removed.\n\n\n");
        }

        public static void Insert(string tableName, List<List<string>> valueLines)
        {
            string filePath = path + $"{tableName}.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("This table doesn't exist!\n\n\n");
                return;
            }
            else
            {
                bool isSuccessful = false;

                using StreamWriter file = new(filePath, append: true);
                {
                    foreach (List<string> values in valueLines)
                    {
                        foreach (string value in values)
                        {
                            file.Write(value);
                            file.Write('\t');
                            isSuccessful = true;
                        }
                        file.WriteLine();
                    }               
                }

                if (isSuccessful)
                    Console.WriteLine($"Line(s) added successfully into {tableName}\n\n\n");
                else
                    Console.WriteLine("Invalid input!\n\n\n");
            }
        }

        public static void ListTables()
        {
            Console.WriteLine("\nAll tables created:");

            foreach (var tablePath in Directory.GetFiles(path))
            {
                var tableName = HelpFuncs.SplitString(tablePath, new char[] { ' ', '\\', ':', '.' });
                Console.WriteLine(tableName[^2]);               
            }

            Console.WriteLine("\n\n\n");
        }

        public static List<string> ListTablesToReturn()
        {
            List<string> listToReturn = new List<string>();

            foreach (var tablePath in Directory.GetFiles(path))
            {
                var tableName = HelpFuncs.SplitString(tablePath, new char[] { ' ', '\\', ':', '.' });
                listToReturn.Add(tableName[^2]);
            }

            return listToReturn;
        }

        public static void TableInfo(string tableName)
        {
            string filePath = path + $"{tableName}.txt";

            if (File.Exists(filePath))
            {
                string stored = File.ReadAllText(filePath);
                int valueLinesCount = File.ReadAllLines(filePath).Length - 3;

                Console.WriteLine();
                Console.WriteLine(stored + "\n");
                Console.Write("Records count: ");
                Console.WriteLine(valueLinesCount);

                FileInfo file = new(filePath);

                long size = file.Length;
                Console.WriteLine($"\nFile {tableName}.txt size in bytes: {size}");

                DateTime creationTime = file.CreationTime;
                Console.WriteLine("Created: {0}", creationTime);

                DateTime updatedTime = file.LastWriteTime;
                Console.WriteLine("Lastly modified: {0}", updatedTime);
                Console.Write("\n\n\n");
            }
            else
            {
                Console.WriteLine("This table doesn't exist!\n\n\n");
            }
        }   
    }
}