using Godot;
using System;

public partial class Spin : Node3D
{
	[Export] float speed;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RotationDegrees = new(RotationDegrees.X, 
			RotationDegrees.Y + (float)delta * speed,
			RotationDegrees.Z);
	}
}
