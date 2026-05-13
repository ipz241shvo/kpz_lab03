// ============================================================
// ПАТЕРН: СТАН (State)
//
// Дозволяє елементу змінювати поведінку рендерингу (inline style)
// залежно від поточного стану без умовних операторів у клієнті.
// Стани: Normal, Hidden, Highlighted
// ============================================================

interface IElementState
{
    string StateName { get; }
    string GetInlineStyle();
}

class NormalState : IElementState
{
    public string StateName => "normal";
    public string GetInlineStyle() => "";
}

class HiddenState : IElementState
{
    public string StateName => "hidden";
    public string GetInlineStyle() => "display:none;";
}

class HighlightedState : IElementState
{
    public string StateName => "highlighted";
    public string GetInlineStyle() => "background:yellow;";
}
