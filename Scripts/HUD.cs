using Godot;
using System;

public partial class HUD : Control
{
	[Export] Panel[] hearts;
	[Export] ProgressBar dashBar;

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
}
