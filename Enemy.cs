using Godot;
using System;

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

	// Signals
	[Signal]
	public delegate void OnClickEventHandler(Enemy enemy);

	[Signal]
	public delegate void OnDeathEventHandler();
	[Signal]
	public delegate void BlockChangedEventHandler(int newBlock);

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

		_enemyHealthLabel.Text = $"HP: {_maxHealth}";

		_enemyBlockLabel = GetNode<Label>("Block");

		_enemyBlockLabel.Text = $"BLOCK: {Block}";

		AddToGroup("Enemies");
	}

	public void TakeDamage(int amount)
	{
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
