using Godot;
using System;

public partial class PowerCard : CardData
{
    public int Block { get; set; }
    public int Strength { get; set; }

    public override void Play(Player player)
    {
        base.Play(player);
        player.AddBlock(Block);
    }

}