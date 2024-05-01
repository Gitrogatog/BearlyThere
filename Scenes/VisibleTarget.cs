using Godot;
using System;

public partial class VisibleTarget : Area2D
{
	bool isVisible = false;
	public void MarkVisible()
	{
		Modulate = Colors.Aqua;
		isVisible = true;
	}
	public override void _Process(double delta)
	{
		if (isVisible)
		{
			isVisible = false;
		}
		else
		{
			Modulate = Colors.White;
		}
	}
}
