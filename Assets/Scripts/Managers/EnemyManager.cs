using Assets.Scripts.GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    // Конфигурация врага с весом появления
    [System.Serializable]
    public class EnemySpawnConfig
    {
        public EnemyType enemyType;
        public GameObject prefab;
        [Tooltip("Вес (вероятность) появления данного врага. Чем больше значение, тем выше шанс появления.")]
        public float spawnChance = 1f;
    }

    public class EnemyManager : MonoBehaviour, IManager
    {
        public event System.Action<EnemyType, GameObject> OnEnemySpawned;

        [Header("Spawn Points")]
        public GameObject[] enemyCells;

        [Header("Enemy Configurations")]
        public List<EnemySpawnConfig> enemyConfigs = new List<EnemySpawnConfig>();

        [Header("Regular Spawn Settings")]
        [Tooltip("Задержка перед первым спавном врага (регулярный цикл)")]
        public float firstRegularSpawnDelay = 40f;
        [Tooltip("Интервал между спавнами врагов в регулярном цикле (в секундах)")]
        public float regularSpawnInterval = 8f;

        [Header("Wave Spawn Settings")]
        [Tooltip("Задержка перед первым спавном волны (волновой цикл)")]
        public float firstWaveSpawnDelay = 40f;
        [Tooltip("Период появления волны (в секундах)")]
        public float spawnCooldown = 30f;
        [Tooltip("Количество врагов в волне")]
        public int enemiesPerWave = 1;
        [Tooltip("Интервал между спавнами врагов внутри волны (в секундах)")]
        public float waveSpawnIntervalInside = 0.8f;

        [Header("Spawn Point Lock Settings")]
        [Tooltip("Время блокировки точки спавна после использования (в секундах)")]
        public float spawnLockDuration = 1f;

        public bool autoSpawnEnabled = true;

        // Внутренние корутины для каждого цикла
        private Coroutine regularSpawnRoutine;
        private Coroutine waveSpawnRoutine;
        // Для волнового спавна – временная корутина, которая спавнит последовательность врагов
        private Coroutine waveRoutine;

        // Список разрешённых типов врагов (заполняется извне, например, через WaveManager)
        private List<EnemyType> allowedEnemyTypes = new List<EnemyType>();
        public int AllowedEnemyCount => allowedEnemyTypes.Count;

        public EStatusManager Status { get; private set; }

        // Хранение времени, когда каждая точка будет доступна для следующего спавна
        private Dictionary<int, float> spawnPointAvailability = new Dictionary<int, float>();

        private void Awake()
        {
            // Инициализация доступности точек спавна:
            for (int i = 0; i < enemyCells.Length; i++)
            {
                spawnPointAvailability[i] = 0f;
            }
        }

        public void Startup()
        {
            if (Status == EStatusManager.Started)
                return;

            Status = EStatusManager.Initializing;

            if (autoSpawnEnabled)
            {
                regularSpawnRoutine = StartCoroutine(RegularSpawnLoop());
                waveSpawnRoutine = StartCoroutine(WaveSpawnLoop());
            }

            Status = EStatusManager.Started;
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
            if (regularSpawnRoutine != null)
            {
                StopCoroutine(regularSpawnRoutine);
                regularSpawnRoutine = null;
            }
            if (waveSpawnRoutine != null)
            {
                StopCoroutine(waveSpawnRoutine);
                waveSpawnRoutine = null;
            }
            if (waveRoutine != null)
            {
                StopCoroutine(waveRoutine);
                waveRoutine = null;
            }
        }

        // Регулярный цикл: спавнит одного врага раз в regularSpawnInterval секунд,
        // начиная через задержку firstRegularSpawnDelay.
        private IEnumerator RegularSpawnLoop()
        {
            yield return new WaitForSeconds(firstRegularSpawnDelay);
            while (Status != EStatusManager.Shutdown)
            {
                SpawnOneEnemy();
                yield return new WaitForSeconds(regularSpawnInterval);
            }
        }

        // Волновой цикл: каждые spawnCooldown секунд запускается волна спавна,
        // стартуя через firstWaveSpawnDelay.
        private IEnumerator WaveSpawnLoop()
        {
            yield return new WaitForSeconds(firstWaveSpawnDelay);
            while (Status != EStatusManager.Shutdown)
            {
                SpawnWave();
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        // Спавнит волну врагов с интервалом waveSpawnIntervalInside между спавнами
        public void SpawnWave()
        {
            if (waveRoutine != null)
                StopCoroutine(waveRoutine);
            waveRoutine = StartCoroutine(WaveSpawnRoutineInternal());
        }

        private IEnumerator WaveSpawnRoutineInternal()
        {
            if (allowedEnemyTypes.Count == 0)
            {
                Debug.LogError("EnemyManager: Нет разрешённых типов врагов для спавна.");
                yield break;
            }

            // Составляем список доступных точек спавна (точка доступна, если Time.time >= spawnPointAvailability[i])
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < enemyCells.Length; i++)
            {
                if (Time.time >= spawnPointAvailability[i])
                    availableIndices.Add(i);
            }

            int spawnCount = Mathf.Min(enemiesPerWave, availableIndices.Count);
            if (spawnCount == 0)
            {
                Debug.LogWarning("EnemyManager (Wave): Нет доступных точек спавна.");
                yield break;
            }

            for (int i = 0; i < spawnCount; i++)
            {
                int randomListIndex = UnityEngine.Random.Range(0, availableIndices.Count);
                int cellIndex = availableIndices[randomListIndex];
                availableIndices.RemoveAt(randomListIndex);

                EnemyType chosenType = GetRandomEnemyTypeWeighted();
                SpawnEnemy(chosenType, enemyCells[cellIndex]);

                // Блокируем выбранную точку на spawnLockDuration
                spawnPointAvailability[cellIndex] = Time.time + spawnLockDuration;

                yield return new WaitForSeconds(waveSpawnIntervalInside);
            }
            waveRoutine = null;
        }

        // Спавн одного врага (используется в регулярном цикле)
        private void SpawnOneEnemy()
        {
            if (allowedEnemyTypes.Count == 0)
            {
                Debug.LogError("EnemyManager: Нет разрешённых типов врагов для спавна.");
                return;
            }

            // Собираем список доступных точек спавна
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < enemyCells.Length; i++)
            {
                if (Time.time >= spawnPointAvailability[i])
                    availableIndices.Add(i);
            }
            if (availableIndices.Count == 0)
            {
                Debug.LogWarning("EnemyManager (Regular): Нет доступных точек спавна.");
                return;
            }

            int randomListIndex = UnityEngine.Random.Range(0, availableIndices.Count);
            int cellIndex = availableIndices[randomListIndex];

            EnemyType chosenType = GetRandomEnemyTypeWeighted();
            SpawnEnemy(chosenType, enemyCells[cellIndex]);

            // Блокируем выбранную точку
            spawnPointAvailability[cellIndex] = Time.time + spawnLockDuration;
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

        // Поиск префаба для заданного типа врага
        public GameObject GetEnemyPrefab(EnemyType type)
        {
            var cfg = enemyConfigs.Find(c => c.enemyType == type);
            return cfg?.prefab;
        }

        // Разрешает определённый тип врага для спавна (вызывается извне, например, WaveManager)
        public void AddAllowedEnemyType(EnemyType type)
        {
            if (!allowedEnemyTypes.Contains(type))
                allowedEnemyTypes.Add(type);
        }

        // Позволяет увеличить число врагов в волне (вызывается извне при достижении новых этапов игры)
        public void IncreaseEnemiesPerWave(int inc = 1)
        {
            enemiesPerWave = Mathf.Min(enemiesPerWave + inc, enemyCells.Length);
        }

        /// <summary>
        /// Выбирает тип врага с учётом веса появления (spawnChance) из списка разрешённых.
        /// </summary>
        /// <returns>Выбранный тип врага</returns>
        private EnemyType GetRandomEnemyTypeWeighted()
        {
            List<EnemySpawnConfig> validConfigs = new List<EnemySpawnConfig>();
            float totalWeight = 0f;
            foreach (var cfg in enemyConfigs)
            {
                if (allowedEnemyTypes.Contains(cfg.enemyType))
                {
                    validConfigs.Add(cfg);
                    totalWeight += cfg.spawnChance;
                }
            }
            float rand = UnityEngine.Random.Range(0, totalWeight);
            foreach (var cfg in validConfigs)
            {
                if (rand < cfg.spawnChance)
                    return cfg.enemyType;
                rand -= cfg.spawnChance;
            }
            return validConfigs[validConfigs.Count - 1].enemyType;
        }
    }
}