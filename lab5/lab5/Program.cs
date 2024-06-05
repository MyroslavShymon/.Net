using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

// Component interface
public interface ITextProcessor
{
    string Process(string text);
    string ProcessReverse(string text);
}

// ConcreteComponent implementation
public class GoogleTranslateProcessor : ITextProcessor
{
    private string fromLanguage;
    private string toLanguage;

    public GoogleTranslateProcessor(string fromLanguage, string toLanguage)
    {
        this.fromLanguage = fromLanguage;
        this.toLanguage = toLanguage;
    }

    public string Process(string text)
    {
        string[] words = text.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = Translate(words[i]);
        }
        return string.Join(" ", words);
    }

    private string Translate(string word)
    {
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(word)}";
        var webClient = new WebClient
        {
            Encoding = Encoding.UTF8
        };
        var result = webClient.DownloadString(url);
        try
        {
            result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
            return result;
        }
        catch
        {
            return "Error";
        }
    }

    public string ProcessReverse(string text)
    {
        var reverseProcessor = new GoogleTranslateProcessor(toLanguage, fromLanguage);
        return reverseProcessor.Process(text);
    }
}

// TranslationDecorator implementation
public class TranslateDecorator : ITextProcessor
{
    private readonly ITextProcessor _innerProcessor;

    public TranslateDecorator(ITextProcessor innerProcessor)
    {
        _innerProcessor = innerProcessor;
    }

    public string Process(string text)
    {
        return _innerProcessor.Process(text);
    }

    public string ProcessReverse(string text)
    {
        return _innerProcessor.ProcessReverse(text);
    }
}

// CaseCorrectionDecorator implementation
public class CorrectCaseDecorator : ITextProcessor
{
    private readonly ITextProcessor _innerProcessor;

    public CorrectCaseDecorator(ITextProcessor innerProcessor)
    {
        _innerProcessor = innerProcessor;
    }

    public string Process(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string[] sentences = text.Split(new char[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < sentences.Length; i++)
        {
            string sentence = sentences[i].Trim();

            if (!string.IsNullOrEmpty(sentence))
            {
                char firstChar = char.ToUpper(sentence[0]);

                string restOfSentence = sentence.Substring(1).ToLower();

                sentences[i] = firstChar + restOfSentence;
            }
        }

        string correctedText = string.Join(". ", sentences) + ".";

        return correctedText;
    }

    public string ProcessReverse(string text)
    {
        return _innerProcessor.ProcessReverse(text);
    }
}

public class TextFileHandler
{
    public void WriteToFile(string filePath, string text)
    {
        File.WriteAllText(filePath, text);
    }

    public string ReadFromFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}

class Program
{
    static void Main()
    {
        string inputText = "HELLo World. this is test.";

        // Create components
        ITextProcessor baseProcessor = new GoogleTranslateProcessor("en", "uk");
        ITextProcessor translator = new TranslateDecorator(baseProcessor);
        ITextProcessor caseCorrector = new CorrectCaseDecorator(translator);

        // Process text
        string translatedText = translator.Process(inputText);
        string correctedText = caseCorrector.Process(translatedText);

        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Original Text: " + inputText);
        Console.WriteLine("Translated Text: " + translatedText);
        Console.WriteLine("Corrected Text: " + correctedText);

        // Handle file operations
        TextFileHandler fileHandler = new TextFileHandler();
        string filePath = "processedText.txt";

        // Write processed text to file
        fileHandler.WriteToFile(filePath, correctedText);

        // Read text from file
        string readText = fileHandler.ReadFromFile(filePath);
        Console.WriteLine("Read Text from File: " + readText);

        // Reverse translation
        string reverseTranslatedText = caseCorrector.ProcessReverse(readText);
        Console.WriteLine("Reverse Translated Text: " + reverseTranslatedText);
    }
}