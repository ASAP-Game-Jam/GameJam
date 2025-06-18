using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Activate
{
    public class GameObjectSpawner : MonoBehaviour, ISpawner
    {
        [Header("Настройки спауна")]
        [SerializeField] private GameObject bearPrefab; 
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float _cooldown = 10f;
        [SerializeField] private float _firstSpawnCoolDown = 10f;
        public float CoolDown { get => _cooldown; set => _cooldown = value; }

        public event Action OnSpawned;

        private void Start()
        {
            if (bearPrefab == null)
            {
                Debug.LogError("Префаб bearPrefab не назначен в инспекторе.", this);
                return;
            }

            // Запуск корутины спауна
            StartCoroutine(SpawnBearCoroutine());
        }

        private IEnumerator SpawnBearCoroutine()
        {
            yield return new WaitForSeconds(_firstSpawnCoolDown);
            // Бесконечный цикл спауна
            while (true)
            {
                SpawnBear();
                OnSpawned?.Invoke();
                yield return new WaitForSeconds(CoolDown);
            }
        }

        private void SpawnBear()
        {
            // Определяем позицию спауна: используем spawnPoint, если он задан, иначе позицию Barrack
            Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
            Instantiate(bearPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
