using System;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// ── 1. Template Method: lifecycle hooks ─────────────────────
Console.WriteLine("=== 1. Template Method: Lifecycle Hooks ===");
var div = new LightElementNode("div", "block", false, logging: true);
div.AddClass("container");
var p = new LightElementNode("p", "block", false, logging: true);
p.AddChild(new LightTextNode("Hello!"));
div.AddChild(p);
Console.WriteLine();

// ── 2. Command: DOM маніпуляції з Undo ──────────────────────
Console.WriteLine("=== 2. Command: DOM маніпуляції з Undo ===");
var editor = new DomEditor();
var ul = new LightElementNode("ul", "block", false);
var li1 = new LightElementNode("li", "block", false);
li1.AddChild(new LightTextNode("Item 1"));
var li2 = new LightElementNode("li", "block", false);
li2.AddChild(new LightTextNode("Item 2"));

editor.Execute(new AddChildCommand(ul, li1));
editor.Execute(new AddChildCommand(ul, li2));
Console.WriteLine($"  Дітей після додавання: {ul.ChildCount()}");
editor.Undo();
Console.WriteLine($"  Дітей після Undo:      {ul.ChildCount()}");
Console.WriteLine();

// ── 3. State: стани елемента ────────────────────────────────
Console.WriteLine("=== 3. State: Стани елемента ===");
var btn = new LightElementNode("button", "inline", false);
btn.AddChild(new LightTextNode("Click me"));
Console.WriteLine($"  Normal:      {btn.OuterHTML()}");
btn.SetState(new HiddenState());
Console.WriteLine($"  Hidden:      {btn.OuterHTML()}");
btn.SetState(new HighlightedState());
Console.WriteLine($"  Highlighted: {btn.OuterHTML()}");
btn.SetState(new NormalState());
Console.WriteLine();

// ── 4. Iterator: обхід DOM в глибину і ширину ───────────────
Console.WriteLine("=== 4. Iterator: Обхід DOM ===");
var root = new LightElementNode("html", "block", false);
var body = new LightElementNode("body", "block", false);
var h1 = new LightElementNode("h1", "block", false);
h1.AddChild(new LightTextNode("Title"));
var ul2 = new LightElementNode("ul", "block", false);
var liA = new LightElementNode("li", "block", false);
liA.AddChild(new LightTextNode("A"));
var liB = new LightElementNode("li", "block", false);
liB.AddChild(new LightTextNode("B"));
ul2.AddChild(liA);
ul2.AddChild(liB);
body.AddChild(h1);
body.AddChild(ul2);
root.AddChild(body);

Console.Write("  DFS (глибина): ");
var dfs = new DepthFirstIterator(root);
while (dfs.HasNext())
{
    var node = dfs.Next();
    if (node is LightNodeBase el) Console.Write($"<{el.TagName}> ");
}
Console.WriteLine();

Console.Write("  BFS (ширина):  ");
var bfs = new BreadthFirstIterator(root);
while (bfs.HasNext())
{
    var node = bfs.Next();
    if (node is LightNodeBase el) Console.Write($"<{el.TagName}> ");
}
Console.WriteLine();
Console.WriteLine();

// ── 5. Visitor: статистика + HTML рендер ────────────────────
Console.WriteLine("=== 5. Visitor: Статистика + HTML рендер ===");

var statsVisitor = new StatisticsVisitor();
var iter = new DepthFirstIterator(root);
while (iter.HasNext())
    iter.Next().Accept(statsVisitor);

Console.WriteLine("  Статистика документа:");
statsVisitor.PrintReport();

Console.WriteLine();
var renderVisitor = new HtmlRenderVisitor();
renderVisitor.Visit((LightElementNode)root);
Console.WriteLine("  HTML (через Visitor):");
Console.WriteLine("  " + renderVisitor.GetResult());
