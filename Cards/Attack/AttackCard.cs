using Godot;
using System;

public partial class AttackCard : CardData
{
    public int Damage { get; set; }
    public int Vulnerable { get; set; }
    public int Weak { get; set; }
    public bool Stun { get; set; }

    public override void Play(Enemy target)
    {
        base.Play(target);
        target.TakeDamage(Damage);
        GD.Print("Enemy has taken damage");
    }

}