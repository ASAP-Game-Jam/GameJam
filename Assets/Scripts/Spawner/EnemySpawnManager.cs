using Assets.Scripts.Enemy;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour, ISpawnerManager
{
    public List<Transform> spawnPoints = new List<Transform>();

    private System.Random random = new System.Random();
    public IBase enemyBase;

    public IEnemyFabric fabric;

    private float cooldown = 10f;
    private float spawnTime;

    public event EventHandler OnSpawn;

    public uint maxCountEnemy = 20;
    private uint currentCountEnemy = 0;

    private Timer timer;
    public bool spawnerOn = true;
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
        timer = FindObjectOfType<Timer>();
        currentCountEnemy = (uint)FindObjectsOfType<Enemy>().Length;
        if (enemyBase != null && enemyBase.BaseType == BaseType.EnemyBase)
            enemyBase.OnDestroy += (object s, EventArgs e) => { spawnerOn = false; };
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
        if (timer != null && !timer.TimerOn)
            cooldown = 0.4f;
    }
    public void Spawn()
    {
        if (spawnPoints.Count == 0)
            throw new InvalidOperationException();

        if (currentCountEnemy < maxCountEnemy && spawnerOn)
        {
            Array values = Enum.GetValues(typeof(EnemyType));
            int index = random.Next(values.Length);

            GameObject obj = fabric.GetPrefab((EnemyType)values.GetValue(index));

            if (obj != null)
            {
                index = random.Next(spawnPoints.Count);
                GameObject myZombie = Instantiate(obj, spawnPoints[index].position, Quaternion.identity);
                if (myZombie != null)
                {
                    currentCountEnemy++;
                    myZombie.GetComponent<IEnemy>().OnDestroy += 
                        (object sender, EventArgs e) => { currentCountEnemy--; };
                    OnSpawn?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}