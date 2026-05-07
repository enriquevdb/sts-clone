using Godot;
using System;
using System.Collections.Generic;

public partial class UI : Control
{
	private Label _healthLabel;
	private Label _energyLabel;
	private Label _blockLabel;
	private Label _drawCount;
	private Label _discardCount;
	private Button _endRound;
	private Player _player;
	private VBoxContainer _statusEffects;
	private Dictionary<string, Label> _effectLabels = new Dictionary<string, Label>();

	[Signal]
	public delegate void EndTurnPressedEventHandler();

	public override void _Ready()
	{
		_player = GetNode<Player>("/root/Main/Player");

		_healthLabel = GetNode<Label>("Health");
		_blockLabel = GetNode<Label>("Block");
		_drawCount = GetNode<Label>("Draw Count");
		_discardCount = GetNode<Label>("Discard Count");
		_endRound = GetNode<Button>("End Round");
		_energyLabel = GetNode<Label>("Energy");
		_statusEffects = GetNode<VBoxContainer>("Status Effects");
		_statusEffects.Position = _player.Position + new Vector2(-220, -60);

		_player.HealthChanged += OnHealthChanged;
		_player.EnergyChanged += OnEnergyChanged;
		_player.BlockChanged += OnBlockChanged;
		_player.StatusEffectUpdate += OnStatusEffectChanged;

		_player.Deck.OnCardDraw += OnDrawPileChanged;
		_player.Deck.OnDiscard += OnDiscardPileChanged;

		_healthLabel.Text = $"HP: {_player.Health}";
		_healthLabel.Position = _player.Position + new Vector2(-40, -100);

		_blockLabel.Text = $"BLOCK: {_player.Block}";
		_blockLabel.Position = _player.Position + new Vector2(-130, -93);

		_drawCount.Text = $"Draw Pile: {_player.Deck.DrawPileCount}";
		_discardCount.Text = $"Discard Pile: {_player.Deck.DiscardPileCount}";

		_energyLabel.Text = $"{_player.Energy}/{_player.MaxEnergy}";
	}

	private void OnHealthChanged(int health)
	{
		_healthLabel.Text = $"HP: {health}";
	}

	private void OnEnergyChanged(int energy)
	{
		_energyLabel.Text = $"{energy}/{_player.MaxEnergy}";
	}

	private void OnBlockChanged(int block)
	{
		_blockLabel.Text = $"Block: {block}";
	}

	private void OnDrawPileChanged()
	{
		_drawCount.Text = $"Draw Pile: {_player.Deck.DrawPileCount}";
	}

	private void OnStatusEffectChanged(string effectName, int stacks)
	{
		if (stacks <= 0)
		{
			if (_effectLabels.TryGetValue(effectName, out Label label))
			{
				label.QueueFree();
				_effectLabels.Remove(effectName);
			}
		}
		else if (_effectLabels.TryGetValue(effectName, out Label existingLabel))
		{
			existingLabel.Text = $"{effectName}: {stacks}";
		}
		else
		{
			Label newLabel = new Label();
			newLabel.Text = $"{effectName}: {stacks}";
			_statusEffects.AddChild(newLabel);
			_effectLabels[effectName] = newLabel;
		}
	}

	private void OnDiscardPileChanged()
	{
		_discardCount.Text = $"Discard Pile: {_player.Deck.DiscardPileCount}";
	}

	public void OnRoundEndPressed()
	{
		GD.Print("End Round");
		EmitSignal(SignalName.EndTurnPressed);
	}
}
