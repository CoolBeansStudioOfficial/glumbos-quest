using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemySpawner : Node3D
{
	[Export] PackedScene[] enemyScenes;
	[Export] float spawnTime;
	[Export] Vector3 spawnPosition;
    [Export] int maxEnemies;

    List<Enemy> enemies = [];
	float timeLeft = 0;

	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (timeLeft <= 0)
		{
            timeLeft = spawnTime;

			if (enemies.Count > maxEnemies) return;

            var enemy = enemyScenes[GD.RandRange(0, enemyScenes.Count() - 1)].Instantiate() as Enemy;
			enemy.Position = spawnPosition;
			AddChild(enemy);
			enemies.Add(enemy);
		}
		else
		{
			timeLeft -= (float)delta;
		}
	}
}
