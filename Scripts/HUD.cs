using Godot;
using System;

public partial class HUD : Control
{
	[Export] Panel[] hearts;
	[Export] ProgressBar dashBar;
    [Export] ProgressBar pushBar;
    [Export] Label coinsLabel;

    public override void _Ready()
	{
	}

	public void SetHearts(int count)
	{
		foreach (var heart in hearts) heart.Visible = false;

		if (count <= 0) return;

		for (int i = 0; i < count; i++)
		{
			hearts[i].Visible = true;
		}
	}

	public void SetDash(float percent)
	{
		dashBar.Value = percent;
	}

    public void SetPush(float percent)
    {
        pushBar.Value = percent;
    }

    public void SetCoins(int coins)
    {
		coinsLabel.Text = $"Coins: {coins}/10";
    }
}
