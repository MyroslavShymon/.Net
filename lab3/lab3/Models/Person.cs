using lab3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.Models
{
    public class Person : IHasId
    {
        public int Id { get; set; }
        public string LastName { get; set; } // Прізвище
        public string FirstName { get; set; } // Ім'я
        public string MiddleName { get; set; } // По батькові
        public DateTime BirthDate { get; set; } // Дата народження

        public Person()
        {

        }

        public Person(int id, string lastName, string firstName, string middleName, DateTime birthDate)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            BirthDate = birthDate;
        }
    }

}
