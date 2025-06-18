using System;

namespace Assets.Scripts.GameObjects.Activate
{
    public interface ISpawner
    {
        event Action OnSpawned;
        public float CoolDown { get; set; }
    }
}
