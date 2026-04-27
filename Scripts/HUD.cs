using Godot;
using System;

public partial class HUD : Control
{
	[Export] Panel[] hearts;
	public override void _Ready()
	{
	}

	public void SetHearts(int count)
	{
		foreach (var heart in hearts) heart.Visible = false;

		for (int i = 0; i < count; i++)
		{
			hearts[i].Visible = true;
		}
	}
}
