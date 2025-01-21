using Assets.Scripts.Base;
using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Enemy;
using Assets.Scripts.Interfaces;
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
    [SerializeField] public Base enemyBase;

    public IEnemyFabric fabric;

    private float cooldown = 15f;
    [SerializeField] private float startCooldown = 15f;
    [SerializeField] private float minCooldown = 5f;
    [SerializeField] private float firstEnemyCreate = 10f;
    private float spawnTime;

    public event EventHandler OnSpawn;

    public uint maxCountEnemy = 20;
    private uint currentCountEnemy = 0;
    private uint countSpawnEnemyOnTime = 1;

    private Timer timer;
    public bool spawnerOn = true;
    public float CoolDown
    {
        get { return cooldown; }
        set { cooldown = (value > 0 && value < startCooldown ? value : 10); }
    }
    public float SpawnTime => spawnTime;
    private void Start()
    {
        spawnTime = firstEnemyCreate;
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
            if (timer != null)
            {
                cooldown = minCooldown + timer.TimeLeft / 300 * (startCooldown - minCooldown);
                if ((300 - (int)timer.TimeLeft) % 50 == 0 && timer.TimeLeft < 300)
                    countSpawnEnemyOnTime += (countSpawnEnemyOnTime < 5 ? 1u : 0);
            }
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
            int[] array = new int[spawnPoints.Count];
            for (int i = 0; i < array.Length; i++)
                array[i] = i;
            ShuffleArray(array);
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < array.Length; i++)
                queue.Enqueue(array[i]);
            for (int i = 0; i < countSpawnEnemyOnTime; i++)
            {
                int index = random.Next(values.Length / (timer.TimeLeft > 300 - 25 ? 3 : timer.TimeLeft > 300 - 60 ? 2 : 1));

                EnemyType enemyType = (EnemyType)values.GetValue(index);

                GameObject obj = fabric.GetPrefab(enemyType);

                if (obj != null)
                {
                    index = random.Next(spawnPoints.Count);
                    GameObject myZombie = Instantiate(obj, spawnPoints[queue.Dequeue()].position, Quaternion.identity);
                    if (myZombie != null)
                    {
                        currentCountEnemy++;
                        myZombie.GetComponent<IEnemy>().OnDestroy +=
                            (object sender, EventArgs e) => { if (currentCountEnemy > 0) currentCountEnemy--; };
                        OnSpawn?.Invoke(this, new EventEnemySpawnArgs(myZombie, enemyType));
                    }
                }
            }
        }
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            // Выбираем случайный индекс от 0 до i
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            // Меняем местами текущий элемент с элементом на случайном индексе
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}