using System;

// ===== Реалізація (Implementor) =====
interface IRenderer
{
    void Render(string shapeName);
}

// ===== Конкретні реалізації =====
class VectorRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as vectors");
    }
}

class RasterRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as pixels");
    }
}

// ===== Абстракція =====
abstract class Shape
{
    protected IRenderer renderer;

    public Shape(IRenderer renderer)
    {
        this.renderer = renderer;
    }

    public abstract void Draw();
}

// ===== Фігури =====
class Circle : Shape
{
    public Circle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Circle");
    }
}

class Square : Shape
{
    public Square(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Square");
    }
}

class Triangle : Shape
{
    public Triangle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Triangle");
    }
}

// ===== Main =====
class Program
{
    static void Main(string[] args)
    {
        IRenderer vector = new VectorRenderer();
        IRenderer raster = new RasterRenderer();

        Shape circle = new Circle(vector);
        Shape square = new Square(raster);
        Shape triangle = new Triangle(vector);

        circle.Draw();
        square.Draw();
        triangle.Draw();

        Console.WriteLine();

        // демонстрація гнучкості
        Shape anotherCircle = new Circle(raster);
        anotherCircle.Draw();
    }
}