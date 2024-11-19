using System;

namespace Assets.Scripts.Interfaces.Enemy
{
    public interface IEnemyAttack
    {
        event EventHandler OnAttack;
        void Attack();
    }
}
