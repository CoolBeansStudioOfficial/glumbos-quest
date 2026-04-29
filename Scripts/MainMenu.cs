using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] Button playButton;
    [Export] Button settingsButton;
    [Export] Button backButton;
    [Export] Button normalButton;
    [Export] Button hardButton;

    [ExportGroup("Submenus")]
    [Export] Control main;
    [Export] Control play;


    public override void _Ready()
	{
        playButton.Pressed += PlayButton_Pressed;
        settingsButton.Pressed += SettingsPressed;
        backButton.Pressed += BackButton_Pressed;
        normalButton.Pressed += NormalButton_Pressed;
        hardButton.Pressed += HardButton_Pressed;
    }    

    void PlayButton_Pressed()
    {
        Menu(true);
    }

    void BackButton_Pressed()
    {
        Menu();
    }

    void NormalButton_Pressed()
    {
        GameManager.Singleton.hard = false;
        GameManager.Singleton.StartGame();
    }

    void HardButton_Pressed()
    {
        GameManager.Singleton.hard = true;
        GameManager.Singleton.StartGame();
    }

    void SettingsPressed()
    {
        GameManager.Singleton.settingsMenu.Visible = true;
    }

    public void Menu(bool game = false)
    {
        main.Visible = !game;
        play.Visible = game;
    }
}
