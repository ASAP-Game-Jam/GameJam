using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.Interfaces.Base
{
    public interface IBase : IDestroyObject
    {
        BaseType BaseType { get; }
    }
}
