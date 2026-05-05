using Godot;
using System;

public partial class Strike : AttackCard
{
    public Strike()
    {
        CardName = "Strike";
        Description = "Deal 6 Damage";
        EnergyCost = 1;
        Damage = 6;
        Vulnerable = 0;
        Weak = 0;
        Stun = false;
    }

}