using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

// EventLogEntry - клас, що представляє запис в журналі подій
public class EventLogEntry
{
    public string Source { get; set; }
    public DateTime Time { get; set; }
    public string Message { get; set; }
    public string Level { get; set; }

    public override string ToString()
    {
        return $"[{Time}] [{Level}] {Source}: {Message}";
    }
}

// EventLogger - абстрактний клас, що визначає спосіб логування подій
public abstract class EventLogger
{
    public abstract void Log(EventLogEntry entry);
}

// TextEventLogger - конкретна реалізація EventLogger для логування в текстовий файл
public class TextEventLogger : EventLogger
{
    private readonly string _filePath;

    public TextEventLogger(string filePath)
    {
        _filePath = filePath;
    }

    public override void Log(EventLogEntry entry)
    {
        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
            writer.WriteLine(entry.ToString());
        }
    }
}

// XMLEventLogger - конкретна реалізація EventLogger для логування в XML файл
public class XMLEventLogger : EventLogger
{
    private readonly string _filePath;

    public XMLEventLogger(string filePath)
    {
        _filePath = filePath;
    }

    public override void Log(EventLogEntry entry)
    {
        XmlDocument doc = new XmlDocument();
        if (File.Exists(_filePath))
        {
            doc.Load(_filePath);
        }
        else
        {
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);
            XmlElement root = doc.CreateElement("Events");
            doc.AppendChild(root);
        }

        XmlElement eventElement = doc.CreateElement("Event");
        eventElement.SetAttribute("Time", entry.Time.ToString());
        eventElement.SetAttribute("Level", entry.Level);
        eventElement.SetAttribute("Source", entry.Source);
        eventElement.InnerText = entry.Message;

        doc.DocumentElement?.AppendChild(eventElement);
        doc.Save(_filePath);
    }
}

// EventLoggerFactory - фабрика для створення відповідних реалізацій EventLogger
public class EventLoggerFactory
{
    public enum LogType { Text, XML }

    public static EventLogger CreateLogger(LogType type, string filePath)
    {
        switch (type)
        {
            case LogType.Text:
                return new TextEventLogger(filePath);
            case LogType.XML:
                return new XMLEventLogger(filePath);
            default:
                throw new ArgumentException("Invalid log type");
        }
    }
}

// EventLogManager - клас-одинак, що відповідає за доступ до журналу подій
public sealed class EventLogManager
{
    private static EventLogManager _instance;
    private List<EventLogEntry> _eventLog;

    private EventLogManager()
    {
        _eventLog = new List<EventLogEntry>();
    }

    public static EventLogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventLogManager();
            }
            return _instance;
        }
    }

    public void AddEvent(EventLogEntry entry)
    {
        _eventLog.Add(entry);
    }

    public List<EventLogEntry> GetLastEvents(int count)
    {
        return _eventLog.Skip(Math.Max(0, _eventLog.Count - count)).ToList();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Приклад використання
        EventLogger textLogger = EventLoggerFactory.CreateLogger(EventLoggerFactory.LogType.Text, "text_log.txt");
        EventLogger xmlLogger = EventLoggerFactory.CreateLogger(EventLoggerFactory.LogType.XML, "xml_log.xml");

        EventLogManager.Instance.AddEvent(new EventLogEntry
        {
            Source = "Main",
            Time = DateTime.Now,
            Level = "Normal",
            Message = "Application started"
        });

        EventLogManager.Instance.AddEvent(new EventLogEntry
        {
            Source = "Database",
            Time = DateTime.Now,
            Level = "Error",
            Message = "Connection failed"
        });

        EventLogManager.Instance.AddEvent(new EventLogEntry
        {
            Source = "Main",
            Time = DateTime.Now,
            Level = "Warning",
            Message = "Memory usage high"
        });

        // Отримуємо останні 10 подій та логуємо їх
        var lastEvents = EventLogManager.Instance.GetLastEvents(10);
        foreach (var log in lastEvents)
        {
            // Логуємо в обидва формати
            textLogger.Log(log);
            xmlLogger.Log(log);
        }

        // Отримуємо останні 10 подій з текстового логу та виводимо їх
        var lastTextEvents = ReadLastEventsFromTextFile("text_log.txt", 10);
        Console.WriteLine("Last 10 events from text log:");
        foreach (var log in lastTextEvents)
        {
            Console.WriteLine(log);
        }

        // Отримуємо останні 10 подій з XML логу та виводимо їх
        var lastXMLEvents = ReadLastEventsFromXMLFile("xml_log.xml", 10);
        Console.WriteLine("\nLast 10 events from XML log:");
        foreach (var log in lastXMLEvents)
        {
            Console.WriteLine(log);
        }

        Console.WriteLine("Events read successfully.");
    }

    // Метод для читання останніх n подій з XML файла
    static List<EventLogEntry> ReadLastEventsFromXMLFile(string filePath, int count)
    {
        List<EventLogEntry> events = new List<EventLogEntry>();
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList eventNodes = doc.SelectNodes("//Event");
            int startIndex = Math.Max(0, eventNodes.Count - count);
            for (int i = startIndex; i < eventNodes.Count; i++)
            {
                XmlNode eventNode = eventNodes[i];
                EventLogEntry entry = new EventLogEntry
                {
                    Time = DateTime.Parse(eventNode.Attributes["Time"].Value),
                    Level = eventNode.Attributes["Level"].Value,
                    Source = eventNode.Attributes["Source"].Value,
                    Message = eventNode.InnerText
                };
                events.Add(entry);
            }
        }
        return events;
    }

    // Метод для читання останніх n подій з текстового файла
    static List<EventLogEntry> ReadLastEventsFromTextFile(string filePath, int count)
    {
        List<EventLogEntry> events = new List<EventLogEntry>();
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            int startIndex = Math.Max(0, lines.Length - count);
            for (int i = startIndex; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(new char[] { '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    EventLogEntry entry = new EventLogEntry
                    {
                        Time = DateTime.Parse(parts[0] + " " + parts[1]),
                        Level = parts[2],
                        Source = parts[3],
                        Message = string.Join(" ", parts.Skip(4))
                    };
                    events.Add(entry);
                }
            }
        }
        return events;
    }
}