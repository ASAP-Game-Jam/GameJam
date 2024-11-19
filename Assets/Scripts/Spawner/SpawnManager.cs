using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
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
        } else
        {
            spawnTime = cooldown;
            GameObject enemyInstane = Instantiate(EnemyPrefabs[random.Next(0, EnemyPrefabs.Count)], transform.GetChild(enemies[random.Next(0, enemies.Count)].Spawner).transform);
            //enemyInstane.GetComponent<EnemyController>().FinelDestination = transform.GetChild(enemies[random.Next(0, enemies.Count)].Spawner).GetComponent<SpawnPoint>().Destination;
        }
    }
}