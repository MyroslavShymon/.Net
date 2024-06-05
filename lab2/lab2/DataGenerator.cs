using lab2.Enums;
using lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal static class DataGenerator
    {
        private static int currentId = 1;
        private static Random random = new Random();

        public static List<Student> GenerateStudents(int count, List<Supervisor> supervisors, List<Thesis> theses)
        {
            if (count <= 0 || supervisors == null || supervisors.Count == 0 || theses == null || theses.Count == 0)
            {
                throw new ArgumentException("Недійсні аргументи для генерації студентів.");
            }

            List<Student> students = new List<Student>();
            List<int> supervisorIds = supervisors.Select(s => s.Id).ToList();
            List<int> thesesIds = theses.Select(t => t.Id).ToList();

            for (int i = 0; i < count; i++)
            {
                Group randomGroup = (Group)random.Next(Enum.GetValues(typeof(Group)).Length);
                int randomSupervisorId = supervisorIds[random.Next(supervisorIds.Count)];
                int randomThesisId = thesesIds[random.Next(thesesIds.Count)];

                Student student = new Student(
                    GetNextId(),
                    $"{randomGroup}-{random.Next(10)}{random.Next(1, 5)}",
                    random.Next(6000, 10001) / 100.0,
                    $"Last{i + 1}",
                    $"First{i + 1}",
                    $"Middle{i + 1}",
                    generateRandomDate(1998, 2009)
                );

                student.ThesisId = randomThesisId;
                student.SupervisorId = randomSupervisorId;
                student.Thesis = theses.Find(t => t.Id == randomThesisId); 
                student.Supervisor = supervisors.Find(s => s.Id == randomSupervisorId); 

                students.Add(student);
            }
            return students;
        }

        public static List<DefenceParticipant> GenerateDefenceParticipants(int count, List<Defence> defences, List<Student> students, List<Supervisor> supervisors)
        {
            if (count <= 0 || defences == null || defences.Count == 0 || students == null || students.Count == 0 || supervisors == null || supervisors.Count == 0)
            {
                throw new ArgumentException("Недійсні аргументи для генерації учасників захисту.");
            }

            List<DefenceParticipant> defenceParticipantsList = new List<DefenceParticipant>();

            for (int i = 0; i < count; i++)
            {
                int defenceId = defences[random.Next(defences.Count)].Id;
                Role role = random.Next(2) == 0 ? Role.Student : Role.Supervisor;

                int participantId = role == Role.Student
                                    ? students[random.Next(students.Count)].Id
                                    : supervisors[random.Next(supervisors.Count)].Id;

                var combination = (DefenceId: defenceId, ParticipantId: participantId, Role: role);
                while (defenceParticipantsList.Any(dp => dp.DefenceId == combination.DefenceId && dp.ParticipantId == combination.ParticipantId && dp.Role == combination.Role))
                {
                    defenceId = defences[random.Next(defences.Count)].Id;
                    role = random.Next(2) == 0 ? Role.Student : Role.Supervisor;

                    participantId = role == Role.Student
                                    ? students[random.Next(students.Count)].Id
                                    : supervisors[random.Next(supervisors.Count)].Id;

                    combination = (DefenceId: defenceId, ParticipantId: participantId, Role: role);
                }

                DefenceParticipant defenceParticipants = new DefenceParticipant(GetNextId(), defenceId, participantId, defences, students, supervisors, role);
                defenceParticipantsList.Add(defenceParticipants);
            }

            return defenceParticipantsList;
        }

        public static List<Thesis> GenerateTheses(int count)
        {
            List<Thesis> theses = new List<Thesis>();
            HashSet<Student> studentsWithThesis = new HashSet<Student>();

            for (int i = 0; i < count; i++)
            {
                Thesis thesis = new Thesis(
                    GetNextId(),
                    $"Thesis {i + 1}",
                    random.Next(80, 240),
                    random.Next(50, 101),
                    random.Next(2) == 0
                );

                theses.Add(thesis);
            }

            return theses;
        }

        public static List<Defence> GenerateDefences(int count)
        {
            List<Defence> defences = new List<Defence>();
            for (int i = 0; i < count; i++)
            {
                Defence defence = new Defence
                (
                    id: GetNextId(),
                    name: $"Defence {i + 1}",
                    defenseDate: generateRandomDate(2010, 2030, true),
                    link: $"Link{i + 1}",
                    location: $"Location{i + 1}",
                    isRemote: random.Next(2) == 0
                );
                defences.Add(defence);
            }
            return defences;
        }
        public static List<Supervisor> GenerateSupervisors(int count)
        {
            List<Supervisor> supervisors = new List<Supervisor>();
            for (int i = 0; i < count; i++)
            {
                Supervisor supervisor = new Supervisor(
                    GetNextId(),
                    $"{(Position)random.Next(Enum.GetValues(typeof(Position)).Length)}", 
                    $"Last{i + 1}", 
                    $"First{i + 1}",
                    $"Middle{i + 1}",
                    generateRandomDate(1950, 1998)
                );
                supervisors.Add(supervisor);
            }
            return supervisors;
        }

        private static DateTime generateRandomDate(int startYear, int endYear, bool time = false)
        {
            int year = random.Next(startYear, endYear + 1);
            int month = random.Next(1, 13);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int day = random.Next(1, daysInMonth + 1);
            if (time) 
            {
                int hour = random.Next(20, 24);
                int minute = random.Next(0, 60);
                return new DateTime(year, month, day, hour, minute, 0);
            }
            DateTime randomDate = new DateTime(year, month, day);
            return randomDate;
        }

        private static int GetNextId()
        {
            return currentId++;
        }
    }
}
