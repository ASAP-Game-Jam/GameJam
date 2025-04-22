using Assets.Scripts.CustomEventArgs;
using System;

namespace Assets.Scripts.Interfaces.Enemy
{
    public interface IAttack
    {
        event EventHandler OnAttack;
        event EventHandler OnReload;
        event EventHandler OnViewEnemyObject;
    }
}
