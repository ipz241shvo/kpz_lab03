using System.Collections.Generic;
using System.Text;

// Базовий абстрактний вузол
abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();
    public abstract void Accept(ILightNodeVisitor visitor);
}

// Текстовий вузол — листок дерева
class LightTextNode : LightNode
{
    private readonly string _text;

    public LightTextNode(string text) => _text = text;

    public override string OuterHTML() => _text;
    public override string InnerHTML() => _text;
    public override void Accept(ILightNodeVisitor visitor) => visitor.Visit(this);
}
