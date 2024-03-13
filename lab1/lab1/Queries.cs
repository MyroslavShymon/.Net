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
                        SupervisorsWithMostAdvisees(theses);
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
                        GetMaxGrade(students, students);
                        break;
                    case "11":
                        JoinStudentNames(students, students);
                        break;
                    case "12":
                        JoinParticipantsAndDefences(defences, defenceParticipants);
                        break;
                    case "13":
                        DisplayDefenceParticipantsInfo(defences, defenceParticipants);
                        break;
                        //
                    case "14":
                        CountUniqueDefencesWithJoin(defenceParticipants);
                        break;
                    case "15":
                        FindTotalStudentsWithThesisWithJoin(students);
                        break;
                    case "16":
                        FindMaxDefenceTimeWithGroupBy(defences);
                        break;
                    case "17":
                        SkipFirstThreeDefencesWithJoin(defences, defenceParticipants);
                        break;
                    case "18":
                        FindParticipantsWithSameSupervisor(defenceParticipants);
                        break;
                    case "19":
                        CompareThesisStudents(theses, theses);
                        break;
                    case "20":
                        CalculateAverageGradeOfThesisWithCondition(students);
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

        private static void GetGoupsWithCountOfStudents(List<Student> students)
        {
            var query = (
                        from student in students
                        group student by student.Group into groupedStudents
                        orderby groupedStudents.Count() descending
                        select new
                        {
                            Group = groupedStudents.Key,
                            StudentsCount = groupedStudents.Count()
                        }
                     );

            Console.WriteLine("\nGet goups with count of students with descending order:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.Group}: {item.StudentsCount}");
            }
        }

        private static void SupervisorsWithMostAdvisees(List<Thesis> theses)
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

        public static void CompareThesisStudents(List<Thesis> thesis1, List<Thesis> thesis2)
        {
            var areEqual = thesis1
                .Select(t1 => t1.Students.Select(s => s.Id).ToList())
                .SequenceEqual(thesis2.Select(t2 => t2.Students.Select(s => s.Id).ToList()));

            Console.WriteLine($"\nThe query compares the students in two different theses using SequenceEqual: {areEqual}\n");
        }


        private static void getOrderedStudentsWithAvarageGradeMoreThan70(List<Student> students)
        {
            var query = students
                    .OrderBy(student => student.Group)
                    .ThenBy(student => student.LastName)
                    .Skip(5) // Пропустити перші 5 елементів
                    .Where(student => student.AverageGrade > 70)
                    .Reverse();

            Console.WriteLine("\nGet students ordered by Group and LastName and Skip first 5 elements also students must have avarage grade more than 70: \n");
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
            var result = (from thesis in theses1.Intersect(theses2)
                          group thesis by new { thesis.Grade, thesis.PagesCount } into grouped
                          let totalTheses = grouped.Count()
                          let totalCommonPages = grouped.Sum(t => t.PagesCount)
                          orderby grouped.Key.Grade descending
                          select new
                          {
                              Grade = grouped.Key.Grade,
                              PagesCount = grouped.Key.PagesCount,
                              TotalTheses = totalTheses,
                              TotalCommonPages = totalCommonPages
                          }).FirstOrDefault();


            Console.WriteLine("\nThe query determines the common characteristics for the theses that are shared between the two lists of theses (theses1 and theses2), including the average grade (Grade), the number of pages (PagesCount), the total number of such theses (TotalTheses), and the total number of pages for these theses (TotalCommonPages ), and returns the first element sorted in descending order of value:\n");
            Console.WriteLine($"Common Theses Count: {result?.TotalTheses}");
            Console.WriteLine($"Highest Rated Thesis - Grade: {result?.Grade}, Pages Count: {result?.PagesCount}, Total Theses: {result?.TotalTheses}");
            Console.WriteLine($"Total Pages for Common Theses: {result?.TotalCommonPages}");
        }

        private static void GetMaxGrade(List<Student> students1, List<Student> students2)
        {
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

        private static void JoinStudentNames(List<Student> students1, List<Student> students2)
        {
            var query = from student1 in students1
                                 join student2 in students2 on student1.FirstName equals student2.FirstName
                                 select student1.FirstName;

            Console.WriteLine("\nassociation of students by name:\n");
            foreach (var item in query)
            {
                Console.WriteLine(item.ToString());
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

        private static void CountUniqueDefencesWithJoin(List<DefenceParticipants> participants)
        {
            var uniqueDefencesCount = (from participant in participants
                                       join defence in participants on participant.DefenceId equals defence.DefenceId
                                       select participant.DefenceId).Distinct().Count();

            Console.WriteLine($"\nThe query counts the number of unique defences with join and group by: {uniqueDefencesCount}\n");
        }


        private static void FindTotalStudentsWithThesisWithJoin(List<Student> students)
        {
            var totalStudentsWithThesis = (from student in students
                                           join thesis in students on student.Id equals thesis.Id
                                           select student).Count();

            Console.WriteLine($"\nThe query finds the total number of students with theses with join and group by: {totalStudentsWithThesis}\n");
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

        private static void FindStudentsWithoutSupervisorWithJoin(List<Student> students)
        {
            var studentsWithoutSupervisor = from student in students
                                            join supervisor in students on student.Supervisor.Id equals supervisor.Id into studentSupervisors
                                            where !studentSupervisors.Any()
                                            select student;

            Console.WriteLine("\nThe query returns a list of students without a supervisor with join and group by:\n");
            foreach (var student in studentsWithoutSupervisor)
            {
                Console.WriteLine($"Student: {student.LastName} {student.FirstName}");
            }
        }

        private static void SkipFirstThreeDefencesWithJoin(List<Defence> defences, List<DefenceParticipants> participants)
        {
            var result = (from defence in defences.Skip(3)
                          join participant in participants on defence.Id equals participant.DefenceId into defenceParticipants
                          select new
                          {
                              DefenceName = defence.Name,
                              ParticipantsCount = defenceParticipants.Count()
                          }).ToList();

            Console.WriteLine("\nThe query skips the first three defences, displays the rest, and shows the number of participants in each defence with join and group by:\n");
            foreach (var item in result)
            {
                Console.WriteLine($"Defence: {item.DefenceName}, Participants Count: {item.ParticipantsCount}");
            }
        }

        private static void FindParticipantsWithSameSupervisor(List<DefenceParticipants> defenceParticipants)
        {
            var result = defenceParticipants
                .Join(defenceParticipants,
                      dp1 => dp1.DefenceId,
                      dp2 => dp2.DefenceId,
                      (dp1, dp2) => new
                      {
                          SupervisorId = dp1.ParticipantId,
                          StudentId = dp2.ParticipantId
                      })
                .Where(pair => pair.SupervisorId != pair.StudentId)
                .GroupBy(pair => new { pair.SupervisorId, pair.StudentId })
                .Where(group => group.Count() > 1);

            Console.WriteLine("\nThe query finds participants with the same supervisor using join, group by, and count:\n");
            foreach (var group in result)
            {
                Console.WriteLine($"Common Supervisor: {group.Key.SupervisorId}, Participants: {string.Join(", ", group.Select(pair => pair.StudentId))}");
            }
        }


    }
}
