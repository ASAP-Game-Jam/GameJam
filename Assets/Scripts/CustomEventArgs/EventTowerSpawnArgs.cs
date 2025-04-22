using System;
using UnityEngine;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventTowerSpawnArgs : EventArgs
    {
        public GameObject GameObject {  get; private set; }
        public TowerType TowerType { get; private set; }
        public EventTowerSpawnArgs(GameObject gameObject, TowerType towerType) {
            GameObject = gameObject;
            TowerType = towerType;
        }
    }
}
