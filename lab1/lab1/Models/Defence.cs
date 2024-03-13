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
        public string Name { get; set; } //  Назва
        public DateTime DefenseDate { get; set; } // Дата захисту
        public string Link { get; set; } // Посилання по якому буде проходити захист
        public int ScheduledDefenceTime { get; set; } // Час запланований для захисту одного дипломного проєкту
        public string Location { get; set; } // Місце проведення захисту
        public bool IsRemote { get; set; } // Прапорець, що вказує, чи є захист віддаленим

        public Defence(int id, string name,  DateTime defenseDate, string link, int scheduledDefenceTime, string location, bool isRemote)
        {
            Id = id;
            Name = name;
            DefenseDate = defenseDate;
            Link = link;
            ScheduledDefenceTime = scheduledDefenceTime;
            Location = location;
            IsRemote = isRemote;
        }

        public override string ToString()
        {
            string remoteStatus = IsRemote ? "Віддалений" : "Не віддалений";

            return $"ID: {Id}, Назва: {Name}, Дата захисту: {DefenseDate.ToString("yyyy-MM-dd")}, Посилання: {Link}, " +
                   $"Час захисту: {DefenseDate.ToString("hh:mm")}, Місце проведення: {Location}, Статус: {remoteStatus}";
        }
    }
}
