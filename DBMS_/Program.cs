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
            bool valid = HelpFuncs.ValidateTables(Functions.ListTablesToReturn());

            if(!valid)
            {
                exit = true;
            }

            while (exit != true)
            {
                Console.WriteLine("Enter a command: ");
                var line = Console.ReadLine();

                List<List<string>> valueLines = new List<List<string>>();
                List<string> lineWords = HelpFuncs.SplitString(line, new char[] {' ', ',', '\"', '(', ')', ':'});
                List<string> splitLine = HelpFuncs.SplitString(line, new char[] {',', '(', ')'});
                var toLowerCommand = HelpFuncs.ToLower(lineWords[0]);
                string tableName;

                switch (toLowerCommand)
                {
                    case "createtable":
                        //CreateTable Sample (Id:int, Name:string, BirthDate:date default 01.01.2022)
                        //CreateTable People (Id:int, Name:string, Town:string)
                        {
                            tableName = lineWords[1];
                            Functions.Create(tableName, splitLine);
                        }
                        break;
                    case "droptable":
                        //DropTable Sample
                        //DropTable People
                        {
                            tableName = lineWords[1];
                            Functions.Drop(tableName);
                        }
                        break;
                    case "insert":
                        //Insert INTO Sample (Id, Name) VALUES (1, Иван)
                        //Insert INTO People (Id, Name, Town) VALUES (1, Валентин, Сливен) (2, Васил, Сливен) (3, Иван, Ямбол)
                        {
                            tableName = lineWords[2];

                            List<string> lineWords2 = HelpFuncs.TrimList(HelpFuncs.SplitString(line, new char[] { '(', ')' }));
                            List<string> userInputColumnOrder;
                            List<string> userInputData;
                            List<string> userInputColumnTypes;

                            if (!(HelpFuncs.ToLower(lineWords[1]) == "into") || !(HelpFuncs.ToLower(lineWords2[2]) == "values")) return;

                            try
                            {
                                if (HelpFuncs.SplitString(lineWords2[1], new char[] { ' ', ',' }).Count
                                    == HelpFuncs.SplitString(lineWords2[3], new char[] { ' ', ',' }).Count)
                                {
                                    for (int i = 3; i < lineWords2.Count; i++)
                                    {
                                        userInputColumnOrder = HelpFuncs.SplitString(lineWords2[1], new char[] { ' ', ',' });
                                        userInputData = HelpFuncs.SplitString(lineWords2[i], new char[] { ' ', ',' });
                                        userInputColumnTypes = HelpFuncs.GetUserOrderColumnTypes(tableName, userInputColumnOrder);

                                        if (!HelpFuncs.DefaultColumnsAreUsed(tableName, userInputColumnOrder))
                                        {
                                            continue;
                                        }

                                        if (userInputColumnOrder.Count == userInputData.Count && Parser<object>.TryParse(userInputData, userInputColumnTypes))
                                        {
                                            valueLines.Add(HelpFuncs.OrderUserInput(tableName, userInputData, userInputColumnOrder));
                                        }                                        
                                    }

                                    Functions.Insert(tableName, valueLines);
                                    valueLines.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input!\n\n\n");
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid command!\n\n\n");
                                break;
                            }
                        }
                        break;
                    case "delete":
                        {
                        }
                        break;
                    case "tableinfo":
                        //TableInfo Sample
                        //TableInfo People
                        {
                            tableName = lineWords[1];
                            Functions.TableInfo(tableName);
                        }
                        break;
                    case "listtables":
                        //ListTables
                        {
                            Functions.ListTables();
                        }
                        break;
                    case "select":
                        {
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
    }
}