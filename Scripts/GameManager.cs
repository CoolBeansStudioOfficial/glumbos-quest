using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Singleton { get; private set; }

	[Export] PackedScene playerScene;

	public Player player;

	public bool paused = false;

	public override void _Ready()
	{
		Singleton = this;

        Input.MouseMode = Input.MouseModeEnum.Captured;

        player = playerScene.Instantiate() as Player;
		player.Position = new(0,2,0);
		AddChild(player);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			paused = !paused;

			if (paused)
			{
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
			else
			{
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
	}
}
