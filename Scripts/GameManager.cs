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
    [Export] public EndMenu endMenu;
    [Export] public Settings settingsMenu;
    [Export] public HUD hud;

    [Export] Node3D menuPoint;
    [Export] Coin coin;
    [Export] AudioStream buttonSound;
    [Export] Area3D exit;
    [Export] CollisionShape3D exitShape;
    [Export] AudioStream exitOpenSound;
    [Export] AudioStream exitSound;

    public Player player;
	public int coins = -1;
    public bool hard = false;

	public bool inMenu = true;
	public bool paused = true;

	public override void _Ready()
	{
		Singleton = this;

        Camera.Singleton.followTarget = menuPoint;
        Camera.Singleton.isControlledByMouse = false;

        exit.BodyEntered += ExitTouched;
	}

    private void ExitTouched(Node3D body)
    {
        if (body is Player player)
		{
            EndGame(true);
            AudioManager.Singleton.PlayStream(exitSound);
        }
    }

    public void ButtonSound()
    {
        AudioManager.Singleton.PlayStream(buttonSound);
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
		hud.SetCoins(0);
        mainMenu.Visible = false;
		endMenu.Visible = false;
		paused = false;
		inMenu = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;

        exit.Visible = false;
        exitShape.Disabled = true;
    }

	public void QuitGame()
	{
		hud.Visible = false;
		endMenu.Visible = false;
        mainMenu.Menu();
		mainMenu.Visible = true;
		inMenu = true;

        spawner.Reset();

        Input.MouseMode = Input.MouseModeEnum.Visible;
		Camera.Singleton.followTarget = menuPoint;
		Camera.Singleton.isControlledByMouse = false;
		if (player is not null) player.QueueFree();
		player = null;
    }

	public void EndGame(bool win = false)
	{
        hud.Visible = false;
        inMenu = true;
        endMenu.SetWin(win, coins);
        endMenu.Visible = true;

        spawner.Reset();

        Input.MouseMode = Input.MouseModeEnum.Visible;
        Camera.Singleton.followTarget = menuPoint;
        Camera.Singleton.isControlledByMouse = false;
        if (player is not null) player.QueueFree();
        player = null;

        exit.Visible = false;
        exitShape.Disabled = true;
    }

	public async Task CollectCoin(bool respawnDelay = true)
	{
        coins++;
        hud.SetCoins(coins);

        if (!hard)
        {
            if (coins == 10)
            {
                exit.Visible = true;
                exitShape.Disabled = false;
                AudioManager.Singleton.PlayStream(exitOpenSound);
            }
        }
        else
        {
            if (coins == 25)
            {
                exit.Visible = true;
                exitShape.Disabled = false;
                AudioManager.Singleton.PlayStream(exitOpenSound);
            }
        }
            

        if (respawnDelay) await Task.Delay(6000);
        coin.Position = new(GD.RandRange(-20, 20), 0.5f, GD.RandRange(-20, 20));
        coin.Visible = true;
    }
}
