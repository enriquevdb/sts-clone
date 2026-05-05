using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node
{
	private List<CardData> _drawPile = new List<CardData>();
	private List<CardData> _discardPile = new List<CardData>();

	[Signal]
	public delegate void OnCardDrawEventHandler();
	[Signal]
	public delegate void OnDiscardEventHandler();

	public void AddCard(CardData card)
	{
		_drawPile.Add(card);
	}

	public void RemoveCard(CardData card)
	{
		_drawPile.Remove(card);
	}

	public CardData DrawCard()
	{
		if(_drawPile.Count == 0)
		{
			_drawPile.AddRange(_discardPile);
			_discardPile.Clear();
			EmitSignal(SignalName.OnDiscard);
			Shuffle(_drawPile);
		}

		if(_drawPile.Count == 0) return null; // JUST IN CASE

		CardData card = _drawPile[0];
		_drawPile.RemoveAt(0);
		EmitSignal(SignalName.OnCardDraw);
		return card;
	}

	public void PopulateStarterDeck()
	{
		for (int i = 0; i < 6; i++) AddCard(new Strike());
		for (int i = 0; i < 4; i++) AddCard(new Defend());		
	}

	public void Shuffle(List<CardData> pile)
	{
		// ** LEARN **
		Random rnd = new Random();
		int n = pile.Count;

		while (n > 1)
		{
			n--;
			int k = rnd.Next(n + 1);
			CardData temp = pile[k];
			pile[k] = pile[n];
			pile[n] = temp;
		}
	}

	public void Discard(CardData card)
	{
		_discardPile.Add(card);
		EmitSignal(SignalName.OnDiscard);
	}

	public int DrawPileCount => _drawPile.Count;
	public int DiscardPileCount => _discardPile.Count;

}
