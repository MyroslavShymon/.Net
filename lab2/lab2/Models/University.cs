using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lab2.Models
{
    [Serializable]
    [XmlRoot("university")]
    public class University
    {
        [XmlArray("Students")]
        [XmlArrayItem("Student")]
        public List<Student> Students { get; set; }

        [XmlArray("Supervisors")]
        [XmlArrayItem("Supervisor")]
        public List<Supervisor> Supervisors { get; set; }

        [XmlArray("Theses")]
        [XmlArrayItem("Thesis")]
        public List<Thesis> Theses { get; set; }

        [XmlArray("Defences")]
        [XmlArrayItem("Defence")]
        public List<Defence> Defences { get; set; }

        [XmlArray("DefenceParticipants")]
        [XmlArrayItem("DefenceParticipant")]
        public List<DefenceParticipant> DefenceParticipants { get; set; }
    }
}
