using Godot;
using System;

public partial class Settings : Panel
{
	[Export] Button closeButton;
    [Export] Slider sensX;
    [Export] Slider sensY;

    public override void _Ready()
	{
        closeButton.Pressed += Close;

        sensX.ValueChanged += (x) => Camera.Singleton.sensitivity.X = (float)x;
        sensY.ValueChanged += (y) => Camera.Singleton.sensitivity.Y = (float)y;
    }

    private void Close()
    {
        Visible = false;
    }
}
