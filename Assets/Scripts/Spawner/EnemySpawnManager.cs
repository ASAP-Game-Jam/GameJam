using Assets.Scripts.Enemy;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour, ISpawnerManager
{
    public List<Transform> spawnPoints = new List<Transform>();

    private System.Random random = new System.Random();

    public IEnemyFabric fabric;

    private float cooldown = 10f;
    private float spawnTime;

    public event EventHandler OnSpawn;

    public float CoolDown
    {
        get { return spawnTime; }
        set { spawnTime = (value > 0 && value < 20 ? value : 10); }
    }
    public float SpawnTime => spawnTime;
    private void Start()
    {
        if (fabric == null)
            fabric = FindObjectOfType<EnemyFabric>();
        if (spawnPoints.Count == 0)
        {
            var points = FindObjectsOfType<SpawnPoint>().ToList<SpawnPoint>();
            if (points.Count > 0)
            {
                spawnPoints.Clear();
                foreach (var point in points)
                    spawnPoints.Add(point.transform);
            }
        }
    }
    private void Update()
    {
        if (spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            spawnTime = cooldown;
            Spawn();
        }
    }
    public void Spawn()
    {
        if (spawnPoints.Count == 0)
            throw new InvalidOperationException();

        Array values = Enum.GetValues(typeof(EnemyType));
        int index = random.Next(values.Length);

        GameObject obj = fabric.GetPrefab((EnemyType)values.GetValue(index));

        if (obj != null)
        {
            index = random.Next(spawnPoints.Count);
            GameObject myZombie = Instantiate(obj, spawnPoints[index].position, Quaternion.identity);
            OnSpawn?.Invoke(this,EventArgs.Empty);
        }
    }
}