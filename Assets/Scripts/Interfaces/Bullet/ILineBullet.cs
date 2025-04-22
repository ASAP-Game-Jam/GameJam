using System.Collections.Generic;

namespace Assets.Scripts.Interfaces.Bullet
{
    public interface ILineBullet : IBullet
    {
        Queue<UnityEngine.Vector2> Points { get; set; }
    }
}
