using Godot;
using System;

public partial class GuardRotate : Node2D
{
    [Export] float minAngle;
    [Export] float maxAngle;
    [Export] float turnSpeed;
    Node2D rotateNode;
    bool turningToMax;
    public override void _Ready()
    {
        rotateNode = GetNode<Node2D>("GuardSightAI");
    }

    public override void _Process(double delta)
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
