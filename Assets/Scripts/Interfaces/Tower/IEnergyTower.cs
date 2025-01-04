using System;

namespace Assets.Scripts.Interfaces.Tower
{
    public interface IEnergyTower
    {
        event EventHandler OnActivated;
        float Cooldown { get; }
        uint EnergyCount { get; }
    }
}
