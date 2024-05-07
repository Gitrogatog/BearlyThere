using Godot;
using System;

public partial class PatrolGuardAI : Node2D
{
    [Export] Node2D[] waypoints;
    [Export] float moveSpeed;
    [Export] float turnSpeed;
    [Export] float waypointDistCheck;
    float sqrDistCheck;
    Node2D moveNode;
    Node2D rotateNode;
    int targetWaypointIndex = 0;

    public override void _Ready()
    {
        sqrDistCheck = waypointDistCheck * waypointDistCheck;
        moveNode = this;
        rotateNode = GetNode<Node2D>("GuardSightAI");
    }
    public override void _Process(double delta)
    {
        Vector2 targetPos = waypoints[targetWaypointIndex].GlobalPosition;
        Vector2 vectorToTarget = targetPos - moveNode.GlobalPosition;
        Vector2 moveVector = vectorToTarget.Normalized() * (moveSpeed * (float)delta);
        moveNode.GlobalPosition += moveVector;

        float targetAngle = vectorToTarget.Angle() - Mathf.Pi;
        float angleDiff = Mathf.AngleDifference(rotateNode.Rotation, targetAngle);
        rotateNode.Rotation -= Mathf.Sign(angleDiff) * Mathf.DegToRad(turnSpeed) * (float)delta;

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
