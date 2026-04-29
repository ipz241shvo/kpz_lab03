using System;
using System.IO;
using System.Text.RegularExpressions;

interface ITextReader
{
    char[][] ReadText(string filePath);
}

// ===== Основний клас =====
class SmartTextReader : ITextReader
{
    public char[][] ReadText(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        char[][] result = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].ToCharArray();
        }

        return result;
    }
}

// ===== Проксі з логуванням =====
class SmartTextChecker : ITextReader
{
    private SmartTextReader reader;

    public SmartTextChecker()
    {
        reader = new SmartTextReader();
    }

    public char[][] ReadText(string filePath)
    {
        Console.WriteLine($"Opening file: {filePath}");

        char[][] text = reader.ReadText(filePath);

        Console.WriteLine("File was successfully read");

        int linesCount = text.Length;
        int charsCount = 0;

        foreach (char[] line in text)
        {
            charsCount += line.Length;
        }

        Console.WriteLine($"Lines count: {linesCount}");
        Console.WriteLine($"Characters count: {charsCount}");
        Console.WriteLine($"Closing file: {filePath}");

        return text;
    }
}

// ===== Проксі з обмеженням доступу =====
class SmartTextReaderLocker : ITextReader
{
    private SmartTextReader reader;
    private Regex restrictedPattern;

    public SmartTextReaderLocker(string pattern)
    {
        reader = new SmartTextReader();
        restrictedPattern = new Regex(pattern);
    }

    public char[][] ReadText(string filePath)
    {
        if (restrictedPattern.IsMatch(filePath))
        {
            Console.WriteLine("Access denied!");
            return Array.Empty<char[]>();
        }

        return reader.ReadText(filePath);
    }
}

// ===== Program =====
class Program
{
    static void Main(string[] args)
    {
        string allowedFile = "allowed.txt";
        string secretFile = "secret.txt";

        File.WriteAllLines(allowedFile, new string[]
        {
            "Hello world",
            "Proxy pattern",
            "CSharp lab"
        });

        File.WriteAllLines(secretFile, new string[]
        {
            "This is secret text",
            "Access must be denied"
        });

        Console.WriteLine("=== SmartTextReader ===");
        ITextReader reader = new SmartTextReader();
        char[][] text = reader.ReadText(allowedFile);
        PrintText(text);

        Console.WriteLine("\n=== SmartTextChecker ===");
        ITextReader checker = new SmartTextChecker();
        char[][] checkedText = checker.ReadText(allowedFile);
        PrintText(checkedText);

        Console.WriteLine("\n=== SmartTextReaderLocker ===");
        ITextReader locker = new SmartTextReaderLocker("secret");
        locker.ReadText(secretFile);

        Console.WriteLine("\n=== Locker with allowed file ===");
        char[][] allowedText = locker.ReadText(allowedFile);
        PrintText(allowedText);
    }

    static void PrintText(char[][] text)
    {
        foreach (char[] line in text)
        {
            Console.WriteLine(new string(line));
        }
    }
}