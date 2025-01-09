using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventBaseArgs : EventArgs
    {
        public uint Damage { get; private set; }
        public uint HP { get; private set; }
        public BaseType BaseType { get; private set; }

        public EventBaseArgs(uint damage, uint hp, BaseType baseType)
        {
            Damage = damage;
            HP = hp;
            BaseType = baseType;
        }
    }
}
