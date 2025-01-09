using System;

namespace Assets.Scripts.Interfaces
{
    public interface IBullet
    {
        event EventHandler OnHit;
        Direction Direction { get; set; }
        uint Damage { get; set; }
        float Speed { get; set; }
    }
    public enum Direction { Left, Right };
}
