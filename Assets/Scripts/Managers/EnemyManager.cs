using Assets.Scripts.GameObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class EnemySpawnConfig
    {
        public EnemyType enemyType;
        public GameObject prefab;
    }

    public class EnemyManager : MonoBehaviour, IManager
    {
        public event Action<EnemyType, GameObject> OnEnemySpawned;

        [Header("Spawn Points")]
        public GameObject[] enemyCells;

        [Header("Enemy Configurations")]
        public List<EnemySpawnConfig> enemyConfigs = new List<EnemySpawnConfig>();

        [Header("Spawn Settings")]
        public float firstSpawnDelay = 40f;
        public float spawnCooldown = 30f;
        public bool autoSpawnEnabled = true;
        public int enemiesPerWave = 1;

        [Tooltip("Интервал между спавнами внутри одной волны")]
        public float spawnIntervalInWave = 0.4f;

        // Внутренняя корутина волны
        private Coroutine waveRoutine;

        private List<EnemyType> allowedEnemyTypes = new List<EnemyType>();
        public int AllowedEnemyCount => allowedEnemyTypes.Count;

        public EStatusManager Status { get; private set; }
        private Coroutine spawnLoopRoutine;

        public void Startup()
        {
            if (Status == EStatusManager.Started) return;

            Status = EStatusManager.Initializing;
            if (autoSpawnEnabled)
                spawnLoopRoutine = StartCoroutine(SpawnLoop());
            Status = EStatusManager.Started;
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;

            if (spawnLoopRoutine != null)
            {
                StopCoroutine(spawnLoopRoutine);
                spawnLoopRoutine = null;
            }
            if (waveRoutine != null)
            {
                StopCoroutine(waveRoutine);
                waveRoutine = null;
            }
        }

        /// <summary>
        /// Запускает волны с задержкой между спавнами внутри волны.
        /// </summary>
        public void SpawnWave()
        {
            if (waveRoutine != null)
                StopCoroutine(waveRoutine);

            waveRoutine = StartCoroutine(SpawnWaveRoutine());
        }

        private IEnumerator SpawnWaveRoutine()
        {
            if (allowedEnemyTypes.Count == 0)
            {
                Debug.LogError("EnemyManager: Нет допустимых типов врагов для спавна.");
                yield break;
            }

            // Готовим список индексов точек спавна
            var availableIndices = new List<int>(enemyCells.Length);
            for (int i = 0; i < enemyCells.Length; i++)
                availableIndices.Add(i);

            int spawnCount = Mathf.Min(enemiesPerWave, enemyCells.Length);

            for (int i = 0; i < spawnCount; i++)
            {
                // выбираем случайную точку
                int randomListIndex = UnityEngine.Random.Range(0, availableIndices.Count);
                int cellIndex = availableIndices[randomListIndex];
                availableIndices.RemoveAt(randomListIndex);

                // выбираем случайный тип врага
                var type = allowedEnemyTypes[
                    UnityEngine.Random.Range(0, allowedEnemyTypes.Count)
                ];

                SpawnEnemy(type, enemyCells[cellIndex]);

                // ждем перед следующим спавном
                yield return new WaitForSeconds(spawnIntervalInWave);
            }

            waveRoutine = null;
        }

        private IEnumerator SpawnLoop()
        {
            yield return new WaitForSeconds(firstSpawnDelay);
            while (Status != EStatusManager.Shutdown)
            {
                SpawnWave();
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        public void SpawnEnemy(EnemyType type)
        {
            int cellIndex = UnityEngine.Random.Range(0, enemyCells.Length);
            SpawnEnemy(type, enemyCells[cellIndex]);
        }

        public void SpawnEnemy(EnemyType type, GameObject spawnPoint)
        {
            var prefab = GetEnemyPrefab(type);
            if (prefab == null || spawnPoint == null)
            {
                Debug.LogError($"EnemyManager: Невозможно заспавнить {type}");
                return;
            }

            var instance = Instantiate(prefab,
                                       spawnPoint.transform.position,
                                       Quaternion.identity);
            OnEnemySpawned?.Invoke(type, instance);
        }

        public GameObject GetEnemyPrefab(EnemyType type)
        {
            var cfg = enemyConfigs.Find(c => c.enemyType == type);
            return cfg?.prefab;
        }

        public void AddAllowedEnemyType(EnemyType type)
        {
            if (!allowedEnemyTypes.Contains(type))
                allowedEnemyTypes.Add(type);
        }

        public void IncreaseEnemiesPerWave(int inc = 1)
        {
            enemiesPerWave = Mathf.Min(enemiesPerWave + inc,
                                       enemyCells.Length);
        }
    }
}