using lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal class ConsoleInput
    {
        public static List<DefenceParticipant> CreateDefenceParticipantsFromConsoleInput(int participantCount, List<Defence> defences, List<Student> students, List<Supervisor> supervisors)
        {
            List<DefenceParticipant> participants = new List<DefenceParticipant>();

            for (int i = 0; i < participantCount; i++)
            {
                Console.WriteLine($"Введіть дані для учасника захисту №{i + 1}:");
                Console.WriteLine($"ID: ");
                int id = ConsoleParser.ParseInt();
                Console.WriteLine($"ID захисту: ");
                int defenceId = ConsoleParser.ParseInt();
                Console.WriteLine($"ID учасника: ");
                int participantId = ConsoleParser.ParseInt();
                Console.Write("Роль (Student/Supervisor/Guest): ");
                string roleString = Console.ReadLine();
                Role role;
                Enum.TryParse(roleString, true, out role);

                try
                {
                    DefenceParticipant participant = new DefenceParticipant(id, defenceId, participantId, defences, students, supervisors, role);
                    participants.Add(participant);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    i--; // Повторити введення для поточного учасника захисту
                }
            }

            return participants;
        }
       
        public static List<Defence> CreateDefencesFromConsoleInput(int defenceCount)
        {
            List<Defence> defences = new List<Defence>();

            for (int i = 0; i < defenceCount; i++)
            {
                Console.WriteLine($"Введіть дані для захисту №{i + 1}:");
                Console.WriteLine("ID: ");
                int id = ConsoleParser.ParseInt();
                Console.Write("Назва: ");
                string name = Console.ReadLine();
                Console.WriteLine("Дата захисту (рррр-мм-дд гг:хх): ");
                DateTime defenseDate = ConsoleParser.ParseDateTime();
                Console.Write("Посилання: ");
                string link = Console.ReadLine();
                Console.Write("Місце проведення: ");
                string location = Console.ReadLine();
                Console.Write("Віддалений (true/false): ");
                bool isRemote = ConsoleParser.ParseBoolean();
/*
                Defence defence = new Defence(id, name, defenseDate, link, location, isRemote);
                defences.Add(defence);*/
            }

            return defences;
        }
       
        public static (List<Student> students, List<Thesis> resultTheses, List<Supervisor> resultSupervisors) CreateStudentsFromConsoleInput(int studentCount, List<Supervisor> supervisors, List<Thesis> theses)
        {
            List<Student> students = new List<Student>();
            List<Thesis> resultTheses = new List<Thesis>();
            List<Supervisor> resultSupervisors = new List<Supervisor>();

            for (int i = 0; i < studentCount; i++)
            {
                Console.WriteLine($"Введіть дані для студента №{i + 1}:");
                Console.WriteLine("ID: ");
                int id = ConsoleParser.ParseInt();
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine();
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine();
                Console.Write("По батькові: ");
                string middleName = Console.ReadLine();
                Console.WriteLine("Дата народження (рррр-мм-дд): ");
                DateTime birthDate = ConsoleParser.ParseDateTime();
                Console.Write("Група: ");
                string group = Console.ReadLine();
                Console.Write("Середній бал: ");
                double averageGrade = ConsoleParser.ParseDouble();
                Console.Write("ID диплому: ");
                int thesisId = ConsoleParser.ParseList(theses);
                Console.Write("ID супервізора: ");
                int supervisorId = ConsoleParser.ParseList(supervisors);

                Student student = new Student(id, group, averageGrade, lastName, firstName, middleName, birthDate, thesisId, supervisorId);
                student.Thesis = theses.Find(t => t.Id == thesisId);
                student.Supervisor = supervisors.Find(s => s.Id == supervisorId);

                Thesis resultThesis = theses.Find(t=> t.Id == student.ThesisId);
                foreach (var thesis in theses)
                {
                    thesis.Students.Add(student);
                }


                Supervisor resultSupervisor = supervisors.Find(s => s.Id == student.SupervisorId);
                foreach (var supervisor in supervisors)
                {
                    supervisor.AddAdvisee(student);
                }
   
                students.Add(student);
            }
            resultTheses = theses;
            resultSupervisors = supervisors;
            return (students, resultTheses, resultSupervisors);
        }
      
        public static List<Supervisor> CreateSupervisorsFromConsoleInput(int supervisorCount)
        {
            List<Supervisor> supervisors = new List<Supervisor>();

            for (int i = 0; i < supervisorCount; i++)
            {
                Console.WriteLine($"Введіть дані для супервізора №{i + 1}:");
                Console.WriteLine("ID: ");
                int id = ConsoleParser.ParseInt();
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine();
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine();
                Console.Write("По батькові: ");
                string middleName = Console.ReadLine();
                Console.Write("Дата народження (рррр-мм-дд): ");
                DateTime birthDate = ConsoleParser.ParseDateTime();
                Console.Write("Посада: ");
                string position = Console.ReadLine();

                Supervisor supervisor = new Supervisor(id, position, lastName, firstName, middleName, birthDate);
                
                supervisors.Add(supervisor);
            }

            return supervisors;
        }

        public static List<Thesis> CreateThesesFromConsoleInput(int thesisCount)
        {
            List<Thesis> theses = new List<Thesis>();

            for (int i = 0; i < thesisCount; i++)
            {
                Console.WriteLine($"Введіть дані для дипломної роботи №{i + 1}:");
                Console.WriteLine("ID: ");
                int id = ConsoleParser.ParseInt();
                Console.Write("Назва: ");
                string title = Console.ReadLine();
                Console.Write("Кількість сторінок: ");
                int pagesCount = ConsoleParser.ParseInt();
                Console.Write("Оцінка: ");
                int grade = ConsoleParser.ParseInt();
                Console.Write("Опублікована (true/false): ");
                bool isPublished = ConsoleParser.ParseBoolean();

                Thesis thesis = new Thesis(id, title, pagesCount, grade, isPublished);
                theses.Add(thesis);
            }

            return theses;
        }
    }
}
