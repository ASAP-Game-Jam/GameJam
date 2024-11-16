using System;

namespace Assets.Scripts.Interfaces.Tower
{
    public interface ITowerEnergy
    {
        event EventHandler OnActive;
        event EventHandler OnInActive;
    }
}
