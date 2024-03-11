using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    internal class Supervisor
    {
        public int Id { get; set; }
        public string FullName { get; set; } // ПІБ
        public string Position { get; set; } // Посада
        public List<Student> Advisees { get; set; } = new List<Student>(); // Список студентів в яких курує диплом
    }
}
