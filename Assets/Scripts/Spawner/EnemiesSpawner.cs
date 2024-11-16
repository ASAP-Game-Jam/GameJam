using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs;
    public List<Enemy> enemies;

    private System.Random random = new System.Random();

    private float cooldown = 3f;
    private float spawnTime;

    private void Update () {
        if (spawnTime > 0)
        { 
            spawnTime -= Time.deltaTime; 
        }else
        {
            spawnTime = cooldown;
            GameObject enemyInstane = Instantiate(EnemyPrefabs[random.Next(0, EnemyPrefabs.Count)], transform.GetChild(enemies[random.Next(0, enemies.Count)].Spawner).transform);
            enemyInstane.GetComponent<EnemyController>().FinelDestination = transform.GetChild(enemies[random.Next(0, enemies.Count)].Spawner).GetComponent<SpawnPoint>().Destination;
        }

        //foreach(Enemy enemy in enemies)
        //{
        //    if (enemy.isSpawned == false && enemy.spawnTime < Time.time)
        //    {
        //        Instantiate(EnemyPrefabs[(int)enemy.enemytype], transform.GetChild(enemy.Spawner).transform);
        //        enemy.isSpawned = true;
        //    }
        //}
    }

}

