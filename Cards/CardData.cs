using System;
using System.Reflection.Metadata;

public class CardData
{
	public string CardName { get; set; }
	public string Description {get; set; }
	public string CardType { get; set; }
	public int EnergyCost {get; set; }
	 

    public virtual void Play(Enemy target){}

	public virtual void Play(Player player){}

}
