using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export] float speed;

	public override void _Process(double delta)
	{
        if (GameManager.Singleton.paused || GameManager.Singleton.player is null) return;
        //rotates just in case billboard texture doesnt work for crap drivers
        Transform = Transform.LookingAt(GameManager.Singleton.player.Position);

        Vector3 velocity = Velocity;
        Vector3 direction = (GameManager.Singleton.player.Position - Position) / Position.DistanceTo(GameManager.Singleton.player.Position);
        Vector3 toPlayer = direction * speed;
		velocity.X = toPlayer.X;
		velocity.Z = toPlayer.Z;

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
