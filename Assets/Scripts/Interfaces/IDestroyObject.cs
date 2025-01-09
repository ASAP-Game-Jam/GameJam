using System;

namespace Assets.Scripts.Interfaces
{
    public interface IDestroyObject
    {
        event EventHandler OnTakeDamage;
        event EventHandler OnDestroy;
        void TakeDamage(uint damage);
    }
}
