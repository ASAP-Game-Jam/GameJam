using UnityEngine;

namespace Assets.Scripts.Interfaces.Tower
{
    public interface ITowerFabric
    {
        GameObject GetPrefab(TowerType type);
    }
}
