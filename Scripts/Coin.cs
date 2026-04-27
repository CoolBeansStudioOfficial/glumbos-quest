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

    private async void Collected(Node3D body)
    {
        if (body is Player player)
        {
            audio.Play();
            Visible = false;
        }
    }
}
