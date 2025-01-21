using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventBaseArgs : EventArgs
    {
        public uint Damage { get; private set; }
        public uint HP { get; private set; }
        public uint MaxHP { get; private set; }
        public BaseType BaseType { get; private set; }

        public EventBaseArgs(uint damage, uint maxHp, uint hp, BaseType baseType)
        {
            MaxHP = maxHp;
            Damage = damage;
            HP = hp;
            BaseType = baseType;
        }
    }
}
