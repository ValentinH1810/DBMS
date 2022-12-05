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

            while (exit != true)
            {
                Console.WriteLine("Enter a command: ");
                var line = Console.ReadLine();
                    
                List<string> lineWords = Functions.SplitString(line, new char[] {' ', ',', '(', ')'});
                List<string> columns = new List<string>();
                List<List<object>> valueLines = new List<List<object>>();

                //string command = Functions.toLowerCase(lineWords[0]);

                string tableName;

                if (lineWords.Count > 1)
                {
                    switch (lineWords[0])
                    {
                        case "CreateTable":
                            {
                                tableName = lineWords[1];

                                for (int i = 2; i < lineWords.Count; i++)
                                {
                                    columns.Add(lineWords[i]);
                                }

                                Functions.Create(tableName, columns);
                            }
                            break;
                        case "DropTable":
                            {
                                tableName = lineWords[1];
                                Functions.Drop(tableName);
                            }
                            break;
                        case "Insert":
                            {
                                if(lineWords[1] == "INTO")
                                {
                                    tableName = lineWords[2];

                                    for (int j = 0; j < lineWords.Count; j++)                                       
                                    {
                                        for (int i = 3; i < columns.Count; i++)
                                        {
                                            if(lineWords[j] == columns[i])
                                            {
                                                if (lineWords[4] == "VALUES")
                                                {
                                                    for (int k = columns.Count - 1; k < lineWords.Count; k++)
                                                    {
                                                        foreach (List<object> values in valueLines)
                                                        {
                                                            foreach (object value in values)
                                                            {
                                                                values.Add(value);
                                                            }

                                                            valueLines.Add(values);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid command!\n\n");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid command!\n\n");
                                            }
                                        }
                                    }

                                    Functions.Insert(tableName, valueLines);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid command!\n\n");
                                }                    
                            }
                            break;
                        case "ListTables":
                            break;
                        case "TableInfo":
                            {
                                tableName = lineWords[1];
                                Functions.TableInfo(tableName);
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid command!\n\n");
                            break;
                    }
                }
                else
                {
                    if (line == "Exit")
                    {
                        exit = true;
                    }
                    else if (line == String.Empty)
                    {
                        Console.WriteLine("Please enter a command to continue!\n\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid command!\n\n");
                    }
                }
            }
        }
    }
}