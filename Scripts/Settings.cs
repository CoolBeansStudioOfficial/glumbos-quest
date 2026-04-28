using Godot;
using System;

public partial class Settings : Panel
{
	[Export] Button closeButton;

	public override void _Ready()
	{
        closeButton.Pressed += Close;
	}

    private void Close()
    {
        Visible = false;
    }
}
