using Godot;
using System;
using System.Threading.Tasks;

public partial class HUD : Control
{
	[Export] Panel[] hearts;
	[Export] ProgressBar dashBar;
    [Export] ProgressBar pushBar;
    [Export] Label coinsLabel;

    public override void _Ready()
	{
	}

	public void SetHearts(int count, bool damage = false)
	{
		foreach (var heart in hearts) heart.Visible = false;

		if (count <= 0) return;

		for (int i = 0; i < count; i++) hearts[i].Visible = true;

		if (damage) FlashHearts();

	}

	async Task FlashHearts()
	{
		bool transparent = false;

		for (int i = 0; i < 5000; i += 100)
		{
			if (transparent)
			{
				foreach (var heart in hearts) heart.Modulate = Colors.Transparent;
			}
			else
			{
                foreach (var heart in hearts) heart.Modulate = Colors.White;
            }

			transparent = !transparent;

			await Task.Delay(100);
		}

        foreach (var heart in hearts) heart.Modulate = Colors.White;
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
		if (!GameManager.Singleton.hard) coinsLabel.Text = $"Coins: {coins}/10";
        else coinsLabel.Text = $"Coins: {coins}/25";
    }
}
