using Godot;
using System;

public partial class GlobalEvents : Node
{
    [Signal] public delegate void ResetLevelEventHandler();
    [Signal] public delegate void StartLevelEventHandler();
}
