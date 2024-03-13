using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    internal class Supervisor : Person
    {
        public string Position { get; set; } // Посада
        public List<Student> Advisees { get; } = new List<Student>(); // Список студентів в яких курує диплом

        public Supervisor(int id, string position, string lastName, string firstName, string middleName, DateTime birthDate)
            : base(id, lastName, firstName, middleName, birthDate)
        {
            Id = id;
            Position = position;
        }

        public void AddAdvisee(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Студент не може бути null.");
            }

            if (student.Supervisor != null && student.Supervisor != this)
            {
                throw new InvalidOperationException($"Студент {student.LastName} вже курується з іншим керівником.");
            }

            Advisees.Add(student);
            student.Supervisor = this;
        }

        public override string ToString()
        {
            string fullName = $"{LastName} {FirstName} {MiddleName}";
            string adviseesList = Advisees.Any() ? string.Join(", ", Advisees.Select(s => $"{s.LastName} {s.FirstName}")) : "Немає студентів";

            return $"ID: {Id}, ПІБ: {fullName}, Посада: {Position}, Кількість студентів: {Advisees.Count}, Студенти: {adviseesList}";
        }
    }
}
