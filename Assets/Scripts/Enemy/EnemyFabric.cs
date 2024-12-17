using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyFabric : MonoBehaviour, IEnemyFabric
    {
        
        public List<EnemyTypeToPrefab> objectList = new List<EnemyTypeToPrefab>();

        public GameObject GetPrefab(EnemyType type)
        {
            foreach (var obj in objectList)
                if (obj.type == type)
                    return obj.prefab;

            return null;
        }
    }
}
