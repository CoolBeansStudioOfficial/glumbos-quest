using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export] float acceleration;
    [Export] float maxSpeed;

    Vector3 tracking;
    float timeToTrack = 0f;

	public override void _Process(double delta)
	{
        if (GameManager.Singleton.paused || GameManager.Singleton.player is null) return;
        //rotates just in case billboard texture doesnt work for crap drivers
        Transform = Transform.LookingAt(GameManager.Singleton.player.Position);

        Vector3 velocity = Velocity;
        Vector3 direction = (GameManager.Singleton.player.Position - Position) / Position.DistanceTo(GameManager.Singleton.player.Position);
        Vector3 toPlayer = direction * acceleration;
		velocity.X += toPlayer.X * (float)delta;
        velocity.X = Mathf.Clamp(velocity.X, -maxSpeed, maxSpeed);
		velocity.Z += toPlayer.Z * (float)delta;
        velocity.Z = Mathf.Clamp(velocity.Z, -maxSpeed, maxSpeed);

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        Velocity = velocity;
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            if (GetSlideCollision(i).GetCollider() is Player player)
            {
                player.Hit();
            }
        }
    }
}
