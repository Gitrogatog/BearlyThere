using Godot;
using System;

public partial class GuardRotate : BaseGuardAI
{
    [Export] float minAngle;
    [Export] float maxAngle;
    [Export] float startAngle = 0;
    [Export] float turnSpeed;
    Node2D rotateNode;
    bool turningToMax;
    public override void _Ready()
    {
        Init();
        rotateNode = guardSightAI;
        GlobalRotation = startAngle;
    }

    public override void _Process(double delta)
    {
        if (guardEnabled)
        {
            MoveGuard((float)delta);
            Vector2 rotVector = Vector2.FromAngle(rotateNode.GlobalRotation);
            SetAnim(rotVector);
        }
    }
    public override void OnStartLevel()
    {
        base.OnStartLevel();
        GlobalRotation = startAngle;
        turningToMax = false;
    }

    public override void MoveGuard(float delta)
    {
        float currentRotation = rotateNode.RotationDegrees;
        if (turningToMax)
        {
            currentRotation += turnSpeed * (float)delta;
            if (currentRotation >= maxAngle)
            {
                currentRotation = maxAngle;
                turningToMax = false;
            }
        }
        else
        {
            currentRotation -= turnSpeed * (float)delta;
            if (currentRotation <= minAngle)
            {
                currentRotation = minAngle;
                turningToMax = true;
            }
        }
        rotateNode.RotationDegrees = currentRotation;
    }
}
