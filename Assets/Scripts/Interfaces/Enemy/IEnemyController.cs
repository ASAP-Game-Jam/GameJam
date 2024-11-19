using System;

namespace Assets.Scripts.Interfaces.Enemy
{
    public interface IEnemyController
    {
        event EventHandler OnMoving;
        void StartMove();
        void StopMove();
    }
}
