using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatManager : Node
{
	// Private Variables
	private Ui _ui;
	private Player _player;
	private List<Enemy> _enemies = new List<Enemy>();
	private Hand _hand;

	// State Manager
	private State _currentState;
	private enum State
	{
		StartOfPlayerTurn,
		PlayerTurn,
		EndOfPlayerTurn,
		StartOfEnemyTurn,
		EnemyTurn,
		EndOfEnemyTurn,
		Victory,
		Defeat
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ui = GetNode<Ui>("/root/Main/UI");
		_player = GetNode<Player>("/root/Main/Player");

		_hand = GetNode<Hand>("/root/Main/UI/Hand");
		_hand.InitializeDeck(_player.Deck);
		
		PopulateEnemies();
		ChangeState(State.StartOfPlayerTurn);

		_player.OnClick += () => _hand.ApplyCard(null);

		_ui.EndTurnPressed += () => ChangeState(State.EndOfPlayerTurn);
		_player.OnDeath += () => ChangeState(State.Defeat);
	}

	public void CheckVictory()
	{
		foreach(Enemy enemy in _enemies)
		{
			if (enemy.Health > 0) return;
		}
		ChangeState(State.Victory);
	}

	private void ChangeState(State newState)
	{
		_currentState = newState;

		switch (_currentState)
		{
			case State.StartOfPlayerTurn:
				GD.Print("Start of Player Turn");
				_hand.PopulateHand();
				_player.ResetEnergy();
				_player.ResetBlock();
				foreach(Enemy enemy in _enemies)
				{
					enemy.DecideIntent();
				}
				ChangeState(State.PlayerTurn);
				break;
			case State.PlayerTurn:
				GD.Print("Player Turn");
				// Player gets to play cards, use potions etc..
				break;
			case State.EndOfPlayerTurn:
				GD.Print("End Of Player Turn");
				_hand.ClearHand();
				ChangeState(State.StartOfEnemyTurn);
				break;
			case State.StartOfEnemyTurn:
				GD.Print("Start of Enemy Turn");
				ChangeState(State.EnemyTurn);
				break;
			case State.EnemyTurn:
				GD.Print("Enemy Turn");
				foreach(Enemy enemy in _enemies)
				{
					enemy.ExecuteIntent(_player);
				}
				ChangeState(State.EndOfEnemyTurn);
				break;
			case State.EndOfEnemyTurn:
				GD.Print("End Of Enemy Turn");
				ChangeState(State.StartOfPlayerTurn);
				break;
			case State.Victory:
				GetTree().ChangeSceneToFile("res://UI/VictoryScreen.tscn");
				break;
			case State.Defeat:
				GetTree().ChangeSceneToFile("res://UI/DefeatScreen.tscn");
				break;
		}
	}

	private void PopulateEnemies()
	{
		foreach(Enemy enemy in GetTree().GetNodesInGroup("Enemies"))
		{
			_enemies.Add(enemy);
			enemy.OnClick += _hand.ApplyCard;
			enemy.OnDeath += CheckVictory;
		}
	}
}
