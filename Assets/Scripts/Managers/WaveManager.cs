using Assets.Scripts.GameObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class WaveManager : MonoBehaviour, IManager
    {
        [Header("Enemy Type Priority List")]
        public List<EnemyType> enemyTypePriorityList = new List<EnemyType>();

        public EStatusManager Status { get; private set; }

        private bool isSubscribed;

        public void Startup()
        {
            if (Status == EStatusManager.Started)
                return;

            Status = EStatusManager.Initializing;

            if (LevelManager.TimerManager == null)
            {
                Debug.LogError("WaveManager: TimerManager не доступен через LevelManager!");
                Status = EStatusManager.Shutdown;
                return;
            }
            if (LevelManager.EnemyManager == null)
            {
                Debug.LogError("WaveManager: EnemyManager не доступен через LevelManager!");
                Status = EStatusManager.Shutdown;
                return;
            }

            // Подписываемся на событие, которое срабатывает каждую минуту
            LevelManager.TimerManager.OnMinutePassed += OnMinutePassed;
            LevelManager.TimerManager.OnTimeExpired += OnTimeExpired;

            AddNewEnemy();

            isSubscribed = true;

            Status = EStatusManager.Started;
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
            if (isSubscribed)
            {
                LevelManager.TimerManager.OnMinutePassed -= OnMinutePassed;
                LevelManager.TimerManager.OnTimeExpired -= OnTimeExpired;
                isSubscribed = false;
            }
        }

        /// <summary>
        /// Каждый раз по истечении минуты WaveManager:
        /// - Добавляет новый тип врага (если он есть в списке приоритетов) в EnemyManager.
        /// - Увеличивает количество противников за раз.
        /// </summary>
        private void OnMinutePassed()
        {
            Debug.Log("WaveManager: Прошла минута.");

            AddNewEnemy();
            IncreaseEnemiesPerWave();

        }

        private void AddNewEnemy()
        {
            // Добавляем новый тип врага, если число уже разрешённых меньше, чем доступно в приоритетном списке
            if (LevelManager.EnemyManager.AllowedEnemyCount < enemyTypePriorityList.Count)
            {
                EnemyType newType = enemyTypePriorityList[LevelManager.EnemyManager.AllowedEnemyCount];
                LevelManager.EnemyManager.AddAllowedEnemyType(newType);
            }
        }

        private void IncreaseEnemiesPerWave()
        {
            // Увеличиваем количество противников за раз
            LevelManager.EnemyManager.IncreaseEnemiesPerWave(1);
        }

        private void OnTimeExpired()
        {
            Debug.Log("WaveManager: Время вышло!");
            // Дополнительные действия по окончании игрового уровня или финальной волне можно реализовать здесь.
            LevelManager.EnemyManager.spawnCooldown = 5;
            LevelManager.EnemyManager.IncreaseEnemiesPerWave(5);
            foreach (var type in enemyTypePriorityList)
            {
                LevelManager.EnemyManager.AddAllowedEnemyType(type);
            }
        }
    }
}