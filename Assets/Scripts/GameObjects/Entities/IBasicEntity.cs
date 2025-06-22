using System;

namespace Assets.Scripts.GameObjects.Entities
{
    public interface IBasicEntity
    {
        event Action<IBasicEntity, int> OnTakenDamage;
        event Action<IBasicEntity> OnDestroyed;
        int MaxHP { get; }
        int HP { get; }
        int Cost { get; set; }
        void TakeDamage(int damage);
    }
    public interface IEntity : IBasicEntity { }
    public interface IBase : IBasicEntity { }
}
