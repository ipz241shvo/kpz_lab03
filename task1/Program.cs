using System;
using System.IO;

// ===== 1. Logger (консольний) =====
class Logger
{
    public void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow; // оранжевий ≈ жовтий
        Console.WriteLine(message);
        Console.ResetColor();
    }
}

// ===== 2. FileWriter =====
class FileWriter
{
    private string path;

    public FileWriter(string path)
    {
        this.path = path;
    }

    public void Write(string message)
    {
        File.AppendAllText(path, message);
    }

    public void WriteLine(string message)
    {
        File.AppendAllText(path, message + Environment.NewLine);
    }
}

// ===== 3. Адаптер =====
class FileLoggerAdapter
{
    private FileWriter fileWriter;

    public FileLoggerAdapter(FileWriter fileWriter)
    {
        this.fileWriter = fileWriter;
    }

    public void Log(string message)
    {
        fileWriter.WriteLine("[LOG] " + message);
    }

    public void Error(string message)
    {
        fileWriter.WriteLine("[ERROR] " + message);
    }

    public void Warn(string message)
    {
        fileWriter.WriteLine("[WARN] " + message);
    }
}

// ===== 4. Main =====
class Program
{
    static void Main(string[] args)
    {
        // Консоль
        Logger logger = new Logger();
        logger.Log("Це звичайне повідомлення");
        logger.Warn("Це попередження");
        logger.Error("Це помилка");

        // Файл через адаптер
        FileWriter writer = new FileWriter("log.txt");
        FileLoggerAdapter fileLogger = new FileLoggerAdapter(writer);

        fileLogger.Log("Запис у файл: лог");
        fileLogger.Warn("Запис у файл: попередження");
        fileLogger.Error("Запис у файл: помилка");

        Console.WriteLine("\nПеревір файл log.txt");
    }
}