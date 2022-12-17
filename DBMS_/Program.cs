using System;
using System.Collections.Generic;
using System.IO;

namespace DBMS_
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            List<string> columns = new List<string>();
            List<List<string>> valueLines = new List<List<string>>();

            while (exit != true)
            {
                Console.WriteLine("Enter a command: ");
                var line = Console.ReadLine();
                    
                List<string> lineWords = Functions.SplitString(line, new char[] {' ', ',', '(', ')', ':'});
                var toLowerCommand = Functions.toLower(lineWords[0]);
                string tableName;             

                switch (toLowerCommand)
                {
                    case "createtable": //CreateTable Sample (Id:int, Name:string, Town:string)
                        {
                            tableName = lineWords[1];

                            for (int i = 2; i < lineWords.Count; i += 2) 
                            {
                                columns.Add(lineWords[i]);
                            }

                            Functions.Create(tableName, columns);
                        }
                        break;
                    case "droptable":
                        {
                            tableName = lineWords[1];
                            Functions.Drop(tableName);
                        }
                        break;
                    case "insert": //Insert INTO Sample (Id, Name, Town) VALUES (1, Martin, Aitos) (2, Valentin, Sliven) (3, Vasil, Sliven)
                        {
                            List<string> lineWords2 = Functions.SplitString(line, new char[] { '(', ')' });

                            if (!Functions.toLower(lineWords[1]).Equals("into") && !Functions.toLower(lineWords2[2]).Equals(" values ")) return;

                            try
                            {
                                tableName = lineWords[2];

                                if (Functions.SplitString(lineWords2[1], new char[] { ' ', ',' }).Count
                                    == Functions.SplitString(lineWords2[3], new char[] { ' ', ',' }).Count)
                                {
                                    for (int i = 3; i < lineWords2.Count; i += 2)
                                    {
                                        valueLines.Add(Functions.SplitString(lineWords2[i], new char[] { ' ', ',' }));
                                    }

                                    Functions.Insert(tableName, valueLines);
                                    valueLines.Clear();
                                }
                            }
                            catch (Exception e)
                            {
                                return;
                            }
                        }
                        break;
                    case "delete": //Delete FROM Sample (Id, Name, Town) VALUES (1, Marto, Aitos)
                        {
                            List<string> lineWords2 = Functions.SplitString(line, new char[] { '(', ')' });

                            if (Functions.toLower(lineWords[1]).Equals("into") && Functions.toLower(lineWords2[2]).Equals(" values ")) return;

                            try
                            {
                                tableName = lineWords[2];

                                if (Functions.SplitString(lineWords2[1], new char[] { ' ', ',' }).Count
                                    == Functions.SplitString(lineWords2[3], new char[] { ' ', ',' }).Count)
                                {
                                    List<string> linesToRemove = new List<string>();

                                    for (int i = 3; i < lineWords2.Count; i += 2)
                                    {
                                        linesToRemove = Functions.SplitString(lineWords2[i], new char[] { ' ', ',' });
                                    }

                                    Functions.Delete(tableName, linesToRemove);
                                    valueLines.Clear();
                                }
                            }
                            catch (Exception e)
                            {
                                return;
                            }
                        }
                        break;
                    case "tableinfo":
                        {
                            tableName = lineWords[1];
                            Functions.TableInfo(tableName);
                        }
                        break;
                    case "listtables":
                        {
                            Functions.ListTables();
                        }
                        break;
                    case "select": //Select (Name, DateBirth) FROM Sample WHERE Id <> 5 AND DateBirth > “01.01.2000”
                        {
                            try
                            {
                                List<string> lineWords2 = Functions.SplitString(line, new char[] { '(', ')' });

                                //read table...
                                //check the columns...
                                //save in list...
                                tableName = lineWords[3];
                                string fileName = $@"D:\{tableName}.txt";
                                string[] lines = File.ReadAllLines(fileName);
                                List<string> selectedColumns = Functions.SplitString(lineWords[1], new char[] { ',', ' ' });

                                //int columnsCount = Functions.split(lines[2], '\t').Count;
                                //int selectedColumns = Functions.SplitString(lineWords[1], new char[] { ',', ' ' }).Count;
                                List<List<string>> selectedColumnsValues = new List<List<string>>();
                                foreach (string selectedColumn in selectedColumns)
                                {
                                    List<string> columnsTotal = Functions.Split(lines[2], '\t');
                                    int selectedColumnIndex = FindIndex(columnsTotal, selectedColumn);
                                    List<string> temp = new List<string>();

                                    for (int i = 3; i < lines.Length; i++)
                                    {
                                        temp.Add(Functions.Split(lines[i], '\t')[selectedColumnIndex]);
                                    }
                                    selectedColumnsValues.Add(temp);
                                    temp.Clear();
                                }
                            }
                            catch (Exception e)
                            {
                                return;
                            }
                        }
                        break;
                    case "exit":
                        exit = true;
                        break;
                    case "":
                        Console.WriteLine("Please enter a command to continue!\n\n\n");
                        break;
                    default:
                        Console.WriteLine("Invalid command!\n\n\n");
                        break;
                }
            }
        }

        private static int FindIndex(List<string> columnsTotal, string selectedColumn)
        {
            int index = 0;

            foreach(string column in columnsTotal)
            {
                if (column.Equals(selectedColumn))
                {
                    return index;
                }
                index++;
            }

            return index;
        }
    }
}