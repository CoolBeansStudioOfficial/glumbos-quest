using Godot;
using System;

public partial class HomingEnemy : Enemy
{
    [Export] float chargeSpeed;
    [Export] float deceleration;
    [Export] float fallMultiplier;
    [Export] float waitSeconds;

    float secondsLeft;

    public override void _Ready()
    {
        secondsLeft = waitSeconds;
    }

    public override void _Process(double delta)
    {
        if (GameManager.Singleton.paused) return;

        if (secondsLeft > 0)
        {
            Velocity = new(Mathf.MoveToward(Velocity.X, 0, deceleration * (float)delta), Velocity.Y, Mathf.MoveToward(Velocity.Z, 0, deceleration * (float)delta));
        }
        else
        {
            Vector3 direction = (GameManager.Singleton.player.Position - Position) / Position.DistanceTo(GameManager.Singleton.player.Position);
            Vector3 velocity = direction * chargeSpeed;
            Velocity = velocity;

            secondsLeft = waitSeconds;
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
                player.Hit();
            }
        }
    }
}
