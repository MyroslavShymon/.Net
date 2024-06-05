using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lab3.Models
{
    public class Student
          : Person
    {
        public string Group { get; set; } // Група
        public double AverageGrade { get; set; } // Середній бал
        public int ThesisId { get; set; } // ID тезису
        public int SupervisorId { get; set; } // ID супервізора

        [JsonIgnore]
        private Thesis _thesis;
        [JsonIgnore]
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

        [JsonIgnore]
        private Supervisor _supervisor;
       /* [JsonIgnore]*/
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
