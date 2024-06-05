using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lab2.Models
{
    [XmlRoot("Student")]
    [Serializable]
    public class Student
        : Person
    {
        [XmlElement("Group")]
        public string Group { get; set; } // Група
        [XmlElement("AverageGrade")]
        public double AverageGrade { get; set; } // Середній бал
        [XmlElement("ThesisId")]
        public int ThesisId { get; set; } // ID тезису
        [XmlElement("SupervisorId")]
        public int SupervisorId { get; set; } // ID супервізора

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

                _thesis = value;
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

                _supervisor = value;
            }
        }
        public Student() { }
        public Student(int id, string group, double averageGrade, string lastName, string firstName, string middleName, DateTime birthDate, int? thesisId = null, int? supervisorId = null)
            : base(id, lastName, firstName, middleName, birthDate)
        {
            Group = group;
            AverageGrade = averageGrade;
            ThesisId = thesisId ?? default(int); // Встановлюємо thesisId або 0, якщо воно null
            SupervisorId = supervisorId ?? default(int); // Встановлюємо supervisorId або 0, якщо воно null
        }

        public override string ToString()
        {
            return $"ID: {Id}, Прізвище: {LastName}, Ім'я: {FirstName}, Дата народження: {BirthDate.ToShortDateString()}, Група: {Group}, Середній бал: {AverageGrade}, {(Thesis != null ? $" Диплом: {Thesis.Title}" : "Нема доплому")}, {(Supervisor != null ? $"Керівник: {Supervisor.LastName}" : "Нема керівника")}";
        }
    }
}
