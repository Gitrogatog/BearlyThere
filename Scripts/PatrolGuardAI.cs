using Godot;
using System;

public partial class PatrolGuardAI : Node2D
{
    [Export] Node2D[] waypoints;
    [Export] float moveSpeed;
    [Export] float turnSpeed;
    [Export] float waypointDistCheck;
    float sqrDistCheck;
    Node2D guardNode;
    int targetWaypointIndex = 0;

    public override void _Ready()
    {
        sqrDistCheck = waypointDistCheck * waypointDistCheck;
        guardNode = this;
    }
    public override void _Process(double delta)
    {
        Vector2 target = waypoints[targetWaypointIndex].GlobalPosition;
        Vector2 vectorToTarget = target - guardNode.GlobalPosition;
        Vector2 moveVector = vectorToTarget.Normalized() * (moveSpeed * (float)delta);
        guardNode.GlobalPosition += moveVector;
        if (vectorToTarget.LengthSquared() <= sqrDistCheck)
        {
            targetWaypointIndex++;
            if (targetWaypointIndex >= waypoints.Length)
            {
                targetWaypointIndex = 0;
            }
        }


    }
}
