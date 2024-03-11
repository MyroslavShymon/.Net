using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    internal class Thesis
    {
        public int Id { get; set; }
        public string Title { get; set; } // Назва диплому
        public int PagesCount { get; set; } // Кількість сторінок у дипломній роботі
        public double Grade { get; set; } // Оцінка за дипломну роботу
        public bool IsPublished { get; set; } // Опублікована чи ні
        public List<Student> _students { get; set; } = new List<Student>(); // Багато студентів можуть писати один диплом

        public List<Student> Students
        {
            get => _students;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Students), "Список студентів не може бути нульовим.");
                }

                foreach (var student in value)
                {
                    if (student == null)
                    {
                        throw new ArgumentNullException(nameof(Students), "Студент у списку не може бути нульовим.");
                    }

                    if (student.Thesis != null && student.Thesis != this)
                    {
                        throw new InvalidOperationException("Кожен студент повинен бути пов'язаний з цією дипломною роботою.");
                    }
                }

                _students = value;
            }
        }

        public Thesis(int id, string title, int pagesCount, double grade, bool isPublished)
        {
            Id = id;
            Title = title;
            PagesCount = pagesCount;
            Grade = grade;
            IsPublished = isPublished;
        }
    }
}
