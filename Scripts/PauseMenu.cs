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
	}

    void ContinueButton_Pressed()
    {
        GameManager.Singleton.SetPause(false);
    }
}
