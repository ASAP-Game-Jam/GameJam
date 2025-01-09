using System;

namespace Assets.Scripts.Interfaces
{
    public interface ISpawnManager
    {
        event EventHandler OnSpawn;
        float SpawnTime { get; }
        float CoolDown { get; set; }
    }
}
