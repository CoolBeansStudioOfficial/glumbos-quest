using Godot;
using System;
using System.Threading.Tasks;

public partial class Coin : Area3D
{
    [Export] AudioStreamPlayer3D audio;

	public override void _Ready()
	{
        BodyEntered += Collected;
	}

    public override void _Process(double delta)
    {
        if (GameManager.Singleton.paused || GameManager.Singleton.player is null) return;
        //rotates just in case billboard texture doesnt work for crap drivers
        Transform = Transform.LookingAt(GameManager.Singleton.player.Position);
    }

    void Collected(Node3D body)
    {
        if (body is Player player)
        {
            audio.Play();
            Visible = false;
            GameManager.Singleton.CollectCoin();
        }
    }
}
