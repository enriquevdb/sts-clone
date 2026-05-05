using Godot;
using System;

public partial class Vulnerable : StatusEffect
{
    public Vulnerable(int stacks) : base("Vulnerable", stacks, "Take 50% more damage")
    {
    }

    public override void Apply(Player player)
    {
        
    }

    public override void Apply(Enemy enemy)
    {
        // increase damage enemy takes by 50%
    }
}