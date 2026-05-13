using System;
using System.Collections.Generic;

// ============================================================
// ПАТЕРН: КОМАНДА (Command)
//
// Інкапсулює DOM-операції як об'єкти, що дозволяє:
//   - відкладати виконання
//   - вести історію та скасовувати дії (Undo)
// DomEditor — інвокер, що виконує команди і зберігає стек.
// ============================================================

interface ICommand
{
    void Execute();
    void Undo();
}

class AddChildCommand : ICommand
{
    private readonly LightNodeBase _parent;
    private readonly LightNode _child;

    public AddChildCommand(LightNodeBase parent, LightNode child)
    {
        _parent = parent;
        _child = child;
    }

    public void Execute() => _parent.AddChild(_child);
    public void Undo() => _parent.RemoveChild(_child);
}

class RemoveChildCommand : ICommand
{
    private readonly LightNodeBase _parent;
    private readonly LightNode _child;
    private bool _wasRemoved;

    public RemoveChildCommand(LightNodeBase parent, LightNode child)
    {
        _parent = parent;
        _child = child;
    }

    public void Execute() => _wasRemoved = _parent.RemoveChild(_child);
    public void Undo() { if (_wasRemoved) _parent.AddChild(_child); }
}

class AddClassCommand : ICommand
{
    private readonly LightNodeBase _element;
    private readonly string _className;

    public AddClassCommand(LightNodeBase element, string className)
    {
        _element = element;
        _className = className;
    }

    public void Execute() => _element.AddClass(_className);
    public void Undo() => _element.RemoveClass(_className);
}

// Інвокер — виконує команди і підтримує стек для Undo
class DomEditor
{
    private readonly Stack<ICommand> _history = new();

    public void Execute(ICommand command)
    {
        command.Execute();
        _history.Push(command);
        Console.WriteLine($"  [Command] Execute: {command.GetType().Name}");
    }

    public void Undo()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("  [Command] Нічого скасовувати");
            return;
        }
        var command = _history.Pop();
        command.Undo();
        Console.WriteLine($"  [Command] Undo:    {command.GetType().Name}");
    }
}
