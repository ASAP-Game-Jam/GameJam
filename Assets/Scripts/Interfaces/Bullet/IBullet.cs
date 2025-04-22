using Assets.Scripts.Other;
using System;

namespace Assets.Scripts.Interfaces
{
    public interface IBullet
    {
        BaseType BaseType { get; set; }
        event EventHandler OnHit;
        Direction Direction { get; set; }
        uint Damage { get; set; }
        float Speed { get; set; }
    }
    public enum Direction { None, Left, Right };
}
