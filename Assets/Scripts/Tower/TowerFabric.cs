using Assets.Scripts.Interfaces.Tower;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerFabric : MonoBehaviour, ITowerFabric
    {
        public List<TowerTypeToPrefab> objectList = new List<TowerTypeToPrefab>();

        public GameObject GetPrefab(TowerType type)
        {
            foreach (var obj in objectList)
                if (obj.type == type)
                    return obj.prefab;

            return null;
        }
    }
}
