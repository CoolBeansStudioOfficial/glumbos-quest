using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemySpawner : Node3D
{
	[Export] PackedScene[] enemyScenes;
	[Export] float spawnTime;
    [Export] int maxEnemies;

    List<Enemy> enemies = [];
	float timeLeft = 0;

	public void Reset()
	{
		if (enemies.Count == 0) return;
		foreach (Enemy enemy in enemies) enemy.QueueFree();
		enemies.Clear();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (GameManager.Singleton.paused) return;

        if (timeLeft <= 0)
		{
            timeLeft = spawnTime;

			if (enemies.Count > maxEnemies || (GameManager.Singleton.hard && enemies.Count > maxEnemies / 2)) return;

			Enemy enemy;
			//normal
            if (!GameManager.Singleton.hard)
			{
                //normal only
                if (GameManager.Singleton.coins < 6)
				{
                    enemy = enemyScenes[GD.RandRange(0, enemyScenes.Count() - 3)].Instantiate() as Enemy;
                }
                //normal + megas
                else
                {
                    enemy = enemyScenes[GD.RandRange(0, enemyScenes.Count() - 1)].Instantiate() as Enemy;
                }
                
            }
			//megas only
			else
			{
                enemy = enemyScenes[GD.RandRange(enemyScenes.Count() - 2, enemyScenes.Count() - 1)].Instantiate() as Enemy;
            }

			Vector3 position = new(GD.RandRange(-50, 50), 10, GD.RandRange(-50, 50));
			//reroll position if it is too close to the player
			if (GameManager.Singleton.player is not null) while(position.DistanceTo(GameManager.Singleton.player.Position) < 10f)
			{
				position = new(GD.RandRange(-50, 50), 10, GD.RandRange(-50, 50));
            }

			enemy.Position = position;
			AddChild(enemy);
			enemies.Add(enemy);
		}
		else
		{
			timeLeft -= (float)delta;
		}
	}
}
