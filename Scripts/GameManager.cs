using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{
	public static GameManager Singleton { get; private set; }

    [Export] PackedScene playerScene;
    [Export] EnemySpawner spawner;

    [Export] public MainMenu mainMenu;
    [Export] public PauseMenu pauseMenu;
    [Export] public HUD hud;

    [Export] Node3D menuPoint;
    [Export] Coin coin;
	[Export] Area3D exit;
    [Export] CollisionShape3D exitShape;

    public Player player;
	int coins = -1;

	public bool inMenu = true;
	public bool paused = true;

	public override void _Ready()
	{
		Singleton = this;

        Camera.Singleton.followTarget = menuPoint;
        Camera.Singleton.isControlledByMouse = false;

        mainMenu.playButton.Pressed += StartGame;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			if (inMenu) return;

			paused = !paused;

            SetPause(paused);
        }
	}

	public void SetPause(bool pause)
	{
        paused = pause;
		pauseMenu.Visible = paused;
		hud.Visible = !paused;

        if (paused)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
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
        player.Position = new(0, 2, 0);
		player.FollowPlayer();
        Camera.Singleton.isControlledByMouse = true;
        AddChild(player);

		coins = -1;
		CollectCoin(false);

		spawner.Reset();

        hud.Visible = true;
		hud.SetHearts(3);
		hud.SetCoins(0);
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
		Camera.Singleton.followTarget = menuPoint;
		Camera.Singleton.isControlledByMouse = false;
		player.QueueFree();
		player = null;
    }

	public async Task CollectCoin(bool respawnDelay = true)
	{
        coins++;
        hud.SetCoins(coins);

		if (coins == 10)
		{
			exit.Visible = true;
			exitShape.Disabled = false;
		}

        if (respawnDelay) await Task.Delay(3000);
        coin.Position = new(GD.RandRange(-20, 20), 0.5f, GD.RandRange(-20, 20));
        coin.Visible = true;
    }
}
