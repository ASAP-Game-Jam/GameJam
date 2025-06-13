using Assets.Scripts.GameObjects.Entities;
using System;

namespace Assets.Scripts.GameObjects
{
    public interface IController
    {
        void Startup();
        void Shutdown();
    }
    public interface IMoveController : IController { 
        void ReverseDirection();
    }
    public interface IAttackController : IController {
        public event Action OnAttacking;
        public event Action<bool> OnViewEnemy;
    }
    public interface ITarget {
        bool CheckEntity(IBasicEntity entity);
    }
}
