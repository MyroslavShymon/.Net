using lab1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class Program
    {
        static List<Defence> defences;
        static List<Supervisor> supervisors;
        static List<Student> students;
        static List<Thesis> theses;
        static List<DefenceParticipants> defenceParticipants;

        static void Main()
        {
            CultureInfo culture = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("1. Generate data");
                Console.WriteLine("2. Show generate data");
                Console.WriteLine("3. Run queries");
                Console.WriteLine("4. Exit");

                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GenerateData();
                        break;
                    case "2":
                        if (defences == null || supervisors == null || students == null || theses == null || defenceParticipants == null)
                        {
                            Console.Write("Please generate data!\n");
                            break;
                        }
                        ShowGeneratedData();
                        break;
                    case "3":
                        if (defences == null || supervisors == null || students == null || theses == null || defenceParticipants == null)
                        {
                            Console.Write("Please generate data!\n");
                            break;
                        }
                        Queries.RunQueries(defences, supervisors, students, theses, defenceParticipants);
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void GenerateData()
        {
            Console.WriteLine("Do you want to enter the same number of data for all categories? (Y/N)");
            string response = Console.ReadLine().ToUpper();

            if (response == "Y")
            {
                int count = GetValidNumber("Enter the number of data to generate: ");

                GenerateObjects(count, count, count, count, count);
            }
            else
            {
                int defencesCount = GetValidNumber("Enter the number of defences to generate: ");
                int supervisorsCount = GetValidNumber("Enter the number of supervisors to generate: ");
                int studentsCount = GetValidNumber("Enter the number of students to generate: ");
                int thesesCount = GetValidNumber("Enter the number of theses to generate: ");
                int defenceParticipantsCount = GetValidNumber("Enter the number of defence participants to generate: ");

                GenerateObjects(defencesCount, supervisorsCount, studentsCount, thesesCount, defenceParticipantsCount);
            }
        }

        static void GenerateObjects(int defencesCount, int supervisorsCount, int studentsCount, int thesesCount, int defenceParticipantsCount)
        {
            defences = DataGenerator.GenerateDefences(defencesCount);
            supervisors = DataGenerator.GenerateSupervisors(supervisorsCount);
            students = DataGenerator.GenerateStudents(studentsCount, supervisors);
            theses = DataGenerator.GenerateTheses(thesesCount, students);
            defenceParticipants = DataGenerator.GenerateDefenceParticipants(defenceParticipantsCount, defences, students, supervisors);

            ShowGeneratedData();
        }

        static int GetValidNumber(string message)
        {
            int number;
            do
            {
                Console.Write(message);
            } while (!int.TryParse(Console.ReadLine(), out number) || number <= 0);
            return number;
        }
        static void ShowGeneratedData()
        {
            // Виводимо згенеровані дані
            Console.WriteLine("\nGenerated Theses:");
            foreach (var thesis in theses)
            {
                Console.WriteLine(thesis.ToString());
            }

            Console.WriteLine("\nGenerated Students:");
            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }

            Console.WriteLine("\nGenerated Defences:");
            foreach (var defence in defences)
            {
                Console.WriteLine(defence.ToString());
            }

            Console.WriteLine("\nGenerated Supervisors:");
            foreach (var supervisor in supervisors)
            {
                Console.WriteLine(supervisor.ToString());
            }

            Console.WriteLine("\nGenerated Defence Participants:");
            foreach (var dp in defenceParticipants)
            {
                Console.WriteLine(dp.ToString());
            }
        }
    }
}