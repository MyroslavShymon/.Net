using lab2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace lab2
{
    internal class XmlHandler
    {
        private List<Student> Students { get; set; }
        private List<Supervisor> Supervisors { get; set; }
        private List<Thesis> Theses { get; set; }
        private List<Defence> Defences { get; set; }
        private List<DefenceParticipant> DefenceParticipants { get; set; }

        public XmlHandler(
            List<Student> students, 
            List<Supervisor> supervisors, 
            List<Thesis> theses, 
            List<Defence> defences, 
            List<DefenceParticipant> defenceParticipants
            )
        {
            Students = students;
            Supervisors = supervisors;
            Theses = theses;
            Defences = defences;
            DefenceParticipants = defenceParticipants;
        }

        public XmlDocument LoadXmlUsingXmlDocument(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            return doc;
        }

        public University LoadXml(string filePath)
        {
            University university;
            XmlSerializer serializer = new XmlSerializer(typeof(University));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                university = (University)serializer.Deserialize(fs);
            }
            return university;
        }

        public void WriteXml(string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartElement("university");

                WriteStudents(writer);
                WriteSupervisors(writer);
                WriteTheses(writer);
                WriteDefences(writer);
                WriteDefencePaticipiants(writer);

                writer.WriteEndElement();
            }
        }

        private void WriteStudents(XmlWriter writer)
        {
            writer.WriteStartElement("Students");
            foreach (var student in Students)
            {
                writer.WriteStartElement("Student");
                writer.WriteElementString("Id", student.Id.ToString());
                writer.WriteElementString("Group", student.Group);
                writer.WriteElementString("AverageGrade", student.AverageGrade.ToString().Replace(',', '.'));
                writer.WriteElementString("LastName", student.LastName);
                writer.WriteElementString("FirstName", student.FirstName);
                writer.WriteElementString("MiddleName", student.MiddleName);
                writer.WriteElementString("BirthDate", student.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                writer.WriteElementString("ThesisId", student.ThesisId.ToString());
                writer.WriteElementString("SupervisorId", student.SupervisorId.ToString());
                // Write other student properties here
                    //Thesis
                    writer.WriteStartElement("Thesis");
                        writer.WriteElementString("Id", student.Thesis.Id.ToString());
                        writer.WriteElementString("Title", student.Thesis.Title);
                        writer.WriteElementString("PagesCount", student.Thesis.PagesCount.ToString());
                        writer.WriteElementString("Grade", student.Thesis.Grade.ToString().Replace(',', '.'));
                        writer.WriteElementString("IsPublished", student.Thesis.IsPublished.ToString().ToLower());
                    writer.WriteEndElement(); // Close thesis element
                    //Supervisor
                    writer.WriteStartElement("Supervisor");
                        writer.WriteElementString("Id", student.Supervisor.Id.ToString());
                        writer.WriteElementString("Position", student.Supervisor.Position);
                        writer.WriteElementString("LastName", student.Supervisor.LastName);
                        writer.WriteElementString("FirstName", student.Supervisor.FirstName);
                        writer.WriteElementString("MiddleName", student.Supervisor.MiddleName);
                        writer.WriteElementString("BirthDate", student.Supervisor.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement(); // Close thesis element
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); // Close students element
        }

        private void WriteSupervisors(XmlWriter writer)
        {
            writer.WriteStartElement("Supervisors");
            foreach (var supervisor in Supervisors)
            {
                writer.WriteStartElement("Supervisor");
                writer.WriteElementString("Id", supervisor.Id.ToString());
                writer.WriteElementString("Position", supervisor.Position);
                writer.WriteElementString("LastName", supervisor.LastName);
                writer.WriteElementString("FirstName", supervisor.FirstName);
                writer.WriteElementString("MiddleName", supervisor.MiddleName);
                writer.WriteElementString("BirthDate", supervisor.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                // Write other supervisor properties here
                    writer.WriteStartElement("Advisees");
                    foreach (var advisee in supervisor.Advisees)
                    {
                        writer.WriteStartElement("Advisee");
                            writer.WriteElementString("Id", advisee.Id.ToString());
                            writer.WriteElementString("Group", advisee.Group);
                            writer.WriteElementString("AverageGrade", advisee.AverageGrade.ToString().Replace(',', '.'));
                            writer.WriteElementString("LastName", advisee.LastName);
                            writer.WriteElementString("FirstName", advisee.FirstName);
                            writer.WriteElementString("MiddleName", advisee.MiddleName);
                            writer.WriteElementString("BirthDate", advisee.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            writer.WriteElementString("ThesisId", advisee.ThesisId.ToString().ToLower());
                            writer.WriteElementString("SupervisorId", advisee.SupervisorId.ToString());
                        writer.WriteEndElement(); // Close advisee element
                    }
                    writer.WriteEndElement(); // Close advisees element
                writer.WriteEndElement(); // Close supervisor element
            }
            writer.WriteEndElement(); // Close supervisors element
        }

        private void WriteTheses(XmlWriter writer)
        {
            writer.WriteStartElement("Theses");
            foreach (var thesis in Theses)
            {
                writer.WriteStartElement("Thesis");
                writer.WriteElementString("Id", thesis.Id.ToString());
                writer.WriteElementString("Title", thesis.Title);
                writer.WriteElementString("PagesCount", thesis.PagesCount.ToString());
                writer.WriteElementString("Grade", thesis.Grade.ToString().Replace(',', '.'));
                writer.WriteElementString("IsPublished", thesis.IsPublished.ToString().ToLower());
                // Write other thesis properties here
                writer.WriteStartElement("Students");
                foreach (var student in thesis.Students)
                {
                    writer.WriteStartElement("Student");
                    writer.WriteElementString("Id", student.Id.ToString());
                    writer.WriteElementString("Group", student.Group);
                    writer.WriteElementString("AverageGrade", student.AverageGrade.ToString().Replace(',', '.'));
                    writer.WriteElementString("LastName", student.LastName);
                    writer.WriteElementString("FirstName", student.FirstName);
                    writer.WriteElementString("MiddleName", student.MiddleName);
                    writer.WriteElementString("BirthDate", student.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteElementString("ThesisId", student.ThesisId.ToString());
                    writer.WriteElementString("SupervisorId", student.SupervisorId.ToString());
                    writer.WriteEndElement(); // Close student element
                }
                writer.WriteEndElement(); // Close students element
                writer.WriteEndElement(); // Close thesis element
            }
            writer.WriteEndElement(); // Close theses element
        }

        private void WriteDefences(XmlWriter writer)
        {
            writer.WriteStartElement("Defences");
            foreach (var defence in Defences)
            {
                writer.WriteStartElement("Defence");
                writer.WriteElementString("Id", defence.Id.ToString());
                writer.WriteElementString("Name", defence.Name);
                writer.WriteElementString("DefenseDate", defence.DefenseDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                writer.WriteElementString("Link", defence.Link);
                writer.WriteElementString("Location", defence.Location);
                writer.WriteElementString("IsRemote", defence.IsRemote.ToString().ToLower());
                writer.WriteEndElement(); // Close defence element
            }
            writer.WriteEndElement(); // Close defences element
        }
        private void WriteDefencePaticipiants(XmlWriter writer)
        {
            writer.WriteStartElement("DefenceParticipants");
            foreach (var defenceParticipant in DefenceParticipants)
            {
                writer.WriteStartElement("DefenceParticipant");
                writer.WriteElementString("Id", defenceParticipant.Id.ToString());
                writer.WriteElementString("DefenceId", defenceParticipant.DefenceId.ToString());
                writer.WriteElementString("ParticipantId", defenceParticipant.ParticipantId.ToString());
                writer.WriteElementString("Role", defenceParticipant.Role.ToString());
                    writer.WriteStartElement("Defence");
                        writer.WriteElementString("Id", defenceParticipant.Defence.Id.ToString());
                        writer.WriteElementString("Name", defenceParticipant.Defence.Name);
                        writer.WriteElementString("DefenseDate", defenceParticipant.Defence.DefenseDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        writer.WriteElementString("Link", defenceParticipant.Defence.Link);
                        writer.WriteElementString("Location", defenceParticipant.Defence.Location);
                        writer.WriteElementString("IsRemote", defenceParticipant.Defence.IsRemote.ToString().ToLower());
                    writer.WriteEndElement(); // Close defence element
                    writer.WriteStartElement("Participiant");
                        writer.WriteElementString("Id", defenceParticipant.Participant.Id.ToString());
                        writer.WriteElementString("LastName", defenceParticipant.Participant.LastName);
                        writer.WriteElementString("FirstName", defenceParticipant.Participant.FirstName);
                        writer.WriteElementString("MiddleName", defenceParticipant.Participant.MiddleName);
                        writer.WriteElementString("BirthDate", defenceParticipant.Participant.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement(); // Close participiant element
                writer.WriteEndElement(); // Close defenceParticipiant element
            }
            writer.WriteEndElement(); // Close defenceParticipiants element
        }

        public void DisplayDeserializedData(University university)
        {
            if(university == null)
            {
                Console.WriteLine("Ви не підгрузили дані");
                return;
            }

            // Displaying students
            Console.WriteLine("\n---------Students:----------\n");
            Console.WriteLine("<Students>");
            foreach (var student in university.Students)
            {
                Console.WriteLine($"  <Student>");
                Console.WriteLine($"    <Id>{student.Id}</Id>");
                Console.WriteLine($"    <Group>{student.Group}</Group>");
                Console.WriteLine($"    <AverageGrade>{student.AverageGrade}</AverageGrade>");
                Console.WriteLine($"    <LastName>{student.LastName}</LastName>");
                Console.WriteLine($"    <FirstName>{student.FirstName}</FirstName>");
                Console.WriteLine($"    <MiddleName>{student.MiddleName}</MiddleName>");
                Console.WriteLine($"    <BirthDate>{student.BirthDate}</BirthDate>");
                Console.WriteLine($"    <ThesisId>{student.ThesisId}</ThesisId>");
                Console.WriteLine($"    <SupervisorId>{student.SupervisorId}</SupervisorId>");

                // Displaying thesis of the student
                var studentThesis = university.Theses.FirstOrDefault(t => t.Id == student.ThesisId);
                if (studentThesis != null)
                {
                    Console.WriteLine($"    <Thesis>");
                    Console.WriteLine($"      <Id>{studentThesis.Id}</Id>");
                    Console.WriteLine($"      <Title>{studentThesis.Title}</Title>");
                    Console.WriteLine($"      <PagesCount>{studentThesis.PagesCount}</PagesCount>");
                    Console.WriteLine($"      <Grade>{studentThesis.Grade}</Grade>");
                    Console.WriteLine($"      <IsPublished>{studentThesis.IsPublished}</IsPublished>");
                    Console.WriteLine($"    </Thesis>");
                }

                // Displaying supervisor of the student
                var studentSupervisor = university.Supervisors.FirstOrDefault(s => s.Id == student.SupervisorId);
                if (studentSupervisor != null)
                {
                    Console.WriteLine($"    <Supervisor>");
                    Console.WriteLine($"      <Id>{studentSupervisor.Id}</Id>");
                    Console.WriteLine($"      <Position>{studentSupervisor.Position}</Position>");
                    Console.WriteLine($"      <LastName>{studentSupervisor.LastName}</LastName>");
                    Console.WriteLine($"      <FirstName>{studentSupervisor.FirstName}</FirstName>");
                    Console.WriteLine($"      <MiddleName>{studentSupervisor.MiddleName}</MiddleName>");
                    Console.WriteLine($"      <BirthDate>{studentSupervisor.BirthDate}</BirthDate>");
                    Console.WriteLine($"    </Supervisor>");
                }

                Console.WriteLine($"  </Student>");
            }
            Console.WriteLine("</Students>");

            // Displaying supervisors
            Console.WriteLine("\n---------Supervisors:----------\n");
            Console.WriteLine("<Supervisors>");
            foreach (var supervisor in university.Supervisors)
            {
                Console.WriteLine($"  <Supervisor>");
                Console.WriteLine($"    <Id>{supervisor.Id}</Id>");
                Console.WriteLine($"    <Position>{supervisor.Position}</Position>");
                Console.WriteLine($"    <LastName>{supervisor.LastName}</LastName>");
                Console.WriteLine($"    <FirstName>{supervisor.FirstName}</FirstName>");
                Console.WriteLine($"    <MiddleName>{supervisor.MiddleName}</MiddleName>");
                Console.WriteLine($"    <BirthDate>{supervisor.BirthDate}</BirthDate>");

                // Displaying advisees
                Console.WriteLine($"    <Advisees>");
                foreach (var advisee in supervisor.Advisees)
                {
                    Console.WriteLine($"      <Advisee>");
                    Console.WriteLine($"        <Id>{advisee.Id}</Id>");
                    Console.WriteLine($"        <Group>{advisee.Group}</Group>");
                    Console.WriteLine($"        <AverageGrade>{advisee.AverageGrade}</AverageGrade>");
                    Console.WriteLine($"        <LastName>{advisee.LastName}</LastName>");
                    Console.WriteLine($"        <FirstName>{advisee.FirstName}</FirstName>");
                    Console.WriteLine($"        <MiddleName>{advisee.MiddleName}</MiddleName>");
                    Console.WriteLine($"        <BirthDate>{advisee.BirthDate}</BirthDate>");
                    Console.WriteLine($"        <ThesisId>{advisee.ThesisId}</ThesisId>");
                    Console.WriteLine($"        <SupervisorId>{advisee.SupervisorId}</SupervisorId>");
                    Console.WriteLine($"      </Advisee>");
                }
                Console.WriteLine($"    </Advisees>");

                Console.WriteLine($"  </Supervisor>");
            }
            Console.WriteLine("</Supervisors>");

            // Displaying theses
            Console.WriteLine("\n---------Theses:----------\n");
            Console.WriteLine("<Theses>");
            foreach (var thesis in university.Theses)
            {
                Console.WriteLine($"  <Thesis>");
                Console.WriteLine($"    <Id>{thesis.Id}</Id>");
                Console.WriteLine($"    <Title>{thesis.Title}</Title>");
                Console.WriteLine($"    <PagesCount>{thesis.PagesCount}</PagesCount>");
                Console.WriteLine($"    <Grade>{thesis.Grade}</Grade>");
                Console.WriteLine($"    <IsPublished>{thesis.IsPublished}</IsPublished>");

                // Displaying students
                Console.WriteLine($"    <Students>");
                foreach (var student in thesis.Students)
                {
                    Console.WriteLine($"      <Student>");
                    Console.WriteLine($"        <Id>{student.Id}</Id>");
                    Console.WriteLine($"        <Group>{student.Group}</Group>");
                    Console.WriteLine($"        <AverageGrade>{student.AverageGrade}</AverageGrade>");
                    Console.WriteLine($"        <LastName>{student.LastName}</LastName>");
                    Console.WriteLine($"        <FirstName>{student.FirstName}</FirstName>");
                    Console.WriteLine($"        <MiddleName>{student.MiddleName}</MiddleName>");
                    Console.WriteLine($"        <BirthDate>{student.BirthDate}</BirthDate>");
                    Console.WriteLine($"        <ThesisId>{student.ThesisId}</ThesisId>");
                    Console.WriteLine($"        <SupervisorId>{student.SupervisorId}</SupervisorId>");
                    Console.WriteLine($"      </Student>");
                }
                Console.WriteLine($"    </Students>");

                Console.WriteLine($"  </Thesis>");
            }
            Console.WriteLine("</Theses>");


            // Displaying defences
            Console.WriteLine("\n---------Defences:----------\n");
            Console.WriteLine("<Defences>");
            foreach (var defence in university.Defences)
            {
                Console.WriteLine($"  <Defence>");
                Console.WriteLine($"    <Id>{defence.Id}</Id>");
                Console.WriteLine($"    <Name>{defence.Name}</Name>");
                Console.WriteLine($"    <DefenseDate>{defence.DefenseDate}</DefenseDate>");
                Console.WriteLine($"    <Link>{defence.Link}</Link>");
                Console.WriteLine($"    <Location>{defence.Location}</Location>");
                Console.WriteLine($"    <IsRemote>{defence.IsRemote}</IsRemote>");
                Console.WriteLine($"  </Defence>");
            }
            Console.WriteLine("</Defences>");


            // Displaying defenceParticipiants
            Console.WriteLine("\n---------DefenceParticipiants:----------\n");
            Console.WriteLine("<DefenceParticipiants>");
            foreach (var defenceParticipiant in university.DefenceParticipants)
            {
                // Display defence participant information
                Console.WriteLine($"  <DefenceParticipiant>");
                Console.WriteLine($"    <Id>{defenceParticipiant.Id}</Id>");
                Console.WriteLine($"    <DefenceId>{defenceParticipiant.DefenceId}</DefenceId>");
                Console.WriteLine($"    <ParticipantId>{defenceParticipiant.ParticipantId}</ParticipantId>");
                Console.WriteLine($"    <Role>{defenceParticipiant.Role}</Role>");

                // Display defence information
                var defence = university.Defences.FirstOrDefault(d => d.Id == defenceParticipiant.DefenceId);
                if (defence != null)
                {
                    Console.WriteLine($"    <Defence>");
                    Console.WriteLine($"      <Id>{defence.Id}</Id>");
                    Console.WriteLine($"      <Name>{defence.Name}</Name>");
                    Console.WriteLine($"      <DefenseDate>{defence.DefenseDate}</DefenseDate>");
                    Console.WriteLine($"      <Link>{defence.Link}</Link>");
                    Console.WriteLine($"      <Location>{defence.Location}</Location>");
                    Console.WriteLine($"      <IsRemote>{defence.IsRemote}</IsRemote>");
                    Console.WriteLine($"    </Defence>");
                }

                // Display participant information
                Person participant = defenceParticipiant.getParticipant(defenceParticipiant.ParticipantId, university.Students, university.Supervisors, defenceParticipiant.Role);
                if (participant != null)
                {
                    Console.WriteLine($"    <Participiant>");
                    Console.WriteLine($"      <Id>{participant.Id}</Id>");
                    Console.WriteLine($"      <LastName>{participant.LastName}</LastName>");
                    Console.WriteLine($"      <FirstName>{participant.FirstName}</LirstName>");
                    Console.WriteLine($"      <MiddleName>{participant.MiddleName}</MiddleName>");
                    Console.WriteLine($"      <BirthDate>{participant.BirthDate}</BirthDate>");
                    Console.WriteLine($"    </Participiant>");
                }

                Console.WriteLine($"  </DefenceParticipiant>");
            }
            Console.WriteLine("</DefenceParticipiants>");
        }

        public void showDataLoadedByXmlDocument(XmlDocument doc)
        {
            if (doc == null)
            {
                Console.WriteLine("Ви не підгрузили дані");
                return;
            }

            foreach (XmlNode node in doc.DocumentElement)
            {
                if (node.Name == "Students")
                {
                    Console.WriteLine("Students:");
                    foreach (XmlNode studentNode in node.ChildNodes)
                    {
                        Console.WriteLine($"Student ID: {studentNode.SelectSingleNode("Id").InnerText}");
                        Console.WriteLine($"Group: {studentNode.SelectSingleNode("Group").InnerText}");
                        Console.WriteLine($"Average Grade: {studentNode.SelectSingleNode("AverageGrade").InnerText}");
                        Console.WriteLine($"Last Name: {studentNode.SelectSingleNode("LastName").InnerText}");
                        Console.WriteLine($"First Name: {studentNode.SelectSingleNode("FirstName").InnerText}");
                        Console.WriteLine($"Middle Name: {studentNode.SelectSingleNode("MiddleName").InnerText}");
                        Console.WriteLine($"Birth Date: {studentNode.SelectSingleNode("BirthDate").InnerText}");
                        Console.WriteLine($"Thesis ID: {studentNode.SelectSingleNode("ThesisId").InnerText}");
                        Console.WriteLine($"Supervisor ID: {studentNode.SelectSingleNode("SupervisorId").InnerText}");
                        Console.WriteLine();
                    }
                }
                else if (node.Name == "Supervisors")
                {
                    Console.WriteLine("Supervisors:");
                    foreach (XmlNode supervisorNode in node.ChildNodes)
                    {
                        Console.WriteLine($"Supervisor ID: {supervisorNode.SelectSingleNode("Id").InnerText}");
                        Console.WriteLine($"Position: {supervisorNode.SelectSingleNode("Position").InnerText}");
                        Console.WriteLine($"Last Name: {supervisorNode.SelectSingleNode("LastName").InnerText}");
                        Console.WriteLine($"First Name: {supervisorNode.SelectSingleNode("FirstName").InnerText}");
                        Console.WriteLine($"Middle Name: {supervisorNode.SelectSingleNode("MiddleName").InnerText}");
                        Console.WriteLine($"Birth Date: {supervisorNode.SelectSingleNode("BirthDate").InnerText}");
                        Console.WriteLine("Advisees:");
                        foreach (XmlNode adviseeNode in supervisorNode.SelectSingleNode("Advisees").ChildNodes)
                        {
                            Console.WriteLine($"Advisee ID: {adviseeNode.SelectSingleNode("Id").InnerText}");
                            Console.WriteLine($"Group: {adviseeNode.SelectSingleNode("Group").InnerText}");
                            Console.WriteLine($"Average Grade: {adviseeNode.SelectSingleNode("AverageGrade").InnerText}");
                            Console.WriteLine($"Last Name: {adviseeNode.SelectSingleNode("LastName").InnerText}");
                            Console.WriteLine($"First Name: {adviseeNode.SelectSingleNode("FirstName").InnerText}");
                            Console.WriteLine($"Middle Name: {adviseeNode.SelectSingleNode("MiddleName").InnerText}");
                            Console.WriteLine($"Birth Date: {adviseeNode.SelectSingleNode("BirthDate").InnerText}");
                            Console.WriteLine($"Thesis ID: {adviseeNode.SelectSingleNode("ThesisId").InnerText}");
                            Console.WriteLine($"Supervisor ID: {adviseeNode.SelectSingleNode("SupervisorId").InnerText}");
                            Console.WriteLine();
                        }
                    }
                }
                else if (node.Name == "DefenceParticipants")
                {
                    Console.WriteLine("Defence Participants:");
                    foreach (XmlNode participantNode in node.ChildNodes)
                    {
                        Console.WriteLine($"Participant ID: {participantNode.SelectSingleNode("Id").InnerText}");
                        Console.WriteLine($"Defence ID: {participantNode.SelectSingleNode("DefenceId").InnerText}");
                        Console.WriteLine($"Role: {participantNode.SelectSingleNode("Role").InnerText}");
                        Console.WriteLine();
                    }
                }
                else if (node.Name == "Defences")
                {
                    Console.WriteLine("Defences:");
                    foreach (XmlNode defenceNode in node.ChildNodes)
                    {
                        Console.WriteLine($"Defence ID: {defenceNode.SelectSingleNode("Id").InnerText}");
                        Console.WriteLine($"Name: {defenceNode.SelectSingleNode("Name").InnerText}");
                        Console.WriteLine($"Defense Date: {defenceNode.SelectSingleNode("DefenseDate").InnerText}");
                        Console.WriteLine($"Link: {defenceNode.SelectSingleNode("Link").InnerText}");
                        Console.WriteLine($"Location: {defenceNode.SelectSingleNode("Location").InnerText}");
                        Console.WriteLine($"Is Remote: {defenceNode.SelectSingleNode("IsRemote").InnerText}");
                        Console.WriteLine();
                    }
                }
                else if (node.Name == "Theses")
                {
                    Console.WriteLine("Theses:");
                    foreach (XmlNode thesisNode in node.ChildNodes)
                    {
                        Console.WriteLine($"Thesis ID: {thesisNode.SelectSingleNode("Id").InnerText}");
                        Console.WriteLine($"Title: {thesisNode.SelectSingleNode("Title").InnerText}");
                        Console.WriteLine($"Pages Count: {thesisNode.SelectSingleNode("PagesCount").InnerText}");
                        Console.WriteLine($"Grade: {thesisNode.SelectSingleNode("Grade").InnerText}");
                        Console.WriteLine($"Is Published: {thesisNode.SelectSingleNode("IsPublished").InnerText}");
                        Console.WriteLine();
                    }
                }
            }
        }

    }
}
