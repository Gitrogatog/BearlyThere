using Godot;
using System;

public partial class FollowPlayerCamera : Camera2D
{
	[Export] Node2D playerNode;
	public override void _Process(double delta)
	{
		Offset = playerNode.GlobalPosition;
	}
}
