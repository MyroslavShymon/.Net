using lab3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace lab3.Enums
{
    internal class JsonHandler
    {
        public static void WriteJson(string filePath, University university)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;

            string jsonString = JsonSerializer.Serialize<University>(university, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static void DeserealizeJson(string filePath)
        {
            var universityJson = File.ReadAllText(filePath);

            University university = JsonSerializer.Deserialize<University>(universityJson);

            // Виведення всіх даних
            Console.WriteLine("Students:");
            foreach (var student in university.Students)
            {
                Console.WriteLine($"Id: {student.Id}, LastName: {student.LastName}, FirstName: {student.FirstName}, MiddleName: {student.MiddleName}, BirthDate: {student.BirthDate}");
                Console.WriteLine($"Group: {student.Group}, AverageGrade: {student.AverageGrade}, ThesisId: {student.ThesisId}, SupervisorId: {student.SupervisorId}");
                Console.WriteLine($"Supervisor: {student.Supervisor.LastName} {student.Supervisor.FirstName}, Position: {student.Supervisor.Position}");
                Console.WriteLine();
            }

            Console.WriteLine("Supervisors:");
            foreach (var supervisor in university.Supervisors)
            {
                Console.WriteLine($"Id: {supervisor.Id}, LastName: {supervisor.LastName}, FirstName: {supervisor.FirstName}, MiddleName: {supervisor.MiddleName}, BirthDate: {supervisor.BirthDate}");
                Console.WriteLine($"Position: {supervisor.Position}");
                Console.WriteLine();
            }

            Console.WriteLine("Theses:");
            foreach (var thesis in university.Theses)
            {
                Console.WriteLine($"Id: {thesis.Id}, Title: {thesis.Title}, PagesCount: {thesis.PagesCount}, Grade: {thesis.Grade}, IsPublished: {thesis.IsPublished}");
                Console.WriteLine("Students:");
                foreach (var student in thesis.Students)
                {
                    Console.WriteLine($"Id: {student.Id}, LastName: {student.LastName}, FirstName: {student.FirstName}, MiddleName: {student.MiddleName}, BirthDate: {student.BirthDate}");
                    Console.WriteLine($"Group: {student.Group}, AverageGrade: {student.AverageGrade}, ThesisId: {student.ThesisId}, SupervisorId: {student.SupervisorId}");
                    Console.WriteLine($"Supervisor: {student.Supervisor.LastName} {student.Supervisor.FirstName}, Position: {student.Supervisor.Position}");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            Console.WriteLine("Defences:");
            foreach (var defence in university.Defences)
            {
                Console.WriteLine($"Id: {defence.Id}, Name: {defence.Name}, DefenseDate: {defence.DefenseDate}, Link: {defence.Link}, Location: {defence.Location}, IsRemote: {defence.IsRemote}");
                Console.WriteLine();
            }

            Console.WriteLine("DefenceParticipants:");
            foreach (var participant in university.DefenceParticipants)
            {
                Console.WriteLine($"Id: {participant.Id}, DefenceId: {participant.DefenceId}, ParticipantId: {participant.ParticipantId}, Role: {participant.Role}");
                Console.WriteLine($"Defence: {participant.Defence.Name}, DefenseDate: {participant.Defence.DefenseDate}, Link: {participant.Defence.Link}, Location: {participant.Defence.Location}, IsRemote: {participant.Defence.IsRemote}");
                Console.WriteLine($"Participant: {participant.Participant.LastName} {participant.Participant.FirstName}, BirthDate: {participant.Participant.BirthDate}");
                Console.WriteLine();
            }
        }

        public static void ShowDataByUsingJsonDocument(string filePath)
        {
            string jsonDocumentReader = File.ReadAllText(filePath);

            // Створюємо JsonDocument для аналізу JSON
            using (JsonDocument document = JsonDocument.Parse(jsonDocumentReader))
            {
                // Отримуємо кореневий елемент
                JsonElement root = document.RootElement;

                // Виводимо дані про студентів
                Console.WriteLine("Students:");
                JsonElement studentsElement = root.GetProperty("Students");
                foreach (JsonElement studentElement in studentsElement.EnumerateArray())
                {
                    Console.WriteLine($"Id: {studentElement.GetProperty("Id").GetInt32()}, LastName: {studentElement.GetProperty("LastName").GetString()}, FirstName: {studentElement.GetProperty("FirstName").GetString()}, MiddleName: {studentElement.GetProperty("MiddleName").GetString()}, BirthDate: {studentElement.GetProperty("BirthDate").GetDateTime()}");
                    Console.WriteLine($"Group: {studentElement.GetProperty("Group").GetString()}, AverageGrade: {studentElement.GetProperty("AverageGrade").GetDouble()}, ThesisId: {studentElement.GetProperty("ThesisId").GetInt32()}, SupervisorId: {studentElement.GetProperty("SupervisorId").GetInt32()}");
                    JsonElement supervisorElement = studentElement.GetProperty("Supervisor");
                    Console.WriteLine($"Supervisor: {supervisorElement.GetProperty("LastName").GetString()} {supervisorElement.GetProperty("FirstName").GetString()}, Position: {supervisorElement.GetProperty("Position").GetString()}");
                    Console.WriteLine();
                }

                // Виводимо дані про керівників
                Console.WriteLine("Supervisors:");
                JsonElement supervisorsElement = root.GetProperty("Supervisors");
                foreach (JsonElement supervisorElement in supervisorsElement.EnumerateArray())
                {
                    Console.WriteLine($"Id: {supervisorElement.GetProperty("Id").GetInt32()}, LastName: {supervisorElement.GetProperty("LastName").GetString()}, FirstName: {supervisorElement.GetProperty("FirstName").GetString()}, MiddleName: {supervisorElement.GetProperty("MiddleName").GetString()}, BirthDate: {supervisorElement.GetProperty("BirthDate").GetDateTime()}");
                    Console.WriteLine($"Position: {supervisorElement.GetProperty("Position").GetString()}");
                    Console.WriteLine();
                }

                // Виводимо дані про тези
                Console.WriteLine("Theses:");
                JsonElement thesesElement = root.GetProperty("Theses");
                foreach (JsonElement thesisElement in thesesElement.EnumerateArray())
                {
                    Console.WriteLine($"Id: {thesisElement.GetProperty("Id").GetInt32()}, Title: {thesisElement.GetProperty("Title").GetString()}, PagesCount: {thesisElement.GetProperty("PagesCount").GetInt32()}, Grade: {thesisElement.GetProperty("Grade").GetInt32()}, IsPublished: {thesisElement.GetProperty("IsPublished").GetBoolean()}");
                    Console.WriteLine("Students:");
                    JsonElement studentsInThesisElement = thesisElement.GetProperty("Students");
                    foreach (JsonElement studentInThesisElement in studentsInThesisElement.EnumerateArray())
                    {
                        Console.WriteLine($"Id: {studentInThesisElement.GetProperty("Id").GetInt32()}, LastName: {studentInThesisElement.GetProperty("LastName").GetString()}, FirstName: {studentInThesisElement.GetProperty("FirstName").GetString()}, MiddleName: {studentInThesisElement.GetProperty("MiddleName").GetString()}, BirthDate: {studentInThesisElement.GetProperty("BirthDate").GetDateTime()}");
                        Console.WriteLine($"Group: {studentInThesisElement.GetProperty("Group").GetString()}, AverageGrade: {studentInThesisElement.GetProperty("AverageGrade").GetDouble()}, ThesisId: {studentInThesisElement.GetProperty("ThesisId").GetInt32()}, SupervisorId: {studentInThesisElement.GetProperty("SupervisorId").GetInt32()}");
                        JsonElement supervisorInThesisElement = studentInThesisElement.GetProperty("Supervisor");
                        Console.WriteLine($"Supervisor: {supervisorInThesisElement.GetProperty("LastName").GetString()} {supervisorInThesisElement.GetProperty("FirstName").GetString()}, Position: {supervisorInThesisElement.GetProperty("Position").GetString()}");
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                // Виводимо дані про захисти
                Console.WriteLine("Defences:");
                JsonElement defencesElement = root.GetProperty("Defences");
                foreach (JsonElement defenceElement in defencesElement.EnumerateArray())
                {
                    Console.WriteLine($"Id: {defenceElement.GetProperty("Id").GetInt32()}, Name: {defenceElement.GetProperty("Name").GetString()}, DefenseDate: {defenceElement.GetProperty("DefenseDate").GetDateTime()}, Link: {defenceElement.GetProperty("Link").GetString()}, Location: {defenceElement.GetProperty("Location").GetString()}, IsRemote: {defenceElement.GetProperty("IsRemote").GetBoolean()}");
                    Console.WriteLine();
                }

                // Виводимо дані про учасників захистів
                Console.WriteLine("DefenceParticipants:");
                JsonElement defenceParticipantsElement = root.GetProperty("DefenceParticipants");
                foreach (JsonElement participantElement in defenceParticipantsElement.EnumerateArray())
                {
                    Console.WriteLine($"Id: {participantElement.GetProperty("Id").GetInt32()}, DefenceId: {participantElement.GetProperty("DefenceId").GetInt32()}, ParticipantId: {participantElement.GetProperty("ParticipantId").GetInt32()}, Role: {participantElement.GetProperty("Role").GetInt32()}");
                    JsonElement defenceElement = participantElement.GetProperty("Defence");
                    Console.WriteLine($"Defence: {defenceElement.GetProperty("Name").GetString()}, DefenseDate: {defenceElement.GetProperty("DefenseDate").GetDateTime()}, Link: {defenceElement.GetProperty("Link").GetString()}, Location: {defenceElement.GetProperty("Location").GetString()}, IsRemote: {defenceElement.GetProperty("IsRemote").GetBoolean()}");
                    JsonElement participant = participantElement.GetProperty("Participant");
                    Console.WriteLine($"Participant: {participant.GetProperty("LastName").GetString()} {participant.GetProperty("FirstName").GetString()}, BirthDate: {participant.GetProperty("BirthDate").GetDateTime()}");
                    Console.WriteLine();
                }
            }
        }

        public static void ShowDataByUsingJsonNode(string filePath)
        {
            string jsonNode = File.ReadAllText(filePath);

            // Створюємо JsonNode для аналізу JSON
            JsonNode rootNode = JsonNode.Parse(jsonNode);

            // Виводимо дані про студентів
            Console.WriteLine("Students:");
            JsonArray studentsArray = rootNode["Students"].AsArray();
            foreach (JsonObject studentObject in studentsArray)
            {
                Console.WriteLine($"Id: {studentObject["Id"]}, LastName: {studentObject["LastName"]}, FirstName: {studentObject["FirstName"]}, MiddleName: {studentObject["MiddleName"]}, BirthDate: {studentObject["BirthDate"]}");
                Console.WriteLine($"Group: {studentObject["Group"]}, AverageGrade: {studentObject["AverageGrade"]}, ThesisId: {studentObject["ThesisId"]}, SupervisorId: {studentObject["SupervisorId"]}");
                JsonObject supervisorObject = studentObject["Supervisor"].AsObject();
                Console.WriteLine($"Supervisor: {supervisorObject["LastName"]} {supervisorObject["FirstName"]}, Position: {supervisorObject["Position"]}");
                Console.WriteLine();
            }

            // Виводимо дані про керівників
            Console.WriteLine("Supervisors:");
            JsonArray supervisorsArray = rootNode["Supervisors"].AsArray();
            foreach (JsonObject supervisorObject in supervisorsArray)
            {
                Console.WriteLine($"Id: {supervisorObject["Id"]}, LastName: {supervisorObject["LastName"]}, FirstName: {supervisorObject["FirstName"]}, MiddleName: {supervisorObject["MiddleName"]}, BirthDate: {supervisorObject["BirthDate"]}");
                Console.WriteLine($"Position: {supervisorObject["Position"]}");
                Console.WriteLine();
            }

            // Виводимо дані про тези
            Console.WriteLine("Theses:");
            JsonArray thesesArray = rootNode["Theses"].AsArray();
            foreach (JsonObject thesisObject in thesesArray)
            {
                Console.WriteLine($"Id: {thesisObject["Id"]}, Title: {thesisObject["Title"]}, PagesCount: {thesisObject["PagesCount"]}, Grade: {thesisObject["Grade"]}, IsPublished: {thesisObject["IsPublished"]}");
                Console.WriteLine("Students:");
                JsonArray studentsInThesisArray = thesisObject["Students"].AsArray();
                foreach (JsonObject studentInThesisObject in studentsInThesisArray)
                {
                    Console.WriteLine($"Id: {studentInThesisObject["Id"]}, LastName: {studentInThesisObject["LastName"]}, FirstName: {studentInThesisObject["FirstName"]}, MiddleName: {studentInThesisObject["MiddleName"]}, BirthDate: {studentInThesisObject["BirthDate"]}");
                    Console.WriteLine($"Group: {studentInThesisObject["Group"]}, AverageGrade: {studentInThesisObject["AverageGrade"]}, ThesisId: {studentInThesisObject["ThesisId"]}, SupervisorId: {studentInThesisObject["SupervisorId"]}");
                    JsonObject supervisorInThesisObject = studentInThesisObject["Supervisor"].AsObject();
                    Console.WriteLine($"Supervisor: {supervisorInThesisObject["LastName"]} {supervisorInThesisObject["FirstName"]}, Position: {supervisorInThesisObject["Position"]}");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            // Виводимо дані про захисти
            Console.WriteLine("Defences:");
            JsonArray defencesArray = rootNode["Defences"].AsArray();
            foreach (JsonObject defenceObject in defencesArray)
            {
                Console.WriteLine($"Id: {defenceObject["Id"]}, Name: {defenceObject["Name"]}, DefenseDate: {defenceObject["DefenseDate"]}, Link: {defenceObject["Link"]}, Location: {defenceObject["Location"]}, IsRemote: {defenceObject["IsRemote"]}");
                Console.WriteLine();
            }

            // Виводимо дані про учасників захистів
            Console.WriteLine("DefenceParticipants:");
            JsonArray defenceParticipantsArray = rootNode["DefenceParticipants"].AsArray();
            foreach (JsonObject participantObject in defenceParticipantsArray)
            {
                Console.WriteLine($"Id: {participantObject["Id"]}, DefenceId: {participantObject["DefenceId"]}, ParticipantId: {participantObject["ParticipantId"]}, Role: {participantObject["Role"]}");
                JsonObject defenceObject = participantObject["Defence"].AsObject();
                Console.WriteLine($"Defence: {defenceObject["Name"]}, DefenseDate: {defenceObject["DefenseDate"]}, Link: {defenceObject["Link"]}, Location: {defenceObject["Location"]}, IsRemote: {defenceObject["IsRemote"]}");
                JsonObject participant = participantObject["Participant"].AsObject();
                Console.WriteLine($"Participant: {participant["LastName"]} {participant["FirstName"]}, BirthDate: {participant["BirthDate"]}");
                Console.WriteLine();
            }
        }

    }
}
