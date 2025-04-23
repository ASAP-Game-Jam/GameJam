using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.Interfaces
{
    public interface IDestroyObject
    {
        BaseType BaseType { get; }
        uint HP { get; }
        event EventHandler OnTakeDamage;
        event EventHandler OnDestroy;
        void TakeDamage(uint damage);
        void Activate();
        void Deactivate() { TakeDamage(HP); }
    }
}
