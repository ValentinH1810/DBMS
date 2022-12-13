using System;
using System.Collections.Generic;
using System.IO;

namespace DBMS_
{
    class Program
    {
        //enum valueTypes
        //{
        //    Int,
        //    String,
        //    Date,
        //}

        static void Main(string[] args)
        {
            bool exit = false;
            List<string> tables = new List<string>();
            List<string> columns = new List<string>();
            List<List<string>> valueLines = new List<List<string>>();

            while (exit != true)
            {
                Console.WriteLine("Enter a command: ");
                var line = Console.ReadLine();
                    
                List<string> lineWords = Functions.SplitString(line, new char[] {' ', ',', '(', ')', ':'});               

                string tableName;
                string valueType = String.Empty;

                //CreateTable Marto (Id:int, Name:string)
                //Insert INTO Marto (Id, Name) VALUES (1, Martin) 

                if (lineWords.Count > 1)
                {
                    switch (lineWords[0])
                    {
                        case "CreateTable":
                            {
                                tableName = lineWords[1];

                                tables.Add(tableName);

                                for (int i = 2; i < lineWords.Count; i += 2)
                                {
                                    columns.Add(lineWords[i]);
                                }

                                //for (int i = 3; i < lineWords.Count; i += 2)
                                //{
                                //    switch (lineWords[i])
                                //    {
                                //        case "int":
                                //            {
                                //                valueType = "Int";
                                //            }
                                //            break;
                                //        case "string":
                                //            {
                                //                valueType = "String";
                                //            }
                                //            break;
                                //        case "date":
                                //            {
                                //                valueType = "Date";
                                //            }
                                //            break;
                                //    }
                                //} 

                                Functions.Create(tableName, columns, tables);
                            }
                            break;
                        case "DropTable":
                            {
                                tableName = lineWords[1];

                                for (int i = 0; i < tables.Count; i++)
                                {
                                    tables.Remove(tableName);
                                }

                                Functions.Drop(tableName);
                            }
                            break;
                        case "Insert": //Insert INTO Marto (Id, Name) VALUES (1, "Ivan")
                            {
                                if (lineWords[1] == "INTO" && lineWords[4] == "VALUES")
                                {
                                    tableName = lineWords[2];
                                    List<string> lineWords2 = Functions.SplitString(line, new char[] { '(', ')' });

                                    if (Functions.SplitString(lineWords2[1], new char[] { ' ', ',' }).Count
                                        == Functions.SplitString(lineWords2[3], new char[] { ' ', ',' }).Count)
                                    {
                                        valueLines.Add(Functions.SplitString(lineWords2[3], new char[] { ' ', ',' }));

                                        Functions.Insert(tableName, valueLines);
                                        valueLines.Clear();
                                        Console.WriteLine("SUCCESS");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid command!\n\n\n");
                                }
                            }
                            break;
                        case "TableInfo":
                            {
                                tableName = lineWords[1];
                                Functions.TableInfo(tableName);
                            }
                            break;
                        case "Select": //Select (Name, DateBirth) FROM Sample WHERE Id <> 5 AND DateBirth > “01.01.2000”
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
                                        List<string> columnsTotal = Functions.split(lines[2], '\t');
                                        int selectedColumnIndex = FindIndex(columnsTotal, selectedColumn);
                                        List<string> temp = new List<string>();

                                        for (int i = 3; i < lines.Length; i++)
                                        {
                                            temp.Add(Functions.split(lines[i], '\t')[selectedColumnIndex]);
                                        }
                                        selectedColumnsValues.Add(temp);
                                        temp.Clear();
                                    }
                                }
                                catch(Exception e)
                                {
                                    return;
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid command!\n\n\n");
                            break;
                    }
                }
                else
                {
                    if(line == "ListTables")
                    {
                        Console.WriteLine();
                        Functions.ListTables();
                    }
                    else if (line == "Exit")
                    {
                        exit = true;
                    }
                    else if (line == String.Empty)
                    {
                        Console.WriteLine("Please enter a command to continue!\n\n\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid command!\n\n\n");
                    }
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