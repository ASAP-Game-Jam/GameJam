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

        // Количество противников, спавнимых за раз (волна)
        public int enemiesPerWave = 1;

        // Список допустимых для спавна типов врагов
        private List<EnemyType> allowedEnemyTypes = new List<EnemyType>();

        // Свойство для получения текущего количества разрешённых типов
        public int AllowedEnemyCount => allowedEnemyTypes.Count;

        public EStatusManager Status { get; private set; }

        private Coroutine spawnRoutine;

        public void Startup()
        {
            if (Status == EStatusManager.Started)
                return;

            Status = EStatusManager.Initializing;
            // Список начальных типов может быть пустым,
            // WaveManager будет добавлять типы через AddAllowedEnemyType().
            if (autoSpawnEnabled)
            {
                spawnRoutine = StartCoroutine(SpawnLoop());
            }
            Status = EStatusManager.Started;
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }

        /// <summary>
        /// Запускает спавн одной волны – спавнит число противников, равное минимальному значению между enemiesPerWave и количеством точек спавна.
        /// В рамках волны для каждой уникальной точки вызывается спавн противника.
        /// </summary>
        public void SpawnWave()
        {
            if (allowedEnemyTypes == null || allowedEnemyTypes.Count == 0)
            {
                Debug.LogError("EnemyManager: Нет допустимых типов врагов для спавна.");
                return;
            }

            // Формируем список индексов spawn точек.
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < enemyCells.Length; i++)
            {
                availableIndices.Add(i);
            }

            // Для каждого спавна выбираем случайный индекс и удаляем его из списка, чтобы избежать повторений.
            for (int i = 0; i < enemiesPerWave; i++)
            {
                int randomListIndex = UnityEngine.Random.Range(0, availableIndices.Count);
                int selectedCellIndex = availableIndices[randomListIndex];
                availableIndices.RemoveAt(randomListIndex);

                // Выбираем случайный тип из списка разрешённых
                int enemyTypeIndex = UnityEngine.Random.Range(0, allowedEnemyTypes.Count);
                EnemyType chosenType = allowedEnemyTypes[enemyTypeIndex];
                SpawnEnemy(chosenType, enemyCells[selectedCellIndex]);
            }
        }

        /// <summary>
        /// Автоматический цикл спавна волн, если включён режим автоспавна.
        /// </summary>
        private IEnumerator SpawnLoop()
        {
            yield return new WaitForSeconds(firstSpawnDelay);
            while (Status != EStatusManager.Shutdown)
            {
                SpawnWave();
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        /// <summary>
        /// Спавнит врага заданного типа, выбирая случайную точку из enemyCells.
        /// Используется метод, когда не важна конкретная spawn точка.
        /// </summary>
        public void SpawnEnemy(EnemyType type)
        {
            // По умолчанию выбираем случайную точку.
            int cellIndex = UnityEngine.Random.Range(0, enemyCells.Length);
            SpawnEnemy(type, enemyCells[cellIndex]);
        }

        /// <summary>
        /// Спавнит врага заданного типа по конкретной spawn точке.
        /// </summary>
        public void SpawnEnemy(EnemyType type, GameObject spawnPoint)
        {
            GameObject prefab = GetEnemyPrefab(type);
            if (prefab == null)
            {
                Debug.LogError("EnemyManager: Префаб для типа " + type + " не найден.");
                return;
            }
            if (spawnPoint == null)
            {
                Debug.LogError("EnemyManager: Спавн-точка не задана.");
                return;
            }
            GameObject enemyInstance = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
            OnEnemySpawned?.Invoke(type, enemyInstance);
        }

        /// <summary>
        /// Находит конфигурацию префаба для заданного типа врага.
        /// </summary>
        public GameObject GetEnemyPrefab(EnemyType type)
        {
            var config = enemyConfigs.Find(e => e.enemyType == type);
            if (config == null)
            {
                Debug.LogError("EnemyManager: Нет конфигурации для типа " + type);
                return null;
            }
            return config.prefab;
        }

        /// <summary>
        /// Добавляет новый тип врага в список допустимых для спавна.
        /// Если тип уже присутствует, ничего не делает.
        /// </summary>
        public void AddAllowedEnemyType(EnemyType type)
        {
            if (!allowedEnemyTypes.Contains(type))
            {
                allowedEnemyTypes.Add(type);
                Debug.Log("EnemyManager: Добавлен тип для спавна: " + type);
            }
        }

        /// <summary>
        /// Увеличивает количество противников, спавнимых за раз, на заданное значение (по умолчанию +1).
        /// При этом максимальное количество противников за волну ограничено числом spawn точек.
        /// </summary>
        public void IncreaseEnemiesPerWave(int increment = 1)
        {
            enemiesPerWave += increment;
            // Ограничиваем значение до количества spawn точек
            enemiesPerWave = Mathf.Min(enemiesPerWave, enemyCells.Length);
            Debug.Log("EnemyManager: Количество противников за волну увеличено до " + enemiesPerWave);
        }
    }
}