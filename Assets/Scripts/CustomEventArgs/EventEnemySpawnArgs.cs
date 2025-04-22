using System;
using UnityEngine;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventEnemySpawnArgs : EventArgs
    {
        public GameObject GameObject { get; private set; }
        public EnemyType EnemyType { get; private set; }
        public EventEnemySpawnArgs(GameObject gameObject, EnemyType towerType)
        {
            GameObject = gameObject;
            EnemyType = towerType;
        }
    }
}
