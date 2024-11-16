using System;

namespace Assets.Scripts.Interfaces.Enemy
{
    public interface IEnemy
    {
        event EventHandler OnGetDamage;
        event EventHandler OnDestroy;
        void TakeDamage(uint damage);
    }
}