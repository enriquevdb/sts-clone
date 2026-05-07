using Godot;
using System;

public partial class Vulnerable : StatusEffect
{
    public Label _vulnerableLabel;

    public Vulnerable(int stacks) : base("Vulnerable", stacks, "Take 50% more damage")
    {
    }

    public override void Apply(Player player)
    {
        
    }

    public override int ModifyDamageReceived(int damage)
    {
        return (int)(damage * 1.5f);
    }

    public override void Apply(Enemy enemy)
    {
        
    }
}