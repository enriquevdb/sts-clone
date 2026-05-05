using Godot;
using System;

public partial class SkillCard : CardData
{
    public int Block { get; set; }
    public int Strength { get; set; }
    public override void Play(Enemy target)
    {
        base.Play(target);
    }

    public override void Play(Player player)
    {
        base.Play(player);
        player.AddBlock(Block);
    }

}