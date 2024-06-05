using lab2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace lab2
{
    internal class Program
    {
        static University loadedUniversity;
        static XmlDocument loadedUniversityByXmlDocument;
        static List<Defence> defences;
        static List<Supervisor> supervisors;
        static List<Student> students;
        static List<Thesis> theses;
        static List<DefenceParticipant> defenceParticipants;

        static void Main()
        {
            CultureInfo culture = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("1. Enter data");
                Console.WriteLine("2. Generate data");
                Console.WriteLine("3. Show generate data");
                Console.WriteLine("4. Run queries");
                Console.WriteLine("5. Write XML");
                Console.WriteLine("6. Load from XML using XmlSerializer");
                Console.WriteLine("7. Load from XML using XmlDocument");
                Console.WriteLine("8. Show data from XML loaded by XmlSerializer");
                Console.WriteLine("9. Show data from XML loaded by XmlDocument");
                Console.WriteLine("10. Exit");

                Console.Write("Select an option: ");
                string choice = Console.ReadLine();
                string filePath = "university.xml";
                XmlHandler xmlHandler = new XmlHandler(students, supervisors, theses, defences, defenceParticipants);

                switch (choice)
                {
                    case "1":
                        InputObjects();
                        break;
                    case "2":
                        GenerateData();
                        break;
                    case "3":
                        if (defences == null || supervisors == null || students == null || theses == null || defenceParticipants == null)
                        {
                            Console.Write("Please generate data!\n");
                            break;
                        }
                        ShowGeneratedData();
                        break;
                    case "4":
                        Queries.LoadXml(filePath);
                        Queries.RunQueries(filePath);
                        break;
                    case "5":
                        xmlHandler.WriteXml(filePath);
                        break;
                    case "6":
                        loadedUniversity = xmlHandler.LoadXml(filePath);
                        Console.WriteLine("Дані завантажені");
                        break;
                    case "7":
                        loadedUniversityByXmlDocument = xmlHandler.LoadXmlUsingXmlDocument(filePath);
                        Console.WriteLine("Дані завантажені");
                        break;
                    case "8":
                        xmlHandler.DisplayDeserializedData(loadedUniversity);
                        break;
                    case "9":
                        xmlHandler.showDataLoadedByXmlDocument(loadedUniversityByXmlDocument);
                        break;
                    case "10":
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

        static void InputObjects() {
            int defencesCount = GetValidNumber("Enter the number of defences to input: ");
            int supervisorsCount = GetValidNumber("Enter the number of supervisors to input: ");
            int studentsCount = GetValidNumber("Enter the number of students to input: ");
            int thesesCount = GetValidNumber("Enter the number of theses to input: ");
            int defenceParticipantsCount = GetValidNumber("Enter the number of defence participants to input: ");

            defences = ConsoleInput.CreateDefencesFromConsoleInput(defencesCount);
            supervisors = ConsoleInput.CreateSupervisorsFromConsoleInput(supervisorsCount);
            theses = ConsoleInput.CreateThesesFromConsoleInput(thesesCount);
            var (resultStudents, resultTheses, resultSupervisors) = ConsoleInput.CreateStudentsFromConsoleInput(studentsCount, supervisors, theses);
            supervisors = resultSupervisors;
            theses = resultTheses;
            students = resultStudents;
            defenceParticipants = ConsoleInput.CreateDefenceParticipantsFromConsoleInput(defenceParticipantsCount, defences, students, supervisors);

            ShowGeneratedData();
        }

        static void GenerateObjects(int defencesCount, int supervisorsCount, int studentsCount, int thesesCount, int defenceParticipantsCount)
        {
            defences = DataGenerator.GenerateDefences(defencesCount);
            supervisors = DataGenerator.GenerateSupervisors(supervisorsCount);
            theses = DataGenerator.GenerateTheses(thesesCount);
            students = DataGenerator.GenerateStudents(studentsCount, supervisors, theses);
            foreach (Thesis thesis in theses)
            {
                thesis._students = students.FindAll(s => s.ThesisId == thesis.Id);
            }
            foreach (Supervisor supervisor in supervisors)
            {
                foreach (Student student in students)
                {
                    if(student.SupervisorId == supervisor.Id)
                    {
                        supervisor.AddAdvisee(student);
                    }
                }
            }
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