using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody3D
{
	[Export] Node3D head;

    [Export] public float SpeedCap = 5.0f;
    [Export] public float DashCap = 5.0f;
    [Export] public float Acceleration = 5.0f;
    [Export] public float Deceleration = 5.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float AirControl = 0.3f;

	float maxSpeed;
	float dashRemaining = 5f;
	bool canDash = true;
	bool dashCooldown = false;

	public void FollowPlayer()
	{
        Camera.Singleton.followTarget = head;
    }

    public override void _Ready()
    {
        maxSpeed = SpeedCap;
    }

	public override void _Process(double delta)
	{
		if (GameManager.Singleton.paused) return;

		if (Input.IsActionPressed("dash") && canDash)
		{
            canDash = false;
            dashRemaining = 5f;
            maxSpeed = DashCap;
        }

        //find forward and right vectors for current rotation
        Quaternion calcRotation = Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(Camera.Singleton.camera.RotationDegrees.Y), 0));
        Vector3 forward = calcRotation * Vector3.Back;
        Vector3 right = calcRotation * Vector3.Right;
        
		
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 input = Input.GetVector("move_left", "move_right", "move_forward", "move_back").Normalized();

        if (input != Vector2.Zero)
		{
			//get move direction relative to what direction camera is facing
			Vector3 rotated = (forward * maxSpeed * input.Y) + (right * maxSpeed * input.X);

			if (IsOnFloor())
			{
                velocity.X = Mathf.MoveToward(velocity.X, rotated.X, Acceleration * (float)delta);
                velocity.Z = Mathf.MoveToward(velocity.Z, rotated.Z, Acceleration * (float)delta);
            }
			else
			{
                velocity.X = Mathf.MoveToward(velocity.X, rotated.X, Acceleration * (float)delta * AirControl);
                velocity.Z = Mathf.MoveToward(velocity.Z, rotated.Z, Acceleration * (float)delta * AirControl);
            }
			
			//hard speed cap
			velocity.X = Mathf.Clamp(velocity.X, -maxSpeed, maxSpeed);
            velocity.Z = Mathf.Clamp(velocity.Z, -maxSpeed, maxSpeed);
        }
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Deceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Deceleration);
		}

		Velocity = velocity;
		MoveAndSlide();

		hitCooldown -= (float)delta;

		
		if (dashRemaining <= 0)
		{
			dashCooldown = true;
            maxSpeed = SpeedCap;
        }
		else
		{
			if (maxSpeed != SpeedCap) GameManager.Singleton.hud.SetDash((dashRemaining / 5) * 100);
			else GameManager.Singleton.hud.SetDash(100);
        }

		if (dashCooldown)
		{
			dashRemaining += (float)delta;
			GameManager.Singleton.hud.SetDash((dashRemaining / 15) * 100);

			if (dashRemaining >= 15)
			{
                canDash = true;
				dashCooldown = false;
            }
		}
		else if (!canDash) dashRemaining -= (float)delta;
	}

	public int hearts = 3;
	float hitCooldown = 0;

	public void Hit()
	{
		if (hitCooldown <= 0)
		{
			hearts--;
			GameManager.Singleton.hud.SetHearts(hearts);
            Velocity = new(Velocity.X, JumpVelocity * 2, Velocity.Z);
            hitCooldown = 5;

			if (hearts == 0) GameManager.Singleton.EndGame();
		}
	}
}
