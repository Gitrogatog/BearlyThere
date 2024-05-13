using Godot;
using System;

public partial class GuardSightAI : Node2D
{
    FieldOfView fov;
    [Export] float detectionPerSecond = 1f;
    [Export] float maxDetectionTime = 1f;
    public float currentDetectionTime = 0f;
    [Signal]
    public delegate void MaxDetectionEventHandler();
    public override void _Ready()
    {
        fov = GetNode<FieldOfView>("SightCone");
    }
    public override void _Process(double delta)
    {
        foreach (Node2D node in fov.visibleTargets)
        {
            // GD.Print("Saw something!");
            if (node.HasMeta("IsPlayer"))
            {
                SeePlayerThisFrame((float)delta);
                return;
            }
        }
    }
    void SeePlayerThisFrame(float frameDelta)
    {
        Detection.UpdateDetectionMeter(frameDelta * detectionPerSecond);
        currentDetectionTime += frameDelta;
        if (currentDetectionTime >= maxDetectionTime)
        {
            EmitSignal(SignalName.MaxDetection);
        }
        // GD.Print(Detection.detectionMeter);
    }
}
