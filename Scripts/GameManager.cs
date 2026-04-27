using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Singleton { get; private set; }

    [Export] PackedScene playerScene;

    [Export] public MainMenu mainMenu;
    [Export] public HUD hud;

	public Player player;

	public bool inMenu = true;
	public bool paused = true;

	public override void _Ready()
	{
		Singleton = this;

		mainMenu.playButton.Pressed += StartGame;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			if (inMenu) return;

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

	public void StartGame()
	{
		if (player is not null)
		{
			player.QueueFree();
			player = null;
		}
        player = playerScene.Instantiate() as Player;
        player.Position = new(GD.RandRange(-50, 50), 2, GD.RandRange(-50, 50));
        AddChild(player);

        hud.Visible = true;
        mainMenu.Visible = false;
		mainMenu.Visible = false;
		paused = false;
		inMenu = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

	public void EndGame()
	{
		hud.Visible = false;
		mainMenu.Visible = true;
		inMenu = true;

        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}
