using Godot;
using System;

public partial class StealthMeter : Control
{
	[Export] ColorRect detectionMeterUI;
	[Export] float maxDetectionPX = 400;
	public override void _Ready()
	{
		Detection.detectionMeter = 0;
	}
	public override void _Process(double delta)
	{
		detectionMeterUI.Size = new Vector2(Detection.detectionMeter * maxDetectionPX, detectionMeterUI.Size.Y);
	}
}
