using Assets.Scripts.GameObjects;
using Assets.Scripts.GameObjects.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Managers
{
    public class EnemyManager : MonoBehaviour, IManager
    {
        public event Action<EnemyType,GameObject> OnSpawned;
        [Header("Entities")]
        public GameObject[] EnemyCells;
        public List<TypeObject<EnemyType, GameObject>> EnemyObjects = new List<TypeObject<EnemyType, GameObject>>();
        [Header("Property")]
        public float Cooldown = 30f;
        public float FirstEntitySpawnCooldown = 40f;
        public EStatusManager Status { get; private set; }
        private List<GameObject> _enemies = new List<GameObject>();
        private float _currentCooldown;
        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
        }
        public void Startup()
        {
            Status = EStatusManager.Initializing;
            _currentCooldown = FirstEntitySpawnCooldown;
            if (EnemyCells?.Length > 0 && EnemyObjects?.Count > 0)
            {
                StartCoroutine(Spawner());
                Status = EStatusManager.Started;
            }
            else if (EnemyObjects?.Count > 0)
                Debug.LogError("EnemyCells is empty");
            else if (EnemyCells?.Length > 0)
                Debug.LogError("EnemyObjects is empty");

            if (Status == EStatusManager.Initializing)
                Status = EStatusManager.Shutdown;
        }
        private IEnumerator Spawner()
        {
            yield return new WaitForSeconds(FirstEntitySpawnCooldown);
            while (Status != EStatusManager.Shutdown)
            {
                var values = Enum.GetValues(typeof(EnemyType));
                if (values.Length > 1)
                {
                    EnemyType enemyType = (EnemyType)values.GetValue(Random.Next(1, values.Length));
                    var cell = EnemyCells[Random.Next(0, EnemyCells.Length)];

                    var pair = EnemyObjects.Find((t) => t.Key == enemyType);
                    GameObject gObject = pair.Key != EnemyType.None ? pair.Value : null;
                    if (gObject == null)
                    {
                        Debug.LogError("EnemyManager: _enemyEntity dont find");
                    }
                    else
                    {
                        GameObject obj = Instantiate(gObject, cell.transform.position, Quaternion.identity);
                        if (obj != null)
                        {
                            OnSpawned?.Invoke(enemyType,obj);
                            obj.transform.parent = null;
                            _enemies.Add(obj);
                            if (obj.GetComponent<IEntity>() is IEntity entity)
                                entity.OnDestroyed += (e) => _enemies.Remove((e as MonoBehaviour)?.gameObject);
                            yield return new WaitForSeconds(Cooldown);
                        }
                    }
                }
                yield return null;
            }
        }
    }
}
