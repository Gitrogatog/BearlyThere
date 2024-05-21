using Godot;
using System;

public partial class FoodMeter : Control
{
	StealthGameLoop gameLoop;
	public override void _Ready()
	{
		// gameLoop = GetNode<StealthGameLoop>("/root/StealthGameLoop");
		var globalEvents = GetNode<GlobalEvents>("/root/GlobalEvents");
		globalEvents.CollectFood += UpdateBar;
		globalEvents.StartLevel += ResetBar;
	}

	void UpdateBar()
	{

	}

	void ResetBar()
	{

	}
}
