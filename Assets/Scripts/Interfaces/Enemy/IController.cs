using System;

namespace Assets.Scripts.Interfaces.Enemy
{
    public interface IController
    {
        event EventHandler OnMoving;
        Direction Direction { get; }
        bool IsMove { get; }
        void StartMove();
        void StopMove();
    }
}
