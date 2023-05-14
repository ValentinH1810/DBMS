using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS_
{
    class HelpFuncs
    {
        public static string path = @"..\..\..\Tables\";

        public static List<string> Split(string text, char splitChar)
        {
            List<string> splitText = new List<string>();
            int currentIndex = 0;
            int startIndex = 0;

            foreach (char character in text)
            {
                if (character == splitChar)
                {
                    splitText.Add(Substring(text, startIndex, currentIndex));
                    startIndex = currentIndex + 1;
                }
                currentIndex++;
            }

            //has split -> add last text
            if (startIndex != 0)
            {
                if (startIndex == text.Length) return splitText;
                splitText.Add(Substring(text, startIndex, text.Length));
            }
            else
            {
                splitText.Add(text);
            }

            return splitText;
        }


        public static List<string> SplitString(string text, char[] regex)
        {
            List<string> splitText = new List<string>();
            int currentIndex = 0;
            int startIndex = 0;
            bool hasEntered = false;

            foreach (char character in text)
            {
                foreach (char regexCharacter in regex)
                {
                    if (regexCharacter == character && !hasEntered)
                    {
                        if (startIndex == currentIndex)
                        {
                            startIndex++;
                            break;
                        }

                        splitText.Add(Substring(text, startIndex, currentIndex));
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
                if (startIndex == text.Length) return splitText;
                splitText.Add(Substring(text, startIndex, text.Length));
            }
            else
            {
                splitText.Add(text);
            }
            return splitText;
        }

        public static string Substring(string text, int start, int end)
        {
            char[] substringArray = new char[end - start];
            int counter = 0;
            for (int i = start; i < end; i++, counter++)
            {
                substringArray[counter] = CharAt(text, i);
            }

            return new string(substringArray);
        }

        public static char CharAt(string text, int index)
        {
            return text[index];
        }

        public static string ToLower(string text)
        {
            char[] textToLowerCase = new char[text.Length];
            int counter = 0;
            foreach (char letter in text)
            {
                //Check if upper case...
                if ((int)HelpFuncs.CharAt(text, counter) >= 65 && (int)HelpFuncs.CharAt(text, counter) <= 90)
                {
                    textToLowerCase[counter] = (char)((int)HelpFuncs.CharAt(text, counter) + 32);
                }
                else
                {
                    textToLowerCase[counter] = HelpFuncs.CharAt(text, counter);
                }
                counter++;
            }
            return new string(textToLowerCase);
        }

        public static List<string> TrimList(List<string> inputList)
        {
            for (int i = 0; i < inputList.Count; i++)
            {
                int start = 0;
                int end = inputList[i].Length - 1;

                while (start < inputList[i].Length && inputList[i][start] == ' ')
                {
                    start++;
                }
                while (end >= 0 && inputList[i][end] == ' ')
                {
                    end--;
                }

                string output = "";

                for (int j = start; j <= end; j++)
                {
                    output += inputList[i][j];
                }

                inputList[i] = output;
            }
            return inputList;
        }

        public static List<string> GetColumnTypeOrder(string tableName)
        {
            try
            {
                string fileName = path + $"{tableName}.txt";
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line;
                    int counter = 1;
                    List<string> columnTypeOrder = new List<string>();
                    string dataTypeWithDefault;
                    string dataType;

                    while ((line = file.ReadLine()) != null)
                    {
                        if (counter == 3)
                        {
                            List<string> columns = Split(line, '\t');
                            foreach (var column in columns)
                            {
                                dataTypeWithDefault = SplitString(column, new char[] { ' ', '(', ')' })[1];
                                dataType = Split(dataTypeWithDefault, ' ')[0];
                                columnTypeOrder.Add(dataType);
                            }
                            return columnTypeOrder;
                        }
                        counter++;
                    }
                    return columnTypeOrder;
                }
            }
            catch (FileNotFoundException ex) 
            {
            } 
            catch (IOException ex) 
            {
                throw new IOException();
            }

            return null;
        }

        public static List<string> GetColumnOrder(string tableName)
        {
            try
            {
                string fileName = path + $"{tableName}.txt";
                List<string> columnOrder = new List<string>();

                using (StreamReader file = new StreamReader(fileName))
                {
                    string line;
                    int counter = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (counter == 3)
                        { 
                            List<string> columns = Split(line, '\t');
                            foreach (var column in columns)
                            {
                                columnOrder.Add(SplitString(column, new char[] { '(', ')' })[0]);
                            }
                        }
                        counter++;
                    }
                    return columnOrder;
                }
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (IOException ex)
            {
                throw new IOException();
            }

            return null;
        }

        public static string GetColumnRow(string tableName)
        {
            try
            {
                string fileName = path + $"{tableName}.txt";
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line;
                    int counter = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (counter == 3)
                        {
                            return line;
                        }
                        counter++;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (IOException ex)
            {
                throw new IOException();
            }

            return null;
        }

        public static List<string> GetUserOrderColumnTypes(string tableName, List<string> userInputColumnOrder)
        {
            List<string> userOrderColumnTypes = new List<string>();
            List<string> columnTypeOrder = GetColumnTypeOrder(tableName);
            List<string> columnOrder = GetColumnOrder(tableName);

            int counter = 0;
            for (int i = 0; i < userInputColumnOrder.Count; i++)
            {
                foreach (var column in columnOrder)
                {
                    if (SplitString(column, new char[] { '(', ')' })[0] == userInputColumnOrder[i])
                    {
                        userOrderColumnTypes.Add(columnTypeOrder[counter]);
                    }
                    counter++;
                }
                counter = 0;
            }

            return userOrderColumnTypes;
        }

        public static List<string> OrderUserInput(string tableName, List<string> userInput, List<string> userInputColumnOrder)
        {
            List<string> sortedRow = new List<string>();
            List<string> columnOrder = GetColumnOrder(tableName);

            int counter = 0;
            string column;
            for (int i = 0; i < columnOrder.Count; i++)
            {
                column = columnOrder[i];
                for (int j = 0; j < columnOrder.Count; j++)
                {
                    try
                    {
                        if (column == userInputColumnOrder[j])
                        {
                            sortedRow.Add(userInput[counter]);
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        if (sortedRow.Count >= i + 1)
                        {
                            break;
                        }
                         sortedRow.Add(GetDefaultValue(tableName, columnOrder[i]));
                        j = columnOrder.Count;
                    }
                    counter++;
                }

                //Default value
                if (sortedRow.Count < i + 1)
                {
                    sortedRow.Add(GetDefaultValue(tableName, columnOrder[i]));
                }
                counter = 0;
            }
            return sortedRow;
        }

        public static string GetDefaultValue(string tableName, string userColumn)
        {
            List<string> columnOrder = GetColumnOrder(tableName);


            int selectedColumnIndex = 0;
            for (int i = 0; i < columnOrder.Count; i++)
            {
                if (columnOrder[i] == userColumn)
                {
                    selectedColumnIndex = i;
                }
            }

            try
            {
                string fileName = path + $"{tableName}.txt";
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line;
                    int counter = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (counter == 3)
                        { 
                            try
                            {
                                List<string> columns = Split(line, '\t');
                                string selectedColumn = columns[selectedColumnIndex];
                                List<string> columnData = SplitString(selectedColumn, new char[] { ' ', '(', ')' });

                                if (columnData.Count > 2 && columnData[2] == "default")
                                {
                                    string defaultValue = columnData[3];
                                    return defaultValue;
                                }
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                return null;
                            }
                        }
                        counter++;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (IOException ex)
            {
                throw new IOException();
            }

            return null;
        }

        //If all default columns are used in the user command
        public static bool DefaultColumnsAreUsed(string tableName, List<string> userInputColumnOrder)
        {
            string columnRow = GetColumnRow(tableName);
            List<string> columnRowSplit = Split(columnRow, '\t');
            List<string> columnData;
            string columnName;
            int defaultCounter = 0;

            foreach (var column in columnRowSplit)
            {
                columnData = SplitString(column, new char[] { ' ', '(', ')' });
                columnName = columnData[0];

                if (columnData.Count > 2 && columnData[2] == "default" && !userInputColumnOrder.Contains(columnName))
                {
                    defaultCounter++;
                }
            }

            return userInputColumnOrder.Count + defaultCounter == columnRowSplit.Count;
        }

        public static bool ValidateTables(List<string> tables)
        {
            bool isValid = true;
            string path = @"..\..\..\Tables\";          

            List<List<string>> columnTypesInAllFiles = new List<List<string>>();
            List<List<List<string>>> lines = new List<List<List<string>>>();

            {
                int filesCounter = 0;
                int linesCounter = 0;

                for (int i = 0; i < tables.Count; i++)
                {
                    int valuesCounter = 0;

                    string filePath = path + $"{tables[i]}.txt";

                    if (File.Exists(filePath))
                    {
                        columnTypesInAllFiles.Add(new List<string>());
                        lines.Add(new List<List<string>>());

                        string[] context = File.ReadAllLines(filePath);
                        string tempColumn = null;

                        for (int j = 0; j < context[2].Length; j++)
                        {
                            if (context[2][j] == '(')
                            {
                                j++;

                                while (context[2][j] != ')' && context[2][j] != ' ')
                                {
                                    tempColumn += context[2][j];
                                    j++;
                                }

                                columnTypesInAllFiles[filesCounter].Add(tempColumn);
                                tempColumn = null;
                            }
                        }

                        filesCounter++;

                        string tempValue = null;

                        for (int k = 3; k < context.Length; k++)
                        {
                            lines[linesCounter].Add(new List<string>());

                            for (int n = 0; n < context[k].Length; n++)
                            {
                                if (context[k][n] != '\t')
                                {
                                    while (context[k][n] != '\t' && n != context[k].Length - 1)
                                    {
                                        tempValue += context[k][n];
                                        n++;
                                    }

                                    lines[linesCounter][valuesCounter].Add(tempValue);

                                    tempValue = null;
                                }
                            }

                            valuesCounter++;
                        }

                        linesCounter++;
                    }
                }
            }

            List<string> userInput = new List<string>();
            List<string> userInputColumnTypes = new List<string>();

            for (int i = 0; i < columnTypesInAllFiles.Count; i++)
            {
                for (int j = 0; j < columnTypesInAllFiles[i].Count; j++)
                {
                    userInputColumnTypes.Add(columnTypesInAllFiles[i][j]);
                }

                for (int j = 0; j < lines[i].Count; j++)
                {
                    for (int k = 0; k < lines[i][j].Count; k++)
                    {
                        userInput.Add(lines[i][j][k]);
                    }

                    isValid = Parser<object>.TryParse(userInput, userInputColumnTypes);

                    if (!isValid)
                    {
                        Console.WriteLine($"Errors have been found in {tables[i]}.txt in row {j + 4}!");
                        break;
                    }

                    userInput.Clear();
                }

                if (!isValid)
                    break;

                userInputColumnTypes.Clear();
            }
         
            return isValid;
        }
    }
}