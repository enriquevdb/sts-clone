using Godot;
using System;
using System.Collections;
using System.Reflection.Metadata;

public partial class Card : Control
{
	private CardData _cardData;
	[Signal]
	public delegate void OnClickEventHandler();

	// UI
	private Label _nameLabel;
	private Label _costLabel;
	private RichTextLabel _description;

	public override void _Ready()
	{
		_nameLabel = GetNode<Label>("Panel/VBoxContainer/Name");
		_costLabel = GetNode<Label>("Panel/VBoxContainer/Cost");
		_description = GetNode<RichTextLabel>("Panel/VBoxContainer/Description");

		_nameLabel.Text = _cardData.CardName;
		_costLabel.Text = $"{_cardData.EnergyCost}";
		_description.Text = _cardData.Description;

	}

	public void SetCardData(CardData data)
	{
		_cardData = data;
	}

	public CardData GetCardData => _cardData;

	public void Play(Enemy target)
	{
		_cardData.Play(target);
	}
	public void Play(Player player)
	{
		_cardData.Play(player);
	}

	public int GetEnergy => _cardData.EnergyCost;

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				EmitSignal(SignalName.OnClick);
			}
		}
	}

	public void Select()
	{
		Scale = new Vector2(1.1f, 1.1f);
		Position = Position + new Vector2(0f, -5f);
	}

	public void Deselect()
	{
		Scale = new Vector2(1f, 1f);
		Position = Position - new Vector2(0f, -5f);
	}

}
