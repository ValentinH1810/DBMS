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
                List<string> lineWords = Functions.SplitString(line, new char[] { ' ', ',', '(', ')' });

                List<string> columns = new List<string>();
                List<List<object>> values = new List<List<object>>();

                string command = Functions.toLowerCase(lineWords[0]);

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
                    if(line == "Exit" || line == "exit" || line == "EXIT")
                    {
                        exit = true;
                    }
                    else if(line == String.Empty)
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