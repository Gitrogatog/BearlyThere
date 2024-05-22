using Godot;
using System;

public partial class PatrolGuardAI : BaseGuardAI
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
        Init();
        sqrDistCheck = waypointDistCheck * waypointDistCheck;
        moveNode = this;
        rotateNode = guardSightAI;
    }
    public override void OnStartLevel()
    {
        base.OnStartLevel();
        targetWaypointIndex = 0;
        GlobalPosition = waypoints[0].GlobalPosition;
        GlobalRotation = 0;
        currentVelocity = moveSpeed;
    }
    public override void _Process(double delta)
    {
        if (guardEnabled)
        {
            MoveGuard((float)delta);
            // SetAnim(rotateNode.GlobalRotation);
        }
    }

    void EnableGuard()
    {
        guardEnabled = true;
        guardSightAI.SetProcess(true);
        guardSightAI.Visible = true;
    }

    void DisableGuard()
    {
        guardEnabled = false;
        guardSightAI.SetProcess(false);
        guardSightAI.Visible = false;
    }

    public override void MoveGuard(float delta)
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
        // Vector2 rotVector = Vector2.FromAngle(rotateNode.GlobalRotation);
        SetAnim(vectorToTarget.Normalized());
    }
}
