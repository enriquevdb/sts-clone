using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : StaticBody2D
{
	// Variables
	private int _health;
	private int _maxHealth = 100;
	private int _block = 0;
	private int _minBlock = 4;
	private int _maxBlock = 11;
	private int _minDamage = 5;
	private int _maxDamage = 30;
	private List<StatusEffect> _effects = new List<StatusEffect>();

	// Intent
	private ActionType _action;
	private int _intentAmount;
	enum ActionType
	{
		Defend,
		Attack
	}

	// UI
	private Label _enemyHealthLabel;
	private Label _enemyBlockLabel;
	private Label _intentActionLabel;
	private Label _intentAmountLabel;
	private VBoxContainer _statusEffects;
	private Dictionary<string, Label> _effectLabels = new Dictionary<string, Label>();

	// Signals
	[Signal]
	public delegate void OnClickEventHandler(Enemy enemy);

	[Signal]
	public delegate void OnDeathEventHandler();
	[Signal]
	public delegate void BlockChangedEventHandler(int newBlock);
	[Signal]
	public delegate void StatusEffectUpdateEventHandler(string effectName, int stacks);

	public int Health
	{
		get => _health;
		set
		{
			_health = Mathf.Clamp(value, 0, _maxHealth);
			if(_enemyHealthLabel != null) _enemyHealthLabel.Text = $"HP: {_health}";
			
			if(Health <= 0)
			{
				EmitSignal(SignalName.OnDeath);
				QueueFree();
			} 
		}
	}

	public int Block
	{
		get => _block;
		set
		{
			_block = value;
			if(_enemyBlockLabel != null) _enemyBlockLabel.Text = $"Block: {Block}";
			EmitSignal(SignalName.BlockChanged, _block);
		}
	}

	public void AddEffect(StatusEffect effect)
	{
		StatusEffect existing = _effects.Find(e => e.Name == effect.Name);
		if (existing != null)
		{
			existing.Stacks += effect.Stacks;
			EmitSignal(SignalName.StatusEffectUpdate, existing.Name, existing.Stacks);
			UpdateEffectLabel(existing.Name, existing.Stacks);
		}
		else
		{
			_effects.Add(effect);
			effect.Apply(this);
			EmitSignal(SignalName.StatusEffectUpdate, effect.Name, effect.Stacks);
			UpdateEffectLabel(effect.Name, effect.Stacks);
		}
	}

	public void TickEffect()
	{
		List<StatusEffect> toRemove = new List<StatusEffect>();
		foreach (StatusEffect effect in _effects)
		{
			effect.Tick();
			EmitSignal(SignalName.StatusEffectUpdate, effect.Name, effect.Stacks);
			UpdateEffectLabel(effect.Name, effect.Stacks);
			if (effect.IsExpired)
			{
				toRemove.Add(effect);
			}
		}

		foreach(StatusEffect effect in toRemove)
		{
			effect.Remove(this);
			_effects.Remove(effect);
		}
	}

	private void UpdateEffectLabel(string effectName, int stacks)
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

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				EmitSignal(SignalName.OnClick, this);
				GD.Print("Enemy clicked");
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Health = _maxHealth;

		_intentActionLabel = GetNode<Label>("Intent Action");
		_intentAmountLabel = GetNode<Label>("Intent Amount");
		
		_enemyHealthLabel = GetNode<Label>("Health");
		_statusEffects = GetNode<VBoxContainer>("Status Effects");

		_enemyHealthLabel.Text = $"HP: {_maxHealth}";

		_enemyBlockLabel = GetNode<Label>("Block");

		_enemyBlockLabel.Text = $"BLOCK: {Block}";

		AddToGroup("Enemies");
	}

	public void TakeDamage(int amount)
	{
		foreach(StatusEffect effect in _effects)
			amount = effect.ModifyDamageReceived(amount);
		
		int remainingDmg = amount - _block;
		Block = Math.Max(0, _block - amount);
		if (remainingDmg > 0) Health -= remainingDmg;
	}

	public void DecideIntent()
	{
		Random rnd = new Random();
		int rando = rnd.Next(1, 10);
		if(rando > 2)
		{
			// Attack
			_action = ActionType.Attack;
			_intentAmount = rnd.Next(_minDamage, _maxDamage);
		}
		else
		{
			// Block
			_action = ActionType.Defend;
			_intentAmount = rnd.Next(_minBlock, _maxBlock);
		}
		_intentActionLabel.Text = $"{_action}";
		_intentAmountLabel.Text = $"{_intentAmount}";
	}

	public void ExecuteIntent(Player player)
	{
		if(_action == ActionType.Attack)
		{
			player.TakeDamage(_intentAmount);
		}
		else
		{
			AddBlock(_intentAmount);
		}
	}
	
	public void AddBlock(int amount) => Block += amount;
}
