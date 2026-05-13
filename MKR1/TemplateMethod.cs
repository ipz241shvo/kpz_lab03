using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// ПАТЕРН: ШАБЛОННИЙ МЕТОД (Template Method)
//
// LightNodeBase визначає скелет алгоритму (OuterHTML, AddChild,
// AddClass) і залишає lifecycle hooks відкритими для підкласів:
//   OnCreated, OnChildAdded, OnChildRemoved, OnClassAdded, OnRendered
// ============================================================

abstract class LightNodeBase : LightNode
{
    public string TagName { get; }
    public string DisplayType { get; }
    public bool IsSelfClosing { get; }

    protected readonly List<string> Classes = new();
    protected readonly List<LightNode> Children = new();

    private IElementState _state = new NormalState();
    public IElementState State => _state;

    protected LightNodeBase(string tagName, string displayType, bool isSelfClosing)
    {
        TagName = tagName;
        DisplayType = displayType;
        IsSelfClosing = isSelfClosing;
        OnCreated();
    }

    // Lifecycle hooks — перевизначаються в підкласах
    protected virtual void OnCreated() { }
    protected virtual void OnChildAdded(LightNode child) { }
    protected virtual void OnChildRemoved(LightNode child) { }
    protected virtual void OnClassAdded(string className) { }
    protected virtual void OnRendered() { }

    public void SetState(IElementState state)
    {
        _state = state;
        Console.WriteLine($"  [State] <{TagName}> -> стан: {state.StateName}");
    }

    public void AddClass(string className)
    {
        Classes.Add(className);
        OnClassAdded(className);
    }

    public void RemoveClass(string className) => Classes.Remove(className);

    public void AddChild(LightNode node)
    {
        Children.Add(node);
        OnChildAdded(node);
    }

    public bool RemoveChild(LightNode node)
    {
        bool removed = Children.Remove(node);
        if (removed) OnChildRemoved(node);
        return removed;
    }

    public IReadOnlyList<LightNode> GetChildren() => Children;
    public int ChildCount() => Children.Count;

    protected string GetClassAttr()
    {
        if (Classes.Count == 0) return "";
        return $" class=\"{string.Join(" ", Classes)}\"";
    }

    protected string GetStyleAttr()
    {
        string style = _state.GetInlineStyle();
        return string.IsNullOrEmpty(style) ? "" : $" style=\"{style}\"";
    }

    // Шаблонний метод: визначає структуру рендерингу
    public override string OuterHTML()
    {
        OnRendered();
        if (IsSelfClosing)
            return $"<{TagName}{GetClassAttr()}{GetStyleAttr()} />";

        var sb = new StringBuilder();
        sb.Append($"<{TagName}{GetClassAttr()}{GetStyleAttr()}>");
        sb.Append(InnerHTML());
        sb.Append($"</{TagName}>");
        return sb.ToString();
    }

    public override string InnerHTML()
    {
        var sb = new StringBuilder();
        foreach (var child in Children)
            sb.Append(child.OuterHTML());
        return sb.ToString();
    }
}

// Конкретний елемент — реалізує hooks з опціональним логуванням
class LightElementNode : LightNodeBase
{
    private readonly bool _logging;

    public LightElementNode(string tagName, string displayType, bool isSelfClosing, bool logging = false)
        : base(tagName, displayType, isSelfClosing)
    {
        _logging = logging;
    }

    protected override void OnCreated()
    {
        if (_logging) Console.WriteLine($"  [Hook] OnCreated       <{TagName}>");
    }

    protected override void OnChildAdded(LightNode child)
    {
        if (_logging) Console.WriteLine($"  [Hook] OnChildAdded    <{TagName}>");
    }

    protected override void OnChildRemoved(LightNode child)
    {
        if (_logging) Console.WriteLine($"  [Hook] OnChildRemoved  <{TagName}>");
    }

    protected override void OnClassAdded(string className)
    {
        if (_logging) Console.WriteLine($"  [Hook] OnClassAdded    <{TagName}> class='{className}'");
    }

    protected override void OnRendered()
    {
        if (_logging) Console.WriteLine($"  [Hook] OnRendered      <{TagName}>");
    }

    public override void Accept(ILightNodeVisitor visitor) => visitor.Visit(this);
}
