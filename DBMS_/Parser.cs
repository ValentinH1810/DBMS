using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS_
{
    static class Parser<T>
    {
        public static bool TryParse(List<string> userInput, List<string> userInputColumnTypes)
        {
            if (userInput.Count != userInputColumnTypes.Count)
            {
                return false;
            }

            for (int i = 0; i < userInput.Count; i++)
            {
                var input = userInput[i];
                var inputType = userInputColumnTypes[i];

                switch (inputType)
                {
                    case "int":
                        if (!ValidateInt(input))
                        {
                            return false;
                        }
                        break;
                    case "double":
                        if (!ValidateDouble(input))
                        {
                            return false;
                        }
                        break;
                    case "date":
                        if(!ValidateDate(input))
                        {
                            return false;
                        }
                        break;
                    case "string":
                        if (!ValidateString(input))
                        {
                            return false;
                        }
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }

        public static bool TryParse(string userInput, string userInputColumnType)
        {
            try
            {
                switch (HelpFuncs.ToLower(userInputColumnType))
                {
                    case "int":
                        if (!ValidateInt(userInput))
                        {
                            return false;
                        }
                        break;
                    case "double":
                        if (!ValidateDouble(userInput))
                        {
                            return false;
                        }
                        break;
                    case "date":
                        if(!ValidateDate(userInput))
                        {
                            return false;
                        }
                        break;
                    case "string":
                        if (!ValidateString(userInput))
                        {
                            return false;
                        }
                        break;
                    default:
                        return false;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return true;
            }

            return true;
        }

        public static bool ValidateDate(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                int asciiValue = (int)input[i];
                if (!((asciiValue <= 57 && asciiValue >= 48) || asciiValue == 46 || asciiValue == 34))
                {
                    return false;
                }
            }

            string[] dateElements = input.Split('.');
            if (dateElements.Length != 3) return false;
            int year, month, day;

            try
            {
                day = int.Parse(dateElements[0]);
                month = int.Parse(dateElements[1]);
                year = int.Parse(dateElements[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (month < 1 || month > 12) return false;
            if (day < 1 || day > DaysInMonth(month, year)) return false;
            if (year < 1 || year > 9999) return false;
            return true;
        }

        private static int DaysInMonth(int month, int year)
        {
            switch (month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    if (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0))
                        return 29;
                    else
                        return 28;
                default:
                    return 31;
            }
        }

        private static bool ValidateInt(string text)
        {
            int asciiValue; //ASCII Table range of numbers try each char...
            for (int i = 0; i < text.Length; i++)
            {
                asciiValue = (int)HelpFuncs.CharAt(text, i);
                if (!(asciiValue <= 57 && asciiValue >= 48))
                {
                    return false;
                }
            }
            return true;
        }

        
        private static bool ValidateString(string text)
        {
            try
            {
                int asciiValue;
                for (int i = 0; i < text.Length; i++)
                {
                    asciiValue = (int)HelpFuncs.CharAt(text, i);
                    if ((asciiValue <= 57 && asciiValue >= 48))
                    {
                        return false;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Errors have been found in some of the tables!");
                Environment.Exit(0);
            }

            return true;
        }

        public static bool ValidateDouble(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                int asciiValue = (int)HelpFuncs.CharAt(text, i);
                if (!((asciiValue <= 57 && asciiValue >= 48) || asciiValue == 46))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
