using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IEnemyFabric
    {
        GameObject GetPrefab(EnemyType type);
    }
}
