using Godot;
using System;
using System.Collections.Generic;

public partial class Hand : Control
{
	public PackedScene CardScene { get; set; } = GD.Load<PackedScene>("res://Cards/Card.tscn");

	private Player _player;
	
	private HBoxContainer _cardContainer;
	private Deck _deck;
	private List<Card> _cardsInHand = new List<Card>();
	private Card _selectedCard;
	

	public void InitializeDeck(Deck deck)
	{
		_deck = deck;
	}

	public override void _Ready()
	{
		_cardContainer = GetNode<HBoxContainer>("CardContainer");
		_player = GetNode<Player>("/root/Main/Player");
		if(_player == null) GD.PrintErr("Player not found");
	}

	public void PopulateHand()
	{
		for (int i = 0; i < 5; i++)
		{
			CardData cardData = _deck.DrawCard();
			if (cardData == null) return;

			Card cardNode = CardScene.Instantiate<Card>();
			cardNode.SetCardData(cardData);

			_cardContainer.AddChild(cardNode);
			_cardsInHand.Add(cardNode);
			cardNode.OnClick += () => SelectCard(cardNode);
		}
	}

	public void SelectCard(Card card)
	{
		if(_selectedCard != null) _selectedCard.Deselect();

		_selectedCard = card;
		_selectedCard.Select();
	}

	public void ApplyCard(Enemy enemy)
	{
		if(_selectedCard == null) return;

		if(_selectedCard.GetCardData is SkillCard && enemy != null) return;

		if(_player.Energy < _selectedCard.GetEnergy) return;

		if(_selectedCard.GetCardData is SkillCard)
		{
			_selectedCard.Play(_player);
		}
		else
		{
			_selectedCard.Play(enemy);
		}
		
		_player.ConsumeEnergy(_selectedCard.GetEnergy);

		_deck.Discard(_selectedCard.GetCardData);

		_cardsInHand.Remove(_selectedCard);
		_selectedCard.QueueFree();
		_selectedCard = null;
	}

	public void ClearHand()
	{
		foreach(Card card in _cardsInHand)
		{
			_deck.Discard(card.GetCardData);
			card.QueueFree();
		}
		_cardsInHand.Clear();
		_selectedCard = null;
	}

}
