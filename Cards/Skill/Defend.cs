using Godot;
using System;

public partial class Defend : SkillCard
{
    public Defend()
    {
        CardName = "Defend";
        Description = "Block 7 Damage";
        EnergyCost = 1;
        Block = 7;
    }

}