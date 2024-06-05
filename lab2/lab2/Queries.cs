using lab2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lab2
{
    static class Queries
    {
        public static XDocument doc;

        public static void LoadXml(string xmlFilePath)
        {
            doc = XDocument.Load(xmlFilePath);
        }
        public static void RunQueries(string xmlFilePath)
        {
            while (true)
            {
                Console.WriteLine("1 - Отримати всі захисти:");
                Console.WriteLine("2 - Отримати тези, які пише більше одного студента і вони не опубліковані:");
                Console.WriteLine("3 - Отримати викладача з більш ніж одним студентом в якого середній бал більше 75:");
                Console.WriteLine("4 - Додати нового студента і відобразити оновлений список:");
                Console.WriteLine("5 - Отримати диплом з ПІБ студента за умови, що диплом опублікований:");
                Console.WriteLine("6 - Отримати групи з кількістю студентів якщо студентів більше одного:");
                Console.WriteLine("7 - Отримати посортованих студентів в яких середній бал більше 70:");
                Console.WriteLine("8 - Отримати об'єднаних студентів з різними умовами:");
                Console.WriteLine("9 - Аналізування статей за кількістю сторінок:");
                Console.WriteLine("10 - Отримати список студентів разом з їхніми керівниками (ім'я, прізвище студента та керівника):");
                Console.WriteLine("11 - Порівнюємо два списка тезисів чи вони мають ідентичні id:");
                Console.WriteLine("12 - Отримати середній бал усіх студентів, які публікували свої тези:");
                Console.WriteLine("13 - Отримати кількість захистів, які були проведені віддалено:");
                Console.WriteLine("14 - Отримати список унікальних керівників, які мають студентів з середнім балом більше 90:");
                Console.WriteLine("15 - Обчислити середній вік студентів та супервізорів, а також проаналізувати кількість студентів і супервізорів у різних вікових групах:");
                Console.Write("Select a query (1-20) or enter '0' to exit: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "0":
                        return;
                    case "1":
                        GetAllDefences();
                        break;
                    case "2":
                        GetMultiStudentUnpublishedTheses();
                        break;
                    case "3":
                        GetSuccessfulSupervisors();
                        break;
                    case "4":
                        AddStudentAndDisplay(xmlFilePath);
                        break;
                    case "5":
                        GetThesesWithStudentFullNameByCondition();
                        break;
                    case "6":
                        GetGoupsWithCountOfStudents();
                        break;
                    case "7":
                        getOrderedStudentsWithAvarageGradeMoreThan70();
                        break;
                    case "8":
                        getUnionQueryWithCondition();
                        break;
                    case "9":
                        PerformThesisAnalysis();
                        break;
                    case "10":
                        GetStudentsWithSupervisors();
                        break;
                    case "11":
                        CompareThesisStudents();
                        break;
                    case "12":
                        CalculateAverageGradeOfPublishedTheses();
                        break;
                    case "13":
                        CountRemoteDefences();
                        break;
                    case "14":
                        GetUniqueSupervisorsWithTopStudents();
                        break;
                    case "15":
                        AgeAnalising();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void GetAllDefences()
        {
            var query = doc.Descendants("Defences").Elements("Defence").Select(defence => new
            {
                Id = (int)defence.Element("Id"),
                Name = (string)defence.Element("Name"),
                DefenseDate = (DateTime)defence.Element("DefenseDate"),
                Link = (string)defence.Element("Link"),
                Location = (string)defence.Element("Location"),
                IsRemote = (bool)defence.Element("IsRemote")
            }).OrderBy(d => d.Id);

            Console.WriteLine("\nShow all defences:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"Id: {item.Id}");
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Defense Date: {item.DefenseDate}");
                Console.WriteLine($"Link: {item.Link}");
                Console.WriteLine($"Location: {item.Location}");
                Console.WriteLine($"Is Remote: {item.IsRemote}");
                Console.WriteLine();
            }
        }

        private static void GetMultiStudentUnpublishedTheses()
        {
            var query = doc.Descendants("Theses").Elements("Thesis")
                           .Where(thesis => thesis.Elements("Students").Elements("Student").Count() > 1 &&
                                            (bool)thesis.Element("IsPublished") == false)
                           .Select(thesis => new
                           {
                               Id = (int)thesis.Element("Id"),
                               Title = (string)thesis.Element("Title"),
                               PagesCount = (int)thesis.Element("PagesCount"),
                               Grade = (int)thesis.Element("Grade"),
                               IsPublished = (bool)thesis.Element("IsPublished")
                           });

            Console.WriteLine("\nTheses that are written by more than one student and are not published:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"Id: {item.Id}");
                Console.WriteLine($"Title: {item.Title}");
                Console.WriteLine($"Pages Count: {item.PagesCount}");
                Console.WriteLine($"Grade: {item.Grade}");
                Console.WriteLine($"Is Published: {item.IsPublished}");
                Console.WriteLine();
            }
        }

        private static void GetSuccessfulSupervisors()
        {
            var query = from supervisor in doc.Descendants("Supervisors").Elements("Supervisor")
                        let advisees = supervisor.Element("Advisees").Elements("Advisee")
                        where advisees.Count() > 2 &&
                              advisees.All(advisee => (double)advisee.Element("AverageGrade") > 75)
                        select new
                        {
                            Id = (int)supervisor.Element("Id"),
                            FirstName = (string)supervisor.Element("FirstName"),
                            LastName = (string)supervisor.Element("LastName"),
                            MiddleName = (string)supervisor.Element("MiddleName"),
                            BirthDate = (DateTime)supervisor.Element("BirthDate")
                        };

            Console.WriteLine("\nGet supervisors with more than two students and all students must have an average grade of more than 75 points:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"Id: {item.Id}");
                Console.WriteLine($"First Name: {item.FirstName}");
                Console.WriteLine($"Last Name: {item.LastName}");
                Console.WriteLine($"Middle Name: {item.MiddleName}");
                Console.WriteLine($"Birth Date: {item.BirthDate}");
                Console.WriteLine();
            }
        }

        public static void AddStudentAndDisplay(string xmlFilePath)
        {
            var newStudent = new XElement("Student",
                new XElement("Id", 100),
                new XElement("Group", "IT-101"),
                new XElement("AverageGrade", 85.5),
                new XElement("LastName", "New"),
                new XElement("FirstName", "Student"),
                new XElement("MiddleName", "Middle"),
                new XElement("BirthDate", DateTime.Now),
                new XElement("ThesisId", 1),
                new XElement("SupervisorId", 1)
            );

            // Знаходимо елемент Students
            var studentsElement = doc.Root?.Element("Students");
            // Додаємо нового студента до елемента Students
            studentsElement?.Add(newStudent);

            // Зберігаємо зміни в XML файл
            doc.Save(xmlFilePath);

            Console.WriteLine("\nThe query adds a new student and displays the updated list:\n");
            DisplayStudents();
        }

        private static void DisplayStudents()
        {
            var students = doc.Root?.Element("Students")?.Elements("Student");
            foreach (var student in students)
            {
                Console.WriteLine($"Id: {student.Element("Id").Value}");
                Console.WriteLine($"Group: {student.Element("Group").Value}");
                Console.WriteLine($"Average Grade: {student.Element("AverageGrade").Value}");
                Console.WriteLine($"Last Name: {student.Element("LastName").Value}");
                Console.WriteLine($"First Name: {student.Element("FirstName").Value}");
                Console.WriteLine($"Middle Name: {student.Element("MiddleName").Value}");
                Console.WriteLine($"Birth Date: {student.Element("BirthDate").Value}");
                Console.WriteLine($"Thesis Id: {student.Element("ThesisId").Value}");
                Console.WriteLine($"Supervisor Id: {student.Element("SupervisorId").Value}");
                Console.WriteLine();
            }
        }

        private static void GetThesesWithStudentFullNameByCondition()
        {
            var query = from thesis in doc.Descendants("Theses").Elements("Thesis")
                        from student in thesis.Elements("Students").Elements("Student")
                        let studentFullName = $"{student.Element("LastName").Value} {student.Element("FirstName").Value}"
                        where (bool)thesis.Element("IsPublished")
                        select new
                        {
                            ThesisTitle = thesis.Element("Title").Value,
                            StudentFullName = studentFullName
                        };

            Console.WriteLine("\nTheses with students where theses are published:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"Thesis: {item.ThesisTitle}, Student: {item.StudentFullName}");
            }
        }

        private static void GetGoupsWithCountOfStudents()
        {
            var query = from student in doc.Descendants("Students").Elements("Student")
                        group student by student.Element("Group").Value into groupedStudents
                        orderby groupedStudents.Count() descending
                        where groupedStudents.Count() > 1
                        select new
                        {
                            Group = groupedStudents.Key,
                            StudentsCount = groupedStudents.Count()
                        };

            Console.WriteLine("\nGet groups with count of students with descending order:\n");
            Console.WriteLine("Group\tCount");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.Group}\t{item.StudentsCount}");
            }
        }

        private static void getOrderedStudentsWithAvarageGradeMoreThan70()
        {
            var query = doc.Descendants("Students").Elements("Student")
                           .Skip(5)
                           .Where(student => (double)student.Element("AverageGrade") > 70)
                           .OrderBy(student => (double)student.Element("AverageGrade"))
                           .ThenBy(student => (string)student.Element("LastName"))
                           .ThenBy(student => (string)student.Element("Group"))
                           .Reverse();

            Console.WriteLine("\nGet students ordered by Group and LastName, AverageGrade and Skip first 5 elements also students must have average grade more than 70: \n");
            foreach (var student in query)
            {
                Console.WriteLine(student);
            }
        }

        private static void getUnionQueryWithCondition()
        {
            DateTime startDate = new DateTime(2002, 1, 1);
            DateTime endDate = new DateTime(2010, 12, 31);

            var query = (from student in doc.Descendants("Students").Elements("Student")
                         .Union(doc.Descendants("Students").Elements("Student"))
                         where (double)student.Element("AverageGrade") > 75
                               && student.Element("Thesis") != null
                               && (bool)student.Element("Thesis").Element("IsPublished")
                               && (DateTime)student.Element("BirthDate") >= startDate
                               && (DateTime)student.Element("BirthDate") <= endDate
                         orderby (double)student.Element("AverageGrade") descending
                         select student
                        )
                        .Skip(2)
                        .Take(5)
                        .Distinct();

            Console.WriteLine("\nThe query selects students from the union of the two lists, taking into account conditions such as GPA above 75, having and publishing a degree, and filtering by date of birth. The results are sorted by descending average score, limited to a sample of a certain number, and duplicates are removed: \n");

            foreach (var student in query)
            {
                Console.WriteLine(student);
            }
        }

        private static void PerformThesisAnalysis()
        {
            var query = (from thesis in doc.Descendants("Theses").Elements("Thesis")
                         group thesis by (bool)thesis.Element("IsPublished") into grouped
                         orderby grouped.Key descending
                         select new
                         {
                             IsPublished = grouped.Key,
                             MinPagesCount = grouped.Min(t => (int)t.Element("PagesCount")),
                             MaxPagesCount = grouped.Max(t => (int)t.Element("PagesCount")),
                             AveragePagesCount = grouped.Average(t => (int)t.Element("PagesCount")),
                             ThesesCount = grouped.Count()
                         });

            Console.WriteLine("Опублікованість\t\tМін\tМакс\tСередня\tКількість статей");
            foreach (var item in query)
            {
                string isPublishedString = item.IsPublished ? "Опублі. статті: " : "Не опуб. статті: ";
                Console.WriteLine($"{isPublishedString}\t\t{item.MinPagesCount}\t{item.MaxPagesCount}\t{item.AveragePagesCount}\t{item.ThesesCount}");
            }
        }

        public static void CompareThesisStudents()
        {
            var thesis1Ids = doc.Descendants("Theses").Elements("Thesis").Select(t => (int)t.Element("Id"));
            var thesis2Ids = doc.Descendants("Theses").Elements("Thesis").Select(t => (int)t.Element("Id"));

            bool areEqual = thesis1Ids.SequenceEqual(thesis2Ids);

            Console.WriteLine($"\nThe query compares theses using SequenceEqual: {areEqual}\n");
        }

        static void GetStudentsWithSupervisors()
        {
            var query = from student in doc.Descendants("Students").Elements("Student")
                        join supervisor in doc.Descendants("Supervisors").Elements("Supervisor") on (int)student.Element("SupervisorId") equals (int)supervisor.Element("Id")
                        select new
                        {
                            StudentName = $"{student.Element("FirstName").Value} {student.Element("LastName").Value}",
                            SupervisorName = $"{supervisor.Element("FirstName").Value} {supervisor.Element("LastName").Value}"
                        };

            Console.WriteLine("\nStudents with their supervisors:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"Student: {item.StudentName}, Supervisor: {item.SupervisorName}");
            }
        }

        static void CalculateAverageGradeOfPublishedTheses()
        {
            var averageGrade = doc.Descendants("Students").Elements("Student")
                .Where(student => student.Element("Thesis") != null &&
                                  student.Element("Thesis").Element("IsPublished") != null &&
                                  (bool)student.Element("Thesis").Element("IsPublished"))
                .Select(student => (double)student.Element("AverageGrade"))
                .DefaultIfEmpty(0)
                .Average();

            Console.WriteLine($"\nAverage grade of students who published their theses: {averageGrade}\n");
        }

        static void CountRemoteDefences()
        {
            var count = doc.Descendants("Defences").Elements("Defence").Count(defence => (bool)defence.Element("IsRemote"));

            Console.WriteLine($"\nNumber of defences conducted remotely: {count}\n");
        }

        static void GetUniqueSupervisorsWithTopStudents()
        {
            var query = doc.Descendants("Students").Elements("Student")
                .Where(student => (double)student.Element("AverageGrade") > 90)
                .Select(student => (int)student.Element("SupervisorId"))
                .Distinct()
                .Join(doc.Descendants("Supervisors").Elements("Supervisor"), sid => sid, supervisor => (int)supervisor.Element("Id"), (sid, supervisor) => supervisor);

            Console.WriteLine("\nUnique supervisors with top-performing students:\n");
            foreach (var supervisor in query)
            {
                Console.WriteLine($"Supervisor: {supervisor.Element("FirstName").Value} {supervisor.Element("LastName").Value}");
            }
        }

        static void AgeAnalising()
        {
            // Середній вік студентів
            var averageStudentAge = doc.Descendants("Students").Elements("Student")
                .Select(student => DateTime.Now.Year - DateTime.Parse(student.Element("BirthDate").Value).Year)
                .Average();

            Console.WriteLine($"Average age of students: {averageStudentAge}");

            // Середній вік супервізорів
            var averageSupervisorAge = doc.Descendants("Supervisors").Elements("Supervisor")
                .Select(supervisor => DateTime.Now.Year - DateTime.Parse(supervisor.Element("BirthDate").Value).Year)
                .Average();

            Console.WriteLine($"Average age of supervisors: {averageSupervisorAge}");

            // Кількість студентів в різних вікових групах
            var studentAgeGroups = doc.Descendants("Students").Elements("Student")
                .GroupBy(student => DateTime.Now.Year - DateTime.Parse(student.Element("BirthDate").Value).Year)
                .OrderBy(group => group.Key)
                .Select(group => new
                {
                    AgeGroup = group.Key,
                    StudentsCount = group.Count()
                });

            Console.WriteLine("Students count by age group:");
            foreach (var group in studentAgeGroups)
            {
                Console.WriteLine($"Age Group: {group.AgeGroup}, Students Count: {group.StudentsCount}");
            }

            // Кількість супервізорів в різних вікових групах
            var supervisorAgeGroups = doc.Descendants("Supervisors").Elements("Supervisor")
                .GroupBy(supervisor => DateTime.Now.Year - DateTime.Parse(supervisor.Element("BirthDate").Value).Year)
                .OrderBy(group => group.Key)
                .Select(group => new
                {
                    AgeGroup = group.Key,
                    SupervisorsCount = group.Count()
                });

            Console.WriteLine("Supervisors count by age group:");
            foreach (var group in supervisorAgeGroups)
            {
                Console.WriteLine($"Age Group: {group.AgeGroup}, Supervisors Count: {group.SupervisorsCount}");
            }
        }
    }
}
