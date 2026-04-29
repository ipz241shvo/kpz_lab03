using System;

// ===== Базовий герой =====
abstract class Hero
{
    public abstract string GetDescription();
    public abstract int GetPower();
}

// ===== Герої =====
class Warrior : Hero
{
    public override string GetDescription()
    {
        return "Warrior";
    }

    public override int GetPower()
    {
        return 100;
    }
}

class Mage : Hero
{
    public override string GetDescription()
    {
        return "Mage";
    }

    public override int GetPower()
    {
        return 80;
    }
}

class Palladin : Hero
{
    public override string GetDescription()
    {
        return "Palladin";
    }

    public override int GetPower()
    {
        return 120;
    }
}

// ===== Базовий декоратор інвентаря =====
abstract class InventoryDecorator : Hero
{
    protected Hero hero;

    public InventoryDecorator(Hero hero)
    {
        this.hero = hero;
    }
}

// ===== Інвентар =====
class Sword : InventoryDecorator
{
    public Sword(Hero hero) : base(hero) { }

    public override string GetDescription()
    {
        return hero.GetDescription() + " + Sword";
    }

    public override int GetPower()
    {
        return hero.GetPower() + 30;
    }
}

class Armor : InventoryDecorator
{
    public Armor(Hero hero) : base(hero) { }

    public override string GetDescription()
    {
        return hero.GetDescription() + " + Armor";
    }

    public override int GetPower()
    {
        return hero.GetPower() + 50;
    }
}

class MagicRing : InventoryDecorator
{
    public MagicRing(Hero hero) : base(hero) { }

    public override string GetDescription()
    {
        return hero.GetDescription() + " + Magic Ring";
    }

    public override int GetPower()
    {
        return hero.GetPower() + 20;
    }
}

class Cloak : InventoryDecorator
{
    public Cloak(Hero hero) : base(hero) { }

    public override string GetDescription()
    {
        return hero.GetDescription() + " + Cloak";
    }

    public override int GetPower()
    {
        return hero.GetPower() + 15;
    }
}

// ===== Program =====
class Program
{
    static void Main(string[] args)
    {
        Hero warrior = new Warrior();
        warrior = new Sword(warrior);
        warrior = new Armor(warrior);
        warrior = new MagicRing(warrior);

        Console.WriteLine(warrior.GetDescription());
        Console.WriteLine("Power: " + warrior.GetPower());

        Console.WriteLine();

        Hero mage = new Mage();
        mage = new MagicRing(mage);
        mage = new Cloak(mage);
        mage = new Armor(mage);

        Console.WriteLine(mage.GetDescription());
        Console.WriteLine("Power: " + mage.GetPower());

        Console.WriteLine();

        Hero palladin = new Palladin();
        palladin = new Sword(palladin);
        palladin = new Sword(palladin);
        palladin = new Armor(palladin);

        Console.WriteLine(palladin.GetDescription());
        Console.WriteLine("Power: " + palladin.GetPower());
    }
}