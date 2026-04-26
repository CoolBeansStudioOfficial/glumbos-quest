using Godot;
using System;
using System.Diagnostics;

public partial class Camera : Node3D
{
    public static Camera Singleton { get; private set; }
    public override void _Ready()
    {
        Singleton = this;
        Input.UseAccumulatedInput = false;
    }

    [Export] public Camera3D camera;
    [Export] Vector2 sensitivity;
    [Export] RayCast3D raycast;

    public bool isControlledByMouse = true;

    public Node3D followTarget;
    Vector3 lastRotation;

    public override void _Process(double delta)
    {
        //follow current target
        if (followTarget != null)
        {
            camera.Position = followTarget.GlobalPosition;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent && isControlledByMouse)
        {
            camera.RotationDegrees = GetRotationFromInput(lastRotation, mouseEvent.Relative, sensitivity);
            lastRotation = camera.RotationDegrees;
        }
    }

    public Vector3 GetRotationFromInput(Vector3 startRotation, Vector2 mouseMove, Vector2 sensitivity)
    {
        float desiredX;
        float mouseX = -(mouseMove.X * sensitivity.X);
        float mouseY = mouseMove.Y * sensitivity.Y;

        //Find current look rotation
        desiredX = startRotation.Y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        float xRotation = ChangeAngleInterval(startRotation.X);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //return the calculated rotation
        return new(xRotation, desiredX, startRotation.Z);
    }

    //RIP Mythic Legion, but thanks for the code
    float ChangeAngleInterval(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
            return angle;
        }
        else return angle;
    }

}
