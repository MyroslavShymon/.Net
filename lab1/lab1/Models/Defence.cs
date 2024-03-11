using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    internal class Defence
    {
        public int Id { get; set; }
        public DateTime DefenseDate { get; set; } // Дата захисту
        public string Link { get; set; } // Посилання по якому буде проходити захист
        public DateTime ScheduledDefenceTime { get; set; } // Час запланований для захисту одного дипломного проєкту
        public string Location { get; set; } // Місце проведення захисту
        public bool IsRemote { get; set; } // Прапорець, що вказує, чи є захист віддаленим
    }
}
