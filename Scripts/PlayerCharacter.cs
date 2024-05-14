using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export] float moveSpeed;
	public bool IsDead = false;
	public override void _Process(double delta)
	{
		if (!IsDead)
		{
			MovePlayer();
		}

	}

	void MovePlayer()
	{
		float horizontalMove = Input.GetAxis("move_left", "move_right");
		float verticalMove = Input.GetAxis("move_up", "move_down");
		int x = 1;
		Vector2 movement = new Vector2(horizontalMove, verticalMove).Normalized();
		Velocity = moveSpeed * movement;
		MoveAndSlide();
	}
}
