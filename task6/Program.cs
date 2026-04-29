using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

abstract class LightNode
{
    public abstract string OuterHTML();
}

class LightTextNode : LightNode
{
    private string text;

    public LightTextNode(string text)
    {
        this.text = text;
    }

    public override string OuterHTML()
    {
        return text;
    }
}

class LightElementNode : LightNode
{
    public string TagName { get; }
    public bool IsSelfClosing { get; }
    public List<LightNode> Children { get; }

    public LightElementNode(string tagName, bool isSelfClosing = false)
    {
        TagName = tagName;
        IsSelfClosing = isSelfClosing;
        Children = new List<LightNode>();
    }

    public void AddChild(LightNode node)
    {
        Children.Add(node);
    }

    public override string OuterHTML()
    {
        if (IsSelfClosing)
        {
            return $"<{TagName}/>";
        }

        StringBuilder sb = new StringBuilder();

        sb.Append($"<{TagName}>");

        foreach (var child in Children)
        {
            sb.Append(child.OuterHTML());
        }

        sb.Append($"</{TagName}>");

        return sb.ToString();
    }
}

// ===== Flyweight =====
class LightElementFlyweight
{
    public string TagName { get; }

    public LightElementFlyweight(string tagName)
    {
        TagName = tagName;
    }
}

class LightElementFactory
{
    private static Dictionary<string, LightElementFlyweight> flyweights = new();

    public static LightElementFlyweight GetFlyweight(string tagName)
    {
        if (!flyweights.ContainsKey(tagName))
        {
            flyweights[tagName] = new LightElementFlyweight(tagName);
        }

        return flyweights[tagName];
    }

    public static int Count()
    {
        return flyweights.Count;
    }
}

class LightElementNodeFlyweight : LightNode
{
    private LightElementFlyweight flyweight;
    private LightTextNode textNode;

    public LightElementNodeFlyweight(LightElementFlyweight flyweight, string text)
    {
        this.flyweight = flyweight;
        this.textNode = new LightTextNode(text);
    }

    public override string OuterHTML()
    {
        return $"<{flyweight.TagName}>{textNode.OuterHTML()}</{flyweight.TagName}>";
    }
}

class Program
{
    static void Main(string[] args)
    {
        string filePath = "book.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл book.txt не знайдено!");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        // ===== WITHOUT FLYWEIGHT =====
        List<LightNode> normalTree = new List<LightNode>();

        long memoryBeforeNormal = GC.GetTotalMemory(true);

        for (int i = 0; i < lines.Length; i++)
        {
            string tagName = GetTagName(lines[i], i);

            LightElementNode element = new LightElementNode(tagName);
            element.AddChild(new LightTextNode(lines[i]));

            normalTree.Add(element);
        }

        long memoryAfterNormal = GC.GetTotalMemory(true);
        long normalMemory = memoryAfterNormal - memoryBeforeNormal;

        Console.WriteLine("=== WITHOUT FLYWEIGHT ===");
        Console.WriteLine("Total elements: " + normalTree.Count);
        Console.WriteLine("Memory used: " + normalMemory + " bytes");

        // ===== WITH FLYWEIGHT =====
        List<LightNode> flyweightTree = new List<LightNode>();

        long memoryBeforeFlyweight = GC.GetTotalMemory(true);

        for (int i = 0; i < lines.Length; i++)
        {
            string tagName = GetTagName(lines[i], i);

            LightElementFlyweight flyweight =
                LightElementFactory.GetFlyweight(tagName);

            LightElementNodeFlyweight element =
                new LightElementNodeFlyweight(flyweight, lines[i]);

            flyweightTree.Add(element);
        }

        long memoryAfterFlyweight = GC.GetTotalMemory(true);
        long flyweightMemory = memoryAfterFlyweight - memoryBeforeFlyweight;

        Console.WriteLine("\n=== WITH FLYWEIGHT ===");
        Console.WriteLine("Total elements: " + flyweightTree.Count);
        Console.WriteLine("Unique tag flyweights: " + LightElementFactory.Count());
        Console.WriteLine("Memory used: " + flyweightMemory + " bytes");

        Console.WriteLine("\n=== SAMPLE OUTPUT ===");

        for (int i = 0; i < Math.Min(15, flyweightTree.Count); i++)
        {
            Console.WriteLine(flyweightTree[i].OuterHTML());
        }
    }

    static string GetTagName(string line, int index)
    {
        if (index == 0)
        {
            return "h1";
        }

        if (line.StartsWith(" "))
        {
            return "blockquote";
        }

        if (line.Length < 20)
        {
            return "h2";
        }

        return "p";
    }
}