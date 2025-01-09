using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.Interfaces.Base
{
    public interface IBase
    {
        event EventHandler OnTakeDamage;
        event EventHandler OnBaseDestroyed;
        BaseType BaseType { get; }
        void TakeDamage(uint damage);
    }
}
