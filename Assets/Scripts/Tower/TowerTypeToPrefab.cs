using UnityEngine;

namespace Assets.Scripts.Tower
{
    [System.Serializable]
    public struct TowerTypeToPrefab
    {
        [SerializeField]
        public TowerType type;
        [SerializeField]
        public GameObject prefab;
    }
}
