using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export] float moveSpeed;
	public bool IsDead = false;
	AnimatedSprite2D spriteNode;
	Vector2 lastMovementInput;
	public override void _Ready()
	{
		spriteNode = GetNode<AnimatedSprite2D>("Sprite2D");
	}
	public override void _Process(double delta)
	{
		if (!IsDead)
		{
			MovePlayer();
			SetAnim(lastMovementInput.Normalized());
		}

	}

	void MovePlayer()
	{
		float horizontalMove = Input.GetAxis("move_left", "move_right");
		float verticalMove = Input.GetAxis("move_up", "move_down");
		Vector2 movement = new Vector2(horizontalMove, verticalMove).Normalized();
		if (movement.LengthSquared() > 0f)
		{
			lastMovementInput = movement;
		}
		// GD.Print($"Movement: {lastMovementInput}, Length: {lastMovementInput.LengthSquared()}");
		Velocity = moveSpeed * movement;
		MoveAndSlide();
	}
	public void SetAnim(Vector2 rotVector)
	{
		bool isRunning = Velocity.LengthSquared() >= 1f;
		if (rotVector.X > 0.5f)
		{
			SetAnimFrame("right", isRunning);
		}
		else if (rotVector.X < -0.5f)
		{
			SetAnimFrame("left", isRunning);
		}
		else
		{
			if (rotVector.Y > 0.5f)
			{
				SetAnimFrame("down", isRunning);
			}
			else if (rotVector.Y < -0.5f)
			{
				SetAnimFrame("up", isRunning);
			}
		}
	}
	void SetAnimFrame(string animName, bool isRunning)
	{
		GD.Print($"Is running: {isRunning}");
		if (!isRunning)
		{
			spriteNode.Play($"{animName}_idle");
		}
		else
		{
			var current_frame = spriteNode.Frame;
			var current_progress = spriteNode.FrameProgress;
			spriteNode.Play($"{animName}_walk");
			spriteNode.SetFrameAndProgress(current_frame, current_progress);
		}

	}
}
