using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export] float speed;

	public override void _Process(double delta)
	{
		Vector3 velocity = Velocity;
		Vector3 toPlayer = (GameManager.Singleton.player.Position - Position) * speed * (float)delta;
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
