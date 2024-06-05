using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lab3.Models
{
    public class University
    {
        public List<Student> Students { get; set; }

        public List<Supervisor> Supervisors { get; set; }

        public List<Thesis> Theses { get; set; }

        public List<Defence> Defences { get; set; }

        public List<DefenceParticipant> DefenceParticipants { get; set; }
    }
}
