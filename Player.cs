using Godot;
using System;
using System.Collections.Generic;

public partial class Player : StaticBody2D
{	
	private int _health;
	private int _maxHealth = 100;
	private int _block = 0;
	private int _energy;
	private int _maxEnergy = 3;
	private List<StatusEffect> _effects = new List<StatusEffect>();
	public Deck Deck { get; private set; } = new Deck();
	
	[Signal]
	public delegate void HealthChangedEventHandler(int newHealth);

	[Signal]
	public delegate void EnergyChangedEventHandler(int newEnergy);
	[Signal]
	public delegate void BlockChangedEventHandler(int newBlock);
	
	[Signal]
	public delegate void OnDeathEventHandler();
	[Signal]
	public delegate void OnClickEventHandler();

	public int Health
	{
		get => _health;
		set
		{
			_health = Mathf.Clamp(value, 0, _maxHealth);
			EmitSignal(SignalName.HealthChanged, _health);

			if(Health <= 0)
			{
				EmitSignal(SignalName.OnDeath);
				QueueFree();
			} 
		}
	}

	public int Energy
	{
		get => _energy;
		set
		{
			_energy = Mathf.Clamp(value, 0, _maxEnergy);
			EmitSignal(SignalName.EnergyChanged, _energy);
		}
	}

	public int Block
	{
		get => _block;
		set
		{
			_block = value;
			EmitSignal(SignalName.BlockChanged, _block);
		}
	}

	public void TakeDamage(int amount)
	{
		int remainingDmg = amount - _block;
		Block = Math.Max(0, _block - amount);
		if (remainingDmg > 0) Health -= remainingDmg;
	}

	public void AddEffect(StatusEffect effect)
	{
		StatusEffect existing = _effects.Find(e => e.Name == effect.Name);
		if (existing != null)
		{
			existing.Stacks += effect.Stacks;
		}
		else
		{
			_effects.Add(effect);
			effect.Apply(this);
		}
	}
	public void TickEffect()
	{
		List<StatusEffect> toRemove = new List<StatusEffect>();
		foreach (StatusEffect effect in _effects)
		{
			effect.Tick();
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

	public override void _Ready()
	{
		Health = _maxHealth;
		Energy = _maxEnergy;
		Deck.PopulateStarterDeck();
		AddChild(Deck);
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				EmitSignal(SignalName.OnClick);
				GD.Print("Player clicked");
			}
		}
	}

	public void ConsumeEnergy(int amount)
	{
		Energy -= amount;
	}

	public void ResetEnergy()
	{
		Energy = _maxEnergy;
	}

	public void ResetBlock()
	{
		Block = 0;
	}

	public void AddBlock(int amount) => Block += amount;

	public int MaxEnergy => _maxEnergy;
}
