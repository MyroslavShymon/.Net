using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal class ConsoleParser
    {
        public static int ParseList<T>(List<T> listOfObjects) where T : IHasId
        {
            int potentialObjectId;
            bool objectParsed = false;

            do
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out potentialObjectId))
                {
                    IHasId potentialObject = listOfObjects.FirstOrDefault(t => t.Id == potentialObjectId);

                    if (potentialObject != null)
                    {
                        objectParsed = true;
                    }
                    else
                    {
                        Console.WriteLine("Такого обєкту не існує");
                    }
                }
                else
                {
                    Console.WriteLine("Неправильне значення. Будь ласка, введіть ціле число.");
                }
            } while (!objectParsed);

            return potentialObjectId;
        }

        public static bool ParseBoolean()
        {
            bool potentialBoolean = false;
            bool booleanParsed = false;
            do
            {
                string remoteInput = Console.ReadLine();
                try
                {
                    potentialBoolean = bool.Parse(remoteInput);
                    booleanParsed = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неправильне значення. Будь ласка, введіть 'true' або 'false'.");
                }
            } while (!booleanParsed);

            return potentialBoolean;
        }

        public static DateTime ParseDateTime()
        {
            DateTime potentialDateTime;
            bool dateParsed = false;
            do
            {
                string input = Console.ReadLine();
                dateParsed = DateTime.TryParse(input, out potentialDateTime);
                if (!dateParsed)
                {
                    Console.WriteLine("Неправильний формат дати. Будь ласка, введіть дату у форматі 'рррр-мм-дд гг:хх'.");
                }
            } while (!dateParsed);

            return potentialDateTime;
        }

        public static int ParseInt()
        {
            int potentialNumber = 0;
            bool intParsed = false;
            do
            {
                string intInput = Console.ReadLine();
                try
                {
                    potentialNumber = int.Parse(intInput);
                    intParsed = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неправильний формат числа. Будь ласка, введіть ціле число.");
                }
            } while (!intParsed);

            return potentialNumber;
        }

        public static double ParseDouble()
        {
            int potentialDouble = 0;
            bool doubleParsed = false;
            do
            {
                string doubleInput = Console.ReadLine();
                try
                {
                    potentialDouble = int.Parse(doubleInput);
                    doubleParsed = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неправильний формат числа. Будь ласка, введіть число.");
                }
            } while (!doubleParsed);

            return potentialDouble;
        }
    }
}
