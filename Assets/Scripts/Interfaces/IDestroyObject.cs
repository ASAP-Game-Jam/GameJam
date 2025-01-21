using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.Interfaces
{
    public interface IDestroyObject
    {
        BaseType BaseType { get; }
        event EventHandler OnTakeDamage;
        event EventHandler OnDestroy;
        void TakeDamage(uint damage);
        uint HP { get; }
    }
}
