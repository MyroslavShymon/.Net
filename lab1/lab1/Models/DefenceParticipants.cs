using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    internal class DefenceParticipants
    {
        public int Id { get; set; }
        public int DefenceId { get; set; }
        public int ParticipantId { get; set; }
        public Role Role { get; set; }
        public DefenceParticipants(int id, int defenceId, int participantId, Role role, List<Defence> defences, List<Student> students, List<Supervisor> supervisors)
        {
            if (!Enum.IsDefined(typeof(Role), role))
            {
                throw new ArgumentException("Не правильне значення ролі.", nameof(role));
            }

            if (!DefenceExists(defenceId, defences))
            {
                throw new ArgumentException($"Помилка: захист із ідентифікатором {defenceId} не знайдено.");
            }

            if (!ParticipantExists(participantId, students, supervisors, role))
            {
                throw new ArgumentException($"Помилка: учасник із ідентифікатором {participantId} не знайдено.");
            }

            Id = id;
            DefenceId = defenceId;
            ParticipantId = participantId;
            Role = role;
        }

        private bool DefenceExists(int defenceId, List<Defence> defences)
        {
            Defence foundDefence = defences.FirstOrDefault(d => d.Id == defenceId);

            return foundDefence != null;
        }

        private bool ParticipantExists(int participantId, List<Student> students, List<Supervisor> supervisors, Role role)
        {
            if (role == Role.Student)
            {
                Student foundStudent = students.FirstOrDefault(s => s.Id == participantId);
                return foundStudent != null ? true : false;
            }

            if (role == Role.Supervisor)
            {
                Supervisor foundSupervisor = supervisors.FirstOrDefault(s => s.Id == participantId);
                return foundSupervisor != null ? true : false;
            }

            if (role == Role.Guest)
            {
                Supervisor foundSupervisor = supervisors.FirstOrDefault(s => s.Id == participantId);
                Student foundStudent = students.FirstOrDefault(s => s.Id == participantId);
                return foundSupervisor == null && foundStudent == null ? false : true;
            }

            return false;
        }
    }
}
