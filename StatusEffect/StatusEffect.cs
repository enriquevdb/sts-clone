using System;
using System.Collections;

public abstract class StatusEffect
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int Stacks {get; set; }

    protected StatusEffect(string name, int stacks, string description = "")
    {
        Name = name;
        Description = description;
        Stacks = stacks;
    }

    public virtual void Apply(Player player) { }
    public virtual void Apply(Enemy enemy) { }
    public virtual void Remove(Player player) { }
    public virtual void Remove(Enemy enemy) { }
    public virtual void Update() { }
    public virtual void Tick()
    {
        Stacks--;
    }

    public bool IsExpired => Stacks <= 0;
}
