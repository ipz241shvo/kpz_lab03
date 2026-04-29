using System;
using System.Collections.Generic;
using System.Text;

// ===== Базовий клас =====
abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();
}

// ===== Текстовий вузол =====
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

    public override string InnerHTML()
    {
        return text;
    }
}

// ===== Елемент =====
class LightElementNode : LightNode
{
    private string tagName;
    private string displayType; // block / inline
    private bool isSelfClosing;

    private List<string> classes = new List<string>();
    private List<LightNode> children = new List<LightNode>();

    public LightElementNode(string tagName, string displayType, bool isSelfClosing)
    {
        this.tagName = tagName;
        this.displayType = displayType;
        this.isSelfClosing = isSelfClosing;
    }

    public void AddClass(string className)
    {
        classes.Add(className);
    }

    public void AddChild(LightNode node)
    {
        children.Add(node);
    }

    public int ChildCount()
    {
        return children.Count;
    }

    private string GetClasses()
    {
        if (classes.Count == 0) return "";
        return $" class=\"{string.Join(" ", classes)}\"";
    }

    public override string OuterHTML()
    {
        if (isSelfClosing)
        {
            return $"<{tagName}{GetClasses()} />";
        }

        StringBuilder sb = new StringBuilder();
        sb.Append($"<{tagName}{GetClasses()}>");
        sb.Append(InnerHTML());
        sb.Append($"</{tagName}>");

        return sb.ToString();
    }

    public override string InnerHTML()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var child in children)
        {
            sb.Append(child.OuterHTML());
        }

        return sb.ToString();
    }
}

// ===== Program =====
class Program
{
    static void Main(string[] args)
    {
        // створимо список <ul>
        var ul = new LightElementNode("ul", "block", false);
        ul.AddClass("list");

        for (int i = 1; i <= 3; i++)
        {
            var li = new LightElementNode("li", "block", false);
            li.AddChild(new LightTextNode($"Item {i}"));
            ul.AddChild(li);
        }

        Console.WriteLine("OuterHTML:");
        Console.WriteLine(ul.OuterHTML());

        Console.WriteLine("\nInnerHTML:");
        Console.WriteLine(ul.InnerHTML());

        Console.WriteLine("\nChildren count: " + ul.ChildCount());
    }
}