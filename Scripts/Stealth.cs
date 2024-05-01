using Godot;

public static class Detection
{
    public static float detectionMeter;
    public static void UpdateDetectionMeter(float detectionChange)
    {
        detectionMeter = Mathf.Clamp(detectionMeter + detectionChange, 0, 1);
    }
}