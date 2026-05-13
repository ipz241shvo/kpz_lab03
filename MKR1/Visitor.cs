using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// ПАТЕРН: ВІДВІДУВАЧ (Visitor)
//
// Дозволяє додавати нові операції над вузлами DOM без зміни
// їхніх класів. Кожен відвідувач реалізує окрему поведінку:
//   HtmlRenderVisitor   — генерує HTML рядок
//   StatisticsVisitor   — збирає статистику по документу
// ============================================================

interface ILightNodeVisitor
{
    void Visit(LightTextNode node);
    void Visit(LightElementNode node);
}

// Відвідувач-рендерер: викликає OuterHTML і збирає рядок
class HtmlRenderVisitor : ILightNodeVisitor
{
    private readonly StringBuilder _sb = new();

    public void Visit(LightTextNode node) => _sb.Append(node.OuterHTML());
    public void Visit(LightElementNode node) => _sb.Append(node.OuterHTML());

    public string GetResult() => _sb.ToString();
}

// Відвідувач-статистик: рахує вузли і частоту тегів
class StatisticsVisitor : ILightNodeVisitor
{
    public int ElementCount { get; private set; }
    public int TextNodeCount { get; private set; }
    public Dictionary<string, int> TagFrequency { get; } = new();

    public void Visit(LightTextNode node) => TextNodeCount++;

    public void Visit(LightElementNode node)
    {
        ElementCount++;
        TagFrequency.TryGetValue(node.TagName, out int count);
        TagFrequency[node.TagName] = count + 1;
    }

    public void PrintReport()
    {
        Console.WriteLine($"  Елементів: {ElementCount}");
        Console.WriteLine($"  Текстових вузлів: {TextNodeCount}");
        Console.WriteLine("  Теги:");
        foreach (var kv in TagFrequency)
            Console.WriteLine($"    <{kv.Key}>: {kv.Value}");
    }
}
