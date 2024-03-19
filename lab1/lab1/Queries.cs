using lab1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    static class Queries
    {
        public static void RunQueries(
            List<Defence> defences,
            List<Supervisor> supervisors,
            List<Student> students,
            List<Thesis> theses,
            List<DefenceParticipants> defenceParticipants
            )
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
                Console.WriteLine("10 - Вибір студентів з першого списку, яких немає в другому списку та отримання максимальної оцінки за диплом:");
                Console.WriteLine("11 - Порівнюємо два списка тезисів чи вони мають ідентичні id:");
                Console.WriteLine("12 - Обчислити середній бал студентів які вчаться на ІС(100+ записів):");
                Console.WriteLine("13 - Найбільший час для захисту:");
                Console.WriteLine("14 - Отримати посортований вік:");
                Console.WriteLine("15 - Обєднали participants і defences:");
                Console.WriteLine("16 - Отримати захисти з id учасників захисту:");
                Console.WriteLine("17 - Чи є студент серед учасників:");
                Console.WriteLine("18 - Показати в яких групах навчаються студенти керівника:");
                Console.WriteLine("19 - Отримати групу з середнім балом здачі тезису:");
                Console.WriteLine("20 - Отримати статистику по віку:");
                Console.Write("Select a query (1-20) or enter '0' to exit: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "0":
                        return;
                    case "1":
                        GetAllDefences(defences);
                        break;
                    case "2":
                        GetMultiStudentUnpublishedTheses(theses);
                        break;
                    case "3":
                        GetSuccessfulSupervisors(supervisors);
                        break;
                    case "4":
                        AddStudentAndDisplay(students);
                        break;
                    case "5":
                        GetThesesWithStudentFullNameByCondition(theses);
                        break;
                    case "6":
                        GetGoupsWithCountOfStudents(students);
                        break;
                    case "7":
                        getOrderedStudentsWithAvarageGradeMoreThan70(students);
                        break;
                    case "8":
                        getUnionQueryWithCondition(students, students);
                        break;
                    case "9":
                        PerformThesisAnalysis(theses, theses);
                        break;
                    case "10":
                        GetMaxGrade(students);
                        break;
                    case "11":
                        CompareThesisStudents(theses, theses);
                        break;
                    case "12":
                        CalculateAverageGradeOfThesisWithCondition(students);
                        break;
                    case "13":
                        FindMaxDefenceTimeWithGroupBy(defences);
                        break;
                    case "14":
                        GetSortedAges(students, supervisors);
                        break;
                    case "15":
                        JoinParticipantsAndDefences(defences, defenceParticipants);
                        break;
                    case "16":
                        DisplayDefenceParticipantsInfo(defences, defenceParticipants);
                        break;
                    case "17":
                        IsThereStudentAmongParticipants(defenceParticipants, students);
                        break;
                    case "18":
                        getInWhichGroupsSupervisorsStudentsStudy(students, supervisors);
                        break;
                    case "19":
                        getGroupWithAverageScoreOfpPassingThesis(students);
                        break;
                    case "20":
                        getDataAbout(students);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void GetAllDefences(List<Defence> defences)
        {
            var query = defences.Select(d => d.ToString());

            Console.WriteLine("\nShow all defences:\n");
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }

        private static void GetMultiStudentUnpublishedTheses(List<Thesis> theses)
        {
            var query = (
                            from thesis in theses
                            where thesis.Students.Count > 1 && !thesis.IsPublished
                            select thesis
                        );
            Console.WriteLine("\nTheses that writes more than one student and it's not published:\n");
            foreach (var item in query)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void GetSuccessfulSupervisors(List<Supervisor> supervisors)
        {
            var query = supervisors.Where(supervisor => supervisor.Advisees.Count > 2 &&
                                                                supervisor.Advisees.All(student => student.AverageGrade > 75));
            Console.WriteLine("\nGet supervisors with more than one students and students must be with avarage grade more then 75 points:\n");
            foreach (var item in query)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public static void AddStudentAndDisplay(List<Student> students)
        {
            var newStudent = new Student(100, "IT-101", 85.5, "New", "Student", "Middle", DateTime.Now);

            var updatedStudents = students
                .Append(newStudent)
                .ToList();

            Console.WriteLine("\nThe query adds a new student using Append and displays the updated list:\n");
            foreach (var student in updatedStudents)
            {
                Console.WriteLine(student.ToString());
            }
        }

        private static void GetThesesWithStudentFullNameByCondition(List<Thesis> theses)
        {
            var query = theses
                .SelectMany(
                    thesis => thesis.Students,
                    (thesis, student) => new
                    {
                        ThesisTitle = thesis.Title,
                        StudentFullName = $"{student.LastName} {student.FirstName}",
                        ThesisIsPublished = thesis.IsPublished
                    })
                .Where(result => result.ThesisIsPublished);

            Console.WriteLine("\nThesis with student where thesis is published:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"Thesis: {item.ThesisTitle}, Student: {item.StudentFullName}");
            }

        }

        private static void GetGoupsWithCountOfStudents(List<Student> students)
        {
            var query = (
                        from student in students
                        group student by student.Group into groupedStudents
                        orderby groupedStudents.Count() descending
                        where groupedStudents.Count() > 1
                        select new
                        {
                            Group = groupedStudents.Key,
                            StudentsCount = groupedStudents.Count()
                        }
                     );

            Console.WriteLine("\nGet goups with count of students with descending order:\n");
            Console.WriteLine("Група\tКількість");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.Group}\t{item.StudentsCount}");
            }
        }

        private static void getOrderedStudentsWithAvarageGradeMoreThan70(List<Student> students)
        {
            var query = students
                    .Skip(5) // Пропустити перші 5 елементів
                    .Where(student => student.AverageGrade > 70)
                    .OrderBy(student => student.AverageGrade)
                    .ThenBy(student => student.LastName)
                    .ThenBy(student => student.Group)
                    .Reverse();

            Console.WriteLine("\nGet students ordered by Group and LastName, AverageGrade and Skip first 5 elements also students must have avarage grade more than 70: \n");
            foreach (var item in query)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void getUnionQueryWithCondition(List<Student> students1, List<Student> students2)
        {
            DateTime startDate = new DateTime(2002, 1, 1);
            DateTime endDate = new DateTime(2010, 12, 31);

            var query = (from student in students1.Union(students2)
                         where student.AverageGrade > 75
                               && student.Thesis != null
                               && student.Thesis.IsPublished
                               && student.BirthDate >= startDate
                               && student.BirthDate <= endDate
                         orderby student.AverageGrade descending
                         select student
                        )
                        .Skip(2)
                        .Take(5)
                        .Distinct();

            Console.WriteLine("\nThe query selects students from the union of the two lists, taking into account conditions such as GPA above 75, having and publishing a degree, and filtering by date of birth. The results are sorted by descending average score, limited to a sample of a certain number, and duplicates are removed: \n");

            foreach (var item in query)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void PerformThesisAnalysis(List<Thesis> theses1, List<Thesis> theses2)
        {
            var query = (from thesis in theses1.Intersect(theses2)
                          group thesis by new { thesis.IsPublished } into grouped
                          let totalTheses = grouped.Count()
                          let maxPagesCount = grouped.Max(t => t.PagesCount)
                          let minPagesCount = grouped.Min(t => t.PagesCount)
                          let avaragePagesCount = grouped.Average(t => t.PagesCount)
                         orderby grouped.Key.IsPublished descending
                          select new
                          {
                              IsPublished = grouped.Key.IsPublished,
                              TotalTheses = totalTheses,
                              maxPagesCount = maxPagesCount,
                              minPagesCount = minPagesCount,
                              avaragePagesCount = avaragePagesCount,
                          })
                          /*.FirstOrDefault()*/;
            Console.WriteLine("Опублікованість\t\tКількість дипломів\tМакс\tМін\tСередня ");
            foreach (var item in query)
            {
                string isPublishedString = item.IsPublished ? "Опублі. статті: " : "Не опуб. статті: ";
                Console.WriteLine($"{isPublishedString}\t\t{item.TotalTheses}\t{item.maxPagesCount}\t{item.minPagesCount}\t{item.avaragePagesCount}");
            }

        }

        private static void GetMaxGrade(List<Student> students1)
        {
            var newStudent1 = new Student(100, "IT-101", 85.5, "New1", "Student1", "Middle1", DateTime.Now);
            var newStudent2 = new Student(111, "IT-111", 90, "New2", "Student2", "Middle2", DateTime.Now);
            var newThesis1 = new Thesis(122, "Title 1", 66, 77, true);
            var newThesis2 = new Thesis(123, "Title 2", 77, 88, false);
            newThesis1.Students.Add(newStudent1);
            newThesis2.Students.Add(newStudent2);
            newStudent1.Thesis = newThesis1;
            newStudent2.Thesis = newThesis2;
            List<Student> students2 = new List<Student>();
            students2.Add(newStudent1);
            students2.Add(newStudent2);

            int maxGrade = students1
                .Except(students2)
                .Where(student => student.Thesis != null)
                .Max(student => student.Thesis.Grade);

            if (maxGrade == 0)
            {
                throw new InvalidOperationException("No joint students with valid diplomas were found");
            }
            Console.WriteLine("\nThis method finds common students in the first list students1 that have no equivalents in the second list students2 and returns the maximum grade of their degrees: \n");
            Console.WriteLine($"Max grade is:{maxGrade}");
        }

        public static void CompareThesisStudents(List<Thesis> thesis1, List<Thesis> thesis2)
        {
            var areEqual = thesis1
                .Select(t1 => t1.Id)
                .SequenceEqual(thesis2.Select(t2 => t2.Id));
            Console.WriteLine($"\nThe query compares theses using SequenceEqual: {areEqual}\n");
        }

        private static void CalculateAverageGradeOfThesisWithCondition(List<Student> students)
        {
            var averageGrade = (from student in students
                                where student.Group.StartsWith("IS") && student.AverageGrade > 60
                                let thesis = student.Thesis
                                where thesis != null && thesis.IsPublished
                                select thesis.Grade)
                                .DefaultIfEmpty(0)
                                .Average();

            Console.WriteLine($"\nCalculate avarage grade of theses that student form Is group with avarage grade more than 60 {averageGrade}\n");
        }

        private static void FindMaxDefenceTimeWithGroupBy(List<Defence> defences)
        {
            var maxDefenceTime = defences.GroupBy(d => d.Id)
                                         .Select(group => new
                                         {
                                             DefenceId = group.Key,
                                             MaxScheduledDefenceTime = group.Max(d => d.ScheduledDefenceTime)
                                         })
                                         .Max(result => result.MaxScheduledDefenceTime);

            Console.WriteLine($"\nThe query finds the maximum defence time among all defences with group by: {maxDefenceTime}\n");
        }

        private static void GetSortedAges(List<Student> students, List<Supervisor> supervisors)
        {
            var studentsAndSupervisors = students.Select(s => new
            {
                Name = $"{s.FirstName} {s.LastName}",
                Age = (DateTime.Now - s.BirthDate).Days / 365,
                Role = Role.Student
            })
            .Union(supervisors.Select(s => new
            {
                Name = $"{s.FirstName} {s.LastName}",
                Age = (DateTime.Now - s.BirthDate).Days / 365,
                Role = Role.Supervisor
            }))
            .OrderBy(x => x.Age)
            .ThenBy(x=> x.Role);

            foreach (var person in studentsAndSupervisors)
            {
                Console.WriteLine($"Ім'я: {person.Name}, Вік: {person.Age} років, Роль: {person.Role}");
            }
        }

        public static void JoinParticipantsAndDefences(List<Defence> defences, List<DefenceParticipants> participants)
        {
            var query = from participant in participants
                             join defence in defences on participant.DefenceId equals defence.Id
                             select $"{defence.Name} - {participant}";

            Console.WriteLine("\nJoin of participants and defences:\n");
            foreach (var item in query)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public static void DisplayDefenceParticipantsInfo(List<Defence> defences, List<DefenceParticipants> participants)
        {
            var result = from defence in defences
                         join participant in participants on defence.Id equals participant.DefenceId
                         group participant by defence.Name into groupedParticipants
                         select new
                         {
                             DefenceName = groupedParticipants.Key,
                             ParticipantsCount = groupedParticipants.Count(),
                             ParticipantsList = string.Join(", ", groupedParticipants.Select(p => p.ParticipantId))
                         };

            Console.WriteLine("\n the query groups protection participants by protection name and outputs information about the number of participants and their identifiers in the form of a string: \n");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.DefenceName}: {item.ParticipantsCount} Participants - {item.ParticipantsList}");
            }
        }

        private static void IsThereStudentAmongParticipants(List<DefenceParticipants> participants, List<Student> students)
        {
            var joinedParticipants = from participant in participants
                                     join student in students on participant.ParticipantId equals student.Id
                                     select new { Participant = participant, Student = student };

            bool hasStudentParticipant = joinedParticipants.Any(jp => jp.Participant.Role == Role.Student);

            Console.WriteLine($"Is there a student among the participants?: {hasStudentParticipant}");
        }

        private static void getInWhichGroupsSupervisorsStudentsStudy(List<Student> students, List<Supervisor> supervisors)
        {
            var supervisorStudentsGroups = from student in students
                                           join supervisor in supervisors on student.Supervisor.Id equals supervisor.Id
                                           group student by new { supervisor.FirstName, supervisor.LastName } into g
                                           select new
                                           {
                                               SupervisorName = $"{g.Key.FirstName} {g.Key.LastName}",
                                               Groups = string.Join(", ", g.Select(s => s.Group))
                                           };

            foreach (var group in supervisorStudentsGroups)
            {
                Console.WriteLine($"Супервізор: {group.SupervisorName}, Групи студентів: {group.Groups}");
            }
        }

        private static void getGroupWithAverageScoreOfpPassingThesis(List<Student> students)
        {
            var groupsAverageGrade = from student in students
                                     group student by student.Group into g
                                     let averageGrade = g.Average(s => s.AverageGrade)
                                     orderby averageGrade descending
                                     select new
                                     {
                                         Group = g.Key,
                                         AverageGrade = averageGrade
                                     };

            foreach (var group in groupsAverageGrade)
            {
                Console.WriteLine($"Група: {group.Group}, Середній бал: {group.AverageGrade}");
            }
        }

        private static void getDataAbout(List<Student> students)
        {
            var groupedStudents = from student in students
                                  let age = (DateTime.Now - student.BirthDate).Days / 365
                                  group new { student, age } by age / 10 into ageGroup // Групуємо студентів за віком у десятки років
                                  orderby ageGroup.Key // Сортуємо за групою віку
                                  select new
                                  {
                                      AgeGroup = $"{ageGroup.Key * 10}-{ageGroup.Key * 10 + 9}", // Рядок з віковою групою
                                      AverageAge = ageGroup.Average(x => x.age), // Середній вік у групі
                                      StudentsCount = ageGroup.Count(), // Кількість студентів у групі
                                  };

            foreach (var group in groupedStudents)
            {
                Console.WriteLine($"Вікова група: {group.AgeGroup}, Середній вік: {group.AverageAge}, Кількість студентів: {group.StudentsCount}");
            }
        }
    }
}
