using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [System.Serializable]
    public struct EnemyTypeToPrefab
    {
        [SerializeField]
        public EnemyType type;
        [SerializeField]
        public GameObject prefab;
    }
}
