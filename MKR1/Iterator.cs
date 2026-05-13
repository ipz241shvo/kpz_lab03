using System.Collections.Generic;

// ============================================================
// ПАТЕРН: ІТЕРАТОР (Iterator)
//
// Надає уніфікований спосіб обходу DOM-дерева без розкриття
// його внутрішньої структури.
// DFS (глибина) — обходить гілку до кінця, потім сусідів.
// BFS (ширина) — обходить рівень за рівнем.
// ============================================================

interface ILightIterator
{
    bool HasNext();
    LightNode Next();
    void Reset();
}

// Обхід в глибину (Depth-First Search) через стек
class DepthFirstIterator : ILightIterator
{
    private readonly LightNode _root;
    private readonly Stack<LightNode> _stack = new();

    public DepthFirstIterator(LightNode root)
    {
        _root = root;
        _stack.Push(root);
    }

    public bool HasNext() => _stack.Count > 0;

    public LightNode Next()
    {
        var node = _stack.Pop();
        if (node is LightNodeBase elem)
        {
            var children = elem.GetChildren();
            for (int i = children.Count - 1; i >= 0; i--)
                _stack.Push(children[i]);
        }
        return node;
    }

    public void Reset()
    {
        _stack.Clear();
        _stack.Push(_root);
    }
}

// Обхід в ширину (Breadth-First Search) через чергу
class BreadthFirstIterator : ILightIterator
{
    private readonly LightNode _root;
    private readonly Queue<LightNode> _queue = new();

    public BreadthFirstIterator(LightNode root)
    {
        _root = root;
        _queue.Enqueue(root);
    }

    public bool HasNext() => _queue.Count > 0;

    public LightNode Next()
    {
        var node = _queue.Dequeue();
        if (node is LightNodeBase elem)
        {
            foreach (var child in elem.GetChildren())
                _queue.Enqueue(child);
        }
        return node;
    }

    public void Reset()
    {
        _queue.Clear();
        _queue.Enqueue(_root);
    }
}
