using Godot;
using System;

public partial class EndMenu : Control
{
    [Export] Button playButton;
    [Export] Button menuButton;

    [Export] Label title;
    [Export] Label coins;


    public override void _Ready()
	{
        playButton.Pressed += TryAgain;
        menuButton.Pressed += MainiMenu;
	}

    void TryAgain()
    {
        GameManager.Singleton.StartGame();
        GameManager.Singleton.ButtonSound();
    }

    void MainiMenu()
    {
        GameManager.Singleton.QuitGame();
        GameManager.Singleton.ButtonSound();
    }

    public void SetWin(bool win, int count)
    {
        if (win)
        {
            title.Text = "You Win!";
            coins.Text = $"Coins: {count}";
        }
        else
        {
            title.Text = "Game Over";
            coins.Text = $"Coins: {count}";
        }
    }
}
