using Assets.Scripts.CustomEventArgs;
using System;

namespace Assets.Scripts.Interfaces.Enemy
{
    public interface IEnemyAttack
    {
        event EventHandler OnAttack;
        event EventHandler OnReload;
        event EventHandler OnViewEnemy;
    }
}
