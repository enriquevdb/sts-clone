using Godot;
using System;

public partial class Bash : AttackCard
{
    public Bash()
    {
        CardName = "Bash";
        Description = "Deal 8 Damage";
        EnergyCost = 2;
        Damage = 8;
        Vulnerable = 2;
        Weak = 0;
        Stun = false;
    }
    public override void Play(Enemy target)
    {
        base.Play(target);
        target.AddEffect(new Vulnerable(2));
        GD.Print("Enemy has taken damage, and Vulnerable has been added");
    }

}