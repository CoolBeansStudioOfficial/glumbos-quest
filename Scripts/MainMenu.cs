using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] public Button playButton;
    [Export] Button settingsButton;

	public override void _Ready()
	{
        playButton.Pressed += () => GameManager.Singleton.StartGame();
        settingsButton.Pressed += SettingsPressed;
    }

    void SettingsPressed()
    {
        GameManager.Singleton.settingsMenu.Visible = true;
    }
}
