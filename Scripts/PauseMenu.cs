using Godot;
using System;

public partial class PauseMenu : Control
{
    [Export] Button continueButton;
    [Export] Button quitButton;
    [Export] Button settingsButton;

    public override void _Ready()
	{
        continueButton.Pressed += ContinueButton_Pressed;
        quitButton.Pressed += QuitButton_Pressed;
	}

    void ContinueButton_Pressed()
    {
        GameManager.Singleton.SetPause(false);
        GameManager.Singleton.ButtonSound();
    }

    void QuitButton_Pressed()
    {
        GameManager.Singleton.SetPause(false);
        GameManager.Singleton.QuitGame();
        GameManager.Singleton.ButtonSound();
    }

    
}
