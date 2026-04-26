using Godot;
using System;

public partial class HomingEnemy : CharacterBody3D
{
    [Export] float speed;
    [Export] float deceleration;
    [Export] float fallMultiplier;
    [Export] float waitSeconds;
    [Export] float chargeSeconds;

    bool charging = false;
    float secondsLeft;
    Vector3 targetPosition;

    public override void _Ready()
    {
        secondsLeft = waitSeconds;
        targetPosition = GameManager.Singleton.player.Position;
    }

    public override void _Process(double delta)
    {
        if (charging)
        {
            Vector3 velocity  = (targetPosition - Position) * speed * (float)delta;
            Velocity = velocity;

            if (secondsLeft <= 0)
            {
                charging = false;
                secondsLeft = waitSeconds;
            }
        }
        else
        {
            Velocity = new(Mathf.MoveToward(Velocity.X, 0, deceleration * (float)delta), Velocity.Y, Mathf.MoveToward(Velocity.Z, 0, deceleration * (float)delta));

            if (secondsLeft <= 0)
            {
                charging = true;
                secondsLeft = chargeSeconds;
                targetPosition = GameManager.Singleton.player.Position;
            }
        }

        // Add the gravity.
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta * fallMultiplier;
        }

        MoveAndSlide();
        secondsLeft -= (float)delta;

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            if (GetSlideCollision(i).GetCollider() is Player player)
            {
                GD.Print("ive been hit!");
            }
        }
    }
}
