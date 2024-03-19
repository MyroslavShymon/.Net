using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    internal class Student
        : Person
    {
        public string Group { get; set; } // Група
        public double AverageGrade { get; set; } // Середній бал

        private Thesis _thesis;
        public Thesis Thesis
        {
            get => _thesis;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Thesis), "Теза не може бути нульовою.");
                }

                if (value.Students.Contains(this))
                {
                    _thesis = value;
                }
                else
                {
                    throw new InvalidOperationException("Студент повинен бути пов’язаний із наданою тезою.");
                }
            }
        }

        private Supervisor _supervisor;
        public Supervisor Supervisor
        {
            get => _supervisor;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Supervisor), "Супервізор не може бути нульовим.");
                }

                if (value.Advisees.Contains(this))
                {
                    _supervisor = value;
                }
                else
                {
                    throw new InvalidOperationException("Студент повинен бути пов’язаний із наданим керівником.");
                }
            }
        }

        public Student(int id, string group, double averageGrade, string lastName, string firstName, string middleName, DateTime birthDate)
            : base(id, lastName, firstName, middleName, birthDate)
        {
            Group = group;
            AverageGrade = averageGrade;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Прізвище: {LastName}, Ім'я: {FirstName}, Дата народження: {BirthDate.ToShortDateString()}, Група: {Group}, Середній бал: {AverageGrade}, {(Thesis != null ? $" Диплом: {Supervisor.LastName}" : "Нема доплому")}, {(Supervisor != null ? $"Керівник: {Supervisor.LastName}" : "Нема керівника")}";
        }
    }
}
