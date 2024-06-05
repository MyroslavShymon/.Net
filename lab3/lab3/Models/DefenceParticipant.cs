using lab3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace lab3.Models
{
    public class DefenceParticipant
    {
        private static HashSet<(int DefenceId, int ParticipantId, Role Role)> usedCombinations = new HashSet<(int, int, Role)>();
        public int Id { get; set; }
        public int DefenceId { get; set; }
        public int ParticipantId { get; set; }
        public Role Role { get; set; }
        public Defence Defence { get; set; }
        public Person Participant { get; set; }
        public DefenceParticipant() { }

        public DefenceParticipant(int id, int defenceId, int participantId, List<Defence> defences, List<Student> students, List<Supervisor> supervisors, Role role)
        {
            if (!Enum.IsDefined(typeof(Role), role))
            {
                throw new ArgumentException("Не правильне значення ролі.", nameof(role));
            }

            if (!DefenceExists(defenceId, defences))
            {
                throw new ArgumentException($"Помилка: захист із ідентифікатором {defenceId} не знайдено.");
            }

            Defence = defences.Find(d => d.Id == defenceId);

            if (!ParticipantExists(participantId, students, supervisors, role))
            {
                throw new ArgumentException($"Помилка: учасник із ідентифікатором {participantId} не знайдено.");
            }

            Participant = getParticipant(participantId, students, supervisors, role);


            var combination = (DefenceId: defenceId, ParticipantId: participantId, Role: role);

            if (usedCombinations.Contains(combination))
            {
                throw new ArgumentException($"Помилка: комбінація {combination} вже використовується.");
            }

            Id = id;
            DefenceId = defenceId;
            ParticipantId = participantId;
            Role = role;

            usedCombinations.Add(combination);
        }

        private bool DefenceExists(int defenceId, List<Defence> defences)
        {
            return defences.Any(d => d.Id == defenceId);
        }

        public Person getParticipant(int participantId, List<Student> students, List<Supervisor> supervisors, Role role)
        {
            Person participant = null;
            if (role == Role.Student)
            {
                participant = students.Find(s => s.Id == participantId);
            }

            if (role == Role.Supervisor)
            {
                participant = supervisors.Find(s => s.Id == participantId);
            }

            if (role == Role.Guest)
            {
                Person foundSupervisor = supervisors.FirstOrDefault(s => s.Id == participantId);
                Person foundStudent = students.FirstOrDefault(s => s.Id == participantId);

                participant = foundSupervisor == null ? foundStudent : foundSupervisor;
            }

            if (participant == null)
            {
                throw new ArgumentException($"Нема такого учасника!");
            }

            return participant;
        }

        private bool ParticipantExists(int participantId, List<Student> students, List<Supervisor> supervisors, Role role)
        {
            if (role == Role.Student)
            {
                return students.Any(s => s.Id == participantId);
            }

            if (role == Role.Supervisor)
            {
                return supervisors.Any(s => s.Id == participantId);
            }

            if (role == Role.Guest)
            {
                Supervisor foundSupervisor = supervisors.FirstOrDefault(s => s.Id == participantId);
                Student foundStudent = students.FirstOrDefault(s => s.Id == participantId);

                return foundSupervisor == null && foundStudent == null ? false : true;
            }

            return false;
        }

        public override string ToString()
        {
            string roleString = Role == Role.Student ? "Студент" :
                                Role == Role.Supervisor ? "Супервізор" :
                                Role == Role.Guest ? "Гість" : "Невідомо";
            return $"ID: {Id}, DefenceID: {DefenceId}, ParticipantID: {ParticipantId}, Роль: {roleString}";
        }
    }

}
