using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.Models
{
    public class Defence
    {
        public int Id { get; set; }
        public string Name { get; set; } //  Назва
        public DateTime DefenseDate { get; set; } // Дата захисту
        public string Link { get; set; } // Посилання по якому буде проходити захист
        public string Location { get; set; } // Місце проведення захисту
        public bool IsRemote { get; set; } // Прапорець, що вказує, чи є захист віддаленим

        public Defence() { }

        public Defence(int id, string name, DateTime defenseDate, string link, string location, bool isRemote)
        {
            Id = id;
            Name = name;
            DefenseDate = defenseDate;
            Link = link;
            Location = location;
            IsRemote = isRemote;
        }

        public override string ToString()
        {
            string remoteStatus = IsRemote ? "Віддалений" : "Не віддалений";

            return $"ID: {Id}, Назва: {Name}, Дата та час захисту: {DefenseDate.ToString("yyyy-MM-dd hh:mm")}, Посилання: {Link}, " +
                   $"Місце проведення: {Location}, Статус: {remoteStatus}";
        }
    }
}
