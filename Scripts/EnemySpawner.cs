using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node3D
{
	[Export] PackedScene enemyScene;
	[Export] float spawnTime;
	[Export] Vector3 spawnPosition;
    [Export] int maxEnemies;

    List<HomingEnemy> enemies = [];
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

            var enemy = enemyScene.Instantiate() as HomingEnemy;
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
